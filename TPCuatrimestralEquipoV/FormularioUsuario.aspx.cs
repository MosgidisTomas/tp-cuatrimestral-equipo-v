using System;
using System.Web.UI;
using Dominio;
using Negocio;

namespace TPCuatrimestralEquipoV
{
    public partial class FormularioUsuario : Page
    {
        public bool EsModificacion { get { return Request.QueryString["id"] != null; } }

        public bool EsUsuarioActual
        {
            get
            {
                Usuario logueado = (Usuario)Session["usuario"];
                return EsModificacion && logueado != null && Request.QueryString["id"] == logueado.Id.ToString();
            }
        }

        public bool UsuarioActivo
        {
            get { return ViewState["UsuarioActivo"] != null ? (bool)ViewState["UsuarioActivo"] : true; }
            set { ViewState["UsuarioActivo"] = value; }
        }

        public string NombreSeleccionado
        {
            get { return ViewState["NombreSeleccionado"] as string ?? ""; }
            set { ViewState["NombreSeleccionado"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            rfvPassword.Enabled = !EsModificacion;
            rfvPassword2.Enabled = !EsModificacion;
            ddlPerfil.Enabled = !EsUsuarioActual;

            try
            {
                if (!IsPostBack && EsModificacion)
                {
                    int id;
                    Usuario seleccionado = null;
                    if (int.TryParse(Request.QueryString["id"], out id))
                        seleccionado = new UsuarioNegocio().buscarPorId(id);

                    if (seleccionado == null)
                    {
                        Response.Redirect("Usuarios.aspx", false);
                        return;
                    }

                    lblTitulo.Text = "Editar usuario";
                    lblPassword.Text = "Nueva contraseña";
                    lblPassword2.Text = "Repetir nueva contraseña";
                    txtId.Text = seleccionado.Id.ToString();
                    txtNombreUsuario.Text = seleccionado.NombreUsuario;
                    txtNombre.Text = seleccionado.Nombre;
                    txtApellido.Text = seleccionado.Apellido;
                    txtEmail.Text = seleccionado.Email;
                    ddlPerfil.SelectedValue = ((int)seleccionado.Perfil).ToString();
                    UsuarioActivo = seleccionado.Activo;
                    NombreSeleccionado = seleccionado.NombreUsuario;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";

                // El CompareValidator no valida si "Repetir" está vacío.
                if (txtPassword.Text != txtPassword2.Text)
                    cmpPassword.IsValid = false;
                if (!Page.IsValid)
                    return;

                string nombreUsuario = txtNombreUsuario.Text.Trim();
                string email = txtEmail.Text.Trim();
                UsuarioNegocio negocio = new UsuarioNegocio();
                int idUsuario = EsModificacion ? int.Parse(Request.QueryString["id"]) : 0;

                if (negocio.existeNombreUsuario(nombreUsuario, idUsuario))
                    cvNombreUsuarioRepetido.IsValid = false;
                if (negocio.existeEmail(email, idUsuario))
                    cvEmailRepetido.IsValid = false;
                if (!Page.IsValid)
                    return;

                Usuario usuario = new Usuario();
                usuario.NombreUsuario = nombreUsuario;
                usuario.Nombre = txtNombre.Text.Trim();
                usuario.Apellido = txtApellido.Text.Trim();
                usuario.Email = email;
                usuario.Perfil = (TipoPerfil)int.Parse(ddlPerfil.SelectedValue);
                if (EsUsuarioActual)
                    usuario.Perfil = TipoPerfil.Administrador;

                if (EsModificacion)
                {
                    usuario.Id = idUsuario;
                    negocio.modificar(usuario, txtPassword.Text);
                }
                else
                {
                    negocio.agregar(usuario, txtPassword.Text);

                    try
                    {
                        EmailService emailService = new EmailService();
                        emailService.armarCorreo(usuario.Email, "Bienvenido a UTN Hardware",
                            "<h1>Hola " + Server.HtmlEncode(usuario.Nombre) + "</h1>" +
                            "<br>Ya podés ingresar al sistema con tu usuario <b>" + Server.HtmlEncode(usuario.NombreUsuario) + "</b>.");
                        emailService.enviarEmail();
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = "El usuario se creó, pero no se pudo enviar el mail de bienvenida (" + ex.Message + "). Tocá \"Cancelar\" para volver al listado.";
                        btnAceptar.Enabled = false;
                        return;
                    }
                }

                Response.Redirect("Usuarios.aspx", false);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected void btnConfirmaBaja_Click(object sender, EventArgs e)
        {
            if (EsUsuarioActual)
                return;

            try
            {
                UsuarioNegocio negocio = new UsuarioNegocio();
                negocio.eliminar(int.Parse(Request.QueryString["id"]));
                Response.Redirect("Usuarios.aspx", false);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected void btnReactivar_Click(object sender, EventArgs e)
        {
            try
            {
                UsuarioNegocio negocio = new UsuarioNegocio();
                negocio.reactivar(int.Parse(Request.QueryString["id"]));
                Response.Redirect("Usuarios.aspx", false);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
    }
}
