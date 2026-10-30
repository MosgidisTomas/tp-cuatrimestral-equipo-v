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
                    </div>
                    <div class="col-12">
                        <asp:Label runat="server" AssociatedControlID="txtRazonSocial" CssClass="form-label" Text="Razón social *" />
                        <asp:TextBox ID="txtRazonSocial" runat="server" CssClass="form-control" MaxLength="100" placeholder="Ej.: Componentes del Norte S.A." />
                    </div>
                    <div class="col-md-6">
                        <asp:Label runat="server" AssociatedControlID="txtTelefono" CssClass="form-label" Text="Teléfono" />
                        <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" MaxLength="30" placeholder="Ej.: 011 4555-1020" />
                    </div>
                    <div class="col-md-6">
                        <asp:Label runat="server" AssociatedControlID="txtEmail" CssClass="form-label" Text="Email" />
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" MaxLength="100" placeholder="Ej.: ventas@empresa.com" />
                    </div>
                    <div class="col-12">
                        <asp:Label runat="server" AssociatedControlID="txtDireccion" CssClass="form-label" Text="Dirección" />
                        <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control" MaxLength="150" />
                    </div>
                </div>

                <p class="texto-ayuda mt-3">El CUIT va con 11 números y no puede repetirse.</p>
                <asp:Label ID="lblError" runat="server" CssClass="text-danger d-block mb-2" />
                <div class="acciones mt-0">
                    <a href="Proveedores.aspx" class="btn btn-outline-secondary">Cancelar</a>
                    <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" CssClass="btn btn-primary" OnClick="btnAceptar_Click" />
                </div>

                <% if (EsModificacion) { %>
                <hr />
                    <% if (ProveedorActivo) { %>
                        <asp:Button ID="btnBaja" runat="server" Text="Dar de baja" CssClass="btn btn-outline-danger" OnClick="btnBaja_Click" />
                        <% if (ConfirmaBaja) { %>
                        <div class="mt-3 p-3 border border-danger rounded">
                            <asp:CheckBox ID="chkConfirmaBaja" runat="server" Text=" Confirmar baja" CssClass="d-block mb-2" />
                            <asp:Button ID="btnConfirmaBaja" runat="server" Text="Confirmar" CssClass="btn btn-danger" OnClick="btnConfirmaBaja_Click" />
                        </div>
                        <% } %>
                    <% } else { %>
                        <p class="texto-ayuda">Este proveedor está dado de baja.</p>
                        <asp:Button ID="btnReactivar" runat="server" Text="Reactivar" CssClass="btn btn-outline-success" OnClick="btnReactivar_Click" />
                    <% } %>
                <% } %>
            </div>
        </div>
    </div>

</asp:Content>
