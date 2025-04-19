using Android.Hardware.Usb;
using MassStorage.UsbScsi.Android.Helpers;
using MassStorage.UsbScsi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassStorage.UsbScsi.Android
{
	public class AndroidUsbScsiConnection : IUsbScsiConnection
	{
		private readonly AndroidUsbScsiConnectionManager _connectionManager;

		public AndroidUsbScsiConnection(UsbManager manager, UsbDevice device)
		{
			_connectionManager = new(manager, device);
		}

		private UsbDeviceConnection? Connection => _connectionManager.Connection;
		public bool IsConnected => Connection != null;

		public IUsbScsiConnectionManager ConnectionManager => _connectionManager;

		public bool Open()
		{
			return _connectionManager.Open();
		}

		public int ControlTransfer(int requestType, int request, int value, int index, byte[]? buffer, int offset, int length, int timeout)
		{
			if (Connection == null)
				throw new InvalidOperationException("Connection is closed.");

			return Connection.ControlTransfer(
				(UsbAddressing)requestType,
				request,
				value,
				index,
				buffer,
				offset,
				length,
				timeout);
		}

		public int BulkTransferIn(byte[] buffer, int offset, int length, int timeout)
		{
			if (Connection == null)
				throw new InvalidOperationException("Connection is closed.");

			return Connection
				.BulkTransfer(_connectionManager.BulkIn!, buffer, offset, length, timeout);
		}

		public int BulkTransferOut(byte[] buffer, int offset, int length, int timeout)
		{
			if (Connection == null)
				throw new InvalidOperationException("Connection is closed.");

			return Connection
				.BulkTransfer(_connectionManager.BulkOut!, buffer, offset, length, timeout);
		}

		public void Dispose()
		{
			_connectionManager.Dispose();
		}
	}
}
