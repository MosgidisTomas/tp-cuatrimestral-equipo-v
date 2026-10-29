<%@ Page Title="Marcas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Marcas.aspx.cs" Inherits="TPCuatrimestralEquipoV.Marcas" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main class="py-4">
        <h1>Marcas</h1>
        <p class="text-muted">Fabricantes de los productos del catálogo (leídas desde la base de datos).</p>

        <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger d-block" Visible="false" />

        <asp:GridView ID="gvMarcas" runat="server" AutoGenerateColumns="false"
            CssClass="table table-striped table-hover" GridLines="None"
            EmptyDataText="No hay marcas cargadas.">
            <Columns>
                <asp:BoundField DataField="Descripcion" HeaderText="Nombre" />
                <asp:BoundField DataField="PaisOrigen" HeaderText="País de origen" />
            </Columns>
        </asp:GridView>
    </main>

</asp:Content>
