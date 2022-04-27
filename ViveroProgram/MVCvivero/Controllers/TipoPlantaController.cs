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
    public class TipoPlantaController : Controller
    {
        
        IManejadorTipoPlanta ManejadorTP { get;   set; }

        public TipoPlantaController(IManejadorTipoPlanta manejadorTp) {
            ManejadorTP = manejadorTp;
        }
        // GET: TipoPlantaController
        public ActionResult Index()
        {
            if (HttpContext.Session.GetString("logueadoEmail") != null)
            {
                IEnumerable<TipoPlanta> ltaTipoPlantas = ManejadorTP.ListarTodosLosTipoPlanta();
                return View(ltaTipoPlantas);
            }
            else
                return RedirectToAction("Index", "Home");
        }
       

        // GET: TipoPlantaController/Details/5
        public ActionResult Details(int id)
        {
            if (HttpContext.Session.GetString("logueadoEmail") != null)
                return View();
            else
                return RedirectToAction("Index", "Home");
        }

        // GET: TipoPlantaController/Create
        public ActionResult Create()
        {
            if (HttpContext.Session.GetString("logueadoEmail") != null)
            {
                TipoPlanta nuevaTP = new TipoPlanta();
                return View(nuevaTP);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        // POST: TipoPlantaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TipoPlanta nuevaTP)
        {
            try
            {
                bool ok = ManejadorTP.AgregarTipoPlanta(nuevaTP);
                if (ok)
                {
                return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Error = "No se pudo dar de alta el tipo de planta";
                    return View(nuevaTP);
                }
            }
            catch
            {
                ViewBag.Error = "No se pudo dar de alta el tipo de planta";
                return View();
            }
        }

        // GET: TipoPlantaController/Edit/5
        public ActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("logueadoEmail") != null)
            {
                TipoPlanta TP = ManejadorTP.BuscarTPporId(id);
                return View(TP);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        // POST: TipoPlantaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TipoPlanta TP)
        {
            try
            {
                bool ok = ManejadorTP.ModificarDescDeTipoPlanta(TP);
                if (ok)
                {

                return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Error = "NO SE PUDO ACTUALIZAR EL TIPO DE PLANTA";
                    return View(TP);
                }
            }
            catch
            {
                ViewBag.Error = "NO SE PUDO ACTUALIZAR EL TIPO DE PLANTA";
                return View();
            }
        }

        // GET: TipoPlantaController/Delete/5
        public ActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("logueadoEmail") != null)
            {
                TipoPlanta TP = ManejadorTP.BuscarTPporId(id);
                return View(TP);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        // POST: TipoPlantaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {               //ManejadorTP.BuscarTPporId(id)
                
                bool ok = ManejadorTP.EliminarTipoPlanta(id);
                if (ok)
                {
                    return RedirectToAction(nameof(Index));
                }
                else {
                    ViewBag.Error = "NO SE PUDO ELIMINAR EL TIPO DE PLANTA";
                    return View();
                }
            }
            catch(Exception ex)
            {
                ViewBag.Error = "NO SE PUDO ELIMINAR EL TIPO DE PLANTA";
                return View();
                throw new Exception(ex.Message);
            }
        }

        public ActionResult MantenimientoTipoPlanta()
        {
            if (HttpContext.Session.GetString("logueadoEmail") != null)
            {
                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult MantenimientoTipoPlanta(string nombre)
        {
            try
            {
                if (nombre != null || nombre != "")
                {
                    TipoPlanta buscada = ManejadorTP.BuscarTPporNombre(nombre);

                    ViewBag.TPBuscada = buscada;
                    ViewBag.Error = "El tipo de planta no existe";
                    return View();

                   
                }
                else
                {
                    ViewBag.Error("Ingrese un nombre valido");
                    return View(); 
                }

            }
            catch (Exception)
            {
                ViewBag.Error("Ingrese un nombre valido");
                return View();
            }

        }
    }
}
