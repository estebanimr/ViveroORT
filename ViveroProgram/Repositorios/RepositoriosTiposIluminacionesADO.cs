using Dominio.EntidadesNegocio;
using Dominio.InterfacesRepositorios;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System;

namespace Repositorios
{
    public class RepositoriosTiposIluminacionesADO : IRepositorioTiposIluminaciones
    {
        private Conexion ManejadorConexiones = new Conexion();
        private SqlConnection cn;

        public bool Add(TipoIluminacion obj) // Quedo falta verificar en BD
        {
            bool ok = false;
            if (obj.Validar())
            {

                cn = ManejadorConexiones.CrearConexion();
                string SqlInsertDeTipoIluminacion = "INSERT INTO TiposIluminaciones VALUEs(@nom);" +
                                                    "SELECT CAST(SCOPE_IDENTITY() AS INT);";
                SqlCommand NuevoComando = new SqlCommand(SqlInsertDeTipoIluminacion, cn);
                NuevoComando.Parameters.AddWithValue("@num", obj.NombreTipoIluminacion);
                SqlTransaction tran = null;
                try
                {
                    ManejadorConexiones.AbrirConexion(cn);
                    tran = cn.BeginTransaction();
                    NuevoComando.Transaction = tran;
                    int ultimoID = (int)NuevoComando.ExecuteScalar();
                    obj.Id = ultimoID;
                    NuevoComando.ExecuteNonQuery();
                    tran.Commit();
                    ok = true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    ok = false;
                    throw;
                }
                finally
                {
                    ManejadorConexiones.CerrarConexion(cn);
                }
            }
            return ok;
        }
  
        public IEnumerable<TipoIluminacion> FindAll() 
        {

            //List<TipoIluminacion> tiposIluminaciones = new List<TipoIluminacion>();
            //cn = ManejadorConexiones.CrearConexion();
            //string SqlObtenerTiposIluminacion = "SELECT * FROM TiposIluminaciones;";
            //SqlCommand nuevoComando = new SqlCommand(SqlObtenerTiposIluminacion, cn);
            //SqlTransaction tran = null;
            //try
            //{


            //}
            //catch (Exception)
            //{

            //    throw;
            //}
            //finally
            //{
            //    ManejadorConexiones.CrearConexion();
            //}
            throw new System.NotImplementedException();
        }

        public TipoIluminacion FindByID(int id)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(TipoIluminacion obj)
        {
            throw new System.NotImplementedException();
        }

        public bool Update(TipoIluminacion obj)
        {
            throw new System.NotImplementedException();
        }
    }

}

