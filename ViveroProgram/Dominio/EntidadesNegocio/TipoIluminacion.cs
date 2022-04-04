using Dominio.EntidadesNegocio;
using System.Collections.Generic;
using Dominio.OtrasInterfaces;
namespace Dominio.EntidadesNegocio
{
	public class TipoIluminacion:IValidable
	{
	

        public int Id { get; set; }

        public string NombreTipoIluminacion { get; set; }

        public bool Validar()
        {
            return false;
        }
        public bool ValidarLargoDelTxt(string texto)
        {
            bool ok = false;
            return ok;
        }

        public bool VerificarRangoAzEspaciosEmbe(string texto)
        {
            bool retorno = false;
            return retorno;

        }
    }

}

