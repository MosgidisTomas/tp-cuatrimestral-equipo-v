using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace TPCuatrimestralEquipoV
{
    public partial class Categorias : Page
    {
        public int IdCategoria
        {
            get { return ViewState["IdCategoria"] != null ? (int)ViewState["IdCategoria"] : 0; }
            set { ViewState["IdCategoria"] = value; }
        }

        public bool CategoriaActiva
        {
            get { return ViewState["CategoriaActiva"] != null ? (bool)ViewState["CategoriaActiva"] : true; }
            set { ViewState["CategoriaActiva"] = value; }
        }

        public string NombreSeleccionado
        {
            get { return ViewState["NombreSeleccionado"] as string ?? ""; }
            set { ViewState["NombreSeleccionado"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
            if (!IsPostBack)
                cargarCategorias();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            foreach (GridViewRow fila in gvCategorias.Rows)
                fila.Attributes["onclick"] = ClientScript.GetPostBackEventReference(new PostBackOptions(gvCategorias, "Select$" + fila.RowIndex), true);

            base.Render(writer);
        }

        private void cargarCategorias()
        {
            try
            {
                CategoriaNegocio negocio = new CategoriaNegocio();
                gvCategorias.DataSource = negocio.listar(false);
                gvCategorias.DataBind();
            }
            catch (Exception ex)
            {
                mostrarError(ex);
            }
        }

        protected void gvCategorias_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int id = (int)gvCategorias.SelectedDataKey.Value;
                CategoriaNegocio negocio = new CategoriaNegocio();
                Categoria seleccionada = negocio.buscarPorId(id);

                if (seleccionada != null)
                {
                    IdCategoria = seleccionada.Id;
                    CategoriaActiva = seleccionada.Activo;
                    NombreSeleccionado = seleccionada.Descripcion;
                    txtDescripcion.Text = seleccionada.Descripcion;
                    lblTituloFormulario.Text = "Editar categoría";
                    btnLimpiar.Text = "Cancelar";
                }
            }
            catch (Exception ex)
            {
                mostrarError(ex);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                CategoriaNegocio negocio = new CategoriaNegocio();

                if (negocio.existeDescripcion(txtDescripcion.Text.Trim(), IdCategoria))
                {
                    cvDescripcionRepetida.IsValid = false;
                    return;
                }

                Categoria categoria = new Categoria();
                categoria.Descripcion = txtDescripcion.Text.Trim();

                if (IdCategoria != 0)
                {
                    categoria.Id = IdCategoria;
                    negocio.modificar(categoria);
                }
                else
                {
                    negocio.agregar(categoria);
                }

                limpiarFormulario();
                cargarCategorias();
            }
            catch (Exception ex)
            {
                mostrarError(ex);
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiarFormulario();
        }

        protected void btnConfirmaBaja_Click(object sender, EventArgs e)
        {
            try
            {
                CategoriaNegocio negocio = new CategoriaNegocio();
                negocio.eliminar(IdCategoria);
                limpiarFormulario();
                cargarCategorias();
            }
            catch (Exception ex)
            {
                mostrarError(ex);
            }
        }

        protected void btnReactivar_Click(object sender, EventArgs e)
        {
            try
            {
                CategoriaNegocio negocio = new CategoriaNegocio();
                negocio.reactivar(IdCategoria);
                limpiarFormulario();
                cargarCategorias();
            }
            catch (Exception ex)
            {
                mostrarError(ex);
            }
        }

        private void limpiarFormulario()
        {
            IdCategoria = 0;
            CategoriaActiva = true;
            NombreSeleccionado = "";
            txtDescripcion.Text = "";
            lblTituloFormulario.Text = "Nueva categoría";
            btnLimpiar.Text = "Limpiar";
            gvCategorias.SelectedIndex = -1;
        }

        private void mostrarError(Exception ex)
        {
            lblError.Text = ex.Message;
            lblError.Visible = true;
        }
    }
}
