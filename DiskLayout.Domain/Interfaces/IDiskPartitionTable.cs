using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskLayout.Domain.Interfaces
{
    public interface IDiskPartitionTable
    {
        string Type { get; }
        IReadOnlyList<Partition> Partitions { get; }
    }
}
