using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidUsbStorageDriver.Exceptions
{
    public class CswWrongTagException : Exception
    {
		public CswWrongTagException() : base("Wrong CSW tag.")
		{
			
		}
	}
}
