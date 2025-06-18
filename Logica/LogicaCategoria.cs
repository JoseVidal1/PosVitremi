using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logica
{
    public class LogicaCategoria
    {
        private readonly RepositorioCategoria repositorioCategoria;
        public LogicaCategoria(RepositorioCategoria repositorioCategoria)
        {
            this.repositorioCategoria = repositorioCategoria;
        }
        public void Add(Categoria categoria)
        {
            try
            {
                ValidarCategoria(categoria);
                repositorioCategoria.Add(categoria);
            }catch(Exception ex)
            {
                throw new Exception("Error al agregar la categoría: " + ex.Message);
            }
        }
        private void ValidarCategoria(Categoria categoria)
        {
            if (categoria == null)
            {
                throw new ArgumentNullException(nameof(categoria), "La categoría no puede ser nula");
            }
            if (string.IsNullOrWhiteSpace(categoria.nombre))
            {
                throw new ArgumentException("El nombre de la categoría no puede estar vacío", nameof(categoria.nombre));
            }
        }
        public List<Categoria> Leer()
        {
            return repositorioCategoria.Leer();
        }
        public Categoria BuscarPorId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID debe ser un número positivo", nameof(id));
            }
            var categorias = repositorioCategoria.Leer();
            return categorias.FirstOrDefault(c => c.id == id);
        }
        public bool Eliminar(int id)
        {
            return repositorioCategoria.Eliminar(id);
        }
        public Categoria BuscarPorNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre de la categoría no puede estar vacío", nameof(nombre));
            }
            var categorias = repositorioCategoria.Leer();
            return categorias.FirstOrDefault(c => c.nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
        }
        public bool Actualizar(Categoria categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria.nombre))
            {
                throw new ArgumentException("El nombre de la categoría no puede estar vacío", nameof(categoria.nombre));
            }
            return repositorioCategoria.Actualizar(categoria);
        }
    }
}
