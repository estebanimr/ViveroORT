using System;
using System.Collections.Generic;
using System.Text;
using Dominio.InterfacesRepositorios;
using Dominio.EntidadesNegocio;
using Microsoft.Data.SqlClient;


namespace Repositorios
{
    public class RepositoriosCuidadosPlantasADO : IRepositorioCuidadosPlantas 
    //se dejo la implementacion del Irepositorio en caso de usarlo para la segunda parte del obligatorio
    {
        private Conexion manejadorConexion = new Conexion();
        private SqlConnection cn;

        public bool Add(CuidadosPlanta obj)
        {
            if (obj == null) return false;
            if (!obj.Validar()) return false;

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;

            string sqlInsert = @"INSERT INTO CuidadosPlantas 
                                 VALUES (@idTipoIluminacion, @cantidadFrecRiego, @unidadFrecRiego, @temperatura)
                                 SELECT CAST(Scope_Identity() as int)";

            SqlCommand cmdAgregarCuidadoPlanta = new SqlCommand(sqlInsert, cn);
            cmdAgregarCuidadoPlanta.Parameters.AddWithValue("@idTipoIluminacion", obj.TipoIluminacion.IdTipoIluminacion);
            cmdAgregarCuidadoPlanta.Parameters.AddWithValue("@cantidadFrecRiego", obj.CantidadFrecRiego);
            cmdAgregarCuidadoPlanta.Parameters.AddWithValue("@unidadFrecRiego", obj.UnidadFrecRiego);
            cmdAgregarCuidadoPlanta.Parameters.AddWithValue("@temperatura", obj.Temperatura);

            try
            {
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmdAgregarCuidadoPlanta.Transaction = trn;

                int idGenerado = (int)cmdAgregarCuidadoPlanta.ExecuteScalar();

                trn.Commit();
                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                trn.Rollback(); //deshace los cambios hechos
                return false;
                throw new Exception(ex.Message + "ERROR al AGREGAR un CUIDADO PLANTA");
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
        }

        public IEnumerable<CuidadosPlanta> FindAll() // FUNCIONA, pero se puede mejorar
        {
            SqlConnection cn2 = new SqlConnection();
            List<CuidadosPlanta> listaCP = new List<CuidadosPlanta>();
            try
            {
                cn = manejadorConexion.CrearConexion();
                cn2 = manejadorConexion.CrearConexion();

                SqlCommand cmdCuidadosPlantaListar = new SqlCommand("SELECT * FROM CuidadosPlantas", cn);
                SqlCommand cmdTipoIluminacionListar = new SqlCommand(@"SELECT * FROM TipoIluminacion 
                                                                       WHERE idTipoIluminacion = @idTipoIluminacionObj", cn2);

                manejadorConexion.AbrirConexion(cn);
                SqlDataReader drCuidadosPlantas = cmdCuidadosPlantaListar.ExecuteReader();//Lista de cuidadosPlantas

                if (drCuidadosPlantas.HasRows)
                {
                    while (drCuidadosPlantas.Read())
                    {
                        manejadorConexion.AbrirConexion(cn2);

                        cmdTipoIluminacionListar.Parameters.AddWithValue("@idTipoIluminacionObj", drCuidadosPlantas.GetInt32(1));
                        SqlDataReader drTipoIluminacion = cmdTipoIluminacionListar.ExecuteReader();

                        TipoIluminacion TipoIluminacionDelCuidadoOBJ = null;
                        while (drTipoIluminacion.Read())
                        {
                            TipoIluminacionDelCuidadoOBJ = new TipoIluminacion()
                            {
                                IdTipoIluminacion = drTipoIluminacion.GetInt32(0),
                                NombreTipoIluminacion = drTipoIluminacion.GetString(1)
                            };
                        }
                        cmdTipoIluminacionListar.Parameters.Clear();
                        manejadorConexion.CerrarConexionConClose(cn2);

                        CuidadosPlanta nuevoCP = new CuidadosPlanta()
                        {
                            idCuidadosPlanta = drCuidadosPlantas.GetInt32(0),
                            TipoIluminacion = TipoIluminacionDelCuidadoOBJ, //TipoIluminacionDelCuidadoOBJ
                            CantidadFrecRiego = drCuidadosPlantas.GetInt32(2),
                            UnidadFrecRiego = drCuidadosPlantas.GetString(3),
                            Temperatura = drCuidadosPlantas.GetDecimal(4)
                        };
                        listaCP.Add(nuevoCP);
                    }
                    return listaCP;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
                throw new Exception(ex.Message + "ERROR al LISTAR TODAS los CUIDADOS de PLANTA");
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
                manejadorConexion.CerrarConexion(cn2);
            }
        }


        public CuidadosPlanta FindByID(int id) // FUNCIONA
        {
            try
            {
                cn = manejadorConexion.CrearConexion();

                string strBuscaCPconTI = @"SELECT * FROM CuidadosPlantas cp
	                                            LEFT JOIN TipoIluminacion ti
	                                            ON cp.idTipoIluminacion = ti.idTipoIluminacion
                                           WHERE cp.idCuidadosPlanta = @idCP";

                SqlCommand cmdBuscaCPconTI = new SqlCommand(strBuscaCPconTI, cn);
                cmdBuscaCPconTI.Parameters.AddWithValue("@idCP", id);

                if (manejadorConexion.AbrirConexion(cn))
                {
                    SqlDataReader dr = cmdBuscaCPconTI.ExecuteReader();
                    while (dr.Read())
                    {
                        CuidadosPlanta buscaCPconTI = CrearCuidadosPlanta(dr);
                        return buscaCPconTI;
                    }                  
                }
                return null;             
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(ex.Message + "ERROR al buscar CUIDADOS PLANTAS por ID");
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
        }


        public bool Remove(CuidadosPlanta obj)
        {
            // si el CuidadoPlanta elegido no esta asignado a una Planta se puede borrar
            bool valido = false;
            if (obj == null) return valido;

            try
            {
                cn = manejadorConexion.CrearConexion();
                // Chequeo que CuidadosPlanta exista en Planta
                string strExisteCuidadosPlantaEnPlanta = @"SELECT COUNT(p.idCuidadosFK) FROM PLANTA p
                                                           WHERE p.idCuidadosFK = @idCuidadosPlanta";

                SqlCommand cmdPlantaBuscada = new SqlCommand(strExisteCuidadosPlantaEnPlanta, cn);

                manejadorConexion.AbrirConexion(cn);
                cmdPlantaBuscada.Parameters.AddWithValue("@idCuidadosPlanta", obj.idCuidadosPlanta);

                int cuidadosPlantaBuscada = (int)cmdPlantaBuscada.ExecuteScalar();

                if (cuidadosPlantaBuscada > 0) return valido;

                //si no existe Planta que use un Cuidado, se puede eliminar
                string strRemoveCP = @"DELETE FROM CuidadosPlantas
                                       WHERE idCuidadosPlanta = @IdCP";

                SqlCommand cmdRemoveCP = new SqlCommand(strRemoveCP, cn);
                cmdRemoveCP.Parameters.AddWithValue("@IdCP", obj.idCuidadosPlanta);

                int filaAEliminar = cmdRemoveCP.ExecuteNonQuery();
                if (filaAEliminar == 1) valido = true;

                return valido;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(ex.Message + "No pudo removerse el campo seleccionado");
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
        }




        public bool Update(CuidadosPlanta obj)
        {
            bool validar = false;
            if (obj == null) return validar;

            try
            {

            }
            catch
            {

            }
            finally
            {

            }


            throw new NotImplementedException();
        }


        private CuidadosPlanta CrearCuidadosPlanta(SqlDataReader read)
        {
            CuidadosPlanta nuevoCP = new CuidadosPlanta()
            {
                idCuidadosPlanta = read.GetInt32(read.GetOrdinal("idCuidadosPlanta")),
                TipoIluminacion = CrearTipoIluminacion(read),
                CantidadFrecRiego = read.GetInt32(read.GetOrdinal("cantidadFrecRiego")),
                UnidadFrecRiego = read.GetString(read.GetOrdinal("unidadFrecRiego")),
                Temperatura = read.GetDecimal(read.GetOrdinal("temperatura"))
            };
            return nuevoCP;
        }

        private TipoIluminacion CrearTipoIluminacion(SqlDataReader read)
        {
            TipoIluminacion nuevoTI = new TipoIluminacion()
            {
                IdTipoIluminacion = read.GetInt32(read.GetOrdinal("idTipoIluminacion")),
                NombreTipoIluminacion = read.GetString(read.GetOrdinal("nombreTipoIluminacion"))
            };
            return nuevoTI;
        }



    }
}

