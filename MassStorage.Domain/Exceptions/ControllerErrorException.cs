using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassStorage.Domain.Exceptions
{
    public class ControllerErrorException : MassStorageException
    {
		public ControllerErrorException(byte asc, byte ascq) 
			: base("Something wrong with storage controller.", 0x4, asc, ascq)
		{
			
		}
	}
}
