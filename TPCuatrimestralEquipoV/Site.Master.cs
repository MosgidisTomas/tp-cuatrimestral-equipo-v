using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Dominio;

namespace TPCuatrimestralEquipoV
{
    public partial class SiteMaster : MasterPage
    {
        private static readonly string[] PaginasVendedor = { "Default", "NuevaVenta", "Ventas", "Factura", "Clientes", "FormularioCliente" };

        public bool EsAdmin
        {
            get
            {
                Usuario usuario = (Usuario)Session["usuario"];
                return usuario != null && usuario.EsAdministrador;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Session.Add("error", "Tenés que iniciar sesión para ingresar.");
                Response.Redirect("~/Error");
            }

            string pagina = Path.GetFileNameWithoutExtension(Request.AppRelativeCurrentExecutionFilePath);
            if (!EsAdmin && !PaginasVendedor.Contains(pagina))
            {
                Session.Add("error", "No tenés permisos para ingresar a esta pantalla. Necesitás perfil Administrador.");
                Response.Redirect("~/Error");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Usuario usuario = (Usuario)Session["usuario"];
            lblUsuario.Text = usuario.NombreCompleto + " · " + usuario.Perfil;
            marcarOpcionActiva();
        }

        protected void lnkCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("~/Login", false);
        }

        private void marcarOpcionActiva()
        {
            string pagina = Path.GetFileNameWithoutExtension(Request.AppRelativeCurrentExecutionFilePath);

            Dictionary<string, HtmlAnchor> opciones = new Dictionary<string, HtmlAnchor>(StringComparer.OrdinalIgnoreCase)
            {
                { "Default", lnkInicio },
                { "NuevaVenta", lnkNuevaVenta },
                { "NuevaCompra", lnkNuevaCompra },
                { "Ventas", lnkVentas },
                { "Factura", lnkVentas },
                { "Compras", lnkCompras },
                { "Productos", lnkProductos },
                { "NuevoProductoForm", lnkProductos },
                { "Marcas", lnkMarcas },
                { "Categorias", lnkCategorias },
                { "ProductosAReponer", lnkReponer },
                { "Clientes", lnkClientes },
                { "FormularioCliente", lnkClientes },
                { "Proveedores", lnkProveedores },
                { "FormularioProveedor", lnkProveedores },
                { "Usuarios", lnkUsuarios },
                { "FormularioUsuario", lnkUsuarios }
            };

            HtmlAnchor opcion;
            if (opciones.TryGetValue(pagina, out opcion))
            {
                opcion.Attributes["class"] = "menu-opcion activa";
                opcion.Attributes["aria-current"] = "page";
            }
        }
    }
}
