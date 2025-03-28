using AndroidUsbStorageDriver.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidUsbStorageDriver.Commands
{
    public class CapacityInfo
    {
        private readonly byte[] _buffer;

		public uint LastBlockAddress => (uint)ByteHelper.ReadBigEndian(_buffer, 0);

		public int BlockSize => ByteHelper.ReadBigEndian(_buffer, 4);

		public uint Capacity => LastBlockAddress - 1 + (uint)BlockSize;

		public CapacityInfo(byte[] buffer)
		{
			_buffer = buffer;
		}
	}
}
