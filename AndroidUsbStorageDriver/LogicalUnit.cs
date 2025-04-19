using AndroidUsbStorageDriver.Commands.Wrappers;
using MassStorage.Domain.Exceptions;
using MassStorage.Domain.Interfaces;

namespace AndroidUsbStorageDriver
{
	public class LogicalUnit : ILogicalUnit
	{
		private readonly UsbMassStorageProtocol _protocol;
		private readonly ErrorHandler _errorHandler;

		private readonly int _logicalUnitNumber;
		private readonly CapacityInfo _capacityInfo;

		public int LogicalUnitNumber => _logicalUnitNumber;
		public uint Capacity => _capacityInfo.Capacity;
		public int BlockSize => _capacityInfo.BlockSize;

		internal LogicalUnit(UsbMassStorageProtocol protocol, ErrorHandler errorHandler,
			int logicalUnitNumber, CapacityInfo capacityInfo)
		{
			_protocol = protocol;
			_errorHandler = errorHandler;
			_logicalUnitNumber = logicalUnitNumber;
			_capacityInfo = capacityInfo;
		}

		public void Reset()
		{
			_protocol.HardReset();
		}

		public void Read(int address, byte[] buffer, int offset, int length)
		{
			var res = _protocol.Read(_logicalUnitNumber, address, BlockSize,
				buffer, offset, length, out var residue);

			_errorHandler.ThrowIfError(LogicalUnitNumber, res);
		}

		public void Write(int address, byte[] buffer, int offset, int length)
		{
			if (address + length > Capacity)
				throw new InvalidOperationException("There is no room for your data.");

			var res = _protocol.Write(_logicalUnitNumber, address, BlockSize,
				buffer, offset, length, out var residue);

			_errorHandler.ThrowIfError(LogicalUnitNumber, res);
		}
	}
}
