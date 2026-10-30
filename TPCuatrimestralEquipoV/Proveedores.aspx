<%@ Page Title="Proveedores" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Proveedores.aspx.cs" Inherits="TPCuatrimestralEquipoV.Proveedores" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="encabezado-pagina">
        <div>
            <h1>Proveedores</h1>
            <p>Empresas a las que se les compra mercadería.</p>
        </div>
        <asp:Button ID="btnNuevo" runat="server" Text="Nuevo proveedor" CssClass="btn btn-primary" OnClick="btnNuevo_Click" />
    </div>

    <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger d-block" Visible="false" />

    <asp:Panel ID="pnlBuscar" runat="server" CssClass="tarjeta mb-4" DefaultButton="btnBuscar">
        <div class="row g-3 align-items-end">
            <div class="col-md-8">
                <asp:Label runat="server" AssociatedControlID="txtBuscar" CssClass="form-label" Text="Buscar" />
                <asp:TextBox ID="txtBuscar" runat="server" TextMode="Search" CssClass="form-control" placeholder="Razón social o CUIT" />
            </div>
            <div class="col-md-2">
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-outline-secondary w-100" OnClick="btnBuscar_Click" />
            </div>
        </div>
    </asp:Panel>

    <div class="tabla-contenedor">
        <asp:GridView ID="gvProveedores" runat="server" AutoGenerateColumns="false"
            CssClass="table table-hover tabla" GridLines="None"
            ShowHeaderWhenEmpty="true" EmptyDataText="No hay proveedores para mostrar."
            DataKeyNames="Id"
            OnSelectedIndexChanged="gvProveedores_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="RazonSocial" HeaderText="Razón social" />
                <asp:BoundField DataField="Cuit" HeaderText="CUIT" />
                <asp:BoundField DataField="Email" HeaderText="Email" />
                <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                <asp:CheckBoxField DataField="Activo" HeaderText="Activo" />
                <asp:CommandField ShowSelectButton="true" SelectText="✏️" HeaderText="Editar" />
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
