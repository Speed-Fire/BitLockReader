using MassStorage.UsbScsi.Helpers;

namespace MassStorage.UsbScsi.Commands.Wrappers
{
	public enum CommandStatus
	{
		Success = 0,
		Failed,
		PhaseError
	}

	public class CSW
	{
		private const int TAG_OFFSET = 0x04;
		private const int RESIDUE_OFFSET = 0x08;
		private const int STATUS_OFFSET = 0x0c;

		public byte[] Buffer { get; } = new byte[13];

		public bool IsValid => ByteHelper.ReadLittleEndian(Buffer, 0) == 0x53425355;

		public int Tag => ByteHelper.ReadLittleEndian(Buffer, TAG_OFFSET);

		public int Residue => ByteHelper.ReadLittleEndian(Buffer, RESIDUE_OFFSET);

		public CommandStatus Status => (CommandStatus)Buffer[STATUS_OFFSET];
	}
}
