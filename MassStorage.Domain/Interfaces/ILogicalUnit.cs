using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassStorage.Domain.Interfaces
{
	public interface ILogicalUnit
	{
		int BlockSize { get; }
		uint Capacity { get; }
		int LogicalUnitNumber { get; }

		void Read(int address, byte[] buffer, int offset, int length);
		void Write(int address, byte[] buffer, int offset, int length);
		void Reset();
	}
}
