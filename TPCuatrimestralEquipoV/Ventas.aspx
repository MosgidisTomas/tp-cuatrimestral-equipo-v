<%@ Page Title="Ventas y facturas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Ventas.aspx.cs" Inherits="TPCuatrimestralEquipoV.Ventas" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="encabezado-pagina">
        <div>
            <h1>Ventas y facturas</h1>
            <p>Ventas registradas, con su factura para ver o reimprimir.</p>
        </div>
        <asp:HyperLink ID="lnkNuevaVenta" runat="server" NavigateUrl="~/NuevaVenta" Text="Nueva venta" CssClass="btn btn-primary" />
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
                <asp:Label runat="server" AssociatedControlID="txtCliente" CssClass="form-label" Text="Cliente" />
                <asp:TextBox ID="txtCliente" runat="server" TextMode="Search" CssClass="form-control" placeholder="Documento o apellido" />
            </div>
            <div class="col-md-2">
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-outline-secondary w-100" OnClick="btnBuscar_Click" />
            </div>
        </div>
    </asp:Panel>

    <div class="tabla-contenedor">
        <asp:GridView ID="gvVentas" runat="server" AutoGenerateColumns="false"
            CssClass="table table-hover tabla tabla-clic" GridLines="None"
            ShowHeaderWhenEmpty="true" EmptyDataText="No hay ventas en el período elegido."
            DataKeyNames="Id"
            OnSelectedIndexChanged="gvVentas_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="NumeroFacturaFormateado" HeaderText="Factura" />
                <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:TemplateField HeaderText="Cliente">
                    <ItemTemplate><%#: Eval("Cliente.Apellido") %>, <%#: Eval("Cliente.Nombre") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Vendedor">
                    <ItemTemplate><%#: Eval("Usuario.NombreUsuario") %></ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="{0:C}" ItemStyle-CssClass="num" HeaderStyle-CssClass="num" />
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
