using Dominio.EntidadesNegocio;

namespace Dominio.InterfacesRepositorios
{
    public interface IRepositorioUsuarios : IRepositorio<Usuario>
    {
        public Usuario AutenticarU(string email, string pass);
    }

}

