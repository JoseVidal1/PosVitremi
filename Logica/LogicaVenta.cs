using System;
using Datos;
using Entidades;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class LogicaVenta
    {
        private readonly RepositorioVenta repositorioVenta;
        private readonly RepositorioProducto repositorioProducto;
        public LogicaVenta(RepositorioVenta repositorioVenta, RepositorioProducto repositorioProducto)
        {
            this.repositorioVenta = repositorioVenta;
            this.repositorioProducto = repositorioProducto;
        }
        public void Add(Venta venta)
        {
            try
            {
                ValidarVenta(venta);
                ValidarDetallesVenta(venta.detalles);
                repositorioVenta.Add(venta);
                repositorioProducto.ActualizarStockVenta(venta.detalles);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar la venta: " + ex.Message);
            }
        }
        private void ValidarVenta(Venta venta)
        {
            if (venta == null)
            {
                throw new ArgumentNullException(nameof(venta), "La venta no puede ser nula");
            }
            if (venta.detalles == null || !venta.detalles.Any())
            {
                throw new ArgumentException("La venta debe contener al menos un producto", nameof(venta.detalles));
            }

        }
        private void ValidarDetallesVenta(List<DetalleVenta> detalles)
        {
            if (detalles == null || !detalles.Any())
            {
                throw new ArgumentException("La venta debe contener al menos un detalle", nameof(detalles));
            }
            foreach (var detalle in detalles)
            {
                if (detalle.producto == null)
                {
                    throw new ArgumentException("Cada detalle de venta debe tener un producto asociado", nameof(detalle.producto));
                }
                if (detalle.cantidad <= 0)
                {
                    throw new ArgumentException("La cantidad en cada detalle de venta debe ser mayor que cero", nameof(detalle.cantidad));
                }
                if(detalle.cantidad > detalle.producto.stock)
                {
                    throw new ArgumentException($"La cantidad solicitada para el producto {detalle.producto.nombre} excede el stock disponible", nameof(detalle.cantidad));
                }
            }
        }
        public List<Venta> Leer()
        {
            return repositorioVenta.Leer();
        }
        public Venta BuscarPorId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID debe ser un número positivo", nameof(id));
            }
            var ventas = repositorioVenta.Leer();
            return ventas.FirstOrDefault(v => v.id == id);
        }
        public bool Eliminar(int id)
        {
            return repositorioVenta.Eliminar(id);
        }
    }
}
