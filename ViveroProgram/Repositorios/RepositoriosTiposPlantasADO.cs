using Dominio.EntidadesNegocio;
using Dominio.InterfacesRepositorios;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Dominio.OtrasInterfaces;
using System;

namespace Repositorios
{
    public class RepositoriosTiposPlantasADO : IRepositorioTiposPlantas
    {
        private Conexion manejadorConexion = new Conexion();
        private SqlConnection cn;

        public bool Add(TipoPlanta obj) // FUNCIONA  OK conecta bien con BD
        {
            if(!obj.Validar()) return false;

            // verificar los datos del objeto segun reglas de negocio.
            //if (!obj.ValidarLargoDelTxt(obj.DescTipoPlanta) && !obj.VerificarRangoAzEspaciosEmbe(obj.NomTipoPlanta)) return false;

            //Conexion manejadorConexion = new Conexion();
            //SqlConnection cn = (SqlConnection)manejadorConexion.CrearConexion();

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {

                string sqlInsert = @"INSERT INTO TipoPlanta VALUES(@NomTipoPlanta,@DescTipoPlanta) 
                                    SELECT CAST(Scope_Identity() as int)";
                SqlCommand cmdAgregarTipoPlanta = new SqlCommand(sqlInsert, cn);
                cmdAgregarTipoPlanta.Parameters.AddWithValue("@NomTipoPlanta", obj.NomTipoPlanta);
                cmdAgregarTipoPlanta.Parameters.AddWithValue("@DescTipoPlanta", obj.DescTipoPlanta);

                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();    //BeginTransaction = Comienza la transaccion
                cmdAgregarTipoPlanta.Transaction = trn; // le asigna que quiere "hacer"

                int idGenerado = (int)cmdAgregarTipoPlanta.ExecuteScalar();
                // te devuelve el id de la transaccion, que seria el ultimo id de esa tabla.
                // trabaja en conjunto con el SELECT CAST(Scope_Identity() as int

                trn.Commit(); 
                // despues de asignada la transaccion que se va hacer,
                // comfirma que los datos estan bien, van a ser realizados. "Compromiso"

                return true;
            }
            catch (Exception ex)
            {
                trn.Rollback(); //deshace los cambios hechos
                return false;
                throw;
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
        }



        public TipoPlanta BuscarTipoPlantaPorNombre(string nombreTipo)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<TipoPlanta> FindAll()// FUNCIONA  OK conecta bien con BD
        {
            try
            {
                cn = manejadorConexion.CrearConexion();
                SqlCommand cmdTipoPlantaListar = new SqlCommand("SELECT * FROM TipoPlanta", cn);
                manejadorConexion.AbrirConexion(cn);
                SqlDataReader dr = cmdTipoPlantaListar.ExecuteReader();
                if (dr.HasRows)
                {
                    List<TipoPlanta> lstTP = new List<TipoPlanta>();
                    while (dr.Read())
                    {
                        lstTP.Add(new TipoPlanta
                        {
                            Id = (int)dr["idTipoPlanta"],
                            NomTipoPlanta = dr["nomTipoPlanta"].ToString(),
                            DescTipoPlanta = dr["descTipoPlanta"].ToString()

                        });

                    }
                    return lstTP;
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

        public TipoPlanta FindByID(int id)// FUNCIONA  OK conecta bien con BD
        {
            try
            {

                cn = manejadorConexion.CrearConexion();
                SqlCommand cmdlistId = new SqlCommand(@"SELECT * FROM TipoPlanta
                                                        where idTipoPlanta=@id", cn);
                cmdlistId.Parameters.AddWithValue("@id", id);
                if (manejadorConexion.AbrirConexion(cn))
                {
                    SqlDataReader dr = cmdlistId.ExecuteReader();
                    while (dr.Read())
                    {
                        return new TipoPlanta
                        {
                            Id = (int)dr["idTipoPlanta"],
                            NomTipoPlanta = dr["nomTipoPlanta"].ToString(),
                            DescTipoPlanta = dr["descTipoPlanta"].ToString()
                        };
                    }
                }
                return null;

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


        // Metodo CRUD - REMOVE 
        public bool Remove(TipoPlanta obj)// FUNCIONA  OK conecta bien con BD
        {
            bool valido = false;
            if (obj == null) return valido;
            if (obj.Validar()) return valido;
            try
            {
                cn = manejadorConexion.CrearConexion(); // creamos conexion
                SqlCommand cmdRemoveTP = new SqlCommand(@"DELETE FROM TipoPlanta
                                                    where idTipoPlanta=@Id", cn); 

                cmdRemoveTP.Parameters.AddWithValue("@Id", obj.Id);
               
                if (manejadorConexion.AbrirConexion(cn))
                {
                    int filaAEliminar = cmdRemoveTP.ExecuteNonQuery(); // si esta fila con el id solicitado, se elimina.
                    if (filaAEliminar == 1) valido= true;
 
                }
                return valido;
            }
            catch (Exception ex)
            {
               
                throw new Exception(ex.Message + "No pudo removerse el campo seleccionado");
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }

           
        }

        public bool Update(TipoPlanta obj)// FUNCIONA  OK conecta bien con BD
        {
            bool validar = false;
            if (obj == null) return validar;

            // verificar los datos del objeto segun reglas de negocio.
            if (!obj.Validar()) return validar;
            // if (!obj.ValidarLargoDelTxt(obj.DescTipoPlanta) && !obj.VerificarRangoAzEspaciosEmbe(obj.NomTipoPlanta)) return validar;
            SqlTransaction trn = null;
            try
            {
                cn = manejadorConexion.CrearConexion();

                SqlCommand cmdUpdateTP = new SqlCommand(@"UPDATE TipoPlanta SET nomTipoPlanta=@nomTipoPlanta,
                                                        descTipoPlanta=@descTipoPlanta ;", cn);

                cmdUpdateTP.Parameters.AddWithValue("@nomTipoPlanta", obj.NomTipoPlanta);
                cmdUpdateTP.Parameters.AddWithValue("@descTipoPlanta", obj.DescTipoPlanta);
                

               if(manejadorConexion.AbrirConexion(cn))
                {
                    trn = cn.BeginTransaction();
                    cmdUpdateTP.Transaction = trn;
                    int filaAfectada = cmdUpdateTP.ExecuteNonQuery();
                    validar = filaAfectada == 1;
                    trn.Commit();
                }
            }
            catch (Exception ex)
            {
                if (trn != null) trn.Rollback();
                    throw new Exception(ex.Message + "No pudo actualizarse los campos");
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
           




            return validar;
        }
        

    }

}

