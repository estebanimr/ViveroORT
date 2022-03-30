using Dominio.EntidadesNegocio;
using Dominio.InterfacesRepositorios;
using System.Collections.Generic;

namespace Repositorios
{
	public class RepositoriosTiposIluminacionesMemoria : IRepositorioTiposIluminaciones
	{
		public List<TipoIluminacion> NombresTiposIluminaciones;

        public bool Add(TipoIluminacion obj)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<TipoIluminacion> FindAll()
        {
            throw new System.NotImplementedException();
        }

        public TipoIluminacion FindByID(object id)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(object id)
        {
            throw new System.NotImplementedException();
        }

        public bool Update(TipoIluminacion obj)
        {
            throw new System.NotImplementedException();
        }
    }

}

