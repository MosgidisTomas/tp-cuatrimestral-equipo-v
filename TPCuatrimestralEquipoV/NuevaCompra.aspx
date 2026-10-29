<%@ Page Title="Nueva compra" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NuevaCompra.aspx.cs" Inherits="TPCuatrimestralEquipoV.NuevaCompra" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="encabezado-pagina">
        <div>
            <h1>Nueva compra</h1>
            <p>Ingreso de mercadería comprada a un proveedor.</p>
        </div>
    </div>

    <section class="tarjeta">
        <h2 class="titulo-seccion">Datos de la compra</h2>
        <div class="row g-3">
            <div class="col-md-4">
                <asp:Label runat="server" AssociatedControlID="ddlProveedor" CssClass="form-label" Text="Proveedor *" />
                <asp:DropDownList ID="ddlProveedor" runat="server" CssClass="form-select">
                    <asp:ListItem Value="" Text="Elegí un proveedor" />
                </asp:DropDownList>
            </div>
            <div class="col-md-4">
                <asp:Label runat="server" AssociatedControlID="txtNumeroFactura" CssClass="form-label" Text="Nº de factura del proveedor *" />
                <asp:TextBox ID="txtNumeroFactura" runat="server" CssClass="form-control" placeholder="Ej.: A-0003-00012845" />
            </div>
            <div class="col-md-4">
                <asp:Label runat="server" AssociatedControlID="txtFecha" CssClass="form-label" Text="Fecha de la factura *" />
                <asp:TextBox ID="txtFecha" runat="server" TextMode="Date" CssClass="form-control" />
            </div>
        </div>
    </section>

    <section class="tarjeta">
        <h2 class="titulo-seccion">Productos</h2>
        <div class="row g-3 align-items-end">
            <div class="col-md-5">
                <asp:Label runat="server" AssociatedControlID="ddlProducto" CssClass="form-label" Text="Producto" />
                <asp:DropDownList ID="ddlProducto" runat="server" CssClass="form-select">
                    <asp:ListItem Value="" Text="Elegí un producto" />
                </asp:DropDownList>
            </div>
            <div class="col-sm-4 col-md-2">
                <asp:Label runat="server" AssociatedControlID="txtCantidad" CssClass="form-label" Text="Cantidad" />
                <asp:TextBox ID="txtCantidad" runat="server" TextMode="Number" CssClass="form-control" min="1" />
            </div>
            <div class="col-sm-4 col-md-3">
                <asp:Label runat="server" AssociatedControlID="txtCosto" CssClass="form-label" Text="Costo unitario ($)" />
                <asp:TextBox ID="txtCosto" runat="server" TextMode="Number" CssClass="form-control" min="0" step="0.01" />
            </div>
            <div class="col-sm-4 col-md-2">
                <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="btn btn-outline-secondary w-100" />
            </div>
        </div>
        <p class="texto-ayuda mt-2 mb-3">Solo aparecen los productos asociados al proveedor elegido.</p>

        <div class="tabla-contenedor">
            <asp:GridView ID="gvDetalle" runat="server" AutoGenerateColumns="false"
                CssClass="table tabla" GridLines="None"
                ShowHeaderWhenEmpty="true" EmptyDataText="Todavía no agregaste productos a la compra.">
                <Columns>
                    <asp:BoundField DataField="Producto" HeaderText="Producto" />
                    <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" ItemStyle-CssClass="num" HeaderStyle-CssClass="num" />
                    <asp:BoundField DataField="PrecioUnitario" HeaderText="Costo unitario" DataFormatString="{0:C}" ItemStyle-CssClass="num" HeaderStyle-CssClass="num" />
                    <asp:BoundField DataField="Subtotal" HeaderText="Subtotal" DataFormatString="{0:C}" ItemStyle-CssClass="num" HeaderStyle-CssClass="num" />
                </Columns>
            </asp:GridView>
        </div>
        <div class="fila-total">
            <span>Total</span>
            <asp:Label ID="lblTotal" runat="server" CssClass="num" Text="$ 0,00" />
        </div>

        <div class="nota-info mt-3">
            Al confirmar se suma el stock de cada producto y su costo pasa a ser la base del precio de venta.
        </div>
    </section>

    <div class="acciones">
        <asp:HyperLink ID="lnkCancelar" runat="server" NavigateUrl="~/" Text="Cancelar" CssClass="btn btn-outline-secondary" />
        <asp:Button ID="btnConfirmar" runat="server" Text="Confirmar compra" CssClass="btn btn-primary" />
    </div>

</asp:Content>
