using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskLayout.Domain.Exceptions
{
    public class UnknownDiskLayoutException : Exception
    {
		public UnknownDiskLayoutException() : base("Disk layout is unknown.")
		{
			
		}
	}
}
