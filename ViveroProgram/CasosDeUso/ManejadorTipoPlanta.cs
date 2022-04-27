using System;
using System.Collections.Generic;
using System.Text;
using Dominio.EntidadesNegocio;
using Dominio.InterfacesRepositorios;
using Repositorios;

namespace Manejadores
{
    public class ManejadorTipoPlanta : IManejadorTipoPlanta
    {
        public IRepositorioTiposPlantas RepoTP { get; set; }
        public ManejadorTipoPlanta (IRepositorioTiposPlantas repo)
        {
            RepoTP = repo;
        }

        public bool AgregarTipoPlanta(TipoPlanta tp)
        {
            return RepoTP.Add(tp);
        }

        

        public bool EliminarTipoPlanta(int id)
        {
            TipoPlanta tp = BuscarTPporId(id);
            return RepoTP.Remove(tp);
        }

        public IEnumerable<TipoPlanta> ListarTodosLosTipoPlanta()
        {
            return RepoTP.FindAll();
        }

        public bool ModificarDescDeTipoPlanta(TipoPlanta tp)
        {
            return RepoTP.Update(tp);
        }

        public TipoPlanta BuscarTPporId(int id)
        {   
            return RepoTP.FindByID(id);
        }

        public TipoPlanta BuscarTPporNombre(string nombre)
        {
            return RepoTP.BuscarTipoPlantaPorNombre(nombre);
        }
    }
}
