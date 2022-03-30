using Dominio.OtrasInterfaces;
using Dominio.EntidadesNegocio;
using System.Collections.Generic;

namespace Dominio.EntidadesNegocio
{
	public class Planta : IValidable
	{
		
        public int Id { get; set; }
        public TipoPlanta Tipo { get; set; }
        public string NombreCientifico { get; set; }
        public List<NombresVulgares> NombresVulgares { get; set; }

        public string DescripcionPlanta { get; set; }
        public CuidadosPlanta CuidadosPlanta { get; set; }

        public TipoAmbiente TipoAmbiente { get; set; }
        public int AlturaMaxima { get; set; }
        public Foto Foto { get; set; }



        /// <see>Dominio.OtrasInterfaces.IValidable#Validar()</see>
        public bool Validar()
		{
			return false;
		}

	}

}

