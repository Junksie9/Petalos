using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Petalos.Helpers
{
    public class Cifrado
    {
        public static string GetHash(string cadena)
        {
            var algoritmo = SHA512.Create();
            var arreglo = Encoding.UTF8.GetBytes(cadena + "regu");
            var hash = algoritmo.ComputeHash(arreglo).Select(x => x.ToString("X2"));
            return string.Join("", hash);
        }
    }
}
