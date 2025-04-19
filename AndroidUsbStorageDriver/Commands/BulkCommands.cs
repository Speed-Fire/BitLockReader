using AndroidUsbStorageDriver.Commands.Wrappers;
using AndroidUsbStorageDriver.Enums;
using AndroidUsbStorageDriver.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidUsbStorageDriver.Commands
{
    internal static class BulkCommands
    {
		#region fields

		private static Dictionary<ModeSensePage, int> _pageLengths = new()
		{
			[ModeSensePage.VendorSpecific] = 256, // page 0x00 - Vendor-Specific
			[ModeSensePage.ReadWriteErrorRecovery] = 10,     // page 0x01
			[ModeSensePage.DisconnectReconnect] = 10,     // page 0x02
			[ModeSensePage.FormatDevice] = 24,     // page 0x03
			[ModeSensePage.RigidDiskGeometry] = 24,     // page 0x04
			[ModeSensePage.FlexibleDisk] = 32,     // page 0x05
			[ModeSensePage.VerifyErrorRecovery] = 10,     // page 0x06
			[ModeSensePage.Caching] = 20,     // page 0x07
			[ModeSensePage.ControlMode] = 10,     // page 0x08
			[ModeSensePage.CdDvdCapabilities] = 20,     // page 0x09
			[ModeSensePage.PowerConditionMode] = 12,     // page 0x0a
			[ModeSensePage.InformationalExceptionsControl] = 10,     // page 0x
			[ModeSensePage.All] = 256,  // page 0x
		};

		#endregion

		internal static void SetReadCapacity(this CBW cbw, int logicalUnitNumber)
		{
			cbw.ClearCommandData();

			cbw.Tag = 1;
			cbw.TransferDataLength = 8;
			cbw.CommandLength = 10;
			cbw.IsInput = true;

			cbw[0] = MspConsts.REQUEST_READ_CAPACITY;
			cbw[1] = LunHelper.GetNumber(logicalUnitNumber);
		}

		internal static void SetRequestSense(this CBW cbw, int logicalUnitNumber, bool extended = false)
		{
			var dataLength = extended ? 32 : 18;

			cbw.ClearCommandData();

			cbw.Tag = 1;
			cbw.TransferDataLength = dataLength;
			cbw.IsInput = true;
			cbw.CommandLength = 6;

			cbw[0] = MspConsts.REQUEST_REQUEST_SENSE;
			cbw[1] = LunHelper.GetNumber(logicalUnitNumber);
			cbw[4] = (byte)dataLength;
		}

		internal static void SetModeSense(this CBW cbw, int logicalUnitNumber, ModeSensePage page,
			int requestedLength = 0)
		{
			var includeDescriptor = false;

			if (requestedLength <= 0)
			{
				requestedLength = 8 + _pageLengths[page];
			}

			cbw.ClearCommandData();

			cbw.Tag = 1;
			cbw.TransferDataLength = requestedLength;
			cbw.IsInput = true;
			cbw.CommandLength = 10;

			cbw[0] = MspConsts.REQUEST_MODE_SENSE_10;
			cbw[1] = (byte)(LunHelper.GetNumber(logicalUnitNumber) + (includeDescriptor ? 0 : 8));
			cbw[2] = (byte)page;

			cbw[7] = (byte)((requestedLength & 0x0000FF00) >> 8);
			cbw[8] = (byte)((requestedLength & 0x000000FF) >> 0);
		}

		public static void SetRead10(this CBW cbw, int logicalUnitNumber, int logicalBlockAddress,
			int dataLength, int blockSize)
        {
			cbw.ClearCommandData();

			cbw.Tag = 1;
			cbw.TransferDataLength = dataLength;
			cbw.IsInput = true;
			cbw.CommandLength = 10;

			var blockCount = (int)Math.Ceiling((double)dataLength / blockSize);

			cbw[0] = MspConsts.REQUEST_READ_10;
			cbw[1] = LunHelper.GetNumber(logicalUnitNumber);
			
			cbw[2] = (byte)((logicalBlockAddress & 0xFF000000) >> 24);
			cbw[3] = (byte)((logicalBlockAddress & 0x00FF0000) >> 16);
			cbw[4] = (byte)((logicalBlockAddress & 0x0000FF00) >> 8);
			cbw[5] = (byte)((logicalBlockAddress & 0x000000FF) >> 0);
			
			cbw[7] = (byte)((blockCount & 0xFF00) >> 8);
			cbw[8] = (byte)((blockCount & 0x00FF) >> 0);
		}

		internal static void SetWrite10(this CBW cbw, int logicalUnitNumber, int logicalBlockAddress,
			int dataLength, int blockSize)
		{
			cbw.ClearCommandData();

			cbw.Tag = 1;
			cbw.TransferDataLength = dataLength;
			cbw.IsInput = false;
			cbw.CommandLength = 10;

			var blockCount = (int)Math.Ceiling((double)dataLength / blockSize);

			cbw[0] = MspConsts.REQUEST_WRITE_10;
			cbw[1] = LunHelper.GetNumber(logicalUnitNumber);
			
			cbw[2] = (byte)((logicalBlockAddress & 0xFF000000) >> 24);
			cbw[3] = (byte)((logicalBlockAddress & 0x00FF0000) >> 16);
			cbw[4] = (byte)((logicalBlockAddress & 0x0000FF00) >> 8);
			cbw[5] = (byte)((logicalBlockAddress & 0x000000FF) >> 0);
			
			cbw[7] = (byte)((blockCount & 0xFF00) >> 8);
			cbw[8] = (byte)((blockCount & 0x00FF) >> 0);
		}
	}
}
