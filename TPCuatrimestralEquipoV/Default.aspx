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

        <a class="acceso" runat="server" href="~/Clientes">
            <span class="acceso-icono"><i class="bi bi-person" aria-hidden="true"></i></span>
            <span class="acceso-titulo">Clientes</span>
            <span class="acceso-detalle">Quienes pueden comprar en la tienda</span>
        </a>
    </div>

</asp:Content>
