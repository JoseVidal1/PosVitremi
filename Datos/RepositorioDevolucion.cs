using Entidades;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{

    public class RepositorioDevolucion : BaseDatos
    {
        private readonly RepositorioVenta repositorioVenta;
        private readonly RepositorioProducto repositorioProducto;
        public RepositorioDevolucion(RepositorioVenta repositorioVenta, RepositorioProducto repositorioProducto)
        {
            this.repositorioVenta = repositorioVenta;
            this.repositorioProducto = repositorioProducto;
        }
        public void Add(Devolucion devolucion)
        {
            try
            {
                string query = "INSERT INTO Devoluciones (venta_id, producto_id, cantidad, precio,motivo, subtotal) VALUES (@id, @ventaId, @productoId, @cantidad, @precio,@motivo, @subtotal)";
                using (MySqlCommand command = new MySqlCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@ventaId", devolucion.venta.id);
                    command.Parameters.AddWithValue("@productoId", devolucion.producto.id);
                    command.Parameters.AddWithValue("@cantidad", devolucion.cantidad);
                    command.Parameters.AddWithValue("@motivo", devolucion.motivo);
                    command.Parameters.AddWithValue("@precio", devolucion.precio);
                    AbrirConexion();
                    command.ExecuteNonQuery();
                }
                Producto p = repositorioProducto.BuscarPorId(devolucion.producto.id);
                p.stock += devolucion.cantidad;
                repositorioProducto.Actualizar(p);
            }
            catch (Exception e)
            {
                throw new Exception("Error al agregar la Devolución: " + e.Message);
            }
            finally
            {
                CerrarConexion();
            }
        }
        public List<Devolucion> Leer()
        {
            List<Devolucion> devoluciones = new List<Devolucion>();
            string consulta = "SELECT * FROM Devoluciones";
            try
            {
                MySqlCommand command = new MySqlCommand(consulta, conexion);
                AbrirConexion();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Devolucion devolucion = new Devolucion
                    {
                        id = reader.GetInt32("id"),
                        venta = repositorioVenta.Buscar(reader.GetInt32("venta_id")),
                        producto = repositorioProducto.BuscarPorId(reader.GetInt32("producto_id")),
                        cantidad = reader.GetInt32("cantidad"),
                        precio = reader.GetDouble("precio"),
                        motivo = reader.GetString("motivo"),
                    };
                    devoluciones.Add(devolucion);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error al leer las Devoluciones: " + e.Message);
            }
            finally
            {
                CerrarConexion();
            }
            return devoluciones;
        }
    }
}
