<%@ Page Title="" Language="C#" MasterPageFile="~/SitioICRL.Master" AutoEventWireup="true" CodeBehind="GestionInspeccion.aspx.cs" Inherits="ICRL.Presentacion.GestionInspeccion" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>



<asp:Content ID="Content1" ContentPlaceHolderID="Contenidohead" runat="server">
    <style type="text/css">
        .collapsed-row {
            display: none;
            padding: 0px;
            margin: 0px;
        }
    </style>
    <script src="../Scripts/ICRL.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPaginas" runat="server">


    <div>
    </div>

    <div>
        <table class="basetable">
            <tr>
                <th>
                    <asp:Label ID="LabelBusqueda" runat="server" Text="Búsqueda"></asp:Label>
                </th>
            </tr>
            <tr>
                <td>
                    <div class="twentyfive">
                        <asp:Label ID="LabelNroFlujo" runat="server" Text="Nro. de Flujo"></asp:Label><br />
                        <asp:TextBox ID="TextBoxNroFlujo" runat="server"></asp:TextBox>
                    </div>
                    <div class="twentyfive">
                        <asp:Label ID="LabelPlaca" runat="server" Text="Placa"></asp:Label><br />
                        <asp:TextBox ID="TextBoxPlaca" runat="server"></asp:TextBox>
                    </div>
                    <div class="twentyfive">
                        <asp:Label ID="LabelFechaInsp" runat="server" Text="Fecha de Inspección desde"></asp:Label><br />
                        <asp:TextBox ID="TextBoxFechaIni" runat="server" AutoComplete="off"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="TextBoxFechaIni_CalendarExtender" runat="server" BehaviorID="TextBoxFechaIni_CalendarExtender" DaysModeTitleFormat="dd/MM/yyyy" TargetControlID="TextBoxFechaIni" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy"></ajaxToolkit:CalendarExtender>
                        <br />
                        <asp:Label ID="LabelHasta" runat="server" Text="hasta"></asp:Label><br />
                        <asp:TextBox ID="TextBoxFechaFin" runat="server" AutoComplete="off"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="TextBoxFechaFin_CalendarExtender" runat="server" BehaviorID="TextBoxFechaFin_CalendarExtender" DaysModeTitleFormat="dd/MM/yyyy" TargetControlID="TextBoxFechaFin" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy"></ajaxToolkit:CalendarExtender>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="ButtonBuscarFlujo" runat="server" Text="Buscar" OnClick="ButtonBuscarFlujo_Click" />
                    <asp:Button ID="ButtonCreaInspeccion" runat="server" Text="Crear Inspección" OnClick="ButtonCreaInspeccion_Click" />
                    <asp:Label ID="Label4" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
        <table class="basetable" style="visibility: hidden; display: none">
            <tr>
                <th>
                    <asp:Label ID="LabelDatosInspector" runat="server" Text="Datos Inspector"></asp:Label>
                </th>
            </tr>
            <tr>
                <td>
                    <div class="twentyfive">
                        <asp:Label ID="LabelInspector" runat="server" Text="Inspector"></asp:Label><br />
                        <asp:TextBox ID="TextBoxInspector" runat="server" Enabled="False"></asp:TextBox>
                    </div>
                    <div class="twentyfive">
                        <asp:Label ID="LabelSucAtencion" runat="server" Text="Sucursal atención"></asp:Label><br />
                        <asp:TextBox ID="TextBoxSucAtencion" runat="server" Enabled="False"></asp:TextBox>
                    </div>
                    <div class="twentyfive">
                        <asp:Label ID="LabelEstado" runat="server" Text="Estado"></asp:Label><br />
                        <asp:TextBox ID="TextBoxEstado" runat="server" Enabled="False"></asp:TextBox>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <table class="basetable">
        <tr>
            <th>
                <asp:Label ID="LabelInspecciones" runat="server" Text="Inspecciones"></asp:Label>
            </th>
        </tr>
        <tr>
            <td>
                <div>
                    <asp:TextBox ID="TextBoxFlujo" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                    <asp:Button ID="ButtonFlujoInspeccion" runat="server" Text="Flujo -&gt; Inspección" OnClick="ButtonFlujoInspeccion_Click" Enabled="False" Visible="False" />
                    
                </div>
            </td>
        </tr>
    </table>
    <div>
        <asp:GridView ID="GridViewMaster" runat="server" CellPadding="4" ForeColor="#333333"
            GridLines="None" Width="100%" AutoGenerateColumns="false" OnRowDataBound="GridViewMaster_RowDataBound" AllowPaging="True" OnPageIndexChanging="GridViewMaster_PageIndexChanging" PageSize="8">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <button class="btn btn-default glyphicon glyphicon-plus" onclick="return ToggleGridPanel(this, 'tr<%# Eval("idFlujo") %>')" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:ButtonField Text="Clic ->" CommandName="Select" ItemStyle-Width="60">
                    <ItemStyle Width="60px"></ItemStyle>
                </asp:ButtonField>
                <asp:BoundField DataField="idFlujo" HeaderText="IdFlujo" />
                <asp:BoundField DataField="flujoOnBase" HeaderText="Flujo" />
                <asp:BoundField DataField="nombreAsegurado" HeaderText="Asegurado" />
                <asp:BoundField DataField="numeroPoliza" HeaderText="Poliza" />
                <asp:BoundField DataField="placaVehiculo" HeaderText="Placa" />
                <asp:TemplateField HeaderText="Doc.Identidad">
                    <ItemTemplate>
                        <%# Eval("docIdAsegurado") %>
                        <%# MyNewRow ( Eval("idFlujo") ) %>
                        <asp:GridView ID="gvInspecciones" runat="server" CellPadding="4" ForeColor="#333333"
                            Width="80%"
                            GridLines="None"
                            AutoGenerateColumns="false" OnSelectedIndexChanged="GridViewgvInspecciones_SelectedIndexChanged">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField DataField="idInspeccion" HeaderText="Inspeccion" />
                                <asp:BoundField DataField="tipoCobertura" HeaderText="Cobertura" />
                                <asp:BoundField DataField="fechaCreacion" HeaderText="Fecha Insp." DataFormatString="{0:dd-MM-yy}" />
                                <asp:BoundField DataField="sucursalAtencion" HeaderText="Sucursal" />
                                <asp:BoundField DataField="idInspector" HeaderText="Inspector" />
                                <asp:BoundField DataField="nombreContacto" HeaderText="Tercero" />
                                <asp:ButtonField Text="Ver/Modif" CommandName="Select" ItemStyle-Width="50" ItemStyle-ForeColor="Blue" />
                            </Columns>
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
                        </asp:GridView>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EditRowStyle BackColor="#7C6F57" />
            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <PagerSettings NextPageText="Siguiente" PreviousPageText="Previo" />
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#E3EAEB" />
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#222222" />
            <SortedAscendingCellStyle BackColor="#F8FAFA" />
            <SortedAscendingHeaderStyle BackColor="#246B61" />
            <SortedDescendingCellStyle BackColor="#D4DFE1" />
            <SortedDescendingHeaderStyle BackColor="#15524A" />
            <PagerStyle BackColor="darkseagreen"
                Height="30px"
                VerticalAlign="Bottom"
                HorizontalAlign="Center" Font-Size="Large" ForeColor="DarkBlue" />
        </asp:GridView>
        <div>
            <asp:Button ID="ButtonMaster" runat="server" Text="LlenaGrid" OnClick="ButtonMaster_Click" Enabled="False" Visible="False" />
        </div>
    </div>
    <div>
        <asp:ImageButton ID="ImgButtonExportExcel" runat="server" ImageUrl="~/img/ico_excel.jpg" OnClick="ImgButtonExportExcel_Click" />
    </div>
    <div>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" BackColor="" ClientIDMode="AutoID" HighlightBackgroundColor="" InternalBorderColor="204, 204, 204" InternalBorderStyle="Solid" InternalBorderWidth="1px" LinkActiveColor="" LinkActiveHoverColor="" LinkDisabledColor="" PrimaryButtonBackgroundColor="" PrimaryButtonForegroundColor="" PrimaryButtonHoverBackgroundColor="" PrimaryButtonHoverForegroundColor="" SecondaryButtonBackgroundColor="" SecondaryButtonForegroundColor="" SecondaryButtonHoverBackgroundColor="" SecondaryButtonHoverForegroundColor="" SplitterBackColor="" ToolbarDividerColor="" ToolbarForegroundColor="" ToolbarForegroundDisabledColor="" ToolbarHoverBackgroundColor="" ToolbarHoverForegroundColor="" ToolBarItemBorderColor="" ToolBarItemBorderStyle="Solid" ToolBarItemBorderWidth="1px" ToolBarItemHoverBackColor="" ToolBarItemPressedBorderColor="51, 102, 153" ToolBarItemPressedBorderStyle="Solid" ToolBarItemPressedBorderWidth="1px" ToolBarItemPressedHoverBackColor="153, 187, 226" Width="923px" Visible="false">
            <LocalReport ReportPath="Reportes\Report3.rdlc">
            </LocalReport>
        </rsweb:ReportViewer>
    </div>
    <asp:Button runat="server" ID="ButtonOcultoparaPopupSiNo" Style="display: none" />
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupSiNo" runat="server"
        PopupControlID="PanelModalPopupSiNo" TargetControlID="ButtonOcultoparaPopupSiNo" PopupDragHandleControlID="ModalPopupSiNoDragHandle"
        RepositionMode="RepositionOnWindowScroll"
        BackgroundCssClass="modalBackground" DropShadow="True" BehaviorID="ModalPopupSiNoBehavior" DynamicServicePath="">
    </ajaxToolkit:ModalPopupExtender>
    <asp:Panel ID="PanelModalPopupSiNo" runat="server" CssClass="modalPopup" Style="width: 100%; height: auto; padding: 10px; background-color: darkcyan;">
        <asp:Panel runat="server" ID="ModalPopupSiNoDragHandle"
            Style="cursor: move; background-color: #DDDDDD; border: solid 1px Gray; color: Black; text-align: center;">
            Ventana Popup de Confirmación
        </asp:Panel>
        <div>
            <table border="1" style="border-collapse: collapse;">
                <tr>
                    <th>
                        <asp:TextBox ID="TextBoxPopupSiNo" runat="server" TextMode="MultiLine" Width="300px"></asp:TextBox>
                    </th>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="ButtonPopupSiNoSI" runat="server" Text="SI" Enabled="True" OnClick="ButtonPopupSiNoSI_Click" />
                        <asp:Button ID="ButtonPopupSiNoNO" runat="server" Text="NO" Enabled="True" OnClick="ButtonPopupSiNoNO_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
</asp:Content>
