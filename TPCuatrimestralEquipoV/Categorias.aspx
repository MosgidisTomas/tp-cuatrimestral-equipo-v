<%@ Page Title="Categorías" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Categorias.aspx.cs" Inherits="TPCuatrimestralEquipoV.Categorias" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="encabezado-pagina">
        <div>
            <h1>Categorías</h1>
            <p>Tipos de componente con los que se clasifican los productos.</p>
        </div>
    </div>

    <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger d-block" Visible="false" />

    <div class="row g-4">
        <div class="col-lg-8">
            <div class="tabla-contenedor">
                <asp:GridView ID="gvCategorias" runat="server" AutoGenerateColumns="false"
                    CssClass="table table-hover tabla" GridLines="None"
                    ShowHeaderWhenEmpty="true" EmptyDataText="No hay categorías para mostrar."
                    DataKeyNames="Id"
                    OnSelectedIndexChanged="gvCategorias_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="Descripcion" HeaderText="Nombre" />
                        <asp:CheckBoxField DataField="Activo" HeaderText="Activa" />
                        <asp:CommandField ShowSelectButton="true" SelectText="✏️" HeaderText="Editar" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <div class="col-lg-4">
            <div class="tarjeta">
                <h2 class="titulo-seccion">
                    <asp:Label ID="lblTituloFormulario" runat="server" Text="Nueva categoría" />
                </h2>
                <div class="mb-3">
                    <asp:Label runat="server" AssociatedControlID="txtDescripcion" CssClass="form-label" Text="Nombre *" />
                    <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" MaxLength="50" placeholder="Ej.: Gabinetes" />
                </div>
                <p class="texto-ayuda">No puede haber dos categorías con el mismo nombre.</p>
                <asp:Label ID="lblErrorFormulario" runat="server" CssClass="text-danger d-block mb-2" />
                <div class="acciones mt-0">
                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-outline-secondary" OnClick="btnLimpiar_Click" />
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardar_Click" />
                </div>

                <% if (IdCategoria != 0) { %>
                <hr />
                    <% if (CategoriaActiva) { %>
                        <asp:Button ID="btnBaja" runat="server" Text="Dar de baja" CssClass="btn btn-outline-danger" OnClick="btnBaja_Click" />
                        <% if (ConfirmaBaja) { %>
                        <div class="mt-3 p-3 border border-danger rounded">
                            <asp:CheckBox ID="chkConfirmaBaja" runat="server" Text=" Confirmar baja" CssClass="d-block mb-2" />
                            <asp:Button ID="btnConfirmaBaja" runat="server" Text="Confirmar" CssClass="btn btn-danger" OnClick="btnConfirmaBaja_Click" />
                        </div>
                        <% } %>
                    <% } else { %>
                        <p class="texto-ayuda">Esta categoría está dada de baja.</p>
                        <asp:Button ID="btnReactivar" runat="server" Text="Reactivar" CssClass="btn btn-outline-success" OnClick="btnReactivar_Click" />
                    <% } %>
                <% } %>
            </div>
        </div>
    </div>

</asp:Content>
