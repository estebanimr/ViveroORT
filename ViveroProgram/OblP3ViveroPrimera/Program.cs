using System;
using System.Collections.Generic;
using Dominio.EntidadesNegocio;
using Dominio.OtrasInterfaces;
using Dominio.InterfacesRepositorios;
using Microsoft.Data.SqlClient;
using Repositorios;

namespace OblP3ViveroPrimera
{
    class Program
    {
        static void Main(string[] args)
        {
            // Comienza Region TIPO PLANTA =========================================================================
            #region TipoPlanta  
            IRepositorioTiposPlantas repoTipoPlantas = new RepositoriosTiposPlantasADO();
            //Constructor de una planta
            TipoPlanta nuevoTipoPlanta = new TipoPlanta()
            {
                NomTipoPlanta = "Ombu",
                DescTipoPlanta = "Uruguayo del monte de ombues"
            };

            bool ok;
            //========
            //  ADD FUNCIONA
            //========
            // ok = repoTipoPlantas.Add(nuevoTipoPlanta);
            // Mostramos resultado del add
            // Console.WriteLine(ok);


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
            //TipoPlanta tpl = repoTipoPlantas.FindByID(1);
            //Console.WriteLine(tpl != null ? tpl.ToString() : "No se encontro el tipo planta");


            //========
            //  REMOVE FUNCIONA
            //========
            //ok = repoTipoPlantas.Remove(repoTipoPlantas.FindByID(1));
            //Console.WriteLine(ok);

            //========
            // UPDATE FUNCIONA
            //========
            TipoPlanta plantaUpdate = repoTipoPlantas.FindByID(2);
            plantaUpdate.NomTipoPlanta = "Tuberculo";
            plantaUpdate.DescTipoPlanta = "Se le ocurrio a guille";
            ok = repoTipoPlantas.Update(plantaUpdate);
            Console.WriteLine(ok);



            #endregion // Termina Region TIPO PLANTA =================================================================









            //#region TipoIluminacion
            //TipoIluminacion nuevaIluminacion0 = new TipoIluminacion()
            //{
            //    NombreTipoIluminacion = "Sombra"
            //};
            //TipoIluminacion nuevaIluminacion1 = new TipoIluminacion()
            //{
            //    NombreTipoIluminacion = "Luz solar indirecta"
            //};
            //TipoIluminacion nuevaIluminacion2 = new TipoIluminacion()
            //{
            //    NombreTipoIluminacion = "Luz solar directa"
            //};
            //#endregion


        }

        private static void ListarTiposPlantas(RepositoriosTiposPlantasMemoria repoPlantas)
        {

            IEnumerable<TipoPlanta> TiposPlantas = repoPlantas.FindAll();
            if (TiposPlantas == null)
            {
                Console.WriteLine("NO hay tipos de plantas");
            }
            else
            {
                foreach (TipoPlanta p in TiposPlantas)
                {
                    Console.WriteLine(p.ToString());
                }
            }
            

        }
    }
}
