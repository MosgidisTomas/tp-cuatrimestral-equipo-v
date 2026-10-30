using System;
using System.Collections.Generic;
using System.Linq;
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
            if (!IsPostBack)
            {
                cargarFiltros();
                cargarGrilla();
            }
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
                Response.Write("<script>alert('" + ex.Message.Replace("'", "") + "');</script>");
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            cargarGrilla();
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            Response.Redirect("NuevoProductoForm.aspx");
        }

        protected void gvProductos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandArgument == null) return;
            int id;
            if (!int.TryParse(e.CommandArgument.ToString(), out id)) return;

            try
            {
                switch (e.CommandName)
                {
                    case "Editar":
                        Response.Redirect("NuevoProductoForm.aspx?id=" + id);
                        break;
                    case "Baja":
                        negocio.eliminar(id);
                        cargarGrilla();
                        break;
                    case "Reactivar":
                        negocio.reactivar(id);
                        cargarGrilla();
                        break;
                }
            }
            catch (System.Threading.ThreadAbortException) { throw; }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message.Replace("'", "") + "');</script>");
            }
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
    }
}
