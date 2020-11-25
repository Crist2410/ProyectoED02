using API_Proyecto.Interfaces;
using API_Proyecto.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Proyecto.Servicios
{
    public class ServicioMensajes
    {
        private readonly IMongoCollection<Mensajes> _Mensajes;
        public ServicioMensajes(IProyectoDataBaseSettings Interface)
        {
            var Cliente = new MongoClient(Interface.ConnectionString);
            var DataBase = Cliente.GetDatabase(Interface.DatabaseName);
            Interface.ProyectoCollectionName = "Mensajes";
            _Mensajes = DataBase.GetCollection<Mensajes>(Interface.ProyectoCollectionName);
        }
        public List<Mensajes> ObtenerTodos()
        {
            return _Mensajes.Find(Mensaje => true).ToList();
        }
        public Mensajes EnivarMensaje(Mensajes Mensaje)
        {
            _Mensajes.InsertOne(Mensaje);
            return Mensaje;
        }
        public List<Mensajes> FiltrarUsuario(string User)
        {
            return _Mensajes.Find(Mensaje => Mensaje.Emisor == User || Mensaje.Receptor == User).ToList();
        }
        public List<Mensajes> FiltrarConversacion(Mensajes value)
        {
            return _Mensajes.Find(Mensaje => Mensaje.Chat.Contains(value.Emisor + value.Receptor)).ToList();
        }
        public List<Mensajes> FitrarChat(string Filtro)
        {
            return _Mensajes.Find(Mensaje => Mensaje.Texto.Contains(Filtro)).ToList();
        }
    }
}
