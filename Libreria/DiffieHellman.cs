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
            BigInteger value = BigInteger.ModPow(entero, (BigInteger)RandomSecret, (BigInteger)primo);
            int numero = (int)value;
            return numero;
        }

        public int GeneracionSecretKey(int RandomSecret, int PublicKey)
        {
            int primo = 233;
            BigInteger value = BigInteger.ModPow(PublicKey, (BigInteger)RandomSecret, (BigInteger)primo);
            int numero = (int)value;
            return numero;
        }
    }
}
