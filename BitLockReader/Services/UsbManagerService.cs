using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if ANDROID
using Android.Hardware.Usb;
using Android.Content;


using BitLockReader.Services;

//[assembly: Xamarin.Forms.Dependency(typeof(UsbManagerService))]


namespace BitLockReader.Services
{
    public class UsbManagerService
    {
        public Dictionary<string, UsbDevice> GetDevices()
        {
            var usbManager = (UsbManager)Android.App.Application.Context.GetSystemService(Context.UsbService)!;

            return new(usbManager.DeviceList);
        }

        public bool RequestPermission(UsbDevice device)
        {
			var usbManager = (UsbManager)Android.App.Application.Context.GetSystemService(Context.UsbService)!;

            var usbConnection = usbManager.OpenDevice(device);
            var interf = device.GetInterface(0);
            usbConnection.ClaimInterface(interf, true);

            var endpoint = interf.GetEndpoint(0);
           

            var usbRequest = new UsbRequest();
            usbRequest.Initialize(usbConnection, endpoint);

            return usbManager.HasPermission(device);
		}
    }
}

#endif