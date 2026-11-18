using System;
using System.Web.UI;
using Dominio;
using Negocio;

namespace TPCuatrimestralEquipoV
{
    public partial class VerCompra : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
            if (IsPostBack)
                return;

            try
            {
                int id;
                Compra compra = null;
                if (int.TryParse(Request.QueryString["id"], out id))
                    compra = new CompraNegocio().buscarPorId(id);

                // Sin id, con un id inválido o de una compra que no existe: vuelve al listado.
                if (compra == null)
                {
                    Response.Redirect("Compras.aspx", false);
                    return;
                }

                lblProveedor.Text = Server.HtmlEncode(compra.Proveedor.RazonSocial);
                lblCuit.Text = compra.Proveedor.Cuit;
                lblFactura.Text = Server.HtmlEncode(compra.NumeroFactura);
                lblFecha.Text = compra.Fecha.ToString("dd/MM/yyyy");
                lblUsuario.Text = Server.HtmlEncode(compra.Usuario.NombreCompleto);
                lblTotal.Text = compra.Total.ToString("C");

                gvDetalle.DataSource = compra.Detalles;
                gvDetalle.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                lblError.Visible = true;
            }
        }
    }
}
