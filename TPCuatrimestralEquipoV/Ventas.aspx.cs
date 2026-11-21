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
                DateTime hoy = DateTime.Today;
                txtDesde.Text = new DateTime(hoy.Year, hoy.Month, 1).ToString("yyyy-MM-dd");
                txtHasta.Text = hoy.ToString("yyyy-MM-dd");
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
                VentaNegocio negocio = new VentaNegocio();
                List<Venta> lista = negocio.listar();

                DateTime desde;
                DateTime hasta;
                bool hayDesde = DateTime.TryParse(txtDesde.Text, out desde);
                bool hayHasta = DateTime.TryParse(txtHasta.Text, out hasta);

                if (hayDesde && hayHasta && desde > hasta)
                {
                    lblError.Text = "La fecha \"Desde\" no puede ser posterior a la fecha \"Hasta\".";
                    lblError.Visible = true;
                    lista = new List<Venta>();
                }
                else
                {
                    if (hayDesde)
                        lista = lista.FindAll(x => x.Fecha >= desde);
                    if (hayHasta)
                        lista = lista.FindAll(x => x.Fecha < hasta.AddDays(1));

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
