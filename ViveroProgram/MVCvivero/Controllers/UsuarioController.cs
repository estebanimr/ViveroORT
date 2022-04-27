using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manejadores;
using Dominio.EntidadesNegocio;
namespace MVCvivero.Controllers
{
    public class UsuarioController : Controller
    {
        public IManejadorUsuario ManejadorUsu { get; set; }

        public UsuarioController(IManejadorUsuario manejadorU)
        {
            ManejadorUsu = manejadorU;
        }
        // GET: UsuarioController
        public ActionResult Index()
        {
            if (HttpContext.Session.GetString("logueadoEmail") != null)
                return View();
            else
                return RedirectToAction("Index", "Home");
        }

        // GET: UsuarioController/Details/5
        public ActionResult Details(int id)
        {
            if (HttpContext.Session.GetString("logueadoEmail") != null)
                return View();
            else
                return RedirectToAction("Index", "Home");
        }

        // GET: UsuarioController/Create
        public ActionResult Create()
        {
            if (HttpContext.Session.GetString("logueadoEmail") != null)
            {
                Usuario u = new Usuario();
                return View(u);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        // POST: UsuarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Usuario u)
        {
            try
            {

                if (ManejadorUsu.AltaUsuario(u))
                {

                    HttpContext.Session.SetString("logueadoEmail", u.Email);

                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    ViewBag.Mensaje = "No se pudo dar de alta el usuario";
                    return View(u);
                }

            }
            catch
            {
                ViewBag.Mensaje = "No se pudo dar de alta el usuario";
                return View();
            }
        }

        public ActionResult Login()
        {
         
                Usuario u = new Usuario();
                return View(u);
           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Usuario u)
        {
            try
            {
                Usuario usu = ManejadorUsu.Login(u.Email, u.Pass);
                if (usu != null)
                {
                    HttpContext.Session.SetString("logueadoEmail", u.Email);

                    return RedirectToAction("Index", "Planta");
                }
                else
                {
                    ViewBag.Mensaje = "No se pudo loguear el Usuario";
                    return View(u);
                }
            }
            catch (Exception)
            {
                ViewBag.Mensaje = "No se pudo loguear el Usuario";
                return View();
            }

        }

        public ActionResult Logout()
        {
            if (HttpContext.Session.GetString("logueadoEmail") != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout(string n)
        {
           

            HttpContext.Session.Clear();
            return RedirectToAction("Login","Usuario");
        }

        // GET: UsuarioController/Edit/5
        public ActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("logueadoEmail") != null)
                return View();
            return RedirectToAction("Index", "Home");

        }

        // POST: UsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsuarioController/Delete/5
        public ActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("logueadoEmail") != null)
                return View();
            else
                return RedirectToAction("Index", "Home");
        }

        // POST: UsuarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
