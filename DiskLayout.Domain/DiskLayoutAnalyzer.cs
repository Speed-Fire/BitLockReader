using DiskLayout.Domain.Exceptions;
using DiskLayout.Domain.Interfaces;
using MassStorage.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskLayout.Domain
{
    public sealed class DiskLayoutAnalyzer
    {
        private readonly List<IDiskLayoutDetector> _detectors = [];

		public void Add(IDiskLayoutDetector detector)
        {
            _detectors.Add(detector);
        }

        public void AddRange(IEnumerable<IDiskLayoutDetector> detectors)
        {
            _detectors.AddRange(detectors);
        }

        public IDiskPartitionTable DetectLayout(ILogicalUnit disk)
        {
            var lba0 = new byte[disk.BlockSize];
            disk.Read(0, lba0, 0, lba0.Length);

            foreach(var detector in _detectors)
            {
                var table = detector.Detect(lba0, disk);

                if (table != null)
                    return table;
            }

            throw new UnknownDiskLayoutException();
        }
    }
}
