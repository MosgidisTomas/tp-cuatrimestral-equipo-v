<%@ Page Title="Factura" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Factura.aspx.cs" Inherits="TPCuatrimestralEquipoV.Factura" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="factura-contenedor">
        <div class="acciones mt-0 mb-2 justify-content-between no-imprimir">
            <a href="Ventas.aspx" class="btn btn-outline-secondary">Volver a ventas</a>
            <button type="button" class="btn btn-primary" onclick="window.print();">
                <i class="bi bi-printer" aria-hidden="true"></i> Imprimir
            </button>
        </div>
        <p class="texto-ayuda no-imprimir">Al imprimir se ocultan el menú, la barra superior y estos botones: sale solo la hoja.</p>

        <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger d-block no-imprimir" Visible="false" />

        <article class="factura-hoja">
            <div class="factura-encabezado">
                <div>
                    <div class="factura-marca">UTN</div>
                    <div class="factura-marca factura-marca-chica">HARDWARE</div>
                    <div class="factura-dato-suave mt-2">Componentes de PC · Av. Hipólito Yrigoyen 288, Gral. Pacheco</div>
                </div>
                <div class="text-end">
                    <div class="factura-marca factura-titulo">FACTURA</div>
                    <div class="num fs-5 fw-semibold mt-2">Nº <asp:Label ID="lblNumero" runat="server" /></div>
                    <div>Fecha: <asp:Label ID="lblFecha" runat="server" /></div>
                </div>
            </div>

            <div class="factura-cliente">
                <div><span class="factura-etiqueta">CLIENTE</span><br /><strong><asp:Label ID="lblCliente" runat="server" /></strong></div>
                <div><span class="factura-etiqueta">DNI</span><br /><asp:Label ID="lblDocumento" runat="server" CssClass="num" /></div>
                <div><span class="factura-etiqueta">EMAIL</span><br /><asp:Label ID="lblEmail" runat="server" /></div>
            </div>

            <table class="factura-detalle">
                <thead>
                    <tr>
                        <th>Producto</th>
                        <th class="num">Cant.</th>
                        <th class="num">Precio unit.</th>
                        <th class="num">Subtotal</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptDetalle" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><%#: Eval("Producto.Marca.Descripcion") %> <%#: Eval("Producto.Nombre") %></td>
                                <td class="num"><%# Eval("Cantidad") %></td>
                                <td class="num"><%# Eval("PrecioUnitario", "{0:C}") %></td>
                                <td class="num"><%# Eval("Subtotal", "{0:C}") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>

            <div class="factura-total">
                <span class="factura-etiqueta">TOTAL</span>
                <asp:Label ID="lblTotal" runat="server" CssClass="factura-marca num" />
            </div>

            <p class="factura-dato-suave text-center mb-0">Gracias por su compra.</p>
        </article>
    </div>

</asp:Content>
