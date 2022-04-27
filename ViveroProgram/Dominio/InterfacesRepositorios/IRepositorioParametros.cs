using Dominio.EntidadesNegocio;
namespace Dominio.InterfacesRepositorios
{
    public interface IRepositorioParametros:IRepositorio<Parametros>
    {
        public Parametros BuscarParametroPorDescripcion(string textoDescripcion);
    }
}
