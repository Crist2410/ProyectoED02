using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API_Proyecto.Models;
using API_Proyecto.Servicios;

namespace API_Proyecto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactosController : ControllerBase
    {
        private readonly ServicioContactos _Contactos;
        //Contructor
        public ContactosController(ServicioContactos Servicio)
        {
            _Contactos = Servicio;
        }

        // GET: api/Usuarios
        //Obtener todos
        [HttpGet]
        public ActionResult<List<Contactos>> Get()
        {
            return _Contactos.ObtenerTodos();
        }

        // GET: api/Usuarios/
        //Obtener un Mensajes por Usuario
        [HttpGet("{User}")]
        public ActionResult<List<Contactos>> GetbyUser(string User)
        {
            var Lista = _Contactos.ObtenerContactos(User);
            if (Lista.Count != 0)
            {
                return Lista;
            }
            return NotFound();
        }

        //Enviar Mensaje
        // POST: api/Usuarios
        [HttpPost]
        public ActionResult<List<Contactos>> AgegrarContacto(Contactos Contacto)
        {
            _Contactos.AgregarContacto(Contacto);
            var Lista = _Contactos.ObtenerContactos(Contacto.Usuario);
            if (Lista.Count != 0)
            {
                return Lista;
            }
            return NotFound();
        }
    }
}
