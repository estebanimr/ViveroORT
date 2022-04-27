using Dominio.OtrasInterfaces;
using Dominio.EntidadesNegocio;
using System.Collections.Generic;

namespace Dominio.EntidadesNegocio
{
	public class Planta : IValidable
	{
		
        public int IdPlanta { get; set; }
        public TipoPlanta Tipo { get; set; }
        public string NombreCientifico { get; set; }
        public string NombresVulgares { get; set; }

        public string DescripcionPlanta { get; set; }
        public CuidadosPlanta CuidadosPlanta { get; set; }

        public Ambiente TipoAmbiente { get; set; }
        public Parametros AlturaMaxima { get; set; }
        public List<Foto> FotosDePlanta { get; set; }

        public enum Ambiente
        {
            interior = 1,
            exterior = 2,
            mixta = 3,
        }

        /// <see>Dominio.OtrasInterfaces.IValidable#Validar()</see>
        public bool Validar()
		{

            if (!VerificarRangoAzEspaciosEmbe(NombreCientifico)) return false;
            if (!ValidarExtDeNombresDeFotoEnLista()) return false;
            IntercambiarNombresFotosXNomCientifcoYaddSec();
            return true;
		}

        public bool ValidarLargoDescPlanta(List<Parametros> parametros)
        {
            bool ok = false;
            //    parametros[1].StringToInt(parametros[1].ValorParametro);
            if (DescripcionPlanta.Length >= parametros[1].StringToInt(parametros[1].ValorParametro)
                && 
                DescripcionPlanta.Length <= parametros[0].StringToInt(parametros[0].ValorParametro)) ok = true;
            return ok;
        }

        public bool VerificarRangoAzEspaciosEmbe(string texto)
        {
            bool retorno = false;
            for (int i = 0; i < texto.Length; i++)
            {
                //A=65 al Z=90 and a=97 al z=122  32=espacio
                if (
                    ((int)texto[i] >= 65 && ((int)texto[i] <= 90)
                    ||
                    ((int)texto[i] >= 97 && (int)texto[i] <= 122)
                    ||
                    (int)texto[i] == 32)) retorno = true;
                else return false;
            }
            if ((int)texto[0] == 32 && (int)texto[texto.Length - 1] == 32) retorno = false;

            return retorno;
        }

        public override string ToString()
        {
            return
            "PLANTA: \n" +
                $"\t Nombre cientifico: {NombreCientifico} \n " +
                $"\t Nombres Vulgares:{NombresVulgares} \n" +
                $"\t Tipo Ambiente: {TipoAmbiente} \n" +
                $"\t Altura Maxima: {AlturaMaxima.ValorParametro} Centimetros \n" +
                /* $"\t Nombres Fotos: {mostrarNombresDeFotos(FotosDePlanta)} \n" +*/
                "TIPO PLANTA:\n" +
                $"\t Tipo planta nombre: {Tipo.NomTipoPlanta}: \n" +
                $"\t Descripcion de tipo de planta: {Tipo.DescTipoPlanta} \n" +
                "CUIDADOS DE PLANTA: \n" +
                $"\t Cuidados de la Planta: {CuidadosPlanta.idCuidadosPlanta} \n" +
                $"\t Cantidad Frecuencia Riego:  {CuidadosPlanta.CantidadFrecRiego}  \n" +
                $"\t Unidad Frecuencia Riego: {CuidadosPlanta.UnidadFrecRiego} \n " +
                $"\t Temperatura: {CuidadosPlanta.Temperatura} \n " +
                $"TIPO ILUMINACION: \n" +
                $"\t idTipoIluminacion: {CuidadosPlanta.TipoIluminacion.IdTipoIluminacion}  \n" +
                $"\t idTipoIluminacion: {CuidadosPlanta.TipoIluminacion.NombreTipoIluminacion} \n";
            
        }

        //public string mostrarNombresDeFotos(List<Foto> LstFotos)
        //{
        //    string nombres = "\n";

        //    foreach (Foto item in LstFotos)
        //    {
        //        nombres += "\t " + item.NombreFoto + "\n";

        //    }
        //    return nombres;
        //}

        #region VALIDACIONES PARA FOTO

        public bool ValidarExtDeNombresDeFotoEnLista()
        {
            bool fotosValidas = false;
            foreach (Foto item in FotosDePlanta)
            {
                if (!item.ValidarExtension()) return false; // valida ".jpg" o ".png"
            }
            fotosValidas = true;
            return fotosValidas;
        }


        public void IntercambiarNombresFotosXNomCientifcoYaddSec() 
        {
            int secuenciador = 001;
            foreach (Foto f in FotosDePlanta)
            {
                string secuen = secuenciador.ToString("D3");
                string ext = f.NombreFoto.Substring(f.NombreFoto.Length - 4, 4);// ".jpg"
                string NomCientificoConGuion = NombreCientifico;
                string NuevoNombreDeFoto = SustituirEspaciosXGuiones(NomCientificoConGuion) + "_" + secuen + ext;
                f.NombreFoto = NuevoNombreDeFoto;
                secuenciador++;
            }
        }


        public string SustituirEspaciosXGuiones(string txt)
        {
            string reemplazo = txt.Replace(" ", "_"); // si hay espacios pongo un guion
            return reemplazo;
        }



        
        #endregion

    }

}

