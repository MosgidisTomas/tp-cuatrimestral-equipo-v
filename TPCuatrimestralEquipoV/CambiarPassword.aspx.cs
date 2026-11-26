using System;
using System.Web.UI;
using Dominio;
using Negocio;

namespace TPCuatrimestralEquipoV
{
    public partial class CambiarPassword : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensaje.Visible = false;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";

                // El CompareValidator no valida si "Repetir" está vacío.
                if (txtNueva.Text != txtNueva2.Text)
                    cmpNueva.IsValid = false;
                if (!Page.IsValid)
                    return;

                Usuario usuario = (Usuario)Session["usuario"];
                UsuarioNegocio negocio = new UsuarioNegocio();

                if (!negocio.verificarPassword(usuario.Id, txtActual.Text))
                {
                    cvActual.IsValid = false;
                    return;
                }

                negocio.cambiarPassword(usuario.Id, txtNueva.Text);
                lblMensaje.Text = "Listo, tu contraseña se cambió. La próxima vez ingresá con la nueva.";
                lblMensaje.Visible = true;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
    }
}
