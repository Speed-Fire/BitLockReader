using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassStorage.UsbScsi.Exceptions
{
    public class InvalidCswException : Exception
    {
		public InvalidCswException() : base("Cannot read CSW or CSW is invalid.")
		{
			
		}
	}
}
