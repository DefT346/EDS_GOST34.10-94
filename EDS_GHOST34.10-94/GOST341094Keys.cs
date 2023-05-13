using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EDS_GHOST34._10_94
{
    internal class GOST341094Keys
    {

        public enum KeySize { k512, k1024 }

        public BigInteger p { get; private set; }
        public BigInteger q { get; private set; }
        public BigInteger a { get; private set; }

        private LinearCongruentialGenerator generator;

        public GOST341094Keys()
        {
            generator = new LinearCongruentialGenerator(0x3DFC46F1, 97781173, 0xD, BigInteger.Pow(2, 32));

            var result = GeneratePrimes512();

            p = result[0];
            q = result[1];
            a = GenerateA(p, q);
        }

        private BigInteger GenerateA(BigInteger p, BigInteger q)
        {
            BigInteger d;
            BigInteger f;
            do
            {
                do
                {
                    d = new BigInteger(BigPrime.GetRandom(p.GetBitLength() - 1));
                } while (d <= 1 || d >= p - 1);

                f = BigInteger.ModPow(d, BigInteger.Divide(p - 1, q), p);
            } while (f == 1);
            return f;
        }

        private BigInteger[] GeneratePrimes512()
        {
            int bitLength = 512;
            List<int> t = new List<int>();
            t.Add(bitLength);

            int index = 0;
            while (t[index] >= 33)
            {
                int value = (int)Math.Floor(t[index] / 2d);
                t.Add(value);
                index++;
            }

            BigInteger[] primes = new BigInteger[t.Count];

            primes[index] = new BigPrime(t[index]).ToBigInteger();
            int m = index - 1;
            bool flag = true;
            do
            {
                int r = (int)Math.Ceiling(t[m] / 32d);
                BigInteger n = BigInteger.Zero;
                BigInteger k = BigInteger.Zero;
                do
                {
                    if (flag)
                    {
                        BigInteger[] y = generator.rand(r);
                        BigInteger sum = BigInteger.Zero;
                        for (int i = 0; i < r - 1; i++)
                        {
                            BigInteger tmp = y[i] * BigInteger.Pow(2, 32);
                            sum += tmp;
                        }
                        sum += generator.getSeed();

                        generator.setSeed(y[r - 1]);

                        BigInteger tmp1 = Utils.DivideRoundingUp(BigInteger.Pow(2, t[m] - 1), primes[m + 1]);

                        BigInteger tmp2 = Utils.DivideRoundingDown(BigInteger.Pow(2, t[m] - 1) * sum, primes[m + 1] * BigInteger.Pow(2, 32 * r));

                        n = tmp1 + tmp2;

                        if (n % 2 != 0)
                        {
                            n = n + 1;
                        }

                        k = BigInteger.Zero;
                    }

                    primes[m] = primes[m + 1] * (n + k) + 1;

                    if (primes[m] > BigInteger.Pow(2, t[m]))
                    {
                        flag = true;
                        continue;
                    }

                    if (BigInteger.ModPow(2, primes[m + 1] * (n + k), primes[m]) != 1 || BigInteger.ModPow(2, n + k, primes[m]) == 1)
                    {
                        flag = false;
                        k += 2;
                    }

                    else
                    {
                        flag = true;
                        break;
                    }
                } while (true);
                m--;
            } while (m >= 0);
            return primes;
        }
    }
}
