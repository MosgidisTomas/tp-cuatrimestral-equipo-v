<%@ Page Title="Nueva compra" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NuevaCompra.aspx.cs" Inherits="TPCuatrimestralEquipoV.NuevaCompra" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="encabezado-pagina">
        <div>
            <h1>Nueva compra</h1>
            <p>Ingreso de mercadería comprada a un proveedor.</p>
        </div>
    </div>

    <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger d-block" Visible="false" />

    <section class="tarjeta">
        <h2 class="titulo-seccion">Datos de la compra</h2>
        <div class="row g-3">
            <div class="col-md-4">
                <asp:Label runat="server" AssociatedControlID="ddlProveedor" CssClass="form-label" Text="Proveedor *" />
                <asp:DropDownList ID="ddlProveedor" runat="server" CssClass="form-select"
                    AutoPostBack="true" OnSelectedIndexChanged="ddlProveedor_SelectedIndexChanged" />
                <asp:RequiredFieldValidator ID="rfvProveedor" runat="server" ControlToValidate="ddlProveedor" InitialValue="" ValidationGroup="Confirmar" ErrorMessage="Elegí un proveedor." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
            </div>
            <div class="col-md-4">
                <asp:Label runat="server" AssociatedControlID="txtNumeroFactura" CssClass="form-label" Text="Nº de factura del proveedor *" />
                <asp:TextBox ID="txtNumeroFactura" runat="server" CssClass="form-control" MaxLength="30" placeholder="Ej.: A-0003-00012845" />
                <asp:RequiredFieldValidator ID="rfvNumeroFactura" runat="server" ControlToValidate="txtNumeroFactura" ValidationGroup="Confirmar" ErrorMessage="El número de factura es obligatorio." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                <asp:CustomValidator ID="cvFacturaRepetida" runat="server" ControlToValidate="txtNumeroFactura" ValidationGroup="Confirmar" ErrorMessage="Ya hay una compra a este proveedor con ese número de factura." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
            </div>
            <div class="col-md-4">
                <asp:Label runat="server" AssociatedControlID="txtFecha" CssClass="form-label" Text="Fecha de la factura *" />
                <asp:TextBox ID="txtFecha" runat="server" TextMode="Date" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvFecha" runat="server" ControlToValidate="txtFecha" ValidationGroup="Confirmar" ErrorMessage="La fecha es obligatoria." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                <asp:CustomValidator ID="cvFecha" runat="server" ControlToValidate="txtFecha" ValidationGroup="Confirmar" ErrorMessage="La fecha no puede ser posterior a hoy." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
            </div>
        </div>
    </section>

    <section class="tarjeta">
        <h2 class="titulo-seccion">Productos</h2>
        <div class="row g-3 align-items-start">
            <div class="col-md-5">
                <asp:Label runat="server" AssociatedControlID="ddlProducto" CssClass="form-label" Text="Producto" />
                <asp:DropDownList ID="ddlProducto" runat="server" CssClass="form-select"
                    AutoPostBack="true" OnSelectedIndexChanged="ddlProducto_SelectedIndexChanged" />
                <asp:RequiredFieldValidator ID="rfvProducto" runat="server" ControlToValidate="ddlProducto" InitialValue="" ValidationGroup="Agregar" ErrorMessage="Elegí un producto." Display="Dynamic" CssClass="text-danger small" />
            </div>
            <div class="col-sm-4 col-md-2">
                <asp:Label runat="server" AssociatedControlID="txtCantidad" CssClass="form-label" Text="Cantidad" />
                <asp:TextBox ID="txtCantidad" runat="server" TextMode="Number" CssClass="form-control" min="1" />
                <asp:RequiredFieldValidator ID="rfvCantidad" runat="server" ControlToValidate="txtCantidad" ValidationGroup="Agregar" ErrorMessage="Obligatoria." Display="Dynamic" CssClass="text-danger small" />
                <asp:RangeValidator ID="rvCantidad" runat="server" ControlToValidate="txtCantidad" Type="Integer" MinimumValue="1" MaximumValue="9999" ValidationGroup="Agregar" ErrorMessage="Entre 1 y 9999." Display="Dynamic" CssClass="text-danger small" />
            </div>
            <div class="col-sm-4 col-md-3">
                <asp:Label runat="server" AssociatedControlID="txtCosto" CssClass="form-label" Text="Costo unitario ($)" />
                <asp:TextBox ID="txtCosto" runat="server" CssClass="form-control" MaxLength="12" placeholder="Ej: 230000,50" />
                <asp:RequiredFieldValidator ID="rfvCosto" runat="server" ControlToValidate="txtCosto" ValidationGroup="Agregar" ErrorMessage="El costo es obligatorio." Display="Dynamic" CssClass="text-danger small" />
                <asp:RangeValidator ID="rvCosto" runat="server" ControlToValidate="txtCosto" Type="Double" MinimumValue="0.01" MaximumValue="99999999.99" CultureInvariantValues="true" ValidationGroup="Agregar" ErrorMessage="Tiene que ser un número mayor a 0 (con coma para los centavos)." Display="Dynamic" CssClass="text-danger small" />
            </div>
            <div class="col-sm-4 col-md-2">
                <span class="form-label d-block">&nbsp;</span>
                <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="btn btn-outline-secondary w-100" ValidationGroup="Agregar" OnClick="btnAgregar_Click" />
            </div>
        </div>
        <div class="row gx-3 mt-2 mb-3 texto-ayuda">
            <div class="col-md-7">Solo aparecen los productos del proveedor elegido. Para cambiar de proveedor, quitá los productos agregados.</div>
            <div class="col-md-3"><asp:Label ID="lblUltimoCosto" runat="server" /></div>
        </div>

        <div class="tabla-contenedor">
            <asp:GridView ID="gvDetalle" runat="server" AutoGenerateColumns="false"
                CssClass="table tabla" GridLines="None"
                ShowHeaderWhenEmpty="true" EmptyDataText="Todavía no agregaste productos a la compra."
                OnRowDeleting="gvDetalle_RowDeleting">
                <Columns>
                    <asp:TemplateField HeaderText="Producto">
                        <ItemTemplate><%#: Eval("Producto.Marca.Descripcion") %> <%#: Eval("Producto.Nombre") %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" ItemStyle-CssClass="num" HeaderStyle-CssClass="num" />
                    <asp:BoundField DataField="PrecioUnitario" HeaderText="Costo unitario" DataFormatString="{0:C}" ItemStyle-CssClass="num" HeaderStyle-CssClass="num" />
                    <asp:BoundField DataField="Subtotal" HeaderText="Subtotal" DataFormatString="{0:C}" ItemStyle-CssClass="num" HeaderStyle-CssClass="num" />
                    <asp:CommandField ShowDeleteButton="true" DeleteText="Quitar" ButtonType="Button" CausesValidation="false"
                        ControlStyle-CssClass="btn btn-outline-danger btn-sm" ItemStyle-CssClass="text-end" />
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
        <asp:Button ID="btnConfirmar" runat="server" Text="Confirmar compra" CssClass="btn btn-primary" ValidationGroup="Confirmar" OnClick="btnConfirmar_Click" />
    </div>

</asp:Content>
