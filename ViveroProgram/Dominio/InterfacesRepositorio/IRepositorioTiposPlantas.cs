using Dominio.EntidadesNegocio;
using Dominio.InterfacesRepositorios;
using Repositorios;
using System.Collections.Generic;

namespace Dominio.InterfacesRepositorios
{
	public interface IRepositorioTiposPlantas : IRepositorio<TipoPlanta>
	{
		TipoPlanta BuscarTipoPlantaPorNombre(string nombreTipo);
		
	}

}

