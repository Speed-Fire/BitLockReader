using DiskLayout.Domain;
using DiskLayout.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskLayout.Mbr
{
	public class MbrPartitionTable : IDiskPartitionTable
	{
		private readonly List<MbrPartition> _partitions;

		public string Type => "MBR";

		IReadOnlyList<Partition> IDiskPartitionTable.Partitions => _partitions;
		public IReadOnlyList<MbrPartition> Partitions => _partitions;

		public MbrPartitionTable(List<MbrPartition> partitions)
		{
			_partitions = partitions;
		}
	}
}
