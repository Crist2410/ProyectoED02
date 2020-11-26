using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MVC_Proyecto.Models
{
    public class Usuario
    {
        //[Required(ErrorMessage ="this field is required")]
        public string User { get; set; }
        public string PassWord { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Genero { get; set; }
        public int Edad { get; set; }
        public List<Usuario> Contactos = new List<Usuario>();
    }
}
