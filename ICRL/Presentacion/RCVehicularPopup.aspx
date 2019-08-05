<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RCVehicularPopup.aspx.cs" Inherits="ICRL.Presentacion.RCVehicularPopup" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Estilo/Formatos.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="TextBoxSecuencialPop" runat="server" Visible="false"></asp:TextBox>
            <asp:TextBox ID="TextBoxIdInspeccionPop" runat="server" Visible="false"></asp:TextBox>
        </div>
        <div>
            <table class="tabla03" border="1" style="border-collapse: collapse;">
                <tr>
                    <th colspan="5">
                        <asp:Label ID="LabelRegistroItemsRCV01" runat="server" Text="Items"></asp:Label></th>
                </tr>
                <tr>

                    <td>
                        <asp:Label ID="LabelItemRCV01" runat="server" Text="Item"></asp:Label></td>
                    <td>
                        <asp:Label ID="LabelCompraRCV01" runat="server" Text="Compra"></asp:Label></td>
                    <td>
                        <asp:Label ID="LabelInstalacionRCV01" runat="server" Text="Instalacion"></asp:Label></td>
                    <td>
                        <asp:Label ID="LabelPinturaRCV01" runat="server" Text="Pintura"></asp:Label></td>
                    <td>
                        <asp:Label ID="LabelMecanicoRCV01" runat="server" Text="Mecanico"></asp:Label></td>

                </tr>
                <tr>

                    <td>
                        <asp:DropDownList ID="DropDownListItemRCV01" runat="server" Width="100px"></asp:DropDownList></td>
                    <td>
                        <asp:TextBox ID="TextBoxCompraRCV01" runat="server"></asp:TextBox></td>
                    <td>
                        <asp:CheckBox ID="CheckBoxInstalacionRCV01" runat="server" /></td>
                    <td>
                        <asp:CheckBox ID="CheckBoxPinturaRCV01" runat="server" /></td>
                    <td>
                        <asp:CheckBox ID="CheckBoxMecanicoRCV01" runat="server" /></td>
                </tr>
                <tr>

                    <td>
                        <asp:Label ID="LabelChaperioRCV01" runat="server" Text="Chaperio"></asp:Label></td>
                    <td>
                        <asp:Label ID="LabelReparacionPreviaRCV01" runat="server" Text="Reparacion Previa"></asp:Label></td>
                    <td>
                        <asp:Label ID="LabelObservacionesRCV01" runat="server" Text="Observaciones"></asp:Label></td>
                    <td>
                        <asp:Label ID="LabelIditemRCV01" runat="server" Text="IdItem" Visible="False"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:DropDownList ID="DropDownListChaperioRCV01" runat="server" Width="100px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="DropDownListRepPreviaRCV01" runat="server" Width="100px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxObservacionesRCV01" runat="server"></asp:TextBox></td>
                    <td>
                        <asp:TextBox ID="TextBoxIdItemRCV01" runat="server" Visible="False" Width="59px"></asp:TextBox>
                    </td>

                    <td>
                        <asp:Button ID="ButtonNuevoRCV01" runat="server" Text="Nuevo" OnClick="ButtonNuevoRCV01_Click" />
                        <asp:Button ID="ButtonGrabarRCV01" runat="server" Text="Grabar" Enabled="False" OnClick="ButtonGrabarRCV01_Click" />
                        <asp:Button ID="ButtonBorrarRCV01" runat="server" Text="Borrar" Enabled="False" OnClick="ButtonBorrarRCV01_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <div>
                            <asp:GridView ID="GridViewRCV01Det" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" OnRowDataBound="GridViewRCV01Det_RowDataBound" OnSelectedIndexChanged="GridViewRCV01Det_SelectedIndexChanged" >
                                <AlternatingRowStyle BackColor="White" />
                                <EditRowStyle BackColor="#7C6F57" />
                                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#E3EAEB" />
                                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                <SortedAscendingHeaderStyle BackColor="#246B61" />
                                <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                <SortedDescendingHeaderStyle BackColor="#15524A" />
                                <Columns>
                                    <asp:ButtonField Text="Clic -->" CommandName="Select">
                                        <ItemStyle Width="60px" />
                                    </asp:ButtonField>
                                </Columns>
                            </asp:GridView>
                        </div>
    </form>
</body>
</html>
