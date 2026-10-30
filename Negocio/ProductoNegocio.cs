using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Negocio
{
    public class ProductoNegocio
    {
        public List<Producto> listar(bool Activo = true)
        {
            List<Producto> lista = new List<Producto>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = @"SELECT P.Id,
                                   P.Codigo,
                                   P.Nombre,
                                   P.Descripcion,
                                   P.UrlImagen,
                                   P.StockActual,
                                   P.StockMinimo,
                                   P.Activo,
                                   P.IdMarca,
                                   M.Descripcion AS Marca,
                                   P.IdCategoria,
                                   C.Descripcion AS Categoria
                            FROM Productos P
                            INNER JOIN Marcas M ON M.Id = P.IdMarca
                            INNER JOIN Categorias C ON C.Id = P.IdCategoria";

                if (Activo)
                    consulta += " WHERE P.Activo = 1";

                consulta += " ORDER BY P.Nombre";

                datos.setearConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Producto aux = new Producto();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.StockActual = (int)datos.Lector["StockActual"];
                    aux.StockMinimo = (int)datos.Lector["StockMinimo"];
                    aux.Activo = (bool)datos.Lector["Activo"];

                    // Columnas que pueden ser NULL
                    if (!(datos.Lector["Descripcion"] is DBNull))
                        aux.Descripcion = (string)datos.Lector["Descripcion"];
                    if (!(datos.Lector["UrlImagen"] is DBNull))
                        aux.UrlImagen = (string)datos.Lector["UrlImagen"];

                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];

                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];

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
        public void agregar(Producto nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.iniciarTransaccion();

                // 1) Insertar el producto y obtener su Id
                datos.setearConsulta(@"INSERT INTO Productos
                    (Codigo, Nombre, Descripcion, UrlImagen, IdMarca, IdCategoria,
                    StockActual, StockMinimo, PorcentajeGananacia, Activo)
                OUTPUT INSERTED.Id
                VALUES
                    (@codigo, @nombre, @descripcion, @urlImagen, @idMarca, @idCategoria,
                     @stockActual, @stockMinimo, @porcentajeGanancia, 1)");

                datos.setearParametro("@codigo", nuevo.Codigo);
                datos.setearParametro("@nombre", nuevo.Nombre);
                datos.setearParametro("@descripcion", nuevo.Descripcion);
                datos.setearParametro("@urlImagen", nuevo.UrlImagen);
                datos.setearParametro("@idMarca", nuevo.Marca.Id);
                datos.setearParametro("@idCategoria", nuevo.Categoria.Id);
                datos.setearParametro("@stockActual", nuevo.StockActual);
                datos.setearParametro("@stockMinimo", nuevo.StockMinimo);
                datos.setearParametro("@porcentajeGanancia", nuevo.PorcentajeGanancia);

                nuevo.Id = Convert.ToInt32(datos.ejecutarAccionEscalar());

                // 2) Insertar las relaciones con proveedores (sin repetidos)
                foreach (int idProveedor in nuevo.Proveedores.Select(p => p.Id).Distinct())
                {
                    datos.setearConsulta(@"INSERT INTO ProductoProveedor (IdProducto, IdProveedor)
                                   VALUES (@idProducto, @idProveedor)");
                    datos.setearParametro("@idProducto", nuevo.Id);
                    datos.setearParametro("@idProveedor", idProveedor);
                    datos.ejecutarAccion();
                }

                datos.confirmarTransaccion();
            }
            catch (Exception)
            {
                datos.cancelarTransaccion();
                throw;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void modificar(Producto producto)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.iniciarTransaccion();

                datos.setearConsulta(@"UPDATE Productos SET
                                    Codigo = @codigo,
                                    Nombre = @nombre,
                                    Descripcion = @descripcion,
                                    UrlImagen = @urlImagen,
                                    IdMarca = @idMarca,
                                    IdCategoria = @idCategoria,
                                    StockMinimo = @stockMinimo,
                                    PorcentajeGanancia = @porcentajeGanancia
                               WHERE Id = @id");

                datos.setearParametro("@codigo", producto.Codigo);
                datos.setearParametro("@nombre", producto.Nombre);
                datos.setearParametro("@descripcion", producto.Descripcion);
                datos.setearParametro("@urlImagen", producto.UrlImagen);
                datos.setearParametro("@idMarca", producto.Marca.Id);
                datos.setearParametro("@idCategoria", producto.Categoria.Id);
                datos.setearParametro("@stockMinimo", producto.StockMinimo);
                datos.setearParametro("@porcentajeGanancia", producto.PorcentajeGanancia);
                datos.setearParametro("@id", producto.Id);
                datos.ejecutarAccion();

                datos.setearConsulta("DELETE FROM ProductoProveedor WHERE IdProducto = @idProducto");
                datos.setearParametro("@idProducto", producto.Id);
                datos.ejecutarAccion();

                foreach (int idProveedor in producto.Proveedores.Select(p => p.Id).Distinct())
                {
                    datos.setearConsulta(@"INSERT INTO ProductoProveedor (IdProducto, IdProveedor)
                                   VALUES (@idProducto, @idProveedor)");
                    datos.setearParametro("@idProducto", producto.Id);
                    datos.setearParametro("@idProveedor", idProveedor);
                    datos.ejecutarAccion();
                }

                datos.confirmarTransaccion();
            }
            catch (Exception)
            {
                datos.cancelarTransaccion();
                throw;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void eliminar(int id)
        {
            cambiarEstado(id, false);
        }

        public void reactivar(int id)
        {
            cambiarEstado(id, true);
        }

        private void cambiarEstado(int id, bool activo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE Productos SET Activo = @activo WHERE Id = @id");
                datos.setearParametro("@activo", activo);
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void actualizarStock(int idProducto, int cantidad)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"UPDATE Productos
                               SET StockActual = StockActual + @cantidad
                               OUTPUT INSERTED.StockActual
                               WHERE Id = @id
                                 AND StockActual + @cantidad >= 0");
                datos.setearParametro("@cantidad", cantidad);
                datos.setearParametro("@id", idProducto);

                if (datos.ejecutarAccionEscalar() == null)
                    throw new InvalidOperationException(
                        "No se pudo actualizar el stock: el producto no existe o el stock resultante sería negativo.");
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        private const string ConsultaBase = @"SELECT   P.Id,
                                                       P.Codigo,
                                                       P.Nombre,
                                                       P.Descripcion,
                                                       P.UrlImagen,
                                                       P.StockActual,
                                                       P.StockMinimo,
                                                       P.PorcentajeGananacia,
                                                       P.Activo,
                                                       P.IdMarca,
                                                       M.Descripcion AS Marca,
                                                       P.IdCategoria,
                                                       C.Descripcion AS Categoria,
                                                       ISNULL((SELECT TOP 1 DC.PrecioUnitario
                                                               FROM DetalleCompras DC
                                                               INNER JOIN Compras CO ON CO.Id = DC.IdCompra
                                                               WHERE DC.IdProducto = P.Id
                                                               ORDER BY CO.Fecha DESC), 0) AS UltimoPrecioCompra,
                                                       CASE WHEN EXISTS (SELECT 1 FROM DetalleCompras DC WHERE DC.IdProducto = P.Id)
                                                            THEN 1 ELSE 0 END AS TieneCompras
                                                FROM Productos P
                                                INNER JOIN Marcas M ON M.Id = P.IdMarca
                                                INNER JOIN Categorias C ON C.Id = P.IdCategoria";

        private Producto mapear(SqlDataReader lector)
        {
            Producto aux = new Producto();
            aux.Id = (int)lector["Id"];
            aux.Codigo = (string)lector["Codigo"];
            aux.Nombre = (string)lector["Nombre"];
            aux.StockActual = (int)lector["StockActual"];
            aux.StockMinimo = (int)lector["StockMinimo"];
            aux.PorcentajeGanancia = (decimal)lector["PorcentajeGanancia"]; 
            aux.Activo = (bool)lector["Activo"];
            aux.UltimoPrecioCompra = Convert.ToDecimal(lector["UltimoPrecioCompra"]);
            aux.TieneCompras = Convert.ToInt32(lector["TieneCompras"]) == 1;

            if (!(lector["Descripcion"] is DBNull))
                aux.Descripcion = (string)lector["Descripcion"];
            if (!(lector["UrlImagen"] is DBNull))
                aux.UrlImagen = (string)lector["UrlImagen"];

            aux.Marca = new Marca();
            aux.Marca.Id = (int)lector["IdMarca"];
            aux.Marca.Descripcion = (string)lector["Marca"];

            aux.Categoria = new Categoria();
            aux.Categoria.Id = (int)lector["IdCategoria"];
            aux.Categoria.Descripcion = (string)lector["Categoria"];

            return aux;
        }

        public Producto buscarPorId(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            Producto producto = null;
            try
            {
                datos.setearConsulta(ConsultaBase + " WHERE P.Id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                    producto = mapear(datos.Lector);
            }
            finally
            {
                datos.cerrarConexion();
            }

            if (producto != null)
                producto.Proveedores = listarProveedores(id);

            return producto;
        }
        private List<Proveedor> listarProveedores(int idProducto)
        {
            List<Proveedor> lista = new List<Proveedor>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"SELECT PR.Id, PR.RazonSocial
                               FROM Proveedores PR
                               INNER JOIN ProductoProveedor PP ON PP.IdProveedor = PR.Id
                               WHERE PP.IdProducto = @idProducto
                               ORDER BY PR.RazonSocial");
                datos.setearParametro("@idProducto", idProducto);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Proveedor aux = new Proveedor();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.RazonSocial = (string)datos.Lector["RazonSocial"];
                    lista.Add(aux);
                }
                return lista;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public bool existeCodigo(string codigo, int idExcluir = 0)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT COUNT(*) FROM Productos WHERE Codigo = @codigo AND Id <> @id");
                datos.setearParametro("@codigo", codigo);
                datos.setearParametro("@id", idExcluir);
                return Convert.ToInt32(datos.ejecutarAccionEscalar()) > 0;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
