using Dominio.EntidadesNegocio;
using System.Collections.Generic;

namespace Dominio.InterfacesRepositorios
{
	public interface IRepositorioPlantas : IRepositorio<Planta>
	{

		
		public List<Parametros> ParametrosValidacionLargoDesc();
		public bool BuscarPlantaPorNombreCientifico(string strNombreCientifico);
		public Planta BuscarPlantaPorNombreCientificoDevuelvePlanta(string nombreCientifico);
		public IEnumerable<Planta> BuscarPlantaPorTexto(string texto);

		public IEnumerable<Planta> BuscarPlantasPorAlturaMinima(int alturaMnima);
		public IEnumerable<Planta> BuscarPlantasPorAlturaMaxima(int masDeXaltura);

		public IEnumerable<Planta> BuscarPlantasPorTipo(TipoPlanta x);
		public IEnumerable<Planta> BuscarPlantasPorAmbiente(string ambiente);


	}
}

