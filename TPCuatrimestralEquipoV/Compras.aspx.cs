using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace TPCuatrimestralEquipoV
{
    public partial class Compras : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
            if (!IsPostBack)
            {
                txtDesde.Text = DateTime.Today.AddMonths(-3).ToString("yyyy-MM-dd");
                txtHasta.Text = DateTime.Today.ToString("yyyy-MM-dd");
                cargarCompras();
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            foreach (GridViewRow fila in gvCompras.Rows)
                fila.Attributes["onclick"] = ClientScript.GetPostBackEventReference(new PostBackOptions(gvCompras, "Select$" + fila.RowIndex), true);

            base.Render(writer);
        }

        private void cargarCompras()
        {
            try
            {
                List<Compra> lista = new List<Compra>();
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
                    lista = new CompraNegocio().listar(desde, hasta);

                    string filtro = txtProveedor.Text.Trim().ToUpper();
                    if (filtro != "")
                    {
                        string cuit = filtro.Replace("-", "");
                        lista = lista.FindAll(x => x.Proveedor.RazonSocial.ToUpper().Contains(filtro) || x.Proveedor.Cuit.Contains(cuit));
                    }
                }

                gvCompras.DataSource = lista;
                gvCompras.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                lblError.Visible = true;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            cargarCompras();
        }

        protected void gvCompras_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id = gvCompras.SelectedDataKey.Value.ToString();
            Response.Redirect("VerCompra.aspx?id=" + id, false);
        }
    }
}
