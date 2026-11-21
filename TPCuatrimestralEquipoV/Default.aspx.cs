using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace TPCuatrimestralEquipoV
{
    public partial class _Default : Page
    {
        public bool EsAdmin
        {
            get
            {
                Usuario usuario = (Usuario)Session["usuario"];
                return usuario != null && usuario.EsAdministrador;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
            if (!IsPostBack)
                cargarEstadisticas();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            foreach (GridViewRow fila in gvUltimasVentas.Rows)
                fila.Attributes["onclick"] = ClientScript.GetPostBackEventReference(new PostBackOptions(gvUltimasVentas, "Select$" + fila.RowIndex), true);

            base.Render(writer);
        }

        private void cargarEstadisticas()
        {
            try
            {
                EstadisticaNegocio negocio = new EstadisticaNegocio();
                DateTime hoy = DateTime.Today;
                DateTime manana = hoy.AddDays(1);
                DateTime inicioMes = new DateTime(hoy.Year, hoy.Month, 1);

                lblVentasHoy.Text = negocio.totalVentas(hoy, manana).ToString("C");
                lblVentasHoyDetalle.Text = textoCantidad(negocio.cantidadVentas(hoy, manana), "venta", "ventas");

                List<Venta> ultimas = new VentaNegocio().listar();
                gvUltimasVentas.DataSource = ultimas.Take(5).ToList();
                gvUltimasVentas.DataBind();

                if (!EsAdmin)
                    return;

                decimal totalMes = negocio.totalVentas(inicioMes, manana);
                int cantidadMes = negocio.cantidadVentas(inicioMes, manana);
                lblVentasMes.Text = totalMes.ToString("C");
                lblVentasMesDetalle.Text = textoCantidad(cantidadMes, "venta", "ventas");
                if (cantidadMes > 0)
                    lblVentasMesDetalle.Text += " · promedio " + (totalMes / cantidadMes).ToString("C");

                lblComprasMes.Text = negocio.totalCompras(inicioMes, manana).ToString("C");
                lblComprasMesDetalle.Text = textoCantidad(negocio.cantidadCompras(inicioMes, manana), "compra", "compras");

                lblReponer.Text = new ProductoNegocio().listarAReponer().Count.ToString();

                List<ProductoVendido> masVendidos = negocio.masVendidos(inicioMes, manana, 5);
                if (masVendidos.Count > 0)
                {
                    lblMasVendido.Text = Server.HtmlEncode(masVendidos[0].Nombre);
                    lblMasVendidoDetalle.Text = textoCantidad(masVendidos[0].Unidades, "unidad", "unidades") + " · " + masVendidos[0].Total.ToString("C");
                }
                else
                {
                    lblMasVendido.Text = "—";
                    lblMasVendidoDetalle.Text = "Sin ventas este mes";
                }
                rptMasVendidos.DataSource = masVendidos;
                rptMasVendidos.DataBind();
                rptMasVendidos.Visible = masVendidos.Count > 0;
                lblSinVentasTop.Visible = masVendidos.Count == 0;

                ProductoVendido menosVendido = negocio.menosVendido(inicioMes, manana);
                int sinVentas = negocio.productosSinVentas(inicioMes, manana);
                if (menosVendido != null)
                {
                    lblMenosVendido.Text = Server.HtmlEncode(menosVendido.Nombre);
                    lblMenosVendidoDetalle.Text = textoCantidad(menosVendido.Unidades, "unidad", "unidades") + " · " + textoCantidad(sinVentas, "producto sin ventas", "productos sin ventas");
                }
                else
                {
                    lblMenosVendido.Text = "—";
                    lblMenosVendidoDetalle.Text = textoCantidad(sinVentas, "producto sin ventas", "productos sin ventas");
                }

                VendedorDelMes vendedor = negocio.mejorVendedor(inicioMes, manana);
                if (vendedor != null)
                {
                    lblMejorVendedor.Text = Server.HtmlEncode(vendedor.Nombre);
                    lblMejorVendedorDetalle.Text = textoCantidad(vendedor.Ventas, "venta", "ventas") + " · " + vendedor.Total.ToString("C");
                }
                else
                {
                    lblMejorVendedor.Text = "—";
                    lblMejorVendedorDetalle.Text = "Sin ventas este mes";
                }

                lblClientesNuevos.Text = negocio.clientesNuevos(inicioMes, manana).ToString();

                List<VentaDiaria> ventasDias = negocio.ventasPorDia(hoy.AddDays(-6), manana);
                List<VentaDiaria> dias = new List<VentaDiaria>();
                for (int i = 6; i >= 0; i--)
                {
                    DateTime dia = hoy.AddDays(-i);
                    VentaDiaria encontrada = ventasDias.Find(x => x.Fecha == dia);

                    VentaDiaria aux = new VentaDiaria();
                    aux.Fecha = dia;
                    aux.Total = encontrada != null ? encontrada.Total : 0;
                    dias.Add(aux);
                }

                decimal maximo = dias.Max(x => x.Total);
                foreach (VentaDiaria dia in dias)
                    dia.Porcentaje = maximo > 0 ? (int)Math.Round(dia.Total * 100 / maximo) : 0;

                rptDias.DataSource = dias;
                rptDias.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                lblError.Visible = true;
            }
        }

        private string textoCantidad(int cantidad, string singular, string plural)
        {
            return cantidad + " " + (cantidad == 1 ? singular : plural);
        }

        protected void gvUltimasVentas_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id = gvUltimasVentas.SelectedDataKey.Value.ToString();
            Response.Redirect("Factura.aspx?id=" + id, false);
        }
    }
}
