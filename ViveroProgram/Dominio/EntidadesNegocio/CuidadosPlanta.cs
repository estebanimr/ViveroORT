using Dominio.OtrasInterfaces;

namespace Dominio.EntidadesNegocio
{
    public class CuidadosPlanta : IValidable
    {
        public int idCuidadosPlanta { get; set; }
        public TipoIluminacion TipoIluminacion { get; set; }
        public int CantidadFrecRiego { get; set; }  // cantidad de...
        public string UnidadFrecRiego { get; set; } // horas, dias, semanas
        public decimal Temperatura { get; set; }


        public override string ToString()
        {
            return "Cuidados de Planta: \n IdCuidados:" + idCuidadosPlanta +
                ",\n \t idTipoIluminacion: " + TipoIluminacion.IdTipoIluminacion +
                ",\n \t idTipoIluminacion: " + TipoIluminacion.NombreTipoIluminacion +
                ",\n Cantidad Frecuencia Riego: " + CantidadFrecRiego +
                ",\n Unidad Frecuencia Riego: " + UnidadFrecRiego +
                ",\n Temperatura " + Temperatura + "\n";
        }

        public bool Validar()
        {
            bool esValido = false;
            if (ValidarTemperatura(Temperatura)
                &&
                ValidarCantidad(CantidadFrecRiego)
                &&
                ValidarUnidad(UnidadFrecRiego)) esValido = true;
            return esValido;
        }

        public bool ValidarTemperatura(decimal temp)
        {
            bool tempValida = false;
            if (temp >= -100 && temp <= 100) tempValida = true;
            // Valido para que la temperatura ingresada esté dentro de los rangos
            // óptimos para que la planta sobreviva en un vivero.
            return tempValida;
        }

        public bool ValidarCantidad(int numeroCant)
        {
            bool esValido = false;
            if (numeroCant > 0) esValido = true;
            return esValido;
        }

        public bool ValidarUnidad(string textoUnidad)
        {
            bool esValido = false;
            if (textoUnidad.ToLower() == "horas"
                ||
                textoUnidad.ToLower() == "dias"
                ||
                textoUnidad.ToLower() == "semanas"
                ||
                textoUnidad.ToLower() == "meses"
                ||
                textoUnidad.ToLower() == "años"
                )
                esValido = true;
            return esValido;
        }




    }

}



