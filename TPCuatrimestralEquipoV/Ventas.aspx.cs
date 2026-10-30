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
                DateTime hoy = DateTime.Today;
                txtDesde.Text = new DateTime(hoy.Year, hoy.Month, 1).ToString("yyyy-MM-dd");
                txtHasta.Text = hoy.ToString("yyyy-MM-dd");

                gvVentas.DataSource = new List<Venta>();
                gvVentas.DataBind();
            }
        }
    }
}
