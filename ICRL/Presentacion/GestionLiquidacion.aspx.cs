using ICRL.BD;
using ICRL.DS;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ICRL.Presentacion
{
  public partial class GestionLiquidacion : System.Web.UI.Page
  {
    public bool bSoloAuditor = false;
    private bool VerificarPagina(bool EsEvento)
    {
      bool blnRespuesta = true;
      if (Session["NomUsr"] == null || string.IsNullOrWhiteSpace(Convert.ToString(Session["NomUsr"])))
      {
        blnRespuesta = false;
        if (!EsEvento) Response.Redirect("../Acceso/Login.aspx");
      }
      return blnRespuesta;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!VerificarPagina(false)) return;

      bool vAcceso = false;
      vAcceso = FValidaRol("ICRLLiquidacionAdministrador", (string[])(Session["RolesUsr"]));
      if (!vAcceso)
      {
        vAcceso = FValidaRol("ICRLLiquidacionUsuario", (string[])(Session["RolesUsr"]));
        if (!vAcceso)
        {
          vAcceso = FValidaRol("ICRLLiquidacionAuditor", (string[])(Session["RolesUsr"]));
          if (!vAcceso)
          {
            Response.Redirect("../Acceso/Login.aspx", false);
          }
          else
          {
            bSoloAuditor = true;
          }
        }
      }

      if (!IsPostBack)
      {
        InicializarEstados();
        InicializarRangosFechaBusqueda();
      }
    }

    public bool FValidaRol(string pRolaValidar, string[] pRoles)
    {
      bool vResultado = false;

      foreach (var vItem in pRoles)
      {
        if (vItem == pRolaValidar)
        {
          vResultado = true;
          break;
        }
      }

      return vResultado;
    }

    #region Eventos de Controles
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      if (Page.IsValid)
      {
        RecuperarDatosOrdenesPago();
      }
    }
    protected void cuvFechaHasta_ServerValidate(object source, ServerValidateEventArgs args)
    {
      DateTime desde = DateTime.ParseExact(txbFechaDesde.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
      DateTime hasta = DateTime.ParseExact(args.Value, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

      if (hasta <= desde)
        args.IsValid = false;
      else
        args.IsValid = true;
    }
    protected void GridViewOrdenesPago_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
      if (!VerificarPagina(true)) return;
      GridViewOrdenesPago.PageIndex = e.NewPageIndex;

      RecuperarDatosOrdenesPago();
    }
    protected void btnExportarResultados_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      try
      {
        #region Recuperar datos
        int estado = 1;
        int.TryParse(ddlEstado.SelectedItem.Value, out estado);
        string proveedor = txbProveedor.Text.Trim().Replace('\'', ' ');
        string sucursal = txbSucursal.Text.Trim().Replace('\'', ' ');
        string flujoon = txbFlujoOnbase.Text.Trim().Replace('\'', ' ');
        string placa = txbPlaca.Text.Trim().Replace('\'', ' ');
        DateTime fechaInicio = DateTime.ParseExact(txbFechaDesde.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        DateTime fechaFin = DateTime.ParseExact(txbFechaHasta.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

        LiquidacionICRL.TipoRespuestaGrilla respuestaGrilla = LiquidacionICRL.LiquidacionGrilla(
          estado, proveedor, sucursal, flujoon, placa, fechaInicio, fechaFin);
        bool operacionExitosa = respuestaGrilla.correcto;
        DataSet respuestaGrillaDataset = new DataSet();
        respuestaGrillaDataset = respuestaGrilla.dsLiquidacionGrilla;
        #endregion

        #region Descripcion estados
        AccesoDatos adatos = new AccesoDatos();
        List<ListaNomenclador> estadosnom = adatos.FlTraeNomenGenerico("Estados", 0);
        #endregion

        #region Creacion del archivo CSV
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=LBCCotizaciones.csv");
        Response.Charset = "";
        Response.ContentType = "text/csv";

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("ORDEN;");
        sb.Append("FECHA ORDEN;");
        sb.Append("PROVEEDOR/BENEFICIARIO;");
        sb.Append("ESTADO;");
        sb.Append("TOTAL ORDEN;");
        sb.Append("PAGADO BS;");
        sb.Append("NO PAGADO BS;");
        sb.Append("PAGADO US;");
        sb.Append("NO PAGADO US;");
        sb.Append("FLUJO ONBASE;");
        sb.Append("CLIENTE;");
        sb.Append("PLACA;");
        sb.Append("ID FLUJO");
        sb.Append("\r\n");

        for (int i = 0; i < respuestaGrillaDataset.Tables[0].Rows.Count; i++)
        {
          for (int j = 0; j < respuestaGrillaDataset.Tables[0].Rows[i].ItemArray.Count(); j++)
          {
            string valorCelda = respuestaGrillaDataset.Tables[0].Rows[i][j].ToString().Trim();
            valorCelda = valorCelda.Replace(";", ",");

            if (j == 3)
            {
              foreach (ListaNomenclador estadonom in estadosnom)
              {
                if (estadonom.codigo.Trim() == valorCelda)
                {
                  valorCelda = estadonom.descripcion;
                  break;
                }
              }
            }

            if (valorCelda.Contains("span"))
            {
              string subs = valorCelda.Substring(valorCelda.IndexOf('>') + 1);
              valorCelda = subs.Remove(subs.LastIndexOf('<'));
            }

            byte[] bytes = System.Text.Encoding.Default.GetBytes(valorCelda);
            valorCelda = System.Text.Encoding.Default.GetString(bytes);

            if (j == respuestaGrillaDataset.Tables[0].Rows[i].ItemArray.Count() - 1)
              sb.Append(valorCelda);
            else
              sb.Append(valorCelda + ';');
          }

          sb.Append("\r\n");
        }

        Response.Output.Write(sb.ToString());
        #endregion

        LabelMensaje.Visible = false;
      }
      catch (Exception ex)
      {
        LabelMensaje.Visible = true;
        LabelMensaje.Text = "Error al exportar los resultados!";
      }
      finally
      {
        Response.Flush();
        Response.Close();
        Response.End();
      }
    }
    protected void btnGenerarReporteExcel_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      ObtenerReporte("xls");
    }
    protected void btnGenerarReportePdf_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      ObtenerReporte("pdf");
    }
    #endregion

    #region Metodos de Soporte
    private void InicializarRangosFechaBusqueda()
    {
      DateTime hoy = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

      if (string.IsNullOrWhiteSpace(txbFechaDesde.Text))
      {
        txbFechaDesde.Text = hoy.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
      }

      if (string.IsNullOrWhiteSpace(txbFechaHasta.Text))
      {
        txbFechaHasta.Text = hoy.AddMonths(1).ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
      }
    }
    private void RecuperarDatosOrdenesPago()
    {
      int estado = 1;
      int.TryParse(ddlEstado.SelectedItem.Value, out estado);
      string proveedor = txbProveedor.Text.Trim().Replace('\'', ' ');
      string sucursal = txbSucursal.Text.Trim().Replace('\'', ' ');
      string flujoon = txbFlujoOnbase.Text.Trim().Replace('\'', ' ');
      string placa = txbPlaca.Text.Trim().Replace('\'', ' ');
      DateTime fechaInicio = DateTime.ParseExact(txbFechaDesde.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
      DateTime fechaFin = DateTime.ParseExact(txbFechaHasta.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

      LiquidacionICRL.TipoRespuestaGrilla respuestaGrilla = LiquidacionICRL.LiquidacionGrilla(
        estado, proveedor, sucursal, flujoon, placa, fechaInicio, fechaFin);
      bool operacionExitosa = respuestaGrilla.correcto;
      DataSet respuestaGrillaDataset = new DataSet();
      respuestaGrillaDataset = respuestaGrilla.dsLiquidacionGrilla;

      if (operacionExitosa)
      {
        if (respuestaGrillaDataset.Tables[0].Rows.Count > 0)
        {
          GridViewOrdenesPago.DataSource = respuestaGrillaDataset;
          GridViewOrdenesPago.DataBind();

          GridViewOrdenesPago.Visible = true;
          lblMensajeOrdenesPago.Text = "";
          btnGenerarReporteExcel.Enabled = true;
          btnGenerarReportePdf.Enabled = true;
        }
        else
        {
          GridViewOrdenesPago.Visible = false;
          lblMensajeOrdenesPago.Text = "<p>No existen datos.</p><p>Introduzca otros valores en su consulta.</p>";
          btnGenerarReporteExcel.Enabled = false;
          btnGenerarReportePdf.Enabled = false;
        }

        LabelMensaje.Visible = false;
      }
      else
      {
        btnGenerarReporteExcel.Enabled = false;
        btnGenerarReportePdf.Enabled = false;
        LabelMensaje.Visible = true;
        LabelMensaje.Text = "Error en la recuperacion de los datos de pago!";
      }
    }
    protected void InicializarEstados()
    {
      try
      {
        AccesoDatos adatos = new AccesoDatos();
        List<ListaNomenclador> estados = adatos.FlTraeNomenGenerico("Estados", 0);

        ddlEstado.DataSource = estados;
        ddlEstado.DataValueField = "codigo";
        ddlEstado.DataTextField = "descripcion";
        ddlEstado.DataBind();

        ddlEstado.Items.Insert(0, new ListItem("TODOS", "0"));

        LabelMensaje.Visible = false;
      }
      catch (Exception ex)
      {
        LabelMensaje.Visible = true;
        LabelMensaje.Text = "Error al recuperar los Estados!";
      }
    }
    protected string VerTextoEstado(object codigoEstado)
    {
      string valor = codigoEstado.ToString();

      AccesoDatos adatos = new AccesoDatos();
      List<ListaNomenclador> estados = adatos.FlTraeNomenGenerico("Estados", 0);

      foreach (ListaNomenclador estado in estados)
      {
        if (estado.codigo.Trim() == valor)
          return estado.descripcion;
      }

      return "";
    }
    private void ObtenerReporte(string tipo)
    {
      int estado = 1;
      int.TryParse(ddlEstado.SelectedItem.Value, out estado);
      string proveedor = txbProveedor.Text.Trim().Replace('\'', ' ');
      string sucursal = txbSucursal.Text.Trim().Replace('\'', ' ');
      string flujoon = txbFlujoOnbase.Text.Trim().Replace('\'', ' ');
      string placa = txbPlaca.Text.Trim().Replace('\'', ' ');
      DateTime fechai = DateTime.ParseExact(txbFechaDesde.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
      DateTime fechaf = DateTime.ParseExact(txbFechaHasta.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

      ReportViewerLiquidacion.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
      ReportViewerLiquidacion.LocalReport.ReportPath = "Reportes/RepFormularioGestionLiquidacion.rdlc";
      DatosReportes reportes = new DatosReportes();
      Microsoft.Reporting.WebForms.ReportDataSource datasource =
        new Microsoft.Reporting.WebForms.ReportDataSource("GestionLiquidacionDS", reportes.ObtenerDatosGrilla(
          estado, proveedor, sucursal, flujoon, placa, fechai, fechaf));
      ReportViewerLiquidacion.LocalReport.DataSources.Clear();
      ReportViewerLiquidacion.LocalReport.DataSources.Add(datasource);

      //Configuracion de pagina
      /*System.Drawing.Printing.PageSettings pagina = new System.Drawing.Printing.PageSettings();
      pagina.Landscape = true;
      pagina.PaperSize = new System.Drawing.Printing.PaperSize("Carta", 1100, 850);
      pagina.PaperSize.RawKind = (int)System.Drawing.Printing.PaperKind.Letter;
      pagina.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
      ReportViewerLiquidacion.SetPageSettings(pagina);*/

      //Exportar reporte
      Microsoft.Reporting.WebForms.Warning[] warnings;
      string[] streamIds;
      string mimeType = string.Empty;
      string encoding = string.Empty;
      string nombre = "LBCGestionLiquidacion";
      string extension = (tipo == "xls") ? "xls" : "pdf";
      string deviceinfo = string.Format("<DeviceInfo><PageHeight>{0}</PageHeight><PageWidth>{1}</PageWidth><MarginBottom>{2}</MarginBottom><MarginLeft>{3}</MarginLeft><MarginRight>{3}</MarginRight><MarginTop>{2}</MarginTop></DeviceInfo>",
        "8.5in", "13in", "0in", "0.1in");

      byte[] bytes = (tipo == "xls") ?
        ReportViewerLiquidacion.LocalReport.Render("Excel", deviceinfo, out mimeType, out encoding, out extension, out streamIds, out warnings) :
        ReportViewerLiquidacion.LocalReport.Render("PDF", deviceinfo, out mimeType, out encoding, out extension, out streamIds, out warnings);

      Response.Buffer = true;
      Response.Clear();
      Response.ContentType = mimeType;
      Response.AddHeader("content-disposition", "attachment; filename=" + nombre + "." + extension);
      Response.BinaryWrite(bytes);
      Response.Flush();
    }
    #endregion
  }
}