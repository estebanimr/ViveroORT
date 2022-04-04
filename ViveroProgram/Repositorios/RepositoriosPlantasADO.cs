using Dominio.EntidadesNegocio;
using Repositorios;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
namespace Repositorios
{
    public class RepositoriosPlantasADO : IRepositorioPlantas
    {
        private Conexion manejadorConexion = new Conexion();
        private SqlConnection cn;

        public bool Add(Planta obj)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Planta> FindAll()
        {
            throw new System.NotImplementedException();
        }


        /*
        public IEnumerable<Planta> FindAll()
        {
            try
            {
                cn = manejadorConexion.CrearConexion();
                string primerselect = "SELECT * FROM Planta";
                //SqlCommand cmdPlantaListar = new SqlCommand(primerselect , cn);
                SqlCommand cmdPlantaListar = new SqlCommand();
                cmdPlantaListar.CommandText = primerselect;
                cmdPlantaListar.Connection=cn;

                manejadorConexion.AbrirConexion(cn);
                SqlDataReader dr = cmdPlantaListar.ExecuteReader();
                if (dr.HasRows)
                {
                    cmdPlantaListar.Parameters.Clear();
                    List<Planta> listaPlanta = new List<Planta>();
                    while (dr.Read())
                    {
                        #region Obtenemos el tipo de planta mediante el ID de tipo planta que viene en planta
                        string TipoPlantaBuscada = @"SELECT * FROM TipoPlanta WHERE Id = @idTipoDePlanta ";
                        int idTipoDePlanta = dr.GetInt32(1);
                        cmdPlantaListar.CommandText = TipoPlantaBuscada;
                        cmdPlantaListar.Connection = cn;
                        cmdPlantaListar.Parameters.AddWithValue("@idTipoDePlanta", idTipoDePlanta);
                        TipoPlanta TipoPlantaLeido = new TipoPlanta() { @Id , @NomTipoPlanta, @DescTipoPlanta }
                        #endregion
                        Planta pla = new Planta()
                        {
                            Id = reader.GetInt32(0),
                            Tipo = TipoPlantaLeido,
                            NombreCientifico = reader.GetString(2),
                            NombresVulgares = reader.GetString(3),
                            DescripcionPlanta = reader.GetString(4),
                            Cuidados = reader.GetString(5),
                            AlturaMaxima = reader.GetInt32(6),
                            Foto = reader.GetString(6)
                            //con GetORdinal(nombre de columna de la bd) y aca te devuelve un indice
                            //idcliente = reader.GetInt32(reader.GetORdinal("IdCliente")) me devolveria la posicion 0
                            //es lo contrario a reader. con reader tenemos que ponerle una posicion de columna
                        };
                        Planta.Add(pla);
                        cmdPlantaListar.Parameters.Clear();



                        listaPlanta.Add(new Planta
                        {
                            Id = (int)dr["Id"],
                            Tipo = (object)dr["TipoPlanta"].GetType(TipoPlanta.Equals),
                            NombreCientifico = dr["NombreCientifico"].ToString(),
                            NombresVulgares = dr["NombresVulgares"].ToString(),
                            DescripcionPlanta = dr["DescripcionPlanta "].ToString(),
                            Cuidados = dr["Cuidados"].ToString(),
                            AlturaMaxima = dr["AlturaMaxima"].int32(),
                            Foto = dr["Foto"].ToString()

                        });

                    }
                    return listaPlanta;
                }
                return null;
            }
            catch (Exception ex)
            {

                return null;
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
        }

        */




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

