using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace TPCuatrimestralEquipoV
{
    public partial class Clientes : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
            if (!IsPostBack)
                cargarClientes();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            foreach (GridViewRow fila in gvClientes.Rows)
                fila.Attributes["onclick"] = ClientScript.GetPostBackEventReference(new PostBackOptions(gvClientes, "Select$" + fila.RowIndex), true);

            base.Render(writer);
        }

        private void cargarClientes()
        {
            try
            {
                ClienteNegocio negocio = new ClienteNegocio();
                List<Cliente> lista = negocio.listar(false);
                string filtro = txtBuscar.Text.Trim().ToUpper();

                if (filtro != "")
                {
                    // El DNI se guarda sin puntos: se sacan también del texto buscado.
                    string filtroDocumento = filtro.Replace(".", "");
                    lista = lista.FindAll(x => x.Documento.Contains(filtroDocumento)
                                            || x.Nombre.ToUpper().Contains(filtro)
                                            || x.Apellido.ToUpper().Contains(filtro));
                }

                gvClientes.DataSource = lista;
                gvClientes.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                lblError.Visible = true;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            cargarClientes();
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormularioCliente.aspx", false);
        }

        protected void gvClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id = gvClientes.SelectedDataKey.Value.ToString();
            Response.Redirect("FormularioCliente.aspx?id=" + id, false);
        }
    }
}
