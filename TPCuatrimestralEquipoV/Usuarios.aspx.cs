using System;
using System.Collections.Generic;
using System.Web.UI;
using Dominio;

namespace TPCuatrimestralEquipoV
{
    public partial class Usuarios : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            chkActivo.InputAttributes["class"] = "form-check-input";

            if (!IsPostBack)
            {
                gvUsuarios.DataSource = new List<Usuario>();
                gvUsuarios.DataBind();
            }
        }
    }
}
