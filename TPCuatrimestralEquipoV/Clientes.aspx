<%@ Page Title="Clientes" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Clientes.aspx.cs" Inherits="TPCuatrimestralEquipoV.Clientes" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="encabezado-pagina">
        <div>
            <h1>Clientes</h1>
            <p>Solo se le puede vender a un cliente registrado. Tocá un cliente para editarlo.</p>
        </div>
        <asp:Button ID="btnNuevo" runat="server" Text="Nuevo cliente" CssClass="btn btn-primary" OnClick="btnNuevo_Click" />
    </div>

    <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger d-block" Visible="false" />

    <asp:Panel ID="pnlBuscar" runat="server" CssClass="tarjeta mb-4" DefaultButton="btnBuscar">
        <div class="row g-3 align-items-end">
            <div class="col-md-8">
                <asp:Label runat="server" AssociatedControlID="txtBuscar" CssClass="form-label" Text="Buscar" />
                <asp:TextBox ID="txtBuscar" runat="server" TextMode="Search" CssClass="form-control" placeholder="DNI, nombre o apellido" />
            </div>
            <div class="col-md-2">
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-outline-secondary w-100" OnClick="btnBuscar_Click" />
            </div>
        </div>
    </asp:Panel>

    <div class="tabla-contenedor">
        <asp:GridView ID="gvClientes" runat="server" AutoGenerateColumns="false"
            CssClass="table table-hover tabla tabla-clic" GridLines="None"
            ShowHeaderWhenEmpty="true" EmptyDataText="No hay clientes para mostrar."
            DataKeyNames="Id"
            OnSelectedIndexChanged="gvClientes_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="Documento" HeaderText="DNI" />
                <asp:BoundField DataField="NombreCompleto" HeaderText="Apellido y nombre" />
                <asp:BoundField DataField="Email" HeaderText="Email" />
                <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                <asp:BoundField DataField="FechaAlta" HeaderText="Fecha de alta" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:TemplateField HeaderText="Estado">
                    <ItemTemplate>
                        <span class='<%# (bool)Eval("Activo") ? "badge text-bg-success" : "badge text-bg-secondary" %>'><%# (bool)Eval("Activo") ? "Activo" : "Inactivo" %></span>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
