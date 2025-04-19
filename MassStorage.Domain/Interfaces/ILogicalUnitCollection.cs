using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassStorage.Domain.Interfaces
{
	public interface ILogicalUnitCollection : IDisposable
	{
		ILogicalUnit this[int id] { get; }

		int Count { get; }

		bool Init();
	}
}
