using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC_Proyecto.Models;
using MVC_Proyecto.Clases;
using System.Net.Http;

namespace MVC_Proyecto.Controllers
{
    public class UsuarioController : Controller
    {
        static List<Usuario> Lista = new List<Usuario>();
        static Usuario UserActivo = new Usuario();
        
        // GET: Usuario
        public ActionResult Index()
        {
            IEnumerable<Usuario> ListaUsuarios;
            HttpResponseMessage response = VariablesGlobales.WebApiClient.GetAsync("Usuarios").Result;
            ListaUsuarios = response.Content.ReadAsAsync<IEnumerable<Usuario>>().Result;
            foreach (var item in ListaUsuarios)
            {
                Lista.Add(item);
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
        public ActionResult GuardarUsuarioAsync (IFormCollection collection)
        {
            Usuario User = new Usuario();
            User.Nombre = collection["Nombre"];
            User.Apellido = collection["Apellido"];
            User.User = collection["User"];
            User.PassWord = collection["PassWord"];
            User.Genero = collection["Genero"];
            User.Edad = Convert.ToInt32(collection["Edad"]);
            Lista.Add(User);
            HttpResponseMessage response = VariablesGlobales.WebApiClient.PostAsJsonAsync("Usuarios", User).Result;
            return View("Index"); 
        }
        public ActionResult Ingresar (IFormCollection collection)
        {
            Usuario User = new Usuario();
            User.User = collection["User"];
            User.PassWord = collection["PassWord"];
            UserActivo = Lista.Find(x => x.User == User.User);
            ViewData["NombreUser"] = UserActivo.User;
            ViewBag.Contactos = UserActivo.Contactos;
            if (User.PassWord == UserActivo.PassWord)
                return View("MenuPrincipal", UserActivo);
            else
                return View("IniciarSesion");
        }

        // GET: Usuario/Details/5
        public ActionResult Details(int id)
        {
            return View();
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