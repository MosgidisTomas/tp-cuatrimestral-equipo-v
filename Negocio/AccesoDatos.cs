using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Negocio
{
    public class AccesoDatos
    {
        private const string ClaveConexion = "ConexionUtnHardware";

        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;
        private SqlTransaction transaccion;

        public SqlDataReader Lector
        {
            get { return lector; }
        }

        public AccesoDatos()
        {
            conexion = new SqlConnection(obtenerCadenaConexion());
            comando = new SqlCommand();
            comando.Connection = conexion;
        }

        private static string obtenerCadenaConexion()
        {
            string cadena = ConfigurationManager.AppSettings[ClaveConexion];
            if (string.IsNullOrWhiteSpace(cadena))
                throw new ConfigurationErrorsException(
                    "Falta la cadena de conexión '" + ClaveConexion + "' en la sección appSettings del Web.config.");
            return cadena;
        }

        public void setearConsulta(string consulta)
        {
            comando.CommandType = CommandType.Text;
            comando.CommandText = consulta;
            comando.Parameters.Clear();
        }

        public void setearProcedimiento(string nombre)
        {
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = nombre;
            comando.Parameters.Clear();
        }

        public void setearParametro(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor ?? DBNull.Value);
        }

        public void ejecutarLectura()
        {
            abrirConexion();
            comando.Transaction = transaccion;
            lector = comando.ExecuteReader();
        }

        public void ejecutarAccion()
        {
            abrirConexion();
            comando.Transaction = transaccion;
            comando.ExecuteNonQuery();
        }

        public object ejecutarAccionEscalar()
        {
            abrirConexion();
            comando.Transaction = transaccion;
            return comando.ExecuteScalar();
        }

        public void iniciarTransaccion()
        {
            abrirConexion();
            transaccion = conexion.BeginTransaction();
        }

        public void confirmarTransaccion()
        {
            cerrarLector();
            transaccion.Commit();
            transaccion = null;
        }

        public void cancelarTransaccion()
        {
            cerrarLector();
            if (transaccion != null)
            {
                transaccion.Rollback();
                transaccion = null;
            }
        }

        public void cerrarLector()
        {
            if (lector != null && !lector.IsClosed)
                lector.Close();
        }

        public void cerrarConexion()
        {
            cerrarLector();
            conexion.Close();
        }

        private void abrirConexion()
        {
            if (conexion.State == ConnectionState.Open)
                return;

            try
            {
                conexion.Open();
            }
            catch (SqlException ex)
            {
                throw new Exception(
                    "No se pudo conectar a la base de datos. Revisá que SQL Server esté corriendo, que exista la base UtnHardware " +
                    "y la cadena de conexión '" + ClaveConexion + "' en Web.config (o en appSettings.local.config).", ex);
            }
        }
    }
}
