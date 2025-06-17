using Entidades;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class RepositorioProducto : BaseDatos
    {
        private readonly RepositorioCategoria repositorioCategoria;
        private readonly RepositorioProveedor repositorioProveedor;
        public RepositorioProducto(RepositorioCategoria repositorioCategoria, RepositorioProveedor repositorioProveedor)
        {
            this.repositorioCategoria = repositorioCategoria;
            this.repositorioProveedor = repositorioProveedor;
        }

        public void Add(Producto producto)
        {
            try
            {
                string query = "INSERT INTO Productos (id, nombre,descripcion, precio_compra,precio_venta, stock,cantidad_min,cantidad_max, categoria_id, proveedor_id) VALUES (@id, @nombre,@descripcion, @precio_compra,@precio_venta,@cantidad_min,@cantidad_max, @categoriaId, @proveedorId)";
                using (MySqlCommand command = new MySqlCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@id", producto.id);
                    command.Parameters.AddWithValue("@nombre", producto.nombre);
                    command.Parameters.AddWithValue("@descripcion", producto.descripcion);
                    command.Parameters.AddWithValue("@precio_compra", producto.precioCompra);
                    command.Parameters.AddWithValue("@precio_venta", producto.precioVenta);
                    command.Parameters.AddWithValue("@stock", producto.stock);
                    command.Parameters.AddWithValue("@cantidad_min", producto.cantidadMinima);
                    command.Parameters.AddWithValue("@cantidad_max", producto.cantidadMaxima);
                    command.Parameters.AddWithValue("@categoriaId", producto.categoria.id);
                    command.Parameters.AddWithValue("@proveedorId", producto.proveedor.id);
                    AbrirConexion();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error al agregar el Producto: " + e.Message);
            }
            finally
            {
                CerrarConexion();
            }
        }
        public List<Producto> Leer()
        {
            List<Producto> productoList = new List<Producto>();
            string Consulta = "SELECT * FROM Productos";
            try
            {
                MySqlCommand command = new MySqlCommand(Consulta, conexion);
                AbrirConexion();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    productoList.Add(Map(reader));
                }
                reader.Close();
                CerrarConexion();
            }
            catch (Exception e)
            {
                throw new Exception("Error al leer los Productos: " + e.Message);
            }
            return productoList;
        }
        private Producto Map(MySqlDataReader reader)
        {
            return new Producto
            {
                id = reader.GetInt32("id"),
                nombre = reader.GetString("nombre"),
                descripcion = reader.GetString("descripcion"),
                precioCompra = reader.GetDouble("precio_compra"),
                precioVenta = reader.GetDouble("precio_venta"),
                stock = reader.GetInt32("stock"),
                cantidadMinima = reader.GetInt32("cantidad_min"),
                cantidadMaxima = reader.GetInt32("cantidad_max"),
                categoria = repositorioCategoria.Leer().FirstOrDefault(c => c.id == reader.GetInt32("categoria_id")),
                proveedor = repositorioProveedor.Leer().FirstOrDefault(p => p.id == reader.GetInt32("proveedor_id"))
            };
        }
        public Producto BuscarPorId(int id)
        {
            Producto producto = null;
            string Consulta = "SELECT * FROM Productos WHERE id = @id";
            try
            {
                MySqlCommand command = new MySqlCommand(Consulta, conexion);
                command.Parameters.AddWithValue("@id", id);
                AbrirConexion();
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    producto = Map(reader);
                }
                reader.Close();
                CerrarConexion();
            }
            catch (Exception e)
            {
                throw new Exception("Error al buscar el Producto: " + e.Message);
            }
            return producto;
        }
        public bool Eliminar(int id)
        {
            const string ESTADO_INACTIVO = "Inactivo";
            string actualizar = "UPDATE Productos SET estado = @estado WHERE id = @id";
            try
            {
                using (MySqlCommand command = new MySqlCommand(actualizar, conexion))
                {
                    command.Parameters.AddWithValue("@estado", ESTADO_INACTIVO);
                    command.Parameters.AddWithValue("@id", id);
                    AbrirConexion();
                    int filasAfectadas = command.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error al eliminar el Producto: " + e.Message);
            }
            finally
            {
                CerrarConexion();
            }
        }
        public void Actualizar(Producto producto)
        {
            string actualizar = "UPDATE Productos SET nombre = @nombre, descripcion = @descripcion, precio_compra = @precio_compra, precio_venta = @precio_venta, stock = @stock, cantidad_min = @cantidad_min, cantidad_max = @cantidad_max, categoria_id = @categoriaId, proveedor_id = @proveedorId WHERE id = @id";
            try
            {
                using (MySqlCommand command = new MySqlCommand(actualizar, conexion))
                {
                    command.Parameters.AddWithValue("@id", producto.id);
                    command.Parameters.AddWithValue("@nombre", producto.nombre);
                    command.Parameters.AddWithValue("@descripcion", producto.descripcion);
                    command.Parameters.AddWithValue("@precio_compra", producto.precioCompra);
                    command.Parameters.AddWithValue("@precio_venta", producto.precioVenta);
                    command.Parameters.AddWithValue("@stock", producto.stock);
                    command.Parameters.AddWithValue("@cantidad_min", producto.cantidadMinima);
                    command.Parameters.AddWithValue("@cantidad_max", producto.cantidadMaxima);
                    command.Parameters.AddWithValue("@categoriaId", producto.categoria.id);
                    command.Parameters.AddWithValue("@proveedorId", producto.proveedor.id);
                    AbrirConexion();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error al actualizar el Producto: " + e.Message);
            }
            finally
            {
                CerrarConexion();
            }
        }
    }
}