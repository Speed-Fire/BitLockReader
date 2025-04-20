using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassStorage.UsbScsi.Helpers
{
    internal static class ByteHelper
    {
        public static void WriteLittleEndian(byte[] buffer, int offset, int value)
        {
            var bytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian)
            {
                buffer[offset] = bytes[0];
                buffer[offset + 1] = bytes[1];
                buffer[offset + 2] = bytes[2];
                buffer[offset + 3] = bytes[3];
            }
            else
            {
				buffer[offset] = bytes[3];
				buffer[offset + 1] = bytes[2];
				buffer[offset + 2] = bytes[1];
				buffer[offset + 3] = bytes[0];
			}
        }

        public static int ReadLittleEndian(byte[] buffer, int offset)
        {
            var bytes = new byte[4];

            if(BitConverter.IsLittleEndian)
            {
                bytes[0] = buffer[offset];
                bytes[1] = buffer[offset + 1];
                bytes[2] = buffer[offset + 2];
                bytes[3] = buffer[offset + 3];
            }
            else
            {
				bytes[0] = buffer[offset + 3];
				bytes[1] = buffer[offset + 2];
				bytes[2] = buffer[offset + 1];
				bytes[3] = buffer[offset];
			}

            return BitConverter.ToInt32(bytes, 0);
        }

		public static uint ReadBigEndian(byte[] buffer, int offset)
		{
			var bytes = new byte[4];
			
			if (!BitConverter.IsLittleEndian)
			{
				bytes[0] = buffer[offset];
				bytes[1] = buffer[offset + 1];
				bytes[2] = buffer[offset + 2];
				bytes[3] = buffer[offset + 3];
			}
			else
			{
				bytes[0] = buffer[offset + 3];
				bytes[1] = buffer[offset + 2];
				bytes[2] = buffer[offset + 1];
				bytes[3] = buffer[offset];
			}

			return BitConverter.ToUInt32(bytes, 0);
		}
	}
}
