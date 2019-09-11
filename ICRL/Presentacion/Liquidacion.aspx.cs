using ICRL.BD;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ICRL.Presentacion
{
  public partial class Liquidacion : System.Web.UI.Page
  {
    private int IdFlujo = 0;
    private Int16 EstadoFlujo = 4;
    private double TotalCotizacionBs = 0.0;
    private double TotalCotizacionUs = 0.0;
    private double TotalLiquidacionBs = 0.0;
    private double TotalLiquidacionUs = 0.0;
    double TipoCambio = 6.96;
    int AjusteMas = 2;
    int AjusteMenos = -2;

    #region Eventos de Controles
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        RecuperarDatosFacturas();
        RecuperarDatosOrdenes();
        LlenarMenuFacturas();
        RecuperarDatosLiquidacion();

        ValidarFlujoCorrecto();
      }
    }
    protected void GridViewDatosFactura_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
      GridViewDatosFactura.EditIndex = -1;
      RecuperarDatosFacturas();
      LlenarMenuFacturas();
    }
    protected void GridViewDatosFactura_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
      string idFactura = GridViewDatosFactura.DataKeys[e.RowIndex].Values["id_factura"].ToString();
      string numeroFactura = ((Label)GridViewDatosFactura.Rows[e.RowIndex].FindControl("lblNumeroFactura")).Text;
      long idFactura_ = string.IsNullOrWhiteSpace(idFactura) ? 0 : Convert.ToInt64(idFactura);

      int.TryParse(Request.QueryString["idflujo"], out IdFlujo);
      bool operacionExitosa = LiquidacionICRL.BorrarLiquidacion001Factura(IdFlujo, idFactura_);

      if (operacionExitosa)
      {
        ActualizarAjusteMenor("0"); //cada vez que se elimina una factura, se actualiza el dato de ajuste
        ActualizarDatosOrden(numeroFactura); //cada vez que se elimina una factura, se actualiza la orden asociada
        RecuperarDatosFacturas();
        RecuperarDatosOrdenes();
        LlenarMenuFacturas();
        RecuperarDatosLiquidacion();

        LabelMensaje.Visible = false;
      }
      else
      {
        LabelMensaje.Visible = true;
        LabelMensaje.Text = "Error al eliminar la factura!";
      }
    }
    protected void GridViewDatosFactura_RowEditing(object sender, GridViewEditEventArgs e)
    {
      GridViewDatosFactura.EditIndex = e.NewEditIndex;

      RecuperarDatosFacturas();
      LlenarMenuFacturas();
    }
    protected void GridViewDatosFactura_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
      string idFactura = GridViewDatosFactura.DataKeys[e.RowIndex].Values["id_factura"].ToString();
      string numero = ((TextBox)GridViewDatosFactura.Rows[e.RowIndex].FindControl("txbNumeroFacturaEditar")).Text;
      string fechaEmision = ((TextBox)GridViewDatosFactura.Rows[e.RowIndex].FindControl("txbEmisionFacturaEditar")).Text;
      string fechaEntrega = ((TextBox)GridViewDatosFactura.Rows[e.RowIndex].FindControl("txbEntregaFacturaEditar")).Text;
      string monto = ((TextBox)GridViewDatosFactura.Rows[e.RowIndex].FindControl("txbMontoFacturaEditar")).Text;
      string moneda = ((DropDownList)GridViewDatosFactura.Rows[e.RowIndex].FindControl("ddlMonedaFacturaEditar")).SelectedItem.Value;

      long idFactura_ = Convert.ToInt64(idFactura);
      long numero_ = Convert.ToInt64(numero);
      DateTime fechaEmision_ = DateTime.ParseExact(fechaEmision, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
      DateTime fechaEntrega_ = DateTime.ParseExact(fechaEntrega, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
      double monto_ = Convert.ToDouble(monto);
      Int16 moneda_ = Convert.ToInt16(moneda);

      double tipo_cambio_ = 6.96;
      double.TryParse(txbTipoCambio.Text, out tipo_cambio_);

      LiquidacionICRL.TipoLiquidacion001Factura factura = new LiquidacionICRL.TipoLiquidacion001Factura();
      int.TryParse(Request.QueryString["idflujo"], out IdFlujo);
      factura.id_flujo = IdFlujo;
      factura.id_factura = idFactura_;
      factura.numero_factura = numero_;
      factura.fecha_emision = fechaEmision_;
      factura.fecha_entrega = fechaEntrega_;
      factura.monto = monto_;
      factura.id_moneda = moneda_;
      factura.tipo_cambio = tipo_cambio_;
      bool operacionExitosa = LiquidacionICRL.ActualizarLiquidacion001Factura(factura);

      if (operacionExitosa)
      {
        //string numeroViejo = e.OldValues[""]

        GridViewDatosFactura.EditIndex = -1;
        ActualizarAjusteMenor("0"); //cada vez que se actualiza una factura se actualiza el dato de ajuste
        ActualizarDatosOrden(((Label)GridViewDatosFactura.Rows[e.RowIndex].FindControl("lblNumeroFacturaEditar")).Text); //cada vez que se actualiza una factura, se actualiza la orden asociada
        RecuperarDatosFacturas();
        RecuperarDatosOrdenes();
        LlenarMenuFacturas();
        RecuperarDatosLiquidacion();

        LabelMensaje.Visible = false;
      }
      else
      {
        LabelMensaje.Visible = true;
        LabelMensaje.Text = "Error al guardar la factura!";
      }
    }
    protected void GridViewDatosFactura_RowCommand(object sender, GridViewCommandEventArgs e)
    {
      if (e.CommandName.Equals("AddNew"))
      {
        string numero = ((TextBox)GridViewDatosFactura.FooterRow.FindControl("txbNumeroFacturaNuevo")).Text.Trim();
        string fechaEmision = ((TextBox)GridViewDatosFactura.FooterRow.FindControl("txbEmisionFacturaNuevo")).Text.Trim();
        string fechaEntrega = ((TextBox)GridViewDatosFactura.FooterRow.FindControl("txbEntregaFacturaNuevo")).Text.Trim();
        string monto = ((TextBox)GridViewDatosFactura.FooterRow.FindControl("txbMontoFacturaNuevo")).Text.Trim();
        string moneda = ((DropDownList)GridViewDatosFactura.FooterRow.FindControl("ddlMonedaFacturaNuevo")).SelectedItem.Value;

        long numero_ = Convert.ToInt64(numero);
        DateTime fechaEmision_ = DateTime.ParseExact(fechaEmision, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        DateTime fechaEntrega_ = DateTime.ParseExact(fechaEntrega, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        double monto_ = Convert.ToDouble(monto);
        Int16 moneda_ = Convert.ToInt16(moneda);

        LiquidacionICRL.TipoLiquidacion001Factura factura = new LiquidacionICRL.TipoLiquidacion001Factura();
        int.TryParse(Request.QueryString["idflujo"], out IdFlujo);
        factura.id_flujo = IdFlujo;
        factura.numero_factura = numero_;
        factura.fecha_emision = fechaEmision_;
        factura.fecha_entrega = fechaEntrega_;
        factura.monto = monto_;
        factura.id_moneda = moneda_;
        bool operacionExitosa = LiquidacionICRL.RegistrarLiquidacion001Factura(factura);

        if (operacionExitosa)
        {
          RecuperarDatosFacturas();
          LlenarMenuFacturas();
          RecuperarDatosLiquidacion();

          LabelMensaje.Visible = false;
          //txbTipoCambio.Enabled = true;
        }
        else
        {
          LabelMensaje.Visible = true;
          LabelMensaje.Text = "Error al guardar la factura!";
        }
      }
    }
    protected void GridViewDatosFactura_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      if (e.Row.RowType == DataControlRowType.DataRow)
      {
        string idFactura = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "id_factura"));
        LinkButton lnkbtnresult = (LinkButton)e.Row.FindControl("btnEliminar");

        if (lnkbtnresult != null)
        {
          lnkbtnresult.Attributes.Add("onclick", "javascript:return ConfirmarEliminar('" + idFactura + "')");
        }
      }
    }
    protected void GridViewDatosOrden_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      if (e.Row.RowType == DataControlRowType.Footer)
      {
        Label lblTotalCotizacionBs = e.Row.FindControl("lblTotalCotizacionBs") as Label;
        Label lblTotalCotizacionUs = e.Row.FindControl("lblTotalCotizacionUs") as Label;

        lblTotalCotizacionBs.Text = TotalCotizacionBs.ToString("N");
        lblTotalCotizacionUs.Text = TotalCotizacionUs.ToString("N");
      }

      if (e.Row.RowType == DataControlRowType.DataRow)
      {
        Label lblCotizacionBs = e.Row.FindControl("lblCotizacionBs") as Label;
        Label lblCotizacionUs = e.Row.FindControl("lblCotizacionUs") as Label;

        TotalCotizacionBs += double.Parse(lblCotizacionBs.Text);
        TotalCotizacionUs += double.Parse(lblCotizacionUs.Text);
      }
    }
    protected void GridViewDatosLiquidacion_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      if (e.Row.RowType == DataControlRowType.Footer)
      {
        Label lblTotalLiquidacionBs = e.Row.FindControl("lblTotalLiquidacionBs") as Label;
        Label lblTotalLiquidacionUs = e.Row.FindControl("lblTotalLiquidacionUs") as Label;
        Label lblSaldoLiquidacionBs = e.Row.FindControl("lblSaldoLiquidacionBs") as Label;
        Label lblSaldoLiquidacionUs = e.Row.FindControl("lblSaldoLiquidacionUs") as Label;

        lblTotalLiquidacionBs.Text = TotalLiquidacionBs.ToString("N");
        lblTotalLiquidacionUs.Text = TotalLiquidacionUs.ToString("N");

        lblSaldoLiquidacionBs.Text = (TotalCotizacionBs - TotalLiquidacionBs).ToString("N");
        lblSaldoLiquidacionUs.Text = (TotalCotizacionUs - TotalLiquidacionUs).ToString("N");
      }

      if (e.Row.RowType == DataControlRowType.DataRow)
      {
        Label lblLiquidacionBs = e.Row.FindControl("lblLiquidacionBs") as Label;
        Label lblLiquidacionUs = e.Row.FindControl("lblLiquidacionUs") as Label;

        TotalLiquidacionBs += double.Parse(lblLiquidacionBs.Text);
        TotalLiquidacionUs += double.Parse(lblLiquidacionUs.Text);
      }
    }
    protected void btnGenerarLiquidacion_Click(object sender, EventArgs e)
    {
      bool operacionExitosa = true;
      List<LiquidacionICRL.TipoLiquidacion001> ordenesLiquidadas = new List<LiquidacionICRL.TipoLiquidacion001>();

      foreach (GridViewRow row in GridViewDatosOrden.Rows)
      {
        LiquidacionICRL.TipoLiquidacion001 liquidacion = new LiquidacionICRL.TipoLiquidacion001();

        if (row.RowType == DataControlRowType.DataRow)
        {
          string fechaRecepcion = row.Cells[5].Text;
          //string fechaOrden = row.Cells[5].Text;
          DateTime fechaLiquidacion = DateTime.Now;

          Label lblFechaLiquidacion = row.FindControl("lblFechaLiquidacion") as Label;
          bool esLiquidada = ((CheckBox)row.Cells[7].FindControl("cbxLiquidacion")).Checked;

          liquidacion.numero_orden = row.Cells[0].Text;
          liquidacion.proveedor = row.Cells[1].Text;
          //liquidacion.item_descripcion = row.Cells[2].Text;
          liquidacion.item_descripcion = ((Label)row.Cells[2].FindControl("lblDescripcion")).Text;
          liquidacion.preciobs = Convert.ToDouble(((Label)row.Cells[3].FindControl("lblCotizacionBs")).Text);
          liquidacion.precious = Convert.ToDouble(((Label)row.Cells[4].FindControl("lblCotizacionUs")).Text);
          if (!string.IsNullOrEmpty(fechaRecepcion) && fechaRecepcion != "&nbsp;")
            liquidacion.fecha_recepcion = DateTime.ParseExact(fechaRecepcion, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
          liquidacion.inspeccion = ((CheckBox)row.Cells[6].FindControl("cbxInspeccion")).Checked;
          liquidacion.liquidacion = esLiquidada;
          liquidacion.num_factura = ((DropDownList)row.Cells[8].FindControl("ddlFacturasLiquidadas")).SelectedItem.Value;
          //liquidacion.fecha_liquidacion = esLiquidada ? Convert.ToDateTime(fechaLiquidacion) : new DateTime(2000, 1, 1);

          liquidacion.id_estado = Convert.ToInt16(((Label)row.Cells[9].FindControl("id_estado")).Text);
          int.TryParse(Request.QueryString["idflujo"], out IdFlujo);
          liquidacion.id_flujo = IdFlujo;
          liquidacion.id_cotizacion = Convert.ToInt32(((Label)row.Cells[9].FindControl("id_cotizacion")).Text);
          liquidacion.tipo_origen = Convert.ToInt16(((Label)row.Cells[9].FindControl("tipo_origen")).Text);
          liquidacion.id_item = Convert.ToInt64(((Label)row.Cells[9].FindControl("id_item")).Text);
          liquidacion.id_tipo_item = Convert.ToInt16(((Label)row.Cells[9].FindControl("id_tipo_item")).Text);
          liquidacion.fecha_orden = Convert.ToDateTime(((Label)row.Cells[9].FindControl("fecha_orden")).Text);

          DateTime fecha;
          if (DateTime.TryParseExact(lblFechaLiquidacion.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out fecha))
            liquidacion.fecha_liquidacion = esLiquidada ? fecha : new DateTime(2000, 1, 1);
          else
            liquidacion.fecha_liquidacion = esLiquidada ? fechaLiquidacion : new DateTime(2000, 1, 1);

          #region Generacion reporte liquidacion
          if (liquidacion.liquidacion)
            ordenesLiquidadas.Add(liquidacion);
          #endregion

          #region Guardado en BD
          operacionExitosa = LiquidacionICRL.ModificarLiquidacion001(liquidacion);

          if (!operacionExitosa)
            break;
          #endregion
        }
      }

      if (operacionExitosa)
      {
        RecuperarDatosOrdenes();
        LlenarMenuFacturas();

        GenerarDatosLiquidacion(ordenesLiquidadas);
      }
      else
      {
        btnGuardarLiquidacion.Enabled = false;
        LabelMensaje.Visible = true;
        LabelMensaje.Text = "Error al guardar generar la liquidación!";
      }
    }
    protected void btnAjusteMenor_Click(object sender, EventArgs e)
    {
      TipoCambio = double.Parse(txbTipoCambio.Text);

      double saldoLiquidacionBs_ = 0.0;
      double saldoLiquidacionUs_ = 0.0;

      string saldoLiquidacionBs = ((Label)GridViewDatosLiquidacion.FooterRow.FindControl("lblSaldoLiquidacionBs")).Text;
      saldoLiquidacionBs_ = double.Parse(saldoLiquidacionBs);
      saldoLiquidacionUs_ = saldoLiquidacionBs_ / TipoCambio;

      if (saldoLiquidacionBs_ != 0 &&
        saldoLiquidacionBs_ >= AjusteMenos && saldoLiquidacionBs_ <= AjusteMas)
      {
        GridViewRow row = GridViewDatosLiquidacion.Rows[GridViewDatosLiquidacion.Rows.Count - 1];
        string idFactura = ((Label)row.FindControl("lblId")).Text;

        if (string.IsNullOrWhiteSpace(idFactura))
        {
          Label lblLiquidacionBs = (Label)row.FindControl("lblLiquidacionBs");
          Label lblLiquidacionUs = (Label)row.FindControl("lblLiquidacionUs");
          lblLiquidacionBs.Text = saldoLiquidacionBs_.ToString("N");
          lblLiquidacionUs.Text = saldoLiquidacionUs_.ToString("N");

          Label lblTotalLiquidacionBs = GridViewDatosLiquidacion.FooterRow.FindControl("lblTotalLiquidacionBs") as Label;
          Label lblSaldoLiquidacionBs = GridViewDatosLiquidacion.FooterRow.FindControl("lblSaldoLiquidacionBs") as Label;
          Label lblTotalLiquidacionUs = GridViewDatosLiquidacion.FooterRow.FindControl("lblTotalLiquidacionUs") as Label;
          Label lblSaldoLiquidacionUs = GridViewDatosLiquidacion.FooterRow.FindControl("lblSaldoLiquidacionUs") as Label;

          lblTotalLiquidacionBs.Text = (saldoLiquidacionBs_ + double.Parse(lblTotalLiquidacionBs.Text)).ToString("N");
          lblSaldoLiquidacionBs.Text = (saldoLiquidacionBs_ - double.Parse(lblSaldoLiquidacionBs.Text)).ToString("N");
          lblTotalLiquidacionUs.Text = (saldoLiquidacionUs_ + double.Parse(lblTotalLiquidacionUs.Text)).ToString("N");
          lblSaldoLiquidacionUs.Text = (saldoLiquidacionUs_ - double.Parse(lblSaldoLiquidacionUs.Text)).ToString("N");
        }

        LabelMensaje.Visible = false;
      }
      else
      {
        LabelMensaje.Visible = true;
        LabelMensaje.Text = string.Format("El 'AJUSTE MENOR' aplica solamente en caso que exista una diferencia de {0} (dos) bolivianos entre el TOTAL COTIZADO y TOTAL PAGADO.", AjusteMas);
      }
    }
    protected void btnGuardarLiquidacion_Click(object sender, EventArgs e)
    {
      bool operacionExitosa = true;

      foreach (GridViewRow row in GridViewDatosLiquidacion.Rows)
      {
        if (row.RowType == DataControlRowType.DataRow)
        {
          int.TryParse(Request.QueryString["idflujo"], out IdFlujo);
          string idFactura = ((Label)row.FindControl("lblId")).Text;
          string numero = ((Label)row.FindControl("lblNumero")).Text;
          string fechaEmision = ((Label)row.FindControl("lblFechaEmision")).Text;
          string fechaEntrega = ((Label)row.FindControl("lblFechaRecepcion")).Text;
          string monto = ((Label)row.FindControl("lblLiquidacionBs")).Text;
          string moneda = ((Label)row.FindControl("lblMoneda")).Text;
          string observaciones = ((TextBox)row.FindControl("txbObservaciones")).Text;
          bool asociada = true;
          double tipo_cambio_ = 6.96;
          double.TryParse(txbTipoCambio.Text, out tipo_cambio_);

          if (!string.IsNullOrWhiteSpace(idFactura))
          {//actualizar facturas liquidadas
            long idFactura_ = Convert.ToInt64(idFactura);
            long numero_ = Convert.ToInt64(numero);
            DateTime fechaEmision_ = DateTime.ParseExact(fechaEmision, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            DateTime fechaEntrega_ = DateTime.ParseExact(fechaEntrega, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            double monto_ = Convert.ToDouble(monto);
            Int16 moneda_ = Convert.ToInt16(moneda);

            LiquidacionICRL.TipoLiquidacion001Factura factura = new LiquidacionICRL.TipoLiquidacion001Factura();
            factura.id_flujo = IdFlujo;
            factura.id_factura = idFactura_;
            factura.numero_factura = numero_;
            factura.fecha_emision = fechaEmision_;
            factura.fecha_entrega = fechaEntrega_;
            factura.monto = monto_;
            factura.id_moneda = moneda_;
            factura.observaciones = observaciones;
            factura.asociada = asociada;
            factura.tipo_cambio = tipo_cambio_;

            operacionExitosa = LiquidacionICRL.ActualizarLiquidacion001Factura(factura);

            if (!operacionExitosa)
              break;
          }
          else
          {//actualizar ajuste menor
            ActualizarAjusteMenor(monto);
          }
        }
      }

      LabelMensaje.Visible = true;
      if (operacionExitosa)
      {
        btnGuardarLiquidacion.Enabled = false;
        btnAjusteMenor.Enabled = false;
        LabelMensaje.Text = "Liquidación guardada exitosamente!";
      }
      else
      {
        btnGuardarLiquidacion.Enabled = true;
        LabelMensaje.Text = "Error al guardar la liquidación!";
      }
    }
    protected void btnLiquidacionTotal_Click(object sender, EventArgs e)
    {
      int.TryParse(Request.QueryString["idflujo"], out IdFlujo);
      LiquidacionICRL.TipoTraerLiquidacion001 ordenes = LiquidacionICRL.TraerLiquidacion001(IdFlujo);
      LiquidacionICRL.TipoTraerLiquidacion001Factura facturas = LiquidacionICRL.TraerLiquidacion001Factura(IdFlujo);

      double totalCotizado = 0.0;
      double totalFacturado = 0.0;

      List<LiquidacionICRL.TipoLiquidacion001> ordenesActualizadas = new List<LiquidacionICRL.TipoLiquidacion001>();
      foreach (LiquidacionICRL.TipoLiquidacion001 orden in ordenes.Liquidaciones001)
      {
        totalCotizado += orden.preciobs;

        orden.id_estado = EstadoFlujo;
        ordenesActualizadas.Add(orden);
      }

      foreach (LiquidacionICRL.TipoLiquidacion001Factura factura in facturas.Facturas)
      {
        if (factura.asociada)
          totalFacturado += factura.monto;
      }

      if (totalCotizado == totalFacturado)
      {
        bool operacionExitosaFlujo = LiquidacionICRL.ActualizarFlujo(IdFlujo, EstadoFlujo);

        foreach (LiquidacionICRL.TipoLiquidacion001 ordenActualizada in ordenesActualizadas)
        {
          bool operacionExitosaOrden = LiquidacionICRL.ModificarLiquidacion001(ordenActualizada);

          if (!operacionExitosaOrden)
            break;
        }

        if (operacionExitosaFlujo)
        {
          LabelMensaje.Visible = true;
          LabelMensaje.Text = "LIQUIDACIÓN TOTAL realiza exitosamente!";

          BloquearControlesLiquidacion(true);
        }
        else
        {
          LabelMensaje.Visible = true;
          LabelMensaje.Text = "Error en el proceso de liquidación total!";
        }
      }
      else
      {
        LabelMensaje.Visible = true;
        LabelMensaje.Text = "Para proceder con la LIQUIDACIÓN TOTAL, los montos de TOTAL COTIZADO y TOTAL PAGADO deben ser iguales.";
      }
    }
    #endregion

    #region Metodos de Soporte
    private void ValidarFlujoCorrecto()
    {
      int.TryParse(Request.QueryString["idflujo"], out IdFlujo);
      //txbNroFlujo.Text = IdFlujo.ToString();

      if (GridViewDatosOrden.Rows.Count == 0)
      {
        GridViewDatosFactura.Enabled = false;
        LabelMensaje.Visible = true;
        LabelMensaje.Text = "No existen Órdenes asociadas al flujo seleccionado!";
      }
      else
      {
        GridViewDatosFactura.Enabled = true;
        LabelMensaje.Visible = false;

        #region Llenar campos de datos
        LiquidacionICRL.TipoFlujo tipoFlujo = LiquidacionICRL.TipoFlujoTraer(IdFlujo);
        txbCliente.Text = tipoFlujo.nombreAsegurado;
        txbTelefono.Text = tipoFlujo.telefonocelAsegurado;
        txbReclamo.Text = tipoFlujo.numeroReclamo;
        txbPoliza.Text = tipoFlujo.numeroPoliza;
        #endregion

        BloquearControlesLiquidacion(false);
      }
    }

    protected string VerTextoMoneda(object valorMoneda)
    {
      string valor = valorMoneda.ToString();

      AccesoDatos adatos = new AccesoDatos();
      List<ListaNomenclador> monedas = adatos.FlTraeNomenGenerico("Moneda", 0);

      foreach (ListaNomenclador moneda in monedas)
      {
        if (moneda.codigo.Trim() == valor)
          return moneda.descripcion;
      }

      return "";
    }
    private void LlenarMenuFacturas()
    {
      List<string> listaFacturas = new List<string>();

      foreach (GridViewRow row in GridViewDatosFactura.Rows)
      {
        try
        {
          string numeroFactura = ((Label)row.FindControl("lblNumeroFactura")).Text.Trim();

          if (numeroFactura != "0")
            listaFacturas.Add(numeroFactura);
        }
        catch { }
      }

      foreach (GridViewRow row in GridViewDatosOrden.Rows)
      {
        if (row.RowType == DataControlRowType.DataRow)
        {
          string ddlText = "";
          string numero = ((Label)row.FindControl("lblNumFactura")).Text;
          if (!string.IsNullOrWhiteSpace(numero) && numero != "&nbsp;")
            ddlText = numero;

          DropDownList ddlFacturasLiquidadas = row.FindControl("ddlFacturasLiquidadas") as DropDownList;
          ddlFacturasLiquidadas.DataSource = listaFacturas;
          ddlFacturasLiquidadas.DataBind();

          ddlFacturasLiquidadas.Items.Insert(0, "");
          try
          {/*En caso de que se elimine una factura ya liquidada*/
            ddlFacturasLiquidadas.SelectedValue = ddlText;
          }
          catch
          {
            ddlFacturasLiquidadas.SelectedValue = "";
          }
        }
      }

      //return listaFacturas;
    }
    protected void RecuperarDatosFacturas()
    {
      int.TryParse(Request.QueryString["idflujo"], out IdFlujo);
      LiquidacionICRL.TipoTraerLiquidacion001Factura facturas = LiquidacionICRL.TraerLiquidacion001Factura(IdFlujo);
      bool operacionExitosa = facturas.correcto;
      DataSet facturasDataset = facturas.dsFacturas;

      if (operacionExitosa)
      {
        if (facturasDataset.Tables[0].Rows.Count > 0)
        {
          GridViewDatosFactura.DataSource = facturasDataset;
          GridViewDatosFactura.DataBind();

          /**/
          foreach (GridViewRow row in GridViewDatosFactura.Rows)
          {
            if (row.FindControl("lblNumeroFactura") == null)
              continue;

            string numeroFactura = ((Label)row.FindControl("lblNumeroFactura")).Text.Trim();
            if (numeroFactura == "0")
            {
              row.Visible = false;
              break;
            }
          }
          /**/

          object tc = facturasDataset.Tables[0].Rows[0]["tipo_cambio"];
          string valorTC = "";
          if (tc != null)
            valorTC = tc.ToString();
          txbTipoCambio.Text = valorTC.ToString();
        }
        else
        {
          facturasDataset.Tables[0].Rows.Add(facturasDataset.Tables[0].NewRow());

          GridViewDatosFactura.DataSource = facturasDataset;
          GridViewDatosFactura.DataBind();
        }

        LabelMensaje.Visible = false;
      }
      else
      {
        LabelMensaje.Visible = true;
        LabelMensaje.Text = "Error en la recuperación de los datos facturas!";
      }
    }
    protected void RecuperarDatosOrdenes()
    {
      int.TryParse(Request.QueryString["idflujo"], out IdFlujo);
      LiquidacionICRL.TipoTraerLiquidacion001 ordenes = LiquidacionICRL.TraerLiquidacion001(IdFlujo);
      bool operacionExitosa = ordenes.correcto;
      DataSet ordenesDataset = ordenes.dsLiquidacion001;

      if (operacionExitosa)
      {
        GridViewDatosOrden.DataSource = ordenesDataset;
        GridViewDatosOrden.DataBind();

        LabelMensaje.Visible = false;
      }
      else
      {
        LabelMensaje.Visible = true;
        LabelMensaje.Text = "Error en la recuperacion de los datos ordenes!";
      }
    }
    private void RecuperarDatosLiquidacion()
    {
      List<LiquidacionICRL.TipoLiquidacion001> ordenesLiquidadas = new List<LiquidacionICRL.TipoLiquidacion001>();

      foreach (GridViewRow row in GridViewDatosOrden.Rows)
      {
        LiquidacionICRL.TipoLiquidacion001 liquidacion = new LiquidacionICRL.TipoLiquidacion001();

        if (row.RowType == DataControlRowType.DataRow)
        {
          string fechaRecepcion = row.Cells[5].Text;
          DateTime fechaLiquidacion = DateTime.Now;

          Label lblFechaLiquidacion = row.FindControl("lblFechaLiquidacion") as Label;
          bool esLiquidada = ((CheckBox)row.Cells[7].FindControl("cbxLiquidacion")).Checked;

          liquidacion.numero_orden = row.Cells[0].Text;
          liquidacion.liquidacion = esLiquidada;
          liquidacion.num_factura = ((DropDownList)row.Cells[8].FindControl("ddlFacturasLiquidadas")).SelectedItem.Value;

          #region Generacion reporte liquidacion
          if (liquidacion.liquidacion)
            ordenesLiquidadas.Add(liquidacion);
          #endregion
        }
      }

      GenerarDatosLiquidacion(ordenesLiquidadas);
    }
    protected void GenerarDatosLiquidacion(List<LiquidacionICRL.TipoLiquidacion001> ordenesLiquidadas)
    {
      if (!string.IsNullOrEmpty(txbTipoCambio.Text))
        TipoCambio = double.Parse(txbTipoCambio.Text);

      DataTable dt = new DataTable();
      dt.Columns.Add("orden");
      dt.Columns.Add("preciobs");
      dt.Columns.Add("precious");
      dt.Columns.Add("fecharec");
      dt.Columns.Add("numero");
      dt.Columns.Add("observaciones");
      dt.Columns.Add("moneda");
      dt.Columns.Add("fechaemi");
      dt.Columns.Add("id");

      GridViewRow rowAjuste = null;
      foreach (GridViewRow row in GridViewDatosFactura.Rows)
      {
        /*if (row.RowType == DataControlRowType.DataRow)
        {*/
        string numeroFactura = ((Label)row.FindControl("lblNumeroFactura")).Text.Trim();

        if (numeroFactura == "0")
        {
          rowAjuste = row;
          continue;
        }

        foreach (LiquidacionICRL.TipoLiquidacion001 ordenLiquidada in ordenesLiquidadas)
        {
          if (ordenLiquidada.liquidacion &&
            numeroFactura == ordenLiquidada.num_factura.Trim() &&
            !string.IsNullOrEmpty(numeroFactura))
          {
            string montoBs = ((Label)row.FindControl("lblMontoFactura")).Text.Trim();
            string montoUs = "";
            if (!string.IsNullOrWhiteSpace(montoBs) && montoBs != "&nbsp;")
              montoUs = (Convert.ToDouble(montoBs) / TipoCambio).ToString("N");
            string fechaRecepcion = ((Label)row.FindControl("lblEntregaFactura")).Text.Trim();

            //string fechaLiquidacion = DateTime.Now.ToString("dd-MM-yyyy");
            string observaciones = ((Label)row.FindControl("lblObservaciones")).Text.Trim();
            string id = ((Label)row.FindControl("lblIdFactura")).Text;
            string fechaEmision = ((Label)row.FindControl("lblEmisionFactura")).Text;
            string moneda = ((Label)row.FindControl("lblIdMonedaFactura")).Text;

            DataRow dr = dt.NewRow();
            dr["orden"] = ordenLiquidada.numero_orden;
            dr["preciobs"] = montoBs;
            dr["precious"] = montoUs;
            dr["fecharec"] = fechaRecepcion;
            dr["numero"] = numeroFactura;
            dr["observaciones"] = observaciones;
            dr["id"] = id;
            dr["fechaemi"] = fechaEmision;
            dr["moneda"] = moneda;

            dt.Rows.Add(dr);

            break;
          }
        }
        /*}*/
      }

      DataRow drAjuste = dt.NewRow();
      drAjuste["orden"] = "---AJUSTE---";
      drAjuste["fecharec"] = "";
      drAjuste["numero"] = "";
      drAjuste["id"] = "";
      drAjuste["fechaemi"] = "";
      drAjuste["moneda"] = "";
      if (rowAjuste == null)
      {
        drAjuste["preciobs"] = "0";
        drAjuste["precious"] = "0";
        drAjuste["observaciones"] = "";
      }
      else
      {
        string montoBs = ((Label)rowAjuste.FindControl("lblMontoFactura")).Text.Trim();
        string montoUs = "";
        if (!string.IsNullOrWhiteSpace(montoBs) && montoBs != "&nbsp;")
          montoUs = (Convert.ToDouble(montoBs) / TipoCambio).ToString("N");
        drAjuste["preciobs"] = montoBs;
        drAjuste["precious"] = montoUs;
        drAjuste["observaciones"] = ((Label)rowAjuste.FindControl("lblObservaciones")).Text.Trim();
      }
      dt.Rows.Add(drAjuste);

      GridViewDatosLiquidacion.DataSource = dt;
      GridViewDatosLiquidacion.DataBind();

      if (dt.Rows[0]["id"].ToString() != "")
      {
        btnAjusteMenor.Enabled = true;
        btnGuardarLiquidacion.Enabled = true;
      }
      else
      {
        btnAjusteMenor.Enabled = false;
        btnGuardarLiquidacion.Enabled = false;
      }
    }
    private void ActualizarAjusteMenor(string monto)
    {
      LiquidacionICRL.TipoTraerLiquidacion001Factura facturas = LiquidacionICRL.TraerLiquidacion001Factura(IdFlujo);
      LiquidacionICRL.TipoLiquidacion001Factura registroAjusteMenor = facturas.Facturas.Find(x => x.numero_factura == 0);
      double monto_ = Convert.ToDouble(monto);

      if (registroAjusteMenor == null)
      {//Se crea el registro de AJUSTE MENOR
        registroAjusteMenor = new LiquidacionICRL.TipoLiquidacion001Factura();
        registroAjusteMenor.id_flujo = IdFlujo;
        registroAjusteMenor.monto = monto_;
        registroAjusteMenor.observaciones = "(ajuste menor)";
        registroAjusteMenor.asociada = true;
        LiquidacionICRL.RegistrarLiquidacion001Factura(registroAjusteMenor);
      }
      else
      {//Se actualiza el registro de AJUSTE MENOR
        registroAjusteMenor.monto = monto_;
        LiquidacionICRL.ActualizarLiquidacion001Factura(registroAjusteMenor);
      }
    }
    private void ActualizarDatosOrden(string numeroFactura)
    {
      int.TryParse(Request.QueryString["idflujo"], out IdFlujo);
      LiquidacionICRL.TipoTraerLiquidacion001 ordenes = LiquidacionICRL.TraerLiquidacion001(IdFlujo);

      foreach (LiquidacionICRL.TipoLiquidacion001 orden in ordenes.Liquidaciones001)
      {
        if (numeroFactura.Trim() == orden.num_factura.Trim())
        {
          LiquidacionICRL.TipoLiquidacion001 ordenModificada = orden;
          ordenModificada.liquidacion = false;
          ordenModificada.num_factura = "";
          ordenModificada.fecha_liquidacion = new DateTime(2000, 1, 1);
          LiquidacionICRL.ModificarLiquidacion001(ordenModificada);
        }
      }
    }
    private void BloquearControlesLiquidacion(bool verificado)
    {
      if (verificado)
      {
        GridViewDatosFactura.Enabled = false;
        GridViewDatosOrden.Enabled = false;
        GridViewDatosLiquidacion.Enabled = false;

        btnGenerarLiquidacion.Enabled = false;
        btnLiquidacionTotal.Enabled = false;
        btnAjusteMenor.Enabled = false;
        btnGuardarLiquidacion.Enabled = false;

        txbTipoCambio.Enabled = false;
      }
      else
      {
        int.TryParse(Request.QueryString["idflujo"], out IdFlujo);
        LiquidacionICRL.TipoFlujo flujo = LiquidacionICRL.TipoFlujoTraer(IdFlujo);

        if (flujo.estado == EstadoFlujo.ToString())
        {
          GridViewDatosFactura.Enabled = false;
          GridViewDatosOrden.Enabled = false;
          GridViewDatosLiquidacion.Enabled = false;

          btnGenerarLiquidacion.Enabled = false;
          btnLiquidacionTotal.Enabled = false;
          btnAjusteMenor.Enabled = false;
          btnGuardarLiquidacion.Enabled = false;

          txbTipoCambio.Enabled = false;

          LabelMensaje.Visible = true;
          LabelMensaje.Text = "Este flujo tiene el estado 'Procesado Liquidación', por lo que no se permiten cambios.";
        }
      }
    }
    #endregion
  }
}