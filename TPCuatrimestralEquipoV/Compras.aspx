<%@ Page Title="Compras" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Compras.aspx.cs" Inherits="TPCuatrimestralEquipoV.Compras" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <%-- Etapa 2: maqueta (listado vacío, como Ventas).
         Etapa 3: listar las compras de la base con los filtros, y al tocar una fila ver su detalle (productos, cantidades y costos). --%>

    <div class="encabezado-pagina">
        <div>
            <h1>Compras</h1>
            <p>Compras registradas a proveedores, con el costo de cada producto.</p>
        </div>
        <asp:HyperLink ID="lnkNuevaCompra" runat="server" NavigateUrl="~/NuevaCompra" Text="Nueva compra" CssClass="btn btn-primary" />
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
                <asp:Label runat="server" AssociatedControlID="txtProveedor" CssClass="form-label" Text="Proveedor" />
                <asp:TextBox ID="txtProveedor" runat="server" TextMode="Search" CssClass="form-control" placeholder="Razón social o CUIT" />
            </div>
            <div class="col-md-2">
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-outline-secondary w-100" />
            </div>
        </div>
    </div>

    <div class="tabla-contenedor">
        <asp:GridView ID="gvCompras" runat="server" AutoGenerateColumns="false"
            CssClass="table table-hover tabla" GridLines="None"
            ShowHeaderWhenEmpty="true" EmptyDataText="No hay compras en el período elegido.">
            <Columns>
                <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:TemplateField HeaderText="Proveedor">
                    <ItemTemplate><%#: Eval("Proveedor.RazonSocial") %></ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="NumeroFactura" HeaderText="Factura del proveedor" />
                <asp:TemplateField HeaderText="Cargada por">
                    <ItemTemplate><%#: Eval("Usuario.NombreUsuario") %></ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="{0:C}" ItemStyle-CssClass="num" HeaderStyle-CssClass="num" />
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
