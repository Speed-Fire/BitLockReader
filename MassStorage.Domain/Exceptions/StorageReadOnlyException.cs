using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassStorage.Domain.Exceptions
{
    public class StorageReadOnlyException : MassStorageException
    {
		public StorageReadOnlyException(byte asc, byte ascq) 
			: base("Storage is read only.", 0x7, asc, ascq)
		{
			
		}
	}
}
