<%@ Page Title="Usuarios" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" Inherits="TPCuatrimestralEquipoV.Usuarios" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="encabezado-pagina">
        <div>
            <h1>Usuarios</h1>
            <p>Quiénes pueden ingresar al sistema y con qué perfil. Tocá un usuario para editarlo.</p>
        </div>
        <asp:Button ID="btnNuevo" runat="server" Text="Nuevo usuario" CssClass="btn btn-primary" OnClick="btnNuevo_Click" />
    </div>

    <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger d-block" Visible="false" />

    <asp:Panel ID="pnlBuscar" runat="server" CssClass="tarjeta mb-4" DefaultButton="btnBuscar">
        <div class="row g-3 align-items-end">
            <div class="col-md-8">
                <asp:Label runat="server" AssociatedControlID="txtBuscar" CssClass="form-label" Text="Buscar" />
                <asp:TextBox ID="txtBuscar" runat="server" TextMode="Search" CssClass="form-control" placeholder="Usuario, nombre, apellido o email" />
            </div>
            <div class="col-md-2">
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-outline-secondary w-100" OnClick="btnBuscar_Click" />
            </div>
        </div>
    </asp:Panel>

    <div class="tabla-contenedor">
        <asp:GridView ID="gvUsuarios" runat="server" AutoGenerateColumns="false"
            CssClass="table table-hover tabla tabla-clic" GridLines="None"
            ShowHeaderWhenEmpty="true" EmptyDataText="No hay usuarios para mostrar."
            DataKeyNames="Id"
            OnSelectedIndexChanged="gvUsuarios_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="NombreUsuario" HeaderText="Usuario" />
                <asp:BoundField DataField="NombreCompleto" HeaderText="Nombre" />
                <asp:BoundField DataField="Email" HeaderText="Email" />
                <asp:BoundField DataField="Perfil" HeaderText="Perfil" />
                <asp:TemplateField HeaderText="Estado">
                    <ItemTemplate>
                        <span class='<%# (bool)Eval("Activo") ? "badge text-bg-success" : "badge text-bg-secondary" %>'><%# (bool)Eval("Activo") ? "Activo" : "Inactivo" %></span>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
