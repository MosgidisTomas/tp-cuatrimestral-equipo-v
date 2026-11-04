<%@ Page Title="Cliente" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FormularioCliente.aspx.cs" Inherits="TPCuatrimestralEquipoV.FormularioCliente" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="encabezado-pagina">
        <div>
            <h1><asp:Label ID="lblTitulo" runat="server" Text="Nuevo cliente" /></h1>
            <p>Datos de la persona a la que se le vende.</p>
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
                        <asp:Label runat="server" AssociatedControlID="txtDocumento" CssClass="form-label" Text="DNI *" />
                        <asp:TextBox ID="txtDocumento" runat="server" CssClass="form-control" MaxLength="10" placeholder="Ej.: 30123456" />
                        <asp:RequiredFieldValidator ID="rfvDocumento" runat="server" ControlToValidate="txtDocumento" ErrorMessage="El DNI es obligatorio." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                        <asp:RegularExpressionValidator ID="revDocumento" runat="server" ControlToValidate="txtDocumento" ValidationExpression="^\d{1,2}\.?\d{3}\.?\d{3}$" ErrorMessage="El DNI tiene que tener 7 u 8 números (con o sin puntos)." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                        <asp:CustomValidator ID="cvDocumentoRepetido" runat="server" ControlToValidate="txtDocumento" ErrorMessage="Ya existe un cliente con ese DNI." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                    </div>
                    <div class="col-md-6">
                        <asp:Label runat="server" AssociatedControlID="txtNombre" CssClass="form-label" Text="Nombre *" />
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" MaxLength="50" placeholder="Ej.: Laura" />
                        <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ControlToValidate="txtNombre" ErrorMessage="El nombre es obligatorio." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                    </div>
                    <div class="col-md-6">
                        <asp:Label runat="server" AssociatedControlID="txtApellido" CssClass="form-label" Text="Apellido *" />
                        <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" MaxLength="50" placeholder="Ej.: Fernández" />
                        <asp:RequiredFieldValidator ID="rfvApellido" runat="server" ControlToValidate="txtApellido" ErrorMessage="El apellido es obligatorio." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                    </div>
                    <div class="col-md-6">
                        <asp:Label runat="server" AssociatedControlID="txtEmail" CssClass="form-label" Text="Email *" />
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" MaxLength="100" placeholder="Ej.: laura@mail.com" />
                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="El email es obligatorio." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                        <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ErrorMessage="El email no es válido." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                        <asp:CustomValidator ID="cvEmailRepetido" runat="server" ControlToValidate="txtEmail" ErrorMessage="Ya existe un cliente con ese email." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                    </div>
                    <div class="col-md-6">
                        <asp:Label runat="server" AssociatedControlID="txtTelefono" CssClass="form-label" Text="Teléfono" />
                        <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" MaxLength="30" placeholder="Ej.: 11 5555-0101" />
                    </div>
                    <div class="col-12">
                        <asp:Label runat="server" AssociatedControlID="txtDireccion" CssClass="form-label" Text="Dirección" />
                        <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control" MaxLength="150" />
                    </div>
                    <% if (EsModificacion) { %>
                    <div class="col-md-4">
                        <asp:Label runat="server" AssociatedControlID="txtFechaAlta" CssClass="form-label" Text="Fecha de alta" />
                        <asp:TextBox ID="txtFechaAlta" runat="server" CssClass="form-control" Enabled="false" />
                    </div>
                    <% } %>
                </div>

                <p class="texto-ayuda mt-3">El DNI y el email no pueden repetirse. La fecha de alta la pone el sistema.</p>
                <asp:Label ID="lblError" runat="server" CssClass="text-danger d-block mb-2" />
                <% if (EsModificacion && !ClienteActivo) { %>
                    <p class="texto-ayuda">Este cliente está inactivo: no se le pueden registrar ventas.</p>
                <% } %>
                <div class="acciones mt-0">
                    <%-- Etapa 3: la baja y la reactivación van a quedar solo para el Administrador. --%>
                    <% if (EsModificacion) { %>
                        <% if (ClienteActivo) { %>
                            <button type="button" class="btn btn-outline-danger me-auto" data-bs-toggle="modal" data-bs-target="#modalBaja">Dar de baja</button>
                        <% } else { %>
                            <asp:Button ID="btnReactivar" runat="server" Text="Reactivar" CssClass="btn btn-outline-success me-auto" OnClick="btnReactivar_Click" CausesValidation="false" />
                        <% } %>
                    <% } %>
                    <a href="Clientes.aspx" class="btn btn-outline-secondary">Cancelar</a>
                    <asp:Button ID="btnAceptar" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnAceptar_Click" />
                </div>
            </div>
        </div>
    </div>

    <% if (EsModificacion && ClienteActivo) { %>
    <div class="modal fade" id="modalBaja" tabindex="-1" aria-labelledby="tituloModalBaja" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h2 class="modal-title fs-5" id="tituloModalBaja">Dar de baja "<%: NombreSeleccionado %>"</h2>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>
                <div class="modal-body">
                    El cliente deja de aparecer al registrar ventas, así que no se le puede vender. Sus ventas anteriores no cambian. Se puede reactivar después.
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
