using System;
using System.Collections.Generic;
using System.Text;
using Dominio.EntidadesNegocio;
namespace Manejadores
{
  public  interface IManejadorUsuario
    {
        bool AltaUsuario(Usuario u);

        Usuario Login(string email, string password);

        bool Logout();
    }
}
