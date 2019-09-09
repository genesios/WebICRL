<%@ Page Title="" Language="C#" MasterPageFile="~/SitioICRL.Master" AutoEventWireup="true" CodeBehind="CotizacionRP.aspx.cs" Inherits="ICRL.Presentacion.CotizacionRP" MaintainScrollPositionOnPostback="true" %>

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
        <div>
            <ajaxToolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server"
                Collapsed="True" CollapsedSize="0"
                ExpandedText="Ocultar" TargetControlID="PanelDatosGen" TextLabelID="textLabel"
                ImageControlID="Image1" ExpandedImage="~/img/collapse.jpg" CollapsedImage="~/img/expand.jpg"
                CollapseControlID="PanelColapseDatosGen" ExpandControlID="PanelColapseDatosGen" CollapsedText="Mostrar" />
            <div>

                <table class="basetable">
                    <tr>
                        <th>
                            <asp:Label ID="LabelTituloCotizacion" runat="server" Text="Cotización"></asp:Label>
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <div class="twentyfive">
                                <asp:Label ID="LabelNroFlujo" runat="server" Text="Nro. de Flujo"></asp:Label><br />
                                <asp:TextBox ID="TextBoxNroFlujo" runat="server" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="twentyfive">
                                <asp:Label ID="LabelNroReclamo" runat="server" Text="Nro. de Reclamo"></asp:Label><br />
                                <asp:TextBox ID="TextBoxNroReclamo" runat="server" Enabled="false"></asp:TextBox>
                                <asp:TextBox ID="TextBoxIdFlujo" runat="server" Enabled="false" Visible="False"></asp:TextBox>
                                <asp:TextBox ID="TextBoxNroCotizacion" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                            </div>
                            <div class="twentyfive">
                                <asp:Label ID="LabelNroCotizacion" runat="server" Text="Nro. de Cotización"></asp:Label><br />
                                <asp:TextBox ID="TextBoxCorrelativo" runat="server" Enabled="False"></asp:TextBox>
                            </div>
                            <div class="twentyfive">
                                <asp:Label ID="LabelTipoCambio" runat="server" Text="Tipo de Cambio"></asp:Label><br />
                                <asp:TextBox ID="TextBoxTipoCambio" runat="server" Text="6.96"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="fifty">
                                <asp:Button ID="ButtonActualizaDesdeOnBase" runat="server" Text="Actualizar desde OnBase"  OnClientClick="return ConfirmarActualizarOnBase();" />
                                <asp:Label ID="LabelMsjGeneral" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="fifty">
                                <asp:Button ID="ButtonFinalizarCotizacion" runat="server" Text="Finalizar Flujo Cotización"  OnClientClick="return ConfirmarFinalizarFlujoOnBase();" OnClick="ButtonFinalizarCotizacion_Click" />
                            </div>
                        </td>
                    </tr>
                </table>

                <asp:Panel runat="server" ID="PanelColapseDatosGen">
                    <div class="collapseBar">
                        <span><strong style="text-transform: uppercase">
                            <asp:Label runat="server" ID="textLabel" />
                            Datos Generales</strong></span>
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/img/collapse.jpg" />
                    </div>
                </asp:Panel>
                <asp:Panel ID="PanelDatosGen" runat="server" CssClass="PanelDatosGen">
                    <table class="basetable">
                        <tr>
                            <th>
                                <asp:Label ID="LabelDatosGenerales" runat="server" Text="Datos Generales"></asp:Label>
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <div class="fifty">
                                    <asp:Label ID="LabelSucAtencion" runat="server" Text="Sucursal de Atención"></asp:Label><br />
                                    <asp:TextBox ID="TextBoxSucAtencion" runat="server"></asp:TextBox>
                                </div>
                                <div class="fifty">
                                    <asp:Label ID="LabelDireccionInspeccion" runat="server" Text="Dirección de Inspección"></asp:Label><br />
                                    <asp:TextBox ID="TextBoxDirecInspeccion" runat="server"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                    </table>

                    <table class="basetable">
                        <tr>
                            <th>
                                <asp:Label ID="LabelDatosSiniestro" runat="server" Text="Datos Siniestro"></asp:Label>
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <div class="fifty">
                                    <asp:Label ID="LabelCausaSiniestro" runat="server" Text="Causa Siniestro"></asp:Label><br />
                                    <asp:TextBox ID="TextBoxCausaSiniestro" runat="server"></asp:TextBox><br />
                                    <asp:Label ID="LabelDescripSiniestro" runat="server" Text="Descripción del Siniestro"></asp:Label><br />
                                    <asp:TextBox ID="TextBoxDescripSiniestro" runat="server" ReadOnly="False"></asp:TextBox>
                                </div>
                                <div class="fifty">
                                    <asp:Label ID="LabelObservacionesInspeccion" runat="server" Text="Observaciones Inspección"></asp:Label><br />
                                    <asp:TextBox ID="TextBoxObservacionesInspec" runat="server" ReadOnly="False"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                    </table>

                    <table class="basetable">
                        <tr>
                            <th>
                                <asp:Label ID="LabelDatosCliente" runat="server" Text="Datos Cliente"></asp:Label>
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <div class="fifty">
                                    <asp:Label ID="LabelNombreAsegurado" runat="server" Text="Nombre Asegurado"></asp:Label><br />
                                    <asp:TextBox ID="TextBoxNombreAsegurado" runat="server"></asp:TextBox>
                                </div>
                                <div class="fifty">
                                    <asp:Label ID="LabelTelefonoAsegurado" runat="server" Text="Teléfono Asegurado"></asp:Label><br />
                                    <asp:TextBox ID="TextBoxTelefonoAsegurado" runat="server"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                    </table>

                    <table class="basetable">
                        <tr>
                            <th>
                                <asp:Label ID="LabelDatosContactoInspec" runat="server" Text="Datos Contacto Inspección"></asp:Label>
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <div class="fifty">
                                    <asp:Label ID="LabelNombreInspector" runat="server" Text="Nombre del Inspector"></asp:Label><br />
                                    <asp:TextBox ID="TextBoxNombreInspector" runat="server"></asp:TextBox><br />
                                    <asp:Label ID="LabelCorreoInspector" runat="server" Text="Correo Electrónico"></asp:Label><br />
                                    <asp:TextBox ID="TextBoxCorreoInspector" runat="server"></asp:TextBox><br />
                                    <asp:Label ID="LabelNombreContacto" runat="server" Text="Nombre del Contacto"></asp:Label><br />
                                    <asp:TextBox ID="TextBoxNombreContacto" runat="server"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                    </table>

                    <table class="basetable">
                        <tr>
                            <th>
                                <asp:Label ID="LabelVehiculoAsegurado" runat="server" Text="Datos Vehículo Asegurado"></asp:Label>
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <div class="fifty">
                                    <asp:Label ID="LabelMarca" runat="server" Text="Marca:"></asp:Label><br />
                                    <asp:TextBox ID="TextBoxMarca" runat="server"></asp:TextBox><br />
                                    <asp:Label ID="LabelModelo" runat="server" Text="Modelo:"></asp:Label><br />
                                    <asp:TextBox ID="TextBoxModelo" runat="server"></asp:TextBox><br />
                                    <asp:Label ID="LabelAnio" runat="server" Text="Año:"></asp:Label><br />
                                    <asp:TextBox ID="TextBoxAnio" runat="server" Enabled="False"></asp:TextBox><br />
                                    <asp:Label ID="LabelPlaca" runat="server" Text="Placa:"></asp:Label><br />
                                    <asp:TextBox ID="TextBoxPlaca" runat="server"></asp:TextBox>
                                </div>
                                <div class="fifty">
                                    <asp:Label ID="LabelColor" runat="server" Text="Color:"></asp:Label><br />
                                    <asp:TextBox ID="TextBoxColor" runat="server"></asp:TextBox><br />
                                    <asp:Label ID="LabelKilometraje" runat="server" Text="Valor:"></asp:Label><br />
                                    <asp:TextBox ID="TextBoxValorAsegurado" runat="server" Enabled="False"></asp:TextBox><br />
                                    <asp:Label ID="LabelNroChasis" runat="server" Text="Número Chasis:"></asp:Label><br />
                                    <asp:TextBox ID="TextBoxNroChasis" runat="server"></asp:TextBox><br />
                                    <asp:TextBox ID="TextBoxKilometraje" runat="server" Visible="False"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                    </table>

                    <table class="basetable">
                        <tr>
                            <th>
                                <asp:Label ID="LabelDatosTaller" runat="server" Text="Datos Taller"></asp:Label>
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <div class="fifty">
                                    <asp:Label ID="LabelTipoTaller" runat="server" Text="Tipo Taller"></asp:Label><br />
                                    <asp:DropDownList ID="DropDownListTipoTallerCoti" runat="server" Enabled="true"></asp:DropDownList>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div>
                        <asp:Label ID="LabelMensaje" runat="server" Text=""></asp:Label>
                    </div>
                </asp:Panel>
            </div>
        </div>
        <br />
        <div>
            <table class="basetable alt2">
                <tr>
                    <th>
                        <asp:Label ID="LabelReparaciones" runat="server" Text="Reparaciones"></asp:Label>
                        <asp:Label ID="LabelMsjReparaciones" runat="server" Text=""></asp:Label>
                    </th>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="ButtonRepaAgregarItem" runat="server" Text="Agregar" OnClick="ButtonRepaAgregarItem_Click" CssClass="alt2" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="GridViewReparaciones" runat="server" CellPadding="4" GridLines="None" AutoGenerateColumns="false"
                            OnSelectedIndexChanged="GridViewReparaciones_SelectedIndexChanged" OnRowDeleting="GridViewReparaciones_RowDeleting" Width="100%">
                            <Columns>
                                <asp:ButtonField Text="Borrar" CommandName="Delete" ItemStyle-Width="50" />
                                <asp:BoundField DataField="id_item" HeaderText="Id" />
                                <asp:BoundField DataField="item_descripcion" HeaderText="Item" />
                                <asp:BoundField DataField="chaperio" HeaderText="Chaperío" />
                                <asp:BoundField DataField="reparacion_previa" HeaderText="Reparación Previa" />
                                <asp:TemplateField HeaderText="Mec.">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" Enabled="false" Checked='<%# Eval("mecanico") %>'></asp:CheckBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="id_moneda" HeaderText="Moneda" />
                                <asp:BoundField DataField="precio_cotizado" HeaderText="Precio Cotizado" DataFormatString="{0:N2}" />
                                <asp:BoundField DataField="id_tipo_descuento" HeaderText="Descuento F/P" />
                                <asp:BoundField DataField="descuento" HeaderText="Monto Descuento" />
                                <asp:BoundField DataField="precio_final" HeaderText="Precio Final" DataFormatString="{0:N2}" />
                                <asp:BoundField DataField="proveedor" HeaderText="Proveedor" />
                                <asp:ButtonField Text="Editar" CommandName="Select" ItemStyle-Width="50" />
                            </Columns>
                            <AlternatingRowStyle BackColor="White" />
                            <RowStyle CssClass="grid_row alt2" />
                            <SelectedRowStyle CssClass="grid_selected alt2" />
                            <EditRowStyle CssClass="grid_edit alt2" />
                            <HeaderStyle CssClass="grid_header alt2" />
                            <FooterStyle CssClass="grid_footer alt2" />
                            <PagerStyle CssClass="grid_pager alt2" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table style="width: 100%; font-size: inherit">
                            <tr>
                                <td style="text-align: left">
                                    <asp:Button ID="ButtonRepaGenerarResumen" runat="server" Text="Generar Resumen" OnClick="ButtonRepaGenerarResumen_Click" CssClass="alt2" />
                                </td>
                                <td style="text-align: right">
                                    <asp:Button ID="ButtonRepaGenerarOrdenes" runat="server" Text="Generar Órdenes" OnClick="ButtonRepaGenerarOrdenes_Click" CssClass="alt2"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="GridViewSumaReparaciones" runat="server" CellPadding="4" GridLines="None" AutoGenerateColumns="false"
                            OnSelectedIndexChanged="GridViewSumaReparaciones_SelectedIndexChanged" Width="100%">
                            <Columns>
                                <asp:BoundField DataField="proveedor" HeaderText="Proveedor" />
                                <asp:BoundField DataField="monto_orden" HeaderText="Monto Orden" DataFormatString="{0:N2}" />
                                <asp:BoundField DataField="id_tipo_descuento_orden" HeaderText="Descuento F/P" />
                                <asp:BoundField DataField="descuento_proveedor" HeaderText="Descuento" DataFormatString="{0:N2}" />
                                <asp:BoundField DataField="deducible" HeaderText="FRA/COA" DataFormatString="{0:N2}" />
                                <asp:BoundField DataField="monto_final" HeaderText="Monto Final" DataFormatString="{0:N2}" />
                                <asp:ButtonField Text="Editar" CommandName="Select" ItemStyle-Width="50" />
                            </Columns>
                            <AlternatingRowStyle BackColor="White" />
                            <RowStyle CssClass="grid_row alt2" />
                            <SelectedRowStyle CssClass="grid_selected alt2" />
                            <EditRowStyle CssClass="grid_edit alt2" />
                            <HeaderStyle CssClass="grid_header alt2" />
                            <FooterStyle CssClass="grid_footer alt2" />
                            <PagerStyle CssClass="grid_pager alt2" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <table class="basetable alt1">
                <tr>
                    <th>
                        <asp:Label ID="LabelRepuestos" runat="server" Text="Repuestos"></asp:Label>
                        <asp:Label ID="LabelMsjRepuestos" runat="server" Text=""></asp:Label>
                    </th>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="ButtonRepuAgregarItem" runat="server" Text="Agregar" OnClick="ButtonRepuAgregarItem_Click" CssClass="alt1" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="GridViewRepuestos" runat="server" CellPadding="4" GridLines="None" AutoGenerateColumns="false"
                            OnSelectedIndexChanged="GridViewRepuestos_SelectedIndexChanged" OnRowDeleting="GridViewRepuestos_RowDeleting" Width="100%">
                            <Columns>
                                <asp:ButtonField Text="Borrar" CommandName="Delete" ItemStyle-Width="50" />
                                <asp:BoundField DataField="id_item" HeaderText="Id" />
                                <asp:BoundField DataField="item_descripcion" HeaderText="Item" />
                                <asp:TemplateField HeaderText="Pint.">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" Enabled="false" Checked='<%# Eval("pintura") %>'></asp:CheckBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Inst.">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" Enabled="false" Checked='<%# Eval("instalacion") %>'></asp:CheckBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="id_moneda" HeaderText="Moneda" />
                                <asp:BoundField DataField="precio_cotizado" HeaderText="Precio Cotizado" DataFormatString="{0:N2}" />
                                <asp:BoundField DataField="id_tipo_descuento" HeaderText="Descuento F/P" />
                                <asp:BoundField DataField="descuento" HeaderText="Monto Descuento" />
                                <asp:BoundField DataField="precio_final" HeaderText="Precio Final" DataFormatString="{0:N2}" />
                                <asp:BoundField DataField="proveedor" HeaderText="Proveedor" />
                                <asp:ButtonField Text="Editar" CommandName="Select" ItemStyle-Width="50" />
                            </Columns>
                            <AlternatingRowStyle BackColor="White" />
                            <RowStyle CssClass="grid_row alt1" />
                            <SelectedRowStyle CssClass="grid_selected alt1" />
                            <EditRowStyle CssClass="grid_edit alt1" />
                            <HeaderStyle CssClass="grid_header alt1" />
                            <FooterStyle CssClass="grid_footer alt1" />
                            <PagerStyle CssClass="grid_pager alt1" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table style="width: 100%; font-size: inherit">
                            <tr>
                                <td style="text-align: left">
                                    <asp:Button ID="ButtonRepuGenerarResumen" runat="server" Text="Generar Resumen" OnClick="ButtonRepuGenerarResumen_Click" CssClass="alt1"></asp:Button>
                                </td>
                                <td style="text-align: right">
                                    <asp:Button ID="ButtonRepuGenerarOrdenes" runat="server" Text="Generar Ordenes" OnClick="ButtonRepuGenerarOrdenes_Click" CssClass="alt1"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="GridViewSumaRepuestos" runat="server" CellPadding="4" GridLines="None" AutoGenerateColumns="false"
                            OnSelectedIndexChanged="GridViewSumaRepuestos_SelectedIndexChanged" Width="100%">
                            <Columns>
                                <asp:BoundField DataField="proveedor" HeaderText="Proveedor" />
                                <asp:BoundField DataField="monto_orden" HeaderText="Monto Orden" DataFormatString="{0:N2}" />
                                <asp:BoundField DataField="id_tipo_descuento_orden" HeaderText="Descuento F/P" />
                                <asp:BoundField DataField="descuento_proveedor" HeaderText="Descuento" DataFormatString="{0:N2}" />
                                <asp:BoundField DataField="deducible" HeaderText="FRA/COA" DataFormatString="{0:N2}" />
                                <asp:BoundField DataField="monto_final" HeaderText="Monto Final" DataFormatString="{0:N2}" />
                                <asp:ButtonField Text="Editar" CommandName="Select" ItemStyle-Width="50" />
                            </Columns>
                            <AlternatingRowStyle BackColor="White" />
                            <RowStyle CssClass="grid_row alt1" />
                            <SelectedRowStyle CssClass="grid_selected alt1" />
                            <EditRowStyle CssClass="grid_edit alt1" />
                            <HeaderStyle CssClass="grid_header alt1" />
                            <FooterStyle CssClass="grid_footer alt1" />
                            <PagerStyle CssClass="grid_pager alt1" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="ButtonRecepcionRepu" runat="server" Text="Recepción Repuestos" CssClass="alt1" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="GridViewRecepRepuestos" runat="server" CellPadding="4" GridLines="None" AutoGenerateColumns="false"
                            OnSelectedIndexChanged="GridViewRecepRepuestos_SelectedIndexChanged" Width="100%">
                            <Columns>
                                <asp:BoundField DataField="id_item" HeaderText="Id" />
                                <asp:BoundField DataField="item_descripcion" HeaderText="Item" />
                                <asp:TemplateField HeaderText="Recibido">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" Enabled="false" Checked='<%# Eval("recepcion") %>'></asp:CheckBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="dias_entrega" HeaderText="Días de Entrega" />
                                <asp:ButtonField Text="Editar" CommandName="Select" ItemStyle-Width="50" />
                            </Columns>
                            <AlternatingRowStyle BackColor="White" />
                            <RowStyle CssClass="grid_row alt1" />
                            <SelectedRowStyle CssClass="grid_selected alt1" />
                            <EditRowStyle CssClass="grid_edit alt1" />
                            <HeaderStyle CssClass="grid_header alt1" />
                            <FooterStyle CssClass="grid_footer alt1" />
                            <PagerStyle CssClass="grid_pager alt1" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>

        <div>
            <table class="basetable alt3">
                <tr>
                    <th>Órdenes Generadas</th>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="GridViewOrdenes" runat="server" CellPadding="4" GridLines="None" AutoGenerateColumns="false"
                            DataKeyNames="numero_orden" OnRowCommand="GridViewOrdenes_RowCommand" OnRowDataBound="GridViewOrdenes_RowDataBound" Width="100%">
                            <Columns>
                                <asp:BoundField DataField="numero_orden" HeaderText="Número Orden" />
                                <asp:BoundField DataField="id_estado" HeaderText="Estado" />
                                <asp:BoundField DataField="proveedor" HeaderText="Proveedor" />
                                <asp:BoundField DataField="moneda" HeaderText="Moneda" />
                                <asp:BoundField DataField="monto_orden" HeaderText="Monto Orden" DataFormatString="{0:N2}" />
                                <asp:BoundField DataField="id_tipo_descuento_orden" HeaderText="Descuento F/P" />
                                <asp:BoundField DataField="descuento_proveedor" HeaderText="Descuento" DataFormatString="{0:N2}" />
                                <asp:BoundField DataField="deducible" HeaderText="FRA/COA" DataFormatString="{0:N2}" />
                                <asp:BoundField DataField="monto_final" HeaderText="Monto Final" DataFormatString="{0:N2}" />
                                <asp:ButtonField CommandName="Imprimir" ButtonType="Button" HeaderText="Opción" Text="Imp" />
                                <asp:ButtonField CommandName="Ver" ButtonType="Button" HeaderText="Opción" Text="Ver" />
                                <asp:ButtonField CommandName="SubirOnBase" ButtonType="Button" HeaderText="Opción" Text="On Base" />
                            </Columns>
                            <AlternatingRowStyle BackColor="White" />
                            <RowStyle CssClass="grid_row alt3" />
                            <SelectedRowStyle CssClass="grid_selected alt3" />
                            <EditRowStyle CssClass="grid_edit alt3" />
                            <HeaderStyle CssClass="grid_header alt3" />
                            <FooterStyle CssClass="grid_footer alt3" />
                            <PagerStyle CssClass="grid_pager alt3" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>

        <div>
            <asp:Button runat="server" ID="ButtonOcultoParaPopupReparaciones" Style="display: none" CssClass="alt2" />

            <ajaxToolkit:ModalPopupExtender runat="server" ID="ModalPopupReparaciones" BehaviorID="ModalPopupReparacionesBehavior"
                TargetControlID="ButtonOcultoParaPopupReparaciones" PopupControlID="PanelModalPopupABMReparaciones"
                BackgroundCssClass="modalBackground" DropShadow="True" PopupDragHandleControlID="ModalPopupDragHandleReparaciones"
                RepositionMode="RepositionOnWindowScroll">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="PanelModalPopupABMReparaciones" runat="server" CssClass="modalPopup">
                <asp:Panel runat="server" ID="ModalPopupDragHandleReparaciones" CssClass="modalPopupHeader">
                    ABM Reparaciones
                </asp:Panel>
                <div>
                    <table class="basetable alt2">
                        <tr>
                            <td>
                                <strong>
                                    <asp:Label ID="LabelRepaRegistroItems" runat="server" Text="Items"></asp:Label></strong>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="twentyfive">
                                    <asp:Label ID="LabelRepaItem" runat="server" Text="Item"></asp:Label><br />
                                    <asp:DropDownList ID="DropDownListRepaItem" runat="server"></asp:DropDownList>
                                    <asp:TextBox ID="TextBoxRepaItem" runat="server" Visible="false"></asp:TextBox>
                                </div>
                                <div class="twentyfive">
                                    <asp:Label ID="LabelRepaChaperio" runat="server" Text="Chaperío"></asp:Label><br />
                                    <asp:DropDownList ID="DropDownListRepaChaperio" runat="server"></asp:DropDownList>
                                </div>
                                <div class="twentyfive">
                                    <asp:Label ID="LabelRepaReparacionPrevia" runat="server" Text="Reparacion Previa"></asp:Label><br />
                                    <asp:DropDownList ID="DropDownListRepaRepPrevia" runat="server"></asp:DropDownList>
                                </div>
                                <div class="twentyfive">
                                    <asp:CheckBox ID="CheckBoxRepaMecanico" runat="server" />
                                    <asp:Label ID="LabelRepaMecanico" runat="server" Text="Mecánico"></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="twenty">
                                    <asp:Label ID="LabelRepaMoneda" runat="server" Text="Moneda"></asp:Label><br />
                                    <asp:DropDownList ID="DropDownListRepaMoneda" runat="server"></asp:DropDownList>
                                </div>
                                <div class="twenty">
                                    <asp:Label ID="LabelPrecioCotizadoRepa" runat="server" Text="Precio Cotizado"></asp:Label><br />
                                    <asp:TextBox ID="TextBoxRepaPrecioCotizado" runat="server" Text="0"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="ValidadorRepaMonto" runat="server" ErrorMessage="*" ControlToValidate="TextBoxRepaPrecioCotizado" CssClass="errormessage"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegExValidatorRepaMonto"
                                        runat="server" ErrorMessage="Verifique el formato del  monto"
                                        ControlToValidate="TextBoxRepaPrecioCotizado" CssClass="errormessage"
                                        ValidationExpression="^[0-9]*\.?[0-9]*$">
                                    </asp:RegularExpressionValidator>
                                </div>
                                <div class="twenty">
                                    <asp:Label ID="LabelRepaTipoDesc" runat="server" Text="Tipo Descuento"></asp:Label><br />
                                    <asp:DropDownList ID="DropDownListRepaTipoDesc" runat="server"></asp:DropDownList>
                                </div>
                                <div class="twenty">
                                    <asp:Label ID="LabelRepaMontoDesc" runat="server" Text="Monto Descuento"></asp:Label><br />
                                    <asp:TextBox ID="TextBoxRepaMontoDesc" runat="server" Text="0"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="ValidadorRepaMontoDesc" runat="server" ErrorMessage="*" ControlToValidate="TextBoxRepaMontoDesc" CssClass="errormessage"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegExValidatorRepaMontoDesc"
                                        runat="server" ErrorMessage="Verifique el formato del  monto"
                                        ControlToValidate="TextBoxRepaMontoDesc" CssClass="errormessage"
                                        ValidationExpression="^[0-9]*\.?[0-9]*$">
                                    </asp:RegularExpressionValidator>
                                </div>
                                <div class="twenty">
                                    <asp:Label ID="LabelRepaPrecioFinal" runat="server" Text="Precio Final"></asp:Label><br />
                                    <asp:TextBox ID="TextBoxRepaPrecioFinal" runat="server" Text="0" Enabled="false"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>

                                <div class="fifty">
                                    <asp:Label ID="LabelRepaProveedor" runat="server" Text="Proveedores"></asp:Label><br />
                                    <asp:DropDownList ID="DropDownListRepaProveedor" runat="server"></asp:DropDownList>
                                </div>
                                <div class="fifty">
                                    <asp:Label ID="LabelRepaIdItem" runat="server" Text="IdItem" Visible="False"></asp:Label><br />
                                    <asp:TextBox ID="TextBoxRepaIdItem" runat="server" Enable="false" Visible="False"></asp:TextBox>
                                    <asp:TextBox ID="TextBoxRepaFlagEd" runat="server" Visible="false"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="ButtonRepaGrabar" runat="server" Text="Grabar" Enabled="False" OnClick="ButtonRepaGrabar_Click" />
                                <asp:Button ID="ButtonRepaCancelar" runat="server" Text="Cancelar" Enabled="False" OnClick="ButtonRepaCancelar_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:Button ID="ButtonCancelPopReparaciones" runat="server" Text="Cerrar" OnClick="ButtonCancelPopReparaciones_Click" />
            </asp:Panel>
        </div>

        <div>
            <asp:Button runat="server" ID="ButtonOcultoParaPopupRepuestos" Style="display: none" CssClass="alt2" />

            <ajaxToolkit:ModalPopupExtender runat="server" ID="ModalPopupRepuestos" BehaviorID="ModalPopupRepuestosBehavior"
                TargetControlID="ButtonOcultoParaPopupRepuestos" PopupControlID="PanelModalPopupABMRepuestos"
                BackgroundCssClass="modalBackground" DropShadow="True" PopupDragHandleControlID="ModalPopupDragHandleRepuestos"
                RepositionMode="RepositionOnWindowScroll">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="PanelModalPopupABMRepuestos" runat="server" CssClass="modalPopup">
                <asp:Panel runat="server" ID="ModalPopupDragHandleRepuestos" CssClass="modalPopupHeader">
                    ABM Repuestos
                </asp:Panel>
                <div>
                    <table class="basetable alt2">
                        <tr>
                            <td>
                                <strong>
                                    <asp:Label ID="LabelRepuRegistroItems" runat="server" Text="Items"></asp:Label></strong>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="twentyfive">
                                    <asp:Label ID="LabelRepuItem" runat="server" Text="Item"></asp:Label><br />
                                    <asp:DropDownList ID="DropDownListRepuItem" runat="server"></asp:DropDownList>
                                    <asp:TextBox ID="TextBoxRepuItem" runat="server" Visible="false"></asp:TextBox>
                                </div>
                                <div class="twentyfive">
                                    <asp:CheckBox ID="CheckBoxRepuPintura" runat="server" />
                                    <asp:Label ID="LabelRepuPintura" runat="server" Text="Pintura"></asp:Label>
                                </div>
                                <div class="twentyfive">
                                    <asp:CheckBox ID="CheckBoxRepuInstalacion" runat="server" />
                                    <asp:Label ID="LabelRepuInstalacion" runat="server" Text="Instalación"></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="twenty">
                                    <asp:Label ID="LabelRepuMoneda" runat="server" Text="Moneda"></asp:Label><br />
                                    <asp:DropDownList ID="DropDownListRepuMoneda" runat="server"></asp:DropDownList>
                                </div>
                                <div class="twenty">
                                    <asp:Label ID="LabelPrecioCotizadoRepu" runat="server" Text="Precio Cotizado"></asp:Label><br />
                                    <asp:TextBox ID="TextBoxRepuPrecioCotizado" runat="server" Text="0"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="ValidadorRepuMonto" runat="server" ErrorMessage="*" ControlToValidate="TextBoxRepuPrecioCotizado" CssClass="errormessage"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegExValidatorRepuMonto"
                                        runat="server" ErrorMessage="Verifique el formato del  monto"
                                        ControlToValidate="TextBoxRepuPrecioCotizado" CssClass="errormessage"
                                        ValidationExpression="^[0-9]*\.?[0-9]*$">
                                    </asp:RegularExpressionValidator>
                                </div>
                                <div class="twenty">
                                    <asp:Label ID="LabelRepuTipoDesc" runat="server" Text="Tipo Descuento"></asp:Label><br />
                                    <asp:DropDownList ID="DropDownListRepuTipoDesc" runat="server"></asp:DropDownList>
                                </div>
                                <div class="twenty">
                                    <asp:Label ID="LabelRepuMontoDesc" runat="server" Text="Monto Descuento"></asp:Label><br />
                                    <asp:TextBox ID="TextBoxRepuMontoDesc" runat="server" Text="0"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="ValidadorRepuMontoDesc" runat="server" ErrorMessage="*" ControlToValidate="TextBoxRepuMontoDesc" CssClass="errormessage"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegExValidatorRepuMontoDesc"
                                        runat="server" ErrorMessage="Verifique el formato del  monto"
                                        ControlToValidate="TextBoxRepuMontoDesc" CssClass="errormessage"
                                        ValidationExpression="^[0-9]*\.?[0-9]*$">
                                    </asp:RegularExpressionValidator>
                                </div>
                                <div class="twenty">
                                    <asp:Label ID="LabelRepuPrecioFinal" runat="server" Text="Precio Final"></asp:Label><br />
                                    <asp:TextBox ID="TextBoxRepuPrecioFinal" runat="server" Text="0" Enabled="false"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="fifty">
                                    <asp:Label ID="LabelRepuProveedor" runat="server" Text="Proveedores"></asp:Label><br />
                                    <asp:DropDownList ID="DropDownListRepuProveedor" runat="server"></asp:DropDownList>
                                </div>
                                <div class="fifty">
                                    <asp:Label ID="LabelRepuIdItem" runat="server" Text="IdItem" Visible="False"></asp:Label><br />
                                    <asp:TextBox ID="TextBoxRepuIdItem" runat="server" Enable="false" Visible="False"></asp:TextBox>
                                    <asp:TextBox ID="TextBoxRepuFlagEd" runat="server" Visible="false"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="ButtonRepuGrabar" runat="server" Text="Grabar" Enabled="False" OnClick="ButtonRepuGrabar_Click" />
                                <asp:Button ID="ButtonRepuCancelar" runat="server" Text="Cancelar" Enabled="False" OnClick="ButtonRepuCancelar_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:Button ID="ButtonCancelPopRepuestos" runat="server" Text="Cerrar" OnClick="ButtonCancelPopRepuestos_Click" />
            </asp:Panel>
        </div>

        <div>
            <asp:Button runat="server" ID="ButtonOcultoParaPopupSumatorias" Style="display: none" CssClass="alt2" />

            <ajaxToolkit:ModalPopupExtender runat="server" ID="ModalPopupSumatorias" BehaviorID="ModalPopupSumatoriasBehavior"
                TargetControlID="ButtonOcultoParaPopupSumatorias" PopupControlID="PanelModalPopupABMSumatorias"
                BackgroundCssClass="modalBackground" DropShadow="True" PopupDragHandleControlID="ModalPopupDragHandleSumatorias"
                RepositionMode="RepositionOnWindowScroll">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="PanelModalPopupABMSumatorias" runat="server" CssClass="modalPopup">
                <asp:Panel runat="server" ID="ModalPopupDragHandleSumatorias" CssClass="modalPopupHeader">
                    Editar Sumatorias
                </asp:Panel>
                <div>
                    <table class="basetable alt2">
                        <tr>
                            <td>
                                <strong>
                                    <asp:Label ID="LabelSumaRegistroItems" runat="server" Text="Items - Sumatoria"></asp:Label></strong>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="twenty">
                                    <asp:Label ID="LabelSumaProveedor" runat="server" Text="Proveedores"></asp:Label><br />
                                    <asp:DropDownList ID="DropDownListSumaProveedor" runat="server" Enabled="False"></asp:DropDownList>
                                </div>
                                <div class="twenty">
                                    <asp:Label ID="LabelSumaMontoOrden" runat="server" Text="Monto Orden"></asp:Label><br />
                                    <asp:TextBox ID="TextBoxSumaMontoOrden" runat="server" Enabled="False"></asp:TextBox>
                                </div>
                                <div class="twenty">
                                    <asp:Label ID="LabelSumaTipoDesc" runat="server" Text="Tipo Descuento"></asp:Label><br />
                                    <asp:DropDownList ID="DropDownListSumaTipoDesc" runat="server"></asp:DropDownList>
                                </div>
                                <div class="twenty">
                                    <asp:Label ID="LabelSumaMontoDescProv" runat="server" Text="Monto Descuento"></asp:Label><br />
                                    <asp:TextBox ID="TextBoxSumaMontoDescProv" runat="server" Text="0"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="ValidadorRepuMontoDescProv" runat="server" ErrorMessage="*" ControlToValidate="TextBoxSumaMontoDescProv" CssClass="errormessage"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegExValidatorMontoDescProv"
                                        runat="server" ErrorMessage="Verifique el formato del  monto"
                                        ControlToValidate="TextBoxSumaMontoDescProv" CssClass="errormessage"
                                        ValidationExpression="^[0-9]*\.?[0-9]*$">
                                    </asp:RegularExpressionValidator>
                                </div>
                                <div class="twenty">
                                    <asp:Label ID="LabelSumaDeducible" runat="server" Text="FRANQUICIA / COA"></asp:Label><br />
                                    <asp:TextBox ID="TextBoxSumaDeducible" runat="server" Text="0"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="ValidadorRepuMontoDeducible" runat="server" ErrorMessage="*" ControlToValidate="TextBoxSumaDeducible" CssClass="errormessage"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegExValidatorMontoDeducible"
                                        runat="server" ErrorMessage="Verifique el formato del  monto"
                                        ControlToValidate="TextBoxSumaDeducible" CssClass="errormessage"
                                        ValidationExpression="^[0-9]*\.?[0-9]*$">
                                    </asp:RegularExpressionValidator>
                                </div>
                                <div class="twenty">
                                    <asp:Label ID="LabelSumaMontoFinal" runat="server" Text="Monto Final"></asp:Label><br />
                                    <asp:TextBox ID="TextBoxSumaMontoFinal" runat="server" Enabled="false"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="ButtonSumaGrabar" runat="server" Text="Grabar" Enabled="False" OnClick="ButtonSumaGrabar_Click" />
                                <asp:Button ID="ButtonSumaCancelar" runat="server" Text="Cancelar" Enabled="False" OnClick="ButtonSumaCancelar_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:Button ID="ButtonCancelPopSumatorias" runat="server" Text="Cerrar" OnClick="ButtonCancelPopSumatorias_Click" />
            </asp:Panel>
        </div>

        <div>
            <asp:Button runat="server" ID="ButtonOcultoParaPopupRecepRepuestos" Style="display: none" CssClass="alt2" />

            <ajaxToolkit:ModalPopupExtender runat="server" ID="ModalPopupRecepRepuestos" BehaviorID="ModalPopupRecepRepuestosBehavior"
                TargetControlID="ButtonOcultoParaPopupRecepRepuestos" PopupControlID="PanelModalPopupRecepRepuestos"
                BackgroundCssClass="modalBackground" DropShadow="True" PopupDragHandleControlID="ModalPopupDragHandleRecepRepuestos"
                RepositionMode="RepositionOnWindowScroll">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="PanelModalPopupRecepRepuestos" runat="server" CssClass="modalPopup">
                <asp:Panel runat="server" ID="ModalPopupDragHandleRecepRepuestos" CssClass="modalPopupHeader">
                    Recepción e Ingreso días Repuestos
                </asp:Panel>
                <div>
                    <table class="basetable alt2">
                        <tr>
                            <td>
                                <strong>
                                    <asp:Label ID="LabelRecepRegistroItems" runat="server" Text="Items"></asp:Label></strong>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="twentyfive">
                                    <asp:Label ID="LabelRecepItem" runat="server" Text="Item"></asp:Label><br />
                                    <asp:DropDownList ID="DropDownListRecepItem" runat="server" Enabled="false"></asp:DropDownList>
                                </div>
                                <div class="twentyfive">
                                    <asp:CheckBox ID="CheckBoxRecepRecibido" runat="server" />
                                    <asp:Label ID="LabelRecepRecibido" runat="server" Text="Recibido"></asp:Label>
                                </div>
                                <div class="twentyfive">
                                    <asp:Label ID="LabelPrecioRecepDiasEntrega" runat="server" Text="Días de Entrega"></asp:Label><br />
                                    <asp:TextBox ID="TextBoxRecepDiasEntrega" runat="server" Text="0"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="fifty">
                                    <asp:Label ID="LabelRecepIdItem" runat="server" Text="IdItem" Visible="False"></asp:Label><br />
                                    <asp:TextBox ID="TextBoxRecepIdItem" runat="server" Enable="false" Visible="False"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="ButtonRecepGrabar" runat="server" Text="Grabar" Enabled="False" OnClick="ButtonRecepGrabar_Click" />
                                <asp:Button ID="ButtonRecepCancelar" runat="server" Text="Cancelar" Enabled="False" OnClick="ButtonRecepCancelar_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:Button ID="ButtonCancelPopRecepRepuestos" runat="server" Text="Cerrar" OnClick="ButtonCancelPopRecepRepuestos_Click" />
            </asp:Panel>
        </div>
        <div>
            <rsweb:ReportViewer ID="ReportViewerCoti" runat="server" BackColor="" ClientIDMode="AutoID" HighlightBackgroundColor="" InternalBorderColor="204, 204, 204" InternalBorderStyle="Solid" InternalBorderWidth="1px" LinkActiveColor="" LinkActiveHoverColor="" LinkDisabledColor="" PrimaryButtonBackgroundColor="" PrimaryButtonForegroundColor="" PrimaryButtonHoverBackgroundColor="" PrimaryButtonHoverForegroundColor="" SecondaryButtonBackgroundColor="" SecondaryButtonForegroundColor="" SecondaryButtonHoverBackgroundColor="" SecondaryButtonHoverForegroundColor="" SplitterBackColor="" ToolbarDividerColor="" ToolbarForegroundColor="" ToolbarForegroundDisabledColor="" ToolbarHoverBackgroundColor="" ToolbarHoverForegroundColor="" ToolBarItemBorderColor="" ToolBarItemBorderStyle="Solid" ToolBarItemBorderWidth="1px" ToolBarItemHoverBackColor="" ToolBarItemPressedBorderColor="51, 102, 153" ToolBarItemPressedBorderStyle="Solid" ToolBarItemPressedBorderWidth="1px" ToolBarItemPressedHoverBackColor="153, 187, 226" Width="923px" Visible="false">
                <LocalReport ReportPath="Reportes\RepFormularioCotiDaniosPropios.rdlc">
                </LocalReport>
            </rsweb:ReportViewer>
            <asp:Button ID="ButtonCierraVerRep" runat="server" Text="Ocultar Reporte" Visible="false" OnClick="ButtonCierraVerRep_Click" />
        </div>
    </div>
</asp:Content>
