<%@ Page Title="Productos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Productos.aspx.cs" Inherits="TPCuatrimestralEquipoV.Productos" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="encabezado-pagina">
        <div>
            <h1>Productos</h1>
            <p>El precio de venta se calcula con el costo de la última compra más el porcentaje de ganancia.</p>
        </div>
        <asp:Button ID="btnNuevo" runat="server" Text="Nuevo producto" CssClass="btn btn-primary" />
    </div>

    <div class="tarjeta mb-4">
        <div class="row g-3 align-items-end">
            <div class="col-md-4">
                <asp:Label runat="server" AssociatedControlID="txtBuscar" CssClass="form-label" Text="Buscar" />
                <asp:TextBox ID="txtBuscar" runat="server" TextMode="Search" CssClass="form-control" placeholder="Código o nombre" />
            </div>
            <div class="col-sm-6 col-md-2">
                <asp:Label runat="server" AssociatedControlID="ddlCategoria" CssClass="form-label" Text="Categoría" />
                <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="form-select">
                    <asp:ListItem Value="" Text="Todas" />
                </asp:DropDownList>
            </div>
            <div class="col-sm-6 col-md-2">
                <asp:Label runat="server" AssociatedControlID="ddlMarca" CssClass="form-label" Text="Marca" />
                <asp:DropDownList ID="ddlMarca" runat="server" CssClass="form-select">
                    <asp:ListItem Value="" Text="Todas" />
                </asp:DropDownList>
            </div>
            <div class="col-sm-6 col-md-2">
                <asp:Label runat="server" AssociatedControlID="ddlEstado" CssClass="form-label" Text="Estado" />
                <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select">
                    <asp:ListItem Value="activos" Text="Activos" />
                    <asp:ListItem Value="inactivos" Text="Dados de baja" />
                    <asp:ListItem Value="todos" Text="Todos" />
                </asp:DropDownList>
            </div>
            <div class="col-sm-6 col-md-2">
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-outline-secondary w-100" />
            </div>
        </div>
    </div>

    <div class="tabla-contenedor">
        <asp:GridView ID="gvProductos" runat="server" AutoGenerateColumns="false"
            CssClass="table table-hover tabla" GridLines="None"
            ShowHeaderWhenEmpty="true" EmptyDataText="No hay productos para mostrar.">
            <Columns>
                <asp:BoundField DataField="Codigo" HeaderText="Código" />
                <asp:BoundField DataField="Nombre" HeaderText="Producto" />
                <asp:BoundField DataField="Marca" HeaderText="Marca" />
                <asp:BoundField DataField="Categoria" HeaderText="Categoría" />
                <asp:BoundField DataField="PorcentajeGanancia" HeaderText="Ganancia (%)" DataFormatString="{0:0.##}" ItemStyle-CssClass="num" HeaderStyle-CssClass="num" />
                <asp:BoundField DataField="PrecioVenta" HeaderText="Precio de venta" DataFormatString="{0:C}" ItemStyle-CssClass="num" HeaderStyle-CssClass="num" />
                <asp:BoundField DataField="StockActual" HeaderText="Stock" ItemStyle-CssClass="num" HeaderStyle-CssClass="num" />
                <asp:BoundField DataField="StockMinimo" HeaderText="Stock mínimo" ItemStyle-CssClass="num" HeaderStyle-CssClass="num" />
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
