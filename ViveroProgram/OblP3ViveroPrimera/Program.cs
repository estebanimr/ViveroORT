using System;
using System.Collections.Generic;
using Dominio.EntidadesNegocio;
using Repositorios;
using Dominio.InterfacesRepositorios;
using System.IO;

namespace OblP3ViveroPrimera
{
    class Program
    {

        static void Main(string[] args)
        {
            // =================================== TIPO PLANTA ==================================================
            #region TIPOPLANTA 
            IRepositorioTiposPlantas repoTipoPlantas = new RepositoriosTiposPlantasADO();
            ////Constructor de una planta
            //TipoPlanta nuevoTipoPlanta = new TipoPlanta()
            //{
            //    NomTipoPlanta = "Repollo",
            //    DescTipoPlanta = "Repollo de Campo"
            //};

            //========
            //  ADD FUNCIONA
            //========
            //bool ok = repoTipoPlantas.Add(nuevoTipoPlanta);
            //// Mostramos resultado del add
            //Console.WriteLine(ok);


            //========
            //  FIND ALL FUNCIONA
            //========
            //IEnumerable<TipoPlanta> listaTP = repoTipoPlantas.FindAll();
            //foreach (TipoPlanta tp in listaTP)
            //{
            //    Console.WriteLine(tp.ToString());
            //}


            //========
            //  FIND BY ID FUNCIONA
            //========
            //TipoPlanta tpl = repoTipoPlantas.FindByID(2004);
            //Console.WriteLine(tpl != null ? tpl.ToString() : "No se encontro el tipo planta");


            //========
            //  REMOVE FUNCIONA
            //========
            //bool ok = repoTipoPlantas.Remove(repoTipoPlantas.FindByID(2005));
            //Console.WriteLine(ok);

            //========
            // UPDATE FUNCIONA
            //========
            //TipoPlanta plantaUpdate = repoTipoPlantas.FindByID(2);
            //plantaUpdate.NomTipoPlanta = "Sandias";
            //plantaUpdate.DescTipoPlanta = "Roja con muchas semillas";
            //bool ok = repoTipoPlantas.Update(plantaUpdate);
            //Console.WriteLine(ok);


            //========
            // BuscarTipoPlantaPorNombre FUNCIONA
            //========
            //Console.WriteLine(repoTipoPlantas.BuscarTipoPlantaPorNombre("Sandia").ToString());

            #endregion



            // ==================================== TIPO ILUMINACION  ===========================================
            #region TIPO ILUMINACION
            IRepositorioTiposIluminaciones repoTipoIlum = new RepositoriosTiposIluminacionesADO();

            //Constructor de un Tipo de Iluminacion
            //TipoIluminacion nuevoTipoIlum = new TipoIluminacion()
            //{     
            // =========== USAR UNO POR VEZ PARA AGREGAR A LA BASE DE DATOS ==========
            // NombreTipoIluminacion = "luz pichirri"
            // NombreTipoIluminacion = "luz indirecta"
            //NombreTipoIluminacion = "sombra"

            // NombreTipoIluminacion = "media luz" --->
            // no se puede ingresar aun porque seria necesario
            // modificar la validacion que corrobora que el nombre que se
            // quiere ingresar, pertezca al grupo de nombres de tipo iluminacion
            //};

            //========
            //  ADD FUNCIONA
            //========
            //bool ok = repoTipoIlum.Add(nuevoTipoIlum);
            //Console.WriteLine(ok); // Mostramos resultado del add

            //========
            //  FIND ALL FUNCIONA
            //========
            //IEnumerable<TipoIluminacion> listaTI = repoTipoIlum.FindAll();
            //foreach (TipoIluminacion ti in listaTI)
            //{
            //    Console.WriteLine(ti.ToString());
            //}

            //========
            //  FIND BY ID FUNCIONA
            //========
            //TipoIluminacion ti = repoTipoIlum.FindByID(2);
            //Console.WriteLine(ti != null ? ti.ToString() : "No se encontro el tipo planta");


            //========
            //  REMOVE  FUNCIONA
            //========
            //bool ok = repoTipoIlum.Remove(repoTipoIlum.FindByID(2));
            //Console.WriteLine(ok);


            //========
            // UPDATE FUNCIONA
            //========
            //TipoIluminacion tiUpdate = repoTipoIlum.FindByID(5);
            //tiUpdate.NombreTipoIluminacion = "luz directa";

            //bool ok = repoTipoIlum.Update(tiUpdate);
            //Console.WriteLine(ok);

            #endregion


            // ==================================== CUIDADOS PLANTA  ============================================
            #region CUIDADOS PLANTA
            IRepositorioCuidadosPlantas repoCuidadosPlantas = new RepositoriosCuidadosPlantasADO();
            //Constructor de un Cuidado de Planta

            //TipoIluminacion nuevoTI = new TipoIluminacion()
            //{
            //    IdTipoIluminacion = 2,
            //    NombreTipoIluminacion = ""
            //};

            //CuidadosPlanta nuevoCuidadoPlanta = new CuidadosPlanta()
            //{
            //    TipoIluminacion = nuevoTI,
            //    CantidadFrecRiego = 3,
            //    UnidadFrecRiego = "dias",
            //    Temperatura = 25
            //};

            ////========
            ////  ADD FUNCIONA
            ////========

            //bool ok = repoCuidadosPlantas.Add(nuevoCuidadoPlanta);
            //Console.WriteLine(ok); // Mostramos resultado del add

            //========
            //  FIND ALL FUNCIONA
            //========

            //IEnumerable<CuidadosPlanta> listaCP = repoCuidadosPlantas.FindAll();
            //foreach (CuidadosPlanta cp in listaCP)
            //{
            //    Console.WriteLine(cp.ToString());
            //}

            //========
            //  FIND BY ID ==  FUNCIONA, CONECTA BIEN CON BD
            //========

            // CuidadosPlanta cp = repoCuidadosPlantas.FindByID(3);
            // Console.WriteLine(cp != null ? cp.ToString() : "No se encontro el Cuidado Planta");

            //========
            //  REMOVE  FUNCIONA
            //========
            // CuidadosPlanta cuidadoBuscado = repoCuidadosPlantas.FindByID(6);
            //bool ok = repoCuidadosPlantas.Remove(cuidadoBuscado);
            //Console.WriteLine(ok);

            //#endregion


            // ====================================== PARAMETROS =====================================================
            //#region PARAMETROS 
            IRepositorioParametros repoParam = new RepositoriosParametrosADO();


            //========
            //  BuscarParametroPorDescripcion FUNCIONA CONECTA BIEN CON BD
            //========
            //Parametros buscado = repoParam.BuscarParametroPorDescripcion("Largo maximo para descripcion de tipo planta");
            //Console.WriteLine(buscado.ToString());

            //========
            //  ADD FUNCIONA, CONECTA BIEN CON BD
            //========
            //Parametros nuevoParametro = new Parametros
            //{
            //    TipoParametro = "Impuesto",
            //    DescParametro = "Arancel Importacion",
            //    ValorParametro = "40"
            //};
            //bool ok = repoParam.Add(nuevoParametro);
            //Console.WriteLine(ok);


            //========
            //  FindAll FUNCIONA, CONECTA BIEN CON BD
            //========
            //IEnumerable<Parametros> listParametrros = repoParam.FindAll();
            //foreach (Parametros par in listParametrros)
            //{
            //    Console.WriteLine(par.ToString());
            //}



            //========
            //  FindById  FUNCIONA, CONECTA BIEN CON BD
            //========

            //Console.WriteLine(repoParam.FindByID(8).ToString());



            //========
            //  Remove  FUNCIONA, CONECTA BIEN CON BD
            //========

            //Console.WriteLine(repoParam.Remove(repoParam.FindByID(8)));


            //========
            //  Update  
            //========
            //Parametros parUp = repoParam.FindByID(7);
            //parUp.DescParametro = "Altura Robles rojos";
            //parUp.ValorParametro = "902";
            //Console.WriteLine(repoParam.Update(parUp));




            #endregion


            // ====================================  USUARIO  ============================================
            #region USUARIO
            #endregion

            // ====================================  PLANTA   ===================================================
            #region PLANTA

            IRepositorioPlantas repoPlanta = new RepositoriosPlantasADO();
            //repoPlanta.ParametrosValidacionLargoDesc();

            //========
            //  ADD ====>>  FUNCIONA
            //========
            //TipoPlanta tipoPlanta1 = repoTipoPlantas.FindByID(2);
            //Parametros nuevaAlturaEnMemoria = new Parametros()
            //{
            //    ValorParametro = "750"
            //};

            //CuidadosPlanta nuevoCuidadosPlantaEnMemoria = new CuidadosPlanta
            //{

            //    TipoIluminacion = repoTipoIlum.FindByID(1),
            //    CantidadFrecRiego = 3,
            //    UnidadFrecRiego = "Dias",
            //    Temperatura = 22
            //};

            //List<Foto> ListaDeFotos = new List<Foto>();
            ////seleccion de fotos que hizo el usuario en memoria
            //Foto foto1 = new Foto() { NombreFoto = "foto1.jpg" };
            //Foto foto2 = new Foto() { NombreFoto = "foto2.png" };
            ////Foto foto3 = new Foto() { NombreFoto = "foto3.txt" };
            ////Foto foto4 = new Foto() { NombreFoto = "foto4.png" };
            //ListaDeFotos.Add(foto1);
            //ListaDeFotos.Add(foto2);
            ////ListaDeFotos.Add(foto3);
            ////ListaDeFotos.Add(foto4);
            ////ValidarListaDeFotos(ListaDeFotos);


            //Planta nuevaPlanta = new Planta()
            //{
            //    NombreCientifico = "SandiaNa",
            //    DescripcionPlanta = "Sandia japonesa",
            //    NombresVulgares = "iaNa",
            //    FotosDePlanta = ListaDeFotos,
            //    TipoAmbiente = Planta.Ambiente.interior,
            //    Tipo = tipoPlanta1,
            //    AlturaMaxima = nuevaAlturaEnMemoria,
            //    CuidadosPlanta = nuevoCuidadosPlantaEnMemoria
            //};
            //bool ok = repoPlanta.Add(nuevaPlanta);
            //Console.WriteLine(ok);


            //========
            //  FIND ALL ====>> FUNCIONA, CONECTA BIEN CON BD
            //========
            //IEnumerable<Planta> ListaPlanta = repoPlanta.FindAll();
            //foreach (Planta p in ListaPlanta)
            //{
            //    Console.WriteLine(p.ToString());
            //}


            //========
            //  FIND BY ID ===>>  FUNCIONA, CONECTA BIEN CON BD
            //========
            //Planta p = repoPlanta.FindByID(25);
            //Console.WriteLine(p.ToString());



            //========
            //  REMOVE ====>> FUNCIONA, Elimina Planta, TP, Params, CP y Foto de la DataBase
            //========
            //TipoPlanta tipoPlanta1 = repoTipoPlantas.FindByID(2);
            //Parametros altura1 = repoParam.FindByID(12);
            //CuidadosPlanta cuidado1 = repoCuidadosPlantas.FindByID(3);


            //Planta nuevaPlanta = new Planta()
            //{
            //    NombreCientifico = "Palmera conica",
            //    DescripcionPlanta = "Palmera conica africana",
            //    NombresVulgares = "conicus, la conica",
            //    TipoAmbiente = Planta.Ambiente.exterior,
            //    Tipo = tipoPlanta1,
            //    AlturaMaxima = altura1,
            //    CuidadosPlanta = cuidado1
            //};
            //Planta plantaABorrar = repoPlanta.FindByID(24);
            //bool ok = repoPlanta.Remove(plantaABorrar);
            //Console.WriteLine(ok);


            //========
            //  UPDATE ====>>
            //========


            //========
            //  BuscarPlantaPorNombre ====>> FUNCIONA
            //========

            //bool buscada = repoPlanta.BuscarPlantaPorNombre("Zanahorius");
            //Console.WriteLine(buscada);

            //========
            //  BuscarPlantaPorTEXTO(valida true o false) ====>> FUNCIONA
            //========
            //IEnumerable<Planta> ListaPlanta = repoPlanta.BuscarPlantaPorTexto("sandi");
            //foreach (Planta p in ListaPlanta)
            //{
            //    Console.WriteLine(p.ToString());
            //}


            //========
            //  BuscarPlantaPorNombreCientificoDevuelvePlanta ====>> FUNCIONA // es validacion
            //========
            //Planta x = repoPlanta.BuscarPlantaPorNombreCientificoDevuelvePlanta("Palmera conica");
            //Console.WriteLine(x.ToString());



            //========
            // BuscarPlantasPorAlturaMinima ====>> FUNCIONA
            //========
            //IEnumerable<Planta> ltPlantas = repoPlanta.BuscarPlantasPorAlturaMinima(650);
            //foreach (Planta p in ltPlantas)
            //{
            //    Console.WriteLine(p.ToString());
            //}


            //========
            // BuscarPlantasPorAlturaMaxima ====>> FUNCIONA
            //========
            //IEnumerable<Planta> ltPlantas = repoPlanta.BuscarPlantasPorAlturaMaxima(650);
            //foreach (Planta p in ltPlantas)
            //{
            //    Console.WriteLine(p.ToString());
            //}

            //========
            //  BuscarPlantaPorTEXTO(valida true o false) ====>> FUNCIONA
            //========
            //TipoPlanta tipoN = repoTipoPlantas.FindByID(5);
            //IEnumerable<Planta> plantasBuscadasPorTipo = repoPlanta.BuscarPlantasPorTipo(tipoN);
            //foreach (Planta item in plantasBuscadasPorTipo)
            //{
            //    Console.WriteLine(item.ToString());
            //}

            //========
            //  BuscarPlantasPorAmbiente ====>> FUNCIONA
            //========
            //IEnumerable<Planta> plantasPorAmbiente = repoPlanta.BuscarPlantasPorAmbiente("interior");
            //foreach (Planta item in plantasPorAmbiente)
            //{
            //    Console.WriteLine(item.ToString());
            //}

            #endregion



            // ====================================  PRUEBAS DE CONSOLA  =========================================


            //PRUEBA EXTENSION
            //string foto = "archivo.jpg";
            //string obtenerExtension = foto.Substring(foto.Length - 4, 4);
            //Console.WriteLine("The last character of the string is: " + obtenerExtension);


            //PRUEBA REEMPLAZAR ESPACIO POR GUION
            //string source = "archivo.jpg";

            //// Replace one substring with another with String.Replace.
            //// Only exact matches are supported.
            //var replacement = source.Replace(" ", "_");
            //Console.WriteLine($"The source string is <{source}>");
            //Console.WriteLine($"The updated string is <{replacement}>");


            // PRUEBA DE AGREGAR SECUENCIADOR AL STRING DE FOTO


            //int idFoto = 1;
            //string strSec = Convert.ToString(idFoto); // convierto el id de la foto en string
            //string nuevoStr = "_"; // defino nuevo str con un guion bajo

            //for (int i = 0; i <= 3; i++) // recorro max 3
            //{
            //    if (strSec.Length < 3 - i) // a la secuencia la recorro y agrego un cero si cumple condicion
            //    {
            //        nuevoStr += "0";
            //    }
            //}

            //string nuevoStrConId = nuevoStr + strSec; // concateno el nuevo string con ceros e idSecuencial
            //Console.WriteLine(nuevoStrConId);

            //String original = "aaabbb.jpg";
            //Console.WriteLine("The original string: '{0}'", original);
            //String modified = original.Insert(original.Length - 4, nuevoStrConId); // los coloco en las ultimas 4 posiciones
            //Console.WriteLine("The modified string: '{0}'", modified);






        }




    }
}
