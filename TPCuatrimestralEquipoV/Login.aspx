<%@ Page Title="Ingresar" Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TPCuatrimestralEquipoV.Login" %>

<!DOCTYPE html>

<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Ingresar - UTN Hardware</title>

    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin="anonymous" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600&amp;family=Montserrat:wght@600;700&amp;display=swap" rel="stylesheet" />
    <webopt:bundlereference runat="server" path="~/Content/css" />
    <link href="~/Content/img/isotipo.png" rel="icon" type="image/png" />
</head>
<body>
    <%-- Página sin Site.Master: no lleva menú porque todavía no hay usuario. --%>
    <form runat="server">
        <asp:ScriptManager runat="server">
            <Scripts>
                <asp:ScriptReference Name="MsAjaxBundle" />
                <asp:ScriptReference Name="jquery" />
                <asp:ScriptReference Name="WebForms.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebForms.js" />
                <asp:ScriptReference Name="WebUIValidation.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebUIValidation.js" />
                <asp:ScriptReference Name="Focus.js" Assembly="System.Web" Path="~/Scripts/WebForms/Focus.js" />
                <asp:ScriptReference Name="WebFormsBundle" />
            </Scripts>
        </asp:ScriptManager>

        <div class="login">
            <div class="login-marca">
                <img runat="server" src="~/Content/img/logo-utn-hardware.jpg" alt="UTN Hardware" class="login-logo" />
                <p>Sistema de gestión de compras, ventas y stock</p>
            </div>

            <div class="login-lado-formulario">
                <asp:Panel ID="pnlLogin" runat="server" CssClass="login-tarjeta" DefaultButton="btnIngresar">
                    <h1>Ingresar</h1>
                    <p class="texto-ayuda mb-4">Usá tu usuario y contraseña del sistema.</p>

                    <div class="mb-3">
                        <asp:Label runat="server" AssociatedControlID="txtUsuario" CssClass="form-label" Text="Usuario" />
                        <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" MaxLength="30" placeholder="nombre.usuario" />
                        <asp:RequiredFieldValidator ID="rfvUsuario" runat="server" ControlToValidate="txtUsuario" ErrorMessage="Ingresá tu usuario." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                    </div>
                    <div class="mb-3">
                        <asp:Label runat="server" AssociatedControlID="txtPassword" CssClass="form-label" Text="Contraseña" />
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Ingresá tu contraseña." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                    </div>

                    <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger d-block small py-2" Visible="false" />

                    <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" CssClass="btn btn-primary w-100" OnClick="btnIngresar_Click" />
                    <div class="text-center mt-3">
                        <%-- Etapa 3: página para recuperar la contraseña por mail. --%>
                        <a href="#">¿Olvidaste tu contraseña?</a>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </form>
</body>
</html>
