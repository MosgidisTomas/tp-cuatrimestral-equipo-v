<%@ Page Title="Categorías" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Categorias.aspx.cs" Inherits="TPCuatrimestralEquipoV.Categorias" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="encabezado-pagina">
        <div>
            <h1>Categorías</h1>
            <p>Tipos de componente con los que se clasifican los productos.</p>
        </div>
    </div>

    <div class="row g-4">
        <div class="col-lg-8">
            <div class="tabla-contenedor">
                <asp:GridView ID="gvCategorias" runat="server" AutoGenerateColumns="false"
                    CssClass="table table-hover tabla" GridLines="None"
                    ShowHeaderWhenEmpty="true" EmptyDataText="No hay categorías para mostrar.">
                    <Columns>
                        <asp:BoundField DataField="Descripcion" HeaderText="Nombre" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <div class="col-lg-4">
            <div class="tarjeta">
                <h2 class="titulo-seccion">Nueva categoría</h2>
                <div class="mb-3">
                    <asp:Label runat="server" AssociatedControlID="txtDescripcion" CssClass="form-label" Text="Nombre *" />
                    <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" MaxLength="50" placeholder="Ej.: Gabinetes" />
                </div>
                <p class="texto-ayuda">No puede haber dos categorías con el mismo nombre.</p>
                <div class="acciones mt-0">
                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-outline-secondary" />
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
