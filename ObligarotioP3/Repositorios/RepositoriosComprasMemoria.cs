using Dominio.EntidadesNegocio;
using Repositorios;
using System.Collections.Generic;

namespace Repositorios
{
	public class RepositoriosComprasMemoria : IRepositorioCompras
	{
		public List<Compra> Compras;

        public bool Add(Compra obj)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Compra> FindAll()
        {
            throw new System.NotImplementedException();
        }

        public Compra FindByID(object id)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(object id)
        {
            throw new System.NotImplementedException();
        }

        public bool Update(Compra obj)
        {
            throw new System.NotImplementedException();
        }
    }

}

