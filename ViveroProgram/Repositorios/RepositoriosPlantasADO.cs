using Dominio.EntidadesNegocio;
using Dominio.InterfacesRepositorios;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System;


namespace Repositorios
{
    public class RepositoriosPlantasADO : IRepositorioPlantas
    {
        private Conexion manejadorConexion = new Conexion();
        private SqlConnection cn;

        public bool Add(Planta obj)
        {
            bool resultado = false;
            if (!obj.Validar()) return false;
            if (!obj.ValidarLargoDescPlanta(ParametrosValidacionLargoDesc())) return false;
            if (BuscarPlantaPorNombreCientifico(obj.NombreCientifico)) return false; // Valida que NombreCientifico es Unico

            // Validaciones de CuidadosPlanta
            if (!obj.CuidadosPlanta.ValidarCantidad(obj.CuidadosPlanta.CantidadFrecRiego)) return resultado;
            if (!obj.CuidadosPlanta.ValidarUnidad(obj.CuidadosPlanta.UnidadFrecRiego)) return resultado;
            if (!obj.CuidadosPlanta.ValidarTemperatura(obj.CuidadosPlanta.Temperatura)) return resultado;


            cn = manejadorConexion.CrearConexion();
            int fkAltura;
            int fkCuidado;
            int idPlanta;

            string strAddAltura = @"INSERT INTO Parametros VALUES(@Tipo, @Descripcion, @Valor) 
                                    SELECT CAST(Scope_Identity() as int)";
            SqlCommand cmdAdd = new SqlCommand(strAddAltura, cn);
            // Defino SqlCommand "cmdAdd" por primera y unica vez aca
            cmdAdd.Parameters.AddWithValue("@Tipo", "Altura");
            cmdAdd.Parameters.AddWithValue("@Descripcion", $"Altura de: {obj.NombreCientifico}");
            cmdAdd.Parameters.AddWithValue("@Valor", obj.AlturaMaxima.ValorParametro);
            // Valor en cm (solo num en string)
            // El valor lo manda el usuario en int



            //======== podriamos plantear el tema de la foto ========


            SqlTransaction trn = null;
            try
            {
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmdAdd.Transaction = trn;
                fkAltura = (int)cmdAdd.ExecuteScalar();
                //este primer execute scalar va a ser para el add de altura y capturar el id de esa altura

                if (fkAltura < 1) return resultado; // resultado = false

                //Arranca add Cuidado de la planta
                string strAddCuidado = @"INSERT INTO CuidadosPlantas 
                                 VALUES(@idTipoIluminacion, @cantidadFrecRiego, @unidadFrecRiego, @temperatura)
                                 SELECT CAST(Scope_Identity() as int)";
                cmdAdd.Parameters.AddWithValue("@idTipoIluminacion", obj.CuidadosPlanta.TipoIluminacion.IdTipoIluminacion);
                cmdAdd.Parameters.AddWithValue("@cantidadFrecRiego", obj.CuidadosPlanta.CantidadFrecRiego);
                cmdAdd.Parameters.AddWithValue("@unidadFrecRiego", obj.CuidadosPlanta.UnidadFrecRiego);
                cmdAdd.Parameters.AddWithValue("@temperatura", obj.CuidadosPlanta.Temperatura);
                cmdAdd.CommandText = (strAddCuidado);//Cambio el commandText al strAddCuidado
                fkCuidado = (int)cmdAdd.ExecuteScalar();
                //Termina add Cuidado de la planta

                if (fkCuidado < 1) return resultado; // resultado = false

                //Arranca Add PLANTA
                string strAddPlanta = @"INSERT INTO planta 
                                    VALUES (@idTipoPlantaFK, 
                                            @nombreCientifico,
                                            @descripcionPlanta,
                                            @idCuidadosFK,
                                            @ambiente,
                                            @alturaMaximaFK,
                                            @nombresVulgares)
                            SELECT CAST(Scope_Identity() as int)";

                cmdAdd.Parameters.AddWithValue("@idTipoPlantaFK", obj.Tipo.Id);
                cmdAdd.Parameters.AddWithValue("@nombreCientifico", obj.NombreCientifico);
                cmdAdd.Parameters.AddWithValue("@descripcionPlanta", obj.DescripcionPlanta);
                cmdAdd.Parameters.AddWithValue("@idCuidadosFK", fkCuidado);//uso el fk de cuidado capturado
                cmdAdd.Parameters.AddWithValue("@ambiente", obj.TipoAmbiente.ToString());
                cmdAdd.Parameters.AddWithValue("@alturaMaximaFK", fkAltura);//uso el fk de altura capturado

                cmdAdd.Parameters.AddWithValue("@nombresVulgares", obj.NombresVulgares);
                cmdAdd.CommandText = (strAddPlanta);//Cambio el commandText al strAddPlanta
                idPlanta = (int)cmdAdd.ExecuteScalar();
                //Termina add PLANTA


                //Arranca Add FOTOS
                string strAddFotos = @"INSERT INTO FOTO  
                                       VALUES(@nombreFoto,@idPlantaFk)
                                       SELECT CAST(Scope_Identity() as int)";
                cmdAdd.CommandText = (strAddFotos);
                foreach (Foto f in obj.FotosDePlanta)
                {
                    cmdAdd.Parameters.AddWithValue("@nombreFoto", f.NombreFoto);
                    cmdAdd.Parameters.AddWithValue("@idPlantaFk", idPlanta);
                    cmdAdd.ExecuteNonQuery();
                    cmdAdd.Parameters.Clear();
                }
                //Termina Add FOTOS
                trn.Commit();
                resultado = true;

                return resultado; //si el if filasAfectadas es == 0, resultado va a ser  = false
            }
            catch (Exception ex)
            {
                trn.Rollback();
                Console.WriteLine(ex);
                throw new Exception(ex.Message);
            }//+ "ERROR al buscar TODAS las PLANTAS"
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
        }



        public IEnumerable<Planta> FindAll()
        {
            cn = manejadorConexion.CrearConexion();
            List<Planta> todasLasPlantas = new List<Planta>();
            string strPlantas = @"SELECT * from planta as pla 
                                        left join TipoPlanta as tp on tp.idTipoPlanta = pla.idTipoPlantaFK
                                        left join CuidadosPlantas as cp on cp.idCuidadosPlanta = pla.idCuidadosFK
                                        left join TipoIluminacion as ti on ti.idTipoIluminacion = cp.idTipoIluminacion
                                        left join Parametros as par on par.idParametro = pla.alturaMaximaFK
                                        ";
            SqlCommand cmdPlantas = new SqlCommand(strPlantas, cn);
            try
            {
                manejadorConexion.AbrirConexion(cn);
                SqlDataReader dr = cmdPlantas.ExecuteReader();
                while (dr.Read())
                {
                    Planta nuevaPlanta = CrearPlanta(dr);
                    nuevaPlanta.CuidadosPlanta = CrearCuidadosPlanta(dr);
                    nuevaPlanta.AlturaMaxima = CrearAlturaMaxima(dr);
                    nuevaPlanta.Tipo = CrearNuevoTP(dr);
                    nuevaPlanta.FotosDePlanta = CrearListaDeFotos2(nuevaPlanta.IdPlanta);
                    todasLasPlantas.Add(nuevaPlanta);
                }
                return todasLasPlantas;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(ex.Message + "ERROR al buscar TODAS las PLANTAS");
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
        }

        public Planta FindByID(int id)
        {
            try
            {
                cn = manejadorConexion.CrearConexion();
                string sqlFindByID = @"select * from planta as pla 
                                        left join TipoPlanta as tp on tp.idTipoPlanta = pla.idTipoPlantaFK
                                        left join CuidadosPlantas as cp on cp.idCuidadosPlanta = pla.idCuidadosFK
                                        left join TipoIluminacion as ti on ti.idTipoIluminacion = cp.idTipoIluminacion
                                        left join Parametros as par on par.idParametro = pla.alturaMaximaFK

                                       
                                        
                                         WHERE pla.idPlanta = @idPlanta";

                SqlCommand cmdFindPlanta = new SqlCommand(sqlFindByID, cn);
                cmdFindPlanta.Parameters.AddWithValue("@idPlanta", id);

                if (manejadorConexion.AbrirConexion(cn))
                {
                    SqlDataReader dr = cmdFindPlanta.ExecuteReader();
                    Planta plantaEncontrada = null;
                    while (dr.Read())
                    {
                        plantaEncontrada = CrearPlanta(dr);
                        plantaEncontrada.CuidadosPlanta = CrearCuidadosPlanta(dr);
                        plantaEncontrada.AlturaMaxima = CrearAlturaMaxima(dr);
                        plantaEncontrada.Tipo = CrearNuevoTP(dr);
                        plantaEncontrada.FotosDePlanta = CrearListaDeFotos2(plantaEncontrada.IdPlanta);
                    }
                    return plantaEncontrada;
                }
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message + "ERROR al buscar  Planta por ID");
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
            return null;
        }



        public bool Remove(Planta obj)
        {
            bool valido = false;
            if (obj == null) return valido;

            cn = manejadorConexion.CrearConexion();

            string strRemoveFotos = @"DELETE FROM Foto 
                                      WHERE idPlantaFK = @idPlanta";
            SqlCommand cmdRemovePlanta = new SqlCommand(strRemoveFotos, cn);
            cmdRemovePlanta.Parameters.AddWithValue("@IdPlanta", obj.IdPlanta);
            // @idPlanta se  setea por unica vez aca, y se puede reutilizar en cualquier parte
            // si no se limpian los parametros
            // por eso podemos usarlo en el momento de remover la planta  * ACA

            SqlTransaction trn = null;
            try
            {
                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmdRemovePlanta.Transaction = trn;
                int filasAfectadasFotos = cmdRemovePlanta.ExecuteNonQuery();
                if (filasAfectadasFotos < 1) return false;

                string strRemovePlanta = @"DELETE FROM planta 
                                           WHERE idPlanta = @IdPlanta;"; // * ACA
                cmdRemovePlanta.CommandText = (strRemovePlanta);
                //cmdRemovePlanta.Parameters.AddWithValue("@IdPlanta", obj.IdPlanta);
                int filasAfectadasPlanta = cmdRemovePlanta.ExecuteNonQuery();

                if (filasAfectadasPlanta < 1) return false;

                //cmdRemovePlanta.Parameters.Clear();
                //NO ES NECESARIO porque se cambia el Command Text y usa otros parametros
                string strRemoveFinal = @"DELETE FROM CuidadosPlantas WHERE CuidadosPlantas.idCuidadosPlanta = @idCiudadosFK; 
                                              DELETE FROM Parametros WHERE Parametros.idParametro = @idAltura;";
                cmdRemovePlanta.CommandText = (strRemoveFinal);
                cmdRemovePlanta.Parameters.AddWithValue("@idCiudadosFK", obj.CuidadosPlanta.idCuidadosPlanta);
                cmdRemovePlanta.Parameters.AddWithValue("@idAltura", obj.AlturaMaxima.IdParametro);
                //cambie el command string del Sqlcommand pero NO la conexion
                int filasAfectadasFinal = cmdRemovePlanta.ExecuteNonQuery();
                if (filasAfectadasFinal < 1) return false;
                trn.Commit();

                valido = true;
                return valido;
            }
            catch (Exception ex)
            {
                trn.Rollback();
                Console.WriteLine(ex);
                throw new Exception(ex.Message + "No pudo removerse el campo seleccionado");
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
        }


        // CHEQUEAR ----->>> SIN PROBAR
        public bool Update(Planta obj)
        {
            //bool esValido = false;
            //bool resUpdate = false;
            //if (obj == null) return esValido;
            //if (obj != null)
            //{
            //    Planta buscada = BuscarPlantaPorNombreCientificoDevuelvePlanta(obj.NombreCientifico);

            //    // busco un objeto (buscada) con un nombre con el nombre del objeto que me llego (obj)
            //    if (buscada != null) // si buscada existe o tiene algo, analizo
            //    {
            //        if (buscada.IdPlanta != obj.IdPlanta) // si el id de buscada es DISTINTO al obj.id que me llego
            //        {                        // entonces
            //            return esValido;     // retorno false
            //        }
            //        else if (buscada.IdPlanta == obj.IdPlanta) // si el id de buscada es IGUAL al obj.id que me llego
            //        {                             // entonces
            //            esValido = true;          // retorno true
            //        }
            //    }
            //    else
            //    {
            //        esValido = true; // si el nombre buscada es nulo retorno true
            //    }
            //}
            //else
            //{
            //    return esValido;
            //}


            //// verificar los datos del objeto segun reglas de negocio.
            //if (!obj.Validar()) return false;
            //if (!obj.ValidarLargoDescPlanta(ParametrosValidacionLargoDesc())) return false;

            //// Validaciones de CuidadosPlanta
            //if (!obj.CuidadosPlanta.ValidarCantidad(obj.CuidadosPlanta.CantidadFrecRiego)) return esValido;
            //if (!obj.CuidadosPlanta.ValidarUnidad(obj.CuidadosPlanta.UnidadFrecRiego)) return esValido;
            //if (!obj.CuidadosPlanta.ValidarTemperatura(obj.CuidadosPlanta.Temperatura)) return esValido;
            //SqlTransaction trn = null;

            //try
            //{
            //    cn = manejadorConexion.CrearConexion();
            //    // UPDATE PARAMETROS

            //    // ====================================
            //    //          SEGUIR ACA ============================
            //    // ====================================
            //    string strUpdateAltura = @"UPDATE Parametros
            //                               SET @Tipo, @Descripcion, @Valor) 
            //                               SELECT CAST(Scope_Identity() as int)";

            //    cmdUpdatePlanta.Parameters.AddWithValue("@Tipo", "Altura");
            //    cmdUpdatePlanta.Parameters.AddWithValue("@Descripcion", $"Altura de: {obj.NombreCientifico}");
            //    cmdUpdatePlanta.Parameters.AddWithValue("@Valor", obj.AlturaMaxima.ValorParametro);

            //    SqlCommand cmdUpdatePlanta = new SqlCommand(strUpdateAltura, cn);

            //    // UPDATE CUIDADOS-PLANTA              
            //    string strUpdateCuidado = @"INSERT INTO CuidadosPlantas 
            //                     VALUES(@idTipoIluminacion, @cantidadFrecRiego, @unidadFrecRiego, @temperatura)
            //                     SELECT CAST(Scope_Identity() as int)";
            //    cmdUpdatePlanta.Parameters.AddWithValue("@idTipoIluminacion", obj.CuidadosPlanta.TipoIluminacion.IdTipoIluminacion);
            //    cmdUpdatePlanta.Parameters.AddWithValue("@cantidadFrecRiego", obj.CuidadosPlanta.CantidadFrecRiego);
            //    cmdUpdatePlanta.Parameters.AddWithValue("@unidadFrecRiego", obj.CuidadosPlanta.UnidadFrecRiego);
            //    cmdUpdatePlanta.Parameters.AddWithValue("@temperatura", obj.CuidadosPlanta.Temperatura);

            //    // cmdUpdatePlanta.CommandText = (strUpdateCuidado); //Cambio el commandText al strUpdateCuidado


            //    // UPDATE PLANTA
            //    string strUpdatePlanta = @"UPDATE Planta 
            //                               SET nombreCientifico = @nombreCientifico, 
            //                                   descripcionPlanta = @descripcionPlanta, 
            //                                   ambiente = @ambiente,
            //                                   foto = @foto,
            //                                   nombresVulgares = @nombresVulgares,
            //                               WHERE idPlanta = @idPlanta";


            //    // FALTA TI y CP

            //    // cmdUpdatePlanta.Parameters.AddWithValue("@idTipoPlantaPK", obj.Tipo.Id); // TipoPlanta FK
            //    cmdUpdatePlanta.Parameters.AddWithValue("@nombreCientifico", obj.NombreCientifico);
            //    cmdUpdatePlanta.Parameters.AddWithValue("@descripcionPlanta", obj.DescripcionPlanta);
            //    // cmdUpdatePlanta.Parameters.AddWithValue("@idCuidadosFK", obj.CuidadosPlanta.idCuidadosPlanta); // CuidadosPlantaFK
            //    cmdUpdatePlanta.Parameters.AddWithValue("@ambiente", obj.TipoAmbiente); //TipoAmbiente
            //    // cmdUpdatePlanta.Parameters.AddWithValue("@alturaMaximaFK", obj.AlturaMaxima.IdParametro); // ParametrosFK (Altura Maxima)
            //    //cmdUpdatePlanta.Parameters.AddWithValue("@foto", obj.NombreFoto);
            //    cmdUpdatePlanta.Parameters.AddWithValue("@nombresVulgares", obj.NombresVulgares);

            //    cmdUpdatePlanta.CommandText = (strUpdatePlanta);

            //    if (manejadorConexion.AbrirConexion(cn))
            //    {
            //        trn = cn.BeginTransaction();
            //        cmdUpdatePlanta.Transaction = trn;
            //        int filaAfectada = cmdUpdatePlanta.ExecuteNonQuery();
            //        esValido = filaAfectada == 1;
            //        trn.Commit();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    if (trn != null) trn.Rollback();
            //    throw new Exception(ex.Message + "No se pudo realizar UPDATE de los campos de PLANTA");
            //}
            //finally
            //{
            //    manejadorConexion.CerrarConexion(cn);
            //}
            return false;
        }





        private Planta CrearPlanta(SqlDataReader read)
        {
            Planta plantaCreada = new Planta();
            plantaCreada.IdPlanta = read.GetInt32(read.GetOrdinal("idPlanta"));
            plantaCreada.NombreCientifico = read.GetString(read.GetOrdinal("nombreCientifico"));
            plantaCreada.NombresVulgares = read.GetString(read.GetOrdinal("nombresVulgares"));
            plantaCreada.DescripcionPlanta = read.GetString(read.GetOrdinal("descripcionPlanta"));



            string ambiente = read.GetString(read.GetOrdinal("ambiente"));
            if (ambiente == "exterior") plantaCreada.TipoAmbiente = Planta.Ambiente.exterior;
            if (ambiente == "interior") plantaCreada.TipoAmbiente = Planta.Ambiente.interior;
            if (ambiente == "mixta") plantaCreada.TipoAmbiente = Planta.Ambiente.mixta;

            return plantaCreada;
        }


        private TipoPlanta CrearNuevoTP(SqlDataReader read)
        {
            TipoPlanta nuevoTP = new TipoPlanta()
            {
                Id = read.GetInt32(read.GetOrdinal("idTipoPlanta")),
                NomTipoPlanta = read.GetString(read.GetOrdinal("nomTipoPlanta")),
                DescTipoPlanta = read.GetString(read.GetOrdinal("descTipoPlanta"))
            };

            return nuevoTP;
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

        private List<Foto> CrearListaDeFotos(SqlDataReader dr)// VERIFICAR
        {
            List<Foto> lstFoto = new List<Foto>();
            while (dr.Read())
            {
                Foto nuevaFoto = new Foto()
                {
                    IdFoto = dr.GetInt32(dr.GetOrdinal("idFoto")),
                    NombreFoto = dr.GetString(dr.GetOrdinal("nombreFoto"))
                };
                lstFoto.Add(nuevaFoto);
            }
            return lstFoto;
        }

        private List<Foto> CrearListaDeFotos2(int id)// VERIFICAR
        {
            List<Foto> lstFotos = new List<Foto>();

            cn = manejadorConexion.CrearConexion();
            string sql = "SELECT * FROM Foto WHERE idPlantaFK=" + id;
            SqlCommand cmdFoto = new SqlCommand(sql, cn);

            try
            {
                manejadorConexion.AbrirConexion(cn);
                SqlDataReader dr = cmdFoto.ExecuteReader();

                while (dr.Read())
                {
                    Foto f = new Foto()
                    {
                        IdFoto = (int)dr["idFoto"],
                        NombreFoto = dr["nombreFoto"].ToString()

                    };

                    lstFotos.Add(f);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }

            return lstFotos;
        }


        private Parametros CrearAlturaMaxima(SqlDataReader read)
        {
            Parametros nuevaAltura = new Parametros()
            {
                IdParametro = read.GetInt32(read.GetOrdinal("idParametro")),
                TipoParametro = read.GetString(read.GetOrdinal("tipo")),
                DescParametro = read.GetString(read.GetOrdinal("descripcion")),
                ValorParametro = read.GetString(read.GetOrdinal("valor"))
            };
            return nuevaAltura;
        }


        public List<Parametros> ParametrosValidacionLargoDesc()
        {
            cn = manejadorConexion.CrearConexion();
            string strParametros = @"SELECT valor FROM Parametros p
                                     WHERE p.Tipo LIKE 'ValidarDescPlanta'";
            SqlCommand cmdObtenerParametros = new SqlCommand(strParametros, cn);
            List<Parametros> listaParametros = new List<Parametros>();
            try
            {
                manejadorConexion.AbrirConexion(cn);
                SqlDataReader dr = cmdObtenerParametros.ExecuteReader();

                while (dr.Read())
                {
                    Parametros nuevoP = new Parametros()
                    {
                        ValorParametro = dr.GetString(0)
                    };
                    listaParametros.Add(nuevoP);
                }
                string alto = listaParametros[0].ValorParametro;
                string bajo = listaParametros[1].ValorParametro;
                return listaParametros;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + "ERROR al traer los valores del PARAMETRO");
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
        }




        // ============================= BUSQUEDAS REQUERIDAS =====================================
        public IEnumerable<Planta> BuscarPlantaPorTexto(string texto)
        {
            try
            {
                cn = manejadorConexion.CrearConexion();
                string sqlConsulta = @" select * from planta as pla 
                                        left join TipoPlanta as tp on tp.idTipoPlanta = pla.idTipoPlantaFK
                                        left join CuidadosPlantas as cp on cp.idCuidadosPlanta = pla.idCuidadosFK
                                        left join TipoIluminacion as ti on ti.idTipoIluminacion = cp.idTipoIluminacion
                                        left join Parametros as par on par.idParametro = pla.alturaMaximaFK

                                       
                                    
                                        where nombreCientifico like @texto
                                        or nombresVulgares like @texto;";

                SqlCommand cmdBuscarPorTexto = new SqlCommand(sqlConsulta, cn);
                cmdBuscarPorTexto.Parameters.AddWithValue("@texto", "%" + texto + "%");
                manejadorConexion.AbrirConexion(cn);
                SqlDataReader dr = cmdBuscarPorTexto.ExecuteReader();
                if (dr.HasRows)
                {
                    List<Planta> lstPlantas = new List<Planta>();
                    Planta plantaEncontrada = null;
                    while (dr.Read())
                    {
                        plantaEncontrada = CrearPlanta(dr);
                        plantaEncontrada.CuidadosPlanta = CrearCuidadosPlanta(dr);
                        plantaEncontrada.AlturaMaxima = CrearAlturaMaxima(dr);
                        plantaEncontrada.Tipo = CrearNuevoTP(dr);
                        plantaEncontrada.FotosDePlanta = CrearListaDeFotos2(plantaEncontrada.IdPlanta);
                        lstPlantas.Add(plantaEncontrada);
                    }
                    return lstPlantas;
                }
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message + "ERROR al buscar  Planta por texto");
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
            return null;
        }

        public IEnumerable<Planta> BuscarPlantasPorTipo(TipoPlanta  obj)
        {
            cn = manejadorConexion.CrearConexion();
            if (obj == null) return null;
            try
            {
                string sql = @"select * from planta as pla 
                                        left join TipoPlanta as tp on tp.idTipoPlanta = pla.idTipoPlantaFK
                                        left join CuidadosPlantas as cp on cp.idCuidadosPlanta = pla.idCuidadosFK
                                        left join TipoIluminacion as ti on ti.idTipoIluminacion = cp.idTipoIluminacion
                                        left join Parametros as par on par.idParametro = pla.alturaMaximaFK
                                        where tp.idTipoPlanta = @idTipoPlanta";

                SqlCommand cmdBuscar = new SqlCommand(sql, cn);
                cmdBuscar.Parameters.AddWithValue("@idTipoPlanta", obj.Id);
                manejadorConexion.AbrirConexion(cn);
                SqlDataReader dr = cmdBuscar.ExecuteReader();
                if (dr.HasRows)
                {
                    List<Planta> lstPlantas = new List<Planta>();
                    Planta p = null;
                    while (dr.Read())
                    {            
                        p = CrearPlanta(dr);
                        p.CuidadosPlanta = CrearCuidadosPlanta(dr);
                        p.AlturaMaxima = CrearAlturaMaxima(dr);
                        p.Tipo = CrearNuevoTP(dr);
                        p.FotosDePlanta  = CrearListaDeFotos2(p.IdPlanta);
                        
                        lstPlantas.Add(p);
                    }
                    return lstPlantas;
                }
                

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
            return null;
        }

        public IEnumerable<Planta> BuscarPlantasPorAmbiente(string ambiente)
        {
            cn = manejadorConexion.CrearConexion();

            try
            {
                string sql = @"select * from planta as pla 
                                        left join TipoPlanta as tp on tp.idTipoPlanta = pla.idTipoPlantaFK
                                        left join CuidadosPlantas as cp on cp.idCuidadosPlanta = pla.idCuidadosFK
                                        left join TipoIluminacion as ti on ti.idTipoIluminacion = cp.idTipoIluminacion
                                        left join Parametros as par on par.idParametro = pla.alturaMaximaFK
                                        
                                         
                                        where pla.ambiente = @ambiente";

                SqlCommand cmdBuscar = new SqlCommand(sql, cn);
                cmdBuscar.Parameters.AddWithValue("@ambiente", ambiente);
                manejadorConexion.AbrirConexion(cn);
                SqlDataReader dr = cmdBuscar.ExecuteReader();
                if (dr.HasRows)
                {
                    List<Planta> lstPlantas = new List<Planta>();
                    Planta p = null;
                    while (dr.Read())
                    {
                        p = CrearPlanta(dr);
                        p.CuidadosPlanta = CrearCuidadosPlanta(dr);
                        p.AlturaMaxima = CrearAlturaMaxima(dr);
                        p.Tipo = CrearNuevoTP(dr);
                        p.FotosDePlanta = CrearListaDeFotos2(p.IdPlanta);
                        lstPlantas.Add(p);
                    }
                    return lstPlantas;
                }


            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
            return null;
        }


        public IEnumerable<Planta> BuscarPlantasPorAlturaMinima(int alturaMinima)
        {
            SqlConnection cn2 = new SqlConnection();
            cn = manejadorConexion.CrearConexion();
            cn2 = manejadorConexion.CrearConexion();

            List<Planta> retornoLtaPlantas = new List<Planta>();
            List<Parametros> ltaParametrosMinimos = new List<Parametros>();

            string strCargandoAlturaMin = "SELECT * FROM parametros WHERE tipo =  'Altura';";
            SqlCommand cmdCargandoAlturaMin = new SqlCommand(strCargandoAlturaMin, cn);

            try
            {
                manejadorConexion.AbrirConexion(cn);
                SqlDataReader dr = cmdCargandoAlturaMin.ExecuteReader(); // Lista de alturas
                
                
                while (dr.Read())
                {

                    manejadorConexion.AbrirConexion(cn2);

                    Parametros par = new Parametros()
                    {
                        IdParametro = dr.GetInt32(0),
                        TipoParametro = dr.GetString(1),
                        DescParametro = dr.GetString(2),
                        ValorParametro = dr.GetString(3)
                    };
                    int valor = par.StringToInt(par.ValorParametro);
                    if (valor < alturaMinima) {
                        ltaParametrosMinimos.Add(par);

                        string strCargandoPlantas = @"SELECT * from planta as pla 
                                        left join TipoPlanta as tp on tp.idTipoPlanta = pla.idTipoPlantaFK
                                        left join CuidadosPlantas as cp on cp.idCuidadosPlanta = pla.idCuidadosFK
                                        left join TipoIluminacion as ti on ti.idTipoIluminacion = cp.idTipoIluminacion
                                        left join Parametros as par on par.idParametro = pla.alturaMaximaFK
                                       
                                        where pla.alturaMaximaFK = @idAltura" ;
                        SqlCommand cmdCargarPlantas = new SqlCommand(strCargandoPlantas, cn2);
                        cmdCargarPlantas.Parameters.AddWithValue("@idAltura", par.IdParametro);
                        
                        
                        SqlDataReader drPlantas = cmdCargarPlantas.ExecuteReader();
                        while (drPlantas.Read())
                        {
                            
                        Planta nuevaPlanta = CrearPlanta(drPlantas);
                        nuevaPlanta.CuidadosPlanta = CrearCuidadosPlanta(drPlantas);
                        nuevaPlanta.AlturaMaxima = CrearAlturaMaxima(drPlantas);
                        nuevaPlanta.Tipo = CrearNuevoTP(drPlantas);
                        //nuevaPlanta.FotosDePlanta = CrearListaDeFotos(drPlantas);
                          nuevaPlanta.FotosDePlanta = CrearListaDeFotos2(nuevaPlanta.IdPlanta);
                            retornoLtaPlantas.Add(nuevaPlanta);
                        cmdCargarPlantas.Parameters.Clear();

                        }
                        manejadorConexion.CerrarConexionConClose(cn2);
                    }
                }  
                if (ltaParametrosMinimos.Count < 1) return null;
                return retornoLtaPlantas;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
                manejadorConexion.CerrarConexion(cn2);
            }

            


        }

        public IEnumerable<Planta> BuscarPlantasPorAlturaMaxima(int masDeXaltura)
        {
            SqlConnection cn2 = new SqlConnection();
            cn = manejadorConexion.CrearConexion();
            cn2 = manejadorConexion.CrearConexion();

            List<Planta> retornoLtaPlantas = new List<Planta>();
            List<Parametros> ltaParametrosMax = new List<Parametros>();

            string strCargandoAlturaMax = "SELECT * FROM parametros WHERE tipo =  'Altura';";
            SqlCommand cmdCargandoAlturaMax = new SqlCommand(strCargandoAlturaMax, cn);

            try
            {
                manejadorConexion.AbrirConexion(cn);
                SqlDataReader dr = cmdCargandoAlturaMax.ExecuteReader(); // Lista de alturas


                while (dr.Read())
                {

                    manejadorConexion.AbrirConexion(cn2);

                    Parametros par = new Parametros()
                    {
                        IdParametro = dr.GetInt32(0),
                        TipoParametro = dr.GetString(1),
                        DescParametro = dr.GetString(2),
                        ValorParametro = dr.GetString(3)
                    };
                    int valor = par.StringToInt(par.ValorParametro);
                    if (masDeXaltura <= valor)
                    {
                        ltaParametrosMax.Add(par);

                        string strCargandoPlantas = @"SELECT * from planta as pla 
                                        left join TipoPlanta as tp on tp.idTipoPlanta = pla.idTipoPlantaFK
                                        left join CuidadosPlantas as cp on cp.idCuidadosPlanta = pla.idCuidadosFK
                                        left join TipoIluminacion as ti on ti.idTipoIluminacion = cp.idTipoIluminacion
                                        left join Parametros as par on par.idParametro = pla.alturaMaximaFK
                                       
                                        where pla.alturaMaximaFK = @idAltura";
                        SqlCommand cmdCargarPlantas = new SqlCommand(strCargandoPlantas, cn2);
                        cmdCargarPlantas.Parameters.AddWithValue("@idAltura", par.IdParametro);


                        SqlDataReader drPlantas = cmdCargarPlantas.ExecuteReader();
                        while (drPlantas.Read())
                        {

                            Planta nuevaPlanta = CrearPlanta(drPlantas);
                            nuevaPlanta.CuidadosPlanta = CrearCuidadosPlanta(drPlantas);
                            nuevaPlanta.AlturaMaxima = CrearAlturaMaxima(drPlantas);
                            nuevaPlanta.Tipo = CrearNuevoTP(drPlantas);
                            //nuevaPlanta.FotosDePlanta = CrearListaDeFotos(drPlantas);
                            nuevaPlanta.FotosDePlanta = CrearListaDeFotos2(nuevaPlanta.IdPlanta);
                            retornoLtaPlantas.Add(nuevaPlanta);
                            cmdCargarPlantas.Parameters.Clear();

                        }
                        manejadorConexion.CerrarConexionConClose(cn2);
                    }
                }
                if (ltaParametrosMax.Count < 1) return null;
                return retornoLtaPlantas;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
                manejadorConexion.CerrarConexion(cn2);
            }
        }

       


        // ============================= VALIDACIONES =========================================
        #region VALIDACIONES
        // Validacion para verificar Nombre para que sea UNICO
        public bool BuscarPlantaPorNombreCientifico(string strNombreCientifico)
        {
            bool esValido = false;
            try
            {
                cn = manejadorConexion.CrearConexion();
                SqlCommand strBuscaPlantaPorNombre = new SqlCommand(@"SELECT p.nombreCientifico FROM Planta p
                                                        WHERE p.nombreCientifico = @nombreCientifico", cn);
                strBuscaPlantaPorNombre.Parameters.AddWithValue("@nombreCientifico", strNombreCientifico);

                if (manejadorConexion.AbrirConexion(cn))
                {
                    // obtenemos valor exacto del nombre y comparamos con el que esta en la Base
                    string resultado = (string)strBuscaPlantaPorNombre.ExecuteScalar();
                    if (resultado == strNombreCientifico) esValido = true;

                }
                return esValido;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "ERROR al buscar Planta por NOMBRE");
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
        }


        public Planta BuscarPlantaPorNombreCientificoDevuelvePlanta(string nombre)
        {
            try
            {
                cn = manejadorConexion.CrearConexion();
                string strBuscaPlantaPorNombre = @"SELECT * from planta as pla
                                        left join TipoPlanta as tp on tp.idTipoPlanta = pla.idTipoPlantaFK
                                        left join CuidadosPlantas as cp on cp.idCuidadosPlanta = pla.idCuidadosFK
                                        left join TipoIluminacion as ti on ti.idTipoIluminacion = cp.idTipoIluminacion
                                        left join Parametros as par on par.idParametro = pla.alturaMaximaFK
                                       
                                        WHERE pla.nombreCientifico = @nombreCientifico";
                SqlCommand cmdBuscaPlantaPorNombre = new SqlCommand(strBuscaPlantaPorNombre, cn);
                cmdBuscaPlantaPorNombre.Parameters.AddWithValue("@nombreCientifico", nombre);

                if (manejadorConexion.AbrirConexion(cn))
                {
                    SqlDataReader dr = cmdBuscaPlantaPorNombre.ExecuteReader();
                    Planta Buscada = null;
                    while (dr.Read())
                    {
                        Buscada = CrearPlanta(dr);
                        Buscada.CuidadosPlanta = CrearCuidadosPlanta(dr);
                        Buscada.AlturaMaxima = CrearAlturaMaxima(dr);
                        Buscada.Tipo = CrearNuevoTP(dr);
                     //   Buscada.FotosDePlanta = CrearListaDeFotos(dr);
                        Buscada.FotosDePlanta = CrearListaDeFotos2(Buscada.IdPlanta);
                    }
                    return Buscada;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "ERROR al buscar el OBJETO Planta por NOMBRE");
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }

        }



        #endregion

    }

}

