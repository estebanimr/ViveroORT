using System;
using System.Collections.Generic;
using Dominio.EntidadesNegocio;
using Dominio.InterfacesRepositorios;
using Microsoft.Data.SqlClient;

namespace Repositorios
{
    public class RepositoriosParametrosADO : IRepositorioParametros
    {
        private Conexion manejadorConexion = new Conexion();
        private SqlConnection cn;
        public bool Add(Parametros obj)
        {
            bool retorno = false;
            if (obj == null) return retorno;
            if (BuscarParametroPorDescripcion(obj.DescParametro) != null) return retorno;

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            string sqlInsertarPar = "INSERT INTO Parametros VALUES (@Tipo, @Descripcion,@Valor)" +
                "SELECT CAST(Scope_Identity() as int)";
            SqlCommand cmdAgregarParametro = new SqlCommand(sqlInsertarPar, cn);
            cmdAgregarParametro.Parameters.AddWithValue("@Tipo", obj.TipoParametro);
            cmdAgregarParametro.Parameters.AddWithValue("@Descripcion", obj.DescParametro);
            cmdAgregarParametro.Parameters.AddWithValue("@Valor", obj.ValorParametro);
            try
            {
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmdAgregarParametro.Transaction = trn;
                int idGenerado = (int)cmdAgregarParametro.ExecuteScalar();
                trn.Commit();
                retorno = true;
                return retorno;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                trn.Rollback();
                retorno = false;
                return retorno;
                throw;
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }

        }// FUNCIONA  OK conecta bien con BD

        public IEnumerable<Parametros> FindAll()
        {

            cn = manejadorConexion.CrearConexion();
            string ListarParametros = "SELECT * FROM Parametros";
            SqlCommand cmdListarParametros = new SqlCommand(ListarParametros, cn);
            try
            {
                manejadorConexion.AbrirConexion(cn);
                SqlDataReader dr = cmdListarParametros.ExecuteReader();
                List<Parametros> listaParametros = new List<Parametros>();
                while (dr.Read())
                {

                    Parametros nuevoParametro = new Parametros()
                    {
                        IdParametro = dr.GetInt32(0),
                        TipoParametro = dr.GetString(1),
                        DescParametro = dr.GetString(2),
                        ValorParametro = dr.GetString(3)

                    };
                    listaParametros.Add(nuevoParametro);



                }
                return listaParametros;
            }
            catch (Exception)
            {
                return null;

                throw;
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }


        }// FUNCIONA  OK conecta bien con BD

        public Parametros FindByID(int id)// FUNCIONA  OK conecta bien con BD
        {
            Parametros buscado = null;
            cn = manejadorConexion.CrearConexion();
            String cadenaConsulta = "SELECT * FROM Parametros where idParametro = @idobj";
            SqlCommand sqlCBuscandoId = new SqlCommand(cadenaConsulta, cn);
            sqlCBuscandoId.Parameters.AddWithValue("@idobj", id);
            try
            {
                manejadorConexion.AbrirConexion(cn);
                SqlDataReader dr = sqlCBuscandoId.ExecuteReader();
                while (dr.Read())
                {
                    buscado = new Parametros()
                    {
                        IdParametro = dr.GetInt32(0),
                        TipoParametro = dr.GetString(1),
                        DescParametro = dr.GetString(2),
                        ValorParametro = dr.GetString(3)
                    };
                }
                return buscado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "ERROR al buscar PARAMETRO por ID");
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }

        }

        public bool Remove(Parametros obj)// FUNCIONA  OK conecta bien con BD
        {
            bool resultado = false;
            if (obj == null) return resultado;
            cn = manejadorConexion.CrearConexion();
            string strDeletePar = "DELETE FROM Parametros where idParametros = @id";
            SqlCommand cmdDeleteandoPar = new SqlCommand(strDeletePar, cn);
            cmdDeleteandoPar.Parameters.AddWithValue("@id", obj.IdParametro);
            try
            {
                manejadorConexion.AbrirConexion(cn);
                int filaAfectada = cmdDeleteandoPar.ExecuteNonQuery();
                if (filaAfectada == 1) resultado = true;
                return resultado;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return resultado;
                throw;
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }

        }

        public bool Update(Parametros obj)
        {
            bool resultadoUpdate = false;
            bool updatear = false;
            if (obj == null) return resultadoUpdate;
            
            Parametros par = BuscarParametroPorDescripcion(obj.DescParametro);

            if (par !=null)
            {
                if (par.IdParametro != obj.IdParametro)
                {
                    return updatear;
                }
                else if (par.IdParametro == obj.IdParametro) updatear = true;
            }
            else
            {
                updatear = true;
            }

            if (updatear != true) return resultadoUpdate;
            //si se valida todo lo anterior recien paso a esto

            cn = manejadorConexion.CrearConexion();
            string strModificarPar = "UPDATE Parametros SET tipo = @tipoObj, Descripcion = @descObj, Valor = @valorObj" +
                " where idParametros = @idParObj";
            SqlCommand cmdModificarPar = new SqlCommand(strModificarPar, cn);
            cmdModificarPar.Parameters.AddWithValue("@tipoObj", obj.TipoParametro);
            cmdModificarPar.Parameters.AddWithValue("@descObj", obj.DescParametro);
            cmdModificarPar.Parameters.AddWithValue("@valorObj", obj.ValorParametro);
            cmdModificarPar.Parameters.AddWithValue("@idParObj", obj.IdParametro);
            SqlTransaction trn = null;
            
            try
            {
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmdModificarPar.Transaction = trn;
                int filaAfectada = cmdModificarPar.ExecuteNonQuery();
                if (filaAfectada == 1) resultadoUpdate = true;
                trn.Commit();
                return resultadoUpdate;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return resultadoUpdate;
                throw;
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
        }

        public Parametros BuscarParametroPorDescripcion(string textoDescripcion)
        {
            Parametros parametroBuscado = null;
            if (textoDescripcion == "") return parametroBuscado;
            cn = manejadorConexion.CrearConexion();
            string cadenaBuscDescParametro = "SELECT * FROM Parametros p WHERE" +
                "  p.descripcion = @txtDesc";
            SqlCommand cmdBuscoDescPar = new SqlCommand(cadenaBuscDescParametro, cn);
            cmdBuscoDescPar.Parameters.AddWithValue("@txtDesc", textoDescripcion);
            manejadorConexion.AbrirConexion(cn);
            try
            {
                SqlDataReader dr = cmdBuscoDescPar.ExecuteReader();


                while (dr.Read())
                {
                    return new Parametros
                    {
                        IdParametro = dr.GetInt32(0),
                        TipoParametro = dr.GetString(1),
                        DescParametro = dr.GetString(2),
                        ValorParametro = dr.GetString(3)
                    };
                }

                return parametroBuscado;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
        }





    }
}
