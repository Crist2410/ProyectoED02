using API_Proyecto.Models;
using API_Proyecto.Servicios;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Proyecto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MensajesController : ControllerBase
    {
        private readonly ServicioMensajes _Mensajes;
        //Contructor
        public MensajesController(ServicioMensajes Servicio)
        {
            _Mensajes = Servicio;
        }

        // GET: api/Usuarios
        //Obtener todos
        [HttpGet]
        public ActionResult<List<Mensajes>> Get()
        {
            return _Mensajes.ObtenerTodos();
        }

        // GET: api/Usuarios/
        //Obtener un Mensajes por Usuario
        [HttpGet("{User}")]
        public ActionResult<List<Mensajes>> GetbyUser(string User)
        {
            var Lista = _Mensajes.FiltrarUsuario(User);
            if (Lista.Count != 0)
            {
                return Lista;
            }
            return NotFound();
        }

        //Cargar conversacion
        [HttpGet("chat/{value}", Name = "ObtenerConversacion")]
        public ActionResult<List<Mensajes>> GetConversation(Mensajes value)
        {
            var Lista = _Mensajes.FiltrarConversacion(value);
            if (Lista.Count != 0)
            {
                return Lista;
            }
            return NotFound();
        }

        // GET: api/Usuarios/
        //Obtener un Mensajes por Usuario
        [HttpGet("{Word}")]
        public ActionResult<List<Mensajes>> GetbyWord(string Word)
        {
            var Lista = _Mensajes.FitrarChat(Word);
            if (Lista.Count != 0)
            {
                return Lista;
            }
            return NotFound();
        }

        //Enviar Mensaje
        // POST: api/Usuarios
        [HttpPost]
        public ActionResult<List<Mensajes>> Send(Mensajes mensaje)
        {
            _Mensajes.EnivarMensaje(mensaje);
            var Lista = _Mensajes.FiltrarConversacion(mensaje);
            if (Lista.Count != 0)
            {
                return Lista;
            }
            return NotFound();
        }
    }
}
