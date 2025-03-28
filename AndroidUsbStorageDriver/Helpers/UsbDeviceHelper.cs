using Android.Content;
using Android.Hardware.Usb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidUsbStorageDriver.Helpers
{
    internal static class UsbDeviceHelper
    {
        public static UsbDeviceConnection? OpenDevice(this UsbManager manager,
            UsbMassStorageDevice device)
        {
            return manager.OpenDevice(device.UnderlyingDevice);
        }

        public static UsbDeviceConnection? OpenDevice(this UsbManager manager,
            UsbDevice device, int triesCount)
        {
            if(triesCount < 1)
                triesCount = 1;

            for(int i = 0; i < triesCount; i++)
            {
                var connection = manager.OpenDevice(device);

                if (connection is not null)
                    return connection;

                Thread.Sleep(100);
            }

            return manager.OpenDevice(device);
		}

		public static UsbDeviceConnection? OpenDevice(this UsbManager manager,
			UsbMassStorageDevice device, int triesCount)
        {
            return OpenDevice(manager, device.UnderlyingDevice, triesCount);
        }


		public static bool HasPermission(this UsbManager manager,
            UsbMassStorageDevice device)
        {
            return manager.HasPermission(device.UnderlyingDevice);
        }

        public static void RequestPermission(this UsbManager manager,
            UsbMassStorageDevice device, PendingIntent? intent)
        {
            manager.RequestPermission(device.UnderlyingDevice, intent);
        }

        public static bool ClaimInterface(this UsbDeviceConnection connection,
            UsbMassStorageInterface @interface, bool force)
        {
            return connection.ClaimInterface(@interface.UnderlyingInterface, force);
        }

		public static bool ReleaseInterface(this UsbDeviceConnection connection,
			UsbMassStorageInterface @interface)
		{
            return connection.ReleaseInterface(@interface.UnderlyingInterface);
		}

		public static int BulkTransfer(this UsbDeviceConnection connection,
            UsbMassStorageEndpoint endpoint,
            byte[] buffer,
            int offset,
            int length,
            int timeout)
        {
            return connection.BulkTransfer(endpoint.UnderlyingEndpoint,
                buffer, offset, length, timeout);
        }

        public static Task<int> BulkTransferAsync(this UsbDeviceConnection connection,
			UsbMassStorageEndpoint endpoint,
			byte[] buffer,
			int offset,
			int length,
			int timeout)
        {
            return connection.BulkTransferAsync(endpoint.UnderlyingEndpoint,
                buffer, offset, length, timeout);
        }

	}
}
