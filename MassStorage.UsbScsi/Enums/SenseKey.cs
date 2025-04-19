using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassStorage.UsbScsi.Enums
{
	public enum SenseKey
	{
		NoSense = 0x0,
		RecoveredError,
		NotReady,
		MediumError,
		HardwareError,
		IllegalRequest,
		UnitAttention,
		DataProtect,
		BlankCheck,
		VendorSpecific,
		CopyAborted,
		AbortedCommand,
		Equal,
		VolumeOverflow,
		Miscompare,
	}
}
