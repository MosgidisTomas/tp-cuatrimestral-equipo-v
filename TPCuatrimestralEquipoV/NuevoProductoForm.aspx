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
                    <asp:TextBox ID="txtCodigo" runat="server" CssClass="form-control" MaxLength="20" />
                    <asp:RequiredFieldValidator ID="rfvCodigo" runat="server" ControlToValidate="txtCodigo" ErrorMessage="El código es obligatorio." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                    <asp:CustomValidator ID="cvCodigoRepetido" runat="server" ControlToValidate="txtCodigo" ErrorMessage="Ya existe otro producto con ese código." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                </div>
                <div class="col-md-8">
                    <asp:Label runat="server" AssociatedControlID="txtNombre" CssClass="form-label" Text="Nombre *" />
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" MaxLength="100" />
                    <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ControlToValidate="txtNombre" ErrorMessage="El nombre es obligatorio." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                </div>

                <div class="col-12">
                    <asp:Label runat="server" AssociatedControlID="txtDescripcion" CssClass="form-label" Text="Descripción" />
                    <asp:TextBox ID="txtDescripcion" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" MaxLength="500" />
                </div>

                <div class="col-12">
                    <asp:Label runat="server" AssociatedControlID="txtUrlImagen" CssClass="form-label" Text="URL de imagen" />
                    <asp:TextBox ID="txtUrlImagen" runat="server" CssClass="form-control" MaxLength="500" />
                    <asp:RegularExpressionValidator ID="revUrlImagen" runat="server" ControlToValidate="txtUrlImagen" ValidationExpression="^https?://\S+$" ErrorMessage="La URL tiene que empezar con http:// o https://." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                </div>

                <div class="col-md-6">
                    <asp:Label runat="server" AssociatedControlID="ddlMarca" CssClass="form-label" Text="Marca *" />
                    <asp:DropDownList ID="ddlMarca" runat="server" CssClass="form-select" />
                </div>
                <div class="col-md-6">
                    <asp:Label runat="server" AssociatedControlID="ddlCategoria" CssClass="form-label" Text="Categoría *" />
                    <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="form-select" />
                </div>

                <div class="col-md-6">
                    <asp:Label runat="server" AssociatedControlID="txtStockMinimo" CssClass="form-label" Text="Stock mínimo *" />
                    <asp:TextBox ID="txtStockMinimo" runat="server" TextMode="Number" CssClass="form-control" Text="0" />
                    <asp:RequiredFieldValidator ID="rfvStockMinimo" runat="server" ControlToValidate="txtStockMinimo" ErrorMessage="El stock mínimo es obligatorio." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                    <asp:RangeValidator ID="rvStockMinimo" runat="server" ControlToValidate="txtStockMinimo" Type="Integer" MinimumValue="0" MaximumValue="2147483647" CultureInvariantValues="true" ErrorMessage="Tiene que ser un número entero mayor o igual a 0." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                </div>
                <div class="col-md-6">
                    <asp:Label runat="server" AssociatedControlID="txtGanancia" CssClass="form-label" Text="Ganancia (%) *" />
                    <asp:TextBox ID="txtGanancia" runat="server" CssClass="form-control" MaxLength="6" placeholder="Ej: 25" />
                    <asp:RequiredFieldValidator ID="rfvGanancia" runat="server" ControlToValidate="txtGanancia" ErrorMessage="La ganancia es obligatoria." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                    <asp:RangeValidator ID="rvGanancia" runat="server" ControlToValidate="txtGanancia" Type="Double" MinimumValue="0" MaximumValue="999.99" CultureInvariantValues="true" ErrorMessage="Tiene que ser un número entre 0 y 999,99." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                </div>

                <div class="col-12">
                    <span class="form-label d-block">Proveedores * (a quiénes se les puede comprar)</span>
                    <div class="lista-casillas">
                        <asp:CheckBoxList ID="cblProveedores" runat="server" RepeatLayout="Flow" />
                    </div>
                    <asp:CustomValidator ID="cvProveedores" runat="server" ClientValidationFunction="validarProveedores" ErrorMessage="Elegí al menos un proveedor." Display="Dynamic" CssClass="text-danger small" />
                </div>

            </div>

            <% if (EsModificacion && !ProductoActivo) { %>
                <p class="texto-ayuda mt-4 mb-0">Este producto está inactivo: no aparece en las compras ni en las ventas.</p>
            <% } %>
            <div class="acciones">
                <% if (EsModificacion) { %>
                    <% if (ProductoActivo) { %>
                        <button type="button" class="btn btn-outline-danger me-auto" data-bs-toggle="modal" data-bs-target="#modalBaja">Dar de baja</button>
                    <% } else { %>
                        <asp:Button ID="btnReactivar" runat="server" Text="Reactivar" CssClass="btn btn-outline-success me-auto" OnClick="btnReactivar_Click" CausesValidation="false" />
                    <% } %>
                <% } %>
                <a href="Productos.aspx" class="btn btn-outline-secondary">Cancelar</a>
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardar_Click" />
            </div>
        </div>
    </asp:Panel>

    <% if (EsModificacion && ProductoActivo) { %>
    <div class="modal fade" id="modalBaja" tabindex="-1" aria-labelledby="tituloModalBaja" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h2 class="modal-title fs-5" id="tituloModalBaja">Dar de baja "<%: NombreSeleccionado %>"</h2>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>
                <div class="modal-body">
                    El producto deja de aparecer en las compras y en las ventas. Su stock y sus ventas anteriores no cambian. Se puede reactivar después.
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button ID="btnConfirmaBaja" runat="server" Text="Sí, dar de baja" CssClass="btn btn-danger" OnClick="btnConfirmaBaja_Click" CausesValidation="false" />
                </div>
            </div>
        </div>
    </div>
    <% } %>

    <script>
        // La llama el CustomValidator cvProveedores (ClientValidationFunction) junto con los demás validadores.
        // Es válido si hay al menos una casilla marcada en la lista de proveedores.
        function validarProveedores(source, args) {
            var casillas = document.querySelectorAll('.lista-casillas input[type="checkbox"]');
            args.IsValid = false;
            for (var i = 0; i < casillas.length; i++)
                if (casillas[i].checked)
                    args.IsValid = true;
        }
    </script>

</asp:Content>
