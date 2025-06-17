using Entidades;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class RepositorioCategoria:BaseDatos
    {
        public void Add(Categoria Categoria)
        {
            try
            {
                string query = "INSERT INTO CATEGORIAS (id, nombre) VALUES (@id, @nombre)";

                using (MySqlCommand command = new MySqlCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@id", Categoria.id);
                    command.Parameters.AddWithValue("@nombre", Categoria.nombre);
                    AbrirConexion();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error al agregar el Categoria: " + e.Message);
            }
            finally
            {
                CerrarConexion();
            }

        }

        public List<Categoria> Leer()
        {
            List<Categoria> CategoriaList = new List<Categoria>();
            string Consulta = "SELECT * FROM Categorias";
            try
            {
                MySqlCommand command = new MySqlCommand(Consulta, conexion);
                AbrirConexion();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    CategoriaList.Add(Map(reader));
                }
                reader.Close();
                CerrarConexion();
            }
            catch (Exception e)
            {
                throw new Exception("Error al leer los Categorias: " + e.Message);
            }
            return CategoriaList;
        }

        public bool Eliminar(int id)
        {
            const string ESTADO_INACTIVO = "Inactivo";
            string actualizar = "UPDATE Categorias SET estado = @estado WHERE id = @id";

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
                    throw new Exception("Error al eliminar la Categoria: " + e.Message);
                }
                finally
                {
                    CerrarConexion();
                }
            }
        }


        public Categoria Buscar(int id)
        {
            try
            {
                return Leer().FirstOrDefault(p => p.id == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar la Categoria: " + ex.Message);
            }
        }

        public bool Actualizar(Categoria CategoriaNew)
        {
            try
            {
                string actualizar = "UPDATE Categoria SET Categoria = @Categoria, nombre = @nombre, contrasena = @Contraseña, rol = @rol WHERE id = @id";
                using (MySqlCommand command = new MySqlCommand(actualizar, conexion))
                {
                    command.Parameters.AddWithValue("@id", CategoriaNew.id);
                    command.Parameters.AddWithValue("@nombre", CategoriaNew.nombre);

                    AbrirConexion();
                    command.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("Error al actualizar la Categoria" + e.Message);
            }
            finally
            {
                CerrarConexion();
            }
        }

        private Categoria Map(MySqlDataReader reader)
        {
            Categoria Categoria = new Categoria
            {
                id = Convert.ToInt32(reader["id"]),
                nombre = Convert.ToString(reader["nombre"]),
                estado = Convert.ToString(reader["estado"])
            };

            return Categoria;

        }
    }
}
