using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace TPCuatrimestralEquipoV
{
    public partial class NuevaCompra : Page
    {
        // Productos agregados a la compra, hasta confirmarla.
        private List<DetalleCompra> Detalle
        {
            get
            {
                if (Session["detalleCompra"] == null)
                    Session.Add("detalleCompra", new List<DetalleCompra>());
                return (List<DetalleCompra>)Session["detalleCompra"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
            try
            {
                if (!IsPostBack)
                {
                    Session.Remove("detalleCompra");

                    txtFecha.Text = DateTime.Today.ToString("yyyy-MM-dd");

                    ddlProveedor.DataSource = new ProveedorNegocio().listar(true);
                    ddlProveedor.DataValueField = "Id";
                    ddlProveedor.DataTextField = "RazonSocial";
                    ddlProveedor.DataBind();
                    ddlProveedor.Items.Insert(0, new ListItem("Elegí un proveedor", ""));

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
            ddlProducto.Items.Clear();
            lblUltimoCosto.Text = "";

            if (ddlProveedor.SelectedValue == "")
            {
                ddlProducto.Items.Add(new ListItem("Elegí primero un proveedor", ""));
                ddlProducto.Enabled = false;
                return;
            }

            ProductoNegocio negocio = new ProductoNegocio();
            List<Producto> productos = negocio.listarPorProveedor(int.Parse(ddlProveedor.SelectedValue));

            ddlProducto.Items.Add(new ListItem(productos.Count > 0 ? "Elegí un producto" : "Este proveedor no tiene productos asociados", ""));
            foreach (Producto producto in productos)
                ddlProducto.Items.Add(new ListItem(producto.Marca.Descripcion + " " + producto.Nombre, producto.Id.ToString()));
            ddlProducto.Enabled = productos.Count > 0;
        }

        // Con productos agregados no se puede cambiar el proveedor.
        private void mostrarDetalle()
        {
            gvDetalle.DataSource = Detalle;
            gvDetalle.DataBind();

            decimal total = 0;
            foreach (DetalleCompra detalle in Detalle)
                total += detalle.Subtotal;
            lblTotal.Text = total.ToString("C");

            ddlProveedor.Enabled = Detalle.Count == 0;
        }

        protected void ddlProveedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cargarProductos();
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
                lblUltimoCosto.Text = "";
                if (ddlProducto.SelectedValue == "")
                    return;

                Producto producto = new ProductoNegocio().buscarPorId(int.Parse(ddlProducto.SelectedValue));
                if (producto == null)
                    return;

                lblUltimoCosto.Text = producto.TieneCompras
                    ? "Última compra: " + producto.UltimoPrecioCompra.ToString("C")
                    : "Todavía no se compró";
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
                if (!Page.IsValid)
                    return;

                int cantidad;
                decimal costo;
                if (!int.TryParse(txtCantidad.Text, out cantidad) || cantidad < 1 ||
                    !decimal.TryParse(txtCosto.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out costo) || costo <= 0)
                {
                    mostrarError("Revisá la cantidad y el costo.");
                    return;
                }

                int idProducto = int.Parse(ddlProducto.SelectedValue);
                if (Detalle.Exists(x => x.Producto.Id == idProducto))
                {
                    mostrarError("Ese producto ya está en la compra. Si querés cambiarlo, quitalo y volvé a agregarlo.");
                    return;
                }

                Producto producto = new ProductoNegocio().buscarPorId(idProducto);
                if (producto == null)
                {
                    mostrarError("No se encontró el producto.");
                    return;
                }

                DetalleCompra detalle = new DetalleCompra();
                detalle.Producto = producto;
                detalle.Cantidad = cantidad;
                detalle.PrecioUnitario = Math.Round(costo, 2);
                Detalle.Add(detalle);

                ddlProducto.SelectedIndex = 0;
                txtCantidad.Text = "";
                txtCosto.Text = "";
                lblUltimoCosto.Text = "";

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
                if (!Page.IsValid)
                    return;

                DateTime fecha;
                if (!DateTime.TryParse(txtFecha.Text, out fecha))
                {
                    mostrarError("Revisá la fecha.");
                    return;
                }

                int idProveedor = int.Parse(ddlProveedor.SelectedValue);
                string numeroFactura = txtNumeroFactura.Text.Trim();
                CompraNegocio negocio = new CompraNegocio();

                if (fecha > DateTime.Today)
                    cvFecha.IsValid = false;
                if (negocio.existeFactura(idProveedor, numeroFactura))
                    cvFacturaRepetida.IsValid = false;
                if (!Page.IsValid)
                    return;

                if (Detalle.Count == 0)
                {
                    mostrarError("Agregá al menos un producto a la compra.");
                    return;
                }

                Compra compra = new Compra();
                compra.Proveedor = new Proveedor();
                compra.Proveedor.Id = idProveedor;
                compra.NumeroFactura = numeroFactura;
                compra.Fecha = fecha;
                compra.Usuario = (Usuario)Session["usuario"];
                compra.Detalles = Detalle;

                negocio.agregar(compra);

                Session.Remove("detalleCompra");
                Response.Redirect("VerCompra.aspx?id=" + compra.Id, false);
            }
            catch (Exception ex)
            {
                mostrarError(ex.Message);
            }
        }

        private void mostrarError(string mensaje)
        {
            lblError.Text = mensaje;
            lblError.Visible = true;
        }
    }
}
