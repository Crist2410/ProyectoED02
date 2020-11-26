using API_Proyecto.Interfaces;
using API_Proyecto.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Proyecto.Servicios
{
    public class ServicioUsuarios
    {
        private readonly IMongoCollection<Usuarios> _Usuarios;
        public ServicioUsuarios(IProyectoDataBaseSettings Interface)
        {
            var Cliente = new MongoClient(Interface.ConnectionString);
            var DataBase = Cliente.GetDatabase(Interface.DatabaseName);
            _Usuarios = DataBase.GetCollection<Usuarios>(Interface.ProyectoCollectionName);
        }
        public List<Usuarios> ObtenerTodos()
        {
            return _Usuarios.Find(Usuario => true).ToList();
        }
        public Usuarios ObtenerUsuario(Usuarios User)
        {
            string Pass = User.PassWord;
            User =  _Usuarios.Find(Usuario => Usuario.User == User.User).FirstOrDefault();
            if (User != null && User.PassWord == Pass)
            {
                return User;
            }
            return default;
        }
        public Usuarios ObtenerPorUser(string value)
        {
            Usuarios User = _Usuarios.Find(Usuario => Usuario.User == value).FirstOrDefault();
            if (User != null)
            {
                return User;
            }
            return default;
        }
        public Usuarios ObtenerID(string Id)
        {
            return _Usuarios.Find(Usuario => Usuario.ID == Id).FirstOrDefault();
        }
        public Usuarios CrearUsuario(Usuarios Usuario)
        {
            _Usuarios.InsertOne(Usuario);
            return Usuario;
        }
        public void Eliminar(string Id)
        {
            _Usuarios.DeleteOne(Usuario => Usuario.ID == Id);
        }
        public Usuarios Modificar(string Id, Usuarios User)
        {
            _Usuarios.ReplaceOne(Usuario => Usuario.ID == Id, User);
            return User;
        }

    }
}
