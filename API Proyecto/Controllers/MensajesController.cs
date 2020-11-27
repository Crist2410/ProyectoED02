using API_Proyecto.Models;
using API_Proyecto.Servicios;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Libreria;
using Microsoft.AspNetCore.Http;
using System.IO;

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
        [HttpPost("chat")]
        public ActionResult<List<Mensajes>> GetConversation(Mensajes value)
        {
            SDES Cifrado = new SDES();
            var Lista = _Mensajes.FiltrarConversacion(value);
            if (Lista.Count != 0)
            {
                foreach (var Item in Lista)
                {
                    if (value.Emisor == Item.Emisor && Item.Texto != null)
                        Item.Texto = Cifrado.descifrado(Item.Texto, Item.RandomSecret, Item.PublicKey);
                    else if (Item.Texto != null)
                        Item.Texto = Cifrado.descifrado(Item.Texto, Item.RandomSecret, Item.PublicKey);

                }
                return Lista;
            }
            return default;
        }
        //Cargar conversacion
        [HttpPost("archivo")]
        public ActionResult<bool> EnviarArchivo(Mensajes value)
        {
            try
            {
                LZW Compresor = new LZW();
                // string RutaOriginal = Path.GetFullPath("Archivos Originales\\" + file.FileName);
                //string RutaCompresion = Path.GetFullPath("Archivos Compress\\" + file.FileName.Split('.')[0] + ".lzw");
                string RutaCompresion = Path.GetFullPath("Archivos Compress\\" + "hola" + ".lzw");
                //FileStream ArchivoOriginal = new FileStream(RutaOriginal, FileMode.OpenOrCreate);
                //file.CopyTo(ArchivoOriginal);
                //ArchivoOriginal.Close();
                Compresor.Comprimir(value.File, RutaCompresion);
                //FileInfo Original = new FileInfo(RutaOriginal);
                //FileInfo Comprimido = new FileInfo(RutaCompresion);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
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

        [HttpPost("descargar")]
        public string Descargar(Mensajes mensaje)
        {
            var Lista = _Mensajes.FiltrarConversacion(mensaje);
            LZW Compresor = new LZW();
            if (Lista.Count != 0)
            {
                foreach (var item in Lista)
                {
                    if(mensaje.File == item.File)
                    {
                        string RutaDecompres = Path.GetFullPath("Archivos Decompress\\" + item.FileNombre);
                        string RutaDevulto = Path.Combine(mensaje.FileNombre, item.FileNombre);
                        Compresor.Descomprimir(item.File, RutaDecompres);
                        FileStream ArchivoFinal = new FileStream(RutaDecompres, FileMode.Open);
                        FileStream ArchivoDevuelto = new FileStream(RutaDevulto, FileMode.OpenOrCreate);
                        FileStreamResult FileFinal = new FileStreamResult(ArchivoFinal, "text/"+item.FileNombre.Split('.')[1]);
                        ArchivoFinal.CopyToAsync(ArchivoDevuelto);
                        ArchivoDevuelto.Close();
                        ArchivoFinal.Close();
                        return RutaDevulto;
                    }
                }
            }
            return default;
        }

        //Enviar Mensaje
        // POST: api/Usuarios
        [HttpPost]
        public ActionResult<List<Mensajes>> Send(Mensajes mensaje)
        {
            SDES Cifrado = new SDES();
            if (mensaje.Texto != "" && mensaje.Texto != null)
            {
                mensaje.Texto = Cifrado.cifrado(mensaje.Texto, mensaje.RandomSecret, mensaje.PublicKey);
                _Mensajes.EnivarMensaje(mensaje);
                var Lista = _Mensajes.FiltrarConversacion(mensaje);
                if (Lista.Count != 0)
                {
                    foreach (var Item in Lista)
                    {
                        if (mensaje.Emisor == Item.Emisor && Item.Texto != null)
                            Item.Texto = Cifrado.descifrado(Item.Texto, Item.RandomSecret, Item.PublicKey);
                        else if (Item.Texto != null)
                            Item.Texto = Cifrado.descifrado(Item.Texto, Item.RandomSecret, Item.PublicKey);
                    }
                    return Lista;
                }
                return default;
            }
            else
            {
                try
                {
                    LZW Compresor = new LZW();
                    string RutaOriginal = Path.GetFullPath("Archivos Compress\\" + mensaje.FileNombre);
                    FileStream Original = new FileStream(RutaOriginal, FileMode.OpenOrCreate);
                    FileStream Base = new FileStream(mensaje.File, FileMode.OpenOrCreate);
                    Base.CopyToAsync(Original);
                    Original.Close();
                    string RutaCompresion = Path.GetFullPath("Archivos Compress\\" + mensaje.FileNombre.Split('.')[0] + ".lzw");
                    Compresor.Comprimir(RutaOriginal, RutaCompresion);
                    mensaje.File = RutaCompresion;
                    _Mensajes.EnivarMensaje(mensaje);
                    var Lista = _Mensajes.FiltrarConversacion(mensaje);
                    if (Lista.Count != 0)
                    {
                        foreach (var Item in Lista)
                        {
                            if (mensaje.Emisor == Item.Emisor && Item.Texto != null)
                                Item.Texto = Cifrado.descifrado(Item.Texto, Item.RandomSecret, Item.PublicKey);
                            else if (Item.Texto != null)
                                Item.Texto = Cifrado.descifrado(Item.Texto, Item.RandomSecret, Item.PublicKey);
                        }
                        return Lista;
                    }
                    return default;
                }
                catch (Exception)
                {
                    return default;
                }
            }
        }
    }
}
