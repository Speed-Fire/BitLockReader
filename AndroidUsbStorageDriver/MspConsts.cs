using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidUsbStorageDriver
{
    internal static class MspConsts
    {
        public const int REQUEST_GET_MAX_LUN = 0xFE;
        public const int REQUEST_MASS_STORAGE_RESET = 0xFF;
        public const int REQUEST_GET_DESCRIPTOR = 0x06;
        public const int REQUEST_READ_CAPACITY = 0x25;
        public const int REQUEST_REQUEST_SENSE = 0x03;
        public const int REQUEST_READ_10 = 0x28;
        public const int REQUEST_WRITE_10 = 0x2A;
        public const int REQUEST_MODE_SENSE_10 = 0x5A;

        public const int VALUE_DEVICE_DESCRIPTOR = 0x0100;

        public const int REQUSET_TYPE_STANDARD = 0x00;
		public const int REQUSET_TYPE_CLASS = 0x21;
		public const int REQUSET_TYPE_VENDOR = 0x40;

        public const int DIRECTION_IN = 0x80;
        public const int DIRECTION_OUT = 0x00;
	}
}
