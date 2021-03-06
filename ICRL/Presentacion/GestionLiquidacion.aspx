﻿<%@ Page Title="" Language="C#" MasterPageFile="~/SitioICRL.Master" AutoEventWireup="true" CodeBehind="GestionLiquidacion.aspx.cs" Inherits="ICRL.Presentacion.GestionLiquidacion"  MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenidohead" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPaginas" runat="server">
  <script type="text/javascript">
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
    <tr><th>Búsqueda Liquidación</th></tr>
    <tr><td>
      <div class="twentyfive">
        Flujo On-Base<br />
        <asp:TextBox ID="txbFlujoOnbase" runat="server"></asp:TextBox>
      </div>
      <div class="twentyfive">
        Proveedor<br />
        <asp:TextBox ID="txbProveedor" runat="server"></asp:TextBox>
      </div>
      <div class="twentyfive">
        Placa<br />
        <asp:TextBox ID="txbPlaca" runat="server"></asp:TextBox>
      </div>
      <div class="twentyfive">
        Sucursal<br />
        <asp:TextBox ID="txbSucursal" runat="server"></asp:TextBox>
      </div>
    </td></tr>
    <tr><td>
      <div class="twentyfive">
        Estado<br />
        <asp:DropDownList ID="ddlEstado" runat="server"></asp:DropDownList>
      </div>
      <div class="twentyfive">
        Fecha Liq. Desde
        <asp:RequiredFieldValidator ID="rfvFechaDesde" runat="server" ControlToValidate="txbFechaDesde"
          Text="* Requerido" Display="Dynamic" CssClass="errormessage"></asp:RequiredFieldValidator>
        <asp:CustomValidator ID="cuvFechaDesde" runat="server" ControlToValidate="txbFechaDesde" Text="* Fecha no válida"
          ClientValidationFunction="ValidarFecha" CssClass="errormessage" Display="Dynamic"></asp:CustomValidator><br />
        <asp:TextBox ID="txbFechaDesde" runat="server" AutoComplete="off" MaxLength="10" placeholder="dd/mm/aaaa"></asp:TextBox>
        <ajaxToolkit:CalendarExtender ID="calFechaDesde" runat="server" BehaviorID="calFechaDesde" TargetControlID="txbFechaDesde"
          DaysModeTitleFormat="dd/MM/yyyy" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy"></ajaxToolkit:CalendarExtender>
      </div>
      <div class="twentyfive">
        Fecha Liq. Hasta
        <asp:RequiredFieldValidator ID="rfvFechaHasta" runat="server" ControlToValidate="txbFechaHasta"
          Text="* Requerido" Display="Dynamic" CssClass="errormessage"></asp:RequiredFieldValidator>
        <asp:CustomValidator ID="cuvFechaHasta0" runat="server" ControlToValidate="txbFechaHasta" Text="* Fecha no válida"
          Display="Dynamic" ClientValidationFunction="ValidarFecha" CssClass="errormessage"></asp:CustomValidator>
        <asp:CustomValidator ID="cuvFechaHasta" runat="server" ControlToValidate="txbFechaHasta" Display="Dynamic"
          CssClass="errormessage" Text="* Rango de fechas inválido" OnServerValidate="cuvFechaHasta_ServerValidate"></asp:CustomValidator><br />
        <asp:TextBox ID="txbFechaHasta" runat="server" AutoComplete="off" MaxLength="10" placeholder="dd/mm/aaaa"></asp:TextBox>
        <ajaxToolkit:CalendarExtender ID="calFechaHasta" runat="server" BehaviorID="calFechaHasta" TargetControlID="txbFechaHasta"
          DaysModeTitleFormat="dd/MM/yyyy" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy"></ajaxToolkit:CalendarExtender>
      </div>
    </td></tr>
    <tr><td>
      <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
    </td></tr>
  </table>

  <table class="basetable">
    <tr><th>Órdenes de Pago</th></tr>
    <tr><td>
      <asp:GridView ID="GridViewOrdenesPago" runat="server" AutoGenerateColumns="False" Width="100%" CellPadding="4"
        GridLines="None" AllowPaging="true" PageSize="25" OnPageIndexChanging="GridViewOrdenesPago_PageIndexChanging">
        <Columns>
          <asp:BoundField DataField="numero_orden" HeaderText="Orden" />
          <asp:BoundField DataField="flujoOnBase" HeaderText="Flujo OnBase" />
          <asp:BoundField DataField="nombreAsegurado" HeaderText="Cliente" />
          <asp:BoundField DataField="placaVehiculo" HeaderText="Placa" />
          <asp:BoundField DataField="proveedor" HeaderText="Proveedor / Beneficiario" />
          <%--<asp:BoundField DataField="fecha_orden" HeaderText="Fecha Orden" DataFormatString="{0:dd/MM/yyyy}"/>--%>
          <asp:TemplateField HeaderText="Fecha Orden">
            <ItemTemplate>
              <asp:Label ID="lblFechaOrden" runat="server"
                Text='<%# Convert.ToDateTime(Eval("fecha_orden").ToString()).ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) %>'></asp:Label>
            </ItemTemplate>
          </asp:TemplateField>
          <asp:BoundField DataField="Total" HeaderText="Total Orden" DataFormatString="{0:N}" ItemStyle-CssClass="price" HeaderStyle-CssClass="price" />
          <asp:BoundField DataField="sumabspagado" HeaderText="Pagado Bs." DataFormatString="{0:N}" ItemStyle-CssClass="price" HeaderStyle-CssClass="price" />
          <asp:BoundField DataField="sumabsnopagado" HeaderText="No Pagado Bs." DataFormatString="{0:N}" ItemStyle-CssClass="price" HeaderStyle-CssClass="price" />
          <asp:BoundField DataField="sumauspagado" HeaderText="Pagado Us." DataFormatString="{0:N}" ItemStyle-CssClass="price" HeaderStyle-CssClass="price" />
          <asp:BoundField DataField="sumausnopagado" HeaderText="No Pagado Us." DataFormatString="{0:N}" ItemStyle-CssClass="price" HeaderStyle-CssClass="price" />
          <asp:TemplateField HeaderText="Estado">
            <ItemTemplate>
              <asp:Label ID="lblEstado" runat="server" Text='<%# VerTextoEstado(Eval("id_estado")) %>'></asp:Label>
            </ItemTemplate>
          </asp:TemplateField>
          <asp:HyperLinkField DataNavigateUrlFields="idFlujo" DataNavigateUrlFormatString="~\Presentacion\Liquidacion.aspx?idflujo={0}" Text="Ver" />
        </Columns>
        <AlternatingRowStyle BackColor="White" />
        <RowStyle CssClass="grid_row" />
        <SelectedRowStyle CssClass="grid_selected" />
        <EditRowStyle CssClass="grid_edit" />
        <HeaderStyle CssClass="grid_header" />
        <FooterStyle CssClass="grid_footer" />
        <PagerStyle CssClass="grid_pager" />
      </asp:GridView>
      <asp:Label ID="lblMensajeOrdenesPago" runat="server" Text="Ingrese valores en el formulario para recuperar la información solicitada."></asp:Label>
    </td></tr>
    <tr><td>
      <asp:Button ID="btnGenerarReporteExcel" runat="server" Text="Exportar a EXCEL" OnClick="btnGenerarReporteExcel_Click" Enabled="false" CssClass="xls" />
      <asp:Button ID="btnGenerarReportePdf" runat="server" Text="Exportar a PDF" OnClick="btnGenerarReportePdf_Click" Enabled="false" CssClass="pdf" />
      <rsweb:ReportViewer ID="ReportViewerLiquidacion" runat="server" Visible="false"></rsweb:ReportViewer>
    </td></tr>
  </table>

  <asp:Label ID="LabelMensaje" CssClass="LabelMensaje" runat="server" Text="" Visible="false"></asp:Label>
</asp:Content>
