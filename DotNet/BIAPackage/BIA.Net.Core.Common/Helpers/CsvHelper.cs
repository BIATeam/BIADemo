namespace BIA.Net.Core.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class CsvHelper
    {
        public static byte[] ToBytes(string csv, Encoding encoding, bool includeBom = false)
        {
            var data = encoding.GetBytes(csv);

            if (!includeBom)
            {
                return data;
            }

            var preamble = encoding.GetPreamble();
            if (preamble == null || preamble.Length == 0)
            {
                return data;
            }

            var result = new byte[preamble.Length + data.Length];
            Buffer.BlockCopy(preamble, 0, result, 0, preamble.Length);
            Buffer.BlockCopy(data, 0, result, preamble.Length, data.Length);
            return result;
        }
    }
}
