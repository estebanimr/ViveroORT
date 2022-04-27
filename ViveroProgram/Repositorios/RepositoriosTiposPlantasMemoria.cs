using Dominio.EntidadesNegocio;
using Dominio.InterfacesRepositorios;
using System.Collections.Generic;

namespace Repositorios
{
    public class RepositoriosTiposPlantasMemoria : IRepositorioTiposPlantas
    {
        public List<TipoPlanta> TiposPlantas { get; set; } = new List<TipoPlanta>();
        public int ultimoID { get; set; } = 1;
        public bool Add(TipoPlanta obj)
        {
            bool ok = false;
            if (obj != null)
            {
                obj.Id = ultimoID ++;
                TiposPlantas.Add(obj);
                ok = true;
            }
            return ok;
        }

        public TipoPlanta BuscarTipoPlantaPorNombre(string nombreTipo)
        {
            TipoPlanta buscada = null;
            int i = 0;
            while (buscada == null && i < TiposPlantas.Count)
            {
                if (nombreTipo == TiposPlantas[i].NomTipoPlanta)
                {
                    buscada = TiposPlantas[i];
                }

                i++;
            }
            return buscada;
            
        }

        public IEnumerable<TipoPlanta> FindAll()
        {
            return TiposPlantas;
        }

        public TipoPlanta FindByID(int id)
        {
            TipoPlanta plantaBuscada = null;// Defino plantaBuscada para poder dar un return, parte de null
            foreach (TipoPlanta TP in TiposPlantas)// recorro la lista de tiposde plantas
            {
                if (id == TP.Id)// si el id recibido coincide con el id de (TP) la planta en la recorrida actual
                {
                    return TP;// hago return de la planta TP que seria la planta que estaba buscando
                }
            }
            return plantaBuscada; // si no entra al if va a salir de la funcion con el item plantaBucada en null;
        }
        
        public bool Remove(TipoPlanta obj)
        {
            bool ok = false;
            if (obj != null)
            {
                TiposPlantas.Remove(obj);
                ok = true;
            }
            return ok;
        }

        public bool Update(TipoPlanta obj)
        {
            bool resultadoUpdate = false;

            TipoPlanta TPaModificar = FindByID(obj.Id);
            //busco el TipoPlanta a modificar por el id.

            if (obj != null && TPaModificar !=null && BuscarTipoPlantaPorNombre(obj.NomTipoPlanta) == null) //falta implementar validaciones mas pro.
                //si el nombre que viene en obj no esta usado , entonces entro al if.
            {
                TPaModificar.NomTipoPlanta = obj.NomTipoPlanta;
                TPaModificar.DescTipoPlanta = obj.DescTipoPlanta;
                resultadoUpdate = true;

            }
            return resultadoUpdate;
        }

       
    }


}

