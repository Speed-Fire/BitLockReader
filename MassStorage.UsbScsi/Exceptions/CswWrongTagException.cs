using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassStorage.UsbScsi.Exceptions
{
    public class CswWrongTagException : Exception
    {
		public CswWrongTagException() : base("Wrong CSW tag.")
		{
			
		}
	}
}
