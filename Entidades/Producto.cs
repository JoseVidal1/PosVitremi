using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Producto
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public double precioCompra { get; set; }
        public double precioVenta { get; set; }
        public int stock { get; set; }
        public int cantidadMinima { get; set; }
        public int cantidadMaxima { get; set; }
        public Proveedor proveedor { get; set; }
        public Categoria categoria { get; set; }
        public string estado { get; set; }
    }
}
