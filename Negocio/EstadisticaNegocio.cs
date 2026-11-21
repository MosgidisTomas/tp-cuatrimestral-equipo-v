using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class EstadisticaNegocio
    {
        public decimal totalVentas(DateTime desde, DateTime hasta)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"SELECT ISNULL(SUM(D.Cantidad * D.PrecioUnitario), 0)
                                       FROM Ventas V
                                       INNER JOIN DetalleVenta D ON D.IdVenta = V.Id
                                       WHERE V.Fecha >= @desde AND V.Fecha < @hasta");
                datos.setearParametro("@desde", desde);
                datos.setearParametro("@hasta", hasta);
                return Convert.ToDecimal(datos.ejecutarAccionEscalar());
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

        public int cantidadVentas(DateTime desde, DateTime hasta)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT COUNT(*) FROM Ventas WHERE Fecha >= @desde AND Fecha < @hasta");
                datos.setearParametro("@desde", desde);
                datos.setearParametro("@hasta", hasta);
                return Convert.ToInt32(datos.ejecutarAccionEscalar());
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

        public decimal totalCompras(DateTime desde, DateTime hasta)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"SELECT ISNULL(SUM(D.Cantidad * D.PrecioUnitario), 0)
                                       FROM Compras C
                                       INNER JOIN DetalleCompra D ON D.IdCompra = C.Id
                                       WHERE C.Fecha >= @desde AND C.Fecha < @hasta");
                datos.setearParametro("@desde", desde);
                datos.setearParametro("@hasta", hasta);
                return Convert.ToDecimal(datos.ejecutarAccionEscalar());
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

        public int cantidadCompras(DateTime desde, DateTime hasta)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT COUNT(*) FROM Compras WHERE Fecha >= @desde AND Fecha < @hasta");
                datos.setearParametro("@desde", desde);
                datos.setearParametro("@hasta", hasta);
                return Convert.ToInt32(datos.ejecutarAccionEscalar());
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

        public int clientesNuevos(DateTime desde, DateTime hasta)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT COUNT(*) FROM Clientes WHERE FechaAlta >= @desde AND FechaAlta < @hasta");
                datos.setearParametro("@desde", desde);
                datos.setearParametro("@hasta", hasta);
                return Convert.ToInt32(datos.ejecutarAccionEscalar());
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

        public List<ProductoVendido> masVendidos(DateTime desde, DateTime hasta, int cantidad)
        {
            return listarVendidos(desde, hasta, cantidad, "DESC");
        }

        public ProductoVendido menosVendido(DateTime desde, DateTime hasta)
        {
            List<ProductoVendido> lista = listarVendidos(desde, hasta, 1, "ASC");
            return lista.Count > 0 ? lista[0] : null;
        }

        private List<ProductoVendido> listarVendidos(DateTime desde, DateTime hasta, int cantidad, string orden)
        {
            List<ProductoVendido> lista = new List<ProductoVendido>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"SELECT TOP (@cantidad)
                                              M.Descripcion + ' ' + P.Nombre AS Nombre,
                                              SUM(D.Cantidad) AS Unidades,
                                              SUM(D.Cantidad * D.PrecioUnitario) AS Total
                                       FROM DetalleVenta D
                                       INNER JOIN Ventas V ON V.Id = D.IdVenta
                                       INNER JOIN Productos P ON P.Id = D.IdProducto
                                       INNER JOIN Marcas M ON M.Id = P.IdMarca
                                       WHERE V.Fecha >= @desde AND V.Fecha < @hasta
                                       GROUP BY P.Id, M.Descripcion, P.Nombre
                                       ORDER BY Unidades " + orden + ", Total " + orden);
                datos.setearParametro("@cantidad", cantidad);
                datos.setearParametro("@desde", desde);
                datos.setearParametro("@hasta", hasta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    ProductoVendido aux = new ProductoVendido();
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Unidades = (int)datos.Lector["Unidades"];
                    aux.Total = (decimal)datos.Lector["Total"];
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

        public int productosSinVentas(DateTime desde, DateTime hasta)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"SELECT COUNT(*)
                                       FROM Productos P
                                       WHERE P.Activo = 1
                                         AND NOT EXISTS (SELECT 1
                                                         FROM DetalleVenta D
                                                         INNER JOIN Ventas V ON V.Id = D.IdVenta
                                                         WHERE D.IdProducto = P.Id
                                                           AND V.Fecha >= @desde AND V.Fecha < @hasta)");
                datos.setearParametro("@desde", desde);
                datos.setearParametro("@hasta", hasta);
                return Convert.ToInt32(datos.ejecutarAccionEscalar());
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

        public VendedorDelMes mejorVendedor(DateTime desde, DateTime hasta)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"SELECT TOP 1
                                              U.Nombre + ' ' + U.Apellido AS Nombre,
                                              COUNT(DISTINCT V.Id) AS Ventas,
                                              SUM(D.Cantidad * D.PrecioUnitario) AS Total
                                       FROM Ventas V
                                       INNER JOIN DetalleVenta D ON D.IdVenta = V.Id
                                       INNER JOIN Usuarios U ON U.Id = V.IdUsuario
                                       WHERE V.Fecha >= @desde AND V.Fecha < @hasta
                                       GROUP BY U.Id, U.Nombre, U.Apellido
                                       ORDER BY Total DESC");
                datos.setearParametro("@desde", desde);
                datos.setearParametro("@hasta", hasta);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    VendedorDelMes aux = new VendedorDelMes();
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Ventas = (int)datos.Lector["Ventas"];
                    aux.Total = (decimal)datos.Lector["Total"];
                    return aux;
                }

                return null;
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

        public List<VentaDiaria> ventasPorDia(DateTime desde, DateTime hasta)
        {
            List<VentaDiaria> lista = new List<VentaDiaria>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"SELECT CAST(V.Fecha AS DATE) AS Dia,
                                              SUM(D.Cantidad * D.PrecioUnitario) AS Total
                                       FROM Ventas V
                                       INNER JOIN DetalleVenta D ON D.IdVenta = V.Id
                                       WHERE V.Fecha >= @desde AND V.Fecha < @hasta
                                       GROUP BY CAST(V.Fecha AS DATE)");
                datos.setearParametro("@desde", desde);
                datos.setearParametro("@hasta", hasta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    VentaDiaria aux = new VentaDiaria();
                    aux.Fecha = (DateTime)datos.Lector["Dia"];
                    aux.Total = (decimal)datos.Lector["Total"];
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
    }
}
