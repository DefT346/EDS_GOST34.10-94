using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EDS_GHOST34._10_94
{
    internal class SignData
    {
        public BigInteger r { get; private set; }
        public BigInteger s { get; private set; }

        public SignData(BigInteger r, BigInteger s)
        {
            this.r = r;
            this.s = s;
        }

        public static SignData Read()
        {
            Console.Write("Введите r: ");
            var r = BigInteger.Parse(Console.ReadLine());

            Console.Write("Введите s: ");
            var s = BigInteger.Parse(Console.ReadLine());

            return new SignData(r, s);
        }
    }

    internal class EDSignature
    {
        public BigInteger p { get; private set; }
        public BigInteger q { get; private set; }
        public BigInteger a { get; private set; }

        public EDSignature()
        {
            Console.WriteLine("Генерация ключей...\n");
            var keys = new GOST341094Keys();
            this.p = keys.p; Console.WriteLine($"p: {p}");
            this.q = keys.q; Console.WriteLine($"q: {q}");
            this.a = keys.a; Console.WriteLine($"a: {a}");
        }

        public BigInteger CreatePrivateKey()
        {
            return RandomValue(q.GetBitLength() - 1);
        }

        public BigInteger GetPublicKey(BigInteger x) // y
        {
            return BigInteger.ModPow(a, x, p);
        }

        private static BigInteger ToHash(byte[] document)
        {
            var hashedDocument = ComputeSHA512(document);

            Utils.PrintByteArray("\nDocument hash: ", hashedDocument);

            return new BigInteger(hashedDocument.Concat(new byte[] { 0 }).ToArray());
        }

        public SignData Sign(BigInteger privateKey, byte[] document)
        {
            var H = ToHash(document);

            do
            {
                var k = RandomValue(q.GetBitLength() - 1);
                var r = Utils.mod(BigInteger.ModPow(a, k, p), q);

                if (r == 0) continue;

                var Hm = Utils.mod(H, q);
                if (Hm == 0) Hm = 1;

                var s = Utils.mod(privateKey * r + k * Hm, q);

                return new SignData(r, s);
            } 
            while (true);          
        }

        public static bool Verify(EDSignature signature, SignData sign, BigInteger publicKey, byte[] document)
        {
            var H = ToHash(document);

            var v = BigInteger.ModPow(H, signature.q - 2, signature.q);

            var z1 = Utils.mod(sign.s * v, signature.q);
            var z2 = Utils.mod((signature.q - sign.r) * v, signature.q);

            var u = Utils.mod(Utils.mod(BigInteger.ModPow(signature.a, z1, signature.p) * BigInteger.ModPow(publicKey, z2, signature.p), signature.p), signature.q);

            return u == sign.r;
        }

        private static byte[] ComputeSHA512(byte[] data)
        {
            using (System.Security.Cryptography.SHA512 shaM = System.Security.Cryptography.SHA512.Create())
                return shaM.ComputeHash(data);
        }

        private BigInteger RandomValue(long bitLength) // x
        {
            if (bitLength < 0 || bitLength < 128 || bitLength > q.GetBitLength())
            {
                throw new ArgumentException("Wrong key length");
            }

            BigInteger result;
            do
            {
                result = new BigInteger(BigPrime.GetRandom(bitLength));
            } while (result < 0 || result > q);
            return result;
        }
    }
}
