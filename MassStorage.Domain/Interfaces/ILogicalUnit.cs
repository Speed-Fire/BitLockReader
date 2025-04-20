using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassStorage.Domain.Interfaces
{
	public interface ILogicalUnit
	{
		uint BlockSize { get; }
		ulong Capacity { get; }
		int LogicalUnitNumber { get; }

		void Read(ulong address, byte[] buffer, int offset, int length);
		void Write(ulong address, byte[] buffer, int offset, int length);
		void Reset();
	}
}
