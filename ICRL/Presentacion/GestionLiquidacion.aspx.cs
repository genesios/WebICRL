using ICRL.BD;
using System;
using System.Collections.Generic;
using System.Data;
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
      DateTime desde = Convert.ToDateTime(txbFechaDesde.Text);
      DateTime hasta = DateTime.Parse(args.Value);

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
    #endregion

    #region Metodos de Soporte
    private void InicializarRangosFechaBusqueda()
    {
      DateTime hoy = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

      if (string.IsNullOrWhiteSpace(txbFechaDesde.Text))
      {
        txbFechaDesde.Text = hoy.ToShortDateString().ToString(System.Globalization.CultureInfo.CurrentCulture);
      }

      if (string.IsNullOrWhiteSpace(txbFechaHasta.Text))
      {
        txbFechaHasta.Text = hoy.AddMonths(1).ToShortDateString().ToString(System.Globalization.CultureInfo.CurrentCulture);
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
      DateTime fechaInicio = Convert.ToDateTime(txbFechaDesde.Text.Trim());
      DateTime fechaFin = Convert.ToDateTime(txbFechaHasta.Text.Trim());

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
        }
        else
        {
          GridViewOrdenesPago.Visible = false;
          lblMensajeOrdenesPago.Text = "<p>No existen datos.</p><p>Introduzca otros valores en su consulta.</p>";
        }

        LabelMensaje.Visible = false;
      }
      else
      {
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