using MassStorage.UsbScsi.Commands;
using MassStorage.UsbScsi.Commands.Wrappers;
using MassStorage.UsbScsi.Enums;
using MassStorage.UsbScsi.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace MassStorage.UsbScsi
{
    public class UsbMassStorageProtocol : IDisposable
    {
        private readonly UsbMassStorageCommunicator _communicator;
		private readonly Mutex _mutex;

		private readonly CBW _command;

		public UsbMassStorageProtocol(UsbMassStorageCommunicator communicator)
		{
			_communicator = communicator;
			_mutex = new();
			_command = new();
		}

		public bool Init()
		{
			return _communicator.Open();
		}

		public int Reset()
		{
			var @interface = _communicator.Connection.ConnectionManager.Interface;
			if (@interface is null)
				throw new Exception("Not initialized.");

			var resetCommand = new MassStorageResetCommand(@interface.Id);
			return _communicator.Send(resetCommand);
		}

		public void HardReset()
		{
			Reset();

			_communicator.ClearInEndpoint(100);
		}

		public int GetLogicalUnitCount()
		{
			var @interface = _communicator.Connection.ConnectionManager.Interface;
			if (@interface is null)
				throw new Exception("Not initialized.");

			var command = new GetMaxLunCommand(@interface.Id);
			_communicator.Send(command);

			return command.LogicalUnitCount;
		}

		public CommandStatus ModeSense(int logicalUnitNumber, ModeSensePage page, int requestedLength,
			out byte[] buffer)
		{
			_command.SetModeSense(logicalUnitNumber, page, requestedLength);

			buffer = new byte[requestedLength];

			var res = Execute(_command, buffer, 0, buffer.Length, out _, false);

			return res;
		}

		public CommandStatus ReadCapacityInfo(int logicalUnitNumber, out CapacityInfo info)
		{
			_command.SetReadCapacity(logicalUnitNumber);
			var output = new byte[8];

			var res = Execute(_command, output, 0, output.Length, out _,false);

			info = new(output);
			return res;
		}

		public CommandStatus RequestSense(int logicalUnitNumber, byte[] buffer, int offset)
		{
			_command.SetRequestSense(logicalUnitNumber);

			return Execute(_command, buffer, offset, 18, out _, false);
		}

		public CommandStatus Read(int logicalUnitNumber, int logicalBlockAddress, int logicalBlockSize,
			byte[] buffer, int offset, int length, out int residue)
		{
			_command.SetRead10(logicalUnitNumber, logicalBlockAddress,
				length, logicalBlockSize);

			return Execute(_command, buffer, offset, length, out residue, false);
		}

		public CommandStatus Write(int logicalUnitNumber, int logicalBlockAddress, int logicalBlockSize,
			byte[] buffer, int offset, int length, out int residue)
		{
			_command.SetWrite10(logicalUnitNumber, logicalBlockAddress,
				length, logicalBlockSize);

			return Execute(_command, buffer, offset, length, out residue, true);
		}

		private CommandStatus Execute(CBW command, byte[] buffer, int offset, int length, out int residue,
			bool isWrite)
		{
			residue = 0;

			try
			{
				_mutex.WaitOne();

				var res = _communicator.Execute(command, buffer, offset, length, out residue,
					isWrite);

				_mutex.ReleaseMutex();

				return res;
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
			catch (Exception)
			{
				_mutex.ReleaseMutex();

				throw;
			}
		}

		private void Reconnect()
		{
			if (!_communicator.Connection.ConnectionManager.IsDeviceVisible())
			{
				_mutex.ReleaseMutex();
				throw new Exception("Device is disconnected.");
			}

			if (!_communicator.Connection.Open())
			{
				_mutex.ReleaseMutex();
				throw new Exception("Failed to reconnect."); 
			}

			HardReset();
		}

		public void Dispose()
		{
			_communicator.Dispose();
			_mutex.Dispose();
		}
	}
}
