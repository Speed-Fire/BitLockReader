using Android.Content;
using Android.Hardware.Usb;
using Android.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassStorage.UsbScsi.Android.Helpers
{
    public class UsbBroadcastHelper
    {
		public const string UsbPermissionAction = "com.example.USB_PERMISSION";

		public static bool RequestUsbPermission(Context context, UsbDevice device)
		{
			UsbManager usbManager = (UsbManager)context.GetSystemService(Context.UsbService)!;

			if (!usbManager.HasPermission(device))
			{
				var permissionIntent = PendingIntent.GetBroadcast(
					context, 0, new Intent(UsbPermissionAction), PendingIntentFlags.Immutable)!;

				usbManager.RequestPermission(device, permissionIntent);
				Log.Info("USB", "Запрос разрешения на USB...");

				return false;
			}

			return true;
		}

		public static void OpenUsbDevice(Context context, UsbDevice device)
		{
			var usbManager = (UsbManager)context.GetSystemService(Context.UsbService)!;
			var connection = usbManager.OpenDevice(device);

			if (connection != null)
			{
				Log.Info("USB", $"Устройство {device.DeviceName} успешно подключено!");
				// Теперь можно работать с устройством (чтение/запись и т. д.)
			}
			else
			{
				Log.Error("USB", $"Не удалось открыть устройство {device.DeviceName}.");
			}
		}
	}
}
