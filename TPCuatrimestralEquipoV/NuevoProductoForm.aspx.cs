using System;
using System.Globalization;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace TPCuatrimestralEquipoV
{
    public partial class NuevoProductoForm : System.Web.UI.Page
    {
        private ProductoNegocio negocio = new ProductoNegocio();

        private int IdProducto
        {
            get
            {
                int id;
                return int.TryParse(Request.QueryString["id"], out id) ? id : 0;
            }
        }

        public bool EsModificacion { get { return IdProducto > 0; } }

        public bool ProductoActivo
        {
            get { return ViewState["ProductoActivo"] != null ? (bool)ViewState["ProductoActivo"] : true; }
            set { ViewState["ProductoActivo"] = value; }
        }

        public string NombreSeleccionado
        {
            get { return ViewState["NombreSeleccionado"] as string ?? ""; }
            set { ViewState["NombreSeleccionado"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            if (Request.QueryString["id"] != null && !EsModificacion)
            {
                Response.Redirect("Productos.aspx", false);
                return;
            }

            try
            {
                cargarCombos();

                if (EsModificacion)
                    cargarProducto();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        private void cargarCombos()
        {
            ddlMarca.DataSource = new MarcaNegocio().listar();
            ddlMarca.DataValueField = "Id";
            ddlMarca.DataTextField = "Descripcion";
            ddlMarca.DataBind();

            ddlCategoria.DataSource = new CategoriaNegocio().listar(true);
            ddlCategoria.DataValueField = "Id";
            ddlCategoria.DataTextField = "Descripcion";
            ddlCategoria.DataBind();

            lbProveedores.DataSource = new ProveedorNegocio().listar();
            lbProveedores.DataValueField = "Id";
            lbProveedores.DataTextField = "RazonSocial";
            lbProveedores.DataBind();
        }

        private void cargarProducto()
        {
            Producto p = negocio.buscarPorId(IdProducto);
            if (p == null)
            {
                Response.Redirect("Productos.aspx", false);
                return;
            }

            lblTitulo.Text = "Modificar producto";
            txtCodigo.Text = p.Codigo;
            txtNombre.Text = p.Nombre;
            txtDescripcion.Text = p.Descripcion;
            txtUrlImagen.Text = p.UrlImagen;
            txtStockMinimo.Text = p.StockMinimo.ToString();
            txtGanancia.Text = p.PorcentajeGanancia.ToString(CultureInfo.CurrentCulture);
            ProductoActivo = p.Activo;
            NombreSeleccionado = p.Nombre;

            seleccionar(ddlMarca, p.Marca.Id);
            seleccionar(ddlCategoria, p.Categoria.Id);

            foreach (Proveedor prov in p.Proveedores)
            {
                ListItem item = lbProveedores.Items.FindByValue(prov.Id.ToString());
                if (item != null) item.Selected = true;
            }
        }

        private void seleccionar(DropDownList ddl, int id)
        {
            ListItem item = ddl.Items.FindByValue(id.ToString());
            if (item != null)
                ddl.SelectedValue = item.Value;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                if (ddlMarca.Items.Count == 0 || ddlCategoria.Items.Count == 0)
                {
                    lblError.Text = "Tiene que haber al menos una marca y una categoría cargadas.";
                    return;
                }

                decimal ganancia;
                int stockMinimo;
                if (!decimal.TryParse(txtGanancia.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out ganancia) ||
                    !int.TryParse(txtStockMinimo.Text, out stockMinimo))
                {
                    lblError.Text = "Revisá la ganancia y el stock mínimo.";
                    return;
                }

                Producto p = new Producto();
                p.Id = IdProducto;
                p.Codigo = txtCodigo.Text.Trim();
                p.Nombre = txtNombre.Text.Trim();
                p.Descripcion = string.IsNullOrWhiteSpace(txtDescripcion.Text) ? null : txtDescripcion.Text.Trim();
                p.UrlImagen = string.IsNullOrWhiteSpace(txtUrlImagen.Text) ? null : txtUrlImagen.Text.Trim();
                p.Marca = new Marca { Id = int.Parse(ddlMarca.SelectedValue) };
                p.Categoria = new Categoria { Id = int.Parse(ddlCategoria.SelectedValue) };
                p.StockMinimo = stockMinimo;
                p.PorcentajeGanancia = ganancia;

                foreach (ListItem item in lbProveedores.Items)
                    if (item.Selected)
                        p.Proveedores.Add(new Proveedor { Id = int.Parse(item.Value) });

                if (negocio.existeCodigo(p.Codigo, p.Id))
                {
                    cvCodigoRepetido.IsValid = false;
                    return;
                }

                if (p.Id == 0)
                    negocio.agregar(p);
                else
                    negocio.modificar(p);

                Response.Redirect("Productos.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
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
                negocio.eliminar(IdProducto);
                Response.Redirect("Productos.aspx", false);
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
                negocio.reactivar(IdProducto);
                Response.Redirect("Productos.aspx", false);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
    }
}
