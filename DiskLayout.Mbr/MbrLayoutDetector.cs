using DiskLayout.Domain;
using DiskLayout.Domain.Enums;
using DiskLayout.Domain.Interfaces;
using MassStorage.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskLayout.Mbr
{
	public class MbrLayoutDetector : IDiskLayoutDetector
	{
		public IDiskPartitionTable? Detect(ReadOnlySpan<byte> lba0, ILogicalUnit disk)
		{
			if (lba0[510] != 0x55 || lba0[511] != 0xAA)
				return null;

			var partitions = new List<MbrPartition>();
			for(int i = 446; i < 510; i += 16)
			{
				var partition = ParsePartition(lba0.Slice(i, 16), disk, (i - 446) / 16 + 1);

				if (partition.SectorCount > 0)
					partitions.Add(partition);
			}

			return new MbrPartitionTable(partitions);
		}

		private static MbrPartition ParsePartition(ReadOnlySpan<byte> data, ILogicalUnit disk, int index)
		{
			var bootable = data[0] == 0x80;
			var fstype = data[4];

			var firstSectorAddress = BitConverter.ToUInt32(data.Slice(8, 4));
			var sectorCount = BitConverter.ToUInt32(data.Slice(12, 4));

			var name = $"Drive {index}";
			var attributes = bootable ? PartitionAttributes.Bootable : PartitionAttributes.None;

			return new(disk, name, attributes, firstSectorAddress, sectorCount, fstype);
		}
	}
}
