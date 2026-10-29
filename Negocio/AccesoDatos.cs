using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Negocio
{
    // Acceso a SQL Server con ADO.NET. Cada clase de negocio crea un AccesoDatos por operación
    // y llama a cerrarConexion() en un finally.
    //
    // Uso típico:
    //   AccesoDatos datos = new AccesoDatos();
    //   try
    //   {
    //       datos.setearProcedimiento("SP_ListarMarcas");
    //       datos.ejecutarLectura();
    //       while (datos.Lector.Read()) { ... }
    //   }
    //   finally
    //   {
    //       datos.cerrarConexion();
    //   }
    public class AccesoDatos
    {
        // Clave de appSettings en Web.config (o en appSettings.local.config, si existe).
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

        // Prepara una consulta SQL. Borra los parámetros del comando anterior,
        // para poder reutilizar el mismo AccesoDatos dentro de una transacción.
        public void setearConsulta(string consulta)
        {
            comando.CommandType = CommandType.Text;
            comando.CommandText = consulta;
            comando.Parameters.Clear();
        }

        // Prepara la llamada a un procedimiento almacenado.
        public void setearProcedimiento(string nombre)
        {
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = nombre;
            comando.Parameters.Clear();
        }

        // Un valor null se manda a la base como NULL.
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

        // Transacciones: para operaciones que tocan varias tablas (compras y ventas).
        //   datos.iniciarTransaccion();
        //   try
        //   {
        //       ... varios setearConsulta / ejecutarAccion ...
        //       datos.confirmarTransaccion();
        //   }
        //   catch
        //   {
        //       datos.cancelarTransaccion();
        //       throw;
        //   }
        //   finally
        //   {
        //       datos.cerrarConexion();
        //   }
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

        // Cierra el lector abierto. Hace falta antes de ejecutar otro comando con la misma conexión
        // (por ejemplo, leer un dato dentro de una transacción y después hacer un INSERT).
        public void cerrarLector()
        {
            if (lector != null && !lector.IsClosed)
                lector.Close();
        }

        public void cerrarConexion()
        {
            cerrarLector();
            // Si quedó una transacción sin confirmar, al cerrar la conexión SQL Server la deshace.
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
