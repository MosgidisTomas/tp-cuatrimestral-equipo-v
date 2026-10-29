using System;
using System.Collections.Generic;
using System.Web.UI;
using Dominio;

namespace TPCuatrimestralEquipoV
{
    public partial class Ventas : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Por defecto se muestra el mes actual: del día 1 a hoy.
                DateTime hoy = DateTime.Today;
                txtDesde.Text = new DateTime(hoy.Year, hoy.Month, 1).ToString("yyyy-MM-dd");
                txtHasta.Text = hoy.ToString("yyyy-MM-dd");

                // Etapa 1: pantalla sin funcionalidad. Se enlaza una lista vacía para que la tabla
                // muestre sus columnas. En la Etapa 3 se carga con VentaNegocio.listar(desde, hasta).
                gvVentas.DataSource = new List<Venta>();
                gvVentas.DataBind();
            }
        }
    }
}
