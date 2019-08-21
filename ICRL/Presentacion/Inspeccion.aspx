<%@ Page Title="" Language="C#" MasterPageFile="~/SitioICRL.Master" AutoEventWireup="true" CodeBehind="Inspeccion.aspx.cs" Inherits="ICRL.Presentacion.Inspeccion" %>

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

    <%--    <%# Eval("placa") %>--%>


    <div>
        <ajaxToolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server"
            Collapsed="True" CollapsedSize="0"
            ExpandedText="Ocultar" TargetControlID="PanelDatosGen" TextLabelID="textLabel"
            ImageControlID="Image1" ExpandedImage="~/img/collapse.jpg" CollapsedImage="~/img/expand.jpg"
            CollapseControlID="Panel1" ExpandControlID="Panel1" CollapsedText="Mostrar" />
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
                            <asp:TextBox ID="TextBoxNroFlujo" runat="server"></asp:TextBox>
                        </div>
                        <div class="twentyfive">
                            <asp:Label ID="Label1" runat="server" Text="Nro. de Reclamo"></asp:Label><br />
                            <asp:TextBox ID="TextBoxNroReclamo" runat="server"></asp:TextBox><br />
                            <asp:TextBox ID="TextBoxIdFlujo" runat="server" Visible="False"></asp:TextBox><br />
                            <asp:TextBox ID="TextBoxNroInspeccion" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                        </div>
                        <div class="twentyfive">
                            <asp:Label ID="LabelNroInspeccion" runat="server" Text="Nro. de Inspección"></asp:Label><br />
                            <asp:TextBox ID="TextBoxCorrelativo" runat="server"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="ButtonActualizaDesdeOnBase" runat="server" Text="Actualizar desde OnBase" OnClick="ButtonActualizaDesdeOnBase_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>
                            <asp:Label ID="LabelCoberturas" runat="server" Text="Cobertura Afectada:"></asp:Label></strong>
                        <div class="twentyfive">
                            <asp:CheckBox ID="CheckBoxDaniosPropios" runat="server" OnCheckedChanged="CheckBoxDaniosPropios_CheckedChanged" AutoPostBack="True" />
                            <asp:Label ID="LabelDaniosPropios" runat="server" Text="Daños Propios"></asp:Label><br />
                            <asp:CheckBox ID="CheckBoxRCObjetos" runat="server" OnCheckedChanged="CheckBoxRCObjetos_CheckedChanged" AutoPostBack="True" />
                            <asp:Label ID="LabelRCObjetos" runat="server" Text="RC a Objetos"></asp:Label>
                        </div>
                        <div class="twentyfive">
                            <asp:CheckBox ID="CheckBoxRCPersonas" runat="server" OnCheckedChanged="CheckBoxRCPersonas_CheckedChanged" AutoPostBack="True" />
                            <asp:Label ID="LabelRCPersonas" runat="server" Text="RC Personas"></asp:Label><br />
                            <asp:CheckBox ID="CheckBoxRoboParcial" runat="server" OnCheckedChanged="CheckBoxRoboParcial_CheckedChanged" AutoPostBack="True" />
                            <asp:Label ID="LabelRoboParcial" runat="server" Text="Robo Parcial"></asp:Label>
                        </div>
                        <div class="twentyfive">
                            <asp:CheckBox ID="CheckBoxRCVehicular01" runat="server" OnCheckedChanged="CheckBoxRCVehicular01_CheckedChanged" AutoPostBack="True" />
                            <asp:Label ID="LabelRCVehicular01" runat="server" Text="RC Vehicular 01"></asp:Label><br />
                            <asp:CheckBox ID="CheckBoxPerdidaTotDanios" runat="server" OnCheckedChanged="CheckBoxPerdidaTotDanios_CheckedChanged" AutoPostBack="True" />
                            <asp:Label ID="LabelPerdidaTotDanios" runat="server" Text="Pérdida Total por Daños"></asp:Label>
                        </div>
                        <div class="twentyfive">
                            <asp:CheckBox ID="CheckBoxPerdidaTotRobo" runat="server" OnCheckedChanged="CheckBoxPerdidaTotRobo_CheckedChanged" AutoPostBack="True" />
                            <asp:Label ID="LabelPerdidaTotRobo" runat="server" Text="Pérdida Total por Robo"></asp:Label>
                        </div>
                    </td>
                </tr>
            </table>


            <asp:Panel runat="server" ID="Panel1">
                <div class="collapseBar">
                    <span>Haga clic aqui para<strong style="text-transform: uppercase"><asp:Label runat="server" ID="textLabel" /></strong></span>
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
                            <div class="fifty">
                                <asp:Label ID="LabelTelefContacto" runat="server" Text="Teléfono Contacto"></asp:Label><br />
                                <asp:TextBox ID="TextBoxTelefContacto" runat="server"></asp:TextBox><br />
                                <asp:Label ID="LabelEmailsEnvio" runat="server" Text="Emails de envío"></asp:Label><br />
                                <asp:TextBox ID="TextBoxEmailsEnvio" runat="server"></asp:TextBox><br />
                                <asp:RequiredFieldValidator ID="ValidadorCorreo" runat="server" ErrorMessage="*" ControlToValidate="TextBoxEmailsEnvio" CssClass="errormessage"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegExValidatorCorreos"
                                    runat="server" ErrorMessage="Verifique el formato de los correos electrónicos"
                                    ControlToValidate="TextBoxEmailsEnvio" CssClass="errormessage"
                                    ValidationExpression="^([\w+-.%]+@[\w.-]+\.[A-Za-z]{2,4})(;[\w+-.%]+@[\w.-]+\.[A-Za-z]{2,4})*$">
                                </asp:RegularExpressionValidator>
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
                                <asp:DropDownList ID="DropDownListTipoTallerInsp" runat="server" Enabled="true"></asp:DropDownList>
                            </div>
                        </td>
                    </tr>
                </table>
                <div>
                    <asp:Button ID="ButtonGrabarDatosInspeccion" runat="server" Text="Grabar Datos" OnClick="ButtonGrabarDatosInspeccion_Click" />
                </div>
                <div>
                    <asp:Label ID="LabelMensaje" runat="server" Text=""></asp:Label>
                </div>
            </asp:Panel>
        </div>

        <div>
            <ajaxToolkit:TabContainer ID="TabContainerCoberturas" runat="server" ActiveTabIndex="0" OnActiveTabChanged="TabContainerCoberturas_ActiveTabChanged" AutoPostBack="True">
                <ajaxToolkit:TabPanel runat="server" HeaderText="Daños Propios" ID="TabPanelDaniosPropios" TabIndex="1" Enabled="False" Visible="false">
                    <ContentTemplate>
                        <div>
                            <table class="basetable">
                                <tr>
                                    <td>
                                        <strong>
                                            <asp:Label ID="LabelRegistroDaniosPropios" runat="server" Text="Daños Propios"></asp:Label></strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="twentyfive">
                                            <asp:Label ID="LabelDPPTipoTaller" runat="server" Text="Tipo de Taller"></asp:Label><br />
                                            <asp:DropDownList ID="DropDownListDPPTipoTaller" runat="server"></asp:DropDownList>
                                        </div>
                                        <div class="twentyfive">
                                            <asp:CheckBox ID="CheckBoxDPPCambioPerdidaTotal" runat="server"></asp:CheckBox>
                                            <asp:Label ID="LabelDPPCambioPerdidaTotal" runat="server" Text="Cambio Pérdida Total"></asp:Label>
                                        </div>
                                        <div class="twentyfive">
                                            <asp:Label ID="LabelDPPSecuencial" runat="server" Text="DPSecuencial" Visible="False"></asp:Label><br />
                                            <asp:TextBox ID="TextBoxDPPSecuencial" runat="server" Visible="False"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="ButtonNuevoDPPadre" runat="server" Text="Agregar" OnClick="ButtonNuevoDPPadre_Click" />
                                        <asp:Button ID="ButtonGrabarDPPadre" runat="server" Text="Grabar" Enabled="False" OnClick="ButtonGrabarDPPadre_Click" />
                                        <asp:Button ID="ButtonBorrarDPPadre" runat="server" Text="Borrar" Enabled="False" OnClick="ButtonBorrarDPPadre_Click" />
                                        <asp:Button ID="ButtonDetalleDPPadre" runat="server" Text="Detalle" Enabled="False" OnClick="ButtonDetalleDPPadre_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div>
                            <table class="basetable" style="width: 100%">
                                <tr>
                                    <td style="text-align: left">
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Button ID="ButtonFinalizarInspDP" runat="server" Text="Finalizar" Visible="False" OnClick="ButtonFinalizarInspDP_Click" ></asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div>
                            <asp:GridView ID="GridViewDaniosPropiosPadre" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" DataKeyNames="secuencial" OnRowDataBound="GridViewDaniosPropiosPadre_RowDataBound" OnSelectedIndexChanged="GridViewDaniosPropiosPadre_SelectedIndexChanged" OnRowCommand="GridViewDaniosPropiosPadre_RowCommand" Width="100%">
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
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <button class="btn btn-default glyphicon glyphicon-plus" onclick="return ToggleGridPanel(this, 'trdp<%# Eval("secuencial") %>')" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:ButtonField Text="Editar" CommandName="Select">
                                        <ItemStyle Width="60px" />
                                    </asp:ButtonField>
                                    <asp:ButtonField CommandName="ImprimirFormularioInsp" ButtonType="Button" HeaderText="Opción" Text="Imp.Form" />
                                    <asp:BoundField DataField="Secuencial" HeaderText="Sec" />
                                    <asp:BoundField DataField="tipoTaller" HeaderText="Tipo de Taller" />
                                    <asp:TemplateField HeaderText="Cambio a Pérdida Total">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" Checked='<%# Eval("cambioAPerdidaTotal") %>'></asp:CheckBox>
                                            <%# MyNewRowDetDP ( Eval("secuencial") ) %>
                                            <asp:GridView ID="gvDPDet" runat="server" CellPadding="4" ForeColor="#333333"
                                                Width="100%" GridLines="Both"
                                                AutoGenerateColumns="false">
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:BoundField DataField="descripcion" HeaderText="Item" />
                                                    <asp:BoundField DataField="compra" HeaderText="Compra" />
                                                    <asp:BoundField DataField="chaperio" HeaderText="Chaperio" />
                                                    <asp:BoundField DataField="reparacionPrevia" HeaderText="Rep.Previa" />
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
                            </asp:GridView>
                        </div>
                        <div>
                            <asp:Button runat="server" ID="ButtonOcultoParaPopupDaniosPropios" Style="display: none" />
                            <ajaxToolkit:ModalPopupExtender ID="ModalPopupDaniosPropios" runat="server"
                                PopupControlID="PanelModalPopupDaniosPropios" TargetControlID="ButtonOcultoParaPopupDaniosPropios" PopupDragHandleControlID="ModalPopupDragHandleDaniosPropios"
                                RepositionMode="None" X="10" Y="10"
                                BackgroundCssClass="modalBackground" DropShadow="True" BehaviorID="ModalPopupDaniosPropiosBehavior" DynamicServicePath="">
                            </ajaxToolkit:ModalPopupExtender>
                            <asp:Panel ID="PanelModalPopupDaniosPropios" runat="server" CssClass="modalPopup">
                                <asp:Panel runat="server" ID="ModalPopupDragHandleDaniosPropios" CssClass="modalPopupHeader">
                                    Ventana PopUp para el Detalle de Daños Propios
                                </asp:Panel>
                                <div>
                                    <table class="basetable">
                                        <tr>
                                            <td>
                                                <strong>
                                                    <asp:Label ID="LabelRegistroItems" runat="server" Text="Items"></asp:Label></strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="twenty">
                                                    <asp:Label ID="LabelItem" runat="server" Text="Item"></asp:Label><br />
                                                    <asp:DropDownList ID="DropDownListItem" runat="server"></asp:DropDownList>
                                                </div>
                                                <div class="twenty">
                                                    <asp:Label ID="LabelCompra" runat="server" Text="Compra"></asp:Label><br />
                                                    <asp:DropDownList ID="DropDownListCompra" runat="server"></asp:DropDownList>
                                                </div>
                                                <div class="twenty">
                                                    <asp:CheckBox ID="CheckBoxInstalacion" runat="server" />
                                                    <asp:Label ID="LabelInstalacion" runat="server" Text="Instalacion"></asp:Label>
                                                </div>
                                                <div class="twenty">
                                                    <asp:CheckBox ID="CheckBoxPintura" runat="server" />
                                                    <asp:Label ID="LabelPintura" runat="server" Text="Pintura"></asp:Label>
                                                </div>
                                                <div class="twenty">
                                                    <asp:CheckBox ID="CheckBoxMecanico" runat="server" />
                                                    <asp:Label ID="LabelMecanico" runat="server" Text="Mecanico"></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="twentyfive">
                                                    <asp:Label ID="LabelChaperio" runat="server" Text="Chaperio"></asp:Label><br />
                                                    <asp:DropDownList ID="DropDownListChaperio" runat="server"></asp:DropDownList>
                                                </div>
                                                <div class="twentyfive">
                                                    <asp:Label ID="LabelReparacionPrevia" runat="server" Text="Reparacion Previa"></asp:Label><br />
                                                    <asp:DropDownList ID="DropDownListRepPrevia" runat="server"></asp:DropDownList>
                                                </div>
                                                <div class="twentyfive">
                                                    <asp:Label ID="LabelObservaciones" runat="server" Text="Observaciones"></asp:Label><br />
                                                    <asp:TextBox ID="TextBoxObservaciones" runat="server" MaxLength="100"></asp:TextBox>
                                                </div>
                                                <div class="twentyfive">
                                                    <asp:Label ID="LabelIditem" runat="server" Text="IdItem" Visible="False"></asp:Label><br />
                                                    <asp:TextBox ID="TextBoxIdItem" runat="server" Visible="False"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="ButtonNuevoDP" runat="server" Text="Agregar" OnClick="ButtonNuevoDP_Click" />
                                                <asp:Button ID="ButtonGrabarDP" runat="server" Text="Grabar" Enabled="False" OnClick="ButtonGrabarDP_Click" />
                                                <asp:Button ID="ButtonBorrarDP" runat="server" Text="Borrar" Enabled="False" OnClick="ButtonBorrarDP_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div>
                                    <asp:GridView ID="GridViewDaniosPropios" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" OnRowDataBound="GridViewDaniosPropios_RowDataBound" OnSelectedIndexChanged="GridViewDaniosPropios_SelectedIndexChanged" Width="100%">
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
                                            <asp:ButtonField Text="Editar" CommandName="Select">
                                                <ItemStyle Width="60px" />
                                            </asp:ButtonField>
                                            <asp:BoundField DataField="idItem" HeaderText="id" />
                                            <asp:BoundField DataField="descripcion" HeaderText="Item" />
                                            <asp:BoundField DataField="compra" HeaderText="Compra" />
                                            <asp:TemplateField HeaderText="Inst.">
                                                <ItemTemplate>
                                                    <asp:CheckBox runat="server" Checked='<%# Eval("instalacion") %>'></asp:CheckBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pint.">
                                                <ItemTemplate>
                                                    <asp:CheckBox runat="server" Checked='<%# Eval("pintura") %>'></asp:CheckBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Mec.">
                                                <ItemTemplate>
                                                    <asp:CheckBox runat="server" Checked='<%# Eval("mecanico") %>'></asp:CheckBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="chaperio" HeaderText="Chaperio" />
                                            <asp:BoundField DataField="reparacionprevia" HeaderText="Reparación Previa" />
                                            <asp:BoundField DataField="observaciones" HeaderText="Observaciones" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <asp:Button ID="ButtonCancelPopDP" runat="server" Text="Cerrar" OnClick="ButtonCancelPopDP_Click" />
                            </asp:Panel>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="TabPanelRCObjetos" TabIndex="2" runat="server" HeaderText="RCObjetos" Enabled="False" Visible="false">
                    <ContentTemplate>
                        <div>
                            <table class="basetable">
                                <tr>
                                    <td>
                                        <strong>
                                            <asp:Label ID="LabelRespObjetos" runat="server" Text="Responsables de los Objetos"></asp:Label></strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="twentyfive">
                                            <asp:Label ID="LabelNombreObjetos" runat="server" Text="Nombre(s) y Apellido(s)"></asp:Label><br />
                                            <asp:TextBox ID="TextBoxNombresApObjeto" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="twentyfive">
                                            <asp:Label ID="LabelDocIdObjetos" runat="server" Text="Documento Identidad"></asp:Label><br />
                                            <asp:TextBox ID="TextBoxDocIdObjeto" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="twentyfive">
                                            <asp:Label ID="LabelObjItem" runat="server" Text="ObjIdSecuencial" Visible="False"></asp:Label><br />
                                            <asp:TextBox ID="TextBoxObjIdSecuencial" runat="server" Visible="False"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="twentyfive">
                                            <asp:Label ID="LabelObsObjeto" runat="server" Text="Observaciones"></asp:Label><br />
                                            <asp:TextBox ID="TextBoxObsObjeto" runat="server" MaxLength="100"></asp:TextBox>
                                        </div>
                                        <div class="twentyfive">
                                            <asp:Label ID="LabelTelfObjeto" runat="server" Text="Teléfono Contacto"></asp:Label><br />
                                            <asp:TextBox ID="TextBoxTelfObjeto" runat="server"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="ButtonNuevoObj" runat="server" Text="Agregar" Enabled="true" OnClick="ButtonNuevoObj_Click" />
                                        <asp:Button ID="ButtonGrabarObj" runat="server" Text="Grabar" Enabled="False" OnClick="ButtonGrabarObj_Click" />
                                        <asp:Button ID="ButtonBorrarObj" runat="server" Text="Borrar" Enabled="False" OnClick="ButtonBorrarObj_Click" />
                                        <asp:Button ID="ButtonDetalleObj" runat="server" Text="Detalle" Enabled="False" OnClick="ButtonDetalleObj_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div>
                            <asp:GridView ID="GridViewObjetos" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="False" DataKeyNames="secuencial" OnRowDataBound="GridViewObjetos_RowDataBound" OnSelectedIndexChanged="GridViewObjetos_SelectedIndexChanged" OnRowCommand="GridViewObjetos_RowCommand" Width="100%">
                                <AlternatingRowStyle BackColor="White" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <button class="btn btn-default glyphicon glyphicon-plus" onclick="return ToggleGridPanel(this, 'trrco<%# Eval("secuencial") %>')" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:ButtonField Text="Editar" CommandName="Select">
                                        <ItemStyle Width="60px" />
                                    </asp:ButtonField>
                                    <asp:ButtonField CommandName="ImprimirFormularioInsp" ButtonType="Button" HeaderText="Opción" Text="Imp.Form" />
                                    <asp:BoundField DataField="secuencial" HeaderText="Sec" />
                                    <asp:BoundField DataField="nombreObjeto" HeaderText="Responsable Objeto" />
                                    <asp:BoundField DataField="docIdentidadObjeto" HeaderText="Doc.Id. Resp." />
                                    <asp:BoundField DataField="telefonoObjeto" HeaderText="Teléfono Resp." />
                                    <asp:TemplateField HeaderText="Observaciones">
                                        <ItemTemplate>
                                            <%# Eval("observacionesObjeto") %>
                                            <%# MyNewRowObjDet ( Eval("secuencial") ) %>
                                            <asp:GridView ID="gvRCObjDet" runat="server" CellPadding="4" ForeColor="#333333"
                                                Width="100%" GridLines="None"
                                                AutoGenerateColumns="false">
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:BoundField DataField="secuencial" HeaderText="Sec" />
                                                    <asp:BoundField DataField="idItem" HeaderText="Item" />
                                                    <asp:BoundField DataField="costoReferencial" HeaderText="Costo Ref." DataFormatString="{0:C2}" />
                                                    <asp:BoundField DataField="descripcion" HeaderText="Descripción" />
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
                            <asp:Button runat="server" ID="ButtonOcultoParaPopupRCObjetos" Style="display: none" />
                            <ajaxToolkit:ModalPopupExtender ID="ModalPopupRCObjetos" runat="server"
                                PopupControlID="PanelModalPopupRCObjetos" TargetControlID="ButtonOcultoParaPopupRCObjetos" PopupDragHandleControlID="ModalPopupDragHandleRCObjetos"
                                RepositionMode="None" X="10" Y="10"
                                BackgroundCssClass="modalBackground" DropShadow="True" BehaviorID="ModalPopupRCObjetosBehavior" DynamicServicePath="">
                            </ajaxToolkit:ModalPopupExtender>
                            <asp:Panel ID="PanelModalPopupRCObjetos" runat="server" CssClass="modalPopup">
                                <asp:Panel runat="server" ID="ModalPopupDragHandleRCObjetos" CssClass="modalPopupHeader">
                                    Ventana PopUp para el Detalle de RC Objetos
                                </asp:Panel>
                                <div>
                                    <asp:TextBox ID="TextBoxSecuencialPopRCObj" runat="server" Visible="False"></asp:TextBox>
                                    <asp:TextBox ID="TextBoxIdInspeccionPopRCObj" runat="server" Visible="False"></asp:TextBox>
                                </div>
                                <div>
                                    <table class="basetable">
                                        <tr>
                                            <td>
                                                <div class="twentyfive">
                                                    <asp:Label ID="LabelObjDetItem" runat="server" Text="Item"></asp:Label><br />
                                                    <asp:TextBox ID="TextBoxObjDetItem" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="twentyfive">
                                                    <asp:Label ID="LabelObjDetCostoRef" runat="server" Text="Costo Ref.Bs."></asp:Label><br />
                                                    <asp:TextBox ID="TextBoxObjDetCostoRef" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="twentyfive">
                                                    <asp:Label ID="LabelObjDetDescrip" runat="server" Text="Descripción"></asp:Label><br />
                                                    <asp:TextBox ID="TextBoxObjDetDescripcion" runat="server"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="ButtonNuevoObjDet" runat="server" Text="Agregar Det" Enabled="true" OnClick="ButtonNuevoObjDet_Click" />
                                                <asp:Button ID="ButtonGrabarObjDet" runat="server" Text="Grabar Det" Enabled="False" OnClick="ButtonGrabarObjDet_Click" />
                                                <asp:Button ID="ButtonBorrarObjDet" runat="server" Text="Borrar Det" Enabled="False" OnClick="ButtonBorrarObjDet_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div>
                                    <asp:GridView ID="GridViewObjDetalle" runat="server" CellPadding="4" ForeColor="#333333" OnRowDataBound="GridViewObjDetalle_RowDataBound" OnSelectedIndexChanged="GridViewObjDetalle_SelectedIndexChanged" GridLines="None" Width="100%">
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:ButtonField Text="Clic ->" CommandName="Select">
                                                <ItemStyle Width="60px" />
                                            </asp:ButtonField>
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
                                </div>
                                <asp:Button ID="ButtonCancelPopRCObj" runat="server" Text="Cerrar" OnClick="ButtonCancelPopRCObj_Click" />
                            </asp:Panel>
                        </div>


                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="TabPanelRCPersonas" TabIndex="3" runat="server" HeaderText="RCPersonas" Enabled="False" Visible="false">
                    <ContentTemplate>
                        <div>
                            <table class="basetable">
                                <tr>
                                    <td>
                                        <strong>
                                            <asp:Label ID="LabelRespPersonas" runat="server" Text="Personas Involucradas con el siniestro"></asp:Label></strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="twenty">
                                            <asp:Label ID="LabelNombrePersonas" runat="server" Text="Nombre(s) y Apellido(s)"></asp:Label><br />
                                            <asp:TextBox ID="TextBoxNombresApPersona" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="twenty">
                                            <asp:Label ID="LabelDocIdPersonas" runat="server" Text="Documento Identidad"></asp:Label><br />
                                            <asp:TextBox ID="TextBoxDocIdPersona" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="twenty">
                                            <asp:Label ID="LabelPersonaSecuencial" runat="server" Text="Persona IdSecuencial" Visible="False"></asp:Label><br />
                                            <asp:TextBox ID="TextBoxPersonaIdSecuencial" runat="server" Visible="False"></asp:TextBox>
                                        </div>
                                        <div class="twenty">
                                            <asp:Label ID="LabelObsPersona" runat="server" Text="Observaciones"></asp:Label><br />
                                            <asp:TextBox ID="TextBoxObsPersona" runat="server" MaxLength="100"></asp:TextBox>
                                        </div>
                                        <div class="twenty">
                                            <asp:Label ID="LabelTelfPersona" runat="server" Text="Teléfono Contacto"></asp:Label><br />
                                            <asp:TextBox ID="TextBoxTelfPersona" runat="server"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="ButtonNuevoPer" runat="server" Text="Agregar" OnClick="ButtonNuevoPer_Click" />
                                        <asp:Button ID="ButtonGrabarPer" runat="server" Text="Grabar" Enabled="False" OnClick="ButtonGrabarPer_Click" />
                                        <asp:Button ID="ButtonBorrarPer" runat="server" Text="Borrar" Enabled="False" OnClick="ButtonBorrarPer_Click" />
                                        <asp:Button ID="ButtonDetallePer" runat="server" Text="Detalle" Enabled="False" OnClick="ButtonDetallePer_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div>
                            <asp:GridView ID="GridViewPersonas" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="False" DataKeyNames="secuencial" OnRowDataBound="GridViewPersonas_RowDataBound" OnSelectedIndexChanged="GridViewPersonas_SelectedIndexChanged" GridLines="None" OnRowCommand="GridViewPersonas_RowCommand" Width="100%">
                                <AlternatingRowStyle BackColor="White" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <button class="btn btn-default glyphicon glyphicon-plus" onclick="return ToggleGridPanel(this, 'trrcp<%# Eval("secuencial") %>')" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:ButtonField Text="Editar" CommandName="Select">
                                        <ItemStyle Width="60px" />
                                    </asp:ButtonField>
                                    <asp:ButtonField CommandName="ImprimirFormularioInsp" ButtonType="Button" HeaderText="Opción" Text="Imp.Form" />
                                    <asp:BoundField DataField="secuencial" HeaderText="Sec" />
                                    <asp:BoundField DataField="nombrePersona" HeaderText="Persona Afectada" />
                                    <asp:BoundField DataField="docIdentidadPersona" HeaderText="Doc.Id." />
                                    <asp:BoundField DataField="telefonoPersona" HeaderText="Teléfono" />
                                    <asp:TemplateField HeaderText="Observaciones">
                                        <ItemTemplate>
                                            <%# Eval("observacionesPersona") %>
                                            <%# MyNewRowPerDet ( Eval("secuencial") ) %>
                                            <asp:GridView ID="gvRCPerDet" runat="server" CellPadding="4" ForeColor="#333333"
                                                Width="100%" GridLines="None"
                                                AutoGenerateColumns="false">
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:BoundField DataField="secuencial" HeaderText="Sec" />
                                                    <asp:BoundField DataField="tipo" HeaderText="Tipo" />
                                                    <asp:BoundField DataField="montoGasto" HeaderText="Monto Gasto" DataFormatString="{0:C2}" />
                                                    <asp:BoundField DataField="descripcion" HeaderText="Descripción" />
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
                            <asp:Button runat="server" ID="ButtonOcultoParaPopupRCPersonas" Style="display: none" />
                            <ajaxToolkit:ModalPopupExtender ID="ModalPopupRCPersonas" runat="server"
                                PopupControlID="PanelModalPopupRCPersonas" TargetControlID="ButtonOcultoParaPopupRCPersonas" PopupDragHandleControlID="ModalPopupDragHandleRCPersonas"
                                RepositionMode="None" X="10" Y="10"
                                BackgroundCssClass="modalBackground" DropShadow="True" BehaviorID="ModalPopupRCPersonasBehavior" DynamicServicePath="">
                            </ajaxToolkit:ModalPopupExtender>
                            <asp:Panel ID="PanelModalPopupRCPersonas" runat="server" CssClass="modalPopup">
                                <asp:Panel runat="server" ID="ModalPopupDragHandleRCPersonas" CssClass="modalPopupHeader">
                                    Ventana PopUp para el Detalle de RC Personas
                                </asp:Panel>
                                <div>
                                    <table class="basetable">
                                        <tr>
                                            <td>
                                                <div class="twentyfive">
                                                    <asp:Label ID="LabelPerDetTipo" runat="server" Text="Tipo"></asp:Label><br />
                                                    <asp:TextBox ID="TextBoxPerDetTipo" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="twentyfive">
                                                    <asp:Label ID="LabelPerDetMontoGasto" runat="server" Text="Gasto en Bs."></asp:Label><br />
                                                    <asp:TextBox ID="TextBoxPerDetMontoGasto" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="twentyfive">
                                                    <asp:Label ID="LabelPerDetDescrip" runat="server" Text="Descripción"></asp:Label><br />
                                                    <asp:TextBox ID="TextBoxPerDetDescripcion" runat="server" Width="255px"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="ButtonNuevoPerDet" runat="server" Text="Agregar Det" Enabled="true" OnClick="ButtonNuevoPerDet_Click" />
                                                <asp:Button ID="ButtonGrabarPerDet" runat="server" Text="Grabar Det" Enabled="False" OnClick="ButtonGrabarPerDet_Click" />
                                                <asp:Button ID="ButtonBorrarPerDet" runat="server" Text="Borrar Det" Enabled="False" OnClick="ButtonBorrarPerDet_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div>
                                    <asp:GridView ID="GridViewPerDetalle" runat="server" CellPadding="4" ForeColor="#333333" OnRowDataBound="GridViewPerDetalle_RowDataBound" OnSelectedIndexChanged="GridViewPerDetalle_SelectedIndexChanged" GridLines="None" Width="100%">
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:ButtonField Text="Clic -->" CommandName="Select">
                                                <ItemStyle Width="60px" />
                                            </asp:ButtonField>
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
                                </div>
                                <asp:Button ID="ButtonCancelPopRCPer" runat="server" Text="Cerrar" OnClick="ButtonCancelPopRCPer_Click" />
                            </asp:Panel>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="TabPanelRoboParcial" TabIndex="4" runat="server" HeaderText="Robo Parcial" Enabled="False" Visible="false">
                    <ContentTemplate>
                        <div>
                            <table class="basetable">
                                <tr>
                                    <td>
                                        <strong>
                                            <asp:Label ID="LabelRegistroItemsRP" runat="server" Text="Items"></asp:Label></strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="twenty">
                                            <asp:Label ID="LabelItemRP" runat="server" Text="Item"></asp:Label><br />
                                            <asp:DropDownList ID="DropDownListItemRP" runat="server"></asp:DropDownList><br />
                                            <asp:Label ID="LabelChaperioRP" runat="server" Text="Chaperio"></asp:Label><br />
                                            <asp:TextBox ID="TextBoxChaperioRP" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="twenty">
                                            <asp:Label ID="LabelCompraRP" runat="server" Text="Compra"></asp:Label><br />
                                            <asp:TextBox ID="TextBoxCompraRP" runat="server"></asp:TextBox><br />
                                            <asp:Label ID="LabelReparacionPreviaRP" runat="server" Text="Reparacion Previa"></asp:Label><br />
                                            <asp:TextBox ID="TextBoxRepPreviaRP" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="twenty">
                                            <asp:Label ID="LabelObservacionesRP" runat="server" Text="Observaciones"></asp:Label><br />
                                            <asp:TextBox ID="TextBoxObservacionesRP" runat="server" MaxLength="100"></asp:TextBox>
                                        </div>
                                        <div class="twenty">
                                            <asp:CheckBox ID="CheckBoxInstalacionRP" runat="server" />
                                            <asp:Label ID="LabelInstalacionRP" runat="server" Text="Instalacion"></asp:Label><br />
                                            <asp:CheckBox ID="CheckBoxPinturaRP" runat="server" />
                                            <asp:Label ID="LabelPinturaRP" runat="server" Text="Pintura"></asp:Label><br />
                                            <asp:CheckBox ID="CheckBoxMecanicoRP" runat="server" />
                                            <asp:Label ID="LabelMecanicoRP" runat="server" Text="Mecanico"></asp:Label>
                                        </div>
                                        <div class="twenty">
                                            <asp:Label ID="LabelIditemRP" runat="server" Text="IdItem" Visible="False"></asp:Label><br />
                                            <asp:TextBox ID="TextBoxIdItemRP" runat="server" Visible="False"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="ButtonNuevoRP" runat="server" Text="Agregar" Enabled="true" OnClick="ButtonNuevoRP_Click" />
                                        <asp:Button ID="ButtonGrabarRP" runat="server" Text="Grabar" Enabled="False" OnClick="ButtonGrabarRP_Click" />
                                        <asp:Button ID="ButtonBorrarRP" runat="server" Text="Borrar" Enabled="False" OnClick="ButtonBorrarRP_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div>
                            <asp:GridView ID="GridViewRoboParcial" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="false" OnRowDataBound="GridViewRoboParcial_RowDataBound" OnSelectedIndexChanged="GridViewRoboParcial_SelectedIndexChanged" Width="100%">
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
                                    <asp:ButtonField Text="Editar" CommandName="Select">
                                        <ItemStyle Width="60px" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="idItem" HeaderText="id" />
                                    <asp:BoundField DataField="descripcion" HeaderText="Item" />
                                    <asp:BoundField DataField="compra" HeaderText="Compra" />
                                    <asp:TemplateField HeaderText="Inst.">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" Checked='<%# Eval("instalacion") %>'></asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Pint.">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" Checked='<%# Eval("pintura") %>'></asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mec.">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" Checked='<%# Eval("mecanico") %>'></asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="chaperio" HeaderText="Chaperio" />
                                    <asp:BoundField DataField="reparacionprevia" HeaderText="Reparación Previa" />
                                    <asp:BoundField DataField="observaciones" HeaderText="Observaciones" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div>
                            <asp:ImageButton ID="ImgButtonExportPdfRoboP" runat="server" ImageUrl="~/img/ico_pdf.jpg" OnClick="ImgButtonExportPdfRoboP_Click" />
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="TabPanelPerdidaTotalDaniosPropios" TabIndex="5" runat="server" HeaderText="PT Danios Propios" Enabled="False" Visible="false">
                    <ContentTemplate>
                        <div>
                            <table class="basetable">
                                <tr>
                                    <td>
                                        <strong>
                                            <asp:Label ID="LabelRegistroDatosPTDP" runat="server" Text="Vehículo con Condiciones Especiales de Indemnización"></asp:Label></strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="twenty">
                                            <asp:Label ID="LabelVersionPTDP" runat="server" Text="Versión"></asp:Label><br />
                                            <asp:TextBox ID="TextBoxVersionPTDP" runat="server"></asp:TextBox><br />
                                            <asp:Label ID="LabelSerieTDP" runat="server" Text="Serie"></asp:Label><br />
                                            <asp:TextBox ID="TextBoxSeriePTDP" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="twenty">
                                            <asp:Label ID="LabelCajaPTDP" runat="server" Text="Caja"></asp:Label><br />
                                            <asp:DropDownList ID="DropDownListCajaPTDP" runat="server"></asp:DropDownList><br />
                                            <asp:Label ID="LabelCombustiblePTDP" runat="server" Text="Combustible"></asp:Label><br />
                                            <asp:DropDownList ID="DropDownListCombustiblePTDP" runat="server"></asp:DropDownList><br />
                                        </div>
                                        <div class="twenty">
                                            <asp:Label ID="LabelCilindraPTDP" runat="server" Text="Cilindrada"></asp:Label><br />
                                            <asp:TextBox ID="TextBoxCilindradaPTDP" runat="server"></asp:TextBox><br />
                                            <asp:Label ID="LabelObservacionesPTDP" runat="server" Text="Observaciones"></asp:Label><br />
                                            <asp:TextBox ID="TextBoxObservacionesPTDP" runat="server" MaxLength="100"></asp:TextBox>
                                        </div>
                                        <div class="twenty">
                                            <asp:CheckBox ID="CheckBoxTechoSolarPTDP" runat="server" />
                                            <asp:Label ID="LabelTechoSolarPTDP" runat="server" Text="Techo Solar"></asp:Label><br />
                                            <asp:CheckBox ID="CheckBoxAsientosCueroPTDP" runat="server" />
                                            <asp:Label ID="LabelAsientosCueroPTDP" runat="server" Text="Asientos de Cuero"></asp:Label>
                                        </div>
                                        <div class="twenty">
                                            <asp:CheckBox ID="CheckBoxArosMagnesioPTDP" runat="server" />
                                            <asp:Label ID="LabelArosMagnesioPTDP" runat="server" Text="Aros de Magnesio"></asp:Label><br />
                                            <asp:CheckBox ID="CheckBoxConvertidoGnvPTDP" runat="server" />
                                            <asp:Label ID="LabelConvertidoGNVPTDP" runat="server" Text="ConvertidoGNV"></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="ButtonNuevoPTDP" runat="server" Text="Agregar" Enabled="true" OnClick="ButtonNuevoPTDP_Click" />
                                        <asp:Button ID="ButtonGrabarPTDP" runat="server" Text="Grabar" Enabled="False" OnClick="ButtonGrabarPTDP_Click" />
                                        <asp:Button ID="ButtonBorrarPTDP" runat="server" Text="Borrar" Enabled="False" OnClick="ButtonBorrarPTDP_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div>
                            <asp:ImageButton ID="ImgButtonExportPdfPTDP" runat="server" ImageUrl="~/img/ico_pdf.jpg" OnClick="ImgButtonExportPdfPTDP_Click" />
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="TabPanelPerdidaTotalRobo" TabIndex="6" runat="server" HeaderText="PT por Robo" Enabled="False" Visible="false">
                    <ContentTemplate>
                        <div>
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
                                        <asp:Button ID="ButtonNuevoPTRO" runat="server" Text="Agregar" Enabled="true" OnClick="ButtonNuevoPTRO_Click" />
                                        <asp:Button ID="ButtonGrabarPTRO" runat="server" Text="Grabar" Enabled="False" OnClick="ButtonGrabarPTRO_Click" />
                                        <asp:Button ID="ButtonBorrarPTRO" runat="server" Text="Borrar" Enabled="False" OnClick="ButtonBorrarPTRO_Click" />
                                    </td>
                                </tr>
                            </table>
                            <div>
                                <asp:ImageButton ID="ImgButtonExportPdfPTRO" runat="server" ImageUrl="~/img/ico_pdf.jpg" OnClick="ImgButtonExportPdfPTRO_Click" />
                            </div>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="TabPanelRCV01" TabIndex="7" runat="server" HeaderText="RC Vehicular" Enabled="False" Visible="false">
                    <ContentTemplate>
                        <div>
                            <table class="basetable">
                                <tr>
                                    <td>
                                        <strong>
                                            <asp:Label ID="LabelVehiculoAseguradoRCV01" runat="server" Text="Datos Responsabilidad Vehicular"></asp:Label></strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="twenty">
                                            <asp:Label ID="LabelNombreTerceroRCV01" runat="server" Text="Nombre Tercero"></asp:Label><br />
                                            <asp:TextBox ID="TextBoxNombreTerceroRCV01" runat="server"></asp:TextBox><br />
                                            <asp:Label ID="LabelDocIdTerceroRCV01" runat="server" Text="DocId. Tercero"></asp:Label><br />
                                            <asp:TextBox ID="TextBoxDocIdTerceroRCV01" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="twenty">
                                            <asp:Label ID="LabelTelfTerceroRCV01" runat="server" Text="Telf. Tercero"></asp:Label><br />
                                            <asp:TextBox ID="TextBoxTelfTerceroRCV01" runat="server"></asp:TextBox><br />
                                            <asp:Label ID="LabelMarcaRCV01" runat="server" Text="Marca:"></asp:Label><br />
                                            <asp:DropDownList ID="DropDownListMarcaRCV01" runat="server"></asp:DropDownList>
                                        </div>
                                        <div class="twenty">
                                            <asp:Label ID="LabelModeloRCV01" runat="server" Text="Modelo:"></asp:Label><br />
                                            <asp:TextBox ID="TextBoxModeloRCV01" runat="server"></asp:TextBox><br />
                                            <asp:Label ID="LabelAnioRCV01" runat="server" Text="Año:"></asp:Label><br />
                                            <asp:TextBox ID="TextBoxAnioRCV01" runat="server"></asp:TextBox><br />
                                            <asp:Label ID="LabelNroChasisRCV01" runat="server" Text="Número Chasis:"></asp:Label><br />
                                            <asp:TextBox ID="TextBoxNroChasisRCV01" runat="server"></asp:TextBox><br />
                                            <asp:TextBox ID="TextBoxSecuencialRCV01" runat="server" Visible="False"></asp:TextBox>
                                        </div>
                                        <div class="twenty">
                                            <asp:Label ID="LabelPlacaRCV01" runat="server" Text="Placa:"></asp:Label><br />
                                            <asp:TextBox ID="TextBoxPlacaRCV01" runat="server"></asp:TextBox><br />
                                            <asp:Label ID="LabelColorRCV01" runat="server" Text="Color:"></asp:Label><br />
                                            <asp:DropDownList ID="DropDownListColorRCV01" runat="server"></asp:DropDownList><br />
                                            <asp:Label ID="LabelKilometrajeRCV01" runat="server" Text="Kilometraje:"></asp:Label><br />
                                            <asp:TextBox ID="TextBoxKilometrajeRCV01" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="twenty">
                                            <asp:Label ID="LabelTipoTallerRCVeh" runat="server" Text="Tipo Taller"></asp:Label><br />
                                            <asp:DropDownList ID="DropDownListTipoTallerRCVeh" runat="server"></asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="ButtonMNuevoRCV01" runat="server" Text="Agregar" Enable="true" OnClick="ButtonMNuevoRCV01_Click" />
                                        <asp:Button ID="ButtonMGrabarRCV01" runat="server" Text="Grabar" Enable="false" OnClick="ButtonMGrabarRCV01_Click" />
                                        <asp:Button ID="ButtonMBorrarRCV01" runat="server" Text="Borrar" Enable="false" OnClick="ButtonMBorrarRCV01_Click" />
                                        <asp:Button ID="ButtonMDetalleRCV01" runat="server" Text="Detalle" Enable="false" OnClick="ButtonMDetalleRCV01_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div>
                            <asp:GridView ID="GridViewRCV01" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" DataKeyNames="secuencial" OnRowDataBound="GridViewRCV01_RowDataBound" OnSelectedIndexChanged="GridViewRCV01_SelectedIndexChanged" OnRowCommand="GridViewRCV01_RowCommand" Width="100%">
                                <AlternatingRowStyle BackColor="White" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <button class="btn btn-default glyphicon glyphicon-plus" onclick="return ToggleGridPanel(this, 'trvh<%# Eval("secuencial") %>')" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:ButtonField Text="Editar" CommandName="Select">
                                        <ItemStyle Width="60px" />
                                    </asp:ButtonField>
                                    <asp:ButtonField CommandName="ImprimirFormularioInsp" ButtonType="Button" HeaderText="Opción" Text="Imp.Form" />
                                    <asp:BoundField DataField="secuencial" HeaderText="Sec" />
                                    <asp:BoundField DataField="nombreTercero" HeaderText="Nombre Tercero" />
                                    <asp:BoundField DataField="marca" HeaderText="Marca" />
                                    <asp:BoundField DataField="modelo" HeaderText="Modelo" />
                                    <asp:BoundField DataField="color" HeaderText="Color" />
                                    <asp:BoundField DataField="anio" HeaderText="Año" />
                                    <asp:BoundField DataField="chasis" HeaderText="Chasis" />
                                    <asp:TemplateField HeaderText="Placa">
                                        <ItemTemplate>
                                            <%# Eval("placa") %>
                                            <%# MyNewRowDet ( Eval("secuencial") ) %>
                                            <asp:GridView ID="gvRCVDet" runat="server" CellPadding="4" ForeColor="#333333"
                                                Width="100%" GridLines="Both"
                                                AutoGenerateColumns="false">
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:BoundField DataField="descripcion" HeaderText="Item" />
                                                    <asp:BoundField DataField="compra" HeaderText="Compra" />
                                                    <asp:BoundField DataField="chaperio" HeaderText="Chaperio" />
                                                    <asp:BoundField DataField="reparacionPrevia" HeaderText="Rep.Previa" />
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
                            <asp:LinkButton runat="server" ID="LinkButtonShowPopup"></asp:LinkButton>
                            <asp:Button runat="server" ID="ButtonOcultoparaPopup" Style="display: none" />
                            <ajaxToolkit:ModalPopupExtender ID="ModalPopupRCV01" runat="server"
                                PopupControlID="PanelModalPopup" TargetControlID="ButtonOcultoparaPopup" PopupDragHandleControlID="ModalPopupDragHandle"
                                RepositionMode="None" X="10" Y="10"
                                BackgroundCssClass="modalBackground" DropShadow="True" BehaviorID="ModalPopupRCV01Behavior" DynamicServicePath="">
                            </ajaxToolkit:ModalPopupExtender>
                            <asp:Panel ID="PanelModalPopup" runat="server" CssClass="modalPopup">
                                <asp:Panel runat="server" ID="ModalPopupDragHandle" CssClass="modalPopupHeader">
                                    Ventana PopUp para el Detalle de RC Vehicular
                                </asp:Panel>
                                <div>
                                    <asp:TextBox ID="TextBoxSecuencialPop" runat="server" Visible="False"></asp:TextBox>
                                    <asp:TextBox ID="TextBoxIdInspeccionPop" runat="server" Visible="False"></asp:TextBox>
                                </div>
                                <div>
                                    <table class="basetable">
                                        <tr>
                                            <td>
                                                <strong>
                                                    <asp:Label ID="LabelRegistroItemsRCV01" runat="server" Text="Items"></asp:Label></strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="twenty">
                                                    <asp:Label ID="LabelItemRCV01" runat="server" Text="Item"></asp:Label><br />
                                                    <asp:DropDownList ID="DropDownListItemRCV01" runat="server"></asp:DropDownList><br />
                                                    <asp:Label ID="LabelCompraRCV01" runat="server" Text="Compra"></asp:Label><br />
                                                    <asp:TextBox ID="TextBoxCompraRCV01" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="twenty">
                                                    <asp:CheckBox ID="CheckBoxInstalacionRCV01" runat="server" />
                                                    <asp:Label ID="LabelInstalacionRCV01" runat="server" Text="Instalacion"></asp:Label><br />
                                                    <asp:CheckBox ID="CheckBoxPinturaRCV01" runat="server" />
                                                    <asp:Label ID="LabelPinturaRCV01" runat="server" Text="Pintura"></asp:Label>

                                                </div>
                                                <div class="twenty">
                                                    <asp:CheckBox ID="CheckBoxMecanicoRCV01" runat="server" />
                                                    <asp:Label ID="LabelMecanicoRCV01" runat="server" Text="Mecanico"></asp:Label><br />

                                                </div>
                                                <div class="twenty">
                                                    <asp:Label ID="LabelChaperioRCV01" runat="server" Text="Chaperio"></asp:Label><br />
                                                    <asp:DropDownList ID="DropDownListChaperioRCV01" runat="server"></asp:DropDownList><br />
                                                    <asp:Label ID="LabelReparacionPreviaRCV01" runat="server" Text="Reparacion Previa"></asp:Label><br />
                                                    <asp:DropDownList ID="DropDownListRepPreviaRCV01" runat="server"></asp:DropDownList>
                                                </div>
                                                <div class="twenty">
                                                    <asp:Label ID="LabelObservacionesRCV01" runat="server" Text="Observaciones"></asp:Label><br />
                                                    <asp:TextBox ID="TextBoxObservacionesRCV01" runat="server" MaxLength="100"></asp:TextBox><br />
                                                    <asp:Label ID="LabelIditemRCV01" runat="server" Text="IdItem" Visible="False"></asp:Label><br />
                                                    <asp:TextBox ID="TextBoxIdItemRCV01" runat="server" Visible="False"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="ButtonDNuevoRCV01" runat="server" Text="Agregar" OnClick="ButtonDNuevoRCV01_Click" />
                                                <asp:Button ID="ButtonDGrabarRCV01" runat="server" Text="Grabar" Enabled="False" OnClick="ButtonDGrabarRCV01_Click" />
                                                <asp:Button ID="ButtonDBorrarRCV01" runat="server" Text="Borrar" Enabled="False" OnClick="ButtonDBorrarRCV01_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div>
                                    <asp:GridView ID="GridViewRCV01Det" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" OnRowDataBound="GridViewRCV01Det_RowDataBound" OnSelectedIndexChanged="GridViewRCV01Det_SelectedIndexChanged" Width="100%">
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
                                            <asp:ButtonField Text="Editar" CommandName="Select">
                                                <ItemStyle Width="60px" />
                                            </asp:ButtonField>
                                            <asp:BoundField DataField="idItem" HeaderText="id" />
                                            <asp:BoundField DataField="descripcion" HeaderText="Item" />
                                            <asp:BoundField DataField="compra" HeaderText="Compra" />
                                            <asp:TemplateField HeaderText="Inst.">
                                                <ItemTemplate>
                                                    <asp:CheckBox runat="server" Checked='<%# Eval("instalacion") %>'></asp:CheckBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pint.">
                                                <ItemTemplate>
                                                    <asp:CheckBox runat="server" Checked='<%# Eval("pintura") %>'></asp:CheckBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Mec.">
                                                <ItemTemplate>
                                                    <asp:CheckBox runat="server" Checked='<%# Eval("mecanico") %>'></asp:CheckBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="chaperio" HeaderText="Chaperio" />
                                            <asp:BoundField DataField="reparacionprevia" HeaderText="Reparación Previa" />
                                            <asp:BoundField DataField="observaciones" HeaderText="Observaciones" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <asp:Button ID="ButtonCancelPop" runat="server" Text="Cerrar" OnClick="ButtonCancelPop_Click" />
                            </asp:Panel>
                        </div>
                        <div></div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>

            </ajaxToolkit:TabContainer>
        </div>

        <%--<div>
            <asp:ImageButton ID="ImgButtonExportPDF" runat="server" ImageUrl="~/img/ico_pdf.jpg" OnClick="ImgButtonExportPdf_Click" />
        </div>--%>
        <div>
            <rsweb:ReportViewer ID="ReportViewerInsp" runat="server" BackColor="" ClientIDMode="AutoID" HighlightBackgroundColor="" InternalBorderColor="204, 204, 204" InternalBorderStyle="Solid" InternalBorderWidth="1px" LinkActiveColor="" LinkActiveHoverColor="" LinkDisabledColor="" PrimaryButtonBackgroundColor="" PrimaryButtonForegroundColor="" PrimaryButtonHoverBackgroundColor="" PrimaryButtonHoverForegroundColor="" SecondaryButtonBackgroundColor="" SecondaryButtonForegroundColor="" SecondaryButtonHoverBackgroundColor="" SecondaryButtonHoverForegroundColor="" SplitterBackColor="" ToolbarDividerColor="" ToolbarForegroundColor="" ToolbarForegroundDisabledColor="" ToolbarHoverBackgroundColor="" ToolbarHoverForegroundColor="" ToolBarItemBorderColor="" ToolBarItemBorderStyle="Solid" ToolBarItemBorderWidth="1px" ToolBarItemHoverBackColor="" ToolBarItemPressedBorderColor="51, 102, 153" ToolBarItemPressedBorderStyle="Solid" ToolBarItemPressedBorderWidth="1px" ToolBarItemPressedHoverBackColor="153, 187, 226" Width="923px" Visible="false">
                <LocalReport ReportPath="Reportes\RepFormularioInsp.rdlc">
                </LocalReport>
            </rsweb:ReportViewer>
        </div>
    </div>
</asp:Content>
