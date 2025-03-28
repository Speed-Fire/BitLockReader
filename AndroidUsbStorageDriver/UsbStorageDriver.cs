using Android.Hardware.Usb;

namespace AndroidUsbStorageDriver
{
	public class UsbStorageDriver
	{
		private readonly UsbManager _usbManager;
		private readonly UsbDevice _device;

		public UsbStorageDriver(UsbManager usbManager, UsbDevice device)
		{
			var conf = device.GetConfiguration(0);
			var interf = conf.GetInterface(0);
			var endpoint = interf.GetEndpoint(0);

			Setup(device);

			_usbManager = usbManager;
			_device = device;
		}

		private static void Setup(UsbDevice device)
		{
			for(int i = 0; i < device.ConfigurationCount; i++)
			{
				var config = device.GetConfiguration(i);

				for(int j = 0; j < config.InterfaceCount; j++)
				{
					var iface = config.GetInterface(j);

					if (iface.InterfaceClass != UsbClass.MassStorage)
						continue;

					UsbEndpoint? bulkIn = null;
					UsbEndpoint? bulkOut = null;
					UsbEndpoint? control = null;

					for(int k = 0; k < iface.EndpointCount; k++)
					{
						var endpoint = iface.GetEndpoint(k)!;

						switch (endpoint.Type)
						{
							case UsbAddressing.XferControl:
								control = endpoint; 
								break;

							case UsbAddressing.XferBulk:
								if (endpoint.Direction == UsbAddressing.In)
									bulkIn = endpoint;
								else if(endpoint.Direction == UsbAddressing.Out)
									bulkOut = endpoint;
								break;
						}
					}
				}
			}
		}

		public void WriteSector(uint sectorNumber, Span<byte> data)
		{

		}

		public void ReadSector(uint sectorNumber, Span<byte> buffer)
		{

		}
	}
}