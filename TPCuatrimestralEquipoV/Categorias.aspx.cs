using System;
using System.Collections.Generic;
using System.Web.UI;
using Dominio;
using Negocio;

namespace TPCuatrimestralEquipoV
{
    public partial class Categorias : Page
    {
        public bool ConfirmaBaja { get; set; }

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

        protected void Page_Load(object sender, EventArgs e)
        {
            ConfirmaBaja = false;
            if (!IsPostBack)
                cargarCategorias();
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
                lblError.Text = ex.Message;
                lblError.Visible = true;
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
                    txtDescripcion.Text = seleccionada.Descripcion;
                    lblTituloFormulario.Text = "Editar categoría";
                    lblErrorFormulario.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                lblError.Visible = true;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                lblErrorFormulario.Text = "";

                if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
                {
                    lblErrorFormulario.Text = "El nombre es obligatorio.";
                    return;
                }

                CategoriaNegocio negocio = new CategoriaNegocio();

                if (negocio.existeDescripcion(txtDescripcion.Text.Trim(), IdCategoria))
                {
                    lblErrorFormulario.Text = "Ya existe una categoría con ese nombre.";
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
                lblError.Text = ex.Message;
                lblError.Visible = true;
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiarFormulario();
        }

        protected void btnBaja_Click(object sender, EventArgs e)
        {
            ConfirmaBaja = true;
        }

        protected void btnConfirmaBaja_Click(object sender, EventArgs e)
        {
            try
            {
                if (!chkConfirmaBaja.Checked)
                {
                    ConfirmaBaja = true;
                    lblErrorFormulario.Text = "Marcá \"Confirmar baja\" para poder darla de baja.";
                    return;
                }

                CategoriaNegocio negocio = new CategoriaNegocio();
                negocio.eliminar(IdCategoria);
                limpiarFormulario();
                cargarCategorias();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                lblError.Visible = true;
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
                lblError.Text = ex.Message;
                lblError.Visible = true;
            }
        }

        private void limpiarFormulario()
        {
            IdCategoria = 0;
            CategoriaActiva = true;
            txtDescripcion.Text = "";
            lblTituloFormulario.Text = "Nueva categoría";
            lblErrorFormulario.Text = "";
            gvCategorias.SelectedIndex = -1;
        }
    }
}
