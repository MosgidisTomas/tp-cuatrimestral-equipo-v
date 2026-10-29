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
            // La clase de Bootstrap va en el <input> del CheckBox, no en el <span> que lo envuelve.
            chkActivo.InputAttributes["class"] = "form-check-input";

            if (!IsPostBack)
            {
                // Etapa 1: pantalla sin funcionalidad. Se enlaza una lista vacía para que la tabla
                // muestre sus columnas. En la Etapa 2 se carga con UsuarioNegocio.listar().
                gvUsuarios.DataSource = new List<Usuario>();
                gvUsuarios.DataBind();
            }
        }
    }
}
