using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskLayout.Domain.Exceptions
{
    public class ReadonlyPartitionException : Exception
    {
        public ReadonlyPartitionException() : base("This partition is readonly. If you still want to write data to it, then use \'force\' parameter. Have in mind - you do it on your own risk.")
        {

        }
    }
}
