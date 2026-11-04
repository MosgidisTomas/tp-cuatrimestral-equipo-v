using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace TPCuatrimestralEquipoV
{
    public partial class Productos : System.Web.UI.Page
    {
        private ProductoNegocio negocio = new ProductoNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
            if (!IsPostBack)
            {
                try
                {
                    cargarFiltros();
                    cargarGrilla();
                }
                catch (Exception ex)
                {
                    mostrarError(ex);
                }
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            foreach (GridViewRow fila in gvProductos.Rows)
                fila.Attributes["onclick"] = ClientScript.GetPostBackEventReference(new PostBackOptions(gvProductos, "Select$" + fila.RowIndex), true);

            base.Render(writer);
        }

        private void cargarFiltros()
        {
            ddlCategoria.DataSource = new CategoriaNegocio().listar();
            ddlCategoria.DataValueField = "Id";
            ddlCategoria.DataTextField = "Descripcion";
            ddlCategoria.DataBind();

            ddlMarca.DataSource = new MarcaNegocio().listar();
            ddlMarca.DataValueField = "Id";
            ddlMarca.DataTextField = "Descripcion";
            ddlMarca.DataBind();
        }

        private void cargarGrilla()
        {
            try
            {
                IEnumerable<Producto> productos = negocio.listar(false);

                switch (ddlEstado.SelectedValue)
                {
                    case "activos": productos = productos.Where(p => p.Activo); break;
                    case "inactivos": productos = productos.Where(p => !p.Activo); break;
                }

                string texto = txtBuscar.Text.Trim();
                if (texto != "")
                    productos = productos.Where(p =>
                        p.Codigo.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        p.Nombre.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0);

                if (ddlCategoria.SelectedValue != "")
                {
                    int idCat = int.Parse(ddlCategoria.SelectedValue);
                    productos = productos.Where(p => p.Categoria.Id == idCat);
                }

                if (ddlMarca.SelectedValue != "")
                {
                    int idMarca = int.Parse(ddlMarca.SelectedValue);
                    productos = productos.Where(p => p.Marca.Id == idMarca);
                }

                gvProductos.DataSource = productos.ToList();
                gvProductos.DataBind();
            }
            catch (Exception ex)
            {
                mostrarError(ex);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            cargarGrilla();
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            Response.Redirect("NuevoProductoForm.aspx", false);
        }

        protected void gvProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id = gvProductos.SelectedDataKey.Value.ToString();
            Response.Redirect("NuevoProductoForm.aspx?id=" + id, false);
        }

        protected void gvProductos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Producto p = (Producto)e.Row.DataItem;
                if (p.Activo && p.NecesitaReposicion)
                    e.Row.CssClass = "table-warning";
                if (!p.Activo)
                    e.Row.CssClass = "text-muted";
            }
        }

        private void mostrarError(Exception ex)
        {
            lblError.Text = ex.Message;
            lblError.Visible = true;
        }
    }
}
