<%@ Page Title="Recuperar contraseña" Language="C#" AutoEventWireup="true" CodeBehind="RecuperarPassword.aspx.cs" Inherits="TPCuatrimestralEquipoV.RecuperarPassword" %>

<!DOCTYPE html>

<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Recuperar contraseña - UTN Hardware</title>

    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin="anonymous" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600&amp;family=Montserrat:wght@600;700&amp;display=swap" rel="stylesheet" />
    <webopt:bundlereference runat="server" path="~/Content/css" />
    <link href="~/Content/img/isotipo.png" rel="icon" type="image/png" />
</head>
<body>
    <form runat="server">
        <div class="login">
            <div class="login-marca">
                <img runat="server" src="~/Content/img/logo-utn-hardware.jpg" alt="UTN Hardware" class="login-logo" />
                <p>Sistema de gestión de compras, ventas y stock</p>
            </div>

            <div class="login-lado-formulario">
                <asp:Panel ID="pnlRecuperar" runat="server" CssClass="login-tarjeta" DefaultButton="btnEnviar">
                    <h1>Recuperar contraseña</h1>
                    <p class="texto-ayuda mb-4">Escribí el email de tu usuario y te mandamos una contraseña nueva.</p>

                    <div class="mb-3">
                        <asp:Label runat="server" AssociatedControlID="txtEmail" CssClass="form-label" Text="Email" />
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" MaxLength="100" placeholder="nombre@ejemplo.com" />
                    </div>

                    <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger d-block small py-2" Visible="false" />
                    <asp:Label ID="lblMensaje" runat="server" CssClass="alert alert-success d-block small py-2" Visible="false" />

                    <asp:Button ID="btnEnviar" runat="server" Text="Enviar contraseña nueva" CssClass="btn btn-primary w-100" OnClick="btnEnviar_Click" />
                    <div class="text-center mt-3">
                        <a runat="server" href="~/Login">Volver a ingresar</a>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </form>
</body>
</html>
