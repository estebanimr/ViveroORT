using Dominio.EntidadesNegocio;

namespace Dominio.EntidadesNegocio
{
	public class Plaza : Compra
	{
		
        public static decimal TasaIva { get; set; }
        public decimal CostoFlete { get; set; }


        public override decimal CalcularImpuesto()
        {
            throw new System.NotImplementedException();
        }

        public override decimal CalcularCostoTotal()
        {
            throw new System.NotImplementedException();
        }

        public override string ObtenerTipoCompra()
        {
            throw new System.NotImplementedException();
        }

     
    }

}

