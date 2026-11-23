using System;
using System.Collections.Generic;
using Dominio;

namespace Negocio
{
    public class CompraNegocio
    {
        private const string ConsultaBase = @"SELECT C.Id,
                                                     C.Fecha,
                                                     C.NumeroFactura,
                                                     PR.Id AS IdProveedor,
                                                     PR.RazonSocial,
                                                     PR.Cuit,
                                                     U.Id AS IdUsuario,
                                                     U.NombreUsuario,
                                                     U.Nombre,
                                                     U.Apellido
                                              FROM Compras C
                                              INNER JOIN Proveedores PR ON PR.Id = C.IdProveedor
                                              INNER JOIN Usuarios U ON U.Id = C.IdUsuario";

        public List<Compra> listar(DateTime desde, DateTime hasta)
        {
            List<Compra> lista = new List<Compra>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(ConsultaBase + " WHERE C.Fecha >= @desde AND C.Fecha < @hasta ORDER BY C.Fecha DESC, C.Id DESC");
                datos.setearParametro("@desde", desde.Date);
                datos.setearParametro("@hasta", hasta.Date.AddDays(1));
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                    lista.Add(leerCompra(datos));
            }
            finally
            {
                datos.cerrarConexion();
            }

            foreach (Compra compra in lista)
                compra.Detalles = listarDetalle(compra.Id);

            return lista;
        }

        public Compra buscarPorId(int id)
        {
            Compra compra = null;
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(ConsultaBase + " WHERE C.Id = @Id");
                datos.setearParametro("@Id", id);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                    compra = leerCompra(datos);
            }
            finally
            {
                datos.cerrarConexion();
            }

            if (compra != null)
                compra.Detalles = listarDetalle(compra.Id);

            return compra;
        }

        public List<DetalleCompra> listarDetalle(int idCompra)
        {
            List<DetalleCompra> lista = new List<DetalleCompra>();
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
                                       FROM DetalleCompra D
                                       INNER JOIN Productos P ON P.Id = D.IdProducto
                                       INNER JOIN Marcas M ON M.Id = P.IdMarca
                                       WHERE D.IdCompra = @IdCompra
                                       ORDER BY D.Id");
                datos.setearParametro("@IdCompra", idCompra);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    DetalleCompra detalle = new DetalleCompra();
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

        public void agregar(Compra nueva)
        {
            if (nueva.Detalles.Count == 0)
                throw new InvalidOperationException("La compra tiene que tener al menos un producto.");

            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.iniciarTransaccion();

                datos.setearConsulta(@"INSERT INTO Compras (IdProveedor, IdUsuario, Fecha, NumeroFactura)
                                       OUTPUT INSERTED.Id
                                       VALUES (@idProveedor, @idUsuario, @fecha, @numeroFactura)");
                datos.setearParametro("@idProveedor", nueva.Proveedor.Id);
                datos.setearParametro("@idUsuario", nueva.Usuario.Id);
                datos.setearParametro("@fecha", nueva.Fecha);
                datos.setearParametro("@numeroFactura", nueva.NumeroFactura);
                nueva.Id = Convert.ToInt32(datos.ejecutarAccionEscalar());

                foreach (DetalleCompra detalle in nueva.Detalles)
                {
                    datos.setearConsulta(@"INSERT INTO DetalleCompra (IdCompra, IdProducto, Cantidad, PrecioUnitario)
                                           VALUES (@idCompra, @idProducto, @cantidad, @precioUnitario)");
                    datos.setearParametro("@idCompra", nueva.Id);
                    datos.setearParametro("@idProducto", detalle.Producto.Id);
                    datos.setearParametro("@cantidad", detalle.Cantidad);
                    datos.setearParametro("@precioUnitario", detalle.PrecioUnitario);
                    datos.ejecutarAccion();

                    datos.setearConsulta("UPDATE Productos SET StockActual = StockActual + @cantidad WHERE Id = @idProducto");
                    datos.setearParametro("@cantidad", detalle.Cantidad);
                    datos.setearParametro("@idProducto", detalle.Producto.Id);
                    datos.ejecutarAccion();

                    datos.setearConsulta(@"INSERT INTO MovimientosStock (IdProducto, Fecha, Tipo, Cantidad, IdCompra)
                                           VALUES (@idProducto, @fecha, @tipo, @cantidad, @idCompra)");
                    datos.setearParametro("@idProducto", detalle.Producto.Id);
                    datos.setearParametro("@fecha", nueva.Fecha);
                    datos.setearParametro("@tipo", (int)TipoMovimiento.Compra);
                    datos.setearParametro("@cantidad", detalle.Cantidad);
                    datos.setearParametro("@idCompra", nueva.Id);
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

        public bool existeFactura(int idProveedor, string numeroFactura)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT COUNT(*) FROM Compras WHERE IdProveedor = @IdProveedor AND NumeroFactura = @NumeroFactura");
                datos.setearParametro("@IdProveedor", idProveedor);
                datos.setearParametro("@NumeroFactura", numeroFactura);
                int cantidad = Convert.ToInt32(datos.ejecutarAccionEscalar());
                return cantidad > 0;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        private Compra leerCompra(AccesoDatos datos)
        {
            Compra compra = new Compra();
            compra.Id = (int)datos.Lector["Id"];
            compra.Fecha = (DateTime)datos.Lector["Fecha"];
            compra.NumeroFactura = (string)datos.Lector["NumeroFactura"];

            compra.Proveedor = new Proveedor();
            compra.Proveedor.Id = (int)datos.Lector["IdProveedor"];
            compra.Proveedor.RazonSocial = (string)datos.Lector["RazonSocial"];
            compra.Proveedor.Cuit = (string)datos.Lector["Cuit"];

            compra.Usuario = new Usuario();
            compra.Usuario.Id = (int)datos.Lector["IdUsuario"];
            compra.Usuario.NombreUsuario = (string)datos.Lector["NombreUsuario"];
            compra.Usuario.Nombre = (string)datos.Lector["Nombre"];
            compra.Usuario.Apellido = (string)datos.Lector["Apellido"];

            return compra;
        }
    }
}
