using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC_Proyecto.Models;
using System.Net.Http;

namespace MVC_Proyecto.Controllers
{
    public class UsuarioController : Controller
    {
        static List<Usuario> ListUsuarios = new List<Usuario>();
        static List<Mensaje> ListMesajes = new List<Mensaje>();
        static Usuario UserActivo = new Usuario();
        string UsuarioSeleccionado;
        static Usuario UserChat = new Usuario();

        // GET: Usuario
        public ActionResult Index()
        {
            UserChat = new Usuario();
            UserActivo = new Usuario();
            return View();
        }
        public ActionResult IniciarSesion()
        {
            return View();
        }
        public ActionResult Registrarse()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GuardarUsuarioAsync(IFormCollection collection)
        {
            Usuario User = new Usuario();
            User.Nombre = collection["Nombre"];
            User.Apellido = collection["Apellido"];
            User.User = collection["User"];
            User.PassWord = collection["PassWord"];
            User.Genero = collection["Genero"];
            User.Edad = Convert.ToInt32(collection["Edad"]);
            ListUsuarios.Add(User);
            HttpResponseMessage response = VariablesGlobales.WebApiClient.PostAsJsonAsync("Usuarios", User).Result;
            return View("Index");
        }
        public ActionResult Ingresar(IFormCollection collection)
        {
            Usuario user = new Usuario();
            user.User = collection["User"];
            user.PassWord = collection["PassWord"];
            HttpResponseMessage response = VariablesGlobales.WebApiClient.PostAsJsonAsync("usuarios/user", user).Result;
            user = response.Content.ReadAsAsync<Usuario>().Result;
            if (user != null)
            {
                UserActivo = user;
                ViewData["NombreUser"] = UserActivo.User;
                HttpResponseMessage response2 = VariablesGlobales.WebApiClient.GetAsync("Contactos").Result;
                IEnumerable<Contacto> ListaContactos = response2.Content.ReadAsAsync<IEnumerable<Contacto>>().Result;
                foreach (var item in ListaContactos)
                {
                    HttpResponseMessage Contact;
                    Usuario contactouser = new Usuario();
                    if (UserActivo.User == item.Usuario && !UserActivo.Contactos.Exists(x => x.User == item.Contact))
                    {
                        contactouser.User = item.Contact;
                        Contact = VariablesGlobales.WebApiClient.PostAsJsonAsync("usuarios/busqueda", contactouser).Result;
                        UserActivo.Contactos.Add(Contact.Content.ReadAsAsync<Usuario>().Result);
                    }
                    else if (UserActivo.User == item.Contact && !UserActivo.Contactos.Exists(x => x.User == item.Usuario))
                    {
                        contactouser.User = item.Usuario;
                        Contact = VariablesGlobales.WebApiClient.PostAsJsonAsync("usuarios/busqueda", contactouser).Result;
                        UserActivo.Contactos.Add(Contact.Content.ReadAsAsync<Usuario>().Result);
                    }
                }
                ViewBag.Contactos = UserActivo.Contactos;
                return View("MenuPrincipal", UserActivo);
            }
            else
            {
                return View("IniciarSesion");
            }
        }

        public ActionResult OpcionContato()
        {
            ViewData["NombreUser"] = UserActivo.User;
            ViewBag.Contactos = UserActivo.Contactos;
            ViewBag.AgregarContacto = true;
            return View("MenuPrincipal", UserActivo);
        }
        public ActionResult AgregarContacto(string Texto)
        {
            HttpResponseMessage Contact;
            Usuario contactouser = new Usuario();
            contactouser.User = Texto;
            Contact = VariablesGlobales.WebApiClient.PostAsJsonAsync("usuarios/busqueda", contactouser).Result;
            contactouser = Contact.Content.ReadAsAsync<Usuario>().Result;

            if (contactouser != null && contactouser.User != UserActivo.User)
            {
                Usuario User = new Usuario();
                User.User = Texto;
                ViewData["NombreUser"] = UserActivo.User;
                Contacto NuevoContacto = new Contacto();
                NuevoContacto.Chat = UserActivo.User + User.User + UserActivo.User + User.User;
                NuevoContacto.Contact = User.User;
                NuevoContacto.Usuario = UserActivo.User;
                HttpResponseMessage response = VariablesGlobales.WebApiClient.PostAsJsonAsync("Contactos", NuevoContacto).Result;
                UserActivo.Contactos.Add(contactouser);
                ViewBag.Contactos = UserActivo.Contactos;
                ViewBag.AgregarContacto = false;
                return View("MenuPrincipal", UserActivo);
            }
            ViewData["NombreUser"] = UserActivo.User;
            ViewBag.Contactos = UserActivo.Contactos;
            ViewBag.AgregarContacto = true;
            return View("MenuPrincipal", UserActivo);
        }

        public ActionResult MostrarChat(string UChat)
        {
            Usuario AuxUser = new Usuario();
            AuxUser.User = UChat;
            UsuarioSeleccionado = UChat;
            HttpResponseMessage R1 = VariablesGlobales.WebApiClient.PostAsJsonAsync("usuarios/busqueda", AuxUser).Result;
            AuxUser = R1.Content.ReadAsAsync<Usuario>().Result;
            if (AuxUser != null)
            {
                UserChat = AuxUser;
                Mensaje msm = new Mensaje();
                msm.RandomSecret = UserActivo.RandomSecret;
                msm.PublicKey = UserChat.PublicKey;
                msm.PublicKeyUser = UserActivo.PublicKey;
                msm.Emisor = UserActivo.User;
                msm.Receptor = UserChat.User;
                IEnumerable<Mensaje> Lista;
                HttpResponseMessage response = VariablesGlobales.WebApiClient.PostAsJsonAsync("mensajes/chat", msm).Result;
                Lista = response.Content.ReadAsAsync<IEnumerable<Mensaje>>().Result;
                List<Mensaje> Chats = new List<Mensaje>();
                if (Lista != null)
                {
                    foreach (var Item in Lista)
                    {
                        if (Item.Chat.Contains(UserChat.User + UserActivo.User))
                        {
                            Chats.Add(Item);
                        }
                    }
                    Chats.Sort((x, y) => x.Fecha.CompareTo(y.Fecha));
                }
                ViewBag.Chats = Chats;
                ViewData["NombreUser"] = UserActivo.User;
                ViewBag.Contactos = UserActivo.Contactos;
                ViewBag.AgregarContacto = false;
                ViewBag.MostarChat = true;
                ViewBag.Enviar = true;
                ViewBag.FiltrarMensaje = true;
                return View("MenuPrincipal", UserActivo);
            }
            ViewData["NombreUser"] = UserActivo.User;
            ViewBag.Contactos = UserActivo.Contactos;
            ViewBag.AgregarContacto = false;
            ViewBag.MostarChat = false;
            return View("MenuPrincipal", UserActivo);

        }
        public ActionResult BuscarMensaje(string Filtro)
        {
            if (Filtro != null)
            {
                Usuario AuxUser = new Usuario();
                AuxUser.User = UserChat.User;
                HttpResponseMessage R1 = VariablesGlobales.WebApiClient.PostAsJsonAsync("usuarios/busqueda", AuxUser).Result;
                AuxUser = R1.Content.ReadAsAsync<Usuario>().Result;
                if (AuxUser != null)
                {
                    UserChat = AuxUser;
                    Mensaje msm = new Mensaje();
                    msm.RandomSecret = UserActivo.RandomSecret;
                    msm.PublicKey = UserChat.PublicKey;
                    msm.PublicKeyUser = UserActivo.PublicKey;
                    msm.Emisor = UserActivo.User;
                    msm.Receptor = UserChat.User;
                    IEnumerable<Mensaje> Lista;
                    HttpResponseMessage response = VariablesGlobales.WebApiClient.PostAsJsonAsync("mensajes/chat", msm).Result;
                    Lista = response.Content.ReadAsAsync<IEnumerable<Mensaje>>().Result;
                    List<Mensaje> Chats = new List<Mensaje>();
                    if (Lista != null)
                    {
                        foreach (var Item in Lista)
                        {
                            if (Item.Chat.Contains(UserChat.User + UserActivo.User) && Item.Texto.Contains(Filtro))
                            {
                                Chats.Add(Item);
                            }
                        }
                        Chats.Sort((x, y) => x.Fecha.CompareTo(y.Fecha));
                    }
                    ViewBag.Chats = Chats;
                    ViewData["NombreUser"] = UserActivo.User;
                    ViewBag.Contactos = UserActivo.Contactos;
                    ViewBag.AgregarContacto = false;
                    ViewBag.MostarChat = true;
                    ViewBag.FiltrarMensaje = true;
                    return View("MenuPrincipal", UserActivo);
                }
            }
            ViewData["NombreUser"] = UserActivo.User;
            ViewBag.Contactos = UserActivo.Contactos;
            ViewBag.AgregarContacto = false;
            ViewBag.MostarChat = false;
            return View("MenuPrincipal", UserActivo);
        }

        public ActionResult EnviarMensaje(string Texto)
        {
            Mensaje NuevoMensaje = new Mensaje();
           // NuevoMensaje.File = File;
            NuevoMensaje.Texto = Texto;
            NuevoMensaje.Emisor = UserActivo.User;
            NuevoMensaje.Receptor = UserChat.User;
            NuevoMensaje.RandomSecret = UserActivo.RandomSecret;
            NuevoMensaje.PublicKey = UserChat.PublicKey;
            NuevoMensaje.PublicKeyUser = UserActivo.PublicKey;
            NuevoMensaje.Fecha = DateTime.Now;
            NuevoMensaje.Chat = NuevoMensaje.Emisor + NuevoMensaje.Receptor + NuevoMensaje.Receptor + NuevoMensaje.Emisor;
            HttpResponseMessage response = VariablesGlobales.WebApiClient.PostAsJsonAsync("Mensajes", NuevoMensaje).Result;
            IEnumerable<Mensaje> Lista = response.Content.ReadAsAsync<IEnumerable<Mensaje>>().Result;
            List<Mensaje> Chats = new List<Mensaje>();
            foreach (var Item in Lista)
            {
                Chats.Add(Item);
            }
            Chats.Sort((x, y) => x.Fecha.CompareTo(y.Fecha));
            ViewBag.Chats = Chats;
            ViewData["NombreUser"] = UserActivo.User;
            ViewBag.Contactos = UserActivo.Contactos;
            ViewBag.MostarChat = true;
            ViewBag.Enviar = true;
            ViewBag.FiltrarMensaje = true;
            return View("MenuPrincipal", UserActivo);
        }
        public ActionResult Recargar()
        {
            Usuario AuxUser = UserChat;
            if (AuxUser != null)
            {
                UserChat = AuxUser;
                Mensaje msm = new Mensaje();
                msm.RandomSecret = UserActivo.RandomSecret;
                msm.PublicKey = UserChat.PublicKey;
                msm.PublicKeyUser = UserActivo.PublicKey;
                msm.Emisor = UserActivo.User;
                msm.Receptor = UserChat.User;
                IEnumerable<Mensaje> Lista;
                HttpResponseMessage response = VariablesGlobales.WebApiClient.PostAsJsonAsync("mensajes/chat", msm).Result;
                Lista = response.Content.ReadAsAsync<IEnumerable<Mensaje>>().Result;
                List<Mensaje> Chats = new List<Mensaje>();
                if (Lista != null)
                {
                    foreach (var Item in Lista)
                    {
                        if (Item.Chat.Contains(UserChat.User + UserActivo.User))
                        {
                            Chats.Add(Item);
                        }
                    }
                    Chats.Sort((x, y) => x.Fecha.CompareTo(y.Fecha));
                }
                ViewBag.Chats = Chats;
                ViewData["NombreUser"] = UserActivo.User;
                ViewBag.Contactos = UserActivo.Contactos;
                ViewBag.AgregarContacto = false;
                ViewBag.MostarChat = true;
                ViewBag.Enviar = true;
                ViewBag.FiltrarMensaje = true;
                return View("MenuPrincipal", UserActivo);
            }
            ViewData["NombreUser"] = UserActivo.User;
            ViewBag.Contactos = UserActivo.Contactos;
            ViewBag.AgregarContacto = false;
            ViewBag.MostarChat = false;
            return View("MenuPrincipal", UserActivo);

        }
    }
}