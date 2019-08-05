<%@ Page Title="" Language="C#" MasterPageFile="~/SitioICRL.Master" AutoEventWireup="true" CodeBehind="CotizacionDPRP.aspx.cs" Inherits="ICRL.Presentacion.CotizacionDPRP" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenidohead" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPaginas" runat="server">
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
                            <asp:TextBox ID="TextBoxNroCotizacion" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                        </div>
                        <div class="twentyfive">
                            <asp:Label ID="LabelNroCotizacion" runat="server" Text="Nro. de Cotización"></asp:Label><br />
                            <asp:TextBox ID="TextBoxCorrelativo" runat="server"></asp:TextBox>
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
    <table class="basetable">
        <tr>
            <th>
                <asp:Label ID="LabelReparaciones" runat="server" Text="Reparaciones"></asp:Label>
            </th>
        </tr>
    </table>
    <div>
        <asp:GridView ID="GridViewReparaciones" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
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
    <table class="basetable">
        <tr>
            <th>
                <asp:Label ID="LabelRepuestos" runat="server" Text="Repuestos"></asp:Label>
            </th>
        </tr>
    </table>
    <div>
        <asp:GridView ID="GridViewRepuestos" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
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
</asp:Content>
