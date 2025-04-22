using DiskLayout.Domain;
using DiskLayout.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskLayout.Gpt
{
	public class GptPartitionTable : IDiskPartitionTable
	{
		private readonly List<GptPartition> _partitions;

		public GptHeader Header { get; }

		public string Type => "GPT";

		IReadOnlyList<Partition> IDiskPartitionTable.Partitions => _partitions;
		public IReadOnlyList<GptPartition> Partitions => _partitions;

		public GptPartitionTable(List<GptPartition> partitions, GptHeader header)
		{
			_partitions = partitions;
			Header = header;
		}
	}
}
