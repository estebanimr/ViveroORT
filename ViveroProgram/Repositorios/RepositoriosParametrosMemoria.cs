using System;
using System.Collections.Generic;
using Dominio.InterfacesRepositorios;
using Dominio.EntidadesNegocio;

namespace Repositorios
{
    public class RepositoriosParametrosMemoria : IRepositorioParametros
    {
        public bool Add(Parametros obj)
        {
            throw new NotImplementedException();
        }

        public Parametros BuscarParametroPorDescripcion(string textoDescripcion)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Parametros> FindAll()
        {
            throw new NotImplementedException();
        }

        public Parametros FindByID(int id)
        {
            throw new NotImplementedException();
        }

        public bool Remove(Parametros obj)
        {
            throw new NotImplementedException();
        }

        public bool Update(Parametros obj)
        {
            throw new NotImplementedException();
        }
    }
}
