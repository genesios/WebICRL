<%@ Page Title="" Language="C#" MasterPageFile="~/SitioICRL.Master" AutoEventWireup="true" CodeBehind="MantenimientoFirma.aspx.cs" Inherits="ICRL.Presentacion.MantenimientoFirma" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenidohead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPaginas" runat="server">
    <div>
        <table class="basetable">
            <tr>
                <th>
                    <asp:Label ID="LabelTituloMantenimiento" runat="server" Text="Actualización de Firma - Sello <-> Usuario"></asp:Label>
                </th>
            </tr>
            <tr>
                <td>
                    <div class="twenty">
                        <asp:Label ID="lblCodEmpleado" runat="server" Text="Código Usuario"></asp:Label><br />
                        <asp:TextBox ID="txtboxCodUsuario" runat="server" Width="104px"></asp:TextBox>
                        <asp:Button ID="ButtonBuscarUsuario" runat="server" OnClick="ButtonBuscarUsuario_Click" Text="Buscar Usuario" />
                    </div>
                    <div class="twenty">
                        <asp:Label ID="LabelIdUsuario" runat="server" Visible="false" Text=""></asp:Label>
                        <asp:Label ID="LabelNombreUsuario" runat="server" Text=""></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="fifty">
                        <asp:Label CssClass="etiquetas" ID="lblMensajeCampo" runat="server" Text="Seleccione la ruta del Archivo Origen"></asp:Label>
                        <asp:FileUpload ID="FileUploadImagen" runat="server" />
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="fifty">
                        <asp:Image ID="ImageFirmaSelloActual" runat="server" BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <asp:Button ID="btnGrabaFirma" runat="server" Text="Grabar Base Datos" OnClick="btnGrabaFirma_Click" Enabled="False"></asp:Button><br />
                        <asp:Label CssClass="etiquetas" ID="lblMensaje" runat="server" Enabled="False"></asp:Label>
                    </div>
                </td>
            </tr>
        </table>
    </div>

</asp:Content>
