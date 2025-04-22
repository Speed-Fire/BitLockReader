using MassStorage.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskLayout.Domain.Interfaces
{
    public interface IDiskLayoutDetector
    {
		IDiskPartitionTable? Detect(ReadOnlySpan<byte> lba0, ILogicalUnit disk);
    }
}
