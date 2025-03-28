using AndroidUsbStorageDriver.Helpers;

namespace AndroidUsbStorageDriver.Commands
{
	public class CBW
	{
		private const int TAG_OFFSET = 0x04;
		private const int TRANSFER_DATA_LENGTH_OFFSET = 0x08;
		private const int DIRECTION_OFFSET = 0x0c;
		private const int LNU_OFFSET = 0x0d;
		private const int COMMAND_LENGTH_OFFSET = 0x0e;
		private const int COMMAND_OFFSET = 0x0f;

		public byte[] Buffer { get; } = new byte[31];

		internal int Tag 
		{ 
			get => ByteHelper.ReadLittleEndian(Buffer, TAG_OFFSET); 
			set => ByteHelper.WriteLittleEndian(Buffer, TAG_OFFSET, value);
		}

		protected int TransferDataLength
		{
			get => ByteHelper.ReadLittleEndian(Buffer, TRANSFER_DATA_LENGTH_OFFSET);
			set => ByteHelper.WriteLittleEndian(Buffer, TRANSFER_DATA_LENGTH_OFFSET, value);
		}

		protected bool IsInput
		{
			get => Buffer[DIRECTION_OFFSET] == 0x80;
			set => Buffer[DIRECTION_OFFSET] = (byte)(value ? 0x80 : 0);
		}

		protected byte LUN
		{
			get => Buffer[LNU_OFFSET];
			set => Buffer[LNU_OFFSET] = value;
		}

		protected byte CommandLength
		{
			get => Buffer[COMMAND_LENGTH_OFFSET];
			set => Buffer[COMMAND_LENGTH_OFFSET] = value;
		}

		protected byte this[int id]
		{
			get => Buffer[COMMAND_OFFSET + id];
			set => Buffer[COMMAND_OFFSET + id] = value;
		}

		public CBW()
		{
			Buffer[0] = 0x55; // CBW Signature
			Buffer[1] = 0x53;
			Buffer[2] = 0x42;
			Buffer[3] = 0x43;
		}
	}
}
