using AndroidUsbStorageDriver.Commands.Wrappers;
using MassStorage.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidUsbStorageDriver
{
    internal class ErrorHandler
    {
		private readonly UsbMassStorageProtocol _protocol;
		private readonly SenseErrorInfo _errorInfo;

		public ErrorHandler(UsbMassStorageProtocol protocol)
		{
			_protocol = protocol;
			_errorInfo = new();
		}

		public void ThrowIfError(int logicalUnitNumber, CommandStatus status)
		{
			switch (status)
			{
				case CommandStatus.Success:
					return;
				case CommandStatus.Failed:
					HandleError(logicalUnitNumber);
					break;
				case CommandStatus.PhaseError:
					throw new PhaseErrorException();
			}
		}

		private bool RequestSense(int logicalUnitNumber)
		{
			var res = _protocol.RequestSense(logicalUnitNumber,
				_errorInfo.Buffer, 0);

			return res == CommandStatus.Success;
		}

		private void HandleError(int logicalUnitNumber)
		{
			var senseRes = RequestSense(logicalUnitNumber);

			if (!senseRes)
				throw new PhaseErrorException();

			var sensekey = _errorInfo.SenseKey;
			var asc = _errorInfo.ASC;
			var ascq = _errorInfo.ASCQ;

			switch (sensekey)
			{
				case Enums.SenseKey.NoSense:
					return;
				case Enums.SenseKey.MediumError:
					throw new CorruptedStorageException(asc, ascq);
				case Enums.SenseKey.HardwareError:
					throw new ControllerErrorException(asc, ascq);
				case Enums.SenseKey.DataProtect:
					throw new StorageReadOnlyException(asc, ascq);
				case Enums.SenseKey.IllegalRequest:
				case Enums.SenseKey.UnitAttention:
				case Enums.SenseKey.RecoveredError:
				case Enums.SenseKey.NotReady:
				case Enums.SenseKey.BlankCheck:
				case Enums.SenseKey.VendorSpecific:
				case Enums.SenseKey.CopyAborted:
				case Enums.SenseKey.AbortedCommand:
				case Enums.SenseKey.Equal:
				case Enums.SenseKey.VolumeOverflow:
				case Enums.SenseKey.Miscompare:
					throw new MassStorageException(sensekey.ToString(), (byte)sensekey, asc, ascq);
			}
		}
	}
}
