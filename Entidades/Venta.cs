using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Venta
    {
        public int id { get; set; }
        public Usuario Usuario { get; set; }
        public Cliente Cliente { get; set; }
        public DateTime fecha { get; set; }
        public List<DetalleVenta> detalles { get; set; }
        public double total { get; set; }
        public string metodoPago { get; set; }  
    }
}
