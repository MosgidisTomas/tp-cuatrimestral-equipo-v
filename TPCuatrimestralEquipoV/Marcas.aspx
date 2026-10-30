<%@ Page Title="Marcas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Marcas.aspx.cs" Inherits="TPCuatrimestralEquipoV.Marcas" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="encabezado-pagina">
        <div>
            <h1>Marcas</h1>
            <p>Fabricantes de los productos del catálogo.</p>
        </div>
    </div>

    <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger d-block" Visible="false" />

    <div class="row g-4">
        <div class="col-lg-8">
            <div class="tabla-contenedor">
                <asp:GridView ID="gvMarcas" runat="server" AutoGenerateColumns="false"
                    CssClass="table table-hover tabla" GridLines="None"
                    EmptyDataText="No hay marcas cargadas."
                    DataKeyNames="Id"
                    OnSelectedIndexChanged="gvMarcas_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="Descripcion" HeaderText="Nombre" />
                        <asp:BoundField DataField="PaisOrigen" HeaderText="País de origen" />
                        <asp:CheckBoxField DataField="Activo" HeaderText="Activa" />
                        <asp:CommandField ShowSelectButton="true" SelectText="✏️" HeaderText="Editar" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <div class="col-lg-4">
            <div class="tarjeta">
                <h2 class="titulo-seccion">
                    <asp:Label ID="lblTituloFormulario" runat="server" Text="Nueva marca" />
                </h2>
                <div class="mb-3">
                    <asp:Label runat="server" AssociatedControlID="txtDescripcion" CssClass="form-label" Text="Nombre *" />
                    <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" MaxLength="50" placeholder="Ej.: Gigabyte" />
                </div>
                <div class="mb-3">
                    <asp:Label runat="server" AssociatedControlID="txtPaisOrigen" CssClass="form-label" Text="País de origen" />
                    <asp:TextBox ID="txtPaisOrigen" runat="server" CssClass="form-control" MaxLength="50" placeholder="Ej.: Taiwán" />
                </div>
                <p class="texto-ayuda">No puede haber dos marcas con el mismo nombre.</p>
                <asp:Label ID="lblErrorFormulario" runat="server" CssClass="text-danger d-block mb-2" />
                <div class="acciones mt-0">
                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-outline-secondary" OnClick="btnLimpiar_Click" />
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardar_Click" />
                </div>

                <% if (IdMarca != 0) { %>
                <hr />
                    <% if (MarcaActiva) { %>
                        <asp:Button ID="btnBaja" runat="server" Text="Dar de baja" CssClass="btn btn-outline-danger" OnClick="btnBaja_Click" />
                        <% if (ConfirmaBaja) { %>
                        <div class="mt-3 p-3 border border-danger rounded">
                            <asp:CheckBox ID="chkConfirmaBaja" runat="server" Text=" Confirmar baja" CssClass="d-block mb-2" />
                            <asp:Button ID="btnConfirmaBaja" runat="server" Text="Confirmar" CssClass="btn btn-danger" OnClick="btnConfirmaBaja_Click" />
                        </div>
                        <% } %>
                    <% } else { %>
                        <p class="texto-ayuda">Esta marca está dada de baja.</p>
                        <asp:Button ID="btnReactivar" runat="server" Text="Reactivar" CssClass="btn btn-outline-success" OnClick="btnReactivar_Click" />
                    <% } %>
                <% } %>
            </div>
        </div>
    </div>

</asp:Content>
