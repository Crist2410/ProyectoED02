using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Libreria
{
    public class SDES
    {
        string k1, k2;

        public string LlavePermutaciones(int Key)
        {
            string Numero = Convert.ToString(Key, 2).PadLeft(10, '0');
            return Numero;
        }

        public void Generacionllaves(string llave)
        {
            string Permutar10 = ProcesoPermutar10(llave);

            string Mitad1 = Permutar10.Substring(0, 5);
            string Mitad2 = Permutar10.Substring(5, 5);

            string LS1 = ProcesoLS1(Mitad1);
            string suma = LS1;
            LS1 = ProcesoLS1(Mitad2);
            suma += LS1;

            k1 = ProcesoPermutacion8(suma);

            Mitad1 = suma.Substring(0, 5);
            Mitad2 = suma.Substring(5, 5);

            LS1 = ProcesoLS2(Mitad1);
            suma = LS1;
            LS1 = ProcesoLS2(Mitad2);
            suma += LS1;

            k2 = ProcesoPermutacion8(suma);
        }

        public string cifrado(string texto, int RandomSecret, int PublicKey)
        {
            DiffieHellman diffie = new DiffieHellman();

            int SecretKey = diffie.GeneracionSecretKey(RandomSecret, PublicKey);

            Generacionllaves(LlavePermutaciones(SecretKey));

            byte[] ArregloBytes = Encoding.UTF8.GetBytes(texto);
            texto = "";
            List<byte> ListaBytes = new List<byte>();

            foreach (var item in ArregloBytes)
            {
                //conversion de numeros a binario
                string Numero = Convert.ToString(item, 2).PadLeft(8, '0');

                //permutacion inicial
                string Inicial = PermutacionInicial(Numero);

                //division y, expandir y permutar
                string Mitad1 = Inicial.Substring(0, 4);
                string Mitad2 = Inicial.Substring(4, 4);
                string PrimeraMitad = Mitad1;
                string SegundaMitad = Mitad2;
                string Expandir = ProcesoExpandir(Mitad2);

                //sumatoria k2 y, expandir y permutar
                string Suma = SumaBinarios(Expandir, k1);

                //separacion suma y obtencion de numeros por S0 y S1
                string SumaS = ProcesoSumaS(Suma);

                //permutacion 4 y sumatoria con la primera mitad
                string Permutacion4 = ProcesoPermutacion4(SumaS);
                Suma = SumaBinarios(PrimeraMitad, Permutacion4);

                //union segunda mitad con la suma anterior
                string Swap = SegundaMitad + Suma;

                //division y, expandir y permutar
                Mitad1 = Swap.Substring(0, 4);
                Mitad2 = Swap.Substring(4, 4);
                PrimeraMitad = Mitad1;
                SegundaMitad = Mitad2;
                Expandir = ProcesoExpandir(Mitad2);

                //sumatoria k2 y, expandir y permutar
                Suma = SumaBinarios(Expandir, k2);

                //separacion suma y obtencion de numeros por S0 y S1
                SumaS = ProcesoSumaS(Suma);

                //permutacion 4 y sumatoria con la primera mitad
                Permutacion4 = ProcesoPermutacion4(SumaS);
                Suma = SumaBinarios(PrimeraMitad, Permutacion4);

                //union segunda mitad con la suma anterior y permutacion inicial inversa
                Swap = Suma + SegundaMitad;
                string PermutacionInversa = ProcesoInverso(Swap);

                //conversion a byte
                byte respuesta = Convert.ToByte(PermutacionInversa.Substring(0, 8), 2);
                ListaBytes.Add(respuesta);
            }
            texto = Convert.ToBase64String(ListaBytes.ToArray());
            return texto;
        }
        public string descifrado(string texto, int RandomSecret, int PublicKey)
        {
            DiffieHellman diffie = new DiffieHellman();

            int SecretKey = diffie.GeneracionSecretKey(RandomSecret, PublicKey);

            Generacionllaves(LlavePermutaciones(SecretKey));

            byte[] ArregloBytes = Convert.FromBase64String(texto);
            List<byte> ListaBytes = new List<byte>();
            string[,] S0 = llenadoS0();
            string[,] S1 = llenadoS1();

            foreach (var item in ArregloBytes)
            {
                //conversion de numeros a binario
                string Numero = Convert.ToString(item, 2).PadLeft(8, '0');
                //string Numero = "10001000";

                //permutacion inicial
                string Inicial = PermutacionInicial(Numero);

                //division y, expandir y permutar
                string Mitad1 = Inicial.Substring(0, 4);
                string Mitad2 = Inicial.Substring(4, 4);
                string PrimeraMitad = Mitad1;
                string SegundaMitad = Mitad2;
                string Expandir = ProcesoExpandir(Mitad2);

                //sumatoria k2 y, expandir y permutar
                string Suma = SumaBinarios(Expandir, k2);

                //separacion suma y obtencion de numeros por S0 y S1
                string SumaS = ProcesoSumaS(Suma);

                //permutacion 4 y sumatoria con la primera mitad
                string Permutacion4 = ProcesoPermutacion4(SumaS);
                Suma = SumaBinarios(PrimeraMitad, Permutacion4);

                //union segunda mitad con la suma anterior
                string Swap = SegundaMitad + Suma;

                //division y, expandir y permutar
                Mitad1 = Swap.Substring(0, 4);
                Mitad2 = Swap.Substring(4, 4);
                PrimeraMitad = Mitad1;
                SegundaMitad = Mitad2;
                Expandir = ProcesoExpandir(Mitad2);

                //sumatoria k2 y, expandir y permutar
                Suma = SumaBinarios(Expandir, k1);

                //separacion suma y obtencion de numeros por S0 y S1
                SumaS = ProcesoSumaS(Suma);

                //permutacion 4 y sumatoria con la primera mitad
                Permutacion4 = ProcesoPermutacion4(SumaS);
                Suma = SumaBinarios(PrimeraMitad, Permutacion4);

                //union segunda mitad con la suma anterior y permutacion inicial inversa
                Swap = Suma + SegundaMitad;
                string PermutacionInversa = ProcesoInverso(Swap);

                //conversion a byte
                byte respuesta = Convert.ToByte(PermutacionInversa.Substring(0, 8), 2);
                ListaBytes.Add(respuesta);
            }
            texto = Encoding.UTF8.GetString(ListaBytes.ToArray());
            return texto;
        }
        public string SumaBinarios(string x, string y)
        {
            string Suma = "";
            for (int i = 0; i < x.Length; i++)
            {
                if (x.Substring(i, 1) == y.Substring(i, 1))
                    Suma += "0";
                else
                    Suma += "1";
            }

            return Suma;
        }
        public string ProcesoSumaS(string Suma)
        {
            string[,] S0 = llenadoS0();
            string[,] S1 = llenadoS1();
            string Mitad1 = Suma.Substring(0, 1) + Suma.Substring(3, 1) + Suma.Substring(1, 1) + Suma.Substring(2, 1);
            string Mitad2 = Suma.Substring(4, 1) + Suma.Substring(7, 1) + Suma.Substring(5, 1) + Suma.Substring(6, 1);
            int UbicacionS1 = Convert.ToInt32(Mitad1.Substring(0, 2), 2);
            int UbicacionS2 = Convert.ToInt32(Mitad1.Substring(2, 2), 2);
            Suma = S0[UbicacionS1, UbicacionS2];
            UbicacionS1 = Convert.ToInt32(Mitad2.Substring(0, 2), 2);
            UbicacionS2 = Convert.ToInt32(Mitad2.Substring(2, 2), 2);
            Suma += S1[UbicacionS1, UbicacionS2];
            return Suma;
        }
        public string ProcesoExpandir(string Mitad2)
        {
            string Expandir = Mitad2.Substring(3, 1) + Mitad2.Substring(0, 1) + Mitad2.Substring(1, 1) + Mitad2.Substring(2, 1) + Mitad2.Substring(1, 1) + Mitad2.Substring(2, 1) + Mitad2.Substring(3, 1) + Mitad2.Substring(0, 1);
            return Expandir;
        }
        public string[,] llenadoS0()
        {
            string[,] S0 = new string[4, 4];
            S0[0, 0] = "01";
            S0[0, 1] = "00";
            S0[0, 2] = "11";
            S0[0, 3] = "10";
            S0[1, 0] = "11";
            S0[1, 1] = "10";
            S0[1, 2] = "01";
            S0[1, 3] = "00";
            S0[2, 0] = "00";
            S0[2, 1] = "10";
            S0[2, 2] = "01";
            S0[2, 3] = "11";
            S0[3, 0] = "11";
            S0[3, 1] = "01";
            S0[3, 2] = "11";
            S0[3, 3] = "10";
            return S0;
        }
        public string[,] llenadoS1()
        {
            string[,] S0 = new string[4, 4];
            S0[0, 0] = "00";
            S0[0, 1] = "01";
            S0[0, 2] = "10";
            S0[0, 3] = "11";
            S0[1, 0] = "10";
            S0[1, 1] = "00";
            S0[1, 2] = "01";
            S0[1, 3] = "11";
            S0[2, 0] = "11";
            S0[2, 1] = "00";
            S0[2, 2] = "01";
            S0[2, 3] = "00";
            S0[3, 0] = "10";
            S0[3, 1] = "01";
            S0[3, 2] = "00";
            S0[3, 3] = "11";
            return S0;
        }
        public string PermutacionInicial(string Numero)
        {
            return (Numero.Substring(1, 1) + Numero.Substring(5, 1) + Numero.Substring(2, 1) + Numero.Substring(0, 1) + Numero.Substring(3, 1) + Numero.Substring(7, 1) + Numero.Substring(4, 1) + Numero.Substring(6, 1));
        }
        public string ProcesoPermutacion4(string SumaS)
        {
            return (SumaS.Substring(1, 1) + SumaS.Substring(3, 1) + SumaS.Substring(2, 1) + SumaS.Substring(0, 1));
        }
        public string ProcesoInverso(string Swap)
        {
            return (Swap.Substring(3, 1) + Swap.Substring(0, 1) + Swap.Substring(2, 1) + Swap.Substring(4, 1) + Swap.Substring(6, 1) + Swap.Substring(1, 1) + Swap.Substring(7, 1) + Swap.Substring(5, 1));
        }
        public string ProcesoPermutar10(string llave)
        {
            return (llave.Substring(2, 1) + llave.Substring(4, 1) + llave.Substring(1, 1) + llave.Substring(6, 1) + llave.Substring(3, 1) + llave.Substring(9, 1) + llave.Substring(0, 1) + llave.Substring(8, 1) + llave.Substring(7, 1) + llave.Substring(5, 1));
        }
        public string ProcesoLS1(string Mitad)
        {
            return (Mitad.Substring(1, 1) + Mitad.Substring(2, 1) + Mitad.Substring(3, 1) + Mitad.Substring(4, 1) + Mitad.Substring(0, 1));
        }
        public string ProcesoLS2(string Mitad)
        {
            return (Mitad.Substring(2, 1) + Mitad.Substring(3, 1) + Mitad.Substring(4, 1) + Mitad.Substring(0, 1) + Mitad.Substring(1, 1));
        }
        public string ProcesoPermutacion8(string suma)
        {
            return (suma.Substring(5, 1) + suma.Substring(2, 1) + suma.Substring(6, 1) + suma.Substring(3, 1) + suma.Substring(7, 1) + suma.Substring(4, 1) + suma.Substring(9, 1) + suma.Substring(8, 1));
        }
    }
}
