using System;
using System.Text;
using System.Web.UI;
using Dominio;
using Negocio;

namespace TPCuatrimestralEquipoV
{
    public partial class RecuperarPassword : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
            lblMensaje.Visible = false;
        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            if (email == "" || !email.Contains("@"))
            {
                lblError.Text = "Ingresá un email válido.";
                lblError.Visible = true;
                return;
            }

            try
            {
                UsuarioNegocio negocio = new UsuarioNegocio();
                Usuario usuario = negocio.buscarPorEmail(email);

                if (usuario != null && usuario.Activo)
                {
                    string passwordNueva = generarPassword();

                    EmailService emailService = new EmailService();
                    emailService.armarCorreo(usuario.Email, "Recuperar contraseña - UTN Hardware",
                        "<h1>Hola " + Server.HtmlEncode(usuario.Nombre) + "</h1>" +
                        "<br>Tu usuario es <b>" + Server.HtmlEncode(usuario.NombreUsuario) + "</b> y tu contraseña nueva es <b>" + passwordNueva + "</b>.");
                    emailService.enviarEmail();

                    negocio.cambiarPassword(usuario.Id, passwordNueva);
                }

                txtEmail.Text = "";
                lblMensaje.Text = "Si el email está registrado, te enviamos una contraseña nueva. Revisá tu correo.";
                lblMensaje.Visible = true;
            }
            catch (Exception)
            {
                lblError.Text = "No se pudo enviar el mail. Probá de nuevo más tarde.";
                lblError.Visible = true;
            }
        }

        private string generarPassword()
        {
            const string caracteres = "abcdefghjkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            Random random = new Random();

            StringBuilder password = new StringBuilder();
            for (int i = 0; i < 8; i++)
                password.Append(caracteres[random.Next(caracteres.Length)]);
            return password.ToString();
        }
    }
}
