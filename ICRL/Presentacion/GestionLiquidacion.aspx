<%@ Page Title="" Language="C#" MasterPageFile="~/SitioICRL.Master" AutoEventWireup="true" CodeBehind="GestionLiquidacion.aspx.cs" Inherits="ICRL.Presentacion.GestionLiquidacion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Contenidohead" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPaginas" runat="server">
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
          <asp:CompareValidator ID="covFechaDesde" runat="server" ControlToValidate="txbFechaDesde"
            Type="Date" Operator="DataTypeCheck" Text="* Fecha no válida" Display="Dynamic" CssClass="errormessage"></asp:CompareValidator><br />
          <asp:TextBox ID="txbFechaDesde" runat="server" AutoComplete="off" MaxLength="10"></asp:TextBox>
          <ajaxToolkit:CalendarExtender ID="calFechaDesde" runat="server" BehaviorID="calFechaDesde" TargetControlID="txbFechaDesde"
            DaysModeTitleFormat="dd/MM/yyyy" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy"></ajaxToolkit:CalendarExtender>
        </div>
        <div class="twentyfive">
          Fecha Liq. Hasta
          <asp:RequiredFieldValidator ID="rfvFechaHasta" runat="server" ControlToValidate="txbFechaHasta"
            Text="* Requerido" Display="Dynamic" CssClass="errormessage"></asp:RequiredFieldValidator>
          <asp:CompareValidator ID="covFechaHasta" runat="server" ControlToValidate="txbFechaHasta"
            Type="Date" Operator="DataTypeCheck" Text="* Fecha no válida" Display="Dynamic" CssClass="errormessage"></asp:CompareValidator>
          <asp:CustomValidator ID="cuvFechaHasta" runat="server" ControlToValidate="txbFechaHasta" Display="Dynamic"
            CssClass="errormessage" Text="* Fecha debe ser mayor a 'Desde'" OnServerValidate="cuvFechaHasta_ServerValidate"></asp:CustomValidator><br />
          <asp:TextBox ID="txbFechaHasta" runat="server" AutoComplete="off" MaxLength="10"></asp:TextBox>
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
          ForeColor="#333333" GridLines="None" AllowPaging="true" PageSize="25" OnPageIndexChanging="GridViewOrdenesPago_PageIndexChanging">
          <AlternatingRowStyle BackColor="White" />
          <Columns>
            <asp:BoundField DataField="numero_orden" HeaderText="Orden" />
            <asp:BoundField DataField="flujoOnBase" HeaderText="Flujo OnBase" />
            <asp:BoundField DataField="nombreAsegurado" HeaderText="Cliente" />
            <asp:BoundField DataField="placaVehiculo" HeaderText="Placa" />
            <asp:BoundField DataField="proveedor" HeaderText="Proveedor / Beneficiario" />
            <asp:BoundField DataField="fecha_orden" HeaderText="Fecha Orden" DataFormatString="{0:dd-MM-yyyy}" />
            <asp:BoundField DataField="Total" HeaderText="Total Orden" DataFormatString="{0:N}" ItemStyle-CssClass="price" HeaderStyle-CssClass="price" />
            <asp:BoundField DataField="sumabspagado" HeaderText="Pagado Bs." DataFormatString="{0:N}" ItemStyle-CssClass="price" HeaderStyle-CssClass="price" />
            <asp:BoundField DataField="sumabsnopagado" HeaderText="No Pagado Bs." DataFormatString="{0:N}" ItemStyle-CssClass="price" HeaderStyle-CssClass="price" />
            <asp:BoundField DataField="sumauspagado" HeaderText="Pagado Us." DataFormatString="{0:N}" ItemStyle-CssClass="price" HeaderStyle-CssClass="price" />
            <asp:BoundField DataField="sumausnopagado" HeaderText="No Pagado Us." DataFormatString="{0:N}" ItemStyle-CssClass="price" HeaderStyle-CssClass="price" />
            <%--<asp:BoundField DataField="id_estado" HeaderText="Estado" />--%>
            <asp:TemplateField HeaderText="Estado">
              <ItemTemplate>
                <asp:Label ID="lblEstado" runat="server" Text='<%# VerTextoEstado(Eval("id_estado")) %>'></asp:Label>
              </ItemTemplate>
            </asp:TemplateField>
            <asp:HyperLinkField DataTextField="idFlujo" DataNavigateUrlFields="idFlujo"
              DataNavigateUrlFormatString="~\Presentacion\Liquidacion.aspx?idflujo={0}" />
          </Columns>
          <EditRowStyle BackColor="#7C6F57" />
          <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
          <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
          <PagerStyle CssClass="gridpager" HorizontalAlign="Center" />
          <RowStyle BackColor="#E3EAEB" />
          <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
          <SortedAscendingCellStyle BackColor="#F8FAFA" />
          <SortedAscendingHeaderStyle BackColor="#246B61" />
          <SortedDescendingCellStyle BackColor="#D4DFE1" />
          <SortedDescendingHeaderStyle BackColor="#15524A" />
        </asp:GridView>
        <asp:Label ID="lblMensajeOrdenesPago" runat="server"></asp:Label>
      </td></tr>
    </table>

    <asp:Label ID="LabelMensaje" CssClass="LabelMensaje" runat="server" Text="" Visible="false"></asp:Label>
</asp:Content>
