using System;
using System.Collections.Generic;
using System.Web.UI;
using Dominio;

namespace TPCuatrimestralEquipoV
{
    public partial class Proveedores : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Etapa 1: pantalla sin funcionalidad. Se enlaza una lista vacía para que la tabla
                // muestre sus columnas. En la Etapa 2 se carga con ProveedorNegocio.listar().
                gvProveedores.DataSource = new List<Proveedor>();
                gvProveedores.DataBind();
            }
        }
    }
}
