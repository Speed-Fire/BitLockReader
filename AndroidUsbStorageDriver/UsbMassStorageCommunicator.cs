using AndroidUsbStorageDriver.Commands.Wrappers;
using AndroidUsbStorageDriver.Exceptions;
using AndroidUsbStorageDriver.Helpers;
using MassStorage.UsbScsi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AndroidUsbStorageDriver
{

	public class UsbMassStorageCommunicator : IDisposable
	{
		internal IUsbScsiConnection Connection { get; }
		private readonly CSW _commandStatus;

		public UsbMassStorageCommunicator(IUsbScsiConnection connection)
		{
			Connection = connection;
			_commandStatus = new();
		}

		public bool Open() => Connection.Open();

		public int Send(ControlCommand command, int timeout = 1000)
		{
			return Connection.ControlTransfer(command.RequestType,
				command.Request,
				command.Value,
				command.Index,
				command.Buffer,
				command.Offset,
				command.Length,
				timeout);
		}

		private int Send(byte[] buffer, int offset, int length, int timeout)
		{
			return Connection.BulkTransferOut(buffer, offset, length, timeout);
		}

		private int Send(CBW command, int timeout)
		{
			return Send(command.Buffer, 0, command.Buffer.Length, timeout);
		}

		private int Receive(byte[] data, int offset, int length, int timeout)
		{
			return Connection.BulkTransferIn(data, offset, length, timeout);
		}

		private int ReceiveCsw(int timeout)
		{
			return Receive(_commandStatus.Buffer, 0, _commandStatus.Buffer.Length, timeout);
		}

		public CommandStatus Execute(CBW command,
			byte[] buffer, int offset, int length, out int residue, bool isWrite,
			int sendTimeout = 200, int readTimeout = 200)
		{
			residue = 0;

			var sendRes = Send(command, sendTimeout);
			if (sendRes < command.Buffer.Length)
				throw new CommandNotSentException();

			var transferred = 0;
			if (buffer != null && length > 0)
			{
				if(isWrite)
					transferred = Send(buffer, offset, length, sendTimeout);
				else
					transferred = Receive(buffer, offset, length, readTimeout);
			}

			var cswResult = ReceiveCsw(readTimeout);
			HandleCsw(command, cswResult);

			residue = _commandStatus.Residue;
			return _commandStatus.Status;
		}

		private void HandleCsw(CBW command, int cswResult)
		{
			if (cswResult < 0 || !_commandStatus.IsValid)
				throw new InvalidCswException();

			if (command.Tag != _commandStatus.Tag)
				throw new CswWrongTagException();
		}

		public void ClearInEndpoint(int timeout)
		{
			var tmp = new byte[256];

			Receive(tmp, 0, tmp.Length, timeout);
		}

		public void Dispose()
		{
			Connection.Dispose();
		}
	}
}
