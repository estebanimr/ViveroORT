using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.EntidadesNegocio;
using Microsoft.AspNetCore.Http;

namespace MVCvivero.Models
{
    public class ViewModelPlanta
    {
        public Planta Planta { get; set; }
        public CuidadosPlanta CuidadosPlanta { get; set; }
        public int Altura { get; set; }
        public IEnumerable<TipoIluminacion> ListaTipoIluminacion { get; set; }
        public IEnumerable<TipoPlanta> ListaTipoPlanta{ get; set; }
        public IEnumerable<Planta> ListaDePlantas { get; set; }
        
        public int idTipoIluminacionSeleccionada { get; set; }
        public int idTipoPlantaSeleccionada { get; set; }
        public IFormFile Imagenes { get; set; }
        //public IEnumerable<IFormFile> Imagenes { get; set; }
        //public TipoPlanta TipoPlanta { get; set; }
        

        public string ambiente { get; set; }



    }
}
