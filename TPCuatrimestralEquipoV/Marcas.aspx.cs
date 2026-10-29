using System;
using System.Web.UI;
using Negocio;

namespace TPCuatrimestralEquipoV
{
    public partial class Marcas : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                cargarMarcas();
        }

        private void cargarMarcas()
        {
            try
            {
                MarcaNegocio negocio = new MarcaNegocio();
                gvMarcas.DataSource = negocio.listar();
                gvMarcas.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                lblError.Visible = true;
            }
        }
    }
}
