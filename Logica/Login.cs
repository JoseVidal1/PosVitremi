using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class Login
    {
        private readonly RepositorioUsuario repositorioUsuario;
        public Login(RepositorioUsuario repositorioUsuario)
        {
            this.repositorioUsuario = repositorioUsuario;
        }
        public Usuario Autenticar(string username, string password)
        {
            return repositorioUsuario.Login(username, password);
        }
    }
}
