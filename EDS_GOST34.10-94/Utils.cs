using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EDS_GHOST34._10_94
{
    internal class Utils
    {
        public static BigInteger mod(BigInteger x, BigInteger m)
        {
            return (x % m + m) % m;
        }

        public static BigInteger DivideRoundingUp(BigInteger x, BigInteger y)
        {
            // TODO: Define behaviour for negative numbers
            BigInteger remainder;
            BigInteger quotient = BigInteger.DivRem(x, y, out remainder);
            return remainder == 0 ? quotient : quotient + 1;
        }

        public static BigInteger DivideRoundingDown(BigInteger x, BigInteger y)
        {
            // TODO: Define behaviour for negative numbers
            BigInteger remainder;
            BigInteger quotient = BigInteger.DivRem(x, y, out remainder);

            if (quotient >= 0)
            {
                return quotient;
            }
            else
            {
                if (remainder != 0)
                    return quotient - 1;
                else
                    return quotient;
            }
        }

        public static void PrintByteArray(string message, byte[] bytes)
        {
            string hex = message + Byte16(bytes);
            Console.WriteLine(hex);
        }

        public static string Byte16(byte[] bytes)
        {
            return string.Join(" ", bytes.Select(x => x.ToString("X2")));
        }

        public byte[] Parse16(string row)
        {
            List<byte> buffer = new List<byte>();
            var arr = row.Split(' ');
            for(int i = 0; i < arr.Length; i++)
            {
                buffer.Add((byte)Convert.ToInt16(arr[i], 16));
            }
            return buffer.ToArray();
        }
    }
}
