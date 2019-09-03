<%@ Page Title="" Language="C#" MasterPageFile="~/SitioICRL.Master" AutoEventWireup="true" CodeBehind="GestionCotizacion.aspx.cs" Inherits="IRCL.Presentacion.GestionCotizacion"  MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="ContentCabecera" ContentPlaceHolderID="Contenidohead" runat="server">
    <style type="text/css">
        .collapsed-row {
            display: none;
            padding: 0px;
            margin: 0px;
        }
    </style>
    <script type="text/javascript">
        function ToggleGridPanel(btn, row) {
            var current = $('#' + row).css('display');
            if (current == 'none') {
                $('#' + row).show();
                $(btn).removeClass('glyphicon-plus')
                $(btn).addClass('glyphicon-minus')
            } else {
                $('#' + row).hide();
                $(btn).removeClass('glyphicon-minus')
                $(btn).addClass('glyphicon-plus')
            }
            return false;
        }
    </script>
</asp:Content>

<asp:Content ID="ContentPagina" ContentPlaceHolderID="ContenidoPaginas" runat="server">
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
                        <asp:Label ID="LabelFechaInsp" runat="server" Text="Fecha de Cotización desde"></asp:Label><br />
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
                    <asp:Button ID="ButtonCreaInspeccion" runat="server" Text="Crear Cotización" OnClick="ButtonCreaCotizacion_Click" />
                    <asp:Label ID="Label4" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </div>
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
                <asp:BoundField DataField="fechaSiniestro" HeaderText="Fecha Siniestro" />
                <asp:TemplateField HeaderText="Estado">
                    <ItemTemplate>
                        <%# Eval("descEstado") %>
                        <%# MyNewRowCot ( Eval("idFlujo") ) %>
                        <asp:GridView ID="gvInspecciones" runat="server" CellPadding="4" ForeColor="#333333"
                            Width="80%"
                            GridLines="None"
                            AutoGenerateColumns="false" OnSelectedIndexChanged="GridViewgvInspecciones_SelectedIndexChanged">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField DataField="idCotizacion" HeaderText="Id.Cot." />
                                <asp:BoundField DataField="tipoCobertura" HeaderText="Cobertura" />
                                <asp:BoundField DataField="secuencialOrden" HeaderText="Orden" />
                                <asp:BoundField DataField="nombreProveedor" HeaderText="Proveedor" />
                                <asp:BoundField DataField="correlativoInspeccion" HeaderText="Inspeccion" />
                                <asp:BoundField DataField="fechaCreacion" HeaderText="Fecha Insp." DataFormatString="{0:dd-MM-yy}" />
                                <asp:BoundField DataField="sumaCosto" HeaderText="Costo USD" />
                                <asp:BoundField DataField="descEstado" HeaderText="Estado" />
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
    </div>
    <div>
        <div>
            <asp:Button runat="server" ID="ButtonOcultoParaPopupCoberturas" Style="display: none" />
            <ajaxToolkit:ModalPopupExtender ID="ModalPopupCoberturas" runat="server"
                PopupControlID="PanelModalPopupCoberturas" TargetControlID="ButtonOcultoParaPopupCoberturas" PopupDragHandleControlID="ModalPopupDragHandleCoberturas"
                RepositionMode="None" X="10" Y="10"
                BackgroundCssClass="modalBackground" DropShadow="True" BehaviorID="ModalPopupCoberturasBehavior" DynamicServicePath="">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="PanelModalPopupCoberturas" runat="server" CssClass="modalPopup">
                <asp:Panel runat="server" ID="ModalPopupDragHandleCoberturas" CssClass="modalPopupHeader">
                    Creación Cotizaciones
                </asp:Panel>
                <div>
                    <table class="basetable">
                        <tr>
                            <td>
                                <div class="thirty">
                                    <asp:Label ID="LabelCoberturas" runat="server" Text="Coberturas"></asp:Label><br />
                                    <asp:DropDownList ID="DropDownListCoberturas" runat="server"></asp:DropDownList><br />
                                </div>
                                <div class="thirty">

                                </div>
                                <div class="thirty">
                                    <asp:Button ID="ButtonCoberturaCrear" runat="server" Text="Crear" OnClick="ButtonCoberturaCrear_Click" />
                                    <asp:Button ID="ButtonCoberturaCancelar" runat="server" Text="Cancelar" OnClick="ButtonCoberturaCancelar_Click" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <%--<asp:Button ID="ButtonCancelPopCobertura" runat="server" Text="Cerrar" />--%>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
