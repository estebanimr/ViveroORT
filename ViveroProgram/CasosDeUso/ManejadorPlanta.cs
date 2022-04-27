using System;
using System.Collections.Generic;
using System.Text;
using Dominio.EntidadesNegocio;
using Dominio.InterfacesRepositorios;
using Repositorios;

namespace Manejadores
{
    public class ManejadorPlanta : IManejadorPlanta
    {
        public IRepositorioPlantas RepoPLANTA { get; set; }
        public IRepositorioTiposPlantas RepoTP { get; set; }
        public IRepositorioTiposIluminaciones RepoTI { get; set; }

        // public IRepositorioFOTO RepoFOTO { get; set; }

        public ManejadorPlanta(IRepositorioPlantas repoPlanta, IRepositorioTiposPlantas repoTipoPlanta, 
            IRepositorioTiposIluminaciones repoTipoIlum)
        {
            RepoTI = repoTipoIlum;
            RepoPLANTA = repoPlanta;
            RepoTP  = repoTipoPlanta;

            //RepoFOTO= iRepoFoto; falta agregarle el parametro en el constructor 
        }

       

        public IEnumerable<TipoIluminacion> ObtenerTiposIluminaciones()
        {
            return RepoTI.FindAll();
        }


        public bool DarDeAltaPlanta(Planta pla, int idIluminacion, int idTipoPlanta, string valorAltura)
        {
            TipoIluminacion TIbuscado = RepoTI.FindByID(idIluminacion);
            pla.CuidadosPlanta.TipoIluminacion = TIbuscado;

            Parametros altura = new Parametros() { ValorParametro = valorAltura };
            pla.AlturaMaxima = altura;

            TipoPlanta TipoPlantaBuscado = RepoTP.FindByID(idTipoPlanta);
            pla.Tipo = TipoPlantaBuscado;
            
            return RepoPLANTA.Add(pla);
        }

        public IEnumerable<TipoPlanta> ObtenerTiposPlantas()
        {
            return RepoTP.FindAll();
        }
      
        public IEnumerable<Planta> ObtenerTodasLasPlantas()
        {
            return RepoPLANTA.FindAll();
        }

        public string CambiarNombreFoto(string nombreCient,string nombreFoto)
        {
            string nuevoNombre = nombreCient.Replace(" ", "_");
            string ext = nombreFoto.Substring(nombreFoto.Length - 4, 4);
            nuevoNombre = nuevoNombre + "_" + "001" + ext;
            return nuevoNombre;
        }

        public Planta ObtenerPlantaPorId(int idPlanta)
        {
            return RepoPLANTA.FindByID(idPlanta);
        }

        public bool EliminarPlanta(int idPlanta)
        {

            return RepoPLANTA.Remove(RepoPLANTA.FindByID(idPlanta));
        }

        public IEnumerable<Planta> BuscarPlantasPorParteDeTexto(string texto)
        {
            return RepoPLANTA.BuscarPlantaPorTexto(texto);
        }

        public IEnumerable<Planta> ListarPlantaPorTP(int id)
        {
            TipoPlanta tp = RepoTP.FindByID(id);
            return RepoPLANTA.BuscarPlantasPorTipo(tp);
        }


        //BUSQUEDA POR ALTURAS
        public IEnumerable<Planta> BuscarPlantasPorAlturaMax(int altura)
        {
            return RepoPLANTA.BuscarPlantasPorAlturaMaxima(altura);
        }

        public IEnumerable<Planta> BuscarPlantasPorAlturaMin(int altura)
        {
            return RepoPLANTA.BuscarPlantasPorAlturaMinima(altura);
        }

        public IEnumerable<Planta> BuscarPlantasPorAmb(string ambiente)
        {
            return RepoPLANTA.BuscarPlantasPorAmbiente(ambiente);
        }
        //BUSQUEDA POR ALTURAS

    }
}
