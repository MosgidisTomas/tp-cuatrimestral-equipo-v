<%@ Page Title="Categorías" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Categorias.aspx.cs" Inherits="TPCuatrimestralEquipoV.Categorias" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="encabezado-pagina">
        <div>
            <h1>Categorías</h1>
            <p>Tipos de componente con los que se clasifican los productos. Tocá una categoría para editarla.</p>
        </div>
    </div>

    <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger d-block" Visible="false" />

    <div class="row g-4">
        <div class="col-lg-8">
            <div class="tabla-contenedor">
                <asp:GridView ID="gvCategorias" runat="server" AutoGenerateColumns="false"
                    CssClass="table table-hover tabla tabla-clic" GridLines="None"
                    ShowHeaderWhenEmpty="true" EmptyDataText="No hay categorías para mostrar."
                    DataKeyNames="Id" SelectedRowStyle-CssClass="fila-seleccionada"
                    OnSelectedIndexChanged="gvCategorias_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="Descripcion" HeaderText="Nombre" />
                        <asp:TemplateField HeaderText="Estado">
                            <ItemTemplate>
                                <span class='<%# (bool)Eval("Activo") ? "badge text-bg-success" : "badge text-bg-secondary" %>'><%# (bool)Eval("Activo") ? "Activa" : "Inactiva" %></span>
                            </ItemTemplate>
                        </asp:TemplateField>
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
                    <asp:RequiredFieldValidator ID="rfvDescripcion" runat="server" ControlToValidate="txtDescripcion" ErrorMessage="El nombre es obligatorio." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                    <asp:CustomValidator ID="cvDescripcionRepetida" runat="server" ControlToValidate="txtDescripcion" ErrorMessage="Ya existe una categoría con ese nombre." Display="Dynamic" CssClass="text-danger small" SetFocusOnError="true" />
                </div>
                <p class="texto-ayuda">No puede haber dos categorías con el mismo nombre.</p>
                <div class="acciones mt-0">
                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-outline-secondary" OnClick="btnLimpiar_Click" CausesValidation="false" />
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardar_Click" />
                </div>

                <% if (IdCategoria != 0) { %>
                <div class="border-top mt-4 pt-4">
                    <% if (CategoriaActiva) { %>
                        <button type="button" class="btn btn-outline-danger w-100" data-bs-toggle="modal" data-bs-target="#modalBaja">Dar de baja</button>
                    <% } else { %>
                        <p class="texto-ayuda">Esta categoría está inactiva y no aparece en los formularios.</p>
                        <asp:Button ID="btnReactivar" runat="server" Text="Reactivar" CssClass="btn btn-outline-success w-100" OnClick="btnReactivar_Click" CausesValidation="false" />
                    <% } %>
                </div>
                <% } %>
            </div>
        </div>
    </div>

    <% if (IdCategoria != 0 && CategoriaActiva) { %>
    <div class="modal fade" id="modalBaja" tabindex="-1" aria-labelledby="tituloModalBaja" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h2 class="modal-title fs-5" id="tituloModalBaja">Dar de baja "<%: NombreSeleccionado %>"</h2>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>
                <div class="modal-body">
                    La categoría deja de aparecer en los formularios. Los productos que la usan no cambian. Se puede reactivar después.
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
