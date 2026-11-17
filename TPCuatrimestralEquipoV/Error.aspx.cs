using System;
using System.Web.UI;

namespace TPCuatrimestralEquipoV
{
    public partial class Error : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["error"] != null)
                lblMensaje.Text = Session["error"].ToString();

            lnkVolver.Visible = Session["usuario"] != null;
        }
    }
}
