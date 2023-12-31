using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumen.Api.Utils
{
    public static class Utilities
    {
        static Random _random = new Random();
        public static byte RandomByte()
        {
            return (byte)_random.Next(0, 256);
        }

        public static double RandomDouble()
        {
            return _random.NextDouble();
        }

        public static double RandomDouble(double low, double high)
        {
            return _random.NextDouble() * (high - low) + low;
        }
    }
}
