using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassStorage.Domain.Exceptions
{
    public class CorruptedStorageException : MassStorageException
    {
		public CorruptedStorageException(byte asc, byte ascq) 
			: base("Storage is corrupted.", 0x03, asc, ascq)
		{
			
		}
	}
}
