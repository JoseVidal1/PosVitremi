using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class DetalleCompra
    {
        public int id { get; set; }
        public int idCompra { get; set; }
        public Proveedor Proveedor { get; set; }
        public Producto producto { get; set; }
        public double precioCompra { get; set; }
        public int cantidad { get; set; }
        public double subTotal { get; set; }

    }
}
