using System;
using System.Collections.Generic;
using System.Web.UI;
using Dominio;

namespace TPCuatrimestralEquipoV
{
    public partial class NuevaCompra : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Etapa 1: pantalla sin funcionalidad. La compra se programa en la Etapa 3.
                txtFecha.Text = DateTime.Today.ToString("yyyy-MM-dd");

                // Lista vacía para que la tabla del detalle muestre sus columnas.
                gvDetalle.DataSource = new List<DetalleCompra>();
                gvDetalle.DataBind();
            }
        }
    }
}
