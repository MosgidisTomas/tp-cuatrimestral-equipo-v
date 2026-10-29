<%@ Page Title="Nueva venta" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NuevaVenta.aspx.cs" Inherits="TPCuatrimestralEquipoV.NuevaVenta" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="encabezado-pagina">
        <div>
            <h1>Nueva venta</h1>
            <p>Venta a un cliente registrado. Al confirmar se genera la factura.</p>
        </div>
    </div>

    <div class="row g-4">
        <div class="col-lg-8">

            <section class="tarjeta">
                <h2 class="titulo-seccion">Cliente</h2>
                <div class="row g-3 align-items-end">
                    <div class="col-sm-8 col-md-6">
                        <asp:Label runat="server" AssociatedControlID="txtDocumento" CssClass="form-label" Text="Documento del cliente *" />
                        <asp:TextBox ID="txtDocumento" runat="server" CssClass="form-control" inputmode="numeric" placeholder="Solo números" />
                    </div>
                    <div class="col-sm-4 col-md-3">
                        <asp:Button ID="btnBuscarCliente" runat="server" Text="Buscar" CssClass="btn btn-outline-secondary w-100" />
                    </div>
                </div>
                <p class="texto-ayuda mt-2 mb-0">
                    ¿No está registrado?
                    <asp:HyperLink ID="lnkNuevoCliente" runat="server" NavigateUrl="~/Clientes" Text="Registrar cliente nuevo" />
                </p>
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
                    <div class="col-sm-4 col-md-3">
                        <asp:Label runat="server" AssociatedControlID="txtPrecio" CssClass="form-label" Text="Precio unitario" />
                        <asp:TextBox ID="txtPrecio" runat="server" CssClass="form-control" ReadOnly="true" />
                    </div>
                    <div class="col-sm-4 col-md-2">
                        <asp:Label runat="server" AssociatedControlID="txtDisponible" CssClass="form-label" Text="Disponible" />
                        <asp:TextBox ID="txtDisponible" runat="server" CssClass="form-control" ReadOnly="true" />
                    </div>
                    <div class="col-sm-4 col-md-2">
                        <asp:Label runat="server" AssociatedControlID="txtCantidad" CssClass="form-label" Text="Cantidad" />
                        <asp:TextBox ID="txtCantidad" runat="server" TextMode="Number" CssClass="form-control" min="1" />
                    </div>
                    <div class="col-12 col-md-3 ms-md-auto">
                        <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="btn btn-outline-secondary w-100" />
                    </div>
                </div>

                <div class="tabla-contenedor mt-3">
                    <asp:GridView ID="gvDetalle" runat="server" AutoGenerateColumns="false"
                        CssClass="table tabla" GridLines="None"
                        ShowHeaderWhenEmpty="true" EmptyDataText="Todavía no agregaste productos a la venta.">
                        <Columns>
                            <asp:BoundField DataField="Producto" HeaderText="Producto" />
                            <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" ItemStyle-CssClass="num" HeaderStyle-CssClass="num" />
                            <asp:BoundField DataField="PrecioUnitario" HeaderText="Precio unitario" DataFormatString="{0:C}" ItemStyle-CssClass="num" HeaderStyle-CssClass="num" />
                            <asp:BoundField DataField="Subtotal" HeaderText="Subtotal" DataFormatString="{0:C}" ItemStyle-CssClass="num" HeaderStyle-CssClass="num" />
                        </Columns>
                    </asp:GridView>
                </div>
            </section>

        </div>

        <div class="col-lg-4">
            <aside class="resumen" aria-label="Resumen de la venta">
                <span class="resumen-etiqueta">RESUMEN</span>
                <div class="resumen-linea"><span>Factura</span><strong>Se asigna al confirmar</strong></div>
                <div class="resumen-linea"><span>Cliente</span><asp:Label ID="lblCliente" runat="server" CssClass="valor" Text="—" /></div>
                <div class="resumen-linea"><span>Unidades</span><asp:Label ID="lblUnidades" runat="server" CssClass="valor" Text="0" /></div>
                <div class="resumen-total">
                    <span class="resumen-etiqueta">TOTAL</span>
                    <asp:Label ID="lblTotal" runat="server" CssClass="total-monto" Text="$ 0,00" />
                </div>
                <asp:Button ID="btnConfirmar" runat="server" Text="Confirmar venta" CssClass="btn btn-cian w-100" />
                <asp:HyperLink ID="lnkCancelar" runat="server" NavigateUrl="~/" Text="Cancelar" CssClass="text-center d-block" />
            </aside>
        </div>
    </div>

</asp:Content>
