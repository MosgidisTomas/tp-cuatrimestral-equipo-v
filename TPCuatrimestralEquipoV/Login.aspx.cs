using System;
using System.Web.UI;
using Dominio;
using Negocio;

namespace TPCuatrimestralEquipoV
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                Usuario usuario = new Usuario();
                usuario.NombreUsuario = txtUsuario.Text.Trim();
                usuario.PasswordHash = UsuarioNegocio.calcularHash(txtPassword.Text);

                UsuarioNegocio negocio = new UsuarioNegocio();
                if (negocio.loguear(usuario))
                {
                    Session.Add("usuario", usuario);
                    Response.Redirect("~/", false);
                }
                else
                {
                    lblError.Text = "Usuario o contraseña incorrectos.";
                    lblError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.Message);
                Response.Redirect("~/Error", false);
            }
        }
    }
}
