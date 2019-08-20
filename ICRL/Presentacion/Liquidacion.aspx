<%@ Page Title="" Language="C#" MasterPageFile="~/SitioICRL.Master" AutoEventWireup="true" CodeBehind="Liquidacion.aspx.cs" Inherits="ICRL.Presentacion.Liquidacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenidohead" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPaginas" runat="server">
  <script type="text/javascript">
    function deleteConfirm(pubid) {
      var result = confirm('Desea eliminar la factura ' + pubid + ' ?');
      if (result) {
        return true;
      }
      else {
        return false;
      }
    }
  </script>

  <table class="basetable">
    <tr><th>Registro Liquidación</th></tr>
    <tr><td>
      <div class="twentyfive">
        Nro. Flujo<br />
        <asp:TextBox ID="txbNroFlujo" runat="server" Enabled="false"></asp:TextBox>
      </div>
    </td></tr>
    <tr><td><strong>Datos Generales</strong></td></tr>
    <tr><td>
      <div class="twentyfive">
        Cliente<br />
        <asp:TextBox ID="txbCliente" runat="server" Enabled="false"></asp:TextBox>
      </div>
      <div class="twentyfive">
        Teléfono<br />
        <asp:TextBox ID="txbTelefono" runat="server" Enabled="false"></asp:TextBox>
      </div>
      <div class="twentyfive">
        Nro. Reclamo<br />
        <asp:TextBox ID="txbReclamo" runat="server" Enabled="false"></asp:TextBox>
      </div>
      <div class="twentyfive">
        Póliza<br />
        <asp:TextBox ID="txbPoliza" runat="server" Enabled="false"></asp:TextBox>
      </div>
    </td></tr>
  </table>

  <table class="basetable">
    <tr><th>Datos Facturas</th></tr>
    <tr><td>
      <asp:GridView ID="GridViewDatosFactura" runat="server" AutoGenerateColumns="False" ShowFooter="True" DataKeyNames="id_factura" Width="100%"
      OnRowCancelingEdit="GridViewDatosFactura_RowCancelingEdit"
      OnRowDeleting="GridViewDatosFactura_RowDeleting"
      OnRowEditing="GridViewDatosFactura_RowEditing"
      OnRowUpdating="GridViewDatosFactura_RowUpdating"
      OnRowCommand="GridViewDatosFactura_RowCommand"
      OnRowDataBound="GridViewDatosFactura_RowDataBound" CellPadding="4" ForeColor="#333333" GridLines="None">
        <AlternatingRowStyle BackColor="White" />
      <Columns>
        <asp:TemplateField HeaderText="ID">
          <ItemTemplate>
            <asp:Label ID="lblIdFactura" runat="server" Text='<%# Eval("id_factura") %>'></asp:Label>
          </ItemTemplate>
          <EditItemTemplate>
            <asp:Label ID="lblIdFacturaEditar" runat="server" Text='<%# Eval("id_factura") %>'></asp:Label>
          </EditItemTemplate>
          <FooterTemplate>
            <asp:Label ID="lblIdFactNuevo" runat="server" Text=""></asp:Label>
          </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Número Factura">
          <ItemTemplate>
            <asp:Label ID="lblNumeroFactura" runat="server" Text='<%# Eval("numero_factura") %>'></asp:Label>
          </ItemTemplate>
          <EditItemTemplate>
            <asp:TextBox ID="txbNumeroFacturaEditar" runat="server" Text='<%# Eval("numero_factura") %>' ValidationGroup="ValidacionEditarFactura"></asp:TextBox><br />
            <asp:RequiredFieldValidator ID="rfvNumeroFacturaEditar" runat="server" ControlToValidate="txbNumeroFacturaEditar"
              Text="* Requerido" Display="Dynamic" CssClass="errormessage" ValidationGroup="ValidacionEditarFactura"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="revNumeroFacturaEditar" runat="server" ControlToValidate="txbNumeroFacturaEditar"
              ValidationExpression="^[0-9]*$" Text="* Solo números" Display="Dynamic" CssClass="errormessage"
              ValidationGroup="ValidacionEditarFactura"></asp:RegularExpressionValidator>
          </EditItemTemplate>
          <FooterTemplate>
            <asp:TextBox ID="txbNumeroFacturaNuevo" runat="server" Text=""></asp:TextBox><br />
            <asp:RequiredFieldValidator ID="rfvNumeroFacturaNuevo" runat="server" ControlToValidate="txbNumeroFacturaNuevo"
              Text="* Requerido" Display="Dynamic" CssClass="errormessage" ValidationGroup="ValidacionNuevaFactura"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="revNumeroFacturaNuevo" runat="server" ControlToValidate="txbNumeroFacturaNuevo"
              ValidationExpression="^[0-9]*$" Text="* Solo números" Display="Dynamic" ValidationGroup="ValidacionNuevaFactura"
              CssClass="errormessage"></asp:RegularExpressionValidator>
          </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Fecha Emisión Factura">
          <ItemTemplate>
            <asp:Label ID="lblEmisionFactura" runat="server" Text='<%# Eval("fecha_emision", "{0:dd-MM-yyyy}") %>'></asp:Label>
          </ItemTemplate>
          <EditItemTemplate>
            <asp:TextBox ID="txbEmisionFacturaEditar" runat="server" Text='<%# Eval("fecha_emision", "{0:dd-MM-yyyy}") %>' ValidationGroup="ValidacionEditarFactura" MaxLength="10"></asp:TextBox><br />
            <asp:RequiredFieldValidator ID="rfvEmisionFacturaEditar" runat="server" ControlToValidate="txbEmisionFacturaEditar"
              Text="* Requerido" Display="Dynamic" CssClass="errormessage" ValidationGroup="ValidacionEditarFactura"></asp:RequiredFieldValidator>
            <asp:CompareValidator ID="covEmisionFacturaEditar" runat="server" ControlToValidate="txbEmisionFacturaEditar"
              Type="Date" Operator="DataTypeCheck" Text="* Fecha no válida" Display="Dynamic" CssClass="errormessage"
              ValidationGroup="ValidacionEditarFactura"></asp:CompareValidator>
          </EditItemTemplate>
          <FooterTemplate>
            <asp:TextBox ID="txbEmisionFacturaNuevo" runat="server"></asp:TextBox><br />
            <asp:RequiredFieldValidator ID="rfvEmisionFacturaNuevo" runat="server" ControlToValidate="txbEmisionFacturaNuevo"
              Text="* Requerido" Display="Dynamic" CssClass="errormessage" ValidationGroup="ValidacionNuevaFactura"></asp:RequiredFieldValidator>
            <asp:CompareValidator ID="covEmisionFacturaNuevo" runat="server" ControlToValidate="txbEmisionFacturaNuevo"
              Type="Date" Operator="DataTypeCheck" Text="* Fecha no válida" Display="Dynamic" ValidationGroup="ValidacionNuevaFactura"
              CssClass="errormessage"></asp:CompareValidator>
          </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Fecha Recepción Factura">
          <ItemTemplate>
            <asp:Label ID="lblEntregaFactura" runat="server" Text='<%# Eval("fecha_entrega", "{0:dd-MM-yyyy}") %>'></asp:Label>
          </ItemTemplate>
          <EditItemTemplate>
            <asp:TextBox ID="txbEntregaFacturaEditar" runat="server" Text='<%# Eval("fecha_entrega", "{0:dd-MM-yyyy}") %>' MaxLength="10" ValidationGroup="ValidacionEditarFactura"></asp:TextBox><br />
            <asp:RequiredFieldValidator ID="rfvEntregaFacturaEditar" runat="server" ControlToValidate="txbEntregaFacturaEditar"
              Text="* Requerido" Display="Dynamic" CssClass="errormessage" ValidationGroup="ValidacionEditarFactura"></asp:RequiredFieldValidator>
            <asp:CompareValidator ID="covEntregaFacturaEditar" runat="server" ControlToValidate="txbEntregaFacturaEditar"
              Type="Date" Operator="DataTypeCheck" Text="* Fecha no válida" Display="Dynamic" CssClass="errormessage"
              ValidationGroup="ValidacionEditarFactura"></asp:CompareValidator>
          </EditItemTemplate>
          <FooterTemplate>
            <asp:TextBox ID="txbEntregaFacturaNuevo" runat="server"></asp:TextBox><br />
            <asp:RequiredFieldValidator ID="rfvEntregaFacturaNuevo" runat="server" ControlToValidate="txbEntregaFacturaNuevo"
              Text="* Requerido" Display="Dynamic" CssClass="errormessage" ValidationGroup="ValidacionNuevaFactura"></asp:RequiredFieldValidator>
            <asp:CompareValidator ID="covEntregaFacturaNuevo" runat="server" ControlToValidate="txbEntregaFacturaNuevo"
              Type="Date" Operator="DataTypeCheck" Text="* Fecha no válida" Display="Dynamic" ValidationGroup="ValidacionNuevaFactura"
              CssClass="errormessage"></asp:CompareValidator>
          </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Monto Factura" ItemStyle-CssClass="price" HeaderStyle-CssClass="price">
          <ItemTemplate>
            <asp:Label ID="lblMontoFactura" runat="server" Text='<%# Eval("monto", "{0:N}") %>'></asp:Label>
          </ItemTemplate>
          <EditItemTemplate>
            <asp:TextBox ID="txbMontoFacturaEditar" runat="server" Text='<%# Eval("monto") %>' ValidationGroup="ValidacionEditarFactura"></asp:TextBox><br />
            <asp:RequiredFieldValidator ID="rfvMontoFacturaEditar" runat="server" ControlToValidate="txbMontoFacturaEditar"
              Text="* Requerido" Display="Dynamic" CssClass="errormessage" ValidationGroup="ValidacionEditarFactura"></asp:RequiredFieldValidator>
            <asp:CompareValidator ID="covMontoFacturaEditar" runat="server" ControlToValidate="txbMontoFacturaEditar"
              Type="Double" Operator="DataTypeCheck" Text="* Monto inválido" Display="Dynamic" CssClass="errormessage"
              ValidationGroup="ValidacionEditarFactura"></asp:CompareValidator>
            <%--<asp:RegularExpressionValidator ID="revNumeroFacturaEditar" runat="server" ControlToValidate="txbNumeroFacturaEditar"
              ValidationExpression="^[0-9]*$" Text="* Solo números" Display="Dynamic"></asp:RegularExpressionValidator>--%>
          </EditItemTemplate>
          <FooterTemplate>
            <asp:TextBox ID="txbMontoFacturaNuevo" runat="server"></asp:TextBox><br />
            <asp:RequiredFieldValidator ID="rfvMontoFacturaNuevo" runat="server" ControlToValidate="txbMontoFacturaNuevo"
              Text="* Requerido" Display="Dynamic" CssClass="errormessage" ValidationGroup="ValidacionNuevaFactura"></asp:RequiredFieldValidator>
            <asp:CompareValidator ID="covMontoFacturaNuevo" runat="server" ControlToValidate="txbMontoFacturaNuevo"
              Type="Double" Operator="DataTypeCheck" Text="* Monto inválido" Display="Dynamic" ValidationGroup="ValidacionNuevaFactura"
              CssClass="errormessage"></asp:CompareValidator>
          </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Moneda Factura">
          <ItemTemplate>
            <asp:Label ID="lblMonedaFactura" runat="server" Text='<%# VerTextoMoneda(Eval("id_moneda")) %>'></asp:Label>
            <asp:Label ID="lblIdMonedaFactura" runat="server" Text='<%# Eval("id_moneda") %>' Visible="false"></asp:Label>
          </ItemTemplate>
          <EditItemTemplate>
            <asp:DropDownList ID="ddlMonedaFacturaEditar" runat="server" SelectedValue='<%# Eval("id_moneda") %>'>
              <asp:ListItem Text="Bs." Value="1"></asp:ListItem>
              <asp:ListItem Text="Us." Value="2"></asp:ListItem>
            </asp:DropDownList>
          </EditItemTemplate>
          <FooterTemplate>
            <asp:DropDownList ID="ddlMonedaFacturaNuevo" runat="server">
              <asp:ListItem Text="Bs." Value="1"></asp:ListItem>
              <asp:ListItem Text="Us." Value="2"></asp:ListItem>
            </asp:DropDownList>
          </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField Visible="false">
          <ItemTemplate>
            <asp:Label ID="lblObservaciones" runat="server" Text='<%# Eval("observaciones") %>'></asp:Label>
          </ItemTemplate>
        </asp:TemplateField>
        <%--<asp:BoundField DataField="observaciones" Visible="false" />--%>
        <asp:TemplateField>
          <ItemTemplate>
            <asp:LinkButton ID="btnEditar" runat="server" CommandName="Edit" Text="Editar" />
            <asp:LinkButton ID="btnEliminar" runat="server" CommandName="Delete" Text="Eliminar" />
          </ItemTemplate>
          <EditItemTemplate>
            <asp:LinkButton ID="btnActualizar" runat="server" CommandName="Update" Text="Actualizar" ValidationGroup="ValidacionEditarFactura" />
            <asp:LinkButton ID="btnCancelar" runat="server" CommandName="Cancel" Text="Cancelar" />
          </EditItemTemplate>
          <FooterTemplate>
            <asp:Button ID="btnNueva" runat="server" CommandName="AddNew" Text="Nueva Factura" ValidationGroup="ValidacionNuevaFactura" />
          </FooterTemplate>
        </asp:TemplateField>
      </Columns>
        <EditRowStyle BackColor="#e6fff4" />
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
    </td></tr>
  </table>

  <table class="basetable">
    <tr><th>Datos Cotización</th></tr>
    <tr><td>
      <div class="twentyfive">
        Ingrese el "Tipo de Cambio" a usar:
        <asp:RequiredFieldValidator ID="rfvTipoCambio" runat="server" ControlToValidate="txbTipoCambio"
          Text="* Requerido" Display="Dynamic" CssClass="errormessage" ValidationGroup="ValidacionTipoCambio"></asp:RequiredFieldValidator>
        <asp:CompareValidator ID="covTipoCambio" runat="server" ControlToValidate="txbTipoCambio" ValidationGroup="ValidacionTipoCambio"
          Type="Double" Operator="DataTypeCheck" Text="* Valor inválido" Display="Dynamic" CssClass="errormessage"></asp:CompareValidator><br />
        <asp:TextBox ID="txbTipoCambio" runat="server" MaxLength="5"></asp:TextBox>
      </div>
    </td></tr>
    <tr><td>
      <asp:GridView ID="GridViewDatosOrden" runat="server" AutoGenerateColumns="False" ShowFooter="True" Width="100%"
        OnRowDataBound="GridViewDatosOrden_RowDataBound" CellPadding="4" ForeColor="#333333" GridLines="None">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
          <asp:BoundField DataField="numero_orden" HeaderText="Orden" />
          <asp:BoundField DataField="proveedor" HeaderText="T/P/B" />
          <%--<asp:BoundField DataField="item_descripcion" HeaderText="Item" />--%>
          <asp:TemplateField HeaderText="Item">
            <ItemTemplate>
              <asp:Label ID="lblDescripcion" runat="server" Text='<%# Eval("item_descripcion") %>'></asp:Label>
            </ItemTemplate>
            <FooterTemplate>
              <strong>TOTAL COTIZADO</strong>
            </FooterTemplate>
          </asp:TemplateField>
          <asp:TemplateField HeaderText="Precio Bs." ItemStyle-CssClass="price" FooterStyle-CssClass="price" HeaderStyle-CssClass="price">
            <ItemTemplate>
              <asp:Label ID="lblCotizacionBs" runat="server" Text='<%# Eval("preciobs", "{0:N}") %>'></asp:Label>
            </ItemTemplate>
            <FooterTemplate>
              <asp:Label ID="lblTotalCotizacionBs" runat="server"></asp:Label>
            </FooterTemplate>
          </asp:TemplateField>
          <asp:TemplateField HeaderText="Precio Us." ItemStyle-CssClass="price" FooterStyle-CssClass="price" HeaderStyle-CssClass="price">
            <ItemTemplate>
              <asp:Label ID="lblCotizacionUs" runat="server" Text='<%# Eval("precious", "{0:N}") %>'></asp:Label>
            </ItemTemplate>
            <FooterTemplate>
              <asp:Label ID="lblTotalCotizacionUs" runat="server"></asp:Label>
            </FooterTemplate>
          </asp:TemplateField>
          <asp:BoundField DataField="fecha_recepcion" HeaderText="Fecha Insp." DataFormatString="{0:dd-MM-yyyy}" />
          <asp:TemplateField HeaderText="Insp.">
            <ItemTemplate>
              <asp:CheckBox ID="cbxInspeccion" runat="server" Checked='<%# Eval("inspeccion") %>'></asp:CheckBox>
            </ItemTemplate>
          </asp:TemplateField>
          <asp:TemplateField HeaderText="Liq.">
            <ItemTemplate>
              <asp:CheckBox ID="cbxLiquidacion" runat="server" Checked='<%# Eval("liquidacion") %>'></asp:CheckBox>
            </ItemTemplate>
          </asp:TemplateField>
          <asp:TemplateField HeaderText="Factura Liq.">
            <ItemTemplate>
              <asp:DropDownList ID="ddlFacturasLiquidadas" runat="server"></asp:DropDownList>
            </ItemTemplate>
          </asp:TemplateField>
          <asp:TemplateField HeaderText="Fecha Liq.">
            <ItemTemplate>
              <asp:Label ID="lblFechaLiquidacion" runat="server" Text='<%# Eval("fecha_liquidacion", "{0:dd-MM-yyyy}") %>'></asp:Label>
            </ItemTemplate>
          </asp:TemplateField>
          <asp:TemplateField Visible="false">
            <ItemTemplate>
              <asp:Label ID="id_estado" runat="server" Text='<%# Eval("id_estado") %>'></asp:Label>
              <asp:Label ID="id_cotizacion" runat="server" Text='<%# Eval("id_cotizacion") %>'></asp:Label>
              <asp:Label ID="tipo_origen" runat="server" Text='<%# Eval("tipo_origen") %>'></asp:Label>
              <asp:Label ID="id_item" runat="server" Text='<%# Eval("id_item") %>'></asp:Label>
              <asp:Label ID="id_tipo_item" runat="server" Text='<%# Eval("id_tipo_item") %>'></asp:Label>
              <asp:Label ID="fecha_orden" runat="server" Text='<%# Eval("fecha_orden") %>'></asp:Label>
            </ItemTemplate>
          </asp:TemplateField>
          <asp:TemplateField Visible="false">
            <ItemTemplate>
              <asp:Label ID="lblNumFactura" runat="server" Text='<%# Eval("num_factura") %>'></asp:Label>
            </ItemTemplate>
          </asp:TemplateField>
        </Columns>
        <EditRowStyle BackColor="#e6fff4" />
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
    </td></tr>
    <tr><td>
      <asp:Button ID="btnGenerarLiquidacion" runat="server" Text="Generar Liquidación" ValidationGroup="ValidacionTipoCambio" OnClick="btnGenerarLiquidacion_Click" />
      <asp:Button ID="btnLiquidacionTotal" runat="server" Text="Liquidación Total" ValidationGroup="ValidacionTipoCambio" />
    </td></tr>
  </table>
  
  <table class="basetable">
    <tr><th>Datos Liquidación</th></tr>
    <tr><td>
      <asp:GridView ID="GridViewDatosLiquidacion" runat="server" AutoGenerateColumns="False" ShowFooter="True" Width="100%"
        OnRowDataBound="GridViewDatosLiquidacion_RowDataBound" CellPadding="4" ForeColor="#333333" GridLines="None">
      <AlternatingRowStyle BackColor="White" />
      <Columns>
        <%--<asp:BoundField DataField="orden" HeaderText="Orden" />--%>
        <asp:TemplateField HeaderText="Orden">
          <ItemTemplate>
            <asp:Label ID="lblOrden" runat="server" Text='<%# Eval("orden") %>'></asp:Label>
          </ItemTemplate>
          <FooterTemplate>
            <strong>TOTAL PAGADO</strong><br /><strong>SALDO COTIZADO</strong>
          </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Precio Liq. Bs." ItemStyle-CssClass="price" FooterStyle-CssClass="price" HeaderStyle-CssClass="price">
          <ItemTemplate>
            <asp:Label ID="lblLiquidacionBs" runat="server" Text='<%# Eval("preciobs") %>'></asp:Label>
          </ItemTemplate>
          <FooterTemplate>
            <asp:Label ID="lblTotalLiquidacionBs" runat="server"></asp:Label><br />
            <asp:Label ID="lblSaldoLiquidacionBs" runat="server"></asp:Label>
          </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Precio Liq. Us." ItemStyle-CssClass="price" FooterStyle-CssClass="price" HeaderStyle-CssClass="price">
          <ItemTemplate>
            <asp:Label ID="lblLiquidacionUs" runat="server" Text='<%# Eval("precious") %>'></asp:Label>
          </ItemTemplate>
          <FooterTemplate>
            <asp:Label ID="lblTotalLiquidacionUs" runat="server"></asp:Label><br />
            <asp:Label ID="lblSaldoLiquidacionUs" runat="server"></asp:Label>
          </FooterTemplate>
        </asp:TemplateField>
        <%--<asp:BoundField DataField="fecharec" HeaderText="Fecha Recep. Fact." />--%>
        <asp:TemplateField HeaderText="Fecha Recep. Fact.">
          <ItemTemplate>
            <asp:Label ID="lblFechaRecepcion" runat="server" Text='<%# Eval("fecharec") %>'></asp:Label>
          </ItemTemplate>
        </asp:TemplateField>
        <%--<asp:BoundField DataField="numero" HeaderText="Núm. Factura" />--%>
        <asp:TemplateField HeaderText="Núm. Factura">
          <ItemTemplate>
            <asp:Label ID="lblNumero" runat="server" Text='<%# Eval("numero") %>'></asp:Label>
          </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Observaciones">
          <ItemTemplate>
            <asp:TextBox ID="txbObservaciones" runat="server" Text='<%# Eval("observaciones") %>'></asp:TextBox>
          </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField Visible="false">
          <ItemTemplate>
            <asp:Label ID="lblMoneda" runat="server" Text='<%# Eval ("moneda") %>'></asp:Label>
            <asp:Label ID="lblFechaEmision" runat="server" Text='<%# Eval ("fechaemi") %>'></asp:Label>
            <asp:Label ID="lblId" runat="server" Text='<%# Eval ("id") %>'></asp:Label>
          </ItemTemplate>
        </asp:TemplateField>
      </Columns>
      <EditRowStyle BackColor="#e6fff4" />
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
    </td></tr>
    <tr><td>
      <asp:Button ID="btnAjusteMenor" runat="server" Text="Ajuste Menor" Enabled="false" OnClick="btnAjusteMenor_Click" />
      <asp:Button ID="btnGuardarLiquidacion" runat="server" Text="Guardar Liquidación" Enabled="false" OnClick="btnGuardarLiquidacion_Click" />
    </td></tr>
  </table>

  <asp:Label ID="LabelMensaje" CssClass="LabelMensaje" runat="server" Text="" Visible="false"></asp:Label>
</asp:Content>
