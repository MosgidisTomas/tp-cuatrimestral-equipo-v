using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace TPCuatrimestralEquipoV
{
    public partial class NuevaVenta : Page
    {
        // Id del cliente encontrado; 0 si todavía no hay cliente. Se usa al confirmar la venta (Etapa 3).
        public int IdCliente
        {
            get { return ViewState["IdCliente"] != null ? (int)ViewState["IdCliente"] : 0; }
            set { ViewState["IdCliente"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
            try
            {
                if (!IsPostBack)
                {
                    cargarProductos();

                    gvDetalle.DataSource = new List<DetalleVenta>();
                    gvDetalle.DataBind();
                }
            }
            catch (Exception ex)
            {
                mostrarError(ex.Message);
            }
        }

        // Solo los productos que se pueden vender: activos, con precio (al menos una compra) y con stock.
        private void cargarProductos()
        {
            List<Producto> productos = new ProductoNegocio().listarParaVender();

            ddlProducto.Items.Clear();
            ddlProducto.Items.Add(new ListItem(productos.Count > 0 ? "Elegí un producto" : "No hay productos con stock", ""));
            foreach (Producto producto in productos)
                ddlProducto.Items.Add(new ListItem(producto.Marca.Descripcion + " " + producto.Nombre, producto.Id.ToString()));
        }

        protected void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                IdCliente = 0;
                lblCliente.Text = "—";

                // El DNI se guarda sin puntos.
                string documento = txtDocumento.Text.Trim().Replace(".", "");
                if (documento == "")
                {
                    mostrarMensajeCliente("Ingresá el DNI del cliente.", false);
                    return;
                }

                Cliente cliente = new ClienteNegocio().buscarPorDocumento(documento);
                if (cliente == null)
                {
                    mostrarMensajeCliente("No hay ningún cliente con ese DNI. Registralo primero.", false);
                    return;
                }
                if (!cliente.Activo)
                {
                    mostrarMensajeCliente(cliente.NombreCompleto + " está inactivo: no se le puede vender.", false);
                    return;
                }

                IdCliente = cliente.Id;
                lblCliente.Text = cliente.NombreCompleto;
                mostrarMensajeCliente("Cliente: " + cliente.NombreCompleto + " (" + cliente.Email + ")", true);
            }
            catch (Exception ex)
            {
                mostrarError(ex.Message);
            }
        }

        // Al elegir un producto se muestran su precio de venta (último costo + ganancia) y el stock disponible.
        protected void ddlProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtPrecio.Text = "";
                txtDisponible.Text = "";
                if (ddlProducto.SelectedValue == "")
                    return;

                Producto producto = new ProductoNegocio().buscarPorId(int.Parse(ddlProducto.SelectedValue));
                if (producto == null)
                    return;

                txtPrecio.Text = producto.PrecioVenta.ToString("C");
                txtDisponible.Text = producto.StockActual.ToString();
                txtCantidad.Attributes["max"] = producto.StockActual.ToString();
            }
            catch (Exception ex)
            {
                mostrarError(ex.Message);
            }
        }

        private void mostrarMensajeCliente(string mensaje, bool encontrado)
        {
            lblClienteMensaje.Text = mensaje;
            lblClienteMensaje.CssClass = encontrado ? "d-block mt-2 text-success small" : "d-block mt-2 text-danger small";
        }

        private void mostrarError(string mensaje)
        {
            lblError.Text = mensaje;
            lblError.Visible = true;
        }
    }
}
