using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidUsbStorageDriver.Helpers
{
    /// <summary>
    /// Helps to get correct number of Logical Unit.
    /// </summary>
    internal static class LunHelper
    {
        public static byte GetNumber(int decimalNumber)
        {
            if (decimalNumber < 0 || decimalNumber > 15)
                throw new ArgumentException("Parameter is out of range (0, 15)!", nameof(decimalNumber));

            return (byte)((decimalNumber / 4) * 16 + (decimalNumber % 4) * 64);
		}
    }
}
