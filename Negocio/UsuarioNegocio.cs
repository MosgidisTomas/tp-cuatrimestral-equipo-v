using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Dominio;

namespace Negocio
{
    public class UsuarioNegocio
    {
        // PasswordHash no se trae en los listados: la pantalla nunca lo necesita.
        private const string COLUMNAS = "Id, NombreUsuario, Nombre, Apellido, Email, Perfil, Activo";

        public List<Usuario> listar(bool soloActivos = true)
        {
            List<Usuario> lista = new List<Usuario>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "SELECT " + COLUMNAS + " FROM Usuarios";
                if (soloActivos)
                    consulta += " WHERE Activo = 1";
                consulta += " ORDER BY NombreUsuario";

                datos.setearConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                    lista.Add(leerUsuario(datos));

                return lista;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public Usuario buscarPorId(int id)
        {
            Usuario aux = null;
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT " + COLUMNAS + " FROM Usuarios WHERE Id = @Id");
                datos.setearParametro("@Id", id);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                    aux = leerUsuario(datos);

                return aux;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        private Usuario leerUsuario(AccesoDatos datos)
        {
            Usuario aux = new Usuario();
            aux.Id = (int)datos.Lector["Id"];
            aux.NombreUsuario = (string)datos.Lector["NombreUsuario"];
            aux.Nombre = (string)datos.Lector["Nombre"];
            aux.Apellido = (string)datos.Lector["Apellido"];
            aux.Email = (string)datos.Lector["Email"];
            aux.Perfil = (TipoPerfil)(int)datos.Lector["Perfil"];
            aux.Activo = (bool)datos.Lector["Activo"];
            return aux;
        }

        // Recibe la contraseña escrita y guarda solo su hash.
        public void agregar(Usuario nuevo, string password)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("INSERT INTO Usuarios (NombreUsuario, Nombre, Apellido, Email, PasswordHash, Perfil) " +
                                     "VALUES (@NombreUsuario, @Nombre, @Apellido, @Email, @PasswordHash, @Perfil)");
                datos.setearParametro("@NombreUsuario", nuevo.NombreUsuario);
                datos.setearParametro("@Nombre", nuevo.Nombre);
                datos.setearParametro("@Apellido", nuevo.Apellido);
                datos.setearParametro("@Email", nuevo.Email);
                datos.setearParametro("@PasswordHash", calcularHash(password));
                datos.setearParametro("@Perfil", (int)nuevo.Perfil);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // Si passwordNueva viene vacía, la contraseña no se cambia.
        public void modificar(Usuario usuario, string passwordNueva)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "UPDATE Usuarios SET NombreUsuario = @NombreUsuario, Nombre = @Nombre, Apellido = @Apellido, " +
                                  "Email = @Email, Perfil = @Perfil";
                bool cambiaPassword = !string.IsNullOrEmpty(passwordNueva);
                if (cambiaPassword)
                    consulta += ", PasswordHash = @PasswordHash";
                consulta += " WHERE Id = @Id";

                datos.setearConsulta(consulta);
                datos.setearParametro("@NombreUsuario", usuario.NombreUsuario);
                datos.setearParametro("@Nombre", usuario.Nombre);
                datos.setearParametro("@Apellido", usuario.Apellido);
                datos.setearParametro("@Email", usuario.Email);
                datos.setearParametro("@Perfil", (int)usuario.Perfil);
                if (cambiaPassword)
                    datos.setearParametro("@PasswordHash", calcularHash(passwordNueva));
                datos.setearParametro("@Id", usuario.Id);
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
                datos.setearConsulta("UPDATE Usuarios SET Activo = 0 WHERE Id = @Id");
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
                datos.setearConsulta("UPDATE Usuarios SET Activo = 1 WHERE Id = @Id");
                datos.setearParametro("@Id", id);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public bool existeNombreUsuario(string nombreUsuario, int idExcluir)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT COUNT(*) FROM Usuarios WHERE NombreUsuario = @NombreUsuario AND Id <> @Id");
                datos.setearParametro("@NombreUsuario", nombreUsuario);
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
                datos.setearConsulta("SELECT COUNT(*) FROM Usuarios WHERE Email = @Email AND Id <> @Id");
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

        // SHA-256 de la contraseña, en hexadecimal y minúsculas (64 caracteres, como la columna PasswordHash).
        // Se usa al crear o cambiar la contraseña y, en la Etapa 3, en el login para comparar.
        public static string calcularHash(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
    }
}
