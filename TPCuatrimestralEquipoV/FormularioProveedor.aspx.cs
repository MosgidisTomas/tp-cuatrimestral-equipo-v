using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Dominio;
using Negocio;

namespace TPCuatrimestralEquipoV
{
    public partial class FormularioProveedor : Page
    {
        public bool ConfirmaBaja { get; set; }
        public bool EsModificacion { get { return Request.QueryString["id"] != null; } }

        public bool ProveedorActivo
        {
            get { return ViewState["ProveedorActivo"] != null ? (bool)ViewState["ProveedorActivo"] : true; }
            set { ViewState["ProveedorActivo"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ConfirmaBaja = false;
            try
            {
                if (!IsPostBack && EsModificacion)
                {
                    ProveedorNegocio negocio = new ProveedorNegocio();
                    Proveedor seleccionado = negocio.buscarPorId(int.Parse(Request.QueryString["id"]));

                    if (seleccionado != null)
                    {
                        lblTitulo.Text = "Editar proveedor";
                        txtId.Text = seleccionado.Id.ToString();
                        txtCuit.Text = seleccionado.Cuit;
                        txtRazonSocial.Text = seleccionado.RazonSocial;
                        txtTelefono.Text = seleccionado.Telefono;
                        txtEmail.Text = seleccionado.Email;
                        txtDireccion.Text = seleccionado.Direccion;
                        ProveedorActivo = seleccionado.Activo;
                    }
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
                string cuit = txtCuit.Text.Trim().Replace("-", "");

                if (cuit.Length != 11 || !cuit.All(char.IsDigit))
                {
                    lblError.Text = "El CUIT tiene que tener 11 números.";
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtRazonSocial.Text))
                {
                    lblError.Text = "La razón social es obligatoria.";
                    return;
                }

                if (txtEmail.Text.Trim() != "" && !txtEmail.Text.Contains("@"))
                {
                    lblError.Text = "El email no es válido.";
                    return;
                }

                ProveedorNegocio negocio = new ProveedorNegocio();
                int idProveedor = EsModificacion ? int.Parse(Request.QueryString["id"]) : 0;

                if (negocio.existeCuit(cuit, idProveedor))
                {
                    lblError.Text = "Ya existe un proveedor con ese CUIT.";
                    return;
                }

                Proveedor proveedor = new Proveedor();
                proveedor.Cuit = cuit;
                proveedor.RazonSocial = txtRazonSocial.Text.Trim();
                proveedor.Telefono = string.IsNullOrWhiteSpace(txtTelefono.Text) ? null : txtTelefono.Text.Trim();
                proveedor.Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim();
                proveedor.Direccion = string.IsNullOrWhiteSpace(txtDireccion.Text) ? null : txtDireccion.Text.Trim();

                if (EsModificacion)
                {
                    proveedor.Id = idProveedor;
                    negocio.modificar(proveedor);
                }
                else
                {
                    negocio.agregar(proveedor);
                }

                Response.Redirect("Proveedores.aspx", false);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
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
                    lblError.Text = "Marcá \"Confirmar baja\" para poder darlo de baja.";
                    return;
                }

                ProveedorNegocio negocio = new ProveedorNegocio();
                negocio.eliminar(int.Parse(Request.QueryString["id"]));
                Response.Redirect("Proveedores.aspx", false);
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
                ProveedorNegocio negocio = new ProveedorNegocio();
                negocio.reactivar(int.Parse(Request.QueryString["id"]));
                Response.Redirect("Proveedores.aspx", false);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
    }
}
