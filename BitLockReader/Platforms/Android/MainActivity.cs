using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Hardware.Usb;
using Android.Net.Nsd;
using Android.OS;
using AndroidUsbStorageDriver.Commands;
using AndroidUsbStorageDriver;
using AndroidUsbStorageDriver.Helpers;
using Xamarin.KotlinX.Coroutines.Channels;

namespace BitLockReader;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
	private UsbPermissionReceiver? _usbPermissionReceiver;

	protected override void OnCreate(Bundle? savedInstanceState)
	{
		base.OnCreate(savedInstanceState);

		_usbPermissionReceiver = new(OnPermissionGranted);
		var filter = new IntentFilter(UsbBroadcastHelper.UsbPermissionAction);
		RegisterReceiver(_usbPermissionReceiver, filter, ReceiverFlags.Exported);

		var usbManager = (UsbManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.UsbService)!;

		var devices = usbManager.DeviceList;
		if (devices.Count > 0)
		{
			var device = devices.Values.ElementAt(0);

			if(UsbBroadcastHelper.RequestUsbPermission(this, device))
				OnPermissionGranted(this, device);
		}
	}

	private void OnPermissionGranted(Context context, UsbDevice device)
	{
		var usbManager = (UsbManager)context.GetSystemService(Context.UsbService)!;
		using var protocol = new UsbMassStorageCommunicator(usbManager, device);

		if (!protocol.Open())
			return;
		//var command = new ReadCapacityCommand(0);

		//var intRes = protocol.NextInterface();
		//protocol.NextEndpointPair();
		

		var resetRes = protocol.Reset();

		byte[] cbw = new byte[31];
		cbw[0] = 0x55; // CBW Signature
		cbw[1] = 0x53;
		cbw[2] = 0x42;
		cbw[3] = 0x43;
		cbw[4] = 1;
		cbw[8] = 8;
		cbw[12] = 0x80;
		cbw[14] = 10;
		cbw[15] = 0x25;
		//cbw[12] = 0x25; // Команда Read Capacity

		var command = new byte[12];
		command[0] = 0x25;
		////command[0] = 0x9E;
		////command[8] = 32;

		// INJURY
		//var command = new byte[12];
		//command[0] = 0x12;
		//command[4] = 0x24;

		var readcapcom = new ReadCapacityCommand();

		//var res = protocol.Send(new BulkData(cbw), 1000);
		//var res1 = protocol.Send(new BulkData(command), 1000);
		var res1 = protocol.Send(readcapcom, 1000);
		//var res1 = protocol.Send(new BulkData(cbw), 1000);

		Thread.Sleep(200);

		var data = new BulkData(21);

		var readCount = protocol.Receive(data, 1000);

		Thread.Sleep(200);

		var data2 = new BulkData(21);

		var readCount2 = protocol.Receive(data, 1000);

		var tmp = new int[56];
	}
}
