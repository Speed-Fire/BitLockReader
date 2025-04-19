using MassStorage.UsbScsi.Commands.Wrappers;
using MassStorage.UsbScsi.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MassStorage.UsbScsi.Commands
{
    internal class GetMaxLunCommand : ControlCommand
    {
		public int LogicalUnitCount => Buffer![0] + 1;

		public GetMaxLunCommand(int interfaceNumber) :
			base(
				MspConsts.REQUSET_TYPE_CLASS | MspConsts.DIRECTION_IN,
				MspConsts.REQUEST_GET_MAX_LUN,
				0,
				interfaceNumber,
				new byte[1], 0, 1)
		{
			
		}
	}

	internal class MassStorageResetCommand : ControlCommand
	{
		public MassStorageResetCommand(int interfaceNumber) :
			base(
				MspConsts.REQUSET_TYPE_CLASS | MspConsts.DIRECTION_OUT,
				MspConsts.REQUEST_MASS_STORAGE_RESET,
				0,
				interfaceNumber,
				null, 0, 0)
		{
			
		}
	}

	internal class GetDeviceDescriptorCommand : ControlCommand
	{
		public GetDeviceDescriptorCommand() :
			base(
				MspConsts.REQUSET_TYPE_STANDARD |  MspConsts.DIRECTION_IN,
				MspConsts.REQUEST_GET_DESCRIPTOR,
				MspConsts.VALUE_DEVICE_DESCRIPTOR,
				0,
				new byte[64], 0, 64)
		{
			
		}
	}
}
