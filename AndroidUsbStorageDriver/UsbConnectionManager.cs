using Android.Hardware.Usb;
using AndroidUsbStorageDriver.Helpers;

namespace AndroidUsbStorageDriver
{
	internal class UsbConnectionManager : IDisposable
	{
		private readonly UsbManager _manager;
		private UsbMassStorageDevice _device;

		public UsbDeviceConnection? Connection { get; set; }

		private int _currentInterfaceId = -1;
		private int _currentInEndpointId = -1;
		private int _currentOutEndpointId = -1;

		private UsbMassStorageInterface? _interface;
		private UsbMassStorageEndpoint? _bulkIn;
		private UsbMassStorageEndpoint? _bulkOut;

		public UsbMassStorageInterface? Interface => _interface;
		public UsbMassStorageEndpoint? BulkIn => _bulkIn;
		public UsbMassStorageEndpoint? BulkOut => _bulkOut;

		private IList<UsbMassStorageEndpoint>? _inEndpoints;
		private IList<UsbMassStorageEndpoint>? _outEndpoints;

		public UsbConnectionManager(UsbManager manager, UsbMassStorageDevice device)
		{
			_manager = manager;
			_device = device;
		}

		public bool IsDeviceVisible()
		{
			return _manager.DeviceList!.ContainsKey(_device.Name!);
		}

		public bool Open()
		{
			Connection?.Close();
			_interface = null;
			_currentInterfaceId = -1;
			_currentInEndpointId = -1;
			_currentOutEndpointId = -1;

			Connection = _manager.OpenDevice(_device, 5);
			if (Connection == null)
				return false;

			NextInterface();
			return true;
		}

		public bool NextInterface()
		{
			if(_interface is not null)
				Connection.ReleaseInterface(_interface);

			_currentInterfaceId++;
			if(_currentInterfaceId >= _device.Interfaces.Count)
				_currentInterfaceId = 0;

			_interface = _device.Interfaces[_currentInterfaceId];

			_inEndpoints = [.. _interface.Endpoints.Where(e => e.Type == UsbAddressing.XferBulk && e.Direction == UsbAddressing.In).OrderBy(e => e.EndpointNumber)];
			_outEndpoints = [.. _interface.Endpoints.Where(e => e.Type == UsbAddressing.XferBulk && e.Direction == UsbAddressing.Out).OrderBy(e => e.EndpointNumber)];
			_currentInEndpointId = _currentOutEndpointId = -1;
			NextEndpointPair();

			return Connection.ClaimInterface(_interface, true);
		}

		public void NextEndpointPair()
		{
			NextInEndpoint();
			NextOutEndpoint();
		}

		public void NextInEndpoint()
		{
			_currentInEndpointId++;
			if(_currentInEndpointId >= _inEndpoints.Count)
				_currentInEndpointId = 0;

			_bulkIn = _inEndpoints[_currentInEndpointId];
		}

		public void NextOutEndpoint()
		{
			_currentOutEndpointId++;
			if (_currentOutEndpointId >= _outEndpoints.Count)
				_currentOutEndpointId = 0;

			_bulkOut = _outEndpoints[_currentOutEndpointId];
		}

		public void Dispose()
		{
			if (Interface is not null)
				Connection?.ReleaseInterface(Interface);
			Connection?.Close();
			Connection = null;
		}
	}
}
