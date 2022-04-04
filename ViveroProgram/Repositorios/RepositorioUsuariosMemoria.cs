using Dominio.EntidadesNegocio;
using Repositorios;
using System.Collections.Generic;

namespace Repositorios
{
	public class RepositorioUsuariosMemoria : IRepositorioUsuarios
	{
		public List<Usuario> Usuarios;

        public bool Add(Usuario obj)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Usuario> FindAll()
        {
            throw new System.NotImplementedException();
        }

        public Usuario FindByID(int id)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(Usuario obj)
        {
            throw new System.NotImplementedException();
        }

        public bool Update(Usuario obj)
        {
            throw new System.NotImplementedException();
        }
    }

}

