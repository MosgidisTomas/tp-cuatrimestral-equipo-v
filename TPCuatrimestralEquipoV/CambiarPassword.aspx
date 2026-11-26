<%@ Page Title="Cambiar contraseña" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CambiarPassword.aspx.cs" Inherits="TPCuatrimestralEquipoV.CambiarPassword" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="encabezado-pagina">
        <div>
            <h1>Cambiar contraseña</h1>
            <p>Cambiá la contraseña con la que ingresás al sistema.</p>
        </div>
    </div>

    <div class="row g-4">
        <div class="col-lg-6">
            <div class="tarjeta">
                <div class="row g-3">
                    <div class="col-12">
                        <asp:Label runat="server" AssociatedControlID="txtActual" CssClass="form-label" Text="Contraseña actual *" />
                        <asp:TextBox ID="txtActual" runat="server" TextMode="Password" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvActual" runat="server" ControlToValidate="txtActual" ErrorMessage="Ingresá tu contraseña actual." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                        <asp:CustomValidator ID="cvActual" runat="server" ControlToValidate="txtActual" ErrorMessage="La contraseña actual no es correcta." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                    </div>
                    <div class="col-md-6">
                        <asp:Label runat="server" AssociatedControlID="txtNueva" CssClass="form-label" Text="Nueva contraseña *" />
                        <asp:TextBox ID="txtNueva" runat="server" TextMode="Password" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvNueva" runat="server" ControlToValidate="txtNueva" ErrorMessage="Ingresá la nueva contraseña." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                        <asp:RegularExpressionValidator ID="revNueva" runat="server" ControlToValidate="txtNueva" ValidationExpression="^.{6,}$" ErrorMessage="La contraseña tiene que tener al menos 6 caracteres." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                    </div>
                    <div class="col-md-6">
                        <asp:Label runat="server" AssociatedControlID="txtNueva2" CssClass="form-label" Text="Repetir nueva contraseña *" />
                        <asp:TextBox ID="txtNueva2" runat="server" TextMode="Password" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvNueva2" runat="server" ControlToValidate="txtNueva2" ErrorMessage="Repetí la nueva contraseña." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                        <asp:CompareValidator ID="cmpNueva" runat="server" ControlToValidate="txtNueva2" ControlToCompare="txtNueva" Operator="Equal" ErrorMessage="Las contraseñas no coinciden." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                    </div>
                </div>

                <asp:Label ID="lblMensaje" runat="server" CssClass="alert alert-success d-block mt-3 mb-0" Visible="false" />
                <asp:Label ID="lblError" runat="server" CssClass="text-danger d-block mt-3" />
                <div class="acciones">
                    <a href="Default.aspx" class="btn btn-outline-secondary">Volver</a>
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardar_Click" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
