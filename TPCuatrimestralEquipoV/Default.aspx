<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TPCuatrimestralEquipoV._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="encabezado-pagina">
        <div>
            <h1>Bienvenido a UTN Hardware</h1>
            <p>Sistema de gestión de compras, ventas y stock. ¿Qué querés hacer hoy?</p>
        </div>
    </div>

    <div class="accesos">
        <a class="acceso" runat="server" href="~/NuevaVenta">
            <span class="acceso-icono"><i class="bi bi-cart3" aria-hidden="true"></i></span>
            <span class="acceso-titulo">Nueva venta</span>
            <span class="acceso-detalle">Facturar a un cliente registrado</span>
        </a>

        <% if (((Dominio.Usuario)Session["usuario"]).EsAdministrador) { %>
        <a class="acceso" runat="server" href="~/NuevaCompra">
            <span class="acceso-icono"><i class="bi bi-box-seam" aria-hidden="true"></i></span>
            <span class="acceso-titulo">Nueva compra</span>
            <span class="acceso-detalle">Ingresar mercadería de un proveedor</span>
        </a>

        <a class="acceso" runat="server" href="~/Productos">
            <span class="acceso-icono"><i class="bi bi-cpu" aria-hidden="true"></i></span>
            <span class="acceso-titulo">Productos</span>
            <span class="acceso-detalle">Catálogo de componentes, precios y stock</span>
        </a>
        <% } %>

        <a class="acceso" runat="server" href="~/Clientes">
            <span class="acceso-icono"><i class="bi bi-person" aria-hidden="true"></i></span>
            <span class="acceso-titulo">Clientes</span>
            <span class="acceso-detalle">Quienes pueden comprar en la tienda</span>
        </a>
    </div>

    <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger d-block mt-4" Visible="false" />

    <h2 class="titulo-seccion mt-5">Resumen</h2>
    <div class="estadisticas">
        <div class="estadistica">
            <div class="estadistica-encabezado">
                <span class="estadistica-etiqueta">VENTAS DE HOY</span>
                <span class="estadistica-icono"><i class="bi bi-cash-coin" aria-hidden="true"></i></span>
            </div>
            <asp:Label ID="lblVentasHoy" runat="server" CssClass="estadistica-valor num" />
            <asp:Label ID="lblVentasHoyDetalle" runat="server" CssClass="estadistica-detalle" />
        </div>
        <% if (EsAdmin) { %>
        <div class="estadistica">
            <div class="estadistica-encabezado">
                <span class="estadistica-etiqueta">VENTAS DEL MES</span>
                <span class="estadistica-icono"><i class="bi bi-graph-up-arrow" aria-hidden="true"></i></span>
            </div>
            <asp:Label ID="lblVentasMes" runat="server" CssClass="estadistica-valor num" />
            <asp:Label ID="lblVentasMesDetalle" runat="server" CssClass="estadistica-detalle" />
        </div>
        <div class="estadistica">
            <div class="estadistica-encabezado">
                <span class="estadistica-etiqueta">COMPRAS DEL MES</span>
                <span class="estadistica-icono"><i class="bi bi-box-seam" aria-hidden="true"></i></span>
            </div>
            <asp:Label ID="lblComprasMes" runat="server" CssClass="estadistica-valor num" />
            <asp:Label ID="lblComprasMesDetalle" runat="server" CssClass="estadistica-detalle" />
        </div>
        <a class="estadistica" runat="server" href="~/ProductosAReponer">
            <div class="estadistica-encabezado">
                <span class="estadistica-etiqueta">PRODUCTOS A REPONER</span>
                <span class="estadistica-icono estadistica-icono-alerta"><i class="bi bi-exclamation-triangle" aria-hidden="true"></i></span>
            </div>
            <asp:Label ID="lblReponer" runat="server" CssClass="estadistica-valor num" />
            <span class="estadistica-detalle">En el stock mínimo o por debajo</span>
        </a>
        <% } %>
    </div>

    <% if (EsAdmin) { %>
    <div class="estadisticas mt-3">
        <div class="estadistica">
            <div class="estadistica-encabezado">
                <span class="estadistica-etiqueta">MÁS VENDIDO DEL MES</span>
                <span class="estadistica-icono"><i class="bi bi-trophy" aria-hidden="true"></i></span>
            </div>
            <asp:Label ID="lblMasVendido" runat="server" CssClass="estadistica-valor estadistica-texto" />
            <asp:Label ID="lblMasVendidoDetalle" runat="server" CssClass="estadistica-detalle" />
        </div>
        <div class="estadistica">
            <div class="estadistica-encabezado">
                <span class="estadistica-etiqueta">MENOS VENDIDO DEL MES</span>
                <span class="estadistica-icono"><i class="bi bi-arrow-down-circle" aria-hidden="true"></i></span>
            </div>
            <asp:Label ID="lblMenosVendido" runat="server" CssClass="estadistica-valor estadistica-texto" />
            <asp:Label ID="lblMenosVendidoDetalle" runat="server" CssClass="estadistica-detalle" />
        </div>
        <div class="estadistica">
            <div class="estadistica-encabezado">
                <span class="estadistica-etiqueta">MEJOR VENDEDOR DEL MES</span>
                <span class="estadistica-icono"><i class="bi bi-person-badge" aria-hidden="true"></i></span>
            </div>
            <asp:Label ID="lblMejorVendedor" runat="server" CssClass="estadistica-valor estadistica-texto" />
            <asp:Label ID="lblMejorVendedorDetalle" runat="server" CssClass="estadistica-detalle" />
        </div>
        <div class="estadistica">
            <div class="estadistica-encabezado">
                <span class="estadistica-etiqueta">CLIENTES NUEVOS DEL MES</span>
                <span class="estadistica-icono"><i class="bi bi-person-plus" aria-hidden="true"></i></span>
            </div>
            <asp:Label ID="lblClientesNuevos" runat="server" CssClass="estadistica-valor num" />
            <span class="estadistica-detalle">Registrados desde el día 1</span>
        </div>
    </div>

    <div class="row g-4 mt-1">
        <div class="col-lg-7">
            <section class="tarjeta h-100 d-flex flex-column">
                <h2 class="titulo-seccion">Ventas de los últimos 7 días</h2>
                <div class="barras">
                    <asp:Repeater ID="rptDias" runat="server">
                        <ItemTemplate>
                            <div class="barra-fila">
                                <span class="barra-dia"><%# Eval("Fecha", "{0:ddd dd/MM}") %></span>
                                <div class="barra-pista">
                                    <div class="barra-relleno" style='<%# "width:" + Eval("Porcentaje") + "%" %>'></div>
                                </div>
                                <span class="barra-monto num"><%# Eval("Total", "{0:C}") %></span>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </section>
        </div>
        <div class="col-lg-5">
            <section class="tarjeta h-100">
                <h2 class="titulo-seccion">Más vendidos del mes</h2>
                <asp:Label ID="lblSinVentasTop" runat="server" CssClass="texto-ayuda" Text="Todavía no hay ventas este mes." Visible="false" />
                <asp:Repeater ID="rptMasVendidos" runat="server">
                        <HeaderTemplate>
                            <table class="table tabla mb-0">
                            <thead>
                                <tr>
                                    <th>#</th>
                                    <th>Producto</th>
                                    <th class="num">Unidades</th>
                                    <th class="num">Total</th>
                                </tr>
                            </thead>
                            <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><span class="ranking"><%# Container.ItemIndex + 1 %></span></td>
                                <td><%#: Eval("Nombre") %></td>
                                <td class="num"><%# Eval("Unidades") %></td>
                                <td class="num"><%# Eval("Total", "{0:C}") %></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </tbody>
                            </table>
                        </FooterTemplate>
                </asp:Repeater>
            </section>
        </div>
    </div>
    <% } %>

    <h2 class="titulo-seccion mt-5">Últimas ventas</h2>
    <div class="tabla-contenedor">
        <asp:GridView ID="gvUltimasVentas" runat="server" AutoGenerateColumns="false"
            CssClass="table table-hover tabla tabla-clic" GridLines="None"
            ShowHeaderWhenEmpty="true" EmptyDataText="Todavía no hay ventas."
            DataKeyNames="Id"
            OnSelectedIndexChanged="gvUltimasVentas_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="NumeroFacturaFormateado" HeaderText="Factura" />
                <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
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
