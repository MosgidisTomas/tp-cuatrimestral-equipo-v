<%@ Page Title="Compras" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Compras.aspx.cs" Inherits="TPCuatrimestralEquipoV.Compras" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="encabezado-pagina">
        <div>
            <h1>Compras</h1>
            <p>Compras registradas a proveedores. Tocá una compra para ver su detalle.</p>
        </div>
        <asp:HyperLink ID="lnkNuevaCompra" runat="server" NavigateUrl="~/NuevaCompra" Text="Nueva compra" CssClass="btn btn-primary" />
    </div>

    <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger d-block" Visible="false" />

    <asp:Panel ID="pnlBuscar" runat="server" CssClass="tarjeta mb-4" DefaultButton="btnBuscar">
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
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-outline-secondary w-100" OnClick="btnBuscar_Click" />
            </div>
        </div>
    </asp:Panel>

    <div class="tabla-contenedor">
        <asp:GridView ID="gvCompras" runat="server" AutoGenerateColumns="false"
            CssClass="table table-hover tabla tabla-clic" GridLines="None"
            ShowHeaderWhenEmpty="true" EmptyDataText="No hay compras para mostrar."
            DataKeyNames="Id"
            OnSelectedIndexChanged="gvCompras_SelectedIndexChanged">
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
