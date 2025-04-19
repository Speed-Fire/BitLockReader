using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassStorage.UsbScsi.Interfaces
{
    public interface IUsbInterface
    {
		int Id { get; }
		string? Name { get; }

		int InterfaceClass { get; }
		int InterfaceSubclass { get; }

		int InterfaceProtocol { get; }
		int AlternateSetting { get; }
	}
}
