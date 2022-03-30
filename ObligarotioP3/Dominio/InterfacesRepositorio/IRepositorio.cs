using Dominio.EntidadesNegocio;
using System.Collections.Generic;

namespace Dominio.InterfacesRepositorios
{
	public interface IRepositorio<T>
	{
		bool Add(T obj);

		bool Remove(object id);

		bool Update(T obj);

		T FindByID(object id);

		IEnumerable<T> FindAll();

	}

}

