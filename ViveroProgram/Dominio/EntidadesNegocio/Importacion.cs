using Dominio.EntidadesNegocio;

namespace Dominio.EntidadesNegocio
{
	public class Importacion : Compra
	{
		

        public static decimal TasaBasica { get; set; }
        public bool EsDeAmericaDelSur { get; set; }
        public static decimal TasaArancelaria { get; set; }
        public string DescripcionSanitaria { get; set; }

        public double CalcularImpuesto(bool esDeAmericaDelSur, decimal tasaBasica, decimal tasaArancelaria)
		{
			return 0;
		}

		public override decimal CalcularCostoTotal()
		{
			return 0;
		}

        public override decimal CalcularImpuesto()
        {
            throw new System.NotImplementedException();
        }

        public override string ObtenerTipoCompra()
        {
            throw new System.NotImplementedException();
        }
    }

}

