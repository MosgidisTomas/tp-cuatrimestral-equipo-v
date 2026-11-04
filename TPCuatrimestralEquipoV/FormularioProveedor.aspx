<%@ Page Title="Proveedor" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FormularioProveedor.aspx.cs" Inherits="TPCuatrimestralEquipoV.FormularioProveedor" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="encabezado-pagina">
        <div>
            <h1><asp:Label ID="lblTitulo" runat="server" Text="Nuevo proveedor" /></h1>
            <p>Datos de la empresa a la que se le compra mercadería.</p>
        </div>
    </div>

    <div class="row g-4">
        <div class="col-lg-8">
            <div class="tarjeta">
                <div class="row g-3">
                    <div class="col-md-4">
                        <asp:Label runat="server" AssociatedControlID="txtId" CssClass="form-label" Text="Id" />
                        <asp:TextBox ID="txtId" runat="server" CssClass="form-control" Enabled="false" />
                    </div>
                    <div class="col-md-8">
                        <asp:Label runat="server" AssociatedControlID="txtCuit" CssClass="form-label" Text="CUIT *" />
                        <asp:TextBox ID="txtCuit" runat="server" CssClass="form-control" MaxLength="13" placeholder="Ej.: 30712345671" />
                        <asp:RequiredFieldValidator ID="rfvCuit" runat="server" ControlToValidate="txtCuit" ErrorMessage="El CUIT es obligatorio." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                        <asp:RegularExpressionValidator ID="revCuit" runat="server" ControlToValidate="txtCuit" ValidationExpression="^\d{2}-?\d{8}-?\d$" ErrorMessage="El CUIT tiene que tener 11 números (con o sin guiones)." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                        <asp:CustomValidator ID="cvCuitRepetido" runat="server" ControlToValidate="txtCuit" ErrorMessage="Ya existe un proveedor con ese CUIT." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                    </div>
                    <div class="col-12">
                        <asp:Label runat="server" AssociatedControlID="txtRazonSocial" CssClass="form-label" Text="Razón social *" />
                        <asp:TextBox ID="txtRazonSocial" runat="server" CssClass="form-control" MaxLength="100" placeholder="Ej.: Componentes del Norte S.A." />
                        <asp:RequiredFieldValidator ID="rfvRazonSocial" runat="server" ControlToValidate="txtRazonSocial" ErrorMessage="La razón social es obligatoria." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                    </div>
                    <div class="col-md-6">
                        <asp:Label runat="server" AssociatedControlID="txtTelefono" CssClass="form-label" Text="Teléfono" />
                        <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" MaxLength="30" placeholder="Ej.: 011 4555-1020" />
                    </div>
                    <div class="col-md-6">
                        <asp:Label runat="server" AssociatedControlID="txtEmail" CssClass="form-label" Text="Email" />
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" MaxLength="100" placeholder="Ej.: ventas@empresa.com" />
                        <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ErrorMessage="El email no es válido." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                    </div>
                    <div class="col-12">
                        <asp:Label runat="server" AssociatedControlID="txtDireccion" CssClass="form-label" Text="Dirección" />
                        <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control" MaxLength="150" />
                    </div>
                </div>

                <p class="texto-ayuda mt-3">El CUIT va con 11 números y no puede repetirse.</p>
                <asp:Label ID="lblError" runat="server" CssClass="text-danger d-block mb-2" />
                <% if (EsModificacion && !ProveedorActivo) { %>
                    <p class="texto-ayuda">Este proveedor está inactivo y no aparece en los formularios.</p>
                <% } %>
                <div class="acciones mt-0">
                    <% if (EsModificacion) { %>
                        <% if (ProveedorActivo) { %>
                            <button type="button" class="btn btn-outline-danger me-auto" data-bs-toggle="modal" data-bs-target="#modalBaja">Dar de baja</button>
                        <% } else { %>
                            <asp:Button ID="btnReactivar" runat="server" Text="Reactivar" CssClass="btn btn-outline-success me-auto" OnClick="btnReactivar_Click" CausesValidation="false" />
                        <% } %>
                    <% } %>
                    <a href="Proveedores.aspx" class="btn btn-outline-secondary">Cancelar</a>
                    <asp:Button ID="btnAceptar" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnAceptar_Click" />
                </div>
            </div>
        </div>
    </div>

    <% if (EsModificacion && ProveedorActivo) { %>
    <div class="modal fade" id="modalBaja" tabindex="-1" aria-labelledby="tituloModalBaja" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h2 class="modal-title fs-5" id="tituloModalBaja">Dar de baja "<%: NombreSeleccionado %>"</h2>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>
                <div class="modal-body">
                    El proveedor deja de aparecer en los formularios, así que no se le pueden cargar compras nuevas. Sus compras anteriores no cambian. Se puede reactivar después.
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button ID="btnConfirmaBaja" runat="server" Text="Sí, dar de baja" CssClass="btn btn-danger" OnClick="btnConfirmaBaja_Click" CausesValidation="false" />
                </div>
            </div>
        </div>
    </div>
    <% } %>

</asp:Content>
