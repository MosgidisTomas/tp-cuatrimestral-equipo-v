<%@ Page Title="Ventas y facturas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Ventas.aspx.cs" Inherits="TPCuatrimestralEquipoV.Ventas" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="encabezado-pagina">
        <div>
            <h1>Ventas y facturas</h1>
            <p>Ventas registradas, con su factura para ver o reimprimir.</p>
        </div>
        <asp:HyperLink ID="lnkNuevaVenta" runat="server" NavigateUrl="~/NuevaVenta" Text="Nueva venta" CssClass="btn btn-primary" />
    </div>

    <div class="tarjeta mb-4">
        <div class="row g-3 align-items-end">
            <div class="col-sm-6 col-md-3">
                <asp:Label runat="server" AssociatedControlID="txtDesde" CssClass="form-label" Text="Desde" />
                <asp:TextBox ID="txtDesde" runat="server" TextMode="Date" CssClass="form-control" />
            </div>
            <div class="col-sm-6 col-md-3">
                <asp:Label runat="server" AssociatedControlID="txtHasta" CssClass="form-label" Text="Hasta" />
                <asp:TextBox ID="txtHasta" runat="server" TextMode="Date" CssClass="form-control" />
            </div>
            <div class="col-md-4">
                <asp:Label runat="server" AssociatedControlID="txtCliente" CssClass="form-label" Text="Cliente" />
                <asp:TextBox ID="txtCliente" runat="server" TextMode="Search" CssClass="form-control" placeholder="Documento o apellido" />
            </div>
            <div class="col-md-2">
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-outline-secondary w-100" />
            </div>
        </div>
    </div>

    <div class="tabla-contenedor">
        <asp:GridView ID="gvVentas" runat="server" AutoGenerateColumns="false"
            CssClass="table table-hover tabla" GridLines="None"
            ShowHeaderWhenEmpty="true" EmptyDataText="No hay ventas en el período elegido.">
            <Columns>
                <asp:BoundField DataField="NumeroFacturaFormateado" HeaderText="Factura" />
                <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="Cliente" HeaderText="Cliente" />
                <asp:BoundField DataField="Usuario" HeaderText="Vendedor" />
                <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="{0:C}" ItemStyle-CssClass="num" HeaderStyle-CssClass="num" />
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
