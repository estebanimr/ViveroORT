using Dominio.EntidadesNegocio;
using System;
using System.Collections.Generic;

namespace Dominio.EntidadesNegocio
{
	public class Foto
	{		
        public int IdFoto { get; set; }
        public string NombreFoto { get; set; }

        public bool ValidarExtension()
        {
            bool esValido = false;
            string obtenerExtension = NombreFoto.Substring(NombreFoto.Length - 4, 4);
            if (obtenerExtension == ".jpg" || obtenerExtension == ".png") esValido = true;
            return esValido;
        }
       
        public override string ToString()
        {
            return $"ID Foto: {IdFoto} \n Nombre Foto: {NombreFoto}";
        }


        //=============PRUEBAS==============
        // ========NO SE IMPLEMENTA=========
        //=============PRUEBAS==============

        // TEORIA BASE PARA FUNCION EN UN FUTURO  
        //public string AgregarSecuenciador(int ultimoSecuenciador)
        //{
        //    // PRUEBA DE AGREGAR SECUENCIADOR AL STRING DE FOTO
        //    string strSec = Convert.ToString(IdFoto); // convierto el id de la foto en string
        //    string nuevoStr = "_"; // defino nuevo str con un guion bajo

        //    for (int i = 0; i <= 3; i++) // recorro max 3
        //    {
        //        if (strSec.Length < 3 - i) // a la secuencia la recorro y agrego un cero si cumple condicion
        //        {
        //            nuevoStr += "0";
        //        }
        //    }

        //    string nuevoStrConId = nuevoStr + strSec; // concateno el nuevo string con ceros e idSecuencial
        //    Console.WriteLine(nuevoStrConId);

        //    String original = SustituirEspaciosXGuiones(NombreCientifico);
        //    //Console.WriteLine("The original string: '{0}'", original);
        //    String modified = original.Insert(original.Length - 4, nuevoStrConId); // los coloco en las ultimas 4 posiciones
        //    //Console.WriteLine("The modified string: '{0}'", modified);

        //    return modified;
        //}


        //public string NombreFotoConGuion()
        //{
        //    string nombreNuevo = NombreFoto;
        //    string reemplazo = nombreNuevo.Replace(" ", "_"); // si hay espacios pongo un guion
        //    return nombreNuevo;
        //}

        //public string AgregarSecuenciador() // NO ESTA EN USO
        //{          
        //    string strSec = Convert.ToString(IdFoto); // convierto el id de la foto en string
        //    string nuevoStr = "_"; // defino nuevo str con un guion bajo

        //    for (int i = 0; i <= 3; i++) // recorro max 3
        //    {
        //        if (strSec.Length < 3 - i) // a la secuencia la recorro y agrego un cero si cumple condicion
        //        {
        //            nuevoStr += "0";
        //        }
        //    }

        //    string nuevoStrConId = nuevoStr + strSec; // concateno el nuevo string con ceros e idSecuencial
        //    Console.WriteLine(nuevoStrConId);

        //    String original = "aaabbb.jpg";
        //    //Console.WriteLine("The original string: '{0}'", original);
        //    String modified = original.Insert(original.Length - 4, nuevoStrConId); // los coloco en las ultimas 4 posiciones
        //    //Console.WriteLine("The modified string: '{0}'", modified);

        //    return modified;
        //}

    }

}

