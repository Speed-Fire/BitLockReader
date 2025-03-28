using Android.App;
using Android.Hardware.Usb;
using Android.Runtime;
using AndroidUsbStorageDriver;
using AndroidUsbStorageDriver.Commands;
using AndroidUsbStorageDriver.Helpers;

namespace BitLockReader;

[Application]
public class MainApplication : MauiApplication
{
	public MainApplication(IntPtr handle, JniHandleOwnership ownership)
		: base(handle, ownership)
	{
	}

	protected override MauiApp CreateMauiApp()
	{
		//var usbManager = (UsbManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.UsbService)!;

		//var devices = usbManager.DeviceList;
		//if (devices.Count > 0)
		//{
		//	var device = devices.Values.ElementAt(0);

		//	UsbBroadcastHelper.RequestUsbPermission(this, device);
		//}

		return MauiProgram.CreateMauiApp();
	}

	public void OnUsbPermissionGranted(UsbDevice device)
	{

	}
}
