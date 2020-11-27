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
        public Archivos Descargar(Mensajes mensaje)
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
                        Compresor.Descomprimir(item.File, RutaDecompres);
                        FileStream ArchivoFinal = new FileStream(RutaDecompres, FileMode.OpenOrCreate);
                        Archivos archivos = new Archivos();
                        ArchivoFinal.Close();
                        archivos.Contenido = GetFile(RutaDecompres);
                        archivos.Nombre = Path.GetFileName(RutaDecompres);
                        archivos.Ruta = RutaDecompres;
                        return archivos;
                    }
                }
            }
            return default;
        }
        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            fs.Close();
            return data;
        }


        [HttpPost("subirarchivo")]
        public bool SubirArchivo(Archivos Archivo)
        {
            LZW Compresor = new LZW();
            if (Archivo != null)
            {
                string RutaOriginal = Path.GetFullPath("Archivos Originales\\" + Archivo.Nombre);
                string RutaCompress = Path.GetFullPath("Archivos Compress\\" + Archivo.Nombre.Split('.')[0]+".lzw");
                FileStream ArchivoOriginal = new FileStream(RutaOriginal, FileMode.OpenOrCreate);
                FileStream ArchivoCompress = new FileStream(RutaCompress, FileMode.OpenOrCreate);
                BinaryWriter binaryWriter = new BinaryWriter(ArchivoOriginal);
                binaryWriter.Write(Archivo.Contenido);
                binaryWriter.Close();
                ArchivoOriginal.Close();
                ArchivoCompress.Close();
                Compresor.Comprimir(RutaOriginal, RutaCompress);
                return true;
            }
            return false;
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
                    string RutaCompresion = Path.GetFullPath("Archivos Compress\\" + mensaje.FileNombre.Split('.')[0] + ".lzw");
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
