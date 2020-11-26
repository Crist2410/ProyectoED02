using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Libreria
{
    public class DiffieHellman
    {
        public int GeneracionPublicKey(int RandomSecret)
        {
            int primo = 233;
            int entero = 80;
            return Convert.ToInt32(BigInteger.ModPow(entero, (BigInteger)RandomSecret, (BigInteger)primo));
        }

        public int GeneracionSecretKey(int RandomSecret, int PublicKey)
        {
            int primo = 233;
            return Convert.ToInt32(BigInteger.ModPow(PublicKey, (BigInteger)RandomSecret, (BigInteger)primo));
        }
    }
}
