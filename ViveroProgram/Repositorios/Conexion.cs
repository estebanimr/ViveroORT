using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions;
namespace Repositorios
{
   public class Conexion
    {

        //private string cadenaConexion = @"SERVER=(localdb)\mssqllocaldb;
        //                        DATABASE=viveroP3;
        //                        INTEGRATED SECURITY= true;
        //                        TrustServerCertificate=true";

        public static string ObtenerConexion()
        {
            string CadenaConexion = "";
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            CadenaConexion = config.GetConnectionString("MiConexion");
            return CadenaConexion;
        }

        public SqlConnection CrearConexion()
        {
            return new SqlConnection(ObtenerConexion());
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
                throw new Exception(ex.Message + "No se pudo abrir la conexion");
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
                throw new Exception(ex.Message + "No se pudo cerrar la conexion");
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
                throw new Exception(ex.Message + "No se pudo cerrarCLOSE la conexion");
            }
            finally
            {
                // cn.Dispose();
                System.Diagnostics.Debug.WriteLine("Entré al finally de cerrar conexión");
            }
        }



    }
}
