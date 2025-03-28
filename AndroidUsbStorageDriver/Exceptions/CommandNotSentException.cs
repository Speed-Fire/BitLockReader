using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidUsbStorageDriver.Exceptions
{
    public class CommandNotSentException : Exception
    {
		public CommandNotSentException() : base("Command not sent.")
		{
			
		}
	}
}
