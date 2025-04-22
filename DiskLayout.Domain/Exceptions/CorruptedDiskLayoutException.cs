using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskLayout.Domain.Exceptions
{
    public class CorruptedDiskLayoutException : Exception
    {
		public CorruptedDiskLayoutException(string layout) 
			: base($"Layout \'{layout}\' is corrupted.")
		{
			
		}
	}
}
