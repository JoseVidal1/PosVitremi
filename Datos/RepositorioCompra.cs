using Entidades;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class RepositorioCompra:BaseDatos
    {
        private readonly RepositorioUsuario repositorioUsuario;
        private readonly RepositorioProducto repositorioProducto;
        private readonly RepositorioProveedor repositorioProveedor;
        public RepositorioCompra(RepositorioUsuario repositorioUsuario, RepositorioProducto repositorioProducto, RepositorioProveedor repositorioProveedor)
        {
            this.repositorioUsuario = repositorioUsuario;
            this.repositorioProducto = repositorioProducto;
            this.repositorioProveedor = repositorioProveedor;
        }
        public void Add(Compra compra)
        {
            try
            {
                string query = "INSERT INTO Compras (id,proveedor_id, total) VALUES (@id, @proveedorId, @total)";
                string sentencia = "Inser into detalle_compras (id,Compra_id, producto_id, cantidad, precio_compra, subtotal) VALUES (@id, @CompraId, @productoId, @cantidad, @precio,@subtotal)";
                using (MySqlCommand command = new MySqlCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@id", compra.id);
                    command.Parameters.AddWithValue("@cliente_id", compra.proveedor.id);
                    command.Parameters.AddWithValue("@total", compra.total);
                    AbrirConexion();
                    command.ExecuteNonQuery();
                }
                using (MySqlCommand command = new MySqlCommand(sentencia, conexion))
                {
                    foreach (var detalle in compra.detalles)
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@CompraId", compra.id);
                        command.Parameters.AddWithValue("@productoId", detalle.producto.id);
                        command.Parameters.AddWithValue("@cantidad", detalle.cantidad);
                        command.Parameters.AddWithValue("@precio", detalle.precioCompra);
                        command.Parameters.AddWithValue("@subtotal", detalle.subTotal);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error al agregar la Compra: " + e.Message);
            }
            finally
            {
                CerrarConexion();
            }

        }

        public List<Compra> Leer()
        {
            List<Compra> CompraList = new List<Compra>();
            string Consulta = "SELECT * FROM Compras";
            try
            {
                MySqlCommand command = new MySqlCommand(Consulta, conexion);
                AbrirConexion();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Compra Compra = Map(reader);
                    Compra.detalles = leerDetalleCompra().Where(d => d.idCompra == Compra.id).ToList();
                    CompraList.Add(Compra);
                }
                reader.Close();

                CerrarConexion();

            }
            catch (Exception e)
            {
                throw new Exception("Error al leer los Compras: " + e.Message);
            }
            return CompraList;
        }
        public List<DetalleCompra> leerDetalleCompra()
        {
            List<DetalleCompra> detalles = new List<DetalleCompra>();
            string consulta = "SELECT * FROM detalle_Compra";
            try
            {
                MySqlCommand command = new MySqlCommand(consulta, conexion);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    detalles.Add(MapDetalle(reader));
                }
                reader.Close();
            }
            catch (Exception e)
            {
                throw new Exception("Error al leer los detalles de la Compra." + e.Message);
            }
            return detalles;
        }
        public bool Eliminar(int id)
        {
            const string ESTADO_INACTIVO = "Inactivo";
            string actualizar = "UPDATE Compras SET estado = @estado WHERE id = @id";

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
                    throw new Exception("Error al eliminar la Compra: " + e.Message);
                }
                finally
                {
                    CerrarConexion();
                }
            }
        }


        public Compra Buscar(int id)
        {
            try
            {
                return Leer().FirstOrDefault(p => p.id == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar el Compra: " + ex.Message);
            }
        }

        private Compra Map(MySqlDataReader reader)
        {
            Compra Compra = new Compra
            {
                id = Convert.ToInt32(reader["id"]),
                proveedor = ObtenerProveedor(Convert.ToInt32(reader["proveedor_id"])),
                fecha = Convert.ToDateTime(reader["fecha"]),
                total = Convert.ToInt32(reader["total"])
            };

            return Compra;

        }
        private Proveedor ObtenerProveedor(int proveedorId)
        {
            return repositorioProveedor.Leer().FirstOrDefault(p => p.id == proveedorId);
        }
        private Usuario ObtenerUsuario(int usuarioId)
        {
            return repositorioUsuario.Leer().FirstOrDefault(u => u.id == usuarioId);
        }

        private DetalleCompra MapDetalle(MySqlDataReader reader)
        {
            DetalleCompra detalle = new DetalleCompra
            {
                id = Convert.ToInt32(reader["id"]),
                idCompra = Convert.ToInt32(reader["Compra_id"]),
                producto = repositorioProducto.Leer().FirstOrDefault(p => p.id == Convert.ToInt32(reader["producto_id"])),
                cantidad = Convert.ToInt32(reader["cantidad"]),
                precioCompra = Convert.ToDouble(reader["precio_compra"]),
                subTotal = Convert.ToDouble(reader["subtotal"])
            };
            return detalle;
        }
    }
}
