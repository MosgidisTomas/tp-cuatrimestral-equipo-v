using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace TPCuatrimestralEquipoV
{
    public partial class Ventas : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
            if (!IsPostBack)
            {
                txtDesde.Text = DateTime.Today.AddMonths(-3).ToString("yyyy-MM-dd");
                txtHasta.Text = DateTime.Today.ToString("yyyy-MM-dd");
                cargarVentas();
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            foreach (GridViewRow fila in gvVentas.Rows)
                fila.Attributes["onclick"] = ClientScript.GetPostBackEventReference(new PostBackOptions(gvVentas, "Select$" + fila.RowIndex), true);

            base.Render(writer);
        }

        private void cargarVentas()
        {
            try
            {
                List<Venta> lista = new List<Venta>();
                DateTime desde;
                DateTime hasta;

                if (!DateTime.TryParse(txtDesde.Text, out desde) || !DateTime.TryParse(txtHasta.Text, out hasta))
                {
                    lblError.Text = "Completá las fechas \"Desde\" y \"Hasta\".";
                    lblError.Visible = true;
                }
                else if (desde > hasta)
                {
                    lblError.Text = "La fecha \"Desde\" no puede ser posterior a la fecha \"Hasta\".";
                    lblError.Visible = true;
                }
                else
                {
                    lista = new VentaNegocio().listar(desde, hasta);

                    string filtro = txtCliente.Text.Trim().ToUpper();
                    if (filtro != "")
                    {
                        string documento = filtro.Replace(".", "");
                        lista = lista.FindAll(x => x.Cliente.Apellido.ToUpper().Contains(filtro) || x.Cliente.Documento.Contains(documento));
                    }
                }

                gvVentas.DataSource = lista;
                gvVentas.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                lblError.Visible = true;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            cargarVentas();
        }

        protected void gvVentas_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id = gvVentas.SelectedDataKey.Value.ToString();
            Response.Redirect("Factura.aspx?id=" + id, false);
        }
    }
}
