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

        public bool ValidarLargoDelTxt(string texto)
        {
            bool ok = false;
            if (texto.Length >= 10 && texto.Length <= 500) ok = true;

            return ok;
        }

        public bool VerificarRangoAzEspaciosEmbe(string texto)
        {
            bool retorno = false;
            for (int i = 0; i < texto.Length; i++)
            {
                //A=65 al Z=90 and a=97 al z=122  32=espacio
                if (
                    ((int)texto[i] >= 65 && ((int)texto[i] <= 90)
                    ||
                    ((int)texto[i] >= 97 && (int)texto[i] <= 122)
                    ||
                    (int)texto[i] == 32)) retorno = true;
                else return false;
            }
            if ((int)texto[0] == 32 && (int)texto[texto.Length - 1] == 32) retorno = false;

            return retorno;
        }
    }

}

