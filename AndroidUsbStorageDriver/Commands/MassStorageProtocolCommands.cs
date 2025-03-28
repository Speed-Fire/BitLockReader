using Android.Hardware.Usb;
using AndroidUsbStorageDriver.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidUsbStorageDriver.Commands
{
    class MassStorageProtocolCommands
    {
    }

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

	//public class ReadCapacityCommand : ControlCommand
	//{
	//	public uint BlockSize => (uint)((Buffer![4] << 24) + (Buffer[5] << 16) + (Buffer[6] << 8) + (Buffer[7] << 0));
	//	public uint BlockCount => (uint)((Buffer![0] << 24) + (Buffer[1] << 16) + (Buffer[2] << 8) + (Buffer[3] << 0)) / BlockSize + 1;

	//	public ReadCapacityCommand(int logicalUnitNumber) :
	//		base(
	//			MspConsts.REQUSET_TYPE_CLASS | MspConsts.DIRECTION_IN,
	//			MspConsts.REQUEST_READ_CAPACITY,
	//			LunHelper.GetNumber(logicalUnitNumber),
	//			0,
	//			new byte[8], 0, 8)
	//	{
			
	//	}
	//}

	public class ReadCapacity10Command : CBW
	{
		public ReadCapacity10Command()
		{
			Tag = 1;
			TransferDataLength = 8;
			CommandLength = 10;
			IsInput = true;
			this[0] = 0x25;
		}
	}

	public class RequestSenseCommand : CBW
	{
		public RequestSenseCommand(bool extended = false)
		{
			var dataLength = extended ? 32 : 18;

			Tag = 1;
			TransferDataLength = dataLength;
			IsInput = true;
			CommandLength = 6;
			this[0] = 0x03;
			this[4] = (byte)dataLength;
		}
	}

	public class ReadCapacity16Command : BulkData
	{
		public ReadCapacity16Command() : base(16)
		{
			Buffer[0] = 0x9E;
			Buffer[8] = 32;
		}
	}

	public class InjuryCommand : BulkData
	{
		public InjuryCommand() : base(12) 
		{
			Buffer[0] = 0x12;
			Buffer[4] = 0x24;
		}
	}

	internal class Read10Command : BulkData
	{
		public Read10Command(int logicalUnitNumber, int logicalBlockAddress, int blockCount) :
			base(12)
		{
			Buffer[0] = MspConsts.REQUEST_READ_10;
			Buffer[1] = LunHelper.GetNumber(logicalUnitNumber);

			Buffer[2] = (byte)((logicalBlockAddress & 0xFF000000) >> 24);
			Buffer[3] = (byte)((logicalBlockAddress & 0x00FF0000) >> 16);
			Buffer[4] = (byte)((logicalBlockAddress & 0x0000FF00) >> 8);
			Buffer[5] = (byte)((logicalBlockAddress & 0x000000FF) >> 0);

			Buffer[7] = (byte)((blockCount & 0xFF00) >> 8);
			Buffer[8] = (byte)((blockCount & 0x00FF) >> 0);
		}
	}

	internal class Write10Command : BulkData
	{
		public Write10Command(int logicalUnitNumber, int logicalBlockAddress, int blockCount) :
			base(12)
		{
			Buffer[0] = MspConsts.REQUEST_WRITE_10;
			Buffer[1] = LunHelper.GetNumber(logicalUnitNumber);

			Buffer[2] = (byte)((logicalBlockAddress & 0xFF000000) >> 24);
			Buffer[3] = (byte)((logicalBlockAddress & 0x00FF0000) >> 16);
			Buffer[4] = (byte)((logicalBlockAddress & 0x0000FF00) >> 8);
			Buffer[5] = (byte)((logicalBlockAddress & 0x000000FF) >> 0);

			Buffer[7] = (byte)((blockCount & 0xFF00) >> 8);
			Buffer[8] = (byte)((blockCount & 0x00FF) >> 0);
		}
	}
}
