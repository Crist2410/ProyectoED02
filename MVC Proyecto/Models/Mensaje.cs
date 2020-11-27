using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Proyecto.Models
{
    public class Mensaje
    {
        public string Texto { get; set; }
        public string Emisor { get; set; }
        public string Receptor { get; set; }
        public string Chat { get; set; }
        public DateTime Fecha { get; set; }
        public int RandomSecret { get; set; }
        public int PublicKeyUser { get; set; }
        public int PublicKey { get; set; }
        public string File { get; set; }
        public string FileNombre { get; set; }
        public IFormFile Archivo;


    }
}
