using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCModuleControl.Tools
{
    public class Converting
    {
        public static string ByteArrayToHexString(byte[] data)
        {
            return string.Join(", ", data.Select(d => "0x" + d.ToString("X2")));
        }
    }
}
