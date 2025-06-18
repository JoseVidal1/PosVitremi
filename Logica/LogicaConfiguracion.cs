using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class LogicaConfiguracion
    {
        private readonly RepositorioConfiguracion repositorioConfiguracion;
        public LogicaConfiguracion(RepositorioConfiguracion repositorioConfiguracion)
        {
            this.repositorioConfiguracion = repositorioConfiguracion;
        }
        public List<Configuracion> Leer()
        {
            return repositorioConfiguracion.Leer();
        }
        public Configuracion BuscarPorId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID debe ser un número positivo", nameof(id));
            }
            var configuraciones = repositorioConfiguracion.Leer();
            return configuraciones.FirstOrDefault(c => c.id == id);
        }
        public void Actualizar(Configuracion configuracion)
        {
            if (configuracion == null)
            {
                throw new ArgumentNullException(nameof(configuracion), "La configuración no puede ser nula");
            }
            if (configuracion.id <= 0)
            {
                throw new ArgumentException("El ID de la configuración debe ser un número positivo", nameof(configuracion.id));
            }
            repositorioConfiguracion.Actualizar(configuracion);
        }
    }
}
