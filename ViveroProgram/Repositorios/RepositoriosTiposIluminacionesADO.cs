using Dominio.EntidadesNegocio;
using Dominio.InterfacesRepositorios;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System;

namespace Repositorios
{
    public class RepositoriosTiposIluminacionesADO : IRepositorioTiposIluminaciones
    {
        private Conexion manejadorConexion = new Conexion();
        private SqlConnection cn;

        public bool Add(TipoIluminacion obj) // Quedo FUNCIONA !!!
        {
            if (obj == null) return false;
            if (BuscarTipoIluminacionPorNombre(obj.NombreTipoIluminacion) != null) return false;

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;

            try
            {
                string stringInsertTipoIluminacion = @"INSERT INTO TipoIluminacion 
                                                       VALUES(@nombre)
                                                       SELECT CAST(Scope_Identity() AS INT);";

                SqlCommand cmdAgregarTipoIluminacion = new SqlCommand(stringInsertTipoIluminacion, cn);
                cmdAgregarTipoIluminacion.Parameters.AddWithValue("@nombre", obj.NombreTipoIluminacion);

                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmdAgregarTipoIluminacion.Transaction = trn;

                int idGenerado = (int)cmdAgregarTipoIluminacion.ExecuteScalar();

                trn.Commit();
                return true;
            }
            catch (Exception ex)
            {
                trn.Rollback();
                return false;
                throw new Exception(ex.Message + "ERROR al AGREGAR un TIPO de ILUMINACION");
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
        }

        public IEnumerable<TipoIluminacion> FindAll()  // VER porque NO devuelve la lista
        {
            try
            {
                cn = manejadorConexion.CrearConexion();
                SqlCommand cmdTipoIlumListar = new SqlCommand("SELECT * FROM TipoIluminacion", cn);
                manejadorConexion.AbrirConexion(cn);
                SqlDataReader dr = cmdTipoIlumListar.ExecuteReader();
                if (dr.HasRows)
                {
                    List<TipoIluminacion> listaTI = new List<TipoIluminacion>();

                    while (dr.Read())
                    {
                        listaTI.Add(new TipoIluminacion
                        {
                            IdTipoIluminacion = (int)dr["IdTipoIluminacion"],
                            NombreTipoIluminacion = dr["nombreTipoIluminacion"].ToString()
                        });

                        //TipoIluminacion nuevoTI = new TipoIluminacion()
                        //{
                        //    IdTipoIluminacion = dr.GetInt32(0),
                        //    NombreTipoIluminacion = dr.GetString(1)
                        //};
                        //ListaTI.Add(nuevoTI);
                    }
                    return listaTI;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message + "ERROR al LISTAR TODAS los TIPOS DE ILUMINACION");
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
        }

        public TipoIluminacion FindByID(int id)
        {
            try
            {
                cn = manejadorConexion.CrearConexion();
                SqlCommand cmdListarTipoIlumPorId = new SqlCommand(@"SELECT * FROM TipoIluminacion
                                                                     WHERE idTipoIluminacion = @idTI", cn);
                cmdListarTipoIlumPorId.Parameters.AddWithValue("@idTI", id);
                if (manejadorConexion.AbrirConexion(cn))
                {
                    SqlDataReader dr = cmdListarTipoIlumPorId.ExecuteReader();
                    while (dr.Read())
                    {
                        return new TipoIluminacion
                        {
                            IdTipoIluminacion = (int)dr["idTipoIluminacion"],
                            NombreTipoIluminacion = dr["nombreTipoIluminacion"].ToString()
                        };
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "ERROR al buscar Tipo Planta por ID");
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
        }

        public bool Remove(TipoIluminacion obj)
        {
            bool valido = false;
            if (obj == null) return valido;

            try
            {
                cn = manejadorConexion.CrearConexion();

                // Busco si existe el Tipo Iluminacion en CuidadosPlanta
                string strExisteTipoIlumEnCuidados = @"SELECT COUNT(cp.idTipoIluminacion) FROM CuidadosPlantas cp
                                                       WHERE cp.idTipoIluminacion = @idTipoIluminacion";
                SqlCommand cmdTipoIlumId = new SqlCommand(strExisteTipoIlumEnCuidados, cn);

                manejadorConexion.AbrirConexion(cn);
                cmdTipoIlumId.Parameters.AddWithValue("@idTipoIluminacion", obj.IdTipoIluminacion);

                int tipoIluminacionBuscado = (int)cmdTipoIlumId.ExecuteScalar();

                // Si encuentro un Tipo de Iluminacion en CuidadosPlanta, no la elimino
                if (tipoIluminacionBuscado > 0) return valido;

                // Si no existe un CuidadoPlanta que use el Tipo de iluminacion, removemos el tipo iluminacion
                string strRemoveTI = @"DELETE FROM TipoIluminacion WHERE idTipoIluminacion = @idTipoIluminacion";
                SqlCommand cmdRemoveTI = new SqlCommand(strRemoveTI, cn);
                cmdRemoveTI.Parameters.AddWithValue("@idTipoIluminacion", obj.IdTipoIluminacion);

                int filaAEliminar = cmdRemoveTI.ExecuteNonQuery();
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

        public bool Update(TipoIluminacion obj)
        {
            bool validar = false;
            if (obj == null) return validar;
            // verificamos que los datos ingresados siguen las reglas del negocio
            // Verificamos por Nombre en la tabla para tener el campo UNIQUE
            if (BuscarTipoIluminacionPorNombre(obj.NombreTipoIluminacion) != null) return false;
            SqlTransaction trn = null;

            try
            {
                cn = manejadorConexion.CrearConexion();

                string strUpdateTI = @"UPDATE TipoIluminacion SET nombreTipoIluminacion = @nombreTipoIluminacion
                                       WHERE idTipoIluminacion = @idTipoIluminacion;";
                SqlCommand cmdUpdateTI = new SqlCommand(strUpdateTI, cn);

                cmdUpdateTI.Parameters.AddWithValue("@idTipoIluminacion", obj.IdTipoIluminacion);
                cmdUpdateTI.Parameters.AddWithValue("@nombreTipoIluminacion", obj.NombreTipoIluminacion);

                if (manejadorConexion.AbrirConexion(cn))
                {
                    trn = cn.BeginTransaction();
                    cmdUpdateTI.Transaction = trn;
                    int filaAfectada = cmdUpdateTI.ExecuteNonQuery();
                    validar = filaAfectada == 1;
                    trn.Commit();
                }

            }
            catch (Exception ex)
            {
                if (trn != null) trn.Rollback();
                throw new Exception(ex.Message + "ERROR, no se pueden actualizar los datos");
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
            return validar;
        }

        public TipoIluminacion BuscarTipoIluminacionPorNombre(string nombre)
        {
            TipoIluminacion nombreBuscado = null;
            if (nombre == "") return nombreBuscado;
            cn = manejadorConexion.CrearConexion();

            string strBuscarNombreTI = @"SELECT * FROM TipoIluminacion ti
                                         WHERE ti.nombreTipoIluminacion = @nombreTI";

            SqlCommand cmdBuscaNombreTI = new SqlCommand(strBuscarNombreTI, cn);

            cmdBuscaNombreTI.Parameters.AddWithValue("@nombreTI", nombre);
            manejadorConexion.AbrirConexion(cn);

            try
            {
                SqlDataReader dr = cmdBuscaNombreTI.ExecuteReader();

                while (dr.Read())
                {
                    return new TipoIluminacion
                    {
                        IdTipoIluminacion = dr.GetInt32(0),
                        NombreTipoIluminacion = dr.GetString(1)
                    };
                }
                return nombreBuscado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "ERROR al Buscar por el Nombre de TIPO ILUMINACION");
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
        }



    }

}

