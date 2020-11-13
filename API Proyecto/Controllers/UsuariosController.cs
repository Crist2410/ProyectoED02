using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Proyecto.Models;
using API_Proyecto.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        // POST: api/Usuarios
        [HttpPost]
        public ActionResult<Usuarios> Post(Usuarios value)
        {
            _Usuarios.CrearUsuario(value);
            return CreatedAtRoute("ObtenerUsuario",new { Id = value.ID.ToString()},value);
        }

        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
