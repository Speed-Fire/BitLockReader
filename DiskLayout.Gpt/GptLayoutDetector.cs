using DiskLayout.Domain.Enums;
using DiskLayout.Domain.Interfaces;
using DiskLayout.Domain;
using MassStorage.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.PortableExecutable;
using Force.Crc32;
using DiskLayout.Domain.Exceptions;
using System.Collections;

namespace DiskLayout.Gpt
{
    public partial class GptLayoutDetector : IDiskLayoutDetector
    {
		private readonly Crc32Algorithm _crc32Hasher = new(false);

		public IDiskPartitionTable? Detect(ReadOnlySpan<byte> lba0, ILogicalUnit disk)
		{
			// check and parse pseudo MBR
			if (lba0[510] != 0x55 || lba0[511] != 0xAA)
				return null;
			List<ulong> partitions = ParseMbr(lba0, disk);

			if (partitions.Count != 1)
				return null;

			// try parse standard GPT table
			if (TryParseGpt(disk, partitions[0], out var table))
				return table;

			// try parse alternate GPT table
			if (TryParseGpt(disk, disk.Capacity - 1, out table))
				return table;

			throw new CorruptedDiskLayoutException("GPT");
		}

		#region Gpt

		private bool TryParseGpt(ILogicalUnit disk, ulong headerLba, out GptPartitionTable? table)
		{
			table = null;

			// read and parse GPT header
			var gptHeaderBuffer = new byte[disk.BlockSize];
			disk.Read(headerLba, gptHeaderBuffer, 0, gptHeaderBuffer.Length);
			var gptHeader = new GptHeader(gptHeaderBuffer);
			gptHeaderBuffer[16] = gptHeaderBuffer[17] = gptHeaderBuffer[18] = gptHeaderBuffer[19] = 0;

			// read GPT table
			var partitionEntriesLength = gptHeader.NumberOfPartitionEntries * gptHeader.SizeOfPartitionEntry;
			var partitionEntriesBuffer = new byte[GetEntriesArraySectorLength(partitionEntriesLength, disk.BlockSize)];
			disk.Read(gptHeader.PartitionEntryLba, partitionEntriesBuffer, 0, (int)partitionEntriesLength);

			// check if header and table are valid
			if (gptHeader.Signature != "EFI PART" ||
				!CheckCrc32(gptHeaderBuffer, 0, 92, gptHeader.HeaderCRC32) ||
				gptHeader.MyLba != headerLba ||
				!CheckCrc32(partitionEntriesBuffer, 0, (int)partitionEntriesLength,
					gptHeader.PartitionEntryArrayCRC32))
				return false;

			// parse table
			table = ParseGptTable(disk, gptHeader,
				partitionEntriesBuffer,
				gptHeader.NumberOfPartitionEntries, 
				gptHeader.SizeOfPartitionEntry);

			return true;
		}

		private GptPartitionTable ParseGptTable(ILogicalUnit disk, GptHeader header,
			ReadOnlySpan<byte> buffer, uint entryCount, uint entrySize)
		{
			var partitions = new List<GptPartition>();

			for(int i = 0; i < entryCount; i++)
			{
				var slice = buffer.Slice(i * (int)entrySize, (int)entrySize);

				var partition = ParseGptPartition(disk, slice, i);
				if(partition != null)
					partitions.Add(partition);
			}

			return new GptPartitionTable(partitions, header);
		}

		private GptPartition? ParseGptPartition(ILogicalUnit disk, 
			ReadOnlySpan<byte> partition, int index)
		{
			var typeGuid = new Guid(partition[0..16]);

			if (typeGuid == Guid.Empty)
				return null;

			var uniqueGuid = new Guid(partition[16..32]);

			var startingLba = BitConverter.ToUInt64(partition[32..40]);
			var endingLba = BitConverter.ToUInt64(partition[40..48]);

			var attributesArray = new BitArray(partition[48..56].ToArray());
			var attributes = PartitionAttributes.None;

			if (attributesArray[0])
				attributes |= PartitionAttributes.Metadata;

			if (attributesArray[1])
				attributes |= PartitionAttributes.Hidden | PartitionAttributes.NoAutomount;

			if (attributesArray[2])
				attributes |= PartitionAttributes.Bootable;

			if (_guidAttributes.TryGetValue(typeGuid, out var guidAttributes))
				attributes |= guidAttributes;

			var nameBuffer = partition[56..128];
			var name = new string([.. nameBuffer[0..nameBuffer.IndexOf((byte)0)].ToArray().Select(x => (char)x)]);
			if (string.IsNullOrEmpty(name))
				name = $"Drive {index}";

			return new(disk, name, attributes, startingLba, endingLba,
				typeGuid, uniqueGuid);
		}

		private bool CheckCrc32(byte[] buffer, int offset, int count, uint crc32)
		{
			var computed = BitConverter.ToUInt32(_crc32Hasher.ComputeHash(buffer, offset, count));

			return computed == crc32;
		}

		private static uint GetEntriesArraySectorLength(uint entriesArrayLength, uint sectorSize)
		{
			return Convert.ToUInt32(Math.Ceiling((double)entriesArrayLength / sectorSize)) * sectorSize;
		}

		#endregion

		#region Mbr

		private static List<ulong> ParseMbr(ReadOnlySpan<byte> lba0, ILogicalUnit disk)
		{
			var partitions = new List<ulong>();
			for (int i = 446; i < 510; i += 16)
			{
				var partition = ParseMbrPartition(lba0.Slice(i, 16), disk, (i - 446) / 16 + 1);

				if (partition == null)
					continue;

				partitions.Add(partition.Value);
			}

			return partitions;
		}

		private static ulong? ParseMbrPartition(ReadOnlySpan<byte> data, ILogicalUnit disk, int index)
		{
			var bootable = data[0] == 0x80;
			var fstype = data[4];

			var firstSectorAddress = BitConverter.ToUInt32(data.Slice(8, 4));
			var sectorCount = BitConverter.ToUInt32(data.Slice(12, 4));

			var name = $"Partition {index}";
			var attributes = bootable ? PartitionAttributes.Bootable : PartitionAttributes.None;

			if (fstype != 0xEE || sectorCount == 0)
				return null;

			return firstSectorAddress;
		}

		#endregion
	}
}
