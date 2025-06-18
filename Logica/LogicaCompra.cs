using Entidades;
using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class LogicaCompra
    {
        private readonly RepositorioCompra repositorioCompra;
        private readonly RepositorioProducto repositorioProducto;
        public LogicaCompra(RepositorioCompra repositorioCompra, RepositorioProducto repositorioProducto)
        {
            this.repositorioCompra = repositorioCompra;
            this.repositorioProducto = repositorioProducto;
        }
        public void Add(Compra compra)
        {
            try
            {
                ValidarCompra(compra);
                ValidarDetallesCompra(compra.detalles);
                repositorioCompra.Add(compra);
                repositorioProducto.ActualizarStockCompra(compra.detalles);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar la compra: " + ex.Message);
            }
        }
        private void ValidarCompra(Compra compra)
        {
            if (compra == null)
            {
                throw new ArgumentNullException(nameof(compra), "La compra no puede ser nula");
            }
            if (compra.proveedor == null)
            {
                throw new ArgumentNullException(nameof(compra.proveedor), "La compra debe tener un proveedor asociado");
            }
        }
        private void ValidarDetallesCompra(List<DetalleCompra> detalles)
        {
            if (detalles == null || !detalles.Any())
            {
                throw new ArgumentException("La compra debe contener al menos un detalle", nameof(detalles));
            }
            foreach (var detalle in detalles)
            {
                if (detalle.producto == null)
                {
                    throw new ArgumentException("Cada detalle de compra debe tener un producto asociado", nameof(detalle.producto));
                }
                if (detalle.cantidad <= 0)
                {
                    throw new ArgumentException("La cantidad en cada detalle de compra debe ser mayor que cero", nameof(detalle.cantidad));
                }
                if(detalle.cantidad + detalle.producto.stock > detalle.producto.cantidadMaxima)
                {
                    throw new ArgumentException("La cantidad comprada no puede ser superior a la cantidad máxima", nameof(detalle.cantidad));
                }
            }
        }
        public List<Compra> Leer()
        {
            return repositorioCompra.Leer();
        }
        public Compra BuscarPorId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID debe ser un número positivo", nameof(id));
            }
            var compras = repositorioCompra.Leer();
            return compras.FirstOrDefault(c => c.id == id);
        }
        public bool Eliminar(int id)
        {
            return repositorioCompra.Eliminar(id);
        }
    }
}
