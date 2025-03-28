using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidUsbStorageDriver.Commands
{
    public class ControlCommand
    {
		public ControlCommand(int requestType, int request, int value, int index, byte[]? buffer, int offset, int length)
		{
			RequestType = requestType;
			Request = request;
			Value = value;
			Index = index;
			Buffer = buffer;
			Offset = offset;
			Length = length;
		}

		public int RequestType { get; }
        public int Request { get; }
        public int Value { get; }
        public int Index { get; }
        public byte[]? Buffer { get; }
        public int Offset { get; }
        public int Length { get; }
    }
}
