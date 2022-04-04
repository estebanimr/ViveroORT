using Dominio.OtrasInterfaces;
using System;
using Dominio.EntidadesNegocio;
using System.Collections.Generic;

namespace Dominio.EntidadesNegocio
{
	public abstract class Compra : IValidable
	{
	

        public int Id { get; set; }

        public DateTime FechaCompra { get; set; }

        public List<ItemPlantaComprada> ItemsPlantasCompradas { get; set; }
        public decimal ImpuestoCompra { get; set; }

        public decimal PrecioTotal { get; set; }

        public string TipoCompra { get; set; }


        public abstract decimal CalcularImpuesto();

		public abstract decimal CalcularCostoTotal();

		public abstract string ObtenerTipoCompra();


		/// <see>Dominio.OtrasInterfaces.IValidable#Validar()</see>
		
		 
		//hacer validacion
		public bool Validar()
		{
			return false;
		}

        public bool VerificarRangoAzEspaciosEmbe(string texto)
        {
            throw new NotImplementedException();
        }

        public bool ValidarLargoDelTxt(string texto)
        {
            throw new NotImplementedException();
        }
    }

}

