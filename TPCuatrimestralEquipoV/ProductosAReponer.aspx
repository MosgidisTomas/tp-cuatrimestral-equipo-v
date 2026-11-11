<%@ Page Title="Productos a reponer" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductosAReponer.aspx.cs" Inherits="TPCuatrimestralEquipoV.ProductosAReponer" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="encabezado-pagina">
        <div>
            <h1>Productos a reponer</h1>
            <p>Productos activos con stock igual o menor al mínimo, ordenados por faltante.</p>
        </div>
    </div>

    <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger d-block" Visible="false" />

    <div class="tabla-contenedor">
        <asp:GridView ID="gvReponer" runat="server" AutoGenerateColumns="false"
            CssClass="table tabla" GridLines="None"
            ShowHeaderWhenEmpty="true" EmptyDataText="No hay productos para reponer: todos tienen stock por encima del mínimo.">
            <Columns>
                <asp:BoundField DataField="Codigo" HeaderText="Código" />
                <asp:TemplateField HeaderText="Producto">
                    <ItemTemplate>
                        <span class="fw-semibold"><%#: Eval("Nombre") %></span><br />
                        <span class="texto-ayuda"><%#: Eval("Marca.Descripcion") %></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Categoría">
                    <ItemTemplate><%#: Eval("Categoria.Descripcion") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Stock" ItemStyle-CssClass="num" HeaderStyle-CssClass="num">
                    <ItemTemplate>
                        <span class='<%# (int)Eval("StockActual") == 0 ? "badge text-bg-danger" : "badge text-bg-warning" %>'><%# Eval("StockActual") %></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="StockMinimo" HeaderText="Mínimo" ItemStyle-CssClass="num" HeaderStyle-CssClass="num" />
                <asp:TemplateField HeaderText="Faltante" ItemStyle-CssClass="num fw-semibold" HeaderStyle-CssClass="num">
                    <ItemTemplate><%# (int)Eval("StockMinimo") - (int)Eval("StockActual") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Proveedores">
                    <ItemTemplate><span class="small"><%#: textoProveedores(Eval("Proveedores")) %></span></ItemTemplate>
                </asp:TemplateField>
                <asp:HyperLinkField Text="Registrar compra" NavigateUrl="~/NuevaCompra" ControlStyle-CssClass="btn btn-outline-secondary btn-sm" ItemStyle-CssClass="text-end" />
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
