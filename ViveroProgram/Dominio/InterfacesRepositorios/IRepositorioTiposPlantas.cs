using Dominio.EntidadesNegocio;
namespace Dominio.InterfacesRepositorios
{
	public interface IRepositorioTiposPlantas : IRepositorio<TipoPlanta>
	{
		TipoPlanta BuscarTipoPlantaPorNombre(string nombreTipo);
	}

}

