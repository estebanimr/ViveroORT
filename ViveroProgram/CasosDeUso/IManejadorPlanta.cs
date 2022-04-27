using System;
using System.Collections.Generic;
using System.Text;
using Dominio.EntidadesNegocio;

namespace Manejadores
{
    public interface IManejadorPlanta
    {
        bool DarDeAltaPlanta(Planta pla, int idIluminacion, int idTipoPlanta, string valorAltura);

        IEnumerable<TipoIluminacion> ObtenerTiposIluminaciones();

        IEnumerable<TipoPlanta> ObtenerTiposPlantas();
       
        IEnumerable<Planta> ObtenerTodasLasPlantas();

        string CambiarNombreFoto(string nombreCien, string nombreFoto);
        Planta ObtenerPlantaPorId(int idPlanta);
        bool EliminarPlanta(int idPlanta);

        IEnumerable<Planta> BuscarPlantasPorParteDeTexto(string texto);

        public IEnumerable<Planta> ListarPlantaPorTP(int id );

        IEnumerable<Planta> BuscarPlantasPorAlturaMax(int altura);
        IEnumerable<Planta> BuscarPlantasPorAlturaMin(int altura);
        IEnumerable<Planta> BuscarPlantasPorAmb(string ambiente);
    }
}
