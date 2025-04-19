using AndroidUsbStorageDriver.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidUsbStorageDriver.Commands.Wrappers
{
    internal class SenseErrorInfo
    {
        private readonly byte[] _buffer = new byte[18];

        public byte[] Buffer => _buffer;

        public ResponseCode ResponseCode => (ResponseCode)_buffer[0];

		public SenseKey SenseKey => (SenseKey)_buffer[2];

        public byte ASC => _buffer[12];

        public byte ASCQ => _buffer[13];
    }
}
