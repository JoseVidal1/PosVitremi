using System;
using Datos;
using Entidades;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class LogicaDevolucion
    {
        private readonly RepositorioDevolucion repositorioDevolucion;
        private readonly RepositorioVenta repositorioVenta;
        public LogicaDevolucion(RepositorioDevolucion repositorioDevolucion, RepositorioVenta repositorioVenta)
        {
            this.repositorioDevolucion = repositorioDevolucion;
            this.repositorioVenta = repositorioVenta;
        }
        public void Add(Devolucion devolucion)
        {
            try
            {
                ValidarDevolucion(devolucion);
                repositorioDevolucion.Add(devolucion);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar la devolución: " + ex.Message);
            }
        }
        private void ValidarDevolucion(Devolucion devolucion)
        {
            if (devolucion == null)
            {
                throw new ArgumentNullException(nameof(devolucion), "La devolución no puede ser nula");
            }
            if (devolucion.producto == null)
            {
                throw new ArgumentException("La devolución debe contener al menos un producto", nameof(devolucion.producto));
            }
        }
        public List<Devolucion> Leer()
        {
            return repositorioDevolucion.Leer();
        }
        public Devolucion BuscarPorId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID debe ser un número positivo", nameof(id));
            }
            var devoluciones = repositorioDevolucion.Leer();
            return devoluciones.FirstOrDefault(d => d.id == id);
        }
    }
}
