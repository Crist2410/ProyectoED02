using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Proyecto.Models;
using API_Proyecto.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Libreria;

namespace API_Proyecto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ServicioUsuarios _Usuarios;
        public UsuariosController(ServicioUsuarios Servicio)
        {
            _Usuarios = Servicio;
        }
        // GET: api/Usuarios
        [HttpGet]
        public ActionResult<List<Usuarios>> Get()
        {
            return _Usuarios.ObtenerTodos();
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}", Name = "ObtenerUsuario")]
        public ActionResult<Usuarios> Get(string id)
        {
            var Usuario = _Usuarios.ObtenerID(id);
            if (Usuario !=  null)
            {
                return Usuario;
            }
            return NotFound();
        }
        // GET: api/Usuarios/5
        [HttpPost("user")]
        public ActionResult<Usuarios> ObtenerUser(Usuarios user)
        {
            CifradoCesar Cesar = new CifradoCesar();
            user.PassWord = Cesar.Cifrar(user.PassWord);
            var Usuario = _Usuarios.ObtenerUsuario(user);
            if (Usuario != null)
            {
                Usuario.PassWord = Cesar.Decifrar(Usuario.PassWord);
                return Usuario;
            }
            return default;
        }

        [HttpPost("busqueda")]
        public ActionResult<Usuarios> ObtenerporUser(Usuarios user)
        {
            var Usuario = _Usuarios.ObtenerPorUser(user.User);
            if (Usuario != null)
            {
                return Usuario;
            }
            return default;
        }

        // POST: api/Usuarios
        [HttpPost]
        public ActionResult<Usuarios> Post(Usuarios value)
        {
            DiffieHellman MayonesaHellmans = new DiffieHellman();
            CifradoCesar Cesar = new CifradoCesar();
            value.PassWord = Cesar.Cifrar(value.PassWord);
            value.RandomSecret = _Usuarios.ObtenerTodos().Count + 3; ;
            value.PublicKey = MayonesaHellmans.GeneracionPublicKey(value.RandomSecret);
            Usuarios AuxUser = _Usuarios.CrearUsuario(value);
            if (AuxUser != null)
                return CreatedAtRoute("ObtenerUsuario", new { Id = value.ID.ToString() }, value);
            else
                return default;
        }

    }
}
