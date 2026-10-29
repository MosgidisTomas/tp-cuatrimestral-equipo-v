using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace TPCuatrimestralEquipoV
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            marcarOpcionActiva();
        }

        // Resalta en el menú la opción de la página que se está viendo.
        private void marcarOpcionActiva()
        {
            // Nombre del archivo de la página actual, sin extensión: "Default", "Marcas", etc.
            string pagina = Path.GetFileNameWithoutExtension(Request.AppRelativeCurrentExecutionFilePath);

            Dictionary<string, HtmlAnchor> opciones = new Dictionary<string, HtmlAnchor>(StringComparer.OrdinalIgnoreCase)
            {
                { "Default", lnkInicio },
                { "NuevaVenta", lnkNuevaVenta },
                { "NuevaCompra", lnkNuevaCompra },
                { "Ventas", lnkVentas },
                { "Productos", lnkProductos },
                { "Marcas", lnkMarcas },
                { "Categorias", lnkCategorias },
                { "Clientes", lnkClientes },
                { "Proveedores", lnkProveedores },
                { "Usuarios", lnkUsuarios }
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
