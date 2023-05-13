using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EDS_GHOST34._10_94
{
    public class LinearCongruentialGenerator
    {

        private BigInteger seed;
        private BigInteger a;
        private BigInteger c;
        private BigInteger m;

        public LinearCongruentialGenerator(BigInteger seed, BigInteger a, BigInteger c, BigInteger m)
        {
            this.seed = seed;
            this.a = a;
            this.c = c;
            this.m = m;
        }

        public BigInteger[] rand(int rm)
        {
            //return iterate(seed, x-> (a * x + c) % m).skip(1); // new ArraySegment<string>(a, 1, 2) это limit()
            var y = new BigInteger[rm];

            for (int i = 0; i < rm - 1; i++)
            {
                y[i + 1] = Utils.mod(a * y[i] + c, BigInteger.Pow(2, 16));
            }
            return y;
        }

        public void setSeed(BigInteger seed)
        {
            this.seed = seed;
        }

        public BigInteger getSeed()
        {
            return seed;
        }
    }
}
