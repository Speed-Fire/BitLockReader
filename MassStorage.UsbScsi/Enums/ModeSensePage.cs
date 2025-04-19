using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassStorage.UsbScsi.Enums
{
	public enum ModeSensePage
	{
		VendorSpecific = 0x00,
		ReadWriteErrorRecovery = 0x01,
		DisconnectReconnect = 0x02,
		FormatDevice = 0x03,
		RigidDiskGeometry = 0x04,
		FlexibleDisk = 0x05,
		VerifyErrorRecovery = 0x07,
		Caching = 0x08,
		ControlMode = 0x0A,
		CdDvdCapabilities = 0x0D,
		PowerConditionMode = 0x1A,
		InformationalExceptionsControl = 0x1C,
		All = 0x3F
	}
}
