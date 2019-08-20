<%@ Page Title="" Language="C#" MasterPageFile="~/SitioICRL.Master" AutoEventWireup="true" CodeBehind="CotizacionRCObj.aspx.cs" Inherits="ICRL.Presentacion.CotizacionRCObj" %>

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
                            <asp:Label ID="LabelTituloCotizacion" runat="server" Text="Cotizacion"></asp:Label>
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <div class="thirty">
                                <asp:Label ID="LabelNroFlujo" runat="server" Text="Nro. de Flujo"></asp:Label><br />
                                <asp:TextBox ID="TextBoxNroFlujo" runat="server" Enabled="false"></asp:TextBox><br />
                            </div>
                            <div class="thirty">
                                <asp:Label ID="LabelNroReclamo" runat="server" Text="Nro. de Reclamo"></asp:Label><br />
                                <asp:TextBox ID="TextBoxNroReclamo" runat="server" Enabled="false"></asp:TextBox><br />
                                <asp:TextBox ID="TextBoxIdFlujo" runat="server" Enabled="false" Visible="False"></asp:TextBox><br />
                                <asp:TextBox ID="TextBoxNroCotizacion" runat="server" Enabled="False" Visible="False"></asp:TextBox><br />
                            </div>
                            <div class="thirty">
                                <asp:Label ID="LabelNroCotizacion" runat="server" Text="Nro. de Cotización"></asp:Label><br />
                                <asp:TextBox ID="TextBoxCorrelativo" runat="server" Enabled="False"></asp:TextBox><br />
                                <asp:Label ID="LabelTipoCambio" runat="server" Text="Tipo de Cambio"></asp:Label><br />
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
                    <div>
                        <asp:Label ID="LabelMensaje" runat="server" Text=""></asp:Label>
                    </div>
                </asp:Panel>
            </div>
        </div>
        <div>
            <asp:Panel ID="PanelRCObjeto" runat="server" CssClass="PanelDatosGen">
                <table class="basetable">
                    <tr>
                        <th>
                            <asp:Label ID="LabelDatosObjeto" runat="server" Text="Datos Básicos Persona Responsable Objeto"></asp:Label><br />
                            <asp:Label ID="LabelDatosObjetoMsj" runat="server" Text=""></asp:Label>
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <div class="thirty">
                                <asp:Label ID="LabelObjNombreTer" runat="server" Text="Nombre(s) y Apellido(s):"></asp:Label><br />
                                <asp:TextBox ID="TextBoxObjNombreTer" runat="server"></asp:TextBox><br />
                                <asp:Label ID="LabelObjDocId" runat="server" Text="Documento Id:"></asp:Label><br />
                                <asp:TextBox ID="TextBoxObjDocId" runat="server"></asp:TextBox><br />
                            </div>
                            <div class="thirty">
                                <asp:Label ID="LabelObjTelefono" runat="server" Text="Telefono de Contacto:"></asp:Label><br />
                                <asp:TextBox ID="TextBoxObjTelefono" runat="server"></asp:TextBox><br />
                                <asp:CheckBox ID="CheckBoxObjReembolso" runat="server"></asp:CheckBox>
                                <asp:Label ID="LabelObjReembolso" runat="server" Text="Reembolso"></asp:Label><br />
                            </div>
                            <div class="thirty">
                                <asp:Label ID="LabelObjIdItem" runat="server" Text="Id Objeto:" Visible="false"></asp:Label><br />
                                <asp:TextBox ID="TextBoxObjIdItem" runat="server" Visible="false"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="fifty">
                                <asp:Label ID="LabelObjItem" runat="server" Text="Item:"></asp:Label><br />
                                <asp:TextBox ID="TextBoxObjItem" runat="server"></asp:TextBox><br />
                                <asp:Label ID="LabelObjMontoItemRef" runat="server" Text="Costo Referencia Bs.:"></asp:Label><br />
                                <asp:TextBox ID="TextBoxObjMontoItemRef" runat="server"></asp:TextBox><br />
                                <asp:Label ID="LabelObjDescripcion" runat="server" Text="Descripción:"></asp:Label><br />
                                <asp:TextBox ID="TextBoxObjDescripcion" runat="server"></asp:TextBox><br />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="ButtonObjAgregar" runat="server" Text="Agregar" Visible="true" OnClick="ButtonObjAgregar_Click" />
                            <asp:Button ID="ButtonObjGrabar" runat="server" Text="Grabar" Visible="false" OnClick="ButtonObjGrabar_Click" />
                            <asp:Button ID="ButtonObjCancelar" runat="server" Text="Cancelar" Visible="false" OnClick="ButtonObjCancelar_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
        <div>
            <asp:GridView ID="GridViewObjDetalle" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="false" DataKeyNames="id_item" OnSelectedIndexChanged="GridViewObjDetalle_SelectedIndexChanged" OnRowDeleting="GridViewObjDetalle_RowDeleting">
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
                    <asp:BoundField DataField="nombre_apellido" HeaderText="Nombre(s) y Apellido(s)" />
                    <asp:BoundField DataField="numero_documento" HeaderText="Documento Id" />
                    <asp:BoundField DataField="tipo_item" HeaderText="Item" />
                    <asp:BoundField DataField="monto_item" HeaderText="Costo Ref.Bs." DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="descripcion" HeaderText="Descripción" />
                    <asp:ButtonField Text="Editar" CommandName="Select" ItemStyle-Width="50" ItemStyle-ForeColor="Blue" />
                </Columns>
            </asp:GridView>
        </div>
        <div>
            <table class="basetable" style="width: 100%">
                <tr>
                    <td style="text-align: right">
                        <asp:Button ID="ButtonGenerarOrden" runat="server" Text="Generar Orden" OnClick="ButtonGenerarOrden_Click" ></asp:Button>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <asp:GridView ID="GridViewOrdenes" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="false" DataKeyNames="numero_orden" OnRowCommand="GridViewOrdenes_RowCommand">
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
                    <asp:BoundField DataField="numero_orden" HeaderText="Número Orden" />
                    <asp:BoundField DataField="id_estado" HeaderText="Estado" />
                    <asp:BoundField DataField="descripcion" HeaderText="Proveedor" />
                    <asp:BoundField DataField="monto_bs" HeaderText="Monto Orden" DataFormatString="{0:N2}" />
                    <asp:ButtonField CommandName="Imprimir" ButtonType="Button" HeaderText="Opción" Text="Imp" />
                    <asp:ButtonField CommandName="Ver" ButtonType="Button" HeaderText="Opción" Text="Ver" />
                    <asp:ButtonField CommandName="SubirOnBase" ButtonType="Button" HeaderText="Opción" Text="On Base" />
                </Columns>
            </asp:GridView>
        </div>
        <div>
            <rsweb:ReportViewer ID="ReportViewerCoti" runat="server" BackColor="" ClientIDMode="AutoID" HighlightBackgroundColor="" InternalBorderColor="204, 204, 204" InternalBorderStyle="Solid" InternalBorderWidth="1px" LinkActiveColor="" LinkActiveHoverColor="" LinkDisabledColor="" PrimaryButtonBackgroundColor="" PrimaryButtonForegroundColor="" PrimaryButtonHoverBackgroundColor="" PrimaryButtonHoverForegroundColor="" SecondaryButtonBackgroundColor="" SecondaryButtonForegroundColor="" SecondaryButtonHoverBackgroundColor="" SecondaryButtonHoverForegroundColor="" SplitterBackColor="" ToolbarDividerColor="" ToolbarForegroundColor="" ToolbarForegroundDisabledColor="" ToolbarHoverBackgroundColor="" ToolbarHoverForegroundColor="" ToolBarItemBorderColor="" ToolBarItemBorderStyle="Solid" ToolBarItemBorderWidth="1px" ToolBarItemHoverBackColor="" ToolBarItemPressedBorderColor="51, 102, 153" ToolBarItemPressedBorderStyle="Solid" ToolBarItemPressedBorderWidth="1px" ToolBarItemPressedHoverBackColor="153, 187, 226" Width="923px" Visible="false">
                <LocalReport ReportPath="Reportes\RepFormularioCotiRCObjetos.rdlc">
                </LocalReport>
            </rsweb:ReportViewer>
            <asp:Button ID="ButtonCierraVerRep" runat="server" Text="Ocultar Reporte" OnClick="ButtonCierraVerRep_Click" />
        </div>
    </div>
</asp:Content>
