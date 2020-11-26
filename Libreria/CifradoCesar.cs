using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria
{
    public class CifradoCesar
    {
        Dictionary<char, char> DicOriginal = new Dictionary<char, char>();
        Dictionary<char, char> DicLlave = new Dictionary<char, char>();
        string Llave = "xilofanewusmy";

        public string Cifrar(string texto)
        {
            RealizarDiccionarios();
            string res = "";
            foreach (var Item in texto)
                if (DicOriginal.ContainsKey(Item))
                    res += DicOriginal[(char)Item];
                else
                    res += Item;

            return res;
        }
        public string Decifrar(string texto)
        {
            RealizarDiccionarios();
            string res = "";
            foreach (var Item in texto)
                if (DicLlave.ContainsKey((char)Item))
                    res += DicLlave[(char)Item];
                else
                    res += Item;

            return res;
        }
        void RealizarDiccionarios()
        {
            DicLlave = new Dictionary<char, char>();
            DicOriginal = new Dictionary<char, char>();
            string Diccionario = Llave.ToUpper();
            for (int i = 65; i < 91; i++)
                if (!Diccionario.Contains((char)i))
                    Diccionario += (char)i;
            string TextoAux = Diccionario;
            //Mayusculas
            for (int i = 65; i < 91; i++)
            {
                char value = Convert.ToChar(TextoAux.Substring(0, 1));
                TextoAux = TextoAux.Substring(1);
                DicOriginal.Add((char)i, value);
                DicLlave.Add(value, (char)i);
            }
            TextoAux = Diccionario.ToLower();
            //Minisculas
            for (int i = 97; i < 123; i++)
            {
                char value = Convert.ToChar(TextoAux.Substring(0, 1));
                TextoAux = TextoAux.Substring(1);
                DicOriginal.Add((char)i, value);
                DicLlave.Add(value, (char)i);
            }
        }
    }
}
