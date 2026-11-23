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
                cargarCompras();
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
                CompraNegocio negocio = new CompraNegocio();
                List<Compra> lista = negocio.listar();

                DateTime desde;
                DateTime hasta;
                bool hayDesde = DateTime.TryParse(txtDesde.Text, out desde);
                bool hayHasta = DateTime.TryParse(txtHasta.Text, out hasta);

                if (hayDesde && hayHasta && desde > hasta)
                {
                    lblError.Text = "La fecha \"Desde\" no puede ser posterior a la fecha \"Hasta\".";
                    lblError.Visible = true;
                    lista = new List<Compra>();
                }
                else
                {
                    if (hayDesde)
                        lista = lista.FindAll(x => x.Fecha >= desde);
                    if (hayHasta)
                        lista = lista.FindAll(x => x.Fecha <= hasta);

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
