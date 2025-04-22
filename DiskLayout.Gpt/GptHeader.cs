using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskLayout.Gpt
{
    public class GptHeader
    {
        public string Signature { get; }

        public string Revision { get; }

		public uint HeaderSize { get; }

		public uint HeaderCRC32 { get; }

        public ulong MyLba { get; }

		public ulong AlternateLba { get; }

		public ulong FirstUsableLba { get; }

		public ulong LastUsableLba { get; }

		public Guid DiskGuid { get; }

		public ulong PartitionEntryLba { get; }

		public uint NumberOfPartitionEntries { get; }

		public uint SizeOfPartitionEntry { get; }

		public uint PartitionEntryArrayCRC32 { get; }

		public GptHeader(ReadOnlySpan<byte> header)
		{
			Signature = new([.. header[0..8].ToArray().Select(x => (char)x)]);

			Revision =
				BitConverter.ToUInt16(header[10..12]).ToString() + "." +
				BitConverter.ToUInt16(header[8..10]).ToString();

			HeaderSize = BitConverter.ToUInt32(header[12..16]);

			HeaderCRC32 = BitConverter.ToUInt32(header[16..20]);

			MyLba = BitConverter.ToUInt64(header[24..32]);

			AlternateLba = BitConverter.ToUInt64(header[32..40]);

			FirstUsableLba = BitConverter.ToUInt64(header[40..48]);

			LastUsableLba = BitConverter.ToUInt64(header[48..56]);

			DiskGuid = new(header[56..72]);

			PartitionEntryLba = BitConverter.ToUInt64(header[72..80]);

			NumberOfPartitionEntries = BitConverter.ToUInt32(header[80..84]);

			SizeOfPartitionEntry = BitConverter.ToUInt32(header[84..88]);

			PartitionEntryArrayCRC32 = BitConverter.ToUInt32(header[88..92]);
		}
	}
}
