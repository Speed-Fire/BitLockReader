using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassStorage.UsbScsi.Enums
{
    internal enum ResponseCode
    {
        CurrentErrors = 0x70,
        DeferredErrors,
        CurrentErrorsNewFormat,
		DeferredErrorsNewFormat
	}
}
