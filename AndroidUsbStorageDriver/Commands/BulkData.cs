using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidUsbStorageDriver.Commands
{
    public class BulkData
    {
        public byte[] Buffer { get; }
        public int Length { get; }
        public int Offset {  get; }

		public BulkData(int length)
		{
			Length = length;
			Buffer = new byte[Length];
			Offset = 0;
		}

		public BulkData(byte[] buffer)
		{
			Buffer = buffer;
			Length = buffer.Length;
			Offset = 0;
		}

		public BulkData(byte[] buffer, int offset, int length)
		{
			Buffer= buffer;
			Length = length;
			Offset = offset;
		}
	}
}
