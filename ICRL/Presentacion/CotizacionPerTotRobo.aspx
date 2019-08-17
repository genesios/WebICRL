<%@ Page Title="" Language="C#" MasterPageFile="~/SitioICRL.Master" AutoEventWireup="true" CodeBehind="CotizacionPerTotRobo.aspx.cs" Inherits="ICRL.Presentacion.CotizacionPerTotRobo" %>

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
            <asp:Panel ID="PanelDatosEspeciales" runat="server" CssClass="PanelDatosGen">
                <table class="basetable">
                    <tr>
                        <td>
                            <strong>
                                <asp:Label ID="LabelRegistroDatosPTRO" runat="server" Text="Vehículo con Condiciones Especiales de Indemnización"></asp:Label></strong>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="twenty">
                                <asp:Label ID="LabelVersionPTRO" runat="server" Text="Versión"></asp:Label><br />
                                <asp:TextBox ID="TextBoxVersionPTRO" runat="server"></asp:TextBox><br />
                                <asp:Label ID="LabelSeriePTRO" runat="server" Text="Serie"></asp:Label><br />
                                <asp:TextBox ID="TextBoxSeriePTRO" runat="server"></asp:TextBox>
                            </div>
                            <div class="twenty">
                                <asp:Label ID="LabelCilindraPTRO" runat="server" Text="Cilindrada"></asp:Label><br />
                                <asp:TextBox ID="TextBoxCilindradaPTRO" runat="server"></asp:TextBox><br />
                                <asp:Label ID="LabelObservacionesPTRO" runat="server" Text="Observaciones"></asp:Label><br />
                                <asp:TextBox ID="TextBoxObservacionesPTRO" runat="server" MaxLength="100"></asp:TextBox>
                            </div>
                            <div class="twenty">
                                <asp:Label ID="LabelCajaPTRO" runat="server" Text="Caja"></asp:Label><br />
                                <asp:DropDownList ID="DropDownListCajaPTRO" runat="server"></asp:DropDownList><br />
                                <asp:Label ID="LabelCombustiblePTRO" runat="server" Text="Combustible"></asp:Label><br />
                                <asp:DropDownList ID="DropDownListCombustiblePTRO" runat="server"></asp:DropDownList><br />
                            </div>
                            <div class="twenty">
                                <asp:CheckBox ID="CheckBoxTechoSolarPTRO" runat="server" />
                                <asp:Label ID="LabelTechoSolarPTRO" runat="server" Text="Techo Solar"></asp:Label><br />
                                <asp:CheckBox ID="CheckBoxAsientosCueroPTRO" runat="server" />
                                <asp:Label ID="LabelAsientosCueroPTRO" runat="server" Text="Asientos de Cuero"></asp:Label>
                            </div>
                            <div class="twenty">
                                <asp:CheckBox ID="CheckBoxArosMagnesioPTRO" runat="server" />
                                <asp:Label ID="LabelArosMagnesioPTRO" runat="server" Text="Aros de Magnesio"></asp:Label><br />
                                <asp:CheckBox ID="CheckBoxConvertidoGnvPTRO" runat="server" />
                                <asp:Label ID="LabelConvertidoGNVPTRO" runat="server" Text="ConvertidoGNV"></asp:Label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="ButtonActualizarPTRO" runat="server" Text="Actualizar" Enabled="true" />
                            <asp:Button ID="ButtonGrabarPTRO" runat="server" Text="Grabar" Enabled="False" />
                            <asp:Button ID="ButtonCancelarPTRO" runat="server" Text="Cancelar" Enabled="False" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
        <div>
            <table class="basetable">
                <tr>
                    <th>
                        <asp:Label ID="LabelDatosIndemnizacion" runat="server" Text="Datos Beneficiario(s) de Indemnización"></asp:Label>
                        <asp:Label ID="LabelDatosDueniosMsj" runat="server" Text=""></asp:Label>
                    </th>
                </tr>
                <tr>
                    <td>
                        <div>
                            <asp:CheckBox ID="CheckBoxPTROTotPagado" runat="server" OnCheckedChanged="CheckBoxPTROTotPagado_CheckedChanged" AutoPostBack="True" />
                            <asp:Label ID="LabelPTROTotPagado" runat="server" Text="¿No Completamente Pagado?"></asp:Label>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="fifty">
                            <asp:Label ID="LabelPTRONombres" runat="server" Text="Nombre(s) y Apellido(s):"></asp:Label><br />
                            <asp:TextBox ID="TextBoxPTRONombres" runat="server"></asp:TextBox>
                            <asp:Label ID="LabelPTRODocumentoId" runat="server" Text="Documento Id.:"></asp:Label><br />
                            <asp:TextBox ID="TextBoxPTRODocumentoId" runat="server"></asp:TextBox>
                        </div>
                        <div class="fifty">
                            <asp:Label ID="LabelPTROMontoPago" runat="server" Text="Monto Pago en Bs.:"></asp:Label><br />
                            <asp:TextBox ID="TextBoxPTROMontoPago" runat="server"></asp:TextBox>
                            <asp:Label ID="LabelPTRODescripcion" runat="server" Text="Descripción:"></asp:Label><br />
                            <asp:TextBox ID="TextBoxPTRODescripcion" runat="server"></asp:TextBox>
                            <asp:TextBox ID="TextBoxPTROIndice" runat="server" Text="" Visible="false"></asp:TextBox>
                            <asp:TextBox ID="TextBoxPTROIdItem" runat="server" Text="" Visible="false"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="ButtonPTROAgregar" runat="server" Text="Agregar" Visible="false" OnClick="ButtonPTROAgregar_Click" />
                        <asp:Button ID="ButtonPTROGrabar" runat="server" Text="Grabar" Visible="false" OnClick="ButtonPTROGrabar_Click" />
                        <asp:Button ID="ButtonPTROCancelar" runat="server" Text="Cancelar" Visible="false" OnClick="ButtonPTROCancelar_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <div>
        </div>
        <div>
            <asp:GridView ID="GridViewPTRODueniosaPagar" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="false" DataKeyNames="id_fila" OnRowDeleting="GridViewPTRODueniosaPagar_RowDeleting" OnSelectedIndexChanged="GridViewPTRODueniosaPagar_SelectedIndexChanged">
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
                    <asp:BoundField DataField="id_fila" HeaderText="Id" />
                    <asp:BoundField DataField="duenio_nombres" HeaderText="Nombre(s) y Apellido(s)" />
                    <asp:BoundField DataField="duenio_documento" HeaderText="Doc.Id." />
                    <asp:BoundField DataField="duenio_monto" HeaderText="Monto Pago Bs." DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="duenio_descripcion" HeaderText="Descripción" />
                    <asp:ButtonField Text="Editar" CommandName="Select" ItemStyle-Width="50" ItemStyle-ForeColor="Blue" />
                </Columns>
            </asp:GridView>
        </div>
        <div>
            <table class="basetable">
                <tr>
                    <th>
                        <asp:Label ID="LabelDatosReferencias" runat="server" Text="Lista de Referencias"></asp:Label>
                        <asp:Label ID="LabelDatosReferMsj" runat="server" Text=""></asp:Label>
                    </th>
                </tr>
                <tr>
                    <td>
                        <div class="fifty">
                            <asp:CheckBox ID="CheckboxReferUtilizada" runat="server"></asp:CheckBox>
                            <asp:Label ID="LabelReferUtilizada" runat="server" Text="Referencia Utilizada:"></asp:Label><br />

                            <asp:Label ID="LabelReferMedioCoti" runat="server" Text="Medio Cotizado:"></asp:Label><br />
                            <asp:TextBox ID="TextBoxReferMedioCoti" runat="server"></asp:TextBox>
                        </div>
                        <div class="fifty">
                            <asp:Label ID="LabelReferDescripcion" runat="server" Text="Descripción:"></asp:Label><br />
                            <asp:TextBox ID="TextBoxReferDescripcion" runat="server"></asp:TextBox>
                            <asp:Label ID="LabelReferMontoCoti" runat="server" Text="Monto Cotizado en Bs.:"></asp:Label><br />
                            <asp:TextBox ID="TextBoxReferMontoCoti" runat="server"></asp:TextBox>
                            <asp:TextBox ID="TextBoxReferIndice" runat="server" Text="" Visible="false"></asp:TextBox>
                            <asp:TextBox ID="TextBoxReferIdItem" runat="server" Text="" Visible="false"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="ButtonReferAgregar" runat="server" Text="Agregar" Visible="true" OnClick="ButtonReferAgregar_Click" />
                        <asp:Button ID="ButtonReferGrabar" runat="server" Text="Grabar" Visible="false" OnClick="ButtonReferGrabar_Click" />
                        <asp:Button ID="ButtonReferCancelar" runat="server" Text="Cancelar" Visible="false" OnClick="ButtonReferCancelar_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <asp:GridView ID="GridViewPTROReferencias" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="false" DataKeyNames="id" OnRowDeleting="GridViewPTROReferencias_RowDeleting" OnSelectedIndexChanged="GridViewPTROReferencias_SelectedIndexChanged">
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
                    <asp:BoundField DataField="id" HeaderText="Id" />
                    <asp:TemplateField HeaderText="Ref.Utilizada">
                        <ItemTemplate>
                            <asp:CheckBox runat="server" Checked='<%# Eval("usada") %>'></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="medios" HeaderText="Medio Ref." />
                    <asp:BoundField DataField="descripcion" HeaderText="Descripción" />
                    <asp:BoundField DataField="monto" HeaderText="Monto en Bs." DataFormatString="{0:N2}" />
                    <asp:ButtonField Text="Editar" CommandName="Select" ItemStyle-Width="50" ItemStyle-ForeColor="Blue" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
