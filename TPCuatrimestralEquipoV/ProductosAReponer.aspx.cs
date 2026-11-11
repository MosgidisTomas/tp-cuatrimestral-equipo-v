using System;
using System.Collections.Generic;
using System.Web.UI;
using Dominio;
using Negocio;

namespace TPCuatrimestralEquipoV
{
    public partial class ProductosAReponer : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
            if (!IsPostBack)
                cargarProductos();
        }

        private void cargarProductos()
        {
            try
            {
                ProductoNegocio negocio = new ProductoNegocio();
                gvReponer.DataSource = negocio.listarAReponer();
                gvReponer.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                lblError.Visible = true;
            }
        }

        // Los proveedores del producto separados por coma, para la columna "Proveedores".
        protected string textoProveedores(object proveedores)
        {
            List<string> nombres = new List<string>();
            foreach (Proveedor proveedor in (List<Proveedor>)proveedores)
                nombres.Add(proveedor.RazonSocial);
            return string.Join(", ", nombres);
        }
    }
}
