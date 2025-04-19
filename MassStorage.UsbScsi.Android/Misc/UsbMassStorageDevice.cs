using Android.Hardware.Usb;
using Android.OS;
using MassStorage.UsbScsi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassStorage.UsbScsi.Android.Misc
{
    internal class UsbMassStorageDevice
    {
        private readonly UsbDevice _device;
		public UsbDevice UnderlyingDevice => _device;

		private readonly List<UsbMassStorageConfiguration> _configurations = [];
		public IList<UsbMassStorageConfiguration> Configurations => _configurations;

		public readonly List<UsbMassStorageInterface> _interfaces = [];
		public IList<UsbMassStorageInterface> Interfaces => _interfaces;

		public int Id => _device.DeviceId;
		public string? Name => _device.DeviceName;

		public UsbClass DeviceClass => _device.DeviceClass;
		public UsbClass DeviceSubclass => _device.DeviceSubclass;

		public int DeviceProtocol => _device.DeviceProtocol;

		public string? ManufacturerName => _device.ManufacturerName;
		public string? ProductName => _device.ProductName;

		public int ProductId => _device.ProductId;
		public int VendorId => _device.VendorId;

		public string? SerialNumber => _device.SerialNumber;
		public string Version => _device.Version;

		public UsbMassStorageDevice(UsbDevice device)
		{
			_device = device;
			Init();
		}

		private void Init()
		{
			for(int i = 0; i < _device.ConfigurationCount; i++)
			{
				var configuration = new UsbMassStorageConfiguration(
					_device.GetConfiguration(i));

				_configurations.Add(configuration);
				_interfaces.AddRange(configuration.Interfaces);
			}
		}
	}

	internal class UsbMassStorageConfiguration
	{
		private readonly UsbConfiguration _configuration;
		public UsbConfiguration UnderlyingConfiguration => _configuration;

		private readonly List<UsbMassStorageInterface> _interfaces = [];
		public IList<UsbMassStorageInterface> Interfaces => _interfaces;

		public int Id => _configuration.Id;
		public string? Name => _configuration.Name;

		public int MaxPower => _configuration.MaxPower;

		public bool IsRemoteWakeup => _configuration.IsRemoteWakeup;
		public bool IsSelfPowered => _configuration.IsSelfPowered;

		public UsbMassStorageConfiguration(UsbConfiguration configuration)
		{
			_configuration = configuration;
			Init();
		}

		private void Init()
		{
			for(int i = 0; i < _configuration.InterfaceCount; i++)
			{
				var iface = _configuration.GetInterface(i);
				if (iface.InterfaceClass != UsbClass.MassStorage)
					continue;

				var @interface = new UsbMassStorageInterface(iface);

				_interfaces.Add(@interface);
			}
		}
	}

	internal class UsbMassStorageInterface : IUsbInterface
	{
		private readonly UsbInterface _interface;
		public UsbInterface UnderlyingInterface => _interface;

		private readonly List<UsbMassStorageEndpoint> _endpoints = [];
		public IList<UsbMassStorageEndpoint> Endpoints => _endpoints;

		public UsbClass InterfaceClass => _interface.InterfaceClass;
		public UsbClass InterfaceSubclass => _interface.InterfaceSubclass;

		public int InterfaceProtocol => _interface.InterfaceProtocol;
		public int Id => _interface.Id;
		public int AlternateSetting => _interface.AlternateSetting;

		public string? Name => _interface.Name;

		int IUsbInterface.InterfaceClass => (int)_interface.InterfaceClass;
		int IUsbInterface.InterfaceSubclass => (int)_interface.InterfaceSubclass;

		public UsbMassStorageInterface(UsbInterface @interface)
		{
			_interface = @interface;
			Init();
		}

		private void Init()
		{
			for(int i = 0; i < _interface.EndpointCount; i++)
			{
				var endpoint = new UsbMassStorageEndpoint(
					_interface.GetEndpoint(i)!);

				_endpoints.Add(endpoint);
			}
		}
	}

	internal class UsbMassStorageEndpoint
	{
		private readonly UsbEndpoint _endpoint;
		public UsbEndpoint UnderlyingEndpoint => _endpoint;

		public UsbAddressing Address => _endpoint.Address;
		public UsbAddressing Type => _endpoint.Type;
		public UsbAddressing Direction => _endpoint.Direction;

		public int Interval => _endpoint.Interval;
		public int EndpointNumber => _endpoint.EndpointNumber;
		public int MaxPacketSize => _endpoint.MaxPacketSize;
		public int Attributes => _endpoint.Attributes;

		public UsbMassStorageEndpoint(UsbEndpoint endpoint)
		{
			_endpoint = endpoint;
		}
	}
}
