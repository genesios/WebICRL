<%@ Page Title="" Language="C#" MasterPageFile="~/SitioICRL.Master" AutoEventWireup="true" CodeBehind="CotizacionDPRP.aspx.cs" Inherits="ICRL.Presentacion.CotizacionDPRP" %>

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
                CollapseControlID="Panel1" ExpandControlID="Panel1" CollapsedText="Mostrar" />

            <ajaxToolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server"
                Collapsed="True" CollapsedSize="0"
                ExpandedText="Ocultar" TargetControlID="PanelRCVehicular" TextLabelID="textLabelVeh"
                ImageControlID="Image2" ExpandedImage="~/img/collapse.jpg" CollapsedImage="~/img/expand.jpg"
                CollapseControlID="PanelColapsableVeh" ExpandControlID="PanelColapsableVeh" CollapsedText="Mostrar" />
            <div>

                <table class="basetable">
                    <tr>
                        <th>
                            <asp:Label ID="LabelTituloInspeccion" runat="server" Text="Inspección"></asp:Label>
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
                                <asp:TextBox ID="TextBoxNroReclamo" runat="server" Enabled="false"></asp:TextBox><br />
                                <asp:TextBox ID="TextBoxIdFlujo" runat="server" Enabled="false" Visible="False"></asp:TextBox><br />
                                <asp:TextBox ID="TextBoxNroCotizacion" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                            </div>
                            <div class="twentyfive">
                                <asp:Label ID="LabelNroCotizacion" runat="server" Text="Nro. de Cotización"></asp:Label><br />
                                <asp:TextBox ID="TextBoxCorrelativo" runat="server"></asp:TextBox>
                                <asp:Label ID="LabelTipoCambio" runat="server" Text="Tipo de Cambio"></asp:Label>
                                <asp:TextBox ID="TextBoxTipoCambio" runat="server" Text="6.96"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="ButtonActualizaDesdeOnBase" runat="server" Text="Actualizar desde OnBase" />
                        </td>
                    </tr>
                </table>

                <asp:Panel runat="server" ID="Panel1">
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
        <div>
            <asp:Panel runat="server" ID="PanelColapsableVeh" >
                <div class="collapseBar">
                    <span><strong style="text-transform: uppercase">
                        <asp:Label runat="server" ID="textLabelVeh" />
                        DATOS VEHICULO TERCERO</strong></span>
                    <asp:Image ID="Image2" runat="server" ImageUrl="~/img/collapse.jpg" />
                </div>
            </asp:Panel>
            <asp:Panel ID="PanelRCVehicular" runat="server" CssClass="PanelDatosGen">
                <table class="basetable">
                    <tr>
                        <th>
                            <asp:Label ID="LabelDatosVehicular" runat="server" Text="Datos Vehiculo Tercero"></asp:Label>
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <div class="fifty">
                                <asp:Label ID="LabelVehMarca" runat="server" Text="Marca:"></asp:Label><br />
                                <asp:TextBox ID="TextBoxVehMarca" runat="server"></asp:TextBox><br />
                                <asp:Label ID="LabelVehModelo" runat="server" Text="Modelo:"></asp:Label><br />
                                <asp:TextBox ID="TextBoxVehModelo" runat="server"></asp:TextBox><br />
                                <asp:Label ID="LabelVehAnio" runat="server" Text="Año:"></asp:Label><br />
                                <asp:TextBox ID="TextBoxVehAnio" runat="server" Enabled="False"></asp:TextBox><br />
                                <asp:Label ID="LabelVehPlaca" runat="server" Text="Placa:"></asp:Label><br />
                                <asp:TextBox ID="TextBoxVehPlaca" runat="server"></asp:TextBox>
                            </div>
                            <div class="fifty">
                                <asp:Label ID="LabelVehColor" runat="server" Text="Color:"></asp:Label><br />
                                <asp:TextBox ID="TextBoxVehColor" runat="server"></asp:TextBox><br />
                                <asp:Label ID="LabelVehKilometraje" runat="server" Text="Valor:"></asp:Label><br />
                                <asp:TextBox ID="TextBoxVehKilometraje" runat="server" Enabled="False"></asp:TextBox><br />
                                <asp:Label ID="LabelVehNroChasis" runat="server" Text="Número Chasis:"></asp:Label><br />
                                <asp:TextBox ID="TextBoxVehNroChasis" runat="server"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
        <div>
            <table class="basetable">
                <tr>
                    <th>
                        <asp:Label ID="LabelReparaciones" runat="server" Text="Reparaciones"></asp:Label>
                    </th>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="ButtonRepaAgregarItem" runat="server" Text="Agregar" OnClick="ButtonRepaAgregarItem_Click" />
                    </td>
                </tr>

            </table>
        </div>

        <div>
            <asp:GridView ID="GridViewReparaciones" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="false" OnSelectedIndexChanged="GridViewReparaciones_SelectedIndexChanged" OnRowDeleting="GridViewReparaciones_RowDeleting">
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
                    <asp:ButtonField Text="Borrar" CommandName="Delete" ItemStyle-Width="50" ItemStyle-ForeColor="Red" />
                    <asp:BoundField DataField="id_item" HeaderText="Id" />
                    <asp:BoundField DataField="item_descripcion" HeaderText="Item" />
                    <asp:BoundField DataField="chaperio" HeaderText="Chaperio" />
                    <asp:BoundField DataField="reparacion_previa" HeaderText="Reparación Previa" />
                    <asp:TemplateField HeaderText="Mec.">
                        <ItemTemplate>
                            <asp:CheckBox runat="server" Checked='<%# Eval("mecanico") %>'></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="id_moneda" HeaderText="Moneda" />
                    <asp:BoundField DataField="precio_cotizado" HeaderText="Precio Cotizado" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="id_tipo_descuento" HeaderText="Descuento F/P" />
                    <asp:BoundField DataField="descuento" HeaderText="Monto Descuento" />
                    <asp:BoundField DataField="precio_final" HeaderText="Precio Final" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="proveedor" HeaderText="Proveedor" />
                    <asp:ButtonField Text="Editar" CommandName="Select" ItemStyle-Width="50" ItemStyle-ForeColor="Blue" />
                </Columns>
            </asp:GridView>
        </div>
        <div>
            <asp:GridView ID="GridViewSumaReparaciones" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
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
            </asp:GridView>
        </div>
        <div>
            <table class="basetable">
                <tr>
                    <th>
                        <asp:Label ID="LabelRepuestos" runat="server" Text="Repuestos"></asp:Label>
                    </th>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="ButtonRepuAgregarItem" runat="server" Text="Agregar" OnClick="ButtonRepuAgregarItem_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <asp:Panel ID="PanelABMRepuestos" runat="server" Enabled="false">
                <table class="basetable">
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
                            </div>
                            <div class="twentyfive">
                                <asp:CheckBox ID="CheckBoxRepuPintura" runat="server" />
                                <asp:Label ID="LabelRepuPintura" runat="server" Text="Pintura"></asp:Label>
                            </div>
                            <div class="twentyfive">
                                <asp:CheckBox ID="CheckBoxRepuInstalacion" runat="server" />
                                <asp:Label ID="LabelRepuInstalacion" runat="server" Text="Instalacion"></asp:Label>
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
                                <asp:TextBox ID="TextBoxRepuPrecioCotizado" runat="server"></asp:TextBox>
                            </div>
                            <div class="twenty">
                                <asp:Label ID="LabelRepuTipoDesc" runat="server" Text="Tipo Descuento"></asp:Label><br />
                                <asp:DropDownList ID="DropDownListRepuTipoDesc" runat="server"></asp:DropDownList>
                            </div>
                            <div class="twenty">
                                <asp:Label ID="LabelRepuMontoDesc" runat="server" Text="Monto Descuento"></asp:Label><br />
                                <asp:TextBox ID="TextBoxRepuMontoDesc" runat="server"></asp:TextBox>
                            </div>
                            <div class="twenty">
                                <asp:Label ID="LabelRepuPrecioFinal" runat="server" Text="Precio Final"></asp:Label><br />
                                <asp:TextBox ID="TextBoxRepuPrecioFinal" runat="server" Enabled="false"></asp:TextBox>
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
            </asp:Panel>
        </div>
        <div>
            <asp:GridView ID="GridViewRepuestos" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="false" OnSelectedIndexChanged="GridViewRepuestos_SelectedIndexChanged" OnRowDeleting="GridViewRepuestos_RowDeleting">
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
                    <asp:ButtonField Text="Borrar" CommandName="Delete" ItemStyle-Width="50" ItemStyle-ForeColor="Red" />
                    <asp:BoundField DataField="id_item" HeaderText="Id" />
                    <asp:BoundField DataField="item_descripcion" HeaderText="Item" />
                    <asp:TemplateField HeaderText="Pint.">
                        <ItemTemplate>
                            <asp:CheckBox runat="server" Checked='<%# Eval("pintura") %>'></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Inst.">
                        <ItemTemplate>
                            <asp:CheckBox runat="server" Checked='<%# Eval("instalacion") %>'></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="id_moneda" HeaderText="Moneda" />
                    <asp:BoundField DataField="precio_cotizado" HeaderText="Precio Cotizado" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="id_tipo_descuento" HeaderText="Descuento F/P" />
                    <asp:BoundField DataField="descuento" HeaderText="Monto Descuento" />
                    <asp:BoundField DataField="precio_final" HeaderText="Precio Final" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="proveedor" HeaderText="Proveedor" />
                    <asp:ButtonField Text="Editar" CommandName="Select" ItemStyle-Width="50" ItemStyle-ForeColor="Blue" />
                </Columns>
            </asp:GridView>
        </div>
        <div>
            <asp:GridView ID="GridViewSumaRepuestos" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
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
            </asp:GridView>
        </div>
        <div>
            <asp:Button runat="server" ID="ButtonOcultoParaPopupReparaciones" Style="display: none" />

            <ajaxToolkit:ModalPopupExtender runat="server" ID="ModalPopupReparaciones" BehaviorID="ModalPopupReparacionesBehavior"
                TargetControlID="ButtonOcultoParaPopupReparaciones" PopupControlID="PanelModalPopupABMReparaciones"
                BackgroundCssClass="modalBackground" DropShadow="True" PopupDragHandleControlID="ModalPopupDragHandleReparaciones"
                RepositionMode="None" X="10" Y="10">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="PanelModalPopupABMReparaciones" runat="server" CssClass="modalPopup">
                <asp:Panel runat="server" ID="ModalPopupDragHandleReparaciones" CssClass="modalPopupHeader">
                    Ventana PopUp para ABM Reparaciones
                </asp:Panel>
                <div>
                    <table class="basetable">
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
                                </div>
                                <div class="twentyfive">
                                    <asp:Label ID="LabelRepaChaperio" runat="server" Text="Chaperio"></asp:Label><br />
                                    <asp:DropDownList ID="DropDownListRepaChaperio" runat="server"></asp:DropDownList>
                                </div>
                                <div class="twentyfive">
                                    <asp:Label ID="LabelRepaReparacionPrevia" runat="server" Text="Reparacion Previa"></asp:Label><br />
                                    <asp:DropDownList ID="DropDownListRepaRepPrevia" runat="server"></asp:DropDownList>
                                </div>
                                <div class="twentyfive">
                                    <asp:CheckBox ID="CheckBoxRepaMecanico" runat="server" />
                                    <asp:Label ID="LabelRepaMecanico" runat="server" Text="Mecanico"></asp:Label>
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
                                    <asp:TextBox ID="TextBoxRepaPrecioCotizado" runat="server"></asp:TextBox>
                                </div>
                                <div class="twenty">
                                    <asp:Label ID="LabelRepaTipoDesc" runat="server" Text="Tipo Descuento"></asp:Label><br />
                                    <asp:DropDownList ID="DropDownListRepaTipoDesc" runat="server"></asp:DropDownList>
                                </div>
                                <div class="twenty">
                                    <asp:Label ID="LabelRepaMontoDesc" runat="server" Text="Monto Descuento"></asp:Label><br />
                                    <asp:TextBox ID="TextBoxRepaMontoDesc" runat="server"></asp:TextBox>
                                </div>
                                <div class="twenty">
                                    <asp:Label ID="LabelRepaPrecioFinal" runat="server" Text="Precio Final"></asp:Label><br />
                                    <asp:TextBox ID="TextBoxRepaPrecioFinal" runat="server" Enabled="false"></asp:TextBox>
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
    </div>
</asp:Content>
