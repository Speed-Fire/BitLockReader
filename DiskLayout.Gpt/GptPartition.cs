using DiskLayout.Domain;
using DiskLayout.Domain.Enums;
using MassStorage.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskLayout.Gpt
{
	public sealed class GptPartition : Partition
	{
		public Guid PartitionTypeGuid { get; }
		public Guid UniquePartitionGuid { get; }

		public GptPartition(
			ILogicalUnit disk,
			string name,
			PartitionAttributes attributes,
			ulong firstSectorAddress,
			ulong lastSectorAddress,
			Guid partitionTypeGuid,
			Guid uniquePartitionGuid)
			: base(disk, name, attributes, firstSectorAddress,
				lastSectorAddress - firstSectorAddress + 1)
		{
			PartitionTypeGuid = partitionTypeGuid;
			UniquePartitionGuid = uniquePartitionGuid;
		}
	}
}
