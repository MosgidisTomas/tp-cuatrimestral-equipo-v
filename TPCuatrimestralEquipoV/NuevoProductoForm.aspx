<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NuevoProductoForm.aspx.cs" Inherits="TPCuatrimestralEquipoV.NuevoProductoForm" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="encabezado-pagina">
        <div>
            <h1><asp:Label ID="lblTitulo" runat="server" Text="Nuevo producto" /></h1>
            <p>Completá los datos del producto. El precio de venta se calcula a partir de la última compra y el porcentaje de ganancia.</p>
        </div>
    </div>

    <asp:Label ID="lblError" runat="server" CssClass="text-danger d-block mb-3" EnableViewState="false" />

    <asp:Panel ID="pnlFormulario" runat="server" DefaultButton="btnGuardar">
        <div class="tarjeta">
            <div class="row g-3">

                <div class="col-md-4">
                    <asp:Label runat="server" AssociatedControlID="txtCodigo" CssClass="form-label" Text="Código *" />
                    <asp:TextBox ID="txtCodigo" runat="server" CssClass="form-control" MaxLength="50" />
                </div>
                <div class="col-md-8">
                    <asp:Label runat="server" AssociatedControlID="txtNombre" CssClass="form-label" Text="Nombre *" />
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" MaxLength="100" />
                </div>

                <div class="col-12">
                    <asp:Label runat="server" AssociatedControlID="txtDescripcion" CssClass="form-label" Text="Descripción" />
                    <asp:TextBox ID="txtDescripcion" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" />
                </div>

                <div class="col-12">
                    <asp:Label runat="server" AssociatedControlID="txtUrlImagen" CssClass="form-label" Text="URL de imagen" />
                    <asp:TextBox ID="txtUrlImagen" runat="server" CssClass="form-control" />
                </div>

                <div class="col-md-6">
                    <asp:Label runat="server" AssociatedControlID="ddlMarca" CssClass="form-label" Text="Marca *" />
                    <asp:DropDownList ID="ddlMarca" runat="server" CssClass="form-select" />
                </div>
                <div class="col-md-6">
                    <asp:Label runat="server" AssociatedControlID="ddlCategoria" CssClass="form-label" Text="Categoría *" />
                    <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="form-select" />
                </div>

                <div class="col-md-4">
                    <asp:Label runat="server" AssociatedControlID="txtStockActual" CssClass="form-label" Text="Stock inicial" />
                    <asp:TextBox ID="txtStockActual" runat="server" TextMode="Number" CssClass="form-control" Text="0" />
                </div>
                <div class="col-md-4">
                    <asp:Label runat="server" AssociatedControlID="txtStockMinimo" CssClass="form-label" Text="Stock mínimo" />
                    <asp:TextBox ID="txtStockMinimo" runat="server" TextMode="Number" CssClass="form-control" Text="0" />
                </div>
                <div class="col-md-4">
                    <asp:Label runat="server" AssociatedControlID="txtGanancia" CssClass="form-label" Text="Ganancia (%) *" />
                    <asp:TextBox ID="txtGanancia" runat="server" CssClass="form-control" placeholder="Ej: 25" />
                </div>

                <div class="col-12">
                    <asp:Label runat="server" AssociatedControlID="lbProveedores" CssClass="form-label" Text="Proveedores (Ctrl + clic para elegir varios)" />
                    <asp:ListBox ID="lbProveedores" runat="server" SelectionMode="Multiple" Rows="6" CssClass="form-select" />
                </div>

            </div>

            <div class="mt-4">
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardar_Click" />
                <a href="Productos.aspx" class="btn btn-outline-secondary">Cancelar</a>
            </div>
        </div>
    </asp:Panel>

</asp:Content>
