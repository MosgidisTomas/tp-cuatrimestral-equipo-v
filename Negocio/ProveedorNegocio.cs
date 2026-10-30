using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class ProveedorNegocio
    {
        public List<Proveedor> listar(bool soloActivos = true)
        {
            List<Proveedor> lista = new List<Proveedor>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "SELECT Id, Cuit, RazonSocial, Telefono, Email, Direccion, Activo FROM Proveedores";
                if (soloActivos)
                    consulta += " WHERE Activo = 1";
                consulta += " ORDER BY RazonSocial";

                datos.setearConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Proveedor aux = new Proveedor();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Cuit = (string)datos.Lector["Cuit"];
                    aux.RazonSocial = (string)datos.Lector["RazonSocial"];
                    if (!(datos.Lector["Telefono"] is DBNull)) aux.Telefono = (string)datos.Lector["Telefono"];
                    if (!(datos.Lector["Email"] is DBNull)) aux.Email = (string)datos.Lector["Email"];
                    if (!(datos.Lector["Direccion"] is DBNull)) aux.Direccion = (string)datos.Lector["Direccion"];
                    aux.Activo = (bool)datos.Lector["Activo"];
                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public Proveedor buscarPorId(int id)
        {
            Proveedor aux = null;
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT Id, Cuit, RazonSocial, Telefono, Email, Direccion, Activo FROM Proveedores WHERE Id = @Id");
                datos.setearParametro("@Id", id);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    aux = new Proveedor();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Cuit = (string)datos.Lector["Cuit"];
                    aux.RazonSocial = (string)datos.Lector["RazonSocial"];
                    if (!(datos.Lector["Telefono"] is DBNull)) aux.Telefono = (string)datos.Lector["Telefono"];
                    if (!(datos.Lector["Email"] is DBNull)) aux.Email = (string)datos.Lector["Email"];
                    if (!(datos.Lector["Direccion"] is DBNull)) aux.Direccion = (string)datos.Lector["Direccion"];
                    aux.Activo = (bool)datos.Lector["Activo"];
                }
                return aux;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void agregar(Proveedor nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("INSERT INTO Proveedores (Cuit, RazonSocial, Telefono, Email, Direccion) " +
                                     "VALUES (@Cuit, @RazonSocial, @Telefono, @Email, @Direccion)");
                datos.setearParametro("@Cuit", nuevo.Cuit);
                datos.setearParametro("@RazonSocial", nuevo.RazonSocial);
                datos.setearParametro("@Telefono", nuevo.Telefono);
                datos.setearParametro("@Email", nuevo.Email);
                datos.setearParametro("@Direccion", nuevo.Direccion);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void modificar(Proveedor proveedor)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE Proveedores SET Cuit = @Cuit, RazonSocial = @RazonSocial, Telefono = @Telefono, " +
                                     "Email = @Email, Direccion = @Direccion WHERE Id = @Id");
                datos.setearParametro("@Cuit", proveedor.Cuit);
                datos.setearParametro("@RazonSocial", proveedor.RazonSocial);
                datos.setearParametro("@Telefono", proveedor.Telefono);
                datos.setearParametro("@Email", proveedor.Email);
                datos.setearParametro("@Direccion", proveedor.Direccion);
                datos.setearParametro("@Id", proveedor.Id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
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
                datos.setearConsulta("UPDATE Proveedores SET Activo = 0 WHERE Id = @Id");
                datos.setearParametro("@Id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
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
                datos.setearConsulta("UPDATE Proveedores SET Activo = 1 WHERE Id = @Id");
                datos.setearParametro("@Id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public bool existeCuit(string cuit, int idExcluir)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT COUNT(*) FROM Proveedores WHERE Cuit = @Cuit AND Id <> @Id");
                datos.setearParametro("@Cuit", cuit);
                datos.setearParametro("@Id", idExcluir);
                int cantidad = Convert.ToInt32(datos.ejecutarAccionEscalar());
                return cantidad > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
