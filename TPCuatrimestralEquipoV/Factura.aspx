<%@ Page Title="Factura" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Factura.aspx.cs" Inherits="TPCuatrimestralEquipoV.Factura" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <%-- Etapa 2: maqueta con datos de ejemplo.
         Etapa 3: se carga la venta real con Factura.aspx?id=<Id de la venta> desde "Ventas y facturas" y al confirmar una venta. --%>

    <div class="factura-contenedor">
        <div class="acciones mt-0 mb-2 justify-content-between no-imprimir">
            <a href="Ventas.aspx" class="btn btn-outline-secondary">Volver a ventas</a>
            <button type="button" class="btn btn-primary" onclick="window.print();">
                <i class="bi bi-printer" aria-hidden="true"></i> Imprimir
            </button>
        </div>
        <p class="texto-ayuda no-imprimir">Al imprimir se ocultan el menú, la barra superior y estos botones: sale solo la hoja.</p>

        <article class="factura-hoja">
            <div class="factura-encabezado">
                <div>
                    <div class="factura-marca">UTN</div>
                    <div class="factura-marca factura-marca-chica">HARDWARE</div>
                    <div class="factura-dato-suave mt-2">Componentes de PC · Av. Hipólito Yrigoyen 288, Gral. Pacheco</div>
                </div>
                <div class="text-end">
                    <div class="factura-marca factura-titulo">FACTURA</div>
                    <div class="num fs-5 fw-semibold mt-2">Nº 0001-00000006</div>
                    <div>Fecha: 28/10/2026</div>
                </div>
            </div>

            <div class="factura-cliente">
                <div><span class="factura-etiqueta">CLIENTE</span><br /><strong>Fernández, Laura</strong></div>
                <div><span class="factura-etiqueta">DNI</span><br /><span class="num">30123456</span></div>
                <div><span class="factura-etiqueta">EMAIL</span><br />laura.fernandez@mail.example</div>
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
                    <tr>
                        <td>AMD Ryzen 5 7600</td>
                        <td class="num">1</td>
                        <td class="num">$ 287.500,00</td>
                        <td class="num">$ 287.500,00</td>
                    </tr>
                    <tr>
                        <td>Kingston Fury Beast 16 GB DDR5</td>
                        <td class="num">2</td>
                        <td class="num">$ 78.000,00</td>
                        <td class="num">$ 156.000,00</td>
                    </tr>
                </tbody>
            </table>

            <div class="factura-total">
                <span class="factura-etiqueta">TOTAL</span>
                <span class="factura-marca num">$ 443.500,00</span>
            </div>

            <p class="factura-dato-suave text-center mb-0">Gracias por su compra.</p>
        </article>
    </div>

</asp:Content>
