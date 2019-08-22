using ICRL.BD;
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
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        InicializarEstados();
        InicializarRangosFechaBusqueda();
      }
    }

    #region Eventos de Controles
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
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
      GridViewOrdenesPago.PageIndex = e.NewPageIndex;

      RecuperarDatosOrdenesPago();
    }
    protected void btnExportarResultados_Click(object sender, EventArgs e)
    {
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

          btnExportarResultados.Enabled = true;
        }
        else
        {
          GridViewOrdenesPago.Visible = false;
          lblMensajeOrdenesPago.Text = "<p>No existen datos.</p><p>Introduzca otros valores en su consulta.</p>";

          btnExportarResultados.Enabled = false;
        }

        LabelMensaje.Visible = false;
      }
      else
      {
        btnExportarResultados.Enabled = false;

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
    #endregion
  }
}