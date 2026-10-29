<%@ Page Title="Clientes" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Clientes.aspx.cs" Inherits="TPCuatrimestralEquipoV.Clientes" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="encabezado-pagina">
        <div>
            <h1>Clientes</h1>
            <p>Solo se le puede vender a un cliente registrado.</p>
        </div>
        <asp:Button ID="btnNuevo" runat="server" Text="Nuevo cliente" CssClass="btn btn-primary" />
    </div>

    <div class="tarjeta mb-4">
        <div class="row g-3 align-items-end">
            <div class="col-md-8">
                <asp:Label runat="server" AssociatedControlID="txtBuscar" CssClass="form-label" Text="Buscar" />
                <asp:TextBox ID="txtBuscar" runat="server" TextMode="Search" CssClass="form-control" placeholder="Documento, nombre o apellido" />
            </div>
            <div class="col-md-2">
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-outline-secondary w-100" />
            </div>
        </div>
    </div>

    <div class="tabla-contenedor">
        <asp:GridView ID="gvClientes" runat="server" AutoGenerateColumns="false"
            CssClass="table table-hover tabla" GridLines="None"
            ShowHeaderWhenEmpty="true" EmptyDataText="No hay clientes para mostrar.">
            <Columns>
                <asp:BoundField DataField="Documento" HeaderText="Documento" />
                <asp:BoundField DataField="NombreCompleto" HeaderText="Apellido y nombre" />
                <asp:BoundField DataField="Email" HeaderText="Email" />
                <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                <asp:BoundField DataField="FechaAlta" HeaderText="Fecha de alta" DataFormatString="{0:dd/MM/yyyy}" />
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
