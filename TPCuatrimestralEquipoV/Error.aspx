<%@ Page Title="Error" Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="TPCuatrimestralEquipoV.Error" %>

<!DOCTYPE html>

<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Hubo un problema - UTN Hardware</title>

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
                <div class="login-tarjeta">
                    <h1>Hubo un problema</h1>
                    <asp:Label ID="lblMensaje" runat="server" CssClass="alert alert-danger d-block my-3" Text="Ocurrió un error inesperado." />
                    <a id="lnkVolver" runat="server" href="~/" class="btn btn-primary w-100">Volver al inicio</a>
                    <a runat="server" href="~/Login" class="btn btn-outline-secondary w-100 mt-2">Ingresar</a>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
