using Dominio.EntidadesNegocio;
using Dominio.InterfacesRepositorios;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System;

namespace Repositorios
{
    public class RepositoriosUsuariosADO : IRepositorioUsuarios
    {
        private Conexion manejadorConexion = new Conexion();
        private SqlConnection cn;
        public bool Add(Usuario obj)
        {

            if (!obj.Validar()) return false;

            cn = manejadorConexion.CrearConexion();
            SqlTransaction trn = null;
            try
            {
                string sqlInsert = @"INSERT INTO Usuario VALUES(@email,@pass) 
                                    SELECT CAST(Scope_Identity() as int)";

                SqlCommand cmdAgregarUsuario = new SqlCommand(sqlInsert, cn);
                cmdAgregarUsuario.Parameters.AddWithValue("@email", obj.Email);
                cmdAgregarUsuario.Parameters.AddWithValue("@pass", obj.Pass);

                manejadorConexion.AbrirConexion(cn);
                trn = cn.BeginTransaction();
                cmdAgregarUsuario.Transaction = trn;
                int idGenerado = (int)cmdAgregarUsuario.ExecuteScalar();

                trn.Commit();
                return true;
            }
            catch (Exception ex)
            {
                trn.Rollback();
                return false;
                throw new Exception(ex.Message + "ERROR al AGREGAR un nuevo Usuario");
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
        }


        public IEnumerable<Usuario> FindAll()
        {
            try
            {
                cn = manejadorConexion.CrearConexion();
                SqlCommand cmdUsuarioListar = new SqlCommand("SELECT * from Usuario", cn);
                manejadorConexion.AbrirConexion(cn);
                SqlDataReader dr = cmdUsuarioListar.ExecuteReader();
                if (dr.HasRows)
                {
                    List<Usuario> listaUsu = new List<Usuario>();
                    while (dr.Read())
                    {
                        listaUsu.Add(new Usuario
                        {
                            Id = (int)dr["id"],
                            Email = dr["email"].ToString(),
                            Pass = dr["pass"].ToString()
                        }); ;
                    }
                    return listaUsu;
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

        public Usuario FindByID(int id)
        {
            try
            {
                cn = manejadorConexion.CrearConexion();
                SqlCommand cmdListarUsuID = new SqlCommand(@"SELECT * from Usuario
                                                          where id=@id", cn);
                cmdListarUsuID.Parameters.AddWithValue("@id", id);
                if (manejadorConexion.AbrirConexion(cn))
                {
                    SqlDataReader dr = cmdListarUsuID.ExecuteReader();
                    while (dr.Read())
                    {
                        return new Usuario
                        {
                            Id = (int)dr["id"],
                            Email = dr["email"].ToString(),
                            Pass = dr["pass"].ToString()
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

        //Busca usuario por email y pass
        public Usuario AutenticarU(string email,string pass)
        {
            try
            {
                cn = manejadorConexion.CrearConexion();
                SqlCommand cmdListarUsu = new SqlCommand(@"SELECT * from Usuario
                                                          where email=@email 
                                                          and pass=@pass", cn);
                cmdListarUsu.Parameters.AddWithValue("@email", email);
                cmdListarUsu.Parameters.AddWithValue("@pass", pass);
                if (manejadorConexion.AbrirConexion(cn))
                {
                    SqlDataReader dr = cmdListarUsu.ExecuteReader();
                    while (dr.Read())
                    {
                        return new Usuario
                        {
                            Id = (int)dr["idUsuario"],
                            Email = dr["email"].ToString(),
                            Pass = dr["pass"].ToString()
                        };
                    }

                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "ERROR al buscar usuario por email");
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
        }

        public bool Remove(Usuario obj)
        {
            bool valido = false;
            if (obj == null) return valido;

            try
            {
                cn = manejadorConexion.CrearConexion();
                SqlCommand cmdUsuarioId = new SqlCommand("DELETE from Usuario where id=@id");
                cmdUsuarioId.Parameters.AddWithValue("@id", obj.Id);
                manejadorConexion.AbrirConexion(cn);

                int filaAEliminar = cmdUsuarioId.ExecuteNonQuery();
                if (filaAEliminar == 1) valido = true;

                return valido;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "No pudo removerse el Usuario seleccionado");
            }
            finally
            {
                manejadorConexion.CerrarConexion(cn);
            }
        }

        public bool Update(Usuario obj)
        {
            bool validar = false;
            if (obj == null) return validar;
            if (!obj.Validar()) return validar;
            SqlTransaction trn = null;

            try
            {
                cn = manejadorConexion.CrearConexion();

                SqlCommand cmdUpdateUsu = new SqlCommand(@"UPDATE usuario SET email=@email,
                                                    password=@password", cn);

                cmdUpdateUsu.Parameters.AddWithValue("@email", obj.Email);
                cmdUpdateUsu.Parameters.AddWithValue("@password", obj.Pass);

                if (manejadorConexion.AbrirConexion(cn))
                {
                    trn = cn.BeginTransaction();
                    cmdUpdateUsu.Transaction = trn;
                    int filaAfectada = cmdUpdateUsu.ExecuteNonQuery();
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

