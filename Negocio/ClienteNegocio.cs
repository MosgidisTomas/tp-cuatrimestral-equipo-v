using System;
using System.Collections.Generic;
using Dominio;

namespace Negocio
{
    public class ClienteNegocio
    {
        private const string COLUMNAS = "Id, Documento, Nombre, Apellido, Telefono, Email, Direccion, FechaAlta, Activo";

        public List<Cliente> listar(bool soloActivos = true)
        {
            List<Cliente> lista = new List<Cliente>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "SELECT " + COLUMNAS + " FROM Clientes";
                if (soloActivos)
                    consulta += " WHERE Activo = 1";
                consulta += " ORDER BY Apellido, Nombre";

                datos.setearConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                    lista.Add(leerCliente(datos));

                return lista;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public Cliente buscarPorId(int id)
        {
            Cliente aux = null;
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT " + COLUMNAS + " FROM Clientes WHERE Id = @Id");
                datos.setearParametro("@Id", id);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                    aux = leerCliente(datos);

                return aux;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // Devuelve null si no hay ningún cliente con ese DNI (sin puntos).
        public Cliente buscarPorDocumento(string documento)
        {
            Cliente aux = null;
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT " + COLUMNAS + " FROM Clientes WHERE Documento = @Documento");
                datos.setearParametro("@Documento", documento);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                    aux = leerCliente(datos);

                return aux;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        private Cliente leerCliente(AccesoDatos datos)
        {
            Cliente aux = new Cliente();
            aux.Id = (int)datos.Lector["Id"];
            aux.Documento = (string)datos.Lector["Documento"];
            aux.Nombre = (string)datos.Lector["Nombre"];
            aux.Apellido = (string)datos.Lector["Apellido"];
            if (!(datos.Lector["Telefono"] is DBNull)) aux.Telefono = (string)datos.Lector["Telefono"];
            aux.Email = (string)datos.Lector["Email"];
            if (!(datos.Lector["Direccion"] is DBNull)) aux.Direccion = (string)datos.Lector["Direccion"];
            aux.FechaAlta = (DateTime)datos.Lector["FechaAlta"];
            aux.Activo = (bool)datos.Lector["Activo"];
            return aux;
        }

        public void agregar(Cliente nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("INSERT INTO Clientes (Documento, Nombre, Apellido, Telefono, Email, Direccion, FechaAlta) " +
                                     "VALUES (@Documento, @Nombre, @Apellido, @Telefono, @Email, @Direccion, @FechaAlta)");
                datos.setearParametro("@Documento", nuevo.Documento);
                datos.setearParametro("@Nombre", nuevo.Nombre);
                datos.setearParametro("@Apellido", nuevo.Apellido);
                datos.setearParametro("@Telefono", nuevo.Telefono);
                datos.setearParametro("@Email", nuevo.Email);
                datos.setearParametro("@Direccion", nuevo.Direccion);
                datos.setearParametro("@FechaAlta", DateTime.Today);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // La fecha de alta no se modifica: es la del día en que se registró el cliente.
        public void modificar(Cliente cliente)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE Clientes SET Documento = @Documento, Nombre = @Nombre, Apellido = @Apellido, " +
                                     "Telefono = @Telefono, Email = @Email, Direccion = @Direccion WHERE Id = @Id");
                datos.setearParametro("@Documento", cliente.Documento);
                datos.setearParametro("@Nombre", cliente.Nombre);
                datos.setearParametro("@Apellido", cliente.Apellido);
                datos.setearParametro("@Telefono", cliente.Telefono);
                datos.setearParametro("@Email", cliente.Email);
                datos.setearParametro("@Direccion", cliente.Direccion);
                datos.setearParametro("@Id", cliente.Id);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void eliminar(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE Clientes SET Activo = 0 WHERE Id = @Id");
                datos.setearParametro("@Id", id);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void reactivar(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE Clientes SET Activo = 1 WHERE Id = @Id");
                datos.setearParametro("@Id", id);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public bool existeDocumento(string documento, int idExcluir)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT COUNT(*) FROM Clientes WHERE Documento = @Documento AND Id <> @Id");
                datos.setearParametro("@Documento", documento);
                datos.setearParametro("@Id", idExcluir);
                int cantidad = Convert.ToInt32(datos.ejecutarAccionEscalar());
                return cantidad > 0;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public bool existeEmail(string email, int idExcluir)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT COUNT(*) FROM Clientes WHERE Email = @Email AND Id <> @Id");
                datos.setearParametro("@Email", email);
                datos.setearParametro("@Id", idExcluir);
                int cantidad = Convert.ToInt32(datos.ejecutarAccionEscalar());
                return cantidad > 0;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
