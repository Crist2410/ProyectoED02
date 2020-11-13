using API_Proyecto.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Proyecto.Models
{
    public class ProyectoDataBaseSettings : IProyectoDataBaseSettings
    {
        public string ProyectoCollectionName { get; set; }
        public string ConnectionString { get; set ; }
        public string DatabaseName { get; set; }
    }
}
