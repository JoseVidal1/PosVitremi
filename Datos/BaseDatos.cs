using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class BaseDatos
    {
        string cadenaConexion = ConfigurationManager.ConnectionStrings["MiConexion"].ConnectionString;

        protected MySqlConnection conexion;
        public BaseDatos()
        {
            conexion = new MySqlConnection(cadenaConexion);

        }
        public bool AbrirConexion()
        {
            if (conexion.State != ConnectionState.Open)
            {
                conexion.Open();
                return true;
            }
            return false;
        }

        public void CerrarConexion()
        {
            conexion.Close();
        }

        public MySqlConnection ObtenerConexion()
        {
            return conexion;
        }
    }
}
