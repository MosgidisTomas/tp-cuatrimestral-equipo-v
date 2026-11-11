<%@ Page Title="Usuario" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FormularioUsuario.aspx.cs" Inherits="TPCuatrimestralEquipoV.FormularioUsuario" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="encabezado-pagina">
        <div>
            <h1><asp:Label ID="lblTitulo" runat="server" Text="Nuevo usuario" /></h1>
            <p>Persona que ingresa al sistema: el Administrador puede hacer todo y el Vendedor registra ventas.</p>
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
                        <asp:Label runat="server" AssociatedControlID="txtNombreUsuario" CssClass="form-label" Text="Nombre de usuario *" />
                        <asp:TextBox ID="txtNombreUsuario" runat="server" CssClass="form-control" MaxLength="30" placeholder="Ej.: laura.fernandez" />
                        <asp:RequiredFieldValidator ID="rfvNombreUsuario" runat="server" ControlToValidate="txtNombreUsuario" ErrorMessage="El nombre de usuario es obligatorio." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                        <asp:RegularExpressionValidator ID="revNombreUsuario" runat="server" ControlToValidate="txtNombreUsuario" ValidationExpression="^[A-Za-z0-9._]{3,30}$" ErrorMessage="Entre 3 y 30 caracteres: letras, números, punto o guion bajo, sin espacios." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                        <asp:CustomValidator ID="cvNombreUsuarioRepetido" runat="server" ControlToValidate="txtNombreUsuario" ErrorMessage="Ya existe un usuario con ese nombre." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
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
                    <div class="col-md-8">
                        <asp:Label runat="server" AssociatedControlID="txtEmail" CssClass="form-label" Text="Email *" />
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" MaxLength="100" placeholder="Ej.: laura@utnhardware.com" />
                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="El email es obligatorio." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                        <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ErrorMessage="El email no es válido." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                        <asp:CustomValidator ID="cvEmailRepetido" runat="server" ControlToValidate="txtEmail" ErrorMessage="Ya existe un usuario con ese email." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                    </div>
                    <div class="col-md-4">
                        <asp:Label runat="server" AssociatedControlID="ddlPerfil" CssClass="form-label" Text="Perfil *" />
                        <asp:DropDownList ID="ddlPerfil" runat="server" CssClass="form-select">
                            <asp:ListItem Value="2" Text="Vendedor" />
                            <asp:ListItem Value="1" Text="Administrador" />
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-6">
                        <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" CssClass="form-label" Text="Contraseña *" />
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="La contraseña es obligatoria." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                        <asp:RegularExpressionValidator ID="revPassword" runat="server" ControlToValidate="txtPassword" ValidationExpression="^.{6,}$" ErrorMessage="La contraseña tiene que tener al menos 6 caracteres." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                    </div>
                    <div class="col-md-6">
                        <asp:Label ID="lblPassword2" runat="server" AssociatedControlID="txtPassword2" CssClass="form-label" Text="Repetir contraseña *" />
                        <asp:TextBox ID="txtPassword2" runat="server" TextMode="Password" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvPassword2" runat="server" ControlToValidate="txtPassword2" ErrorMessage="Repetí la contraseña." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                        <asp:CompareValidator ID="cmpPassword" runat="server" ControlToValidate="txtPassword2" ControlToCompare="txtPassword" Operator="Equal" ErrorMessage="Las contraseñas no coinciden." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                    </div>
                </div>

                <% if (EsModificacion) { %>
                    <p class="texto-ayuda mt-3">La contraseña no se muestra. Dejala vacía para no cambiarla.</p>
                <% } else { %>
                    <p class="texto-ayuda mt-3">El nombre de usuario y el email no pueden repetirse. La contraseña no se guarda tal cual: se guarda su hash.</p>
                <% } %>
                <asp:Label ID="lblError" runat="server" CssClass="text-danger d-block mb-2" />
                <% if (EsModificacion && !UsuarioActivo) { %>
                    <p class="texto-ayuda">Este usuario está inactivo: no puede ingresar al sistema.</p>
                <% } %>
                <div class="acciones mt-0">
                    <%-- Etapa 3 (con login): que un administrador no pueda darse de baja a sí mismo ni sacarse el perfil de Administrador. --%>
                    <% if (EsModificacion) { %>
                        <% if (UsuarioActivo) { %>
                            <button type="button" class="btn btn-outline-danger me-auto" data-bs-toggle="modal" data-bs-target="#modalBaja">Dar de baja</button>
                        <% } else { %>
                            <asp:Button ID="btnReactivar" runat="server" Text="Reactivar" CssClass="btn btn-outline-success me-auto" OnClick="btnReactivar_Click" CausesValidation="false" />
                        <% } %>
                    <% } %>
                    <a href="Usuarios.aspx" class="btn btn-outline-secondary">Cancelar</a>
                    <asp:Button ID="btnAceptar" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnAceptar_Click" />
                </div>
            </div>
        </div>
    </div>

    <% if (EsModificacion && UsuarioActivo) { %>
    <div class="modal fade" id="modalBaja" tabindex="-1" aria-labelledby="tituloModalBaja" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h2 class="modal-title fs-5" id="tituloModalBaja">Dar de baja "<%: NombreSeleccionado %>"</h2>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>
                <div class="modal-body">
                    El usuario ya no va a poder ingresar al sistema. Las ventas y compras que registró no cambian. Se puede reactivar después.
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
