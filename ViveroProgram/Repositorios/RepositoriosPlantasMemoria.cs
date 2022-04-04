using Dominio.EntidadesNegocio;
using Repositorios;
using System.Collections.Generic;

namespace Repositorios
{
	public class RepositoriosPlantasMemoria : IRepositorioPlantas
	{

        //[DE CLASE]//+ CreateNewPlanta(tipo : TipoPlanta, nomCien : string, nomVulg : List<NombresVulgares>, descPlanta : string, ambiente : TipoAmbiente, altMax : int, foto : int) : Planta 
        //[AGREGAR firma EN IRepositorioPlantas, DEFINIR RepositoriosPlantasMemoria]//+ SearchPlantaPorNombre(textoBusqueda : string) : List<Planta>
        //[AGREGAR firma EN IRepositorioPlantas, DEFINIR RepositoriosPlantasMemoria]//+ SearchPorTipo(tipo : TipoPlanta) : List<Planta>                          
        //[AGREGAR firma EN IRepositorioPlantas, DEFINIR RepositoriosPlantasMemoria]//+ SearchPorAmbiente(TipoAmbiente : Enum) : List<Planta>                                                        
        //+ SearchAlturaBajas(altura : int) : List<Planta>                                                                                                                                                                                                        
        //+ SearchAlturaAltas(altura : int) : List<Planta>                                                                                                                                                                                                           






        public List<Planta> Plantas;


        
        
        
        public bool Add(Planta obj)
        {
            
            throw new System.NotImplementedException();
        }
        //+ SearchTodasPlantas() : List<Planta>
        public IEnumerable<Planta> FindAll()
        {
            throw new System.NotImplementedException();
        }

        public Planta FindByID(int id)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(Planta obj)
        {
            throw new System.NotImplementedException();
        }

        public bool Update(Planta obj)
        {
            throw new System.NotImplementedException();
        }
    }

}

