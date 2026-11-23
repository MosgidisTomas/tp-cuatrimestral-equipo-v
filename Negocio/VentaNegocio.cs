using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class VentaNegocio
    {
        private const string ConsultaBase = @"V.Id,
                                                     V.Fecha,
                                                     V.NumeroFactura,
                                                     C.Id AS IdCliente,
                                                     C.Documento,
                                                     C.Nombre AS NombreCliente,
                                                     C.Apellido AS ApellidoCliente,
                                                     C.Email AS EmailCliente,
                                                     U.Id AS IdUsuario,
                                                     U.NombreUsuario,
                                                     U.Nombre AS NombreVendedor,
                                                     U.Apellido AS ApellidoVendedor
                                              FROM Ventas V
                                              INNER JOIN Clientes C ON C.Id = V.IdCliente
                                              INNER JOIN Usuarios U ON U.Id = V.IdUsuario";

        public List<Venta> listar(DateTime desde, DateTime hasta)
        {
            List<Venta> lista = new List<Venta>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT " + ConsultaBase + " WHERE V.Fecha >= @desde AND V.Fecha < @hasta ORDER BY V.Fecha DESC, V.Id DESC");
                datos.setearParametro("@desde", desde.Date);
                datos.setearParametro("@hasta", hasta.Date.AddDays(1));
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                    lista.Add(leerVenta(datos));
            }
            finally
            {
                datos.cerrarConexion();
            }

            foreach (Venta venta in lista)
                venta.Detalles = listarDetalle(venta.Id);

            return lista;
        }

        public List<Venta> listarUltimas(int cantidad)
        {
            List<Venta> lista = new List<Venta>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT TOP (@cantidad) " + ConsultaBase + " ORDER BY V.Fecha DESC, V.Id DESC");
                datos.setearParametro("@cantidad", cantidad);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                    lista.Add(leerVenta(datos));
            }
            finally
            {
                datos.cerrarConexion();
            }

            foreach (Venta venta in lista)
                venta.Detalles = listarDetalle(venta.Id);

            return lista;
        }

        public Venta buscarPorId(int id)
        {
            Venta venta = null;
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT " + ConsultaBase + " WHERE V.Id = @Id");
                datos.setearParametro("@Id", id);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                    venta = leerVenta(datos);
            }
            finally
            {
                datos.cerrarConexion();
            }

            if (venta != null)
                venta.Detalles = listarDetalle(venta.Id);

            return venta;
        }

        public List<DetalleVenta> listarDetalle(int idVenta)
        {
            List<DetalleVenta> lista = new List<DetalleVenta>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"SELECT D.Id,
                                              D.Cantidad,
                                              D.PrecioUnitario,
                                              P.Id AS IdProducto,
                                              P.Codigo,
                                              P.Nombre,
                                              M.Descripcion AS Marca
                                       FROM DetalleVenta D
                                       INNER JOIN Productos P ON P.Id = D.IdProducto
                                       INNER JOIN Marcas M ON M.Id = P.IdMarca
                                       WHERE D.IdVenta = @IdVenta
                                       ORDER BY D.Id");
                datos.setearParametro("@IdVenta", idVenta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    DetalleVenta detalle = new DetalleVenta();
                    detalle.Id = (int)datos.Lector["Id"];
                    detalle.Cantidad = (int)datos.Lector["Cantidad"];
                    detalle.PrecioUnitario = (decimal)datos.Lector["PrecioUnitario"];

                    detalle.Producto = new Producto();
                    detalle.Producto.Id = (int)datos.Lector["IdProducto"];
                    detalle.Producto.Codigo = (string)datos.Lector["Codigo"];
                    detalle.Producto.Nombre = (string)datos.Lector["Nombre"];
                    detalle.Producto.Marca = new Marca();
                    detalle.Producto.Marca.Descripcion = (string)datos.Lector["Marca"];

                    lista.Add(detalle);
                }

                return lista;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        private Venta leerVenta(AccesoDatos datos)
        {
            Venta venta = new Venta();
            venta.Id = (int)datos.Lector["Id"];
            venta.Fecha = (DateTime)datos.Lector["Fecha"];
            venta.NumeroFactura = (int)datos.Lector["NumeroFactura"];

            venta.Cliente = new Cliente();
            venta.Cliente.Id = (int)datos.Lector["IdCliente"];
            venta.Cliente.Documento = (string)datos.Lector["Documento"];
            venta.Cliente.Nombre = (string)datos.Lector["NombreCliente"];
            venta.Cliente.Apellido = (string)datos.Lector["ApellidoCliente"];
            venta.Cliente.Email = (string)datos.Lector["EmailCliente"];

            venta.Usuario = new Usuario();
            venta.Usuario.Id = (int)datos.Lector["IdUsuario"];
            venta.Usuario.NombreUsuario = (string)datos.Lector["NombreUsuario"];
            venta.Usuario.Nombre = (string)datos.Lector["NombreVendedor"];
            venta.Usuario.Apellido = (string)datos.Lector["ApellidoVendedor"];

            return venta;
        }

        public void agregar(Venta nueva)
        {
            if (nueva.Detalles.Count == 0)
                throw new InvalidOperationException("La venta tiene que tener al menos un producto.");

            Cliente cliente = new ClienteNegocio().buscarPorId(nueva.Cliente.Id);
            if (cliente == null || !cliente.Activo)
                throw new InvalidOperationException("El cliente fue dado de baja: no se le puede vender.");

            ProductoNegocio productoNegocio = new ProductoNegocio();
            foreach (DetalleVenta detalle in nueva.Detalles)
            {
                Producto producto = productoNegocio.buscarPorId(detalle.Producto.Id);
                if (producto == null || !producto.Activo)
                    throw new InvalidOperationException(detalle.Producto.Nombre + " fue dado de baja: quitalo de la venta.");
            }

            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.iniciarTransaccion();

                datos.setearConsulta("SELECT ISNULL(MAX(NumeroFactura), 0) + 1 FROM Ventas");
                nueva.NumeroFactura = Convert.ToInt32(datos.ejecutarAccionEscalar());

                datos.setearConsulta(@"INSERT INTO Ventas (IdCliente, IdUsuario, Fecha, NumeroFactura)
                                       OUTPUT INSERTED.Id
                                       VALUES (@idCliente, @idUsuario, @fecha, @numeroFactura)");
                datos.setearParametro("@idCliente", nueva.Cliente.Id);
                datos.setearParametro("@idUsuario", nueva.Usuario.Id);
                datos.setearParametro("@fecha", nueva.Fecha);
                datos.setearParametro("@numeroFactura", nueva.NumeroFactura);
                nueva.Id = Convert.ToInt32(datos.ejecutarAccionEscalar());

                foreach (DetalleVenta detalle in nueva.Detalles)
                {
                    datos.setearConsulta(@"INSERT INTO DetalleVenta (IdVenta, IdProducto, Cantidad, PrecioUnitario)
                                           VALUES (@idVenta, @idProducto, @cantidad, @precioUnitario)");
                    datos.setearParametro("@idVenta", nueva.Id);
                    datos.setearParametro("@idProducto", detalle.Producto.Id);
                    datos.setearParametro("@cantidad", detalle.Cantidad);
                    datos.setearParametro("@precioUnitario", detalle.PrecioUnitario);
                    datos.ejecutarAccion();

                    datos.setearConsulta(@"UPDATE Productos SET StockActual = StockActual - @cantidad
                                           OUTPUT INSERTED.StockActual
                                           WHERE Id = @idProducto AND StockActual >= @cantidad");
                    datos.setearParametro("@cantidad", detalle.Cantidad);
                    datos.setearParametro("@idProducto", detalle.Producto.Id);
                    if (datos.ejecutarAccionEscalar() == null)
                        throw new InvalidOperationException("No hay stock suficiente de " + detalle.Producto.Nombre + ".");

                    datos.setearConsulta(@"INSERT INTO MovimientosStock (IdProducto, Fecha, Tipo, Cantidad, IdVenta)
                                           VALUES (@idProducto, @fecha, @tipo, @cantidad, @idVenta)");
                    datos.setearParametro("@idProducto", detalle.Producto.Id);
                    datos.setearParametro("@fecha", nueva.Fecha);
                    datos.setearParametro("@tipo", (int)TipoMovimiento.Venta);
                    datos.setearParametro("@cantidad", detalle.Cantidad);
                    datos.setearParametro("@idVenta", nueva.Id);
                    datos.ejecutarAccion();
                }

                datos.confirmarTransaccion();
            }
            catch (SqlException ex)
            {
                datos.cancelarTransaccion();
                if (ex.Number == 2627 || ex.Number == 2601)
                    throw new InvalidOperationException("Otra venta se registró al mismo tiempo. Tocá \"Confirmar venta\" de nuevo.");
                throw;
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
    }
}
