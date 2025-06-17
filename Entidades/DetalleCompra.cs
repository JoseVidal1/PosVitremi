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
        public Compra compra { get; set; }
        public Proveedor Proveedor { get; set; }
        public Producto producto { get; set; }
        public decimal precioCompra { get; set; }
        public decimal cantidad { get; set; }
        public decimal subTotal { get; set; }

    }
}
