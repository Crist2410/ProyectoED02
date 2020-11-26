using API_Proyecto.Interfaces;
using API_Proyecto.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Proyecto.Servicios
{
    public class ServicioContactos
    {
        private readonly IMongoCollection<Contactos> _Contactos;
        public ServicioContactos(IProyectoDataBaseSettings Interface)
        {
            var Cliente = new MongoClient(Interface.ConnectionString);
            var DataBase = Cliente.GetDatabase(Interface.DatabaseName);
            Interface.ProyectoCollectionName = "Contactos";
            _Contactos = DataBase.GetCollection<Contactos>(Interface.ProyectoCollectionName);
        }
        public List<Contactos> ObtenerTodos()
        {
            return _Contactos.Find(Contactos => true).ToList();
        }
        public Contactos AgregarContacto(Contactos Contacto)
        {
            _Contactos.InsertOne(Contacto);
            return Contacto;
        }
        public List<Contactos> ObtenerbyUser(string User)
        {
            return _Contactos.Find(Contactos => Contactos.Usuario == User || Contactos.Contact == User).ToList();
        }
        public List<Contactos> ObtenerContactos(string User)
        {
            return _Contactos.Find(Contactos => Contactos.Usuario == User).ToList();
        }
    }
}
