using Dominio.OtrasInterfaces;
using Dominio.EntidadesNegocio;

namespace Dominio.EntidadesNegocio
{
	public class Usuario : IValidable
	{
		

        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool EsAutenticado { get; set; }



        public bool LogIn()
		{
			return false;
		}

		public bool LogOut()
		{
			return false;
		}


		/// <see>Dominio.OtrasInterfaces.IValidable#Validar()</see>
		public bool Validar()
		{
			return false;
		}

	}

}

