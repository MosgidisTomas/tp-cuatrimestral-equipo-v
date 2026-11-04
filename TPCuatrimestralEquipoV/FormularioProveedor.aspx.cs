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
        public bool EsModificacion { get { return Request.QueryString["id"] != null; } }

        public bool ProveedorActivo
        {
            get { return ViewState["ProveedorActivo"] != null ? (bool)ViewState["ProveedorActivo"] : true; }
            set { ViewState["ProveedorActivo"] = value; }
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
                    Proveedor seleccionado = null;
                    if (int.TryParse(Request.QueryString["id"], out id))
                        seleccionado = new ProveedorNegocio().buscarPorId(id);

                    if (seleccionado == null)
                    {
                        Response.Redirect("Proveedores.aspx", false);
                        return;
                    }

                    lblTitulo.Text = "Editar proveedor";
                    txtId.Text = seleccionado.Id.ToString();
                    txtCuit.Text = seleccionado.Cuit;
                    txtRazonSocial.Text = seleccionado.RazonSocial;
                    txtTelefono.Text = seleccionado.Telefono;
                    txtEmail.Text = seleccionado.Email;
                    txtDireccion.Text = seleccionado.Direccion;
                    ProveedorActivo = seleccionado.Activo;
                    NombreSeleccionado = seleccionado.RazonSocial;
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

                string cuit = txtCuit.Text.Trim().Replace("-", "");
                ProveedorNegocio negocio = new ProveedorNegocio();
                int idProveedor = EsModificacion ? int.Parse(Request.QueryString["id"]) : 0;

                if (negocio.existeCuit(cuit, idProveedor))
                {
                    cvCuitRepetido.IsValid = false;
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

        protected void btnConfirmaBaja_Click(object sender, EventArgs e)
        {
            try
            {
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
