using System;
using System.Collections.Generic;
using System.Text;
using Dominio.OtrasInterfaces;

namespace Dominio.EntidadesNegocio
{
    public class Parametros:IValidable
    {
        public int IdParametro { get; set; }
        public string TipoParametro { get; set; }
        public string DescParametro { get; set; }
        public string ValorParametro { get; set; }

        public int StringToInt(string texto)
        {
            int valorInt = Convert.ToInt32(texto);
            return valorInt;
        }
        public decimal StringToDecimal(string texto)
        {
            decimal valorDecimal = Convert.ToDecimal(texto);
            return valorDecimal;
        }

        public bool Validar()
        {
            throw new NotImplementedException();
        }
        public override string ToString()
        {
            return IdParametro + " - " + TipoParametro + " - " + DescParametro + " - " + ValorParametro;
        }
    }
}
