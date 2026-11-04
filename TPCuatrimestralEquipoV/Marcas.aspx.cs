using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace TPCuatrimestralEquipoV
{
    public partial class Marcas : Page
    {
        public int IdMarca
        {
            get { return ViewState["IdMarca"] != null ? (int)ViewState["IdMarca"] : 0; }
            set { ViewState["IdMarca"] = value; }
        }

        public bool MarcaActiva
        {
            get { return ViewState["MarcaActiva"] != null ? (bool)ViewState["MarcaActiva"] : true; }
            set { ViewState["MarcaActiva"] = value; }
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
                cargarMarcas();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            foreach (GridViewRow fila in gvMarcas.Rows)
                fila.Attributes["onclick"] = ClientScript.GetPostBackEventReference(new PostBackOptions(gvMarcas, "Select$" + fila.RowIndex), true);

            base.Render(writer);
        }

        private void cargarMarcas()
        {
            try
            {
                MarcaNegocio negocio = new MarcaNegocio();
                gvMarcas.DataSource = negocio.listar(false);
                gvMarcas.DataBind();
            }
            catch (Exception ex)
            {
                mostrarError(ex);
            }
        }

        protected void gvMarcas_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int id = (int)gvMarcas.SelectedDataKey.Value;
                MarcaNegocio negocio = new MarcaNegocio();
                Marca seleccionada = negocio.buscarPorId(id);

                if (seleccionada != null)
                {
                    IdMarca = seleccionada.Id;
                    MarcaActiva = seleccionada.Activo;
                    NombreSeleccionado = seleccionada.Descripcion;
                    txtDescripcion.Text = seleccionada.Descripcion;
                    txtPaisOrigen.Text = seleccionada.PaisOrigen;
                    lblTituloFormulario.Text = "Editar marca";
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

                MarcaNegocio negocio = new MarcaNegocio();

                if (negocio.existeDescripcion(txtDescripcion.Text.Trim(), IdMarca))
                {
                    cvDescripcionRepetida.IsValid = false;
                    return;
                }

                Marca marca = new Marca();
                marca.Descripcion = txtDescripcion.Text.Trim();
                marca.PaisOrigen = string.IsNullOrWhiteSpace(txtPaisOrigen.Text) ? null : txtPaisOrigen.Text.Trim();

                if (IdMarca != 0)
                {
                    marca.Id = IdMarca;
                    negocio.modificar(marca);
                }
                else
                {
                    negocio.agregar(marca);
                }

                limpiarFormulario();
                cargarMarcas();
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
                MarcaNegocio negocio = new MarcaNegocio();
                negocio.eliminar(IdMarca);
                limpiarFormulario();
                cargarMarcas();
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
                MarcaNegocio negocio = new MarcaNegocio();
                negocio.reactivar(IdMarca);
                limpiarFormulario();
                cargarMarcas();
            }
            catch (Exception ex)
            {
                mostrarError(ex);
            }
        }

        private void limpiarFormulario()
        {
            IdMarca = 0;
            MarcaActiva = true;
            NombreSeleccionado = "";
            txtDescripcion.Text = "";
            txtPaisOrigen.Text = "";
            lblTituloFormulario.Text = "Nueva marca";
            btnLimpiar.Text = "Limpiar";
            gvMarcas.SelectedIndex = -1;
        }

        private void mostrarError(Exception ex)
        {
            lblError.Text = ex.Message;
            lblError.Visible = true;
        }
    }
}
