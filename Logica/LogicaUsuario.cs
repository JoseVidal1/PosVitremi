using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class LogicaUsuario
    {
        private readonly Datos.RepositorioUsuario repositorioUsuario;
        public LogicaUsuario(Datos.RepositorioUsuario repositorioUsuario)
        {
            this.repositorioUsuario = repositorioUsuario;
        }
        public void Add(Usuario usuario)
        {
            if (usuario == null)
            {
                throw new ArgumentNullException(nameof(usuario), "El usuario no puede ser nulo");
            }
            if (string.IsNullOrWhiteSpace(usuario.usuario) || string.IsNullOrWhiteSpace(usuario.contraseña))
            {
                throw new ArgumentException("El usuario y la contraseña no pueden estar vacíos");
            }
            repositorioUsuario.Add(usuario);
        }
        public List<Entidades.Usuario> Leer()
        {
            return repositorioUsuario.Leer();
        }
        public Usuario BuscarPorId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID debe ser un número positivo", nameof(id));
            }
            var usuarios = repositorioUsuario.Leer();
            return usuarios.FirstOrDefault(u => u.id == id);
        }
        public bool Eliminar(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID debe ser un número positivo", nameof(id));
            }
            return repositorioUsuario.Eliminar(id);
        }
        public Usuario BuscarPorUsuario(string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario))
            {
                throw new ArgumentException("El nombre de usuario no puede estar vacío", nameof(usuario));
            }
            var usuarios = repositorioUsuario.Leer();
            return usuarios.FirstOrDefault(u => u.usuario.Equals(usuario, StringComparison.OrdinalIgnoreCase));
        }
        public bool ValidarCredenciales(string usuario, string contraseña)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contraseña))
            {
                throw new ArgumentException("El usuario y la contraseña no pueden estar vacíos");
            }
            var usuarioEncontrado = BuscarPorUsuario(usuario);
            return usuarioEncontrado != null && usuarioEncontrado.contraseña == contraseña;
        }
        public List<Usuario> BuscarPorRol(string rol)
        {
            if (string.IsNullOrWhiteSpace(rol))
            {
                throw new ArgumentException("El rol no puede estar vacío", nameof(rol));
            }
            var usuarios = repositorioUsuario.Leer();
            return usuarios.Where(u => u.rol.Equals(rol, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        public void Actualizar(Usuario usuario)
        {
            if (usuario == null)
            {
                throw new ArgumentNullException(nameof(usuario), "El usuario no puede ser nulo");
            }
            if (usuario.id <= 0)
            {
                throw new ArgumentException("El ID del usuario debe ser un número positivo", nameof(usuario.id));
            }
            if (string.IsNullOrWhiteSpace(usuario.usuario) || string.IsNullOrWhiteSpace(usuario.contraseña))
            {
                throw new ArgumentException("El usuario y la contraseña no pueden estar vacíos");
            }
            repositorioUsuario.Actualizar(usuario);
        }
    }
}
