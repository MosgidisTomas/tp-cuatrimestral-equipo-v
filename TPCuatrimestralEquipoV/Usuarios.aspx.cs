using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace TPCuatrimestralEquipoV
{
    public partial class Usuarios : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
            if (!IsPostBack)
                cargarUsuarios();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            foreach (GridViewRow fila in gvUsuarios.Rows)
                fila.Attributes["onclick"] = ClientScript.GetPostBackEventReference(new PostBackOptions(gvUsuarios, "Select$" + fila.RowIndex), true);

            base.Render(writer);
        }

        private void cargarUsuarios()
        {
            try
            {
                UsuarioNegocio negocio = new UsuarioNegocio();
                List<Usuario> lista = negocio.listar(false);
                string filtro = txtBuscar.Text.Trim().ToUpper();

                if (filtro != "")
                {
                    lista = lista.FindAll(x => x.NombreUsuario.ToUpper().Contains(filtro)
                                            || x.Nombre.ToUpper().Contains(filtro)
                                            || x.Apellido.ToUpper().Contains(filtro)
                                            || x.Email.ToUpper().Contains(filtro));
                }

                gvUsuarios.DataSource = lista;
                gvUsuarios.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                lblError.Visible = true;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            cargarUsuarios();
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormularioUsuario.aspx", false);
        }

        protected void gvUsuarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id = gvUsuarios.SelectedDataKey.Value.ToString();
            Response.Redirect("FormularioUsuario.aspx?id=" + id, false);
        }
    }
}
