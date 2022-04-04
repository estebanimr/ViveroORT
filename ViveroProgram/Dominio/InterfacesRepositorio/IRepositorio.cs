using Dominio.EntidadesNegocio;
using System.Collections.Generic;

namespace Dominio.InterfacesRepositorios
{
	public interface IRepositorio<T>
	{
		bool Add(T obj);

		bool Remove(T obj);

		bool Update(T obj);

		T FindByID(int id);

		IEnumerable<T> FindAll();

	}

}

