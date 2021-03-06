﻿<%@ Page Title="" Language="C#" MasterPageFile="~/SitioICRL.Master" AutoEventWireup="true" CodeBehind="Liquidacion.aspx.cs" Inherits="ICRL.Presentacion.Liquidacion"  MaintainScrollPositionOnPostback="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenidohead" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPaginas" runat="server">
  <script type="text/javascript">
    function ConfirmarEliminar(idf) {
      var resultado = confirm('Desea eliminar la factura ' + idf + ' ?');
      if (resultado) {
        return true;
      }
      else {
        return false;
      }
    }

    function ValidarFecha(sender, args) {
      var cadenaFecha = document.getElementById(sender.controltovalidate).value;
      var regex = /(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$/;
      if (regex.test(cadenaFecha)) {
        var parts = cadenaFecha.split("/");
        var dt = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
        args.IsValid = (dt.getDate() == parts[0] && dt.getMonth() + 1 == parts[1] && dt.getFullYear() == parts[2]);
      }
      else {
        args.IsValid = false;
      }
    }
  </script>

  <table class="basetable">
    <tr><th>Registro Liquidación</th></tr>
    <tr><td>
      <div class="twenty">
        Nro. Flujo<br />
        <asp:TextBox ID="txbNroFlujo" runat="server" Enabled="false"></asp:TextBox>
      </div>
      <div class="twenty">
        Cliente<br />
        <asp:TextBox ID="txbCliente" runat="server" Enabled="false"></asp:TextBox>
      </div>
      <div class="twenty">
        Teléfono<br />
        <asp:TextBox ID="txbTelefono" runat="server" Enabled="false"></asp:TextBox>
      </div>
      <div class="twenty">
        Nro. Reclamo<br />
        <asp:TextBox ID="txbReclamo" runat="server" Enabled="false"></asp:TextBox>
      </div>
      <div class="twenty">
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
            <asp:Label ID="lblNumeroFacturaEditar" runat="server" Text='<%# Eval("numero_factura") %>' Visible="false"></asp:Label>
            <asp:TextBox ID="txbNumeroFacturaEditar" runat="server" Text='<%# Eval("numero_factura") %>' ValidationGroup="ValidacionEditarFactura"></asp:TextBox><br />
            <asp:RequiredFieldValidator ID="rfvNumeroFacturaEditar" runat="server" ControlToValidate="txbNumeroFacturaEditar"
              Text="* Requerido" Display="Dynamic" CssClass="errormessage" ValidationGroup="ValidacionEditarFactura"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="revNumeroFacturaEditar" runat="server" ControlToValidate="txbNumeroFacturaEditar"
              ValidationExpression="^[0-9]*$" Text="* Solo números" Display="Dynamic" CssClass="errormessage"
              ValidationGroup="ValidacionEditarFactura"></asp:RegularExpressionValidator>
            <asp:CompareValidator ID="comNumeroFacturaEditar" runat="server" ControlToValidate="txbNumeroFacturaEditar" ValueToCompare="0" Type="Integer" Operator="GreaterThan"
              Text="* Valor inválido" Display="Dynamic" CssClass="errormessage" ValidationGroup="ValidacionEditarFactura"></asp:CompareValidator>
          </EditItemTemplate>
          <FooterTemplate>
            <asp:TextBox ID="txbNumeroFacturaNuevo" runat="server" Text=""></asp:TextBox><br />
            <asp:RequiredFieldValidator ID="rfvNumeroFacturaNuevo" runat="server" ControlToValidate="txbNumeroFacturaNuevo"
              Text="* Requerido" Display="Dynamic" CssClass="errormessage" ValidationGroup="ValidacionNuevaFactura"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="revNumeroFacturaNuevo" runat="server" ControlToValidate="txbNumeroFacturaNuevo"
              ValidationExpression="^[0-9]*$" Text="* Solo números" Display="Dynamic" ValidationGroup="ValidacionNuevaFactura"
              CssClass="errormessage"></asp:RegularExpressionValidator>
            <asp:CompareValidator ID="comNumeroFacturaNuevo" runat="server" ControlToValidate="txbNumeroFacturaNuevo" ValueToCompare="0" Type="Integer" Operator="GreaterThan"
              Text="* Valor inválido" Display="Dynamic" CssClass="errormessage" ValidationGroup="ValidacionNuevaFactura"></asp:CompareValidator>
          </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Fecha Emisión Factura">
          <ItemTemplate>
            <asp:Label ID="lblEmisionFactura" runat="server"
              Text='<%# string.IsNullOrEmpty(Eval("fecha_emision").ToString()) ? Eval("fecha_emision") : Convert.ToDateTime(Eval("fecha_emision").ToString()).ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) %>'></asp:Label>
          </ItemTemplate>
          <EditItemTemplate>
            <asp:TextBox ID="txbEmisionFacturaEditar" runat="server" AutoComplete="off" MaxLength="10" placeholder="dd/mm/aaaa" ValidationGroup="ValidacionEditarFactura"
              Text='<%# string.IsNullOrEmpty(Eval("fecha_emision").ToString()) ? Eval("fecha_emision") : Convert.ToDateTime(Eval("fecha_emision").ToString()).ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) %>'></asp:TextBox><br />
            <asp:RequiredFieldValidator ID="rfvEmisionFacturaEditar" runat="server" ControlToValidate="txbEmisionFacturaEditar"
              Text="* Requerido" Display="Dynamic" CssClass="errormessage" ValidationGroup="ValidacionEditarFactura"></asp:RequiredFieldValidator>
            <asp:CustomValidator ID="cuvEmisionFacturaEditar" runat="server" ControlToValidate="txbEmisionFacturaEditar" Text="* Fecha no válida"
              ClientValidationFunction="ValidarFecha" CssClass="errormessage" Display="Dynamic" ValidationGroup="ValidacionEditarFactura"></asp:CustomValidator>
            <ajaxToolkit:CalendarExtender ID="calEmisionFacturaEditar" runat="server" BehaviorID="calEmisionFacturaEditar" TargetControlID="txbEmisionFacturaEditar"
              DaysModeTitleFormat="dd/MM/yyyy" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy"></ajaxToolkit:CalendarExtender>
          </EditItemTemplate>
          <FooterTemplate>
            <asp:TextBox ID="txbEmisionFacturaNuevo" runat="server" AutoComplete="off" MaxLength="10" placeholder="dd/mm/aaaa"></asp:TextBox><br />
            <asp:RequiredFieldValidator ID="rfvEmisionFacturaNuevo" runat="server" ControlToValidate="txbEmisionFacturaNuevo"
              Text="* Requerido" Display="Dynamic" CssClass="errormessage" ValidationGroup="ValidacionNuevaFactura"></asp:RequiredFieldValidator>
            <asp:CustomValidator ID="cuvEmisionFacturaNuevo" runat="server" ControlToValidate="txbEmisionFacturaNuevo" Text="* Fecha no válida"
              ClientValidationFunction="ValidarFecha" CssClass="errormessage" Display="Dynamic" ValidationGroup="ValidacionNuevaFactura"></asp:CustomValidator>
            <ajaxToolkit:CalendarExtender ID="calEmisionFacturaNuevo" runat="server" BehaviorID="calEmisionFacturaNuevo" TargetControlID="txbEmisionFacturaNuevo"
              DaysModeTitleFormat="dd/MM/yyyy" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy"></ajaxToolkit:CalendarExtender>
          </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Fecha Recepción Factura">
          <ItemTemplate>
            <asp:Label ID="lblEntregaFactura" runat="server"
              Text='<%# string.IsNullOrEmpty(Eval("fecha_entrega").ToString()) ? Eval("fecha_entrega") : Convert.ToDateTime(Eval("fecha_entrega").ToString()).ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) %>'></asp:Label>
          </ItemTemplate>
          <EditItemTemplate>
            <asp:TextBox ID="txbEntregaFacturaEditar" runat="server" AutoComplete="off" MaxLength="10" placeholder="dd/mm/aaaa" ValidationGroup="ValidacionEditarFactura"
              Text='<%# string.IsNullOrEmpty(Eval("fecha_entrega").ToString()) ? Eval("fecha_entrega") : Convert.ToDateTime(Eval("fecha_entrega").ToString()).ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) %>'></asp:TextBox><br />
            <asp:RequiredFieldValidator ID="rfvEntregaFacturaEditar" runat="server" ControlToValidate="txbEntregaFacturaEditar"
              Text="* Requerido" Display="Dynamic" CssClass="errormessage" ValidationGroup="ValidacionEditarFactura"></asp:RequiredFieldValidator>
            <asp:CustomValidator ID="cuvEntregaFacturaEditar" runat="server" ControlToValidate="txbEntregaFacturaEditar" Text="* Fecha no válida"
              ClientValidationFunction="ValidarFecha" CssClass="errormessage" Display="Dynamic" ValidationGroup="ValidacionEditarFactura"></asp:CustomValidator>
            <ajaxToolkit:CalendarExtender ID="calEntregaFacturaEditar" runat="server" BehaviorID="calEntregaFacturaEditar" TargetControlID="txbEntregaFacturaEditar"
              DaysModeTitleFormat="dd/MM/yyyy" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy"></ajaxToolkit:CalendarExtender>
          </EditItemTemplate>
          <FooterTemplate>
            <asp:TextBox ID="txbEntregaFacturaNuevo" runat="server" AutoComplete="off" MaxLength="10" placeholder="dd/mm/aaaa"></asp:TextBox><br />
            <asp:RequiredFieldValidator ID="rfvEntregaFacturaNuevo" runat="server" ControlToValidate="txbEntregaFacturaNuevo"
              Text="* Requerido" Display="Dynamic" CssClass="errormessage" ValidationGroup="ValidacionNuevaFactura"></asp:RequiredFieldValidator>
            <asp:CustomValidator ID="cuvEntregaFacturaNuevo" runat="server" ControlToValidate="txbEntregaFacturaNuevo" Text="* Fecha no válida"
              ClientValidationFunction="ValidarFecha" CssClass="errormessage" Display="Dynamic" ValidationGroup="ValidacionNuevaFactura"></asp:CustomValidator>
            <ajaxToolkit:CalendarExtender ID="calEntregaFacturaNuevo" runat="server" BehaviorID="calEntregaFacturaNuevo" TargetControlID="txbEntregaFacturaNuevo"
              DaysModeTitleFormat="dd/MM/yyyy" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy"></ajaxToolkit:CalendarExtender>
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
        <AlternatingRowStyle BackColor="White" />
        <RowStyle CssClass="grid_row" />
        <SelectedRowStyle CssClass="grid_selected" />
        <EditRowStyle CssClass="grid_edit" />
        <HeaderStyle CssClass="grid_header" />
        <FooterStyle CssClass="grid_footer" />
        <PagerStyle CssClass="grid_pager" />
    </asp:GridView>
    </td></tr>
  </table>

  <table class="basetable">
    <tr><th>Datos Cotización</th></tr>
    <tr><td>
      <div class="twentyfive">
        Tipo de Cambio 
        <asp:RequiredFieldValidator ID="rfvTipoCambio" runat="server" ControlToValidate="txbTipoCambio"
          Text="* Requerido" Display="Dynamic" CssClass="errormessage" ValidationGroup="ValidacionTipoCambio"></asp:RequiredFieldValidator>
        <asp:CompareValidator ID="covTipoCambio" runat="server" ControlToValidate="txbTipoCambio" ValidationGroup="ValidacionTipoCambio"
          Type="Double" Operator="DataTypeCheck" Text="* Valor inválido" Display="Dynamic" CssClass="errormessage"></asp:CompareValidator><br />
        <asp:TextBox ID="txbTipoCambio" runat="server" MaxLength="5" Text="6.96"></asp:TextBox>
      </div>
    </td></tr>
    <tr><td>
      <asp:GridView ID="GridViewDatosOrden" runat="server" AutoGenerateColumns="False" ShowFooter="True" Width="100%"
        OnRowDataBound="GridViewDatosOrden_RowDataBound" CellPadding="4" GridLines="None">
        <Columns>
          <asp:BoundField DataField="numero_orden" HeaderText="Orden" />
          <asp:BoundField DataField="proveedor" HeaderText="T/P/B" />
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
          <%--<asp:BoundField DataField="fecha_recepcion" HeaderText="Fecha Insp." DataFormatString="{0:dd-MM-yyyy}" />--%>
          <asp:TemplateField HeaderText="Fecha Insp.">
            <ItemTemplate>
              <asp:Label ID="lblFechaOrden" runat="server"
                Text='<%# (Eval("fecha_recepcion") != null && !string.IsNullOrWhiteSpace(Eval("fecha_recepcion").ToString())) ?
                  Convert.ToDateTime(Eval("fecha_recepcion").ToString()).ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) :
                  string.Empty %>'></asp:Label>
            </ItemTemplate>
          </asp:TemplateField>
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
              <asp:Label ID="lblFechaLiquidacion" runat="server"
                Text='<%# (Eval("fecha_liquidacion") != null && !string.IsNullOrWhiteSpace(Eval("fecha_liquidacion").ToString())) ?
                  Convert.ToDateTime(Eval("fecha_liquidacion").ToString()).ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) :
                  string.Empty %>'></asp:Label>
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
        <AlternatingRowStyle BackColor="White" />
        <RowStyle CssClass="grid_row" />
        <SelectedRowStyle CssClass="grid_selected" />
        <EditRowStyle CssClass="grid_edit" />
        <HeaderStyle CssClass="grid_header" />
        <FooterStyle CssClass="grid_footer" />
        <PagerStyle CssClass="grid_pager" />
      </asp:GridView>
    </td></tr>
    <tr><td>
      <asp:Button ID="btnGenerarLiquidacion" runat="server" Text="Generar Liquidación" ValidationGroup="ValidacionTipoCambio" OnClick="btnGenerarLiquidacion_Click" />
    </td></tr>
  </table>
  
  <table class="basetable">
    <tr><th>Datos Liquidación</th></tr>
    <tr><td>
      <asp:GridView ID="GridViewDatosLiquidacion" runat="server" AutoGenerateColumns="False" ShowFooter="True" Width="100%"
        OnRowDataBound="GridViewDatosLiquidacion_RowDataBound" CellPadding="4" GridLines="None">
      <Columns>
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
        <asp:TemplateField HeaderText="Fecha Recep. Fact.">
          <ItemTemplate>
            <asp:Label ID="lblFechaRecepcion" runat="server" Text='<%# Eval("fecharec") %>'></asp:Label>
          </ItemTemplate>
        </asp:TemplateField>
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
      <AlternatingRowStyle BackColor="White" />
      <RowStyle CssClass="grid_row" />
      <SelectedRowStyle CssClass="grid_selected" />
      <EditRowStyle CssClass="grid_edit" />
      <HeaderStyle CssClass="grid_header" />
      <FooterStyle CssClass="grid_footer" />
      <PagerStyle CssClass="grid_pager" />
    </asp:GridView>
    </td></tr>
    <tr><td>
      <table style="width:100%;font-size:inherit">
        <tr>
          <td>
            <asp:Button ID="btnAjusteMenor" runat="server" Text="Ajuste Menor" Enabled="false" OnClick="btnAjusteMenor_Click" />
            <asp:Button ID="btnGuardarLiquidacion" runat="server" Text="Guardar Liquidación" Enabled="false" OnClick="btnGuardarLiquidacion_Click" />
          </td>
          <td style="text-align:right">
            <asp:Button ID="btnLiquidacionTotal" runat="server" Text="Liquidación Total" Enabled="false" ValidationGroup="ValidacionTipoCambio" OnClick="btnLiquidacionTotal_Click" />
          </td>
        </tr>
      </table>
    </td></tr>
  </table>

  <asp:Label ID="LabelMensaje" CssClass="LabelMensaje" runat="server" Text="" Visible="false"></asp:Label>
</asp:Content>
