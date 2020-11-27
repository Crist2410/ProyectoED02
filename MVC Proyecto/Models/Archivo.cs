using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Proyecto.Models
{
    public class Archivo
    {
        public string Nombre { get; set; }
        public byte[] Contenido { get; set; }
        public string Ruta { get; set; }
    }
}
