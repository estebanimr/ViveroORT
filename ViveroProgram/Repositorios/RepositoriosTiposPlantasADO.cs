using Dominio.EntidadesNegocio;
using Dominio.InterfacesRepositorios;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System;

namespace Repositorios
{
    public class RepositoriosTiposPlantasADO : IRepositorioTiposPlantas
    {
        private Conexion manejadorConexion = new Conexion();
        private SqlConnection cn;

        public bool Add(TipoPlanta obj) // FUNCIONA  OK conecta bien con BD
        {
            //traerTopes()
            if (!obj.Validar()) return false;
            if (!obj.ValidarLargoDescTP(ParametrosValidacionLargoDesc())) return false;
            if(BuscarTipoPlantaPorNombre(obj.NomTipoPlanta) != null) return false;

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
                throw new Exception(ex.Message + "ERROR al AGREGAR un nuevo Tipo de Planta");
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
        }

        public TipoPlanta BuscarTipoPlantaPorNombre(string nombreTipo)
        {
            try
            {

                cn = manejadorConexion.CrearConexion();
                SqlCommand cmdlistId = new SqlCommand(@"SELECT * FROM TipoPlanta
                                                        where nomTipoPlanta=@nomTipoPlanta", cn);
                cmdlistId.Parameters.AddWithValue("@nomTipoPlanta", nombreTipo);
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
            catch (Exception ex)
            {

                throw new Exception(ex.Message + "ERROR al buscar Tipo Planta por NOMBRE");
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
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
                    List<TipoPlanta> listaTP = new List<TipoPlanta>();
                    while (dr.Read())
                    {
                        listaTP.Add(new TipoPlanta
                        {
                            Id = (int)dr["idTipoPlanta"],
                            NomTipoPlanta = dr["nomTipoPlanta"].ToString(),
                            DescTipoPlanta = dr["descTipoPlanta"].ToString()

                        });

                    }
                    return listaTP;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message + "ERROR. No pudo listar los tipos de plantas");
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "ERROR al buscar Tipo Planta por ID");
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

            try
            {
                cn = manejadorConexion.CrearConexion();
                // creamos conexion
                //if(buscarTipoPlantaxNombre) 

                // Busco si tipo planta esta en alguna planta
                string cmdExisteTipoEnPlanta = "SELECT COUNT(p.idTipoPlantaFK) FROM Planta" +
                    " p WHERE p.idTipoPlantaFK = @idTipoPlanta";
                SqlCommand cmdTipoPlantaId = new SqlCommand(cmdExisteTipoEnPlanta, cn);
                manejadorConexion.AbrirConexion(cn);
                cmdTipoPlantaId.Parameters.AddWithValue("@idTipoPlanta", obj.Id);

                int tipoPlantaBuscada = (int)cmdTipoPlantaId.ExecuteScalar();
                //Console.WriteLine(tipoPlantaBuscada);
                //si encuentro TipoPlanta en Planta NO la elimino
                if (tipoPlantaBuscada > 0)
                {
                    return valido;
                }

                // Si no existe planta que use el tipo, removemos el tipo
                string stringRemoveTP = "DELETE FROM TipoPlanta WHERE idTipoPlanta=@Id";
                SqlCommand cmdRemoveTP = new SqlCommand(stringRemoveTP, cn);
                cmdRemoveTP.Parameters.AddWithValue("@Id", obj.Id);

                int filaAEliminar = cmdRemoveTP.ExecuteNonQuery();
                if (filaAEliminar == 1) valido = true;


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
            bool resUpdate = false;

            if (obj != null)
            {
                TipoPlanta buscada = BuscarTipoPlantaPorNombre(obj.NomTipoPlanta);

                // busco un objeto (buscada) con un nombre, con el nombre del objeto que me llego (obj)
                if (buscada != null) // si buscada existe o tiene algo, analizo
                {
                    if (buscada.Id != obj.Id) // si el id de buscada es DISTINTO al obj.id que me llego
                    {                        // entonces
                        return validar;     // retorno false
                    }
                    else if (buscada.Id == obj.Id) // si el id de buscada es IGUAL al obj.id que me llego
                    {                             // entonces
                        validar = true;          // retorno true
                    } 
                }
                else
                {
                    validar = true; // si el nombre buscada es nulo retorno true
                }
            }
            else 
            { 
                return validar; 
            }


            // verificar los datos del objeto segun reglas de negocio.
            if (!obj.Validar()) return resUpdate;
            if (!obj.ValidarLargoDescTP(ParametrosValidacionLargoDesc())) return resUpdate;
            // if (!obj.ValidarLargoDelTxt(obj.DescTipoPlanta) && !obj.VerificarRangoAzEspaciosEmbe(obj.NomTipoPlanta)) return validar;
            SqlTransaction trn = null;
            try
            {
                cn = manejadorConexion.CrearConexion();

                SqlCommand cmdUpdateTP = new SqlCommand(@"UPDATE TipoPlanta SET nomTipoPlanta=@nomTipoPlanta,
                                                        descTipoPlanta=@descTipoPlanta WHERE idTipoPlanta = @idObj;", cn);

                cmdUpdateTP.Parameters.AddWithValue("@nomTipoPlanta", obj.NomTipoPlanta);
                cmdUpdateTP.Parameters.AddWithValue("@descTipoPlanta", obj.DescTipoPlanta);
                cmdUpdateTP.Parameters.AddWithValue("@idObj", obj.Id);

                if (manejadorConexion.AbrirConexion(cn))
                {
                    trn = cn.BeginTransaction();
                    cmdUpdateTP.Transaction = trn;
                    int filaAfectada = cmdUpdateTP.ExecuteNonQuery();
                    resUpdate = filaAfectada == 1;
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

            return resUpdate;
        }


        public List<Parametros> ParametrosValidacionLargoDesc()
        {
            cn = manejadorConexion.CrearConexion();
            string traerParametros = "select * from Parametros" +
                " where Parametros.Tipo LIKE 'ValidarDescTP'";
            SqlCommand cmdTraerDAtosPArametros = new SqlCommand(traerParametros, cn);
            List<Parametros> ListaParametros = new List<Parametros>();
            try
            {
                manejadorConexion.AbrirConexion(cn);
                SqlDataReader nombreReader = cmdTraerDAtosPArametros.ExecuteReader();
                while (nombreReader.Read()) // procesa las filas hasta que read sea false
                {
                    Parametros nuevoP = new Parametros()
                    {
                        IdParametro = nombreReader.GetInt32(0),
                        TipoParametro = nombreReader.GetString(1),
                        DescParametro = nombreReader.GetString(2),
                        ValorParametro = nombreReader.GetString(3)
                    };
                    ListaParametros.Add(nuevoP);
                }
                return ListaParametros;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }

        }// FUNCIONA  OK conecta bien con BD

        //ParametrosValidacionLargoDesc()  solo esta en ADO no en MEMORIA



    }

}

