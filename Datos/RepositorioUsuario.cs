using Entidades;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class RepositorioUsuario:BaseDatos
    {
        public void Add(Usuario usuario)
        {
            try
            {
                string query = "INSERT INTO USUARIOS (usuario, id, nombre, contrasena, rol) VALUES (@usuario, @id, @nombre, @contrasena, @rol)";

                using (MySqlCommand command = new MySqlCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@usuario", usuario.usuario);
                    command.Parameters.AddWithValue("@id", usuario.id);
                    command.Parameters.AddWithValue("@nombre", usuario.nombre);
                    command.Parameters.AddWithValue("@contrasena", usuario.contraseña);
                    command.Parameters.AddWithValue("@rol", usuario.rol);
                    AbrirConexion();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error al agregar el usuario: " + e.Message);
            }
            finally
            {
                CerrarConexion();
            }

        }

        public List<Usuario> Leer()
        {
            List<Usuario> usuarioList = new List<Usuario>();
            string Consulta = "SELECT * FROM USUARIOS";
            try
            {
                MySqlCommand command = new MySqlCommand(Consulta, conexion);
                AbrirConexion();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    usuarioList.Add(Map(reader));
                }
                reader.Close();
                CerrarConexion();
            }
            catch (Exception e)
            {
                throw new Exception("Error al leer los usuarios: " + e.Message);
            }
            return usuarioList;
        }

        public bool Eliminar(int id)
        {
            const string ESTADO_INACTIVO = "Inactivo";
            string actualizar = "UPDATE USUARIO SET estado = @estado WHERE id = @id";

            using (MySqlCommand command = new MySqlCommand(actualizar, ObtenerConexion()))
            {
                command.Parameters.AddWithValue("@estado", ESTADO_INACTIVO);
                command.Parameters.AddWithValue("@id", id); 

                try
                {
                    AbrirConexion();
                    int filasAfectadas = command.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
                catch (Exception e)
                {
                    throw new Exception("Error al eliminar el usuario: " + e.Message);
                }
                finally
                {
                    CerrarConexion();
                }
            }
        }


        public Usuario Buscar(int id)
        {
            try
            {
                return Leer().FirstOrDefault(p => p.id == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar el usuario: " + ex.Message);
            }
        }

        public bool Actualizar(Usuario usuarioNew)
        {
            try
            {
                string actualizar = "UPDATE USUARIO SET usuario = @usuario, nombre = @nombre, contrasena = @Contraseña, rol = @rol WHERE id = @id";
                using (MySqlCommand command = new MySqlCommand(actualizar, conexion))
                {
                    command.Parameters.AddWithValue("@usuario", usuarioNew.usuario);
                    command.Parameters.AddWithValue("@nombre", usuarioNew.nombre);
                    command.Parameters.AddWithValue("@Contraseña", usuarioNew.contraseña);
                    command.Parameters.AddWithValue("@rol", usuarioNew.rol);
                    command.Parameters.AddWithValue("@id", usuarioNew.id);

                    AbrirConexion();
                    command.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("Error al actualizar usuario" + e.Message);
            }
            finally
            {
                CerrarConexion();
            }
        }

        private Usuario Map(MySqlDataReader reader)
        {
            Usuario usuario = new Usuario
            {
                id = Convert.ToInt32(reader["id"]),
                usuario = Convert.ToString(reader["usuario"]),
                nombre = Convert.ToString(reader["nombre"]),
                contraseña = Convert.ToString(reader["contrasena"]),
                rol = Convert.ToString(reader["Rol"]),
                estado = Convert.ToString(reader["estado"])
            };

            return usuario;

        }
    }
}
