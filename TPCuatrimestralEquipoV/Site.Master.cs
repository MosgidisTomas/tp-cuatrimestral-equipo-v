using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Dominio;

namespace TPCuatrimestralEquipoV
{
    public partial class SiteMaster : MasterPage
    {
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
                Response.Redirect("~/Login");

            if (!EsAdmin && !EsPaginaDeVendedor())
            {
                Session.Add("error", "No tenés permisos para ingresar a esta pantalla. Necesitás perfil Administrador.");
                Response.Redirect("~/Error");
            }
        }

        // Pantallas a las que puede entrar el vendedor. Se pregunta por el tipo de la página
        // (como hace el profe con "Page is Login"), así no depende de cómo está escrita la URL.
        private bool EsPaginaDeVendedor()
        {
            return Page is _Default || Page is NuevaVenta || Page is Ventas || Page is Factura
                || Page is Clientes || Page is FormularioCliente;
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
            Response.Redirect("~/Login");
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
                { "VerCompra", lnkCompras },
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
