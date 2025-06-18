using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logica
{
    public class LogicaProducto
    {
        private readonly RepositorioProducto repositorioProducto;
        public LogicaProducto(RepositorioProducto repositorioProducto)
        {
            this.repositorioProducto = repositorioProducto;
        }
        public void Add(Producto producto)
        {
            try
            {
                ValidarProducto(producto);
                repositorioProducto.Add(producto);
            }catch(Exception ex)
            {
                throw new Exception("Error al agregar el producto: " + ex.Message);
            }

        }
        private void ValidarNombre(string nombre)
        {
            if (!string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre del producto no puede estar vacío", nameof(nombre));
            }
        }
        private void ValidarDescripcion(string descripcion)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
            {
                throw new ArgumentException("La descripción del producto no puede estar vacía", nameof(descripcion));
            }
        }
        private void ValidarPrecio(double precioVenta, double precioCompra)
        {
            if(precioVenta <=0 || precioCompra<=0)
            {
                throw new ArgumentException("El precio de venta y el precio de compra deben ser valores validos");
            }
        }
        private void ValidarStock(int stock)
        {
            if (stock < 0)
            {
                throw new ArgumentException("El stock no puede ser negativo", nameof(stock));
            }
        }
        private void ValidarCategoria(Producto producto)
        {
            if (producto.categoria == null)
            {
                throw new ArgumentNullException(nameof(producto.categoria), "El producto debe tener una categoría asociada");
            }
            if (producto.categoria.estado=="Inactivo")
            {
                throw new ArgumentException("El producto debe tener una categoría válida", nameof(producto.categoria));
            }
        }
        private void ValidarProveedor(Producto producto)
        {
            if (producto.proveedor == null)
            {
                throw new ArgumentNullException(nameof(producto.proveedor), "El producto debe tener un proveedor asociado");
            }
            if (producto.proveedor.estado == "Inactivo")
            {
                throw new ArgumentException("El producto debe tener un proveedor válido", nameof(producto.proveedor));
            }
        }
        private void ValidarCantidad(int cantidadMinima, int cantidadMaxima)
        {
            if (cantidadMinima <=0 || cantidadMaxima <=0)
            {
                throw new ArgumentException("La cantidad min o max no es valida", nameof(cantidadMinima));
            }
            if (cantidadMinima > cantidadMaxima)
            {
                throw new ArgumentException("La cantidad mínima no puede ser mayor que la cantidad máxima", nameof(cantidadMinima));
            }
        }
        private void ValidarProducto(Producto producto)
        {
            if (producto == null)
            {
                throw new ArgumentNullException(nameof(producto), "El producto no puede ser nulo");
            }
            ValidarNombre(producto.nombre);
            ValidarDescripcion(producto.descripcion);
            ValidarPrecio(producto.precioVenta, producto.precioCompra);
            ValidarStock(producto.stock);
            ValidarCategoria(producto);
            ValidarProveedor(producto);
            ValidarCantidad(producto.cantidadMinima, producto.cantidadMaxima);
        }
        public List<Producto> Leer()
        {
            return repositorioProducto.Leer();
        }
        public Producto BuscarPorId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID debe ser un número positivo", nameof(id));
            }
            return repositorioProducto.BuscarPorId(id);
        }
       
        public List<Producto> ProductosBajoStock()
        {
            return repositorioProducto.Leer().Where(p => p.stock <= p.cantidadMinima).ToList();
        }
        public bool NotificarBajoStock()
        {
            return ProductosBajoStock().Count > 0;
        }
        public bool Eliminar(int id)
        {
            return repositorioProducto.Eliminar(id);
        }
        public bool Actualizar(Producto producto)
        {
            if (producto == null)
            {
                throw new ArgumentNullException(nameof(producto), "El producto no puede ser nulo");
            }
           return repositorioProducto.Actualizar(producto);
        }
    }
}
