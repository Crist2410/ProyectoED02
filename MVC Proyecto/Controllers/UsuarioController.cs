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
        static Usuario UserChat = new Usuario();

        // GET: Usuario
        public ActionResult Index()
        {
            IEnumerable<Usuario> ListaUsuarios;
            HttpResponseMessage response = VariablesGlobales.WebApiClient.GetAsync("Usuarios").Result;
            ListaUsuarios = response.Content.ReadAsAsync<IEnumerable<Usuario>>().Result;
            foreach (var item in ListaUsuarios)
            {
                if (!ListUsuarios.Exists(x => x.User == item.User))
                    ListUsuarios.Add(item);
            }
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
            Usuario User = new Usuario();
            User.User = collection["User"];
            User.PassWord = collection["PassWord"];
            UserActivo = ListUsuarios.Find(x => x.User == User.User);
            ViewData["NombreUser"] = UserActivo.User;
            ViewBag.Contactos = UserActivo.Contactos;
            if (User.PassWord == UserActivo.PassWord)
                return View("MenuPrincipal", UserActivo);
            else
                return View("IniciarSesion");
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
            if (ListUsuarios.Exists(x => x.User == Texto))
            {
                Usuario User = new Usuario();
                User.User = Texto;
                ViewData["NombreUser"] = UserActivo.User;
                UserActivo.Contactos.Add(ListUsuarios.Find(x => x.User == User.User));
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
            if (ListUsuarios.Exists(x => x.User == UChat))
            {
                UserChat = ListUsuarios.Find(x => x.User == UChat);
                IEnumerable<Mensaje> Lista;
                HttpResponseMessage response = VariablesGlobales.WebApiClient.GetAsync("Mensajes").Result;
                Lista = response.Content.ReadAsAsync<IEnumerable<Mensaje>>().Result;
                List<Mensaje> Chats = new List<Mensaje>();
                foreach (var Item in Lista)
                {
                    if (!ListMesajes.Exists(x => x == Item))
                    {
                        ListMesajes.Add(Item);
                    }
                    if (Item.Chat.Contains(UserChat.User + UserActivo.User))
                    {
                        Chats.Add(Item);
                    }
                }
                Chats.Sort((x, y) => x.Fecha.CompareTo(y.Fecha));
                ViewBag.Chats = Chats;
                ViewData["NombreUser"] = UserActivo.User;
                ViewBag.Contactos = UserActivo.Contactos;
                ViewBag.AgregarContacto = false;
                ViewBag.MostarChat = true;
                return View("MenuPrincipal", UserActivo);
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
            NuevoMensaje.Texto = Texto;
            NuevoMensaje.Emisor = UserActivo.User;
            NuevoMensaje.Receptor = UserChat.User;
            NuevoMensaje.Fecha = DateTime.UtcNow.Date;
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
            return View("MenuPrincipal", UserActivo);
        }

        // GET: Usuario/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Usuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Usuario/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Usuario/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Usuario/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}