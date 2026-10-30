using System;
using System.Web.UI;
using Dominio;
using Negocio;

namespace TPCuatrimestralEquipoV
{
    public partial class Marcas : Page
    {
        public bool ConfirmaBaja { get; set; }

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

        protected void Page_Load(object sender, EventArgs e)
        {
            ConfirmaBaja = false;
            if (!IsPostBack)
                cargarMarcas();
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
                lblError.Text = ex.Message;
                lblError.Visible = true;
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
                    txtDescripcion.Text = seleccionada.Descripcion;
                    txtPaisOrigen.Text = seleccionada.PaisOrigen;
                    lblTituloFormulario.Text = "Editar marca";
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

                MarcaNegocio negocio = new MarcaNegocio();

                if (negocio.existeDescripcion(txtDescripcion.Text.Trim(), IdMarca))
                {
                    lblErrorFormulario.Text = "Ya existe una marca con ese nombre.";
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

                MarcaNegocio negocio = new MarcaNegocio();
                negocio.eliminar(IdMarca);
                limpiarFormulario();
                cargarMarcas();
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
                MarcaNegocio negocio = new MarcaNegocio();
                negocio.reactivar(IdMarca);
                limpiarFormulario();
                cargarMarcas();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                lblError.Visible = true;
            }
        }

        private void limpiarFormulario()
        {
            IdMarca = 0;
            MarcaActiva = true;
            txtDescripcion.Text = "";
            txtPaisOrigen.Text = "";
            lblTituloFormulario.Text = "Nueva marca";
            lblErrorFormulario.Text = "";
            gvMarcas.SelectedIndex = -1;
        }
    }
}
