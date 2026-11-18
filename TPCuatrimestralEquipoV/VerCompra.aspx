<%@ Page Title="Detalle de compra" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VerCompra.aspx.cs" Inherits="TPCuatrimestralEquipoV.VerCompra" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="encabezado-pagina">
        <div>
            <h1>Detalle de compra</h1>
            <p>Productos comprados, con la cantidad y el costo de esta compra.</p>
        </div>
        <a runat="server" href="~/Compras" class="btn btn-outline-secondary">Volver a compras</a>
    </div>

    <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger d-block" Visible="false" />

    <div class="row g-4">
        <div class="col-lg-8">
            <div class="tabla-contenedor">
                <asp:GridView ID="gvDetalle" runat="server" AutoGenerateColumns="false"
                    CssClass="table tabla" GridLines="None"
                    ShowHeaderWhenEmpty="true" EmptyDataText="La compra no tiene productos.">
                    <Columns>
                        <asp:TemplateField HeaderText="Código">
                            <ItemTemplate><%#: Eval("Producto.Codigo") %></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Producto">
                            <ItemTemplate><%#: Eval("Producto.Nombre") %> <span class="text-muted">· <%#: Eval("Producto.Marca.Descripcion") %></span></ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" ItemStyle-CssClass="num" HeaderStyle-CssClass="num" />
                        <asp:BoundField DataField="PrecioUnitario" HeaderText="Costo unitario" DataFormatString="{0:C}" ItemStyle-CssClass="num" HeaderStyle-CssClass="num" />
                        <asp:BoundField DataField="Subtotal" HeaderText="Subtotal" DataFormatString="{0:C}" ItemStyle-CssClass="num" HeaderStyle-CssClass="num" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <div class="col-lg-4">
            <div class="resumen">
                <span class="resumen-etiqueta">DATOS DE LA COMPRA</span>
                <div class="resumen-linea"><span>Proveedor</span><asp:Label ID="lblProveedor" runat="server" CssClass="valor" /></div>
                <div class="resumen-linea"><span>CUIT</span><asp:Label ID="lblCuit" runat="server" CssClass="valor num" /></div>
                <div class="resumen-linea"><span>Factura</span><asp:Label ID="lblFactura" runat="server" CssClass="valor" /></div>
                <div class="resumen-linea"><span>Fecha</span><asp:Label ID="lblFecha" runat="server" CssClass="valor" /></div>
                <div class="resumen-linea"><span>Cargada por</span><asp:Label ID="lblUsuario" runat="server" CssClass="valor" /></div>
                <div class="resumen-total">
                    <span class="resumen-etiqueta">TOTAL</span>
                    <asp:Label ID="lblTotal" runat="server" CssClass="total-monto num" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
