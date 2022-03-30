using Dominio.EntidadesNegocio;
using Repositorios;
using System.Collections.Generic;

namespace Repositorios
{
	public class RepositoriosPlantasMemoria : IRepositorioPlantas
	{
		public List<Planta> Plantas;

        public bool Add(Planta obj)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Planta> FindAll()
        {
            throw new System.NotImplementedException();
        }

        public Planta FindByID(object id)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(object id)
        {
            throw new System.NotImplementedException();
        }

        public bool Update(Planta obj)
        {
            throw new System.NotImplementedException();
        }
    }

}

