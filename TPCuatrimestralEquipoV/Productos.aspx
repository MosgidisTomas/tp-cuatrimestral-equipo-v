<%@ Page Title="Productos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Productos.aspx.cs" Inherits="TPCuatrimestralEquipoV.Productos" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="encabezado-pagina">
        <div>
            <h1>Productos</h1>
            <p>El precio de venta se calcula con el costo de la última compra más el porcentaje de ganancia.</p>
        </div>
        <asp:Button ID="btnNuevo" runat="server" Text="Nuevo producto" CssClass="btn btn-primary" OnClick="btnNuevo_Click" UseSubmitBehavior="false" />
    </div>

    <asp:Panel ID="pnlFiltros" runat="server" DefaultButton="btnBuscar">
    <div class="tarjeta mb-4">
        <div class="row g-3 align-items-end">
            <div class="col-md-4">
                <asp:Label runat="server" AssociatedControlID="txtBuscar" CssClass="form-label" Text="Buscar" />
                <asp:TextBox ID="txtBuscar" runat="server" TextMode="Search" CssClass="form-control" placeholder="Código o nombre" />
            </div>
            <div class="col-sm-6 col-md-2">
                <asp:Label runat="server" AssociatedControlID="ddlCategoria" CssClass="form-label" Text="Categoría" />
                <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="form-select" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="btnBuscar_Click">
                    <asp:ListItem Value="" Text="Todas" />
                </asp:DropDownList>
            </div>
            <div class="col-sm-6 col-md-2">
                <asp:Label runat="server" AssociatedControlID="ddlMarca" CssClass="form-label" Text="Marca" />
                <asp:DropDownList ID="ddlMarca" runat="server" CssClass="form-select" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="btnBuscar_Click">
                    <asp:ListItem Value="" Text="Todas" />
                </asp:DropDownList>
            </div>
            <div class="col-sm-6 col-md-2">
                <asp:Label runat="server" AssociatedControlID="ddlEstado" CssClass="form-label" Text="Estado" />
                <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select" AutoPostBack ="true" OnSelectedIndexChanged="btnBuscar_Click">
                    <asp:ListItem Value="activos" Text="Activos" />
                    <asp:ListItem Value="inactivos" Text="Dados de baja" />
                    <asp:ListItem Value="todos" Text="Todos" />
                </asp:DropDownList>
            </div>
            <div class="col-sm-6 col-md-2">
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-outline-secondary w-100" OnClick="btnBuscar_Click" />
            </div>
        </div>
    </div>
</asp:Panel>

    <div class="tabla-contenedor">
        <asp:GridView ID="gvProductos" runat="server" AutoGenerateColumns="false" DataKeyNames="Id"
            CssClass="table table-hover tabla" GridLines="None"
            ShowHeaderWhenEmpty="true" EmptyDataText="No hay productos para mostrar."
            OnRowCommand="gvProductos_RowCommand" OnRowDataBound="gvProductos_RowDataBound">
            <Columns>
                <asp:BoundField DataField="Codigo" HeaderText="Código" />
                <asp:BoundField DataField="Nombre" HeaderText="Producto" />
                <asp:TemplateField HeaderText="Marca">
                    <ItemTemplate><%# Eval("Marca.Descripcion") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Categoría">
                    <ItemTemplate><%# Eval("Categoria.Descripcion") %></ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="PorcentajeGanancia" HeaderText="Ganancia (%)" DataFormatString="{0:0.##}" ItemStyle-CssClass="num" HeaderStyle-CssClass="num" />
                <asp:TemplateField HeaderText="Precio de venta" ItemStyle-CssClass="num" HeaderStyle-CssClass="num">
                    <ItemTemplate>
                        <%# (bool)Eval("TieneCompras") ? Eval("PrecioVenta", "{0:C}") : "Sin compras" %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="StockActual" HeaderText="Stock" ItemStyle-CssClass="num" HeaderStyle-CssClass="num" />
                <asp:BoundField DataField="StockMinimo" HeaderText="Stock mínimo" ItemStyle-CssClass="num" HeaderStyle-CssClass="num" />
                <asp:TemplateField HeaderText="Acciones">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEditar" runat="server" Text="Editar" CssClass="btn btn-sm btn-outline-primary"
                            CommandName="Editar" CommandArgument='<%# Eval("Id") %>' />
                        <asp:LinkButton ID="lnkBaja" runat="server" Text="Dar de baja" CssClass="btn btn-sm btn-outline-danger"
                            CommandName="Baja" CommandArgument='<%# Eval("Id") %>'
                            OnClientClick="return confirm('¿Dar de baja este producto?');"
                            Visible='<%# (bool)Eval("Activo") %>' />
                        <asp:LinkButton ID="lnkReactivar" runat="server" Text="Reactivar" CssClass="btn btn-sm btn-outline-success"
                            CommandName="Reactivar" CommandArgument='<%# Eval("Id") %>'
                            Visible='<%# !(bool)Eval("Activo") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
