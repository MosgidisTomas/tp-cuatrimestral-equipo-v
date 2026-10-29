using System;
using System.Collections.Generic;
using System.Web.UI;
using Dominio;

namespace TPCuatrimestralEquipoV
{
    public partial class Productos : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Etapa 1: pantalla sin funcionalidad. Se enlaza una lista vacía para que la tabla
                // muestre sus columnas. En la Etapa 2 se carga con ProductoNegocio.listar().
                gvProductos.DataSource = new List<Producto>();
                gvProductos.DataBind();
            }
        }
    }
}
