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
        public override string ToString()
        {
            return Id + " " + NomTipoPlanta + " " + DescTipoPlanta;
        }

        //TODO implementar validacion 
        public bool Validar()
        {
            bool esValido = false;
            if (VerificarRangoAzEspaciosEmbe(NomTipoPlanta)) esValido = true;
            return esValido;
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

        public bool ValidarLargoDescTP(List<Parametros> parametros)
        {
            bool valido = false;
            int largoMinimo = parametros[1].StringToInt(parametros[1].ValorParametro);
            int largoMaximo = parametros[0].StringToInt(parametros[0].ValorParametro);
            if (DescTipoPlanta.Length >= largoMinimo && DescTipoPlanta.Length <= largoMaximo) valido = true;
            return valido;
        }
    }

}

