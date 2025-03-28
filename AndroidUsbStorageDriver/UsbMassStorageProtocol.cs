using AndroidUsbStorageDriver.Commands;
using AndroidUsbStorageDriver.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace AndroidUsbStorageDriver
{
    public class UsbMassStorageProtocol
    {
        private readonly UsbMassStorageCommunicator _communicator;

		private readonly byte[] _commandResultBuffer = new byte[256];

		private CapacityInfo _capacityInfo;

		public UsbMassStorageProtocol(UsbMassStorageCommunicator communicator)
		{
			_communicator = communicator;
		}

		public bool Init()
		{
			if (!_communicator.Open())
				return false;

			_capacityInfo = ReadCapacityInfo();

			return true;
		}

		public int Reset()
		{
			var @interface = _communicator.ConnectionManager.Interface;
			if (@interface is null)
				throw new Exception("Not initialized.");

			var resetCommand = new MassStorageResetCommand(@interface.Id);
			return _communicator.Send(resetCommand);
		}

		private CapacityInfo ReadCapacityInfo()
		{
			var command = new ReadCapacity10Command();
			var output = new byte[8];

			var res = Execute(command, output, 0, output.Length);

			return new(output);
		}

		public CommandStatus Execute(CBW command, byte[] buffer, int offset, int length)
		{
			try
			{
				return _communicator.Execute(command, buffer, offset, length);
			}
			catch (CommandNotSentException)
			{
				Reconnect();
				return CommandStatus.Failed;
			}
			catch (InvalidCswException)
			{
				Reconnect();
				return CommandStatus.Failed;
			}
		}

		private void Reconnect()
		{
			if (!_communicator.ConnectionManager.IsDeviceVisible())
				throw new Exception("Device is disconnected.");

			if (!_communicator.ConnectionManager.Open())
				throw new Exception("Failed to reconnect.");
		}
	}
}
