using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LbcOnBaseWS;
using ICRL.BD;
using ICRL.ModeloDB;
using Microsoft.Reporting.WebForms;
using System.Globalization;

namespace ICRL.Presentacion
{

  public partial class GestionInspeccion : System.Web.UI.Page
  {
    public int gRespuestaSiNo = -1;
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
      DateTime vFechaIni = DateTime.Now;
      DateTime vFechaFin = DateTime.Now;
      if (null == TextBoxFechaIni_CalendarExtender.SelectedDate)
      {
        vFechaIni = new DateTime(vFechaIni.Year, vFechaIni.Month, 1, 0, 0, 0);
        vFechaIni = vFechaIni.AddMonths(-1);
        TextBoxFechaIni_CalendarExtender.SelectedDate = vFechaIni;
        TextBoxFechaIni.Text = vFechaIni.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
      }

      if (null == TextBoxFechaFin_CalendarExtender.SelectedDate)
      {
        vFechaFin = vFechaIni.AddDays(60);
        TextBoxFechaFin_CalendarExtender.SelectedDate = vFechaFin;
        TextBoxFechaFin.Text = vFechaFin.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
      }

      TextBoxInspector.Text = Session["NomUsr"].ToString();
      TextBoxSucAtencion.Text = Session["SucursalUsr"].ToString();
      TextBoxEstado.Text = String.Empty;
      Label4.Text = string.Empty;


      ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;

      if (!IsPostBack)
      {
        PBusquedaInspecciones();

        //int vIdFlujo = 2;

        ////////AccesoDatos vAccesoDatos = new AccesoDatos();
        ////////LBCDesaEntities db = new LBCDesaEntities();
        ////////var vListaInsp = from i in db.Inspeccion
        ////////                 join f in db.Flujo on i.idFlujo equals f.idFlujo
        ////////                 select new
        ////////                 {
        ////////                     f.flujoOnBase,
        ////////                     i.idInspeccion,
        ////////                     i.fechaCreacion,
        ////////                     i.idInspector,
        ////////                     f.numeroPoliza
        ////////                 };

        //////////GridViewInspecciones.DataSource = vLst.ToList();
        //////////GridViewInspecciones.DataBind();
        //////////var vListaInsp = vAccesoDatos.TraeInspeccionesPorFlujo(vIdFlujo);

        ////////ReportDataSource datasource = new ReportDataSource("DataSet1", vListaInsp);
        ////////ReportViewer1.LocalReport.DataSources.Clear();
        ////////ReportViewer1.LocalReport.DataSources.Add(datasource);
      }

      if (Session["PopupModalSiNo"] != null)
      {
        int vPopup = -1;
        vPopup = int.Parse(Session["PopupModalSiNo"].ToString());
        if (1 == vPopup)
          this.ModalPopupSiNo.Show();
        else
          this.ModalPopupSiNo.Hide();
      }


    }


    private int flTraeDatosFlujo(string pNumFlujo)
    {
      int vResultado = 0;

      /*** CONECTAR A WS ***/
      var servicioOnBase = new OnBaseWS();
      /*** ESTABLECER LA APLICACIÓN ORIGEN POR DEFECTO PARA GESPRO ***/
      var origen = SistemaOrigen.ICRL;
      /*** INSTANCIAR EL RESULTADO COMO ResultadoEntity ***/
      ResultadoEntity resultado = new ResultadoEntity();
      /*** LLAMAR A LA FUNCIÓN DEL WS ***/
      resultado = servicioOnBase.ObtenerInformacionSolicitudOnBase(pNumFlujo, origen);

      /*** SI EL RESULTADO ES CORRECTO, SE EXTRAE LA INFORMACIÓN DEL FLUJO ***/
      if (resultado.EsValido)
      {
        // MOSTRAR DATOS EN LA APLICACIÓN DE PRUEBA
        Session["NumFlujo"] = TextBoxFlujo.Text;
        vResultado = 1;
      }
      else
      {
        Label4.Text = resultado.Mensaje;
      }

      return (vResultado);
    }

    //private int FlTraeInspecciones()
    //{
    //    int vResultado = 0;

    //    using (LBCDesaEntities db = new LBCDesaEntities())
    //    {
    //        var vLst = from i in db.Inspeccion
    //                   join u in db.Usuario on i.idUsuario equals u.idUsuario
    //                   join f in db.Flujo on i.idFlujo equals f.idFlujo
    //                   select new
    //                   {
    //                       f.flujoOnBase,
    //                       i.idInspeccion,
    //                       f.nombreAsegurado,
    //                       f.numeroPoliza,
    //                       f.placaVehiculo,
    //                       i.fecha_siniestro
    //                   };

    //        GridViewInspecciones.DataSource = vLst.ToList();
    //        GridViewInspecciones.DataBind();
    //        vResultado = 1;
    //    }

    //    return vResultado;
    //}


    protected void PCreaInspeccion()
    {
      InspeccionICRL vInspeccionICRL = new InspeccionICRL();
      AccesoDatos vAccesoDatos = new AccesoDatos();
      string vCodUsuario = Session["IdUsr"].ToString();
      int vCorrelativo = 0;

      if (string.Empty == TextBoxNroFlujo.Text.ToUpper())
      {
        //No se tiene Flujo OnBase Asociado
        //Opcion para crear un flujo temporal
        //Se confirma con el usuario


        int vIdFlujoTemporal = 0;
        vIdFlujoTemporal = vAccesoDatos.FGrabaFlujoTempICRL();
        vInspeccionICRL.idFlujo = vIdFlujoTemporal;
        Session["NumFlujo"] = vIdFlujoTemporal;
        vCorrelativo = vAccesoDatos.fObtieneContadorInspeccionFlujo(vIdFlujoTemporal);

        int vIdUsuario = vAccesoDatos.FValidaExisteUsuarioICRL(vCodUsuario);
        if (vIdUsuario > 0)
        {

          vInspeccionICRL.idUsuario = vIdUsuario;
          vInspeccionICRL.sucursalAtencion = Session["SucursalUsr"].ToString();
          vInspeccionICRL.direccion = string.Empty;
          vInspeccionICRL.zona = string.Empty;
          vInspeccionICRL.causaSiniestro = string.Empty;
          vInspeccionICRL.descripcionSiniestro = string.Empty;
          vInspeccionICRL.observacionesInspec = string.Empty;
          vInspeccionICRL.idInspector = vCodUsuario;
          vInspeccionICRL.nombreContacto = string.Empty;
          vInspeccionICRL.telefonoContacto = string.Empty;
          vInspeccionICRL.correosDeEnvio = string.Empty;
          vInspeccionICRL.recomendacionPerdidaTotal = false;
          vInspeccionICRL.estado = 1;
          vInspeccionICRL.fechaSiniestro = DateTime.Now;
          vInspeccionICRL.tipoInspeccion = (int)ICRL.BD.AccesoDatos.TipoInspeccion.DaniosPropios;
          vInspeccionICRL.correlativo = vCorrelativo;

          int vRespuesta = vAccesoDatos.FGrabaInspeccionICRL(vInspeccionICRL);

          Response.Redirect("~/Presentacion/Inspeccion.aspx?nroInsp=" + vRespuesta.ToString());
        }
      }
      else
      {

        //Se tiene Flujo OnBase Asociado
        int vIdFlujo = vAccesoDatos.FValidaExisteFlujoICRL(TextBoxNroFlujo.Text.ToUpper());
        int vIdInspeccion = 0;
        vInspeccionICRL.idFlujo = vIdFlujo;
        Session["NumFlujo"] = vIdFlujo;


        //Validar si el Flujo tiene InspeccionDaniosPropios
        //Solo si no es asi se debe crear una inspección DaniosPropios
        vIdInspeccion = vAccesoDatos.FFlujoTieneDaniosPropios(vIdFlujo);

        if (vIdInspeccion > 0)
        {
          Response.Redirect("~/Presentacion/Inspeccion.aspx?nroInsp=" + vIdInspeccion.ToString());
        }
        else
        {

          vCorrelativo = vAccesoDatos.fObtieneContadorInspeccionFlujo(vIdFlujo);

          int vIdUsuario = vAccesoDatos.FValidaExisteUsuarioICRL(vCodUsuario);
          if (vIdUsuario > 0)
          {

            vInspeccionICRL.idUsuario = vIdUsuario;
            vInspeccionICRL.sucursalAtencion = Session["SucursalUsr"].ToString();
            vInspeccionICRL.direccion = string.Empty;
            vInspeccionICRL.zona = string.Empty;
            vInspeccionICRL.causaSiniestro = string.Empty;
            vInspeccionICRL.descripcionSiniestro = string.Empty;
            vInspeccionICRL.observacionesInspec = string.Empty;
            vInspeccionICRL.idInspector = vCodUsuario;
            vInspeccionICRL.nombreContacto = string.Empty;
            vInspeccionICRL.telefonoContacto = string.Empty;
            vInspeccionICRL.correosDeEnvio = string.Empty;
            vInspeccionICRL.recomendacionPerdidaTotal = false;
            vInspeccionICRL.estado = 1;
            vInspeccionICRL.fechaSiniestro = DateTime.Now;
            vInspeccionICRL.tipoInspeccion = (int)ICRL.BD.AccesoDatos.TipoInspeccion.DaniosPropios;
            vInspeccionICRL.correlativo = vCorrelativo;
            vInspeccionICRL.tipoTaller = string.Empty;

            int vRespuesta = vAccesoDatos.FGrabaInspeccionICRL(vInspeccionICRL);

            Response.Redirect("~/Presentacion/Inspeccion.aspx?nroInsp=" + vRespuesta.ToString());
          }
        }
      }



    }
    protected void ButtonCreaInspeccion_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      Label4.Text = string.Empty;
      TextBoxPlaca.Text = string.Empty;
      if (string.Empty == TextBoxNroFlujo.Text.ToUpper())
      {
        TextBoxPopupSiNo.Text = "No se tiene Flujo Asociado, se creara un Flujo Temporal";
        Session["PopupModalSiNo"] = 1;
        this.ModalPopupSiNo.Show();
      }
      else
      {
        int vResultadoFlujo = 0;
        vResultadoFlujo = PTraeFlujoOnBase(TextBoxNroFlujo.Text.ToUpper());
        if (1 == vResultadoFlujo)
        {
          PCreaInspeccion();
        }
        else
        {
          Label4.Text = "NO existe el Flujo " + TextBoxNroFlujo.Text.ToUpper() + " en OnBase, verifique";
        }
      }


      //InspeccionICRL vInspeccionICRL = new InspeccionICRL();
      //AccesoDatos vAccesoDatos = new AccesoDatos();
      //string vCodUsuario = Session["IdUsr"].ToString();
      //int vCorrelativo = 0;

      //if (string.Empty == TextBoxNroFlujo.Text)
      //{
      //    //No se tiene Flujo OnBase Asociado
      //    //Opcion para crear un flujo temporal
      //    //Se confirma con el usuario


      //    int vIdFlujoTemporal = 0;
      //    vIdFlujoTemporal = vAccesoDatos.FGrabaFlujoTempICRL();
      //    vInspeccionICRL.idFlujo = vIdFlujoTemporal;
      //    Session["NumFlujo"] = vIdFlujoTemporal;
      //    vCorrelativo = vAccesoDatos.fObtieneContadorInspeccionFlujo(vIdFlujoTemporal);
      //}
      //else
      //{
      //    //Se tiene Flujo OnBase Asociado
      //    int vIdFlujo = vAccesoDatos.FValidaExisteFlujoICRL(TextBoxNroFlujo.Text);
      //    vInspeccionICRL.idFlujo = vIdFlujo;
      //    Session["NumFlujo"] = vIdFlujo;
      //    vCorrelativo = vAccesoDatos.fObtieneContadorInspeccionFlujo(vIdFlujo);
      //}


      //int vIdUsuario = vAccesoDatos.FValidaExisteUsuarioICRL(vCodUsuario);
      //if (vIdUsuario > 0)
      //{

      //    vInspeccionICRL.idUsuario = vIdUsuario;
      //    vInspeccionICRL.sucursalAtencion = Session["SucursalUsr"].ToString();
      //    vInspeccionICRL.direccion = string.Empty;
      //    vInspeccionICRL.zona = string.Empty;
      //    vInspeccionICRL.causaSiniestro = string.Empty;
      //    vInspeccionICRL.descripcionSiniestro = string.Empty;
      //    vInspeccionICRL.observacionesInspec = string.Empty;
      //    vInspeccionICRL.idInspector = vCodUsuario;
      //    vInspeccionICRL.nombreContacto = string.Empty;
      //    vInspeccionICRL.telefonoContacto = string.Empty;
      //    vInspeccionICRL.correosDeEnvio = string.Empty;
      //    vInspeccionICRL.recomendacionPerdidaTotal = false;
      //    vInspeccionICRL.estado = 1;
      //    vInspeccionICRL.fechaSiniestro = DateTime.Now;
      //    vInspeccionICRL.tipoInspeccion = (int)ICRL.BD.AccesoDatos.TipoInspeccion.DaniosPropios;
      //    vInspeccionICRL.correlativo = vCorrelativo;

      //    int vRespuesta = vAccesoDatos.FGrabaInspeccionICRL(vInspeccionICRL);

      //    Response.Redirect("~/Presentacion/Inspeccion.aspx?nroInsp=" + vRespuesta.ToString());
      //}
    }

    protected void ButtonBuscarFlujo_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      PBusquedaInspecciones();
    }

    //protected void FBusquedaOriginal()
    //{
    //    int vResultado = 0;
    //    var vAccesoDatos = new AccesoDatos();
    //    var vFlujoICRL = new FlujoICRL();
    //    TextBoxPlaca.Text = string.Empty;

    //    vResultado = vAccesoDatos.FValidaExisteFlujoOnBase(TextBoxNroFlujo.Text);
    //    if (1 == vResultado)
    //    {
    //        vFlujoICRL = vAccesoDatos.FTraeDatosFlujoOnBase(TextBoxNroFlujo.Text);
    //        TextBoxPlaca.Text = vFlujoICRL.placaVehiculo;
    //        int vRespuesta = vAccesoDatos.FValidaExisteFlujoICRL(TextBoxNroFlujo.Text);
    //        if (0 == vRespuesta)
    //        {
    //            int vGrabacion = vAccesoDatos.FGrabaFlujoICRL(vFlujoICRL);
    //            if (0 == vGrabacion)
    //            {
    //                Label4.Text = "Error de Grabacion Flujo ICRL";
    //            }
    //        }
    //        else
    //        {
    //            vFlujoICRL.idFlujo = vRespuesta;
    //            int vActualizacion = vAccesoDatos.FActualizaFlujoICRL(vFlujoICRL);
    //            if (0 == vActualizacion)
    //            {
    //                Label4.Text = "Error de actualizacion Flujo ICRL";
    //            }
    //        }

    //    }
    //    int vSalida = FlTraeInspecciones();
    //}


    //protected void GridViewInspecciones_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    string vFilaFlujo = GridViewInspecciones.SelectedRow.Cells[1].Text;
    //    int vIdInspeccion = int.Parse(GridViewInspecciones.SelectedRow.Cells[2].Text);
    //    Label4.Text = "<b>Fila Flujo   :     " + vFilaFlujo + "</b>";
    //    TextBoxFlujo.Text = vFilaFlujo;
    //    Session["NumFlujo"] = TextBoxFlujo.Text;

    //    Response.Redirect("~/Presentacion/Inspeccion.aspx?nroInsp=" + vIdInspeccion.ToString());
    //}

    //protected void GridViewInspecciones_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        e.Row.Attributes["onmouseover"] = "this.style.backgroundColor='aquamarine';";
    //        e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='white';";
    //        e.Row.ToolTip = "Haz clic en la primera columna para seleccionar la fila.";
    //    }
    //}

    protected void ButtonFlujoInspeccion_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      try
      {
        if (1 == flTraeDatosFlujo(TextBoxFlujo.Text))
        {
          Response.Redirect("~/Presentacion/Inspeccion.aspx");
        }
      }
      catch (Exception ex)
      {

        Label4.Text = string.Empty;
        Label4.Text = ex.Message;
      }
    }

    protected void ButtonMaster_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      //FlTraeInspeccionesPadre();
      PBusquedaInspecciones();
    }

    private int FlTraeInspeccionesPadre()
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vLst = from i in db.Inspeccion
                   join u in db.Usuario on i.idUsuario equals u.idUsuario
                   join f in db.Flujo on i.idFlujo equals f.idFlujo
                   orderby f.flujoOnBase
                   select new
                   {
                     f.idFlujo,
                     f.flujoOnBase,
                     f.nombreAsegurado,
                     f.numeroPoliza,
                     f.placaVehiculo,
                     f.docIdAsegurado
                   };

        GridViewMaster.DataSource = vLst.Distinct().ToList();
        GridViewMaster.DataBind();
        vResultado = 1;
      }

      return vResultado;
    }

    public string MyNewRow(object pFlujoOnBase)
    {
      return String.Format(@"</td></tr><tr id ='tr{0}' class='collapsed-row'>
                                <td></td><td colspan='100' style='padding:0px; margin:0px;'>", pFlujoOnBase);
    }

    private int FlTraeInspeccionesHijo(string pFlujoOnBase)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vLst = from i in db.Inspeccion
                   join u in db.Usuario on i.idUsuario equals u.idUsuario
                   join f in db.Flujo on i.idFlujo equals f.idFlujo
                   where f.flujoOnBase == pFlujoOnBase
                   select new
                   {
                     f.flujoOnBase,
                     f.idFlujo,
                     i.idInspeccion,
                     f.placaVehiculo,
                     f.chasisVehiculo
                   };


        //GridViewHijos.DataSource = vLst.ToList();
        //GridViewHijos.DataBind();


        //gvOrders.DataSource = vLst.ToList();
        //gvOrders.DataBind();
        vResultado = 1;
      }

      return vResultado;
    }

    protected void GridViewMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      if (!VerificarPagina(true)) return;
      if (e.Row.RowType == DataControlRowType.DataRow)
      {
        e.Row.Attributes["onmouseover"] = "this.style.backgroundColor='aquamarine';";
        e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='white';";



        DateTime vFechaIni = DateTime.ParseExact(TextBoxFechaIni.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        TextBoxFechaIni_CalendarExtender.SelectedDate = vFechaIni;

        DateTime vFechaFin = DateTime.ParseExact(TextBoxFechaFin.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        TextBoxFechaFin_CalendarExtender.SelectedDate = vFechaFin;

        string vIdFlujo = string.Empty;
        int vIdInspeccion = 0;

        vIdFlujo = e.Row.Cells[2].Text;
        vIdInspeccion = int.Parse(vIdFlujo);

        //Label lblIdInsp = (Label)e.Row.FindControl("lblInspeccion");
        //Label lblNombreAse = (Label)e.Row.FindControl("lblAsegurado");
        //vContador++;
        //Label4.Text = vContador.ToString();

        AccesoDatos vAccesoDatos = new AccesoDatos();
        var gvInspecciones = (GridView)e.Row.FindControl("gvInspecciones");

        vFechaFin = vFechaFin.AddDays(1);
        using (LBCDesaEntities db = new LBCDesaEntities())
        {
          //var vLst = from i in db.Inspeccion
          //           where (i.idFlujo == vIdInspeccion)
          //           && (i.fechaCreacion >= vFechaIni && i.fechaCreacion <= vFechaFin)
          //           orderby i.idInspeccion
          //           select new
          //           {
          //               i.idInspeccion,
          //               i.fechaCreacion,
          //               i.sucursalAtencion,
          //               i.idInspector,
          //               i.nombreContacto
          //           };

          var vLst = (from i in db.Inspeccion
                      join idpp in db.InspDaniosPropiosPadre on i.idInspeccion equals idpp.idInspeccion
                      where (i.idFlujo == vIdInspeccion)
                      && (i.fechaCreacion >= vFechaIni && i.fechaCreacion <= vFechaFin)
                      orderby i.idInspeccion
                      select new
                      {
                        i.idInspeccion,
                        tipoCobertura = "Daños Propios",
                        i.fechaCreacion,
                        i.sucursalAtencion,
                        i.idInspector,
                        nombreContacto = i.nombreContacto + idpp.secuencial,
                      }).Union
                     (from i in db.Inspeccion
                      join ircobj in db.InspRCObjeto on i.idInspeccion equals ircobj.idInspeccion
                      where (i.idFlujo == vIdInspeccion)
                      && (i.fechaCreacion >= vFechaIni && i.fechaCreacion <= vFechaFin)
                      orderby i.idInspeccion
                      select new
                      {
                        i.idInspeccion,
                        tipoCobertura = "RC Objetos",
                        i.fechaCreacion,
                        i.sucursalAtencion,
                        i.idInspector,
                        nombreContacto = ircobj.nombreObjeto
                      }).Union
                      (from i in db.Inspeccion
                       join ircper in db.InspRCPersona on i.idInspeccion equals ircper.idInspeccion
                       where (i.idFlujo == vIdInspeccion)
                       && (i.fechaCreacion >= vFechaIni && i.fechaCreacion <= vFechaFin)
                       orderby i.idInspeccion
                       select new
                       {
                         i.idInspeccion,
                         tipoCobertura = "RC Personas",
                         i.fechaCreacion,
                         i.sucursalAtencion,
                         i.idInspector,
                         nombreContacto = ircper.nombrePersona
                       }).Union
                       (from i in db.Inspeccion
                        join ircveh in db.InspRCVehicular on i.idInspeccion equals ircveh.idInspeccion
                        where (i.idFlujo == vIdInspeccion)
                        && (i.fechaCreacion >= vFechaIni && i.fechaCreacion <= vFechaFin)
                        orderby i.idInspeccion
                        select new
                        {
                          i.idInspeccion,
                          tipoCobertura = "RC Vehiculos",
                          i.fechaCreacion,
                          i.sucursalAtencion,
                          i.idInspector,
                          nombreContacto = ircveh.nombreTercero
                        }).Union
                        (from i in db.Inspeccion
                         join irp in db.InspRoboParcial on i.idInspeccion equals irp.idInspeccion
                         where (i.idFlujo == vIdInspeccion)
                         && (i.fechaCreacion >= vFechaIni && i.fechaCreacion <= vFechaFin)
                         orderby i.idInspeccion
                         select new
                         {
                           i.idInspeccion,
                           tipoCobertura = "Robo Parcial",
                           i.fechaCreacion,
                           i.sucursalAtencion,
                           i.idInspector,
                           i.nombreContacto
                         }).Union
                         (from i in db.Inspeccion
                          join iptdp in db.InspPerdidaTotalDanios on i.idInspeccion equals iptdp.idInspeccion
                          where (i.idFlujo == vIdInspeccion)
                          && (i.fechaCreacion >= vFechaIni && i.fechaCreacion <= vFechaFin)
                          orderby i.idInspeccion
                          select new
                          {
                            i.idInspeccion,
                            tipoCobertura = "Pérdida Total por Daños Propios",
                            i.fechaCreacion,
                            i.sucursalAtencion,
                            i.idInspector,
                            i.nombreContacto
                          }).Union
                          (from i in db.Inspeccion
                           join iptrob in db.InspPerdidaTotalRobo on i.idInspeccion equals iptrob.idInspeccion
                           where (i.idFlujo == vIdInspeccion)
                           && (i.fechaCreacion >= vFechaIni && i.fechaCreacion <= vFechaFin)
                           orderby i.idInspeccion
                           select new
                           {
                             i.idInspeccion,
                             tipoCobertura = "Pérdida Total por Robo",
                             i.fechaCreacion,
                             i.sucursalAtencion,
                             i.idInspector,
                             i.nombreContacto
                           })
                     ;
          gvInspecciones.DataSource = vLst.ToList();
          gvInspecciones.DataBind();
        }



        //var vListaInsp = vAccesoDatos.TraeInspeccionesPorFlujo(int.Parse(vIdFlujo));
        //gvOrders.DataSource = vListaInsp;
        //gvOrders.DataBind();
      }
    }

    protected void GridViewgvInspecciones_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      string vFilaFlujo = string.Empty;
      int vIdFlujo = 0;

      var gvInspecciones = (GridView)sender;
      vFilaFlujo = gvInspecciones.SelectedRow.Cells[0].Text;


      int vIdInspeccion = int.Parse(vFilaFlujo);
      AccesoDatos vAccesoDatos = new AccesoDatos();
      vIdFlujo = vAccesoDatos.FTraeIdFlujoInspeccion(vIdInspeccion);

      Session["NumFlujo"] = vIdFlujo;

      Response.Redirect("~/Presentacion/Inspeccion.aspx?nroInsp=" + vIdInspeccion.ToString());

      //string vRutaUrl = string.Empty;
      //vRutaUrl = "Inspeccion.aspx?nroInsp=" + vIdInspeccion.ToString();
      //ScriptManager.RegisterStartupScript(this, this.GetType(), "Open", "window.open('" + vRutaUrl + "');", true);

    }

    protected int PBusquedaInspecciones()
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
        vNroFlujo = TextBoxNroFlujo.Text.ToUpper();

      if (TextBoxPlaca.Text != string.Empty)
        vPlaca = TextBoxPlaca.Text;

      if (null == TextBoxFechaIni_CalendarExtender.SelectedDate)
      {
        vFechaIni = new DateTime(vFechaIni.Year, vFechaIni.Month, 1, 0, 0, 0);
        vFechaIni = vFechaIni.AddMonths(-1);
        TextBoxFechaIni_CalendarExtender.SelectedDate = vFechaIni;
      }
      else
      {
        vFechaIni = DateTime.ParseExact(TextBoxFechaIni.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
      }

      if (null == TextBoxFechaFin_CalendarExtender.SelectedDate)
      {
        vFechaFin = vFechaIni.AddDays(60);
        TextBoxFechaFin_CalendarExtender.SelectedDate = vFechaFin;
      }
      else
      {
        vFechaFin = DateTime.ParseExact(TextBoxFechaFin.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
      }

      vFechaFin = vFechaFin.AddDays(1);
      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vLst = from i in db.Inspeccion
                   join u in db.Usuario on i.idUsuario equals u.idUsuario
                   join f in db.Flujo on i.idFlujo equals f.idFlujo
                   where (vNroFlujo == null || f.flujoOnBase == vNroFlujo)
                   && (vPlaca == null || f.placaVehiculo == vPlaca)
                   && (i.fechaCreacion >= vFechaIni && i.fechaCreacion <= vFechaFin)
                   orderby f.flujoOnBase
                   select new
                   {
                     f.idFlujo,
                     f.flujoOnBase,
                     f.nombreAsegurado,
                     f.numeroPoliza,
                     f.placaVehiculo,
                     f.docIdAsegurado
                   };

        GridViewMaster.DataSource = vLst.Distinct().OrderBy(vlst => vlst.flujoOnBase).ToList();
        GridViewMaster.DataBind();
      }

      return vResul;

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
        TextBoxPlaca.Text = vFlujoICRL.placaVehiculo;
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

    protected void ButtonTraeFlujoOnBase_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      //int vResultado = 0;
      //var vAccesoDatos = new AccesoDatos();
      //var vFlujoICRL = new FlujoICRL();
      //TextBoxPlaca.Text = string.Empty;

      //vResultado = vAccesoDatos.FValidaExisteFlujoOnBase(TextBoxNroFlujo.Text);
      //if (1 == vResultado)
      //{
      //    vFlujoICRL = vAccesoDatos.FTraeDatosFlujoOnBase(TextBoxNroFlujo.Text);
      //    TextBoxPlaca.Text = vFlujoICRL.placaVehiculo;
      //    int vRespuesta = vAccesoDatos.FValidaExisteFlujoICRL(TextBoxNroFlujo.Text);
      //    if (0 == vRespuesta)
      //    {
      //        int vGrabacion = vAccesoDatos.FGrabaFlujoICRL(vFlujoICRL);
      //        if (0 == vGrabacion)
      //        {
      //            Label4.Text = "Error de Grabacion Flujo ICRL";
      //        }
      //    }
      //    else
      //    {
      //        vFlujoICRL.idFlujo = vRespuesta;
      //        int vActualizacion = vAccesoDatos.FActualizaFlujoICRL(vFlujoICRL);
      //        if (0 == vActualizacion)
      //        {
      //            Label4.Text = "Error de actualizacion Flujo ICRL";
      //        }
      //    }

      //}
      //else
      //{
      //    TextBoxPlaca.Text = "Flujo No encontrado";
      //}
    }

    protected void ImgButtonExportExcel_Click(object sender, ImageClickEventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesoDatos = new AccesoDatos();
      LBCDesaEntities db = new LBCDesaEntities();

      Warning[] warnings;
      string[] streamIds;
      string mimeType = string.Empty;
      string encoding = string.Empty;
      string extension = "xls";
      string fileName = "RepInspeccionxFlujo";

      string vNroFlujo = null;
      string vPlaca = null;
      DateTime vFechaIni = DateTime.Now;
      DateTime vFechaFin = DateTime.Now;

      if (TextBoxNroFlujo.Text != string.Empty)
        vNroFlujo = TextBoxNroFlujo.Text.ToUpper();

      if (TextBoxPlaca.Text != string.Empty)
        vPlaca = TextBoxPlaca.Text;

      vFechaIni = DateTime.ParseExact(TextBoxFechaIni.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
      vFechaFin = DateTime.ParseExact(TextBoxFechaFin.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
      vFechaFin = vFechaFin.AddDays(1);

      var vListaInsp = from i in db.Inspeccion
                       join f in db.Flujo on i.idFlujo equals f.idFlujo
                       where (vNroFlujo == null || f.flujoOnBase == vNroFlujo)
                       && (vPlaca == null || f.placaVehiculo == vPlaca)
                       && (i.fechaCreacion >= vFechaIni && i.fechaCreacion <= vFechaFin)
                       orderby i.idInspeccion
                       select new
                       {
                         f.flujoOnBase,
                         f.nombreAsegurado,
                         f.docIdAsegurado,
                         f.telefonocelAsegurado,
                         f.numeroPoliza,
                         f.placaVehiculo,
                         f.marcaVehiculo,
                         f.modeloVehiculo,
                         f.colorVehiculo,
                         f.anioVehiculo,
                         i.idInspeccion,
                         i.causaSiniestro,
                         i.descripcionSiniestro,
                         i.fechaCreacion,
                         i.idInspector,
                         i.nombreContacto,
                         i.sucursalAtencion
                       };

      ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
      ReportDataSource datasource = new ReportDataSource("DataSet1", vListaInsp);

      ReportViewer1.LocalReport.DataSources.Clear();
      ReportViewer1.LocalReport.DataSources.Add(datasource);
      byte[] VArrayBytes = ReportViewer1.LocalReport.Render("Excel", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

      //enviar el array de bytes a cliente
      Response.Buffer = true;
      Response.Clear();
      Response.ContentType = mimeType;
      Response.AddHeader("content-disposition", "attachment; filename=" + fileName + "." + extension);
      Response.BinaryWrite(VArrayBytes); // create the file
      Response.Flush(); // send it to the client to download

    }

    protected void GridViewMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
      if (!VerificarPagina(true)) return;
      //se realiza una búsqueda por prioridad
      //primero por flujo, después por placa y finalmente por fecha
      //validar los campos de busqueda
      int vResul = 1;
      DateTime vFechaIni = DateTime.Now;
      DateTime vFechaFin = DateTime.Now;

      string vNroFlujo = null;
      string vPlaca = null;

      if (TextBoxNroFlujo.Text != string.Empty)
        vNroFlujo = TextBoxNroFlujo.Text.ToUpper();

      if (TextBoxPlaca.Text != string.Empty)
        vPlaca = TextBoxPlaca.Text;

      if (null == TextBoxFechaIni_CalendarExtender.SelectedDate)
      {
        vFechaIni = new DateTime(vFechaIni.Year, vFechaIni.Month, 1, 0, 0, 0);
        vFechaIni = vFechaIni.AddMonths(-1);
        TextBoxFechaIni_CalendarExtender.SelectedDate = vFechaIni;
      }
      else
      {
        vFechaIni = DateTime.ParseExact(TextBoxFechaIni.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
      }

      if (null == TextBoxFechaFin_CalendarExtender.SelectedDate)
      {
        vFechaFin = vFechaIni.AddDays(60);
        TextBoxFechaFin_CalendarExtender.SelectedDate = vFechaFin;
      }
      else
      {
        vFechaFin = DateTime.ParseExact(TextBoxFechaFin.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
      }

      vFechaFin = vFechaFin.AddDays(1);
      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vLst = from i in db.Inspeccion
                   join u in db.Usuario on i.idUsuario equals u.idUsuario
                   join f in db.Flujo on i.idFlujo equals f.idFlujo
                   where (vNroFlujo == null || f.flujoOnBase == vNroFlujo)
                   && (vPlaca == null || f.placaVehiculo == vPlaca)
                   && (i.fechaCreacion >= vFechaIni && i.fechaCreacion <= vFechaFin)
                   orderby f.flujoOnBase
                   select new
                   {
                     f.idFlujo,
                     f.flujoOnBase,
                     f.nombreAsegurado,
                     f.numeroPoliza,
                     f.placaVehiculo,
                     f.docIdAsegurado
                   };

        GridViewMaster.DataSource = vLst.Distinct().OrderBy(vlst => vlst.flujoOnBase).ToList();
        GridViewMaster.PageIndex = e.NewPageIndex;
        GridViewMaster.DataBind();
      }
    }

    protected void ButtonPopupSiNoSI_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      gRespuestaSiNo = 1;
      Session["PopupModalSiNo"] = 0;
      this.ModalPopupSiNo.Hide();
      PCreaInspeccion();
    }

    protected void ButtonPopupSiNoNO_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      gRespuestaSiNo = 0;
      Session["PopupModalSiNo"] = 0;
      this.ModalPopupSiNo.Hide();
    }

  }

}