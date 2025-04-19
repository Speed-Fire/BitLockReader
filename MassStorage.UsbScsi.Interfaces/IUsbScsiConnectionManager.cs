using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassStorage.UsbScsi.Interfaces
{
    public interface IUsbScsiConnectionManager : IDisposable
    {
		IUsbInterface? Interface { get; }

		bool IsDeviceVisible();
		bool Open();
		bool NextInterface();
		void NextEndpointPair();
		void NextInEndpoint();
		void NextOutEndpoint();
	}
}
