using Dominio.EntidadesNegocio;
using System;
using System.Collections.Generic;
using System.Text;
using Dominio.InterfacesRepositorios;
namespace Manejadores
{
    public class ManejadorUsuario : IManejadorUsuario
    {
        public IRepositorioUsuarios RepoUsuarios { get; set; }

        public ManejadorUsuario(IRepositorioUsuarios repoUsu)
        {
            RepoUsuarios = repoUsu;
        }
        public bool AltaUsuario(Usuario u)
        {
            return RepoUsuarios.Add(u);
        }

        public Usuario Login(string email, string pass)
        {
          return  RepoUsuarios.AutenticarU(email,pass);
        }

        public bool Logout()
        {
            throw new NotImplementedException();
        }
    }
}
