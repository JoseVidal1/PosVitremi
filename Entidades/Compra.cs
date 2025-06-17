using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Compra
    {
        public int id { get; set; }
        public DateTime fecha { get; set; }
        public Proveedor proveedor { get; set; }
        public double total { get; set; }
        public string estado { get; set; }
        public List<DetalleCompra> detalles { get; set; }


    }
}
