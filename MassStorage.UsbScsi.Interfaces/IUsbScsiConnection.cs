using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassStorage.UsbScsi.Interfaces
{
    public interface IUsbScsiConnection : IDisposable
    {
        bool IsConnected { get; }

        IUsbScsiConnectionManager ConnectionManager { get; }

        bool Open();
        int ControlTransfer(
            int requestType,
            int request,
            int value,
            int index,
            byte[]? buffer,
            int offset,
            int length,
            int timeout);

        int BulkTransferIn(byte[] buffer, int offset, int length, int timeout);
        int BulkTransferOut(byte[] buffer, int offset, int length, int timeout);
    }
}
