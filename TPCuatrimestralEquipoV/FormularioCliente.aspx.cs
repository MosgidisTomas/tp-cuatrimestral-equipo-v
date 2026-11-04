using System;
using System.Web.UI;
using Dominio;
using Negocio;

namespace TPCuatrimestralEquipoV
{
    public partial class FormularioCliente : Page
    {
        public bool EsModificacion { get { return Request.QueryString["id"] != null; } }

        public bool ClienteActivo
        {
            get { return ViewState["ClienteActivo"] != null ? (bool)ViewState["ClienteActivo"] : true; }
            set { ViewState["ClienteActivo"] = value; }
        }

        public string NombreSeleccionado
        {
            get { return ViewState["NombreSeleccionado"] as string ?? ""; }
            set { ViewState["NombreSeleccionado"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack && EsModificacion)
                {
                    int id;
                    Cliente seleccionado = null;
                    if (int.TryParse(Request.QueryString["id"], out id))
                        seleccionado = new ClienteNegocio().buscarPorId(id);

                    if (seleccionado == null)
                    {
                        Response.Redirect("Clientes.aspx", false);
                        return;
                    }

                    lblTitulo.Text = "Editar cliente";
                    txtId.Text = seleccionado.Id.ToString();
                    txtDocumento.Text = seleccionado.Documento;
                    txtNombre.Text = seleccionado.Nombre;
                    txtApellido.Text = seleccionado.Apellido;
                    txtEmail.Text = seleccionado.Email;
                    txtTelefono.Text = seleccionado.Telefono;
                    txtDireccion.Text = seleccionado.Direccion;
                    txtFechaAlta.Text = seleccionado.FechaAlta.ToString("dd/MM/yyyy");
                    ClienteActivo = seleccionado.Activo;
                    NombreSeleccionado = seleccionado.NombreCompleto;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                if (!Page.IsValid)
                    return;

                string documento = txtDocumento.Text.Trim().Replace(".", "");
                string email = txtEmail.Text.Trim();
                ClienteNegocio negocio = new ClienteNegocio();
                int idCliente = EsModificacion ? int.Parse(Request.QueryString["id"]) : 0;

                // Se revisan los dos antes de salir, para mostrar todos los errores juntos.
                if (negocio.existeDocumento(documento, idCliente))
                    cvDocumentoRepetido.IsValid = false;
                if (negocio.existeEmail(email, idCliente))
                    cvEmailRepetido.IsValid = false;
                if (!Page.IsValid)
                    return;

                Cliente cliente = new Cliente();
                cliente.Documento = documento;
                cliente.Nombre = txtNombre.Text.Trim();
                cliente.Apellido = txtApellido.Text.Trim();
                cliente.Email = email;
                cliente.Telefono = string.IsNullOrWhiteSpace(txtTelefono.Text) ? null : txtTelefono.Text.Trim();
                cliente.Direccion = string.IsNullOrWhiteSpace(txtDireccion.Text) ? null : txtDireccion.Text.Trim();

                if (EsModificacion)
                {
                    cliente.Id = idCliente;
                    negocio.modificar(cliente);
                }
                else
                {
                    negocio.agregar(cliente);
                }

                Response.Redirect("Clientes.aspx", false);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected void btnConfirmaBaja_Click(object sender, EventArgs e)
        {
            try
            {
                ClienteNegocio negocio = new ClienteNegocio();
                negocio.eliminar(int.Parse(Request.QueryString["id"]));
                Response.Redirect("Clientes.aspx", false);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected void btnReactivar_Click(object sender, EventArgs e)
        {
            try
            {
                ClienteNegocio negocio = new ClienteNegocio();
                negocio.reactivar(int.Parse(Request.QueryString["id"]));
                Response.Redirect("Clientes.aspx", false);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
    }
}
