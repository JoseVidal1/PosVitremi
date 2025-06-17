using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class DetalleVenta
    {
        public int id { get; set; }
        public int ventaId { get; set; }
        public Producto producto { get; set; }
        public int cantidad { get; set; }
        public double precioUnitario { get; set; }
        public double subtotal
        {
            set { subtotal = value; }
            get { return cantidad * precioUnitario; }
        }

    }
}
