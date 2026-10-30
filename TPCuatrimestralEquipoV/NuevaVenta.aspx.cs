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
                gvDetalle.DataSource = new List<DetalleVenta>();
                gvDetalle.DataBind();
            }
        }
    }
}
