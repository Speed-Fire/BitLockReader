using DiskLayout.Domain;
using DiskLayout.Domain.Enums;
using MassStorage.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskLayout.Mbr
{
	public sealed class MbrPartition : Partition
	{
		public byte PartitionType { get; }

		public MbrPartition(
			ILogicalUnit disk,
			string name,
			PartitionAttributes attributes, 
			ulong firstSectorAddress,
			ulong sectorCount,
			byte type) 
			: base(disk, name, attributes, firstSectorAddress, sectorCount)
		{
			PartitionType = type;
		}
	}
}
