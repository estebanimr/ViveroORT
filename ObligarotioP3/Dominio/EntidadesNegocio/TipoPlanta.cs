using Dominio.OtrasInterfaces;
using System.Collections.Generic;
using Dominio.EntidadesNegocio;

namespace Dominio.EntidadesNegocio
{
	public class TipoPlanta : IValidable
	{
	
        public int Id { get; set; }
        public string NomTipoPlanta { get; set; }
        public string DescTipoPlanta { get; set; }



        /// <see>Dominio.OtrasInterfaces.IValidable#Validar()</see>
        public bool Validar()
		{
			return false;
		}

	}

}

