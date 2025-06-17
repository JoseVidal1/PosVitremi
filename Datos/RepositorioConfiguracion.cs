using Entidades;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class RepositorioConfiguracion : BaseDatos
    {
        public List<Configuracion> Leer()
        {
            var lista = new List<Configuracion>();
            const string query = "SELECT * FROM configuracion";

            try
            {
                using (var conn = ObtenerConexion())
                {
                    conn.Open();

                    using (var command = new MySqlCommand(query, conn))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lista.Add(Map(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error al leer la configuración: " + e.Message);
            }

            return lista;
        }

        public bool Actualizar(Configuracion config)
        {
            const string query = @"
                UPDATE configuracion 
                SET 
                    nombre_negocio = @nombre_negocio, 
                    direccion = @direccion,
                    telefono = @telefono, 
                    correo = @correo, 
                    logo = @logo
                WHERE id = @id";

            try
            {
                using (var conn = ObtenerConexion())
                {
                    conn.Open();

                    using (var command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@nombre_negocio", config.nombre_negocio);
                        command.Parameters.AddWithValue("@direccion", config.direccion);
                        command.Parameters.AddWithValue("@telefono", config.telefono);
                        command.Parameters.AddWithValue("@correo", config.correo);
                        command.Parameters.AddWithValue("@logo", config.logo);
                        command.Parameters.AddWithValue("@id", config.id);

                        int filasAfectadas = command.ExecuteNonQuery();
                        return filasAfectadas > 0;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error al actualizar la configuración: " + e.Message);
            }
        }

        private Configuracion Map(MySqlDataReader reader)
        {
            return new Configuracion
            {
                id = Convert.ToInt32(reader["id"]),
                nombre_negocio = reader["nombre_negocio"].ToString(),
                direccion = reader["direccion"] != DBNull.Value ? reader["direccion"].ToString() : null,
                telefono = reader["telefono"] != DBNull.Value ? reader["telefono"].ToString() : null,
                correo = reader["correo"] != DBNull.Value ? reader["correo"].ToString() : null,
                logo = reader["logo"] != DBNull.Value ? reader["logo"].ToString() : null
            };
        }
    }
}





