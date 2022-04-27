using System;
using System.Collections.Generic;
using System.Text;
using Dominio.EntidadesNegocio;

namespace Manejadores
{
    public interface IManejadorTipoPlanta
    {
        bool AgregarTipoPlanta(TipoPlanta tp);
        
        IEnumerable<TipoPlanta> ListarTodosLosTipoPlanta();
        bool EliminarTipoPlanta(int id);
        bool ModificarDescDeTipoPlanta(TipoPlanta tp);
        TipoPlanta BuscarTPporId(int id);

        TipoPlanta BuscarTPporNombre(string nombre);


        
        
    }
}
