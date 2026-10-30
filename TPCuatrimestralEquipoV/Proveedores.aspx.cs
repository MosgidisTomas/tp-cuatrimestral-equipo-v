using System;
using System.Collections.Generic;
using System.Web.UI;
using Dominio;
using Negocio;

namespace TPCuatrimestralEquipoV
{
    public partial class Proveedores : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                cargarProveedores();
        }

        private void cargarProveedores()
        {
            try
            {
                ProveedorNegocio negocio = new ProveedorNegocio();
                List<Proveedor> lista = negocio.listar(false);
                string filtro = txtBuscar.Text.Trim().ToUpper();

                if (filtro != "")
                    lista = lista.FindAll(x => x.RazonSocial.ToUpper().Contains(filtro) || x.Cuit.Contains(filtro));

                gvProveedores.DataSource = lista;
                gvProveedores.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                lblError.Visible = true;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            cargarProveedores();
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormularioProveedor.aspx", false);
        }

        protected void gvProveedores_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id = gvProveedores.SelectedDataKey.Value.ToString();
            Response.Redirect("FormularioProveedor.aspx?id=" + id, false);
        }
    }
}
