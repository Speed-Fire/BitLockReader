using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassStorage.Domain.Exceptions
{
    public class MassStorageException : Exception
    {
		public MassStorageException(string message, byte senseKey, byte asc, byte ascq) 
			: base($"{message + (!string.IsNullOrEmpty(message) ? "\n" : "")}Sense key: {senseKey:X2}; ASC/ASCQ: {asc:X2}/{ascq:X2}")
		{
			
		}
	}
}
