using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassStorage.Domain.Exceptions
{
    public class PhaseErrorException : Exception
    {
		public PhaseErrorException() : base("Communication phase error occured.")
		{
			
		}
	}
}
