using Android.Content;
using Android.Hardware.Usb;
using Android.OS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidUsbStorageDriver.Helpers
{
	[BroadcastReceiver(Exported = true, Enabled = true)]
	[IntentFilter([UsbBroadcastHelper.UsbPermissionAction])]
	public class UsbPermissionReceiver : BroadcastReceiver
	{
		private readonly Action<Context, UsbDevice> _onPermissionReceived;

		public UsbPermissionReceiver()
		{
			
		}

		public UsbPermissionReceiver(Action<Context, UsbDevice> onPermissionReceived)
		{
			_onPermissionReceived = onPermissionReceived;
		}

		public override void OnReceive(Context? context, Intent? intent)
		{
			if (intent is null || intent.Action != UsbBroadcastHelper.UsbPermissionAction)
				return;

			UsbDevice device;

			if ((int)Build.VERSION.SdkInt >= 33)
			{
				device = (UsbDevice)intent.GetParcelableExtra(UsbManager.ExtraDevice,
					Java.Lang.Class.FromType(typeof(UsbDevice)))!;
			}
			else
			{
				device = (UsbDevice)intent.GetParcelableExtra(UsbManager.ExtraDevice)!;
			}

			var permissionGranted = intent.GetBooleanExtra(UsbManager.ExtraPermissionGranted, false);
			if (permissionGranted)
			{
				_onPermissionReceived.Invoke(context, device);
				//UsbBroadcastHelper.OpenUsbDevice(context, device);
			}
		}
	}
}
