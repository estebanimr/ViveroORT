using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.EntidadesNegocio;
using Manejadores;
using MVCvivero.Models;
using Microsoft.AspNetCore.Hosting;// para manejo de file system
using System.IO; //para manejo de file system


namespace MVCvivero.Controllers
{
    public class PlantaController : Controller
    {
        public IManejadorPlanta ManejadorPlanta { get; set; }
        public IWebHostEnvironment WebHostEnvironment { get; set; }

 
        public PlantaController(IManejadorPlanta iManejadorP, IWebHostEnvironment whenv)
        //el IWebHostEnvironment es para poder obtener la ruta a la carpeta root, tiene using
        {
            ManejadorPlanta = iManejadorP;
            WebHostEnvironment = whenv;
        }
        // GET: PlantaController
        public ActionResult Index()
        {

            if (HttpContext.Session.GetString("logueadoEmail") != null)
            {
                ViewModelPlanta vm = new ViewModelPlanta();
                    vm.ListaDePlantas = ManejadorPlanta.ObtenerTodasLasPlantas();
                    return View(vm.ListaDePlantas);
            }
            else
            return RedirectToAction("Index", "Home");
        }

        // GET: PlantaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PlantaController/Create
        public ActionResult Create()
        {
            if (HttpContext.Session.GetString("logueadoEmail") != null)
            {
                ViewModelPlanta vm = new ViewModelPlanta();
                vm.CuidadosPlanta = new CuidadosPlanta();
                vm.ListaTipoIluminacion = ManejadorPlanta.ObtenerTiposIluminaciones();
                vm.ListaTipoPlanta = ManejadorPlanta.ObtenerTiposPlantas();
                return View(vm);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: PlantaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ViewModelPlanta vm)
        {
            
            
            
            try
            {
                bool ok = false;

                if (vm.ambiente != "")
                {
                    if (vm.Imagenes != null)
                    {
                        List<Foto> listaDeFotos = new List<Foto>();
                      
                              Foto nuevaFoto = new Foto()
                            {
                                NombreFoto = vm.Imagenes.FileName
                            };
                            listaDeFotos.Add(nuevaFoto);
                        

                        switch (vm.ambiente)
                        {
                            case "interior":
                                vm.Planta.TipoAmbiente = Planta.Ambiente.interior;
                                break;
                            case "exterior":
                                vm.Planta.TipoAmbiente = Planta.Ambiente.exterior;
                                break;
                            case "mixto":
                                vm.Planta.TipoAmbiente = Planta.Ambiente.mixta;
                                break;
                        }
                        vm.Planta.CuidadosPlanta = vm.CuidadosPlanta;
                        vm.Planta.FotosDePlanta = listaDeFotos;
                        string valorAltura = vm.Altura.ToString();
                        ok = ManejadorPlanta.DarDeAltaPlanta(vm.Planta, vm.idTipoIluminacionSeleccionada, vm.idTipoPlantaSeleccionada, valorAltura);
                    }
                }
                if (ok)
                {
                    string rutaRaizApp = WebHostEnvironment.WebRootPath;
                    //trae la ruta del root sin importar donde este
                    rutaRaizApp = Path.Combine(rutaRaizApp, "img");
                    string nombreParaLaFoto = ManejadorPlanta.CambiarNombreFoto(vm.Planta.NombreCientifico, vm.Imagenes.FileName);
                    string rutaCompleta = Path.Combine(rutaRaizApp, nombreParaLaFoto);
                    using (FileStream stream = new FileStream(rutaCompleta, FileMode.Create))
                    //con OPEN podriamos traer a memoria
                    vm.Imagenes.CopyTo(stream);
                    

                    //agarro nombre cientifico de la planta vm.Planta.NombreCientifico
                    //creo un contador en int  = 001; le sumo 1 en cada vuelta
                    //string nombreCien = vm.Planta.NombreCientifico;
                    //int secuenciador = 001;

                    //foreach (var item in vm.Imagenes)
                    //{
                    //    string rutaCompleta = Path.Combine(rutaRaizApp, "img", item.FileName);

                    //    //imgobl.jpg
                    //    //nombrecientifco_001.jpg
                    //    //item.FileName = "hola";
                    //    //Path.Combine(item.FileName, nombreCien);
                    //}
                    //cargar la imagen con el nombre que viene y renombrarla FUERA de la carga
                    //

                    //System.IO.File.Copy (imgobl.jpg, nombrecientifco_001.jpg)

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Error = "No se pudo dar de alta la planta";

                    vm.CuidadosPlanta = new CuidadosPlanta();
                    vm.ListaTipoIluminacion = ManejadorPlanta.ObtenerTiposIluminaciones();
                    vm.ListaTipoPlanta = ManejadorPlanta.ObtenerTiposPlantas();
                    return View(vm);
                }
            }
            catch
            {
                ViewBag.Error = "No se pudo dar de alta la planta";
                vm.CuidadosPlanta = new CuidadosPlanta();
                vm.ListaTipoIluminacion = ManejadorPlanta.ObtenerTiposIluminaciones();
                vm.ListaTipoPlanta = ManejadorPlanta.ObtenerTiposPlantas();
                return View(vm);
            }
        }

        // GET: PlantaController/Edit/5
        public ActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("logueadoEmail") != null)
                return View();
            else
                return RedirectToAction("Index", "Home");
        }

        // POST: PlantaController/Edit/5
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

        // GET: PlantaController/Delete/5
        public ActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("logueadoEmail") != null)
                return View();
            else
                return RedirectToAction("Index", "Home");
        }

        // POST: PlantaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                bool ok = ManejadorPlanta.EliminarPlanta(id);
                if (ok)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Error = "No se pudo eliminar la planta";
                    return View();
                }
                
            }
            catch
            {
                ViewBag.Error = "No se pudo eliminar la planta CATCH";
                return View();
            }
        }


        // GET: BUSQUEDAS
        public ActionResult BuscarPlantaPorParteDelTextoNombre()
        {
            if (HttpContext.Session.GetString("logueadoEmail") != null)
                return View();
            else
                return RedirectToAction("Index", "Home");

        }

        // POST: Busquedas
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BuscarPlantaPorParteDelTextoNombre( string textoBuscado)
        {
            // ViewModelPlanta vm = new ViewModelPlanta();
            try
            {
               
                IEnumerable<Planta> plantaBuscada = ManejadorPlanta.BuscarPlantasPorParteDeTexto(textoBuscado);
                

                if (plantaBuscada.Count() > 0)
                {
                    ViewBag.lst = plantaBuscada.Count();
                    ViewBag.LstPlantas = plantaBuscada;                   
                }
                else
                {
                    ViewBag.Error = "Ocurrio un error, no se puden mostrar las busquedas de plantas por texto";
                }
                return View();
            }
            catch
            {
                ViewBag.Error = "Ocurrio un error, no se puden mostrar las busquedas de plantas por texto";
                return View();
            }

        }

        public ActionResult BuscarPlantaXTP()
        {
            if (HttpContext.Session.GetString("logueadoEmail") != null)
            {
                IEnumerable<TipoPlanta> lstTP = ManejadorPlanta.ObtenerTiposPlantas();
                ViewBag.LstTP = lstTP;
                ViewBag.LstPlantas = "";

                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult BuscarPlantaXTP(int tipoPlanta)
        {
            if (HttpContext.Session.GetString("logueadoEmail") != null)
            {
                try
                {
                    IEnumerable<TipoPlanta> lstTP = ManejadorPlanta.ObtenerTiposPlantas();
                    ViewBag.LstTP = lstTP;

                    IEnumerable<Planta> lstPlantas = ManejadorPlanta.ListarPlantaPorTP(tipoPlanta);
                    if (lstPlantas.Count() > 0)
                    {
                        ViewBag.P = lstPlantas.Count();
                        ViewBag.LstPlantas = lstPlantas;
                        return View();
                    }

                    ViewBag.Error = "No se han encontrado Plantas de ese tipo.";
                    return View();


                }
                catch (Exception)
                {
                    ViewBag.Error = "No se han encontrado Plantas de ese tipo.";
                    return View();
                }
            }
            else
                return RedirectToAction("Index", "Home");
         
        }





        //BUSQUEDA POR ALTURA
        public ActionResult BuscarPorAlturas()
        {
            if (HttpContext.Session.GetString("logueadoEmail") != null)
            {
                ViewBag.Buscando = "off";
                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }

        // POST: Busquedas
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BuscarPorAlturas(int altura, string tipoAltura)
        {


            try
            {

                if (altura > 0 && tipoAltura != "")
                {

                    if (tipoAltura == "alturaMaxima")
                    {
                        IEnumerable<Planta> ltaPlantasEncontradas = ManejadorPlanta.BuscarPlantasPorAlturaMax(altura);
                        if (ltaPlantasEncontradas != null)
                        {
                            ViewBag.Buscando = "ok";
                            return View(ltaPlantasEncontradas);
                        }
                        ViewBag.Error = "No se encontro una planta con esa altura maxima o mayor a ella";
                        return View();
                    }
                    if (tipoAltura == "alturaMinima")
                    {
                        IEnumerable<Planta> ltaPlantasEncontradas = ManejadorPlanta.BuscarPlantasPorAlturaMin(altura);
                        if (ltaPlantasEncontradas != null)
                        {
                            ViewBag.Buscando = "ok";
                            return View(ltaPlantasEncontradas);
                        }
                        ViewBag.Error = "No se encontro una planta con menos de esa altura";
                        return View();
                    }
                }
                ViewBag.Error = "Verifique los datos ingresados";

                return View();
            }
            catch
            {
                ViewBag.Error = "Verifique los datos ingresados";
                return View();
            }

        }




        public ActionResult CuidadosPlanta(int id)
        {
            if (HttpContext.Session.GetString("logueadoEmail") != null)
            {
                Planta plantaAMostrar = ManejadorPlanta.ObtenerPlantaPorId(id);
                return View(plantaAMostrar);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        // POST: PlantaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CuidadosPlanta(int id, IFormCollection collection)
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

        public ActionResult BuscarPlantasPorAmbiente()
        {
            if (HttpContext.Session.GetString("logueadoEmail") != null)
            {
                ViewBag.Buscando = "of";
                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }

        // POST: PlantaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BuscarPlantasPorAmbiente(string ambi)
        {



            try
            {
                if (ambi != "")
                {
                    IEnumerable<Planta> ltaPlantas = ManejadorPlanta.BuscarPlantasPorAmb(ambi);
                    if(ltaPlantas != null)
                    {
                    ViewBag.Buscando = "ok";
                    return View(ltaPlantas);
                    }

                }
                ViewBag.Error = "No extisten plantas con ese tipo de ambiente";
                ViewBag.Buscando = "of";
                return View();

            }
            catch
            {
                ViewBag.Buscando = "of";
                return View();
            }
        }



    }
}
