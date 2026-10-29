using System;
using System.Collections.Generic;
using System.Web.UI;
using Dominio;

namespace TPCuatrimestralEquipoV
{
    public partial class NuevaVenta : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Etapa 1: pantalla sin funcionalidad. La venta se programa en la Etapa 3.
                // Lista vacía para que la tabla del detalle muestre sus columnas.
                gvDetalle.DataSource = new List<DetalleVenta>();
                gvDetalle.DataBind();
            }
        }
    }
}
