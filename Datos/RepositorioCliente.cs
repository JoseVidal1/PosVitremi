using Entidades;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    internal class RepositorioCliente : BaseDatos
    {

        public void Add(Cliente cliente)
        {
            string query = @"INSERT INTO clientes (id, nombre, telefono, correo, direccion, estado)
                             VALUES (@id, @nombre, @telefono, @correo, @direccion, @estado)";
            try
            {
                using (var conn = ObtenerConexion())
                {
                    conn.Open();
                    using (var command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@id", cliente.id);
                        command.Parameters.AddWithValue("@nombre", cliente.nombre);
                        command.Parameters.AddWithValue("@telefono", cliente.telefono);
                        command.Parameters.AddWithValue("@correo", cliente.correo);
                        command.Parameters.AddWithValue("@direccion", cliente.direccion);
                        command.Parameters.AddWithValue("@estado", cliente.estado);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error al agregar el cliente: " + e.Message);
            }
        }

        public List<Cliente> Leer()
        {
            var lista = new List<Cliente>();
            string query = "SELECT * FROM clientes";

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
                throw new Exception("Error al leer los clientes: " + e.Message);
            }

            return lista;
        }

        public Cliente Buscar(int id)
        {
            try
            {
                return Leer().FirstOrDefault(c => c.id == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar cliente: " + ex.Message);
            }
        }

        public bool Actualizar(Cliente cliente)
        {
            string query = @"UPDATE clientes 
                             SET nombre = @nombre, telefono = @telefono, correo = @correo, 
                                 direccion = @direccion, estado = @estado
                             WHERE id = @id";
            try
            {
                using (var conn = ObtenerConexion())
                {
                    conn.Open();
                    using (var command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@nombre", cliente.nombre);
                        command.Parameters.AddWithValue("@telefono", cliente.telefono);
                        command.Parameters.AddWithValue("@correo", cliente.correo);
                        command.Parameters.AddWithValue("@direccion", cliente.direccion);
                        command.Parameters.AddWithValue("@estado", cliente.estado);
                        command.Parameters.AddWithValue("@id", cliente.id);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error al actualizar cliente: " + e.Message);
            }
        }

        public bool Eliminar(int id)
        {
            const string ESTADO_INACTIVO = "inactivo";
            string query = "UPDATE clientes SET estado = @estado WHERE id = @id";

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
                throw new Exception("Error al eliminar cliente: " + e.Message);
            }
        }

        private Cliente Map(MySqlDataReader reader)
        {
            return new Cliente
            {
                id = Convert.ToInt32(reader["id"]),
                nombre = Convert.ToString(reader["nombre"]),
                telefono = reader["telefono"] != DBNull.Value ? Convert.ToString(reader["telefono"]) : null,
                correo = reader["correo"] != DBNull.Value ? Convert.ToString(reader["correo"]) : null,
                direccion = reader["direccion"] != DBNull.Value ? Convert.ToString(reader["direccion"]) : null,
                estado = Convert.ToString(reader["estado"])
            };
        }
    }
}





