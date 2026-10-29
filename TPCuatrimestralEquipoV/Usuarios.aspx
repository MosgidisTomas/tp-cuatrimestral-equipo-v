<%@ Page Title="Usuarios" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" Inherits="TPCuatrimestralEquipoV.Usuarios" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="encabezado-pagina">
        <div>
            <h1>Usuarios</h1>
            <p>Quiénes pueden ingresar al sistema y con qué perfil.</p>
        </div>
    </div>

    <div class="row g-4">
        <div class="col-lg-8">
            <div class="tabla-contenedor">
                <asp:GridView ID="gvUsuarios" runat="server" AutoGenerateColumns="false"
                    CssClass="table table-hover tabla" GridLines="None"
                    ShowHeaderWhenEmpty="true" EmptyDataText="No hay usuarios para mostrar.">
                    <Columns>
                        <asp:BoundField DataField="NombreUsuario" HeaderText="Usuario" />
                        <asp:BoundField DataField="NombreCompleto" HeaderText="Nombre" />
                        <asp:BoundField DataField="Email" HeaderText="Email" />
                        <asp:BoundField DataField="Perfil" HeaderText="Perfil" />
                        <asp:CheckBoxField DataField="Activo" HeaderText="Activo" ReadOnly="true" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <div class="col-lg-4">
            <div class="tarjeta">
                <h2 class="titulo-seccion">Nuevo usuario</h2>
                <div class="mb-3">
                    <asp:Label runat="server" AssociatedControlID="txtNombreUsuario" CssClass="form-label" Text="Nombre de usuario *" />
                    <asp:TextBox ID="txtNombreUsuario" runat="server" CssClass="form-control" MaxLength="30" placeholder="nombre.apellido" />
                </div>
                <div class="row g-3 mb-3">
                    <div class="col-sm-6">
                        <asp:Label runat="server" AssociatedControlID="txtNombre" CssClass="form-label" Text="Nombre *" />
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" MaxLength="50" />
                    </div>
                    <div class="col-sm-6">
                        <asp:Label runat="server" AssociatedControlID="txtApellido" CssClass="form-label" Text="Apellido *" />
                        <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" MaxLength="50" />
                    </div>
                </div>
                <div class="mb-3">
                    <asp:Label runat="server" AssociatedControlID="txtEmail" CssClass="form-label" Text="Email *" />
                    <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="form-control" MaxLength="100" />
                </div>
                <div class="mb-3">
                    <asp:Label runat="server" AssociatedControlID="ddlPerfil" CssClass="form-label" Text="Perfil *" />
                    <asp:DropDownList ID="ddlPerfil" runat="server" CssClass="form-select">
                        <asp:ListItem Value="2" Text="Vendedor" />
                        <asp:ListItem Value="1" Text="Administrador" />
                    </asp:DropDownList>
                </div>
                <div class="mb-3">
                    <asp:Label runat="server" AssociatedControlID="txtPassword" CssClass="form-label" Text="Contraseña *" />
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" />
                </div>
                <div class="mb-3">
                    <asp:Label runat="server" AssociatedControlID="txtPassword2" CssClass="form-label" Text="Repetir contraseña *" />
                    <asp:TextBox ID="txtPassword2" runat="server" TextMode="Password" CssClass="form-control" />
                </div>
                <div class="form-check mb-2">
                    <asp:CheckBox ID="chkActivo" runat="server" Checked="true" />
                    <asp:Label runat="server" AssociatedControlID="chkActivo" CssClass="form-check-label" Text="Usuario activo" />
                </div>
                <p class="texto-ayuda">Un usuario inactivo no puede ingresar, pero sus datos se conservan.</p>
                <div class="acciones mt-0">
                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-outline-secondary" />
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
