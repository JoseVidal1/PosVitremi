using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Devolucion
    {
        public int id { get; set; }
        public Venta venta { get; set; }
        public Producto producto { get; set; }
        public int cantidad { get; set; }
        public DateTime fechaDevolucion { get; set; }
        public string motivo { get; set; }

    }
}
