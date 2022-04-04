using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;
namespace Repositorios
{
   public class Conexion
    {
        private string cadenaConexion = @"SERVER=(localdb)\mssqllocaldb;
                                DATABASE=viveroP3;
                                INTEGRATED SECURITY= true;
                                TrustServerCertificate=true";

        public SqlConnection CrearConexion()
        {
            return new SqlConnection(cadenaConexion);
        }

        public bool AbrirConexion(SqlConnection cn)
        {
            try
            {
                if (cn.State != ConnectionState.Open)
                {
                    cn.Open();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {

                return false;
            }
            finally
            {
                System.Diagnostics.Debug.WriteLine("Entré al finally de abrir conexión");
            }
        }

        public bool CerrarConexion(SqlConnection cn)
        {
            try
            {
                if (cn.State != ConnectionState.Closed)
                {
                    cn.Close();

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                cn.Dispose();
                System.Diagnostics.Debug.WriteLine("Entré al finally de cerrar conexión");
            }
        }

        public bool CerrarConexionConClose(SqlConnection cn)
        {
            try
            {
                if (cn.State != ConnectionState.Closed)
                {
                    cn.Close();

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                // cn.Dispose();
                System.Diagnostics.Debug.WriteLine("Entré al finally de cerrar conexión");
            }
        }



    }
}
