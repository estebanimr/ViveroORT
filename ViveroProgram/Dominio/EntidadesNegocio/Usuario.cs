using Dominio.OtrasInterfaces;
using Dominio.EntidadesNegocio;
using System.ComponentModel.DataAnnotations;
namespace Dominio.EntidadesNegocio
{
	public class Usuario : IValidable
	{

        public int Id { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }
        public bool EsAutenticado { get; set; }

        /// <see>Dominio.OtrasInterfaces.IValidable#Validar()</see>
        public bool Validar()
        {
            if (Email == null || Pass == null) return false;
            if (new EmailAddressAttribute().IsValid(Email) && ValidarPassword(Pass))
            {
                return true;
            }
            return false;
        }

        public bool ValidarPassword(string texto)
        {
            bool valido = false;
            bool mayus = false;
            bool minus = false;
            bool num = false;

            if (texto.Trim().Length < 6) return false;
            
            for (int i = 0; i < texto.Length && !valido; i++)
            {
                if (texto[i] >= 48 && texto[i] <= 57 && !num) num = true;
                if (texto[i] >= 65 && texto[i] <= 90 && !mayus) mayus = true;
                if (texto[i] >= 97 && texto[i] <= 122 && !minus) minus = true;
                if (num && mayus && minus) valido = true;
            }
            return valido;
        }

        public override string ToString()
        {
            return  $"Id: {Id} \n Email: {Email}";
        }
    }

}

