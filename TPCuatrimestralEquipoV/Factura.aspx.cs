using System;
using System.Web.UI;
using Dominio;
using Negocio;

namespace TPCuatrimestralEquipoV
{
    public partial class Factura : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
            if (IsPostBack)
                return;

            try
            {
                int id;
                Venta venta = null;
                if (int.TryParse(Request.QueryString["id"], out id))
                    venta = new VentaNegocio().buscarPorId(id);

                if (venta == null)
                {
                    Response.Redirect("Ventas.aspx", false);
                    return;
                }

                lblNumero.Text = venta.NumeroFacturaFormateado;
                lblFecha.Text = venta.Fecha.ToString("dd/MM/yyyy");
                lblCliente.Text = Server.HtmlEncode(venta.Cliente.Apellido + ", " + venta.Cliente.Nombre);
                lblDocumento.Text = Server.HtmlEncode(venta.Cliente.Documento);
                lblEmail.Text = Server.HtmlEncode(venta.Cliente.Email);
                lblTotal.Text = venta.Total.ToString("C");

                rptDetalle.DataSource = venta.Detalles;
                rptDetalle.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                lblError.Visible = true;
            }
        }
    }
}
