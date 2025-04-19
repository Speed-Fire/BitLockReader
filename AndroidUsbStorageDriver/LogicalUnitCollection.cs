using AndroidUsbStorageDriver.Commands;
using MassStorage.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AndroidUsbStorageDriver
{
	public class LogicalUnitCollection : ILogicalUnitCollection
	{
		private readonly UsbMassStorageProtocol _protocol;
		private readonly ErrorHandler _errorHandler;

		public int Count { get; private set; }
		private readonly Dictionary<int, LogicalUnit> _logicalUnits = [];

		public LogicalUnitCollection(UsbMassStorageProtocol protocol)
		{
			_protocol = protocol;
			_errorHandler = new(protocol);
		}

		public ILogicalUnit this[int id]
		{
			get
			{
				if (id >= Count || id < 0)
					throw new ArgumentOutOfRangeException(nameof(id));

				if (_logicalUnits.TryGetValue(id, out var unit))
					return unit;

				var res = _protocol.ReadCapacityInfo(id, out var capacityInfo);
				_errorHandler.ThrowIfError(id, res);

				unit = new(_protocol, _errorHandler, id, capacityInfo);
				_logicalUnits[id] = unit;
				return unit;
			}
		}

		public bool Init()
		{
			var initRes = _protocol.Init();
			if (!initRes)
				return false;
			_protocol.HardReset();

			Count = _protocol.GetLogicalUnitCount();
			return true;
		}

		public void Dispose()
		{
			_protocol.Dispose();
		}
	}
}
