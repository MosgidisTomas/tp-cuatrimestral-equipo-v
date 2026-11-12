using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace TPCuatrimestralEquipoV
{
    public partial class NuevaCompra : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
            try
            {
                if (!IsPostBack)
                {
                    txtFecha.Text = DateTime.Today.ToString("yyyy-MM-dd");

                    ddlProveedor.DataSource = new ProveedorNegocio().listar(true);
                    ddlProveedor.DataValueField = "Id";
                    ddlProveedor.DataTextField = "RazonSocial";
                    ddlProveedor.DataBind();
                    ddlProveedor.Items.Insert(0, new ListItem("Elegí un proveedor", ""));

                    cargarProductos();

                    gvDetalle.DataSource = new List<DetalleCompra>();
                    gvDetalle.DataBind();
                }
            }
            catch (Exception ex)
            {
                mostrarError(ex.Message);
            }
        }

        // Solo se ofrecen los productos asociados al proveedor elegido.
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

        // Muestra como ayuda el costo de la última compra del producto elegido.
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

        private void mostrarError(string mensaje)
        {
            lblError.Text = mensaje;
            lblError.Visible = true;
        }
    }
}
