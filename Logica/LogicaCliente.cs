using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class LogicaCliente
    {
        private readonly RepositorioCliente repositorioCliente;
        public LogicaCliente(RepositorioCliente repositorioCliente)
        {
            this.repositorioCliente = repositorioCliente;
        }
        public void Add(Cliente cliente)
        {
            try
            {
                ValidarCliente(cliente);
                repositorioCliente.Add(cliente);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar el cliente: " + ex.Message);
            }
        }
        private void ValidarCliente(Cliente cliente)
        {
            if (cliente == null)
            {
                throw new ArgumentNullException(nameof(cliente), "El cliente no puede ser nulo");
            }
            if (string.IsNullOrWhiteSpace(cliente.nombre))
            {
                throw new ArgumentException("El nombre del cliente no puede estar vacíos");
            }
        }
        public void Actualizar(Cliente cliente)
        {
            if (cliente == null)
            {
                throw new ArgumentNullException(nameof(cliente), "El cliente no puede ser nulo");
            }
            if (cliente.id <= 0)
            {
                throw new ArgumentException("El ID del cliente debe ser un número positivo", nameof(cliente.id));
            }
            repositorioCliente.Actualizar(cliente);
        }
        public List<Cliente> Leer()
        {
            return repositorioCliente.Leer();
        }
        public Cliente BuscarPorId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID debe ser un número positivo", nameof(id));
            }
            var clientes = repositorioCliente.Leer();
            return clientes.FirstOrDefault(c => c.id == id);
        }
        public bool Eliminar(int id)
        {
            return repositorioCliente.Eliminar(id);
        }
    }
}
