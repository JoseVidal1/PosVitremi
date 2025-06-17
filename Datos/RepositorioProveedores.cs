using Entidades;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class RepositorioProveedor : BaseDatos
    {
        public void Add(Proveedor proveedor)
        {
            string query = @"INSERT INTO proveedores (id, nombre, telefono, direccion, estado)
                             VALUES (@id, @nombre, @telefono, @direccion, @estado)";
            try
            {
                using (var conn = ObtenerConexion())
                {
                    conn.Open();
                    using (var command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@id", proveedor.id);
                        command.Parameters.AddWithValue("@nombre", proveedor.nombre);
                        command.Parameters.AddWithValue("@telefono", proveedor.telefono);
                        command.Parameters.AddWithValue("@direccion", proveedor.direccion);
                        command.Parameters.AddWithValue("@estado", proveedor.estado);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error al agregar el proveedor: " + e.Message);
            }
        }

        public List<Proveedor> Leer()
        {
            var lista = new List<Proveedor>();
            string query = "SELECT * FROM proveedores";

            try
            {
                using (var conn = ObtenerConexion())
                {
                    conn.Open();
                    using (var command = new MySqlCommand(query, conn))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(Map(reader));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error al leer los proveedores: " + e.Message);
            }

            return lista;
        }

        public Proveedor Buscar(int id)
        {
            try
            {
                return Leer().FirstOrDefault(p => p.id == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar proveedor: " + ex.Message);
            }
        }

        public bool Actualizar(Proveedor proveedor)
        {
            string query = @"UPDATE proveedores 
                             SET nombre = @nombre, telefono = @telefono, 
                                 direccion = @direccion, estado = @estado
                             WHERE id = @id";
            try
            {
                using (var conn = ObtenerConexion())
                {
                    conn.Open();
                    using (var command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@nombre", proveedor.nombre);
                        command.Parameters.AddWithValue("@telefono", proveedor.telefono);
                        command.Parameters.AddWithValue("@direccion", proveedor.direccion);
                        command.Parameters.AddWithValue("@estado", proveedor.estado);
                        command.Parameters.AddWithValue("@id", proveedor.id);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error al actualizar proveedor: " + e.Message);
            }
        }

        public bool Eliminar(int id)
        {
            const string ESTADO_INACTIVO = "inactivo";
            string query = "UPDATE proveedores SET estado = @estado WHERE id = @id";

            try
            {
                using (var conn = ObtenerConexion())
                {
                    conn.Open();
                    using (var command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@estado", ESTADO_INACTIVO);
                        command.Parameters.AddWithValue("@id", id);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error al eliminar proveedor: " + e.Message);
            }
        }

        private Proveedor Map(MySqlDataReader reader)
        {
            return new Proveedor
            {
                id = Convert.ToInt32(reader["id"]),
                nombre = Convert.ToString(reader["nombre"]),
                telefono = reader["telefono"] != DBNull.Value ? Convert.ToString(reader["telefono"]) : null,
                direccion = reader["direccion"] != DBNull.Value ? Convert.ToString(reader["direccion"]) : null,
                estado = Convert.ToString(reader["estado"])
            };
        }
    }
}




