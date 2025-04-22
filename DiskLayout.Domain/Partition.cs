using DiskLayout.Domain.Enums;
using DiskLayout.Domain.Exceptions;
using DiskLayout.Domain.Interfaces;
using MassStorage.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DiskLayout.Domain
{
    public class Partition
    {
        private readonly ILogicalUnit _disk;
        private readonly ulong _firstSectorAddress;

		public string Name { get; }
		public PartitionAttributes Attributes { get; }

		public uint SectorSize => _disk.BlockSize;
		public ulong SectorCount { get; }

		public Partition(
            ILogicalUnit disk,
            string name,
            PartitionAttributes attributes,
            ulong firstSectorAddress,
            ulong sectorCount)
		{
			_disk = disk;
			Name = name;
			Attributes = attributes;
			_firstSectorAddress = firstSectorAddress;
			SectorCount = sectorCount;
		}

        public void Read(ulong lba, byte[] buffer, int offset, int length)
        {
            if (_firstSectorAddress + lba + ((ulong)length / SectorSize) >= SectorCount)
                throw new IndexOutOfRangeException("Attempt to read from outside partition boundary.");

            _disk.Read(_firstSectorAddress + lba, buffer, offset, length);
        }

        public byte[] Read(ulong lba, uint sectorCount)
        {
            var buffer = new byte[SectorSize * sectorCount];
            Read(lba, buffer, -, buffer.Length);
            return buffer;
        }

        public void Write(ulong lba, byte[] buffer, int offset, int length, bool force = false)
        {
            if (Attributes.HasFlag(PartitionAttributes.ReadOnly) && !force)
                throw new ReadonlyPartitionException();

			if (_firstSectorAddress + lba + ((ulong)length / SectorSize) >= SectorCount)
				throw new IndexOutOfRangeException("Attempt to write beyond partition boundary.");

			_disk.Write(_firstSectorAddress + lba, buffer, offset, length);
        }
    }
}
