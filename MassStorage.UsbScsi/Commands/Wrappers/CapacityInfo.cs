using MassStorage.UsbScsi.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassStorage.UsbScsi.Commands.Wrappers
{
    public class CapacityInfo
    {
        private readonly byte[] _buffer;

		public ulong LastBlockAddress => ByteHelper.ReadBigEndian(_buffer, 0);

		public uint BlockSize => ByteHelper.ReadBigEndian(_buffer, 4);

		public ulong Capacity => LastBlockAddress + 1;

		public CapacityInfo(byte[] buffer)
		{
			_buffer = buffer;
		}
	}
}
