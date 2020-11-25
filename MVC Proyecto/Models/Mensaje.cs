using System;
using System.Collections.Generic;
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
    }
}
