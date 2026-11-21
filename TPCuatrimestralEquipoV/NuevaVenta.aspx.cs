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
        public int IdCliente
        {
            get { return ViewState["IdCliente"] != null ? (int)ViewState["IdCliente"] : 0; }
            set { ViewState["IdCliente"] = value; }
        }

        private List<DetalleVenta> Detalle
        {
            get
            {
                if (Session["detalleVenta"] == null)
                    Session.Add("detalleVenta", new List<DetalleVenta>());
                return (List<DetalleVenta>)Session["detalleVenta"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
            try
            {
                if (!IsPostBack)
                {
                    Session.Remove("detalleVenta");
                    cargarProductos();
                    mostrarDetalle();
                }
            }
            catch (Exception ex)
            {
                mostrarError(ex.Message);
            }
        }

        private void cargarProductos()
        {
            List<Producto> productos = new ProductoNegocio().listarParaVender();

            ddlProducto.Items.Clear();
            ddlProducto.Items.Add(new ListItem(productos.Count > 0 ? "Elegí un producto" : "No hay productos con stock", ""));
            foreach (Producto producto in productos)
                ddlProducto.Items.Add(new ListItem(producto.Marca.Descripcion + " " + producto.Nombre, producto.Id.ToString()));
        }

        private void mostrarDetalle()
        {
            gvDetalle.DataSource = Detalle;
            gvDetalle.DataBind();

            decimal total = 0;
            int unidades = 0;
            foreach (DetalleVenta detalle in Detalle)
            {
                total += detalle.Subtotal;
                unidades += detalle.Cantidad;
            }
            lblTotal.Text = total.ToString("C");
            lblUnidades.Text = unidades.ToString();
        }

        protected void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                IdCliente = 0;
                lblCliente.Text = "—";

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

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlProducto.SelectedValue == "")
                {
                    mostrarError("Elegí un producto.");
                    return;
                }

                int cantidad;
                if (!int.TryParse(txtCantidad.Text, out cantidad) || cantidad < 1)
                {
                    mostrarError("La cantidad tiene que ser un número mayor a 0.");
                    return;
                }

                int idProducto = int.Parse(ddlProducto.SelectedValue);
                if (Detalle.Exists(x => x.Producto.Id == idProducto))
                {
                    mostrarError("Ese producto ya está en la venta. Si querés cambiar la cantidad, quitalo y volvé a agregarlo.");
                    return;
                }

                Producto producto = new ProductoNegocio().buscarPorId(idProducto);
                if (producto == null)
                {
                    mostrarError("No se encontró el producto.");
                    return;
                }

                if (cantidad > producto.StockActual)
                {
                    mostrarError("No hay stock suficiente: de " + producto.Nombre + " quedan " + producto.StockActual + " unidades.");
                    return;
                }

                DetalleVenta detalle = new DetalleVenta();
                detalle.Producto = producto;
                detalle.Cantidad = cantidad;
                detalle.PrecioUnitario = producto.PrecioVenta;
                Detalle.Add(detalle);

                ddlProducto.SelectedIndex = 0;
                txtCantidad.Text = "";
                txtPrecio.Text = "";
                txtDisponible.Text = "";

                mostrarDetalle();
            }
            catch (Exception ex)
            {
                mostrarError(ex.Message);
            }
        }

        protected void gvDetalle_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Detalle.RemoveAt(e.RowIndex);
            mostrarDetalle();
        }

        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (IdCliente == 0)
                {
                    mostrarError("Buscá el cliente antes de confirmar la venta.");
                    return;
                }

                if (Detalle.Count == 0)
                {
                    mostrarError("Agregá al menos un producto a la venta.");
                    return;
                }

                Venta venta = new Venta();
                venta.Cliente = new Cliente();
                venta.Cliente.Id = IdCliente;
                venta.Usuario = (Usuario)Session["usuario"];
                venta.Fecha = DateTime.Now;
                venta.Detalles = Detalle;

                VentaNegocio negocio = new VentaNegocio();
                negocio.agregar(venta);

                Session.Remove("detalleVenta");
                Response.Redirect("Factura.aspx?id=" + venta.Id, false);
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
