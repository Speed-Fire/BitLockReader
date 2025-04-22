using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskLayout.Domain.Enums
{
    [Flags]
    public enum PartitionAttributes
    {
        None = 0,
		ReadOnly = 1,
		Hidden = 2,
		NoAutomount = 4,
		Bootable = 8,
		Encrypted = 16,
		System = 32,
		Recovery = 64,
		RAID = 128,
		Offline = 512,
		Snapshot = 1024,
		Metadata = 2048,
	}
}
