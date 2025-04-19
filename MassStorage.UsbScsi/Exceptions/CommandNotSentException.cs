using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassStorage.UsbScsi.Exceptions
{
    public class CommandNotSentException : Exception
    {
		public CommandNotSentException() : base("Command not sent.")
		{
			
		}
	}
}
