using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ICRL.BD;
using ICRL.ModeloDB;
using Microsoft.Reporting.WebForms;
using System.Globalization;

namespace IRCL.Presentacion
{
  public partial class GestionCotizacion : System.Web.UI.Page
  {
    public string vColorFilaGrid = "#E3EAEB";
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
      DateTime vFechaIni = DateTime.Now;
      DateTime vFechaFin = DateTime.Now;
      if (!VerificarPagina(false)) return;

      bool vAcceso = false;
      vAcceso = FValidaRol("ICRLCotizacionAdministrador", (string[])(Session["RolesUsr"]));
      if (!vAcceso)
      {
        vAcceso = FValidaRol("ICRLCotizacionUsuario", (string[])(Session["RolesUsr"]));
        if (!vAcceso)
        {
          vAcceso = FValidaRol("ICRLCotizacionAuditor", (string[])(Session["RolesUsr"]));
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

      if (bSoloAuditor)
      {
        ButtonCreaInspeccion.Enabled = false;
      }

      if (null == TextBoxFechaIni_CalendarExtender.SelectedDate)
      {
        vFechaIni = new DateTime(vFechaIni.Year, vFechaIni.Month, 1, 0, 0, 0);
        vFechaIni = vFechaIni.AddMonths(-5);
        TextBoxFechaIni_CalendarExtender.SelectedDate = vFechaIni;
        TextBoxFechaIni.Text = vFechaIni.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
      }

      if (null == TextBoxFechaFin_CalendarExtender.SelectedDate)
      {
        vFechaFin = vFechaIni.AddDays(180);
        TextBoxFechaFin_CalendarExtender.SelectedDate = vFechaFin;
        TextBoxFechaFin.Text = vFechaFin.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
      }


      Label4.Text = string.Empty;

      if (!IsPostBack)
      {
        PBusquedaCotizaciones();
        FlTraeNomenCoberturas();
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

    protected void ButtonBuscarFlujo_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      Label4.Text = string.Empty;
      PBusquedaCotizaciones();
    }

    protected void ButtonCreaCotizacion_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      Label4.Text = string.Empty;
      TextBoxPlaca.Text = string.Empty;
      if (string.Empty != TextBoxNroFlujo.Text)
      {
        //mostrar el Popup de selección
        Session["PopupModalCoberturas"] = 1;
        this.ModalPopupCoberturas.Show();
      }
      else
      {
        //mostrar mensaje de error
        Label4.Text = "NO se puede crear una cotización sin flujo Onbase asociado";
      }
    }

    protected void GridViewMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      if (!VerificarPagina(true)) return;
      if (e.Row.RowType == DataControlRowType.DataRow)
      {
        e.Row.Attributes["onmouseover"] = "this.style.backgroundColor='aquamarine';";
        //e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='white';";
        e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='"+ vColorFilaGrid + "';";
        if ( "#E3EAEB" == vColorFilaGrid )
        {
          vColorFilaGrid = "white";
        }
        else
        {
          vColorFilaGrid = "#E3EAEB";
        }

        DateTime vFechaIni = DateTime.ParseExact(TextBoxFechaIni.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        TextBoxFechaIni_CalendarExtender.SelectedDate = vFechaIni;

        DateTime vFechaFin = DateTime.ParseExact(TextBoxFechaFin.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        TextBoxFechaFin_CalendarExtender.SelectedDate = vFechaFin;

        string vIdFlujo = string.Empty;
        int vIdInspeccion = 0;

        vIdFlujo = e.Row.Cells[2].Text;
        vIdInspeccion = int.Parse(vIdFlujo);

        AccesoDatos vAccesoDatos = new AccesoDatos();
        var gvInspecciones = (GridView)e.Row.FindControl("gvInspecciones");

        vFechaFin = vFechaFin.AddDays(1);
        using (LBCDesaEntities db = new LBCDesaEntities())
        {

          var vLst = (from c in db.Cotizacion
                      join cf in db.CotizacionFlujo on c.idFlujo equals cf.idFlujo
                      where (c.idFlujo == vIdInspeccion)
                      && (c.fechaCreacion >= vFechaIni && c.fechaCreacion <= vFechaFin)
                      && (c.tipoCobertura == (int)AccesoDatos.TipoInspeccion.DaniosPropios)
                      orderby c.idInspeccion
                      select new
                      {
                        c.idCotizacion,
                        tipoCobertura = "Daños Propios",
                        secuencialOrden = "DP - Pendiente",
                        nombreProveedor = "N/A",
                        correlativoInspeccion = c.correlativo,
                        c.fechaCreacion,
                        sumaCosto = 0,
                        descEstado = "Coti.Pendiente"
                      }).Union
                      (from c in db.Cotizacion
                       join cf in db.CotizacionFlujo on c.idFlujo equals cf.idFlujo
                       where (c.idFlujo == vIdInspeccion)
                       && (c.fechaCreacion >= vFechaIni && c.fechaCreacion <= vFechaFin)
                       && (c.tipoCobertura == (int)AccesoDatos.TipoInspeccion.RoboParcial)
                       orderby c.idInspeccion
                       select new
                       {
                         c.idCotizacion,
                         tipoCobertura = "Robo Parcial",
                         secuencialOrden = "RP - Pendiente",
                         nombreProveedor = "N/A",
                         correlativoInspeccion = c.correlativo,
                         c.fechaCreacion,
                         sumaCosto = 0,
                         descEstado = "Coti.Pendiente"
                       }).Union
                      (from c in db.Cotizacion
                       join cf in db.CotizacionFlujo on c.idFlujo equals cf.idFlujo
                       where (c.idFlujo == vIdInspeccion)
                       && (c.fechaCreacion >= vFechaIni && c.fechaCreacion <= vFechaFin)
                       && (c.tipoCobertura == (int)AccesoDatos.TipoInspeccion.RCVEhicular)
                       orderby c.idInspeccion
                       select new
                       {
                         c.idCotizacion,
                         tipoCobertura = "RC Vehicular",
                         secuencialOrden = "RCV - Pendiente",
                         nombreProveedor = "N/A",
                         correlativoInspeccion = c.correlativo,
                         c.fechaCreacion,
                         sumaCosto = 0,
                         descEstado = "Coti.Pendiente"
                       }).Union
                      (from c in db.Cotizacion
                       join cf in db.CotizacionFlujo on c.idFlujo equals cf.idFlujo
                       where (c.idFlujo == vIdInspeccion)
                       && (c.fechaCreacion >= vFechaIni && c.fechaCreacion <= vFechaFin)
                       && (c.tipoCobertura == (int)AccesoDatos.TipoInspeccion.RCPersonas)
                       orderby c.idInspeccion
                       select new
                       {
                         c.idCotizacion,
                         tipoCobertura = "RC Personas",
                         secuencialOrden = "RCPer - Pendiente",
                         nombreProveedor = "N/A",
                         correlativoInspeccion = c.correlativo,
                         c.fechaCreacion,
                         sumaCosto = 0,
                         descEstado = "Coti.Pendiente"
                       }).Union
                      (from c in db.Cotizacion
                       join cf in db.CotizacionFlujo on c.idFlujo equals cf.idFlujo
                       where (c.idFlujo == vIdInspeccion)
                       && (c.fechaCreacion >= vFechaIni && c.fechaCreacion <= vFechaFin)
                       && (c.tipoCobertura == (int)AccesoDatos.TipoInspeccion.RCObjetos)
                       orderby c.idInspeccion
                       select new
                       {
                         c.idCotizacion,
                         tipoCobertura = "RC Objetos",
                         secuencialOrden = "RCObj - Pendiente",
                         nombreProveedor = "N/A",
                         correlativoInspeccion = c.correlativo,
                         c.fechaCreacion,
                         sumaCosto = 0,
                         descEstado = "Coti.Pendiente"
                       }).Union
                      (from c in db.Cotizacion
                       join cf in db.CotizacionFlujo on c.idFlujo equals cf.idFlujo
                       where (c.idFlujo == vIdInspeccion)
                       && (c.fechaCreacion >= vFechaIni && c.fechaCreacion <= vFechaFin)
                       && (c.tipoCobertura == (int)AccesoDatos.TipoInspeccion.PerdidaTotalRobo)
                       orderby c.idInspeccion
                       select new
                       {
                         c.idCotizacion,
                         tipoCobertura = "PT Robo",
                         secuencialOrden = "PT Robo - Pendiente",
                         nombreProveedor = "N/A",
                         correlativoInspeccion = c.correlativo,
                         c.fechaCreacion,
                         sumaCosto = 0,
                         descEstado = "Coti.Pendiente"
                       }).Union
                      (from c in db.Cotizacion
                       join cf in db.CotizacionFlujo on c.idFlujo equals cf.idFlujo
                       where (c.idFlujo == vIdInspeccion)
                       && (c.fechaCreacion >= vFechaIni && c.fechaCreacion <= vFechaFin)
                       && (c.tipoCobertura == (int)AccesoDatos.TipoInspeccion.PerdidaTotalDaniosPropios)
                       orderby c.idInspeccion
                       select new
                       {
                         c.idCotizacion,
                         tipoCobertura = "PT Danios Propios",
                         secuencialOrden = "PT Daños Propios - Pendiente",
                         nombreProveedor = "N/A",
                         correlativoInspeccion = c.correlativo,
                         c.fechaCreacion,
                         sumaCosto = 0,
                         descEstado = "Coti.Pendiente"
                       });

          gvInspecciones.DataSource = vLst.ToList();
          gvInspecciones.DataBind();
        }
      }
    }

    protected void GridViewMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
      if (!VerificarPagina(true)) return;
      //se realiza una búsqueda por prioridad
      //primero por flujo, después por placa y finalmente por fecha
      //validar los campos de busqueda
      DateTime vFechaIni = DateTime.Now;
      DateTime vFechaFin = DateTime.Now;

      string vNroFlujo = null;
      string vPlaca = null;

      if (TextBoxNroFlujo.Text != string.Empty)
        vNroFlujo = TextBoxNroFlujo.Text;

      if (TextBoxPlaca.Text != string.Empty)
        vPlaca = TextBoxPlaca.Text;

      if (null == TextBoxFechaIni_CalendarExtender.SelectedDate)
      {
        vFechaIni = new DateTime(vFechaIni.Year, vFechaIni.Month, 1, 0, 0, 0);
        vFechaIni = vFechaIni.AddMonths(-5);
        TextBoxFechaIni_CalendarExtender.SelectedDate = vFechaIni;
      }
      else
      {
        vFechaIni = DateTime.ParseExact(TextBoxFechaIni.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
      }

      if (null == TextBoxFechaFin_CalendarExtender.SelectedDate)
      {
        vFechaFin = vFechaIni.AddDays(180);
        TextBoxFechaFin_CalendarExtender.SelectedDate = vFechaFin;
      }
      else
      {
        vFechaFin = DateTime.ParseExact(TextBoxFechaFin.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
      }

      vFechaFin = vFechaFin.AddDays(1);
      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vLst = from c in db.Cotizacion
                   join u in db.Usuario on c.idUsuario equals u.idUsuario
                   join f in db.Flujo on c.idFlujo equals f.idFlujo
                   where (vNroFlujo == null || f.flujoOnBase == vNroFlujo)
                   && (vPlaca == null || f.placaVehiculo == vPlaca)
                   && (c.fechaCreacion >= vFechaIni && c.fechaCreacion <= vFechaFin)
                   orderby f.flujoOnBase
                   select new
                   {
                     f.idFlujo,
                     f.flujoOnBase,
                     f.nombreAsegurado,
                     f.numeroPoliza,
                     f.placaVehiculo,
                     f.fechaSiniestro,
                     descEstado = "Pendiente"
                   };

        GridViewMaster.DataSource = vLst.Distinct().ToList();
        GridViewMaster.PageIndex = e.NewPageIndex;
        GridViewMaster.DataBind();
      }
    }

    public string MyNewRowCot(object pFlujoOnBase)
    {
      return String.Format(@"</td></tr><tr id ='tr{0}' class='collapsed-row'>
                                <td></td><td colspan='100' style='padding:0px; margin:0px;'>", pFlujoOnBase);
    }

    protected void GridViewgvInspecciones_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      string vFilaFlujo = string.Empty;
      string vCobertura = string.Empty;
      int vIdFlujo = 0;

      var gvInspecciones = (GridView)sender;
      vFilaFlujo = gvInspecciones.SelectedRow.Cells[0].Text;
      vCobertura = gvInspecciones.SelectedRow.Cells[1].Text;
      vCobertura = vCobertura.Replace("&#241;", "ñ");


      int vIdCotizacion = int.Parse(vFilaFlujo);
      AccesoDatos vAccesoDatos = new AccesoDatos();
      vIdFlujo = vAccesoDatos.FTraeIdFlujoCotizacion(vIdCotizacion);

      Session["NumFlujo"] = vIdFlujo;

      switch (vCobertura)
      {
        case "Daños Propios":
          Response.Redirect("~/Presentacion/CotizacionDP.aspx?nroCoti=" + vIdCotizacion.ToString());
          break;
        case "RC Objetos":
          Response.Redirect("~/Presentacion/CotizacionRCObj.aspx?nroCoti=" + vIdCotizacion.ToString());
          break;
        case "RC Personas":
          Response.Redirect("~/Presentacion/CotizacionRCPer.aspx?nroCoti=" + vIdCotizacion.ToString());
          break;
        case "Robo Parcial":
          Response.Redirect("~/Presentacion/CotizacionRP.aspx?nroCoti=" + vIdCotizacion.ToString());
          break;
        case "RC Vehicular":
          Response.Redirect("~/Presentacion/CotizacionRCVehicular.aspx?nroCoti=" + vIdCotizacion.ToString());
          break;
        case "PT Robo":
          Response.Redirect("~/Presentacion/CotizacionPerTotRobo.aspx?nroCoti=" + vIdCotizacion.ToString());
          break;
        case "PT Danios Propios":
          Response.Redirect("~/Presentacion/CotizacionPerTotDP.aspx?nroCoti=" + vIdCotizacion.ToString());
          break;
        default:
          break;
      }

    }

    #region Grilla Maestra

    protected int PBusquedaCotizaciones()
    {
      //se realiza una búsqueda por prioridad
      //primero por flujo, después por placa y finalmente por fecha
      //validar los campos de busqueda
      int vResul = 1;
      DateTime vFechaIni = DateTime.Now;
      DateTime vFechaFin = DateTime.Now;

      string vNroFlujo = null;
      string vPlaca = null;

      if (TextBoxNroFlujo.Text != string.Empty)
        vNroFlujo = TextBoxNroFlujo.Text;

      if (TextBoxPlaca.Text != string.Empty)
        vPlaca = TextBoxPlaca.Text;

      if (null == TextBoxFechaIni_CalendarExtender.SelectedDate)
      {
        vFechaIni = new DateTime(vFechaIni.Year, vFechaIni.Month, 1, 0, 0, 0);
        vFechaIni = vFechaIni.AddMonths(-5);
        TextBoxFechaIni_CalendarExtender.SelectedDate = vFechaIni;
      }
      else
      {
        vFechaIni = DateTime.ParseExact(TextBoxFechaIni.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
      }

      if (null == TextBoxFechaFin_CalendarExtender.SelectedDate)
      {
        vFechaFin = vFechaIni.AddDays(180);
        TextBoxFechaFin_CalendarExtender.SelectedDate = vFechaFin;
      }
      else
      {
        vFechaFin = DateTime.ParseExact(TextBoxFechaFin.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
      }

      vFechaFin = vFechaFin.AddDays(1);
      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vLst = from c in db.Cotizacion
                   join u in db.Usuario on c.idUsuario equals u.idUsuario
                   join f in db.Flujo on c.idFlujo equals f.idFlujo
                   where (vNroFlujo == null || f.flujoOnBase == vNroFlujo)
                   && (vPlaca == null || f.placaVehiculo == vPlaca)
                   && (c.fechaCreacion >= vFechaIni && c.fechaCreacion <= vFechaFin)
                   orderby f.flujoOnBase
                   select new
                   {
                     f.idFlujo,
                     f.flujoOnBase,
                     f.nombreAsegurado,
                     f.numeroPoliza,
                     f.placaVehiculo,
                     f.fechaSiniestro,
                     descEstado = "Pendiente"
                   };

        GridViewMaster.DataSource = vLst.Distinct().ToList();
        GridViewMaster.DataBind();
      }

      return vResul;

    }
    #endregion

    #region Creacion Cotizacion

    private int FlTraeNomenCoberturas()
    {
      int vResultado = 0;
      string vCategoria = "Coberturas";
      int vOrdenCodigo = 2;
      AccesoDatos vAccesoDatos = new AccesoDatos();

      DropDownListCoberturas.DataValueField = "codigo";
      DropDownListCoberturas.DataTextField = "descripcion";
      DropDownListCoberturas.DataSource = vAccesoDatos.FlTraeNomenGenerico(vCategoria, vOrdenCodigo);
      DropDownListCoberturas.DataBind();

      return vResultado;
    }

    protected void ButtonCoberturaCrear_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      int vResultado = 0;
      int vTipoCobertura = 0;
      string vIdCobertura = string.Empty;

      Label4.Text = string.Empty;
      vIdCobertura = DropDownListCoberturas.SelectedValue.ToString().Trim();
      vTipoCobertura = int.Parse(vIdCobertura);

      int vResultadoFlujo = 0;
      vResultadoFlujo = PTraeFlujoOnBase(TextBoxNroFlujo.Text.ToUpper());
      if (1 == vResultadoFlujo)
      {
        vResultado = FCreaCotizacion(vTipoCobertura);
        if (vResultado > 0)
        {
          Label4.Text = "Cotización creada exitosamente";
        }
        else
        {
          Label4.Text = "No se pudo crear la cotización";
        }
        Session["PopupModalCoberturas"] = 0;
        this.ModalPopupCoberturas.Hide();
      }
      else
      {
        Label4.Text = "NO existe el Flujo " + TextBoxNroFlujo.Text.ToUpper() + " en OnBase, verifique";
      }

      
      PBusquedaCotizaciones();
    }

    protected void ButtonCoberturaCancelar_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      Session["PopupModalCoberturas"] = 0;
      this.ModalPopupCoberturas.Hide();
    }

    public int FCreaCotizacion(int pTipoCobertura)
    {
      int vResultado = 0;
      int vResultadoCotiFlujo = 0;
      int vIdFlujo = 0;
      AccesoDatos vAccesoDatos = new AccesoDatos();
      int vTipoCobertura = 0;
      string vCodUsuario = Session["IdUsr"].ToString();

      int vIdUsuario = vAccesoDatos.FValidaExisteUsuarioICRL(vCodUsuario);

      vIdFlujo = vAccesoDatos.FValidaExisteFlujoICRL(TextBoxNroFlujo.Text);

      switch (pTipoCobertura)
      {
        case 1:
          vTipoCobertura = (int)AccesoDatos.TipoInspeccion.DaniosPropios;
          break;
        case 2:
          vTipoCobertura = (int)AccesoDatos.TipoInspeccion.RCObjetos;
          break;
        case 3:
          vTipoCobertura = (int)AccesoDatos.TipoInspeccion.RCPersonas;
          break;
        case 4:
          vTipoCobertura = (int)AccesoDatos.TipoInspeccion.RoboParcial;
          break;
        case 5:
          vTipoCobertura = (int)AccesoDatos.TipoInspeccion.PerdidaTotalDaniosPropios;
          break;
        case 6:
          vTipoCobertura = (int)AccesoDatos.TipoInspeccion.PerdidaTotalRobo;
          break;
        case 7:
          vTipoCobertura = (int)AccesoDatos.TipoInspeccion.RCVEhicular;
          break;
        default:
          Label4.Text = "Error en la selección de Cobertura";
          break;
      }

      Coti vCoti = new Coti();
      vCoti.idUsuario = vIdUsuario;
      vCoti.idFlujo = vIdFlujo;
      vCoti.fechaCreacion = DateTime.Today;
      vCoti.inspector = Session["NomUsr"].ToString();
      vCoti.sucursal = string.Empty;
      vCoti.idInspeccion = 0;
      vCoti.estado = 1;
      vCoti.tipoCobertura = vTipoCobertura;
      vCoti.correlativo = 1;
      vCoti.tipoTaller = string.Empty;

      vResultado = vAccesoDatos.FGrabaCotiICRL(vCoti);

      CotiFlujo vCotiFlujo = new CotiFlujo();
      vCotiFlujo.idFlujo = vIdFlujo;
      vCotiFlujo.idUsuario = vIdUsuario;
      vCotiFlujo.fechaCreacion = DateTime.Today;
      vCotiFlujo.observacionesSiniestro = string.Empty;
      vCotiFlujo.inspector = Session["NomUsr"].ToString();
      vCotiFlujo.nombreContacto = string.Empty;
      vCotiFlujo.telefonoContacto = string.Empty;
      vCotiFlujo.correosDeEnvio = string.Empty;
      vCotiFlujo.estado = 1;
      vCotiFlujo.fechaSiniestro = DateTime.Today;
      vCotiFlujo.correlativo = 1;
      vCotiFlujo.usuario_modificacion = vIdUsuario;
      vCotiFlujo.fechaModificacion = DateTime.Now;

      //VAlidar si ya existe el registro FlujoCotizacion
      vResultadoCotiFlujo = vAccesoDatos.FValidaExisteCotiFlujoICRL(vIdFlujo);
      if (0 == vResultadoCotiFlujo)
      {
        vResultadoCotiFlujo = vAccesoDatos.FGrabaCotiFlujoICRL(vCotiFlujo);
      }

      return vResultado;
    }

    int PTraeFlujoOnBase(string pNroFlujo)
    {
      int vResultado = 0;
      var vAccesoDatos = new AccesoDatos();
      var vFlujoICRL = new FlujoICRL();
      TextBoxPlaca.Text = string.Empty;

      vResultado = vAccesoDatos.FValidaExisteFlujoOnBase(pNroFlujo);
      if (1 == vResultado)
      {
        vFlujoICRL = vAccesoDatos.FTraeDatosFlujoOnBase(pNroFlujo);
        //TextBoxPlaca.Text = vFlujoICRL.placaVehiculo;
        int vRespuesta = vAccesoDatos.FValidaExisteFlujoICRL(pNroFlujo);
        if (0 == vRespuesta)
        {
          int vGrabacion = vAccesoDatos.FGrabaFlujoICRL(vFlujoICRL);
          if (0 == vGrabacion)
          {
            Label4.Text = "Error de Grabacion Flujo ICRL";
          }
        }
        else
        {
          vFlujoICRL.idFlujo = vRespuesta;
          //ajuste para no perder el contador
          int vContador = 0;
          vContador = vAccesoDatos.FTraeContadorFlujoICRL(TextBoxNroFlujo.Text);
          vFlujoICRL.contador = vContador;
          //fin ajuste
          int vActualizacion = vAccesoDatos.FActualizaFlujoICRL(vFlujoICRL);
          if (0 == vActualizacion)
          {
            Label4.Text = "Error de actualizacion Flujo ICRL";
          }
        }

      }
      else
      {
        TextBoxPlaca.Text = "Flujo No encontrado";
      }
      return vResultado;
    }

    #endregion


  }
}