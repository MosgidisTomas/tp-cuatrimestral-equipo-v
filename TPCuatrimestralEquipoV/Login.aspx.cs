using System;
using System.Web.UI;

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

            // Etapa 2: solo navegación, todavía no se valida el usuario.
            // Etapa 3: buscar el usuario por NombreUsuario, comparar UsuarioNegocio.calcularHash(txtPassword.Text)
            // con PasswordHash, verificar Activo y guardarlo en Session; si no coincide, mostrar lblError.
            Response.Redirect("~/", false);
        }
    }
}
