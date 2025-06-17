using Entidades;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class RepositorioVenta: BaseDatos
    {
        private readonly RepositorioUsuario repositorioUsuario;
        private readonly RepositorioCliente repositorioCliente;
        public RepositorioVenta(RepositorioUsuario repositorioUsuario, RepositorioCliente repositorioCliente)
        {
            this.repositorioUsuario = repositorioUsuario;
            this.repositorioCliente = repositorioCliente;
        }
        public void Add(Venta venta)
        {
            try
            {
                string query = "INSERT INTO Ventas (id,cliente_id, total, metodo_pago) VALUES (@id, @clienteId, @total, @metodoPago)";
                string sentencia = "Inser into detalle_ventas (id,venta_id, producto_id, cantidad, precio, subtotal) VALUES (@id, @ventaId, @productoId, @cantidad, @precio,@subtotal)";
                using (MySqlCommand command = new MySqlCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@id", venta.id);
                    command.Parameters.AddWithValue("@cliente_id", venta.Cliente.id);
                    command.Parameters.AddWithValue("@total", venta.total);
                    command.Parameters.AddWithValue("@metodoPago", venta.metodoPago);
                    AbrirConexion();
                    command.ExecuteNonQuery();
                }
                using (MySqlCommand command = new MySqlCommand(sentencia, conexion))
                {
                    foreach (var detalle in venta.detalles)
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@id", detalle.id);
                        command.Parameters.AddWithValue("@ventaId", venta.id);
                        command.Parameters.AddWithValue("@productoId", detalle.producto.id);
                        command.Parameters.AddWithValue("@cantidad", detalle.cantidad);
                        command.Parameters.AddWithValue("@precio", detalle.precioUnitario);
                        command.Parameters.AddWithValue("@subtotal", detalle.subtotal);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error al agregar la Venta: " + e.Message);
            }
            finally
            {
                CerrarConexion();
            }

        }

        public List<Venta> Leer()
        {
            List<Venta> VentaList = new List<Venta>();
            string Consulta = "SELECT * FROM Ventas";
            try
            {
                MySqlCommand command = new MySqlCommand(Consulta, conexion);
                AbrirConexion();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    VentaList.Add(Map(reader));
                }
                reader.Close();
                CerrarConexion();
            }
            catch (Exception e)
            {
                throw new Exception("Error al leer los Ventas: " + e.Message);
            }
            return VentaList;
        }

        public bool Eliminar(int id)
        {
            const string ESTADO_INACTIVO = "Inactivo";
            string actualizar = "UPDATE Ventas SET estado = @estado WHERE id = @id";

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
                    throw new Exception("Error al eliminar la Venta: " + e.Message);
                }
                finally
                {
                    CerrarConexion();
                }
            }
        }


        public Venta Buscar(int id)
        {
            try
            {
                return Leer().FirstOrDefault(p => p.id == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar el Venta: " + ex.Message);
            }
        }

        public bool Actualizar(Venta VentaNew)
        {
            try
            {
                string actualizar = "UPDATE Venta SET Venta id=@id ,cliente_id=@cliente_id, total=@total, metodo_pago= @metodo_pago WHERE id = @id";
                using (MySqlCommand command = new MySqlCommand(actualizar, conexion))
                {
                    command.Parameters.AddWithValue("@cliente_id", VentaNew.Cliente.id);
                    command.Parameters.AddWithValue("@total", VentaNew.total);
                    command.Parameters.AddWithValue("@metodo_pago", VentaNew.metodoPago);
                    command.Parameters.AddWithValue("@id", VentaNew.id);
                    AbrirConexion();
                    command.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("Error al actualizar Venta" + e.Message);
            }
            finally
            {
                CerrarConexion();
            }
        }

        private Venta Map(MySqlDataReader reader)
        {
            Venta Venta = new Venta
            {
                id = Convert.ToInt32(reader["id"]),
                Cliente = ObtenerCliente(Convert.ToInt32(reader["cliente_id"])),
                Usuario = ObtenerUsuario(Convert.ToInt32(reader["usuario_id"])),
                fecha = Convert.ToDateTime(reader["fecha"]),
                metodoPago = Convert.ToString(reader["metodo_pago"]),
                total = Convert.ToInt32(reader["total"])
            };

            return Venta;

        }
        private Usuario ObtenerUsuario(int usuarioId)
        {
            return repositorioUsuario.Leer().FirstOrDefault(u => u.id == usuarioId);
        }
        private Cliente ObtenerCliente(int clienteId)
        {
            return repositorioCliente.Leer().FirstOrDefault(c => c.id == clienteId);
        }
        private DetalleVenta MapDetalle(MySqlDataReader reader)
        {
            DetalleVenta detalle = new DetalleVenta
            {
                id = Convert.ToInt32(reader["id"]),
                producto = new Producto { id = Convert.ToInt32(reader["producto_id"]) },
                cantidad = Convert.ToInt32(reader["cantidad"]),
                precioUnitario = Convert.ToDouble(reader["precio"]),
                subtotal = Convert.ToDouble(reader["subtotal"])
            };
            return detalle;
        }
    }
}

