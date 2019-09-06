using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LbcOnBaseWS;
using ICRL.ModeloDB;
using ICRL.BD;
using Microsoft.Reporting.WebForms;

namespace ICRL.Presentacion
{
  public partial class Inspeccion : System.Web.UI.Page
  {
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
      try
      {
        int vIdInspeccion = 0;
        string vlNumFlujo = string.Empty;
        if (Request.QueryString["nroInsp"] != null)
        {
          vIdInspeccion = int.Parse(Request.QueryString["nroInsp"]);
        }


        if (Session["NumFlujo"] != null)
        {
          vlNumFlujo = Session["NumFlujo"].ToString();
          TextBoxIdFlujo.Text = Session["NumFlujo"].ToString();
        }

        if (!IsPostBack)
        {
          FlTraeNomenTipoTallerInsp();
          FlTraeNomenTipoTallerRCVeh();
          FlTraeNomenTipoTallerDPPadre();
          FlTraeNomenCompraDet();
          FlTraeNomenChaperioDet();
          FlTraeNomenRepPreviaDet();
          FlTraeItemsNomenclador();
          FlTraeItemsNomencladorRP();
          FlTraeNomenCompraRP();
          FlTraeNomenChaperioRP();
          FlTraeNomenRepPreviaRP();
          FlTraeItemsNomencladorCajaPTDP();
          FlTraeItemsNomencladorCombustiblePTDP();
          FlTraeItemsNomencladorCajaPTRO();
          FlTraeItemsNomencladorCombustiblePTRO();
          //FlTraeItemsNomencladorRCV01();
          FlTraeItemsMarcaRCV01();
          FlTraeItemsColorRCV01();
          FlTraeNomenChaperio();
          FlTraeNomenRepPrevia();
          FlTraeNomenCompraDetRCV01();
          FlTraeNomenItemRCV01();
          FlTraeDatosInspeccion(vIdInspeccion, vlNumFlujo);

          //ValidaDaniosPropios(vIdInspeccion);
          //ValidaRCObjetos(vIdInspeccion);
          //ValidaRCPersonas(vIdInspeccion);
          //ValidaRCVehicular01(vIdInspeccion);
          //ValidaRoboParcial(vIdInspeccion);
          //ValidaPerdidaTotalDP(vIdInspeccion);
          //ValidaPerdidaTotalRO(vIdInspeccion);
          int vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
          ValidaDaniosPropiosFlujo(vIdFlujo);
          ValidaRCObjetosFlujo(vIdFlujo);
          ValidaRCPersonasFlujo(vIdFlujo);
          ValidaRCVehicular01Flujo(vIdFlujo);
          ValidaRoboParcialFlujo(vIdFlujo);
          ValidaPerdidaTotalDPFlujo(vIdFlujo);
          ValidaPerdidaTotalROFlujo(vIdFlujo);
          // Se obtiene el tipo de Inspeccion que se recibio como argumento y se coloca como pestaña activa
          AccesoDatos vAccesoDatos = new AccesoDatos();
          ICRL.ModeloDB.Inspeccion vFilaInspeccion = new ICRL.ModeloDB.Inspeccion();
          vFilaInspeccion = vAccesoDatos.FTraeDatosBasicosInspeccion(vIdInspeccion);
          TabContainerCoberturas.ActiveTabIndex = (vFilaInspeccion.tipoCobertura) - 1;

          if ((int)AccesoDatos.TipoInspeccion.RoboParcial == vFilaInspeccion.tipoCobertura)
          {
            DropDownListTipoTallerInsp.Enabled = true;
            PanelDatosTaller.Visible = true;
            if (1 == vFilaInspeccion.estado)
            {
              ButtonFinRoboP.Enabled = true;
            }
            else
            {
              ButtonFinRoboP.Enabled = false;
            }
          }

          if ((int)AccesoDatos.TipoInspeccion.PerdidaTotalDaniosPropios == vFilaInspeccion.tipoCobertura)
          {
            if (1 == vFilaInspeccion.estado)
            {
              ButtonFinPTDaniosP.Enabled = true;
            }
            else
            {
              ButtonFinPTDaniosP.Enabled = false;
            }
          }

          if ((int)AccesoDatos.TipoInspeccion.PerdidaTotalRobo == vFilaInspeccion.tipoCobertura)
          {
            if (1 == vFilaInspeccion.estado)
            {
              ButtonFinPTRobo.Enabled = true;
            }
            else
            {
              ButtonFinPTRobo.Enabled = false;
            }
          }
        }

        if (Session["PopupDPHabilitado"] != null)
        {
          int vPopup = -1;
          vPopup = int.Parse(Session["PopupDPHabilitado"].ToString());
          if (1 == vPopup)
            this.ModalPopupDaniosPropios.Show();
          else
            this.ModalPopupDaniosPropios.Hide();
        }

        if (Session["PopupRCObjHabilitado"] != null)
        {
          int vPopup = -1;
          vPopup = int.Parse(Session["PopupRCObjHabilitado"].ToString());
          if (1 == vPopup)
            this.ModalPopupRCObjetos.Show();
          else
            this.ModalPopupRCObjetos.Hide();
        }

        if (Session["PopupRCPerHabilitado"] != null)
        {
          int vPopup = -1;
          vPopup = int.Parse(Session["PopupRCPerHabilitado"].ToString());
          if (1 == vPopup)
            this.ModalPopupRCPersonas.Show();
          else
            this.ModalPopupRCPersonas.Hide();
        }

        if (Session["PopupHabilitado"] != null)
        {
          int vPopup = -1;
          vPopup = int.Parse(Session["PopupHabilitado"].ToString());
          if (1 == vPopup)
            this.ModalPopupRCV01.Show();
          else
            this.ModalPopupRCV01.Hide();
        }

        //ValidaDaniosPropios(vIdInspeccion);
        //ValidaRCObjetos(vIdInspeccion);
        //ValidaRCPersonas(vIdInspeccion);
        //ValidaRoboParcial(vIdInspeccion);
        //ValidaPerdidaTotalDP(vIdInspeccion);
      }
      catch (Exception ex)
      {
        if (Session["MsjEstado"] != null)
        {
          Session["MsjEstado"] = string.Empty;
        }
        Session["MsjEstado"] = ex.Message;
      }
    }

    #region ValidaCoberturasFlujo



    private void ValidaRCObjetosFlujo(int pIdFlujo)
    {
      AccesoDatos vAccesodatos = new AccesoDatos();

      bool vSeleccionado = false;
      int vResul = 0;
      int vIdInspeccion = 0;

      vIdInspeccion = vAccesodatos.FFlujoTieneRCObjetos(pIdFlujo);

      if (vIdInspeccion > 0)
      {
        vSeleccionado = vAccesodatos.FInspeccionTieneObjCRL(vIdInspeccion);
        if (vSeleccionado)
        {
          TabPanelRCObjetos.Enabled = true;
          TabPanelRCObjetos.Visible = true;
          CheckBoxRCObjetos.Checked = vSeleccionado;
          vResul = FlTraeDatosRCObjetos(vIdInspeccion);
          //TabContainerCoberturas.ActiveTabIndex = 0;
        }
      }
    }

    private void ValidaRCPersonasFlujo(int pIdFlujo)
    {
      AccesoDatos vAccesodatos = new AccesoDatos();

      bool vSeleccionado = false;
      int vResul = 0;
      int vIdInspeccion = 0;

      vIdInspeccion = vAccesodatos.FFlujoTieneRCPersonas(pIdFlujo);

      if (vIdInspeccion > 0)
      {
        vSeleccionado = vAccesodatos.FInspeccionTienePerCRL(vIdInspeccion);
        if (vSeleccionado)
        {
          TabPanelRCPersonas.Enabled = true;
          TabPanelRCPersonas.Visible = true;
          CheckBoxRCPersonas.Checked = vSeleccionado;
          vResul = FlTraeDatosRCPersonas(vIdInspeccion);
          //TabContainerCoberturas.ActiveTabIndex = 0;
        }
      }
    }



    #endregion

    #region ValidaInspecciones - Obsoleto

    private void ValidaDaniosPropios(int pIdInspeccion)
    {
      AccesoDatos vAccesodatos = new AccesoDatos();

      bool vSeleccionado = false;
      int vResul = 0;
      vSeleccionado = vAccesodatos.FInspeccionTieneDPICRL(pIdInspeccion);
      if (vSeleccionado)
      {
        TabPanelDaniosPropios.Enabled = true;
        TabPanelDaniosPropios.Visible = true;
        CheckBoxDaniosPropios.Checked = vSeleccionado;
        vResul = FlTraeDatosDaniosPropios(pIdInspeccion);
      }
    }

    private void ValidaRCObjetos(int pIdInspeccion)
    {
      AccesoDatos vAccesodatos = new AccesoDatos();

      bool vSeleccionado = false;
      int vResul = 0;
      vSeleccionado = vAccesodatos.FInspeccionTieneObjCRL(pIdInspeccion);
      if (vSeleccionado)
      {
        TabPanelRCObjetos.Enabled = true;
        TabPanelRCObjetos.Visible = true;
        CheckBoxRCObjetos.Checked = vSeleccionado;
        vResul = FlTraeDatosRCObjetos(pIdInspeccion);
      }
    }

    private void ValidaRCPersonas(int pIdInspeccion)
    {
      AccesoDatos vAccesodatos = new AccesoDatos();

      bool vSeleccionado = false;
      int vResul = 0;
      vSeleccionado = vAccesodatos.FInspeccionTienePerCRL(pIdInspeccion);
      if (vSeleccionado)
      {
        TabPanelRCPersonas.Enabled = true;
        TabPanelRCPersonas.Visible = true;
        CheckBoxRCPersonas.Checked = vSeleccionado;
        vResul = FlTraeDatosRCPersonas(pIdInspeccion);
      }
    }

    private void ValidaRoboParcial(int pIdInspeccion)
    {
      AccesoDatos vAccesodatos = new AccesoDatos();

      bool vSeleccionado = false;
      int vResul = 0;
      vSeleccionado = vAccesodatos.FInspeccionTieneRPICRL(pIdInspeccion);

      if (vSeleccionado)
      {
        TabPanelRoboParcial.Enabled = true;
        TabPanelRoboParcial.Visible = true;
        CheckBoxRoboParcial.Checked = vSeleccionado;
        vResul = FlTraeDatosRoboParcial(pIdInspeccion);
      }
    }

    private void ValidaPerdidaTotalDP(int pIdInspeccion)
    {
      AccesoDatos vAccesodatos = new AccesoDatos();

      bool vSeleccionado = false;
      int vResul = 0;
      vSeleccionado = vAccesodatos.FInspeccionTienePTDPICRL(pIdInspeccion);
      if (vSeleccionado)
      {
        TabPanelPerdidaTotalDaniosPropios.Enabled = true;
        TabPanelPerdidaTotalDaniosPropios.Visible = true;
        CheckBoxPerdidaTotDanios.Checked = vSeleccionado;
        vResul = FlTraeDatosPerdidaTotalDP(pIdInspeccion);
      }
    }

    private void ValidaPerdidaTotalRO(int pIdInspeccion)
    {
      AccesoDatos vAccesodatos = new AccesoDatos();

      bool vSeleccionado = false;
      int vResul = 0;
      vSeleccionado = vAccesodatos.FInspeccionTienePTROICRL(pIdInspeccion);
      if (vSeleccionado)
      {
        TabPanelPerdidaTotalRobo.Enabled = true;
        TabPanelPerdidaTotalRobo.Visible = true;
        CheckBoxPerdidaTotRobo.Checked = vSeleccionado;
        vResul = FlTraeDatosPerdidaTotalRO(pIdInspeccion);
      }
    }

    private void ValidaRCVehicular01(int pIdInspeccion)
    {
      AccesoDatos vAccesodatos = new AccesoDatos();

      bool vSeleccionado = false;
      int vResul = 0;
      vSeleccionado = vAccesodatos.FInspeccionTieneRCVehicularICRL(pIdInspeccion);

      if (vSeleccionado)
      {
        TabPanelRCV01.Enabled = true;
        TabPanelRCV01.Visible = true;
        CheckBoxRCVehicular01.Checked = vSeleccionado;
        vResul = FlTraeDatosRCV01(pIdInspeccion);
        PBloqueoRCVehicular01(true);
      }
    }

    #endregion

    #region CreaCoberturasFlujo

    private int CreaInspRCObjetosFlujo(int pIdFlujo)
    {
      AccesoDatos vAccesoDatos = new AccesoDatos();
      InspeccionICRL vInspeccionICRL = new InspeccionICRL();
      string vCodUsuario = Session["IdUsr"].ToString();
      int vResultado = 0;
      int vIdInspeccion = 0;

      int vIdUsuario = vAccesoDatos.FValidaExisteUsuarioICRL(vCodUsuario);
      vIdInspeccion = vAccesoDatos.FFlujoTieneRCObjetos(pIdFlujo);
      if (0 == vIdInspeccion)
      {
        vInspeccionICRL.idFlujo = pIdFlujo;
        vInspeccionICRL.correlativo = vAccesoDatos.fObtieneContadorInspeccionFlujo(pIdFlujo);
        vInspeccionICRL.idUsuario = vIdUsuario;
        vInspeccionICRL.sucursalAtencion = string.Empty;
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
        vInspeccionICRL.tipoInspeccion = (int)ICRL.BD.AccesoDatos.TipoInspeccion.RCObjetos;
        int vRespuesta = vAccesoDatos.FGrabaInspeccionICRL(vInspeccionICRL);

        if (0 == vRespuesta)
        {
          LabelMensaje.Text = "Error al crear la Inspeccion RC Objetos";
        }
        else
        {
          TextBoxNroInspeccion.Text = vRespuesta.ToString();
          vResultado = vRespuesta;
        }
      }
      return vResultado;
    }

    #endregion


    private void FlTraeDatosInspeccion(int pIdInspeccion, string pNumFlujo)
    {
      if (string.Empty == pNumFlujo)
      {
        using (LBCDesaEntities db = new LBCDesaEntities())
        {
          var vLst = from i in db.Inspeccion
                     join u in db.Usuario on i.idUsuario equals u.idUsuario
                     where i.idInspeccion == pIdInspeccion
                     select new
                     {
                       i.idInspeccion,
                       i.causaSiniestro,
                       i.descripcionSiniestro,
                       i.zona,
                       i.observacionesSiniestro,
                       i.sucursalAtencion,
                       i.fecha_siniestro,
                       i.nombreContacto,
                       i.telefonoContacto,
                       i.correlativo,
                       u.nombreVisible,
                       u.correoElectronico
                     };
          var vFilaTabla = vLst.FirstOrDefault();

          if (null != vFilaTabla)
          {
            TextBoxNroInspeccion.Text = vFilaTabla.idInspeccion.ToString();
            TextBoxCorrelativo.Text = vFilaTabla.correlativo.ToString();
            TextBoxSucAtencion.Text = vFilaTabla.sucursalAtencion;
            TextBoxCausaSiniestro.Text = vFilaTabla.causaSiniestro;
            TextBoxDescripSiniestro.Text = vFilaTabla.descripcionSiniestro;
            TextBoxObservacionesInspec.Text = vFilaTabla.observacionesSiniestro;
            TextBoxNombreInspector.Text = vFilaTabla.nombreVisible;
            TextBoxCorreoInspector.Text = vFilaTabla.correoElectronico;
          }

        }
      }
      else
      {
        using (LBCDesaEntities db = new LBCDesaEntities())
        {
          var vLst = from i in db.Inspeccion
                     join u in db.Usuario on i.idUsuario equals u.idUsuario
                     join f in db.Flujo on i.idFlujo equals f.idFlujo
                     where i.idInspeccion == pIdInspeccion
                     select new
                     {
                       f.flujoOnBase,
                       f.nombreAsegurado,
                       f.telefonocelAsegurado,
                       f.numeroReclamo,
                       f.causaSiniestro,
                       f.placaVehiculo,
                       f.chasisVehiculo,
                       f.colorVehiculo,
                       f.modeloVehiculo,
                       f.marcaVehiculo,
                       f.anioVehiculo,
                       f.valorAsegurado,
                       i.idInspeccion,
                       f.descripcionSiniestro,
                       f.direccionInspeccion,
                       f.agenciaAtencion,
                       i.zona,
                       i.observacionesSiniestro,
                       i.sucursalAtencion,
                       i.fecha_siniestro,
                       i.nombreContacto,
                       i.telefonoContacto,
                       i.correosDeEnvio,
                       i.correlativo,
                       i.tipoTaller,
                       u.nombreVisible,
                       u.correoElectronico,
                     };
          var vFilaTabla = vLst.FirstOrDefault();

          if (null != vFilaTabla)
          {
            TextBoxNroFlujo.Text = vFilaTabla.flujoOnBase;
            TextBoxNroInspeccion.Text = vFilaTabla.idInspeccion.ToString();
            TextBoxCorrelativo.Text = vFilaTabla.correlativo.ToString();
            TextBoxNroReclamo.Text = vFilaTabla.numeroReclamo.ToString();
            TextBoxSucAtencion.Text = vFilaTabla.agenciaAtencion.Trim();
            TextBoxDirecInspeccion.Text = vFilaTabla.direccionInspeccion.Trim();
            //TextBoxZona.Text = vFilaTabla.zona.Trim();
            TextBoxCausaSiniestro.Text = vFilaTabla.causaSiniestro.Trim();
            TextBoxDescripSiniestro.Text = vFilaTabla.descripcionSiniestro.Trim();
            TextBoxObservacionesInspec.Text = vFilaTabla.observacionesSiniestro.Trim();
            TextBoxNombreAsegurado.Text = vFilaTabla.nombreAsegurado;
            TextBoxTelefonoAsegurado.Text = vFilaTabla.telefonocelAsegurado;
            TextBoxNombreInspector.Text = vFilaTabla.nombreVisible;
            TextBoxCorreoInspector.Text = vFilaTabla.correoElectronico;
            TextBoxNombreContacto.Text = vFilaTabla.nombreContacto.Trim();
            TextBoxTelefContacto.Text = vFilaTabla.telefonoContacto.Trim();
            TextBoxEmailsEnvio.Text = vFilaTabla.correosDeEnvio.Trim();
            TextBoxMarca.Text = vFilaTabla.marcaVehiculo;
            TextBoxModelo.Text = vFilaTabla.modeloVehiculo;
            TextBoxPlaca.Text = vFilaTabla.placaVehiculo;
            TextBoxColor.Text = vFilaTabla.colorVehiculo;
            TextBoxNroChasis.Text = vFilaTabla.chasisVehiculo;
            TextBoxAnio.Text = vFilaTabla.anioVehiculo.ToString();
            TextBoxValorAsegurado.Text = vFilaTabla.valorAsegurado.ToString();

            string vTempoCadena = string.Empty;
            vTempoCadena = vFilaTabla.tipoTaller.Trim();
            DropDownListTipoTallerInsp.ClearSelection();
            DropDownListTipoTallerInsp.Items.FindByText(vTempoCadena).Selected = true;
          }

        }
      }
    }

    private int FGrabaCambiosInspeccion()
    {
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionICRL vInspeccionICRL = new InspeccionICRL();
      string vCadenaAux = string.Empty;
      int vResultado = 0;

      vInspeccionICRL.idInspeccion = int.Parse(TextBoxNroInspeccion.Text);
      vCadenaAux = string.Empty;
      vCadenaAux = TextBoxSucAtencion.Text;
      if(vCadenaAux.Length > 25)
      {
        vInspeccionICRL.sucursalAtencion = vCadenaAux.Substring(0,25);
      }
      else
      {
        vInspeccionICRL.sucursalAtencion = vCadenaAux;
      }

      vCadenaAux = string.Empty;
      vCadenaAux = TextBoxDirecInspeccion.Text;
      if(vCadenaAux.Length > 40)
      {
        vInspeccionICRL.direccion = vCadenaAux.Substring(0,40);
      }
      else
      {
        vInspeccionICRL.direccion = vCadenaAux;
      }
      
      //vInspeccionICRL.zona = TextBoxZona.Text;
      vInspeccionICRL.zona = string.Empty;
      vInspeccionICRL.causaSiniestro = TextBoxCausaSiniestro.Text;
      vInspeccionICRL.descripcionSiniestro = TextBoxDescripSiniestro.Text;

      vCadenaAux = string.Empty;
      vCadenaAux = TextBoxObservacionesInspec.Text;
      if(vCadenaAux.Length > 100)
      {
        vInspeccionICRL.observacionesInspec = vCadenaAux.Substring(0,100);
      }
      else
      {
        vInspeccionICRL.observacionesInspec = vCadenaAux;
      }
      
      vInspeccionICRL.idInspector = TextBoxNombreInspector.Text;
      vInspeccionICRL.nombreContacto = TextBoxNombreContacto.Text;
      vInspeccionICRL.telefonoContacto = TextBoxTelefContacto.Text;
      vInspeccionICRL.correosDeEnvio = TextBoxEmailsEnvio.Text;
      vInspeccionICRL.recomendacionPerdidaTotal = false;
      vInspeccionICRL.estado = 1;
      vInspeccionICRL.tipoTaller = DropDownListTipoTallerInsp.SelectedItem.Text;

      vResultado = vAccesodatos.FActualizaInspeccionICRL(vInspeccionICRL);
      if (0 == vResultado)
      {
        LabelMensaje.Text = "Error al grabar los datos de la Inspección";
      }
      else
      {
        LabelMensaje.Text = string.Empty;
      }

      return vResultado;
    }

    private int FlTraeDatosDaniosPropios(int pSecuencial)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vLst = from idp in db.InspDaniosPropios
                   join n in db.Nomenclador on idp.idItem equals n.codigo
                   where (idp.secuencial == pSecuencial)
                      && (n.categoriaNomenclador == "Item")
                   select new
                   {
                     idp.idItem,
                     n.descripcion,
                     idp.compra,
                     idp.instalacion,
                     idp.pintura,
                     idp.mecanico,
                     idp.chaperio,
                     idp.reparacionPrevia,
                     idp.observaciones,
                     idp.nro_item
                   };

        GridViewDaniosPropios.DataSource = vLst.ToList();
        GridViewDaniosPropios.DataBind();

      }

      return vResultado;
    }

    private int FlTraeDatosDaniosPropiosPadre(int pIdInspeccion)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vLst = from i in db.Inspeccion
                   join idpp in db.InspDaniosPropiosPadre on i.idInspeccion equals idpp.idInspeccion
                   where (i.idInspeccion == pIdInspeccion)
                   select new
                   {
                     idpp.secuencial,
                     idpp.tipoTaller,
                     idpp.cambioAPerdidaTotal,
                     idpp.estado
                   };

        GridViewDaniosPropiosPadre.DataSource = vLst.ToList();
        GridViewDaniosPropiosPadre.DataBind();

      }

      return vResultado;
    }

    private int FlTraeItemsNomenclador()
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vLst = from n in db.Nomenclador
                   where n.categoriaNomenclador == "Item"
                   orderby n.descripcion
                   select new
                   {
                     n.codigo,
                     n.descripcion,
                   };

        DropDownListItem.DataValueField = "codigo";
        DropDownListItem.DataTextField = "descripcion";
        DropDownListItem.DataSource = vLst.ToList();
        DropDownListItem.DataBind();

      }

      return vResultado;
    }

    public string MyNewRowDetDP(object pIdSecuencial)
    {
      string vTempo = string.Empty;
      vTempo = String.Format(@"</td></tr><tr id ='trdp{0}' class='collapsed-row'><td></td><td colspan='100' style='padding:0px; margin:0px;'>", pIdSecuencial);
      return vTempo;
    }

    #region 2do Revisar
    //private int flTraeDatosFlujo(string pNumFlujo)
    //{
    //    int vResultado = 0;

    //    /*** CONECTAR A WS ***/
    //    var servicioOnBase = new OnBaseWS();
    //    /*** ESTABLECER LA APLICACIÓN ORIGEN POR DEFECTO PARA GESPRO ***/
    //    var origen = SistemaOrigen.ICRL;
    //    /*** INSTANCIAR EL RESULTADO COMO ResultadoEntity ***/
    //    ResultadoEntity resultado = new ResultadoEntity();
    //    /*** LLAMAR A LA FUNCIÓN DEL WS ***/
    //    resultado = servicioOnBase.ObtenerInformacionSolicitudOnBase(pNumFlujo, origen);

    //    /*** SI EL RESULTADO ES CORRECTO, SE EXTRAE LA INFORMACIÓN DEL FLUJO ***/
    //    if (resultado.EsValido)
    //    {
    //        string txtRespuesta = String.Empty;
    //        // OBTENER EL OBJETO CON LA INFORMACIÓN QUE SE NECESITA
    //        var documento = (DocumentoOnBaseEntity)resultado.DatosAdicionales;

    //        // DATOS PRINCIPALES DEL DOCUMENTO (FLUJO)
    //        txtRespuesta += "--- INFORMACIÓN DEL DOCUMENTO ---" + Environment.NewLine;
    //        txtRespuesta += "Solicitud: " + documento.nroSolicitud + Environment.NewLine;
    //        txtRespuesta += "Nombre Documento: " + documento.nombreDocumento + Environment.NewLine;
    //        txtRespuesta += "Tipo Documento: " + documento.tipoDocumento + Environment.NewLine;
    //        txtRespuesta += "Usuario Apertura: " + documento.usuarioApertura + Environment.NewLine;
    //        txtRespuesta += "Usuario Nombre: " + documento.usuarioNombreApertura + Environment.NewLine;
    //        txtRespuesta += "--- DATOS DEL DOCUMENTO (KEYWORDS) ---" + Environment.NewLine;

    //        // OBTENER LA LISTA DE LOS DATOS (KEYWORDS) DEL DOCUMENTO
    //        var listaKeywords = (KeywordOnBaseEntity[])documento.keywords;
    //        foreach (var keyword in listaKeywords)
    //        {
    //            txtRespuesta += keyword.nombre + ": " + keyword.valor + Environment.NewLine;
    //        }

    //        // MOSTRAR DATOS EN LA APLICACIÓN DE PRUEBA

    //        vResultado = 1;
    //    }
    //    else
    //    {
    //        TextBoxWebService.Text = resultado.Mensaje;
    //    }

    //    return (vResultado);
    //}
    #endregion

    #region CheckBox Coberturas

    protected void CheckBoxDaniosPropios_CheckedChanged(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      if (CheckBoxDaniosPropios.Checked)
      {
        TabPanelDaniosPropios.Enabled = true;
        TabPanelDaniosPropios.Visible = true;
        int vResul = FGrabaCambiosInspeccion();
        vResul = FlTraeDatosDaniosPropios(int.Parse(TextBoxNroInspeccion.Text));
      }
      else
        TabPanelDaniosPropios.Enabled = false;
    }

    protected void CheckBoxRCObjetos_CheckedChanged(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      if (CheckBoxRCObjetos.Checked)
      {
        AccesoDatos vAccesoDatos = new AccesoDatos();
        int vSeGraboInspeccionRCObj = 0;
        int vIdInspeccion = vAccesoDatos.FFlujoTieneRCObjetos(int.Parse(TextBoxIdFlujo.Text));
        //Si no existe la inspeccion de RC Objetos se crea la inspeccion correspondiente
        if (0 == vIdInspeccion)
        {
          vSeGraboInspeccionRCObj = CreaInspRCObjetosFlujo(int.Parse(TextBoxIdFlujo.Text));

          if (vSeGraboInspeccionRCObj > 0)
          {
            TabPanelRCObjetos.Enabled = true;
            TabPanelRCObjetos.Visible = true;
            int vResul = FGrabaCambiosInspeccion();
            vResul = FlTraeDatosRCObjetos(int.Parse(TextBoxNroInspeccion.Text));
          }
        }
        else
        {
          TabPanelRCObjetos.Enabled = true;
          TabPanelRCObjetos.Visible = true;
          int vResul = FGrabaCambiosInspeccion();
          vResul = FlTraeDatosRCObjetos(int.Parse(TextBoxNroInspeccion.Text));
        }
      }
      else
        TabPanelRCObjetos.Enabled = false;
    }

    protected void CheckBoxRCPersonas_CheckedChanged(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      if (CheckBoxRCPersonas.Checked)
      {
        AccesoDatos vAccesoDatos = new AccesoDatos();
        int vSeGraboInspeccionRCPer = 0;
        int vIdInspeccion = vAccesoDatos.FFlujoTieneRCPersonas(int.Parse(TextBoxIdFlujo.Text));
        //Si no existe la inspeccion de RC Personas se crea la inspeccion correspondiente
        if (0 == vIdInspeccion)
        {
          vSeGraboInspeccionRCPer = CreaInspRCPersonasFlujo(int.Parse(TextBoxIdFlujo.Text));

          if (vSeGraboInspeccionRCPer > 0)
          {
            TabPanelRCPersonas.Enabled = true;
            TabPanelRCPersonas.Visible = true;
            int vResul = FGrabaCambiosInspeccion();
            vResul = FlTraeDatosRCPersonas(int.Parse(TextBoxNroInspeccion.Text));
          }
        }
        else
        {
          TabPanelRCPersonas.Enabled = true;
          TabPanelRCPersonas.Visible = true;
          int vResul = FGrabaCambiosInspeccion();
          vResul = FlTraeDatosRCPersonas(int.Parse(TextBoxNroInspeccion.Text));
        }
      }
      else
        TabPanelRCPersonas.Enabled = false;
    }

    protected void CheckBoxRoboParcial_CheckedChanged(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      if (CheckBoxRoboParcial.Checked)
      {
        AccesoDatos vAccesoDatos = new AccesoDatos();
        int vSeGraboInspeccionRoboParcial = 0;
        int vIdInspeccion = vAccesoDatos.FFlujoTieneRoboParcial(int.Parse(TextBoxIdFlujo.Text));
        //Si no existe la inspeccion de Robo Parcial se crea la inspeccion correspondiente
        if (0 == vIdInspeccion)
        {
          vSeGraboInspeccionRoboParcial = CreaInspRoboParcialFlujo(int.Parse(TextBoxIdFlujo.Text));

          if (vSeGraboInspeccionRoboParcial > 0)
          {
            TabPanelRoboParcial.Enabled = true;
            TabPanelRoboParcial.Visible = true;
            int vResul = FGrabaCambiosInspeccion();
            vResul = FlTraeDatosRoboParcial(int.Parse(TextBoxNroInspeccion.Text));
          }
        }
        else
        {
          TabPanelRoboParcial.Enabled = true;
          TabPanelRoboParcial.Visible = true;
          int vResul = FGrabaCambiosInspeccion();
          vResul = FlTraeDatosRoboParcial(int.Parse(TextBoxNroInspeccion.Text));
        }
      }
      else
        TabPanelRoboParcial.Enabled = false;
    }

    protected void CheckBoxPerdidaTotDanios_CheckedChanged(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      if (CheckBoxPerdidaTotDanios.Checked)
      {
        AccesoDatos vAccesoDatos = new AccesoDatos();
        int vSeGraboInspeccionPTDaniosPropios = 0;
        int vIdInspeccion = vAccesoDatos.FFlujoTienePerdidaTotDaniosPropios(int.Parse(TextBoxIdFlujo.Text));
        //Si no existe la inspeccion de PerdidaTotal por Danios Propios se crea la inspeccion correspondiente
        if (0 == vIdInspeccion)
        {
          vSeGraboInspeccionPTDaniosPropios = CreaInspPTDaniosPropiosFlujo(int.Parse(TextBoxIdFlujo.Text));

          if (vSeGraboInspeccionPTDaniosPropios > 0)
          {
            TabPanelPerdidaTotalDaniosPropios.Enabled = true;
            TabPanelPerdidaTotalDaniosPropios.Visible = true;
            int vResul = FGrabaCambiosInspeccion();
            vResul = FlTraeDatosPerdidaTotalDP(int.Parse(TextBoxNroInspeccion.Text));
          }
        }
        else
        {
          TabPanelPerdidaTotalDaniosPropios.Enabled = true;
          TabPanelPerdidaTotalDaniosPropios.Visible = true;
          int vResul = FGrabaCambiosInspeccion();
          vResul = FlTraeDatosPerdidaTotalDP(int.Parse(TextBoxNroInspeccion.Text));
        }
      }
      else
        TabPanelPerdidaTotalDaniosPropios.Enabled = false;
    }

    protected void CheckBoxPerdidaTotRobo_CheckedChanged(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      if (CheckBoxPerdidaTotRobo.Checked)
      {
        AccesoDatos vAccesoDatos = new AccesoDatos();
        int vSeGraboInspeccionPTRobo = 0;
        int vIdInspeccion = vAccesoDatos.FFlujoTienePerdidaTotRobo(int.Parse(TextBoxIdFlujo.Text));
        //Si no existe la inspeccion de PerdidaTotal por Robo se crea la inspeccion correspondiente
        if (0 == vIdInspeccion)
        {
          vSeGraboInspeccionPTRobo = CreaInspPTRoboFlujo(int.Parse(TextBoxIdFlujo.Text));

          if (vSeGraboInspeccionPTRobo > 0)
          {
            TabPanelPerdidaTotalRobo.Enabled = true;
            TabPanelPerdidaTotalRobo.Visible = true;
            int vResul = FGrabaCambiosInspeccion();
            vResul = FlTraeDatosPerdidaTotalRO(int.Parse(TextBoxNroInspeccion.Text));
          }
        }
        else
        {
          TabPanelPerdidaTotalRobo.Enabled = true;
          TabPanelPerdidaTotalRobo.Visible = true;
          int vResul = FGrabaCambiosInspeccion();
          vResul = FlTraeDatosPerdidaTotalRO(int.Parse(TextBoxNroInspeccion.Text));
        }
      }
      else
        TabPanelPerdidaTotalRobo.Enabled = false;
    }

    protected void CheckBoxRCVehicular01_CheckedChanged(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      if (CheckBoxRCVehicular01.Checked)
      {
        AccesoDatos vAccesoDatos = new AccesoDatos();
        int vSeGraboInspeccionRCVehicular = -1;
        int vIdInspeccion = vAccesoDatos.FFlujoTieneRCVehicular(int.Parse(TextBoxIdFlujo.Text));
        //Si no existe la inspeccion de RC Vehicular se crea la inspeccion correspondiente
        if (0 == vIdInspeccion)
        {
          vSeGraboInspeccionRCVehicular = CreaInspRCVehicularFlujo(int.Parse(TextBoxIdFlujo.Text));

          if (vSeGraboInspeccionRCVehicular > 0)
          {
            TabPanelRCV01.Enabled = true;
            TabPanelRCV01.Visible = true;
            int vResul = FGrabaCambiosInspeccion();
            vResul = FlTraeDatosRCV01(int.Parse(TextBoxNroInspeccion.Text));
            PBloqueoRCVehicular01(true);
          }
        }
        else
        {
          TabPanelRCV01.Enabled = true;
          TabPanelRCV01.Visible = true;
          int vResul = FGrabaCambiosInspeccion();
          vResul = FlTraeDatosRCV01(int.Parse(TextBoxNroInspeccion.Text));
          PBloqueoRCVehicular01(true);
        }

      }
      else
        TabPanelRCV01.Enabled = false;
    }

    #endregion

    #region Danios Propios Padre

    protected void GridViewDaniosPropiosPadre_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      if (!VerificarPagina(true)) return;
      if (e.Row.RowType == DataControlRowType.DataRow)
      {
        //e.Row.Attributes["onmouseover"] = "this.style.backgroundColor='aquamarine';";
        //e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='white';";

        //verificar el estado del registro
        string vEstadoCadena = string.Empty;
        int vEstado = 0;
        vEstadoCadena = e.Row.Cells[5].Text;
        vEstado = int.Parse(vEstadoCadena);
        if (1 == vEstado)
        {
          (e.Row.Cells[6].Controls[0] as LinkButton).Enabled = true;
          //ConfirmarFinalizarInspeccion
          (e.Row.Cells[6].Controls[0] as LinkButton).Attributes.Add("OnClick", "javascript:return ConfirmarFinalizarInspeccion()");
        }
        else
        {
          (e.Row.Cells[6].Controls[0] as LinkButton).Enabled = false;
        }

        //generamos la consulta para cada fila de la grilla maestra
        string vTextoSecuencial = string.Empty;
        int vSecuencial = 0;

        vTextoSecuencial = e.Row.Cells[3].Text;
        vSecuencial = int.Parse(vTextoSecuencial);

        AccesoDatos vAccesoDatos = new AccesoDatos();
        var gvDPDet = (GridView)e.Row.FindControl("gvDPDet");

        using (LBCDesaEntities db = new LBCDesaEntities())
        {
          var vLst = from idpp in db.InspDaniosPropiosPadre
                     join idp in db.InspDaniosPropios on idpp.secuencial equals idp.secuencial
                     join n in db.Nomenclador on idp.idItem equals n.codigo
                     where (idpp.secuencial == vSecuencial)
                        && (n.categoriaNomenclador == "Item")
                     select new
                     {
                       idp.secuencial,
                       n.descripcion,
                       idp.compra,
                       idp.chaperio,
                       idp.mecanico
                     };

          gvDPDet.DataSource = vLst.ToList();
          gvDPDet.DataBind();
        }

      }
    }

    protected void GridViewDaniosPropiosPadre_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesoDatos = new AccesoDatos();
      InspeccionDaniosPropiosPadre vInspeccionDPPadre = new InspeccionDaniosPropiosPadre();

      int vDPPadreSecuencial = 0;
      int vDPPadreIdInspeccion = 0;

      vDPPadreSecuencial = int.Parse(GridViewDaniosPropiosPadre.SelectedRow.Cells[3].Text);
      vDPPadreIdInspeccion = int.Parse(TextBoxNroInspeccion.Text);

      vInspeccionDPPadre = vAccesoDatos.FTraeInspDPPadreICRL(vDPPadreSecuencial, vDPPadreIdInspeccion);

      if (null != vInspeccionDPPadre)
      {
        TextBoxDPPSecuencial.Text = vDPPadreSecuencial.ToString();
        string vTextoTipoTaller = string.Empty;
        vTextoTipoTaller = vInspeccionDPPadre.tipoTaller.Trim();
        DropDownListDPPTipoTaller.ClearSelection();
        DropDownListDPPTipoTaller.Items.FindByText(vTextoTipoTaller).Selected = true;
        CheckBoxDPPCambioPerdidaTotal.Checked = (GridViewDaniosPropiosPadre.SelectedRow.Cells[7].Controls[1] as CheckBox).Checked;
      }
      PBloqueaDPPadreEdicion(true);
    }

    protected void PBloqueaDPPadreEdicion(bool pEstado)
    {
      if (pEstado)
      {
        DropDownListDPPTipoTaller.Enabled = true;
        CheckBoxDPPCambioPerdidaTotal.Enabled = true;
        ButtonNuevoDPPadre.Enabled = false;
        ButtonGrabarDPPadre.Enabled = true;
        ButtonBorrarDPPadre.Enabled = true;
        ButtonDetalleDPPadre.Enabled = true;
        GridViewDaniosPropiosPadre.Enabled = false;
      }
      else
      {
        DropDownListDPPTipoTaller.Enabled = true;
        CheckBoxDPPCambioPerdidaTotal.Enabled = true;
        ButtonNuevoDPPadre.Enabled = true;
        ButtonGrabarDPPadre.Enabled = false;
        ButtonBorrarDPPadre.Enabled = false;
        ButtonDetalleDPPadre.Enabled = false;
        GridViewDaniosPropiosPadre.Enabled = true;
      }
    }

    private int FlTraeNomenTipoTallerDPPadre()
    {
      int vResultado = 0;
      string vCategoria = "Tipo Taller";
      int vOrdenCodigo = 2;
      AccesoDatos vAccesoDatos = new AccesoDatos();

      DropDownListDPPTipoTaller.DataValueField = "codigo";
      DropDownListDPPTipoTaller.DataTextField = "descripcion";
      DropDownListDPPTipoTaller.DataSource = vAccesoDatos.FlTraeNomenGenerico(vCategoria, vOrdenCodigo);
      DropDownListDPPTipoTaller.DataBind();

      return vResultado;
    }

    protected void ButtonNuevoDPPadre_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionDaniosPropiosPadre vInspeccionDaniosPropiosPadre = new InspeccionDaniosPropiosPadre();

      vInspeccionDaniosPropiosPadre.idInspeccion = int.Parse(TextBoxNroInspeccion.Text);
      vInspeccionDaniosPropiosPadre.tipoTaller = DropDownListDPPTipoTaller.SelectedItem.Text;
      vInspeccionDaniosPropiosPadre.cambioAPerdidaTotal = CheckBoxDPPCambioPerdidaTotal.Checked;
      vInspeccionDaniosPropiosPadre.estado = 1;

      int vResultado = vAccesodatos.FGrabaInspDaniosPropiosPadreICRL(vInspeccionDaniosPropiosPadre);

      if (vResultado > 0)
      {
        int vResul = FlTraeDatosDaniosPropiosPadre(int.Parse(TextBoxNroInspeccion.Text));
        PLimpiaSeccionDaniosPropiosPadre();
        PBloqueaDPPadreEdicion(false);
      }
    }

    protected void ButtonGrabarDPPadre_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionDaniosPropiosPadre vInspeccionDaniosPropiosPadre = new InspeccionDaniosPropiosPadre();

      vInspeccionDaniosPropiosPadre.secuencial = int.Parse(TextBoxDPPSecuencial.Text);
      vInspeccionDaniosPropiosPadre.idInspeccion = int.Parse(TextBoxNroInspeccion.Text);
      vInspeccionDaniosPropiosPadre.tipoTaller = DropDownListDPPTipoTaller.SelectedItem.Text;
      vInspeccionDaniosPropiosPadre.cambioAPerdidaTotal = CheckBoxDPPCambioPerdidaTotal.Checked;

      int vResultado = vAccesodatos.FActualizaInspDaniosPropiosPadreICRL(vInspeccionDaniosPropiosPadre);

      if (vResultado > 0)
      {
        int vResul = FlTraeDatosDaniosPropiosPadre(int.Parse(TextBoxNroInspeccion.Text));
        PLimpiaSeccionDaniosPropiosPadre();
        PBloqueaDPPadreEdicion(false);
      }
    }

    protected void ButtonBorrarDPPadre_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionDaniosPropiosPadre vInspeccionDaniosPropiosPadre = new InspeccionDaniosPropiosPadre();

      vInspeccionDaniosPropiosPadre.secuencial = int.Parse(TextBoxDPPSecuencial.Text);
      vInspeccionDaniosPropiosPadre.idInspeccion = int.Parse(TextBoxNroInspeccion.Text);

      int vResultado = vAccesodatos.FBorrarInspDaniosPropiosPadreICRL(vInspeccionDaniosPropiosPadre);

      if (vResultado > 0)
      {
        int vResul = FlTraeDatosDaniosPropiosPadre(int.Parse(TextBoxNroInspeccion.Text));
        PLimpiaSeccionDaniosPropiosPadre();
        PBloqueaDPPadreEdicion(false);
      }
    }

    protected void ButtonDetalleDPPadre_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      PBloqueaDPPadreEdicion(false);
      //PBloqueaPersonaDet(true);
      int vSecuencial = int.Parse(TextBoxDPPSecuencial.Text);
      FlTraeDatosDaniosPropios(vSecuencial);
      Session["PopupDPHabilitado"] = 1;
      this.ModalPopupDaniosPropios.Show();
    }

    protected void PLimpiaSeccionDaniosPropiosPadre()
    {
      DropDownListDPPTipoTaller.SelectedIndex = 0;
      CheckBoxDPPCambioPerdidaTotal.Checked = false;
    }

    protected void ButtonCancelPopDP_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      int vResul = 0;
      PLimpiaSeccionDaniosPropiosPadre();
      PLimpiaSeccionDatosPropios();
      PBloqueaDPPadreEdicion(false);
      PBloqueaDPEdicion(false);
      vResul = FlTraeDatosDaniosPropiosPadre(int.Parse(TextBoxNroInspeccion.Text));
      Session["PopupDPHabilitado"] = 0;
      this.ModalPopupDaniosPropios.Hide();
    }

    protected void GridViewDaniosPropiosPadre_RowCommand(object sender, GridViewCommandEventArgs e)
    {
      if (!VerificarPagina(true)) return;
      if (0 == e.CommandName.CompareTo("ImprimirFormularioInsp"))
      {
        string vTextoSecuencial = string.Empty;
        int vIndex = 0;
        int vSecuencial = 0;

        vIndex = Convert.ToInt32(e.CommandArgument);
        vSecuencial = Convert.ToInt32(GridViewDaniosPropiosPadre.DataKeys[vIndex].Value);
        PImprimeFormularioInspDaniosPropios(vSecuencial);
      }

      if (0 == e.CommandName.CompareTo("FinalizarInsp"))
      {
        string vTextoSecuencial = string.Empty;
        int vIndex = 0;
        int vSecuencial = 0;

        vIndex = Convert.ToInt32(e.CommandArgument);
        vSecuencial = Convert.ToInt32(GridViewDaniosPropiosPadre.DataKeys[vIndex].Value);
        //proceso que copia los datos de Inps a Coti
        AccesoDatos vAccesoDatos = new AccesoDatos();
        int vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
        int vidInspecccion = int.Parse(TextBoxNroInspeccion.Text);
        vAccesoDatos.fCopiaDaniosPropiosInspecACotizacion(vIdFlujo, vidInspecccion, vSecuencial);
        //cambiar estado de la cobertura para que no se pueda volver a ejecutar
        BD.InspeccionDaniosPropiosPadre vInspeccionDaniosPropiosPadre = new InspeccionDaniosPropiosPadre();
        vInspeccionDaniosPropiosPadre.idInspeccion = vidInspecccion;
        vInspeccionDaniosPropiosPadre.secuencial = vSecuencial;
        int vResultado = 0;
        vResultado = vAccesoDatos.FDaniosPropioPadreCambiaEstado(vInspeccionDaniosPropiosPadre);
      }

      int vResul = 0;
      vResul = FlTraeDatosDaniosPropiosPadre(int.Parse(TextBoxNroInspeccion.Text));
    }

    #endregion

    #region Danios Propios



    private void ValidaDaniosPropiosFlujo(int pIdFlujo)
    {
      AccesoDatos vAccesodatos = new AccesoDatos();

      bool vSeleccionado = false;
      int vResul = 0;
      int vIdInspeccion = 0;

      vIdInspeccion = vAccesodatos.FFlujoTieneDaniosPropios(pIdFlujo);

      if (vIdInspeccion > 0)
      {
        vSeleccionado = vAccesodatos.FInspeccionTieneDPICRL(vIdInspeccion);
        if (vSeleccionado)
        {
          TabPanelDaniosPropios.Enabled = true;
          TabPanelDaniosPropios.Visible = true;
          CheckBoxDaniosPropios.Checked = vSeleccionado;
          vResul = FlTraeDatosDaniosPropiosPadre(vIdInspeccion);
          //TabContainerCoberturas.ActiveTabIndex = 0;
        }
      }
    }

    protected void GridViewDaniosPropios_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      if (!VerificarPagina(true)) return;
      if (e.Row.RowType == DataControlRowType.DataRow)
      {
        e.Row.Attributes["onmouseover"] = "this.style.backgroundColor='aquamarine';";
        e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='white';";
        e.Row.ToolTip = "Haz clic en la primera columna para seleccionar la fila.";
      }
    }

    protected void PLimpiaSeccionDatosPropios()
    {
      DropDownListItem.SelectedIndex = 0;
      DropDownListItem.Enabled = true;
      DropDownListCompra.SelectedIndex = 0;
      DropDownListCompra.Enabled = true;
      //TextBoxCompra.Text = string.Empty;
      CheckBoxInstalacion.Checked = false;
      CheckBoxPintura.Checked = false;
      CheckBoxMecanico.Checked = false;
      DropDownListChaperio.SelectedIndex = 0;
      DropDownListChaperio.Enabled = true;
      //TextBoxChaperio.Text = string.Empty;
      DropDownListRepPrevia.SelectedIndex = 0;
      DropDownListRepPrevia.Enabled = true;
      //TextBoxRepPrevia.Text = string.Empty;
      TextBoxObservaciones.Text = string.Empty;
      ButtonGrabarDP.Enabled = false;
      ButtonBorrarDP.Enabled = false;
      ButtonNuevoDP.Enabled = true;
    }

    protected void PBloqueaDPEdicion(bool pEstado)
    {
      if (pEstado)
      {
        //DropDownListDPPTipoTaller.Enabled = true;
        //CheckBoxDPPCambioPerdidaTotal.Enabled = true;
        ButtonNuevoDP.Enabled = false;
        ButtonGrabarDP.Enabled = true;
        ButtonBorrarDP.Enabled = true;
        GridViewDaniosPropios.Enabled = false;
      }
      else
      {
        //DropDownListDPPTipoTaller.Enabled = true;
        //CheckBoxDPPCambioPerdidaTotal.Enabled = true;
        ButtonNuevoDP.Enabled = true;
        ButtonGrabarDP.Enabled = false;
        ButtonBorrarDP.Enabled = false;
        GridViewDaniosPropios.Enabled = true;
      }
    }

    protected void GridViewDaniosPropios_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      int vSecuencial = 0;
      string vTextoTemporal = string.Empty;
      vSecuencial = int.Parse(TextBoxDPPSecuencial.Text);

      DropDownListItem.Enabled = false;
      TextBoxIdItem.Text = string.Empty;
      TextBoxIdItem.Text = GridViewDaniosPropios.SelectedRow.Cells[1].Text.Substring(0, 8);
      string vTextoItemDP = GridViewDaniosPropios.SelectedRow.Cells[2].Text;

      DropDownListItem.ClearSelection();
      DropDownListItem.Items.FindByValue(TextBoxIdItem.Text).Selected = true;

      string vTextoCompra = string.Empty;
      vTextoCompra = GridViewDaniosPropios.SelectedRow.Cells[3].Text.Trim();
      DropDownListCompra.ClearSelection();
      DropDownListCompra.Items.FindByText(vTextoCompra).Selected = true;

      CheckBoxInstalacion.Checked = (GridViewDaniosPropios.SelectedRow.Cells[4].Controls[1] as CheckBox).Checked;
      CheckBoxPintura.Checked = (GridViewDaniosPropios.SelectedRow.Cells[5].Controls[1] as CheckBox).Checked;
      CheckBoxMecanico.Checked = (GridViewDaniosPropios.SelectedRow.Cells[6].Controls[1] as CheckBox).Checked;

      string vTextoChaperio = string.Empty;
      vTextoChaperio = GridViewDaniosPropios.SelectedRow.Cells[7].Text.Trim();
      DropDownListChaperio.ClearSelection();
      DropDownListChaperio.Items.FindByText(vTextoChaperio).Selected = true;

      string vTextoRepPrevia = string.Empty;
      vTextoRepPrevia = GridViewDaniosPropios.SelectedRow.Cells[8].Text.Trim();
      DropDownListRepPrevia.ClearSelection();
      DropDownListRepPrevia.Items.FindByText(vTextoRepPrevia).Selected = true;

      vTextoTemporal = string.Empty;
      vTextoTemporal = GridViewDaniosPropios.SelectedRow.Cells[9].Text;
      vTextoTemporal = vTextoTemporal.Replace("&#209;", "Ñ");
      vTextoTemporal = vTextoTemporal.Replace("&nbsp;", string.Empty);
      TextBoxObservaciones.Text = vTextoTemporal;
      TextBoxNroItem.Text = GridViewDaniosPropios.SelectedRow.Cells[10].Text;
      ButtonNuevoDP.Enabled = false;
      ButtonGrabarDP.Enabled = true;
      ButtonBorrarDP.Enabled = true;
    }

    protected void ButtonNuevoDP_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionDaniosPropios vInspDaniosPropios = new InspeccionDaniosPropios();

      vInspDaniosPropios.secuencial = int.Parse(TextBoxDPPSecuencial.Text);
      vInspDaniosPropios.idItem = DropDownListItem.SelectedValue;
      vInspDaniosPropios.compra = DropDownListCompra.SelectedItem.Text;
      vInspDaniosPropios.instalacion = CheckBoxInstalacion.Checked;
      vInspDaniosPropios.pintura = CheckBoxPintura.Checked;
      vInspDaniosPropios.mecanico = CheckBoxMecanico.Checked;
      vInspDaniosPropios.chaperio = DropDownListChaperio.SelectedItem.Text;
      vInspDaniosPropios.reparacionPrevia = DropDownListRepPrevia.SelectedItem.Text;
      vInspDaniosPropios.observaciones = TextBoxObservaciones.Text.ToUpper().Trim();

      int vResultado = vAccesodatos.FGrabaInspDaniosPropiosICRL(vInspDaniosPropios);

      if (vResultado > 0)
      {
        int vResul = FlTraeDatosDaniosPropios(int.Parse(TextBoxDPPSecuencial.Text));
        PLimpiaSeccionDatosPropios();
      }
    }

    protected void ButtonGrabarDP_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionDaniosPropios vInspDaniosPropios = new InspeccionDaniosPropios();

      vInspDaniosPropios.secuencial = int.Parse(TextBoxDPPSecuencial.Text);
      vInspDaniosPropios.idItem = DropDownListItem.SelectedValue;
      vInspDaniosPropios.compra = DropDownListCompra.SelectedItem.Text;
      vInspDaniosPropios.instalacion = CheckBoxInstalacion.Checked;
      vInspDaniosPropios.pintura = CheckBoxPintura.Checked;
      vInspDaniosPropios.mecanico = CheckBoxMecanico.Checked;
      vInspDaniosPropios.chaperio = DropDownListChaperio.SelectedItem.Text;
      vInspDaniosPropios.reparacionPrevia = DropDownListRepPrevia.SelectedItem.Text;
      vInspDaniosPropios.observaciones = TextBoxObservaciones.Text.ToUpper().Trim();
      vInspDaniosPropios.nro_item = long.Parse(TextBoxNroItem.Text);

      int vResultado = vAccesodatos.FActualizaInspDaniosPropiosICRL(vInspDaniosPropios);

      if (vResultado > 0)
      {
        int vResul = FlTraeDatosDaniosPropios(int.Parse(TextBoxDPPSecuencial.Text));
        PLimpiaSeccionDatosPropios();
      }
    }

    protected void ButtonBorrarDP_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionDaniosPropios vInspDaniosPropios = new InspeccionDaniosPropios();

      vInspDaniosPropios.secuencial = int.Parse(TextBoxDPPSecuencial.Text);
      vInspDaniosPropios.idItem = DropDownListItem.SelectedValue;
      vInspDaniosPropios.nro_item = long.Parse(TextBoxNroItem.Text);

      int vResultado = vAccesodatos.FBorrarInspDaniosPropiosICRL(vInspDaniosPropios);

      if (vResultado > 0)
      {
        int vResul = FlTraeDatosDaniosPropios(int.Parse(TextBoxDPPSecuencial.Text));
        PLimpiaSeccionDatosPropios();
      }
    }

    private int FlTraeNomenCompraDet()
    {
      int vResultado = 0;
      string vCategoria = "Compra Repuesto";
      int vOrdenCodigo = 2;
      AccesoDatos vAccesoDatos = new AccesoDatos();

      DropDownListCompra.DataValueField = "codigo";
      DropDownListCompra.DataTextField = "descripcion";
      DropDownListCompra.DataSource = vAccesoDatos.FlTraeNomenGenerico(vCategoria, vOrdenCodigo);
      DropDownListCompra.DataBind();

      return vResultado;
    }

    private int FlTraeNomenChaperioDet()
    {
      int vResultado = 0;
      string vCategoria = "Nivel de Daño";
      int vOrdenCodigo = 2;
      AccesoDatos vAccesoDatos = new AccesoDatos();

      DropDownListChaperio.DataValueField = "codigo";
      DropDownListChaperio.DataTextField = "descripcion";
      DropDownListChaperio.DataSource = vAccesoDatos.FlTraeNomenGenerico(vCategoria, vOrdenCodigo);
      DropDownListChaperio.DataBind();

      return vResultado;
    }

    private int FlTraeNomenRepPreviaDet()
    {
      int vResultado = 0;
      string vCategoria = "Nivel de Daño";
      int vOrdenCodigo = 2;
      AccesoDatos vAccesoDatos = new AccesoDatos();

      DropDownListRepPrevia.DataValueField = "codigo";
      DropDownListRepPrevia.DataTextField = "descripcion";
      DropDownListRepPrevia.DataSource = vAccesoDatos.FlTraeNomenGenerico(vCategoria, vOrdenCodigo);
      DropDownListRepPrevia.DataBind();

      return vResultado;
    }

    #endregion

    #region RC Objeto

    private int FlTraeDatosRCObjetos(int pIdInspeccion)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vLst = from i in db.Inspeccion
                   join ob in db.InspRCObjeto on i.idInspeccion equals ob.idInspeccion
                   where i.idInspeccion == pIdInspeccion
                   select new
                   {
                     ob.secuencial,
                     ob.nombreObjeto,
                     ob.docIdentidadObjeto,
                     ob.telefonoObjeto,
                     ob.observacionesObjeto,
                     ob.estado
                   };
        GridViewObjetos.DataSource = vLst.ToList();
        GridViewObjetos.DataBind();
      }

      return vResultado;
    }

    public string MyNewRowObjDet(object pIdSecuencial)
    {
      return String.Format(@"</td></tr><tr id ='trrco{0}' class='collapsed-row'>
                                <td></td><td colspan='100' style='padding:0px; margin:0px;'>", pIdSecuencial);
    }

    protected void GridViewObjetos_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesoDatos = new AccesoDatos();
      InspeccionRCObjeto vInspeccionRCObjeto = new InspeccionRCObjeto();
      string vTextoTemporal = string.Empty;

      int vObjIdSecuencial = 0;
      int vObjIdInspeccion = 0;

      vObjIdSecuencial = int.Parse(GridViewObjetos.SelectedRow.Cells[3].Text);
      vObjIdInspeccion = int.Parse(TextBoxNroInspeccion.Text);

      vInspeccionRCObjeto = vAccesoDatos.FTraeInspRCObjetoICRL(vObjIdSecuencial, vObjIdInspeccion);

      if (null != vInspeccionRCObjeto)
      {
        TextBoxNombresApObjeto.Enabled = false;
        TextBoxDocIdObjeto.Enabled = false;
        TextBoxObjIdSecuencial.Text = vObjIdSecuencial.ToString();
        TextBoxNombresApObjeto.Text = vInspeccionRCObjeto.nombreObjeto;
        TextBoxDocIdObjeto.Text = vInspeccionRCObjeto.docIdentidadObjeto;
        TextBoxTelfObjeto.Text = vInspeccionRCObjeto.telefonoObjeto;
        vTextoTemporal = string.Empty;
        vTextoTemporal = vInspeccionRCObjeto.observacionesObjeto;
        vTextoTemporal = vTextoTemporal.Replace("&#209;", "Ñ");
        vTextoTemporal = vTextoTemporal.Replace("&nbsp;", string.Empty);
        TextBoxObsObjeto.Text = vTextoTemporal;
      }
      PBloqueaObjetoEdicion(true);
    }

    protected void PBloqueaObjetoEdicion(bool pEstado)
    {
      if (pEstado)
      {
        TextBoxNombresApObjeto.Enabled = false;
        TextBoxDocIdObjeto.Enabled = false;
        TextBoxObsObjeto.Enabled = true;
        TextBoxTelfObjeto.Enabled = true;
        ButtonDetalleObj.Enabled = true;
        ButtonGrabarObj.Enabled = true;
        ButtonBorrarObj.Enabled = true;
        ButtonNuevoObj.Enabled = false;
        GridViewObjetos.Enabled = false;
      }
      else
      {
        TextBoxNombresApObjeto.Enabled = true;
        TextBoxDocIdObjeto.Enabled = true;
        TextBoxObsObjeto.Enabled = true;
        TextBoxTelfObjeto.Enabled = true;
        ButtonDetalleObj.Enabled = false;
        ButtonGrabarObj.Enabled = false;
        ButtonBorrarObj.Enabled = false;
        ButtonNuevoObj.Enabled = true;
        GridViewObjetos.Enabled = true;
      }
    }

    protected void PLimpiaSeccionRCObjetos()
    {
      TextBoxNombresApObjeto.Enabled = true;
      TextBoxDocIdObjeto.Enabled = true;
      TextBoxNombresApObjeto.Text = string.Empty;
      TextBoxDocIdObjeto.Text = string.Empty;
      TextBoxObsObjeto.Text = string.Empty;
      TextBoxTelfObjeto.Text = string.Empty;
      ButtonGrabarObj.Enabled = false;
      ButtonBorrarObj.Enabled = false;
      ButtonNuevoObj.Enabled = true;
      ButtonDetalleObj.Enabled = false;
    }

    protected void GridViewObjetos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
      if (!VerificarPagina(true)) return;
      if (0 == e.CommandName.CompareTo("ImprimirFormularioInsp"))
      {
        string vTextoSecuencial = string.Empty;
        int vIndex = 0;
        int vSecuencial = 0;

        vIndex = Convert.ToInt32(e.CommandArgument);
        vSecuencial = Convert.ToInt32(GridViewObjetos.DataKeys[vIndex].Value);
        PImprimeFormularioInspRCObjeto(vSecuencial);
      }

      if (0 == e.CommandName.CompareTo("FinalizarInsp"))
      {
        string vTextoSecuencial = string.Empty;
        int vIndex = 0;
        int vSecuencial = 0;

        vIndex = Convert.ToInt32(e.CommandArgument);
        vSecuencial = Convert.ToInt32(GridViewObjetos.DataKeys[vIndex].Value);
        //proceso que copia los datos de Inps a Coti
        AccesoDatos vAccesoDatos = new AccesoDatos();
        int vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
        int vidInspecccion = int.Parse(TextBoxNroInspeccion.Text);
        vAccesoDatos.fCopiaRCObjetoInspACotizacion(vIdFlujo, vidInspecccion, vSecuencial);
        //cambiar estado de la cobertura para que no se pueda volver a ejecutar
        BD.InspeccionRCObjeto vInspeccionRCObjeto = new InspeccionRCObjeto();
        vInspeccionRCObjeto.idInspeccion = vidInspecccion;
        vInspeccionRCObjeto.secuencial = vSecuencial;
        int vResultado = 0;
        vResultado = vAccesoDatos.FRCObjetoCambiaEstado(vInspeccionRCObjeto);
      }

      int vResul = 0;
      vResul = FlTraeDatosRCObjetos(int.Parse(TextBoxNroInspeccion.Text));
    }

    protected void GridViewObjetos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      if (!VerificarPagina(true)) return;
      if (e.Row.RowType == DataControlRowType.DataRow)
      {
        //e.Row.Attributes["onmouseover"] = "this.style.backgroundColor='aquamarine';";
        //e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='white';";

        //verificar el estado del registro
        string vEstadoCadena = string.Empty;
        int vEstado = 0;
        vEstadoCadena = e.Row.Cells[7].Text;
        vEstado = int.Parse(vEstadoCadena);
        if (1 == vEstado)
        {
          (e.Row.Cells[8].Controls[0] as LinkButton).Enabled = true;
          //ConfirmarFinalizarInspeccion
          (e.Row.Cells[8].Controls[0] as LinkButton).Attributes.Add("OnClick", "javascript:return ConfirmarFinalizarInspeccion()");
        }
        else
        {
          (e.Row.Cells[8].Controls[0] as LinkButton).Enabled = false;
        }

        //generamos la consulta para cada fila de la grilla maestra
        string vTextoSecuencial = string.Empty;
        int vSecuencial = 0;

        vTextoSecuencial = e.Row.Cells[3].Text;
        vSecuencial = int.Parse(vTextoSecuencial);

        AccesoDatos vAccesoDatos = new AccesoDatos();
        var gvRCObjDet = (GridView)e.Row.FindControl("gvRCObjDet");

        using (LBCDesaEntities db = new LBCDesaEntities())
        {
          var vLst = from ircobj in db.InspRCObjeto
                     join ircobjdet in db.InspRCObjetoDetalle on ircobj.secuencial equals ircobjdet.secuencial
                     where (ircobj.secuencial == vSecuencial)
                     select new
                     {
                       ircobjdet.secuencial,
                       ircobjdet.idItem,
                       ircobjdet.costoReferencial,
                       ircobjdet.descripcion
                     };

          gvRCObjDet.DataSource = vLst.ToList();
          gvRCObjDet.DataBind();
        }

      }
    }

    protected void ButtonNuevoObj_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionRCObjeto vInspeccionRCObjetos = new InspeccionRCObjeto();

      vInspeccionRCObjetos.idInspeccion = int.Parse(TextBoxNroInspeccion.Text);
      vInspeccionRCObjetos.nombreObjeto = TextBoxNombresApObjeto.Text;
      vInspeccionRCObjetos.docIdentidadObjeto = TextBoxDocIdObjeto.Text;
      vInspeccionRCObjetos.observacionesObjeto = TextBoxObsObjeto.Text.ToUpper().Trim();
      vInspeccionRCObjetos.telefonoObjeto = TextBoxTelfObjeto.Text;
      vInspeccionRCObjetos.estado = 1;

      int vResultado = vAccesodatos.FGrabaInspRCObjetosICRL(vInspeccionRCObjetos);

      if (vResultado > 0)
      {
        int vResul = FlTraeDatosRCObjetos(int.Parse(TextBoxNroInspeccion.Text));
        PLimpiaSeccionRCObjetos();
        PBloqueaObjetoEdicion(false);
      }
    }

    protected void ButtonGrabarObj_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionRCObjeto vInspeccionRCObjetos = new InspeccionRCObjeto();

      vInspeccionRCObjetos.idInspeccion = int.Parse(TextBoxNroInspeccion.Text);
      vInspeccionRCObjetos.secuencial = int.Parse(TextBoxObjIdSecuencial.Text);
      vInspeccionRCObjetos.nombreObjeto = TextBoxNombresApObjeto.Text;
      vInspeccionRCObjetos.docIdentidadObjeto = TextBoxDocIdObjeto.Text;
      vInspeccionRCObjetos.observacionesObjeto = TextBoxObsObjeto.Text.ToUpper().Trim();
      vInspeccionRCObjetos.telefonoObjeto = TextBoxTelfObjeto.Text;

      int vResultado = vAccesodatos.FActualizaInspRCObjetosICRL(vInspeccionRCObjetos);

      if (vResultado > 0)
      {
        int vResul = FlTraeDatosRCObjetos(int.Parse(TextBoxNroInspeccion.Text));
        PLimpiaSeccionRCObjetos();
        PBloqueaObjetoEdicion(false);
      }
    }

    protected void ButtonBorrarObj_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionRCObjeto vInspeccionRCObjetos = new InspeccionRCObjeto();

      vInspeccionRCObjetos.idInspeccion = int.Parse(TextBoxNroInspeccion.Text);
      vInspeccionRCObjetos.secuencial = int.Parse(TextBoxObjIdSecuencial.Text);

      int vResultado = vAccesodatos.FBorrarInspRCObjetosICRL(vInspeccionRCObjetos);

      if (vResultado > 0)
      {
        int vResul = FlTraeDatosRCObjetos(int.Parse(TextBoxNroInspeccion.Text));
        PLimpiaSeccionRCObjetos();
        PBloqueaObjetoEdicion(false);
      }
    }

    protected void PBloqueaObjeto(bool pEstado)
    {
      TextBoxNombresApObjeto.Enabled = pEstado;
      TextBoxDocIdObjeto.Enabled = pEstado;
      TextBoxObsObjeto.Enabled = pEstado;
      TextBoxTelfObjeto.Enabled = pEstado;
      ButtonGrabarObj.Enabled = pEstado;
      ButtonBorrarObj.Enabled = pEstado;
      ButtonNuevoObj.Enabled = pEstado;
      ButtonDetalleObj.Enabled = pEstado;
      GridViewObjetos.Enabled = pEstado;
    }

    protected void ButtonDetalleObj_Click(object sender, EventArgs e)
    {
      //int vSecuencial = int.Parse(TextBoxObjIdSecuencial.Text);
      //PBloqueaObjeto(false);
      //PBloqueaObjetoDet(true);
      //PLimpiaSeccionRCObjetoDetalle();
      //FlTraeDatosRCObjetoDetalles(vSecuencial);
      if (!VerificarPagina(true)) return;
      PBloqueaObjeto(false);
      PBloqueaObjetoDet(true);
      int vSecuencial = int.Parse(TextBoxObjIdSecuencial.Text);
      FlTraeDatosRCObjetoDetalles(vSecuencial);
      Session["PopupRCObjHabilitado"] = 1;
      this.ModalPopupRCObjetos.Show();

    }


    //Este boton cerraba el detalle antes
    //protected void ButtonCierraObjDet_Click(object sender, EventArgs e)
    //{
    //    int vResul = 0;
    //    PBloqueaObjeto(true);
    //    PBloqueaObjetoDet(false);
    //    PLimpiaSeccionRCObjetos();
    //    vResul = FlTraeDatosRCObjetos(int.Parse(TextBoxNroInspeccion.Text));
    //}

    protected void ButtonCancelPopRCObj_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      int vResul = 0;
      PBloqueaObjeto(true);
      PLimpiaSeccionRCObjetos();
      vResul = FlTraeDatosRCObjetos(int.Parse(TextBoxNroInspeccion.Text));
      Session["PopupRCObjHabilitado"] = 0;
      this.ModalPopupRCObjetos.Hide();
    }

    #endregion

    #region RC Objeto Detalle

    protected void PBloqueaObjetoDet(bool pEstado)
    {
      TextBoxObjDetItem.Enabled = pEstado;
      TextBoxObjDetCostoRef.Enabled = pEstado;
      TextBoxObjDetDescripcion.Enabled = pEstado;
      ButtonGrabarObjDet.Enabled = !pEstado;
      ButtonBorrarObjDet.Enabled = !pEstado;
      ButtonNuevoObjDet.Enabled = pEstado;
      //ButtonCierraObjDet.Enabled = pEstado;
      GridViewObjDetalle.Enabled = pEstado;
    }

    protected void PLimpiaSeccionRCObjetoDetalle()
    {
      TextBoxObjDetItem.Text = string.Empty;
      TextBoxObjDetCostoRef.Text = "0";
      TextBoxObjDetDescripcion.Text = string.Empty;
      //ButtonGrabarObjDet.Enabled = false;
      //ButtonBorrarObjDet.Enabled = false;
      //ButtonNuevoObjDet.Enabled = true;
      PBloqueaObjetoDet(true);
      //ButtonCierraObjDet.Enabled = true;
    }

    private int FlTraeDatosRCObjetoDetalles(int pSecuencial)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vLst = from ob in db.InspRCObjeto
                   join obdet in db.InspRCObjetoDetalle on ob.secuencial equals obdet.secuencial
                   where ob.secuencial == pSecuencial
                   select new
                   {
                     ob.secuencial,
                     obdet.idItem,
                     obdet.costoReferencial,
                     obdet.descripcion,
                   };
        GridViewObjDetalle.DataSource = vLst.ToList();
        GridViewObjDetalle.DataBind();
      }

      return vResultado;
    }

    protected void ButtonNuevoObjDet_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionRCObjetoDet vInspeccionRCObjetoDetalle = new InspeccionRCObjetoDet();

      vInspeccionRCObjetoDetalle.secuencial = int.Parse(TextBoxObjIdSecuencial.Text);
      vInspeccionRCObjetoDetalle.itemObjDet = TextBoxObjDetItem.Text;
      vInspeccionRCObjetoDetalle.costoRefObjDet = decimal.Parse(TextBoxObjDetCostoRef.Text);
      vInspeccionRCObjetoDetalle.descripObjDet = TextBoxObjDetDescripcion.Text;

      int vResultado = vAccesodatos.FGrabaInspRCObjetoDetICRL(vInspeccionRCObjetoDetalle);

      if (vResultado > 0)
      {
        int vResul = FlTraeDatosRCObjetoDetalles(int.Parse(TextBoxObjIdSecuencial.Text));
        PLimpiaSeccionRCObjetoDetalle();
      }
    }

    protected void ButtonGrabarObjDet_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionRCObjetoDet vInspeccionRCObjetoDetalle = new InspeccionRCObjetoDet();

      vInspeccionRCObjetoDetalle.secuencial = int.Parse(TextBoxObjIdSecuencial.Text);
      vInspeccionRCObjetoDetalle.itemObjDet = TextBoxObjDetItem.Text;
      vInspeccionRCObjetoDetalle.costoRefObjDet = decimal.Parse(TextBoxObjDetCostoRef.Text);
      vInspeccionRCObjetoDetalle.descripObjDet = TextBoxObjDetDescripcion.Text;

      int vResultado = vAccesodatos.FActualizaInspRCObjetoDetICRL(vInspeccionRCObjetoDetalle);

      if (vResultado > 0)
      {
        int vResul = FlTraeDatosRCObjetoDetalles(int.Parse(TextBoxObjIdSecuencial.Text));
        PLimpiaSeccionRCObjetoDetalle();
      }
    }

    protected void ButtonBorrarObjDet_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionRCObjetoDet vInspeccionRCObjetoDetalle = new InspeccionRCObjetoDet();

      vInspeccionRCObjetoDetalle.secuencial = int.Parse(TextBoxObjIdSecuencial.Text);
      vInspeccionRCObjetoDetalle.itemObjDet = TextBoxObjDetItem.Text;
      vInspeccionRCObjetoDetalle.costoRefObjDet = decimal.Parse(TextBoxObjDetCostoRef.Text);
      vInspeccionRCObjetoDetalle.descripObjDet = TextBoxObjDetDescripcion.Text;

      int vResultado = vAccesodatos.FBorrarInspRCObjetoDetICRL(vInspeccionRCObjetoDetalle);

      if (vResultado > 0)
      {
        int vResul = FlTraeDatosRCObjetoDetalles(int.Parse(TextBoxObjIdSecuencial.Text));
        PLimpiaSeccionRCObjetoDetalle();
      }
    }

    protected void GridViewObjDetalle_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      int vObjIdSecuencial = 0;
      string vTextoTemporal = string.Empty;

      vObjIdSecuencial = int.Parse(GridViewObjDetalle.SelectedRow.Cells[1].Text);

      TextBoxObjDetItem.Text = GridViewObjDetalle.SelectedRow.Cells[2].Text;
      TextBoxObjDetItem.Enabled = false;
      TextBoxObjDetCostoRef.Text = GridViewObjDetalle.SelectedRow.Cells[3].Text;
      vTextoTemporal = string.Empty;
      vTextoTemporal = GridViewObjDetalle.SelectedRow.Cells[4].Text;
      vTextoTemporal = vTextoTemporal.Replace("&#209;", "Ñ");
      vTextoTemporal = vTextoTemporal.Replace("&nbsp;", string.Empty);
      TextBoxObjDetDescripcion.Text = vTextoTemporal;
      ButtonNuevoObjDet.Enabled = false;
      ButtonGrabarObjDet.Enabled = true;
      ButtonBorrarObjDet.Enabled = true;
      //ButtonCierraObjDet.Enabled = true;
    }

    protected void GridViewObjDetalle_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      if (!VerificarPagina(true)) return;
      if (e.Row.RowType == DataControlRowType.DataRow)
      {
        e.Row.Attributes["onmouseover"] = "this.style.backgroundColor='aquamarine';";
        e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='white';";
        e.Row.ToolTip = "Haz clic en la primera columna para seleccionar la fila.";
      }
    }


    #endregion

    #region RC Personas 

    public string MyNewRowPerDet(object pIdSecuencial)
    {
      return String.Format(@"</td></tr><tr id ='trrcp{0}' class='collapsed-row'>
                                <td></td><td colspan='100' style='padding:0px; margin:0px;'>", pIdSecuencial);
    }

    protected void GridViewPersonas_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesoDatos = new AccesoDatos();
      InspeccionRCPersona vInspeccionRCPersona = new InspeccionRCPersona();
      string vTextoTemporal = string.Empty;

      int vPerIdSecuencial = 0;
      int vPerIdInspeccion = 0;


      vPerIdSecuencial = int.Parse(GridViewPersonas.SelectedRow.Cells[3].Text);
      vPerIdInspeccion = int.Parse(TextBoxNroInspeccion.Text);

      vInspeccionRCPersona = vAccesoDatos.FTraeInspRCPersonaICRL(vPerIdSecuencial, vPerIdInspeccion);

      if (null != vInspeccionRCPersona)
      {
        TextBoxNombresApPersona.Enabled = false;
        TextBoxDocIdPersona.Enabled = false;
        TextBoxPersonaIdSecuencial.Text = vPerIdSecuencial.ToString();
        TextBoxNombresApPersona.Text = vInspeccionRCPersona.nombrePersona;
        TextBoxDocIdPersona.Text = vInspeccionRCPersona.docIdentidadPersona;
        TextBoxTelfPersona.Text = vInspeccionRCPersona.telefonoPersona;
        vTextoTemporal = string.Empty;
        vTextoTemporal = vInspeccionRCPersona.observacionesPersona;
        vTextoTemporal = vTextoTemporal.Replace("&#209;", "Ñ");
        vTextoTemporal = vTextoTemporal.Replace("&nbsp;", string.Empty);
        TextBoxObsPersona.Text = vTextoTemporal;
      }
      PBloqueaPersonaEdicion(true);
    }

    protected void GridViewPersonas_RowCommand(object sender, GridViewCommandEventArgs e)
    {
      if (!VerificarPagina(true)) return;
      if (0 == e.CommandName.CompareTo("ImprimirFormularioInsp"))
      {
        string vTextoSecuencial = string.Empty;
        int vIndex = 0;
        int vSecuencial = 0;

        vIndex = Convert.ToInt32(e.CommandArgument);
        vSecuencial = Convert.ToInt32(GridViewPersonas.DataKeys[vIndex].Value);
        PImprimeFormularioInspRCPersona(vSecuencial);
      }

      if (0 == e.CommandName.CompareTo("FinalizarInsp"))
      {
        string vTextoSecuencial = string.Empty;
        int vIndex = 0;
        int vSecuencial = 0;

        vIndex = Convert.ToInt32(e.CommandArgument);
        vSecuencial = Convert.ToInt32(GridViewPersonas.DataKeys[vIndex].Value);
        //proceso que copia los datos de Inps a Coti
        AccesoDatos vAccesoDatos = new AccesoDatos();
        int vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
        int vidInspecccion = int.Parse(TextBoxNroInspeccion.Text);
        vAccesoDatos.fCopiaRCPersonaInspACotizacion(vIdFlujo, vidInspecccion, vSecuencial);

        //cambiar estado de la cobertura para que no se pueda volver a ejecutar
        BD.InspeccionRCPersona vInspeccionRCPersona = new InspeccionRCPersona();
        vInspeccionRCPersona.idInspeccion = vidInspecccion;
        vInspeccionRCPersona.secuencial = vSecuencial;
        int vResultado = 0;
        vResultado = vAccesoDatos.FRCPersonaCambiaEstado(vInspeccionRCPersona);
      }

      int vResul = 0;
      vResul = FlTraeDatosRCPersonas(int.Parse(TextBoxNroInspeccion.Text));
    }

    protected void GridViewPersonas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      if (!VerificarPagina(true)) return;
      if (e.Row.RowType == DataControlRowType.DataRow)
      {
        //e.Row.Attributes["onmouseover"] = "this.style.backgroundColor='aquamarine';";
        //e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='white';";

        //verificar el estado del registro
        string vEstadoCadena = string.Empty;
        int vEstado = 0;
        vEstadoCadena = e.Row.Cells[7].Text;
        vEstado = int.Parse(vEstadoCadena);
        if (1 == vEstado)
        {
          (e.Row.Cells[8].Controls[0] as LinkButton).Enabled = true;
          //ConfirmarFinalizarInspeccion
          (e.Row.Cells[8].Controls[0] as LinkButton).Attributes.Add("OnClick", "javascript:return ConfirmarFinalizarInspeccion()");
        }
        else
        {
          (e.Row.Cells[8].Controls[0] as LinkButton).Enabled = false;
        }

        //generamos la consulta para cada fila de la grilla maestra persona
        string vTextoSecuencial = string.Empty;
        int vSecuencial = 0;

        vTextoSecuencial = e.Row.Cells[3].Text;
        vSecuencial = int.Parse(vTextoSecuencial);

        AccesoDatos vAccesoDatos = new AccesoDatos();
        var gvRCPerDet = (GridView)e.Row.FindControl("gvRCPerDet");

        using (LBCDesaEntities db = new LBCDesaEntities())
        {
          var vLst = from ircper in db.InspRCPersona
                     join ircperdet in db.InspRCPersonaDetalle on ircper.secuencial equals ircperdet.secuencial
                     where (ircper.secuencial == vSecuencial)
                     select new
                     {
                       ircperdet.secuencial,
                       ircperdet.tipo,
                       ircperdet.montoGasto,
                       ircperdet.descripcion,
                     };

          gvRCPerDet.DataSource = vLst.ToList();
          gvRCPerDet.DataBind();
        }
      }
    }

    protected void PLimpiaSeccionRCPersonas()
    {
      TextBoxNombresApPersona.Enabled = true;
      TextBoxDocIdPersona.Enabled = true;
      TextBoxNombresApPersona.Text = string.Empty;
      TextBoxDocIdPersona.Text = string.Empty;
      TextBoxObsPersona.Text = string.Empty;
      TextBoxTelfPersona.Text = string.Empty;
      ButtonGrabarPer.Enabled = false;
      ButtonBorrarPer.Enabled = false;
      ButtonNuevoPer.Enabled = true;
      ButtonDetallePer.Enabled = false;
    }


    protected void ButtonNuevoPer_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionRCPersona vInspeccionRCPersonas = new InspeccionRCPersona();

      vInspeccionRCPersonas.idInspeccion = int.Parse(TextBoxNroInspeccion.Text);
      vInspeccionRCPersonas.nombrePersona = TextBoxNombresApPersona.Text;
      vInspeccionRCPersonas.docIdentidadPersona = TextBoxDocIdPersona.Text;
      vInspeccionRCPersonas.observacionesPersona = TextBoxObsPersona.Text.ToUpper().Trim();
      vInspeccionRCPersonas.telefonoPersona = TextBoxTelfPersona.Text;
      vInspeccionRCPersonas.estado = 1;

      int vResultado = vAccesodatos.FGrabaInspRCPersonasICRL(vInspeccionRCPersonas);

      if (vResultado > 0)
      {
        int vResul = FlTraeDatosRCPersonas(int.Parse(TextBoxNroInspeccion.Text));
        PLimpiaSeccionRCPersonas();
        PBloqueaPersonaEdicion(false);
      }
    }

    protected void ButtonGrabarPer_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionRCPersona vInspeccionRCPersonas = new InspeccionRCPersona();

      vInspeccionRCPersonas.idInspeccion = int.Parse(TextBoxNroInspeccion.Text);
      vInspeccionRCPersonas.secuencial = int.Parse(TextBoxPersonaIdSecuencial.Text);
      vInspeccionRCPersonas.nombrePersona = TextBoxNombresApPersona.Text;
      vInspeccionRCPersonas.docIdentidadPersona = TextBoxDocIdPersona.Text;
      vInspeccionRCPersonas.observacionesPersona = TextBoxObsPersona.Text.ToUpper().Trim();
      vInspeccionRCPersonas.telefonoPersona = TextBoxTelfPersona.Text;

      int vResultado = vAccesodatos.FActualizaInspRCPersonasICRL(vInspeccionRCPersonas);

      if (vResultado > 0)
      {
        int vResul = FlTraeDatosRCPersonas(int.Parse(TextBoxNroInspeccion.Text));
        PLimpiaSeccionRCPersonas();
        PBloqueaPersonaEdicion(false);
      }
    }

    protected void ButtonBorrarPer_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionRCPersona vInspeccionRCPersonas = new InspeccionRCPersona();

      vInspeccionRCPersonas.idInspeccion = int.Parse(TextBoxNroInspeccion.Text);
      vInspeccionRCPersonas.secuencial = int.Parse(TextBoxPersonaIdSecuencial.Text);

      int vResultado = vAccesodatos.FBorrarInspRCPersonasICRL(vInspeccionRCPersonas);

      if (vResultado > 0)
      {
        int vResul = FlTraeDatosRCPersonas(int.Parse(TextBoxNroInspeccion.Text));
        PLimpiaSeccionRCPersonas();
        PBloqueaPersonaEdicion(false);
      }
    }

    protected void ButtonDetallePer_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      //int vSecuencial = int.Parse(TextBoxPersonaIdSecuencial.Text);
      //PBloqueaPersona(false);
      //PBloqueaPersonaDet(true);
      //PLimpiaSeccionRCPersonaDetalle();
      //FlTraeDatosRCPersonaDetalles(vSecuencial);

      PBloqueaPersona(false);
      PBloqueaPersonaDet(true);
      int vSecuencial = int.Parse(TextBoxPersonaIdSecuencial.Text);
      FlTraeDatosRCPersonaDetalles(vSecuencial);
      Session["PopupRCPerHabilitado"] = 1;
      this.ModalPopupRCPersonas.Show();
    }

    protected void PBloqueaPersona(bool pEstado)
    {
      TextBoxNombresApPersona.Enabled = pEstado;
      TextBoxDocIdPersona.Enabled = pEstado;
      TextBoxObsPersona.Enabled = pEstado;
      TextBoxTelfPersona.Enabled = pEstado;
      ButtonGrabarPer.Enabled = pEstado;
      ButtonBorrarPer.Enabled = pEstado;
      ButtonNuevoPer.Enabled = pEstado;
      ButtonDetallePer.Enabled = pEstado;
      GridViewPersonas.Enabled = pEstado;
    }

    protected void PBloqueaPersonaEdicion(bool pEstado)
    {
      if (pEstado)
      {
        TextBoxNombresApPersona.Enabled = false;
        TextBoxDocIdPersona.Enabled = false;
        TextBoxObsPersona.Enabled = true;
        TextBoxTelfPersona.Enabled = true;
        ButtonDetallePer.Enabled = true;
        ButtonGrabarPer.Enabled = true;
        ButtonBorrarPer.Enabled = true;
        ButtonNuevoPer.Enabled = false;
        GridViewPersonas.Enabled = false;
      }
      else
      {
        TextBoxNombresApPersona.Enabled = true;
        TextBoxDocIdPersona.Enabled = true;
        TextBoxObsPersona.Enabled = true;
        TextBoxTelfPersona.Enabled = true;
        ButtonDetallePer.Enabled = false;
        ButtonGrabarPer.Enabled = false;
        ButtonBorrarPer.Enabled = false;
        ButtonNuevoPer.Enabled = true;
        GridViewPersonas.Enabled = true;
      }
    }

    private int FlTraeDatosRCPersonas(int pIdInspeccion)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vLst = from i in db.Inspeccion
                   join per in db.InspRCPersona on i.idInspeccion equals per.idInspeccion
                   where i.idInspeccion == pIdInspeccion
                   select new
                   {
                     per.secuencial,
                     per.nombrePersona,
                     per.docIdentidadPersona,
                     per.telefonoPersona,
                     per.observacionesPersona,
                     per.estado
                   };
        GridViewPersonas.DataSource = vLst.ToList();
        GridViewPersonas.DataBind();
      }

      return vResultado;
    }

    private int CreaInspRCPersonasFlujo(int pIdFlujo)
    {
      AccesoDatos vAccesoDatos = new AccesoDatos();
      InspeccionICRL vInspeccionICRL = new InspeccionICRL();
      string vCodUsuario = Session["IdUsr"].ToString();
      int vResultado = 0;
      int vIdInspeccion = 0;

      int vIdUsuario = vAccesoDatos.FValidaExisteUsuarioICRL(vCodUsuario);
      vIdInspeccion = vAccesoDatos.FFlujoTieneRCPersonas(pIdFlujo);
      if (0 == vIdInspeccion)
      {
        vInspeccionICRL.idFlujo = pIdFlujo;
        vInspeccionICRL.correlativo = vAccesoDatos.fObtieneContadorInspeccionFlujo(pIdFlujo);
        vInspeccionICRL.idUsuario = vIdUsuario;
        vInspeccionICRL.sucursalAtencion = string.Empty;
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
        vInspeccionICRL.tipoInspeccion = (int)ICRL.BD.AccesoDatos.TipoInspeccion.RCPersonas;
        int vRespuesta = vAccesoDatos.FGrabaInspeccionICRL(vInspeccionICRL);

        if (0 == vRespuesta)
        {
          LabelMensaje.Text = "Error al crear la Inspeccion RC Personas";
        }
        else
        {
          TextBoxNroInspeccion.Text = vRespuesta.ToString();
          vResultado = vRespuesta;
        }
      }
      return vResultado;
    }

    #endregion

    #region Inspeccion RC Persona Detalle

    protected void PBloqueaPersonaDet(bool pEstado)
    {
      TextBoxPerDetTipo.Enabled = pEstado;
      TextBoxPerDetMontoGasto.Enabled = pEstado;
      TextBoxPerDetDescripcion.Enabled = pEstado;
      GridViewPerDetalle.Enabled = pEstado;
      ButtonGrabarPerDet.Enabled = !pEstado;
      ButtonBorrarPerDet.Enabled = !pEstado;
      ButtonNuevoPerDet.Enabled = pEstado;
      //ButtonCierraPerDet.Enabled = pEstado;

    }

    protected void PLimpiaSeccionRCPersonaDetalle()
    {
      TextBoxPerDetTipo.Text = string.Empty;
      TextBoxPerDetMontoGasto.Text = "0";
      TextBoxPerDetDescripcion.Text = string.Empty;

      //ButtonCierraPerDet.Enabled = true;
      PBloqueaPersonaDet(true);
    }

    private int FlTraeDatosRCPersonaDetalles(int pSecuencial)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vLst = from per in db.InspRCPersona
                   join perdet in db.InspRCPersonaDetalle on per.secuencial equals perdet.secuencial
                   where per.secuencial == pSecuencial
                   select new
                   {
                     per.secuencial,
                     perdet.tipo,
                     perdet.montoGasto,
                     perdet.descripcion,
                   };
        GridViewPerDetalle.DataSource = vLst.ToList();
        GridViewPerDetalle.DataBind();
      }

      return vResultado;
    }

    protected void GridViewPerDetalle_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      int vObjIdSecuencial = 0;
      string vTextoTemporal = string.Empty;
      vObjIdSecuencial = int.Parse(GridViewPerDetalle.SelectedRow.Cells[1].Text);

      TextBoxPerDetTipo.Text = GridViewPerDetalle.SelectedRow.Cells[2].Text;
      TextBoxPerDetTipo.Enabled = false;
      TextBoxPerDetMontoGasto.Text = GridViewPerDetalle.SelectedRow.Cells[3].Text;
      vTextoTemporal = string.Empty;
      vTextoTemporal = GridViewPerDetalle.SelectedRow.Cells[4].Text;
      vTextoTemporal = vTextoTemporal.Replace("&#209;", "Ñ");
      vTextoTemporal = vTextoTemporal.Replace("&nbsp;", string.Empty);
      TextBoxPerDetDescripcion.Text = vTextoTemporal;
      ButtonNuevoPerDet.Enabled = false;
      ButtonGrabarPerDet.Enabled = true;
      ButtonBorrarPerDet.Enabled = true;
      //ButtonCierraPerDet.Enabled = true;
    }

    protected void GridViewPerDetalle_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      if (!VerificarPagina(true)) return;
      if (e.Row.RowType == DataControlRowType.DataRow)
      {
        e.Row.Attributes["onmouseover"] = "this.style.backgroundColor='aquamarine';";
        e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='white';";
        //e.Row.ToolTip = "Haz clic en la primera columna para seleccionar la fila.";
      }
    }

    protected void ButtonNuevoPerDet_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionRCPersonaDet vInspeccionRCPersonaDetalle = new InspeccionRCPersonaDet();

      vInspeccionRCPersonaDetalle.secuencial = int.Parse(TextBoxPersonaIdSecuencial.Text);
      vInspeccionRCPersonaDetalle.tipoPerDet = TextBoxPerDetTipo.Text;
      vInspeccionRCPersonaDetalle.montoGastoPerDet = decimal.Parse(TextBoxPerDetMontoGasto.Text);
      vInspeccionRCPersonaDetalle.descripPerDet = TextBoxPerDetDescripcion.Text;

      int vResultado = vAccesodatos.FGrabaInspRCPersonaDetICRL(vInspeccionRCPersonaDetalle);

      if (vResultado > 0)
      {
        int vResul = FlTraeDatosRCPersonaDetalles(int.Parse(TextBoxPersonaIdSecuencial.Text));
        PLimpiaSeccionRCPersonaDetalle();
      }
    }

    protected void ButtonGrabarPerDet_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionRCPersonaDet vInspeccionRCPersonaDetalle = new InspeccionRCPersonaDet();

      vInspeccionRCPersonaDetalle.secuencial = int.Parse(TextBoxPersonaIdSecuencial.Text);
      vInspeccionRCPersonaDetalle.tipoPerDet = TextBoxPerDetTipo.Text;
      vInspeccionRCPersonaDetalle.montoGastoPerDet = decimal.Parse(TextBoxPerDetMontoGasto.Text);
      vInspeccionRCPersonaDetalle.descripPerDet = TextBoxPerDetDescripcion.Text;

      int vResultado = vAccesodatos.FActualizaInspRCPersonaDetICRL(vInspeccionRCPersonaDetalle);

      if (vResultado > 0)
      {
        int vResul = FlTraeDatosRCPersonaDetalles(int.Parse(TextBoxPersonaIdSecuencial.Text));
        PLimpiaSeccionRCPersonaDetalle();
      }
    }

    protected void ButtonBorrarPerDet_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionRCPersonaDet vInspeccionRCPersonaDetalle = new InspeccionRCPersonaDet();

      vInspeccionRCPersonaDetalle.secuencial = int.Parse(TextBoxPersonaIdSecuencial.Text);
      vInspeccionRCPersonaDetalle.tipoPerDet = TextBoxPerDetTipo.Text;
      vInspeccionRCPersonaDetalle.montoGastoPerDet = decimal.Parse(TextBoxPerDetMontoGasto.Text);
      vInspeccionRCPersonaDetalle.descripPerDet = TextBoxPerDetDescripcion.Text;

      int vResultado = vAccesodatos.FBorrarInspRCPersonaDetICRL(vInspeccionRCPersonaDetalle);

      if (vResultado > 0)
      {
        int vResul = FlTraeDatosRCPersonaDetalles(int.Parse(TextBoxPersonaIdSecuencial.Text));
        PLimpiaSeccionRCPersonaDetalle();
      }
    }

    //Este boton cerraba el detalle antes
    //protected void ButtonCierraPerDet_Click(object sender, EventArgs e)
    //{
    //    int vResul = 0;
    //    PBloqueaPersona(true);
    //    PBloqueaPersonaDet(false);
    //    PLimpiaSeccionRCPersonas();
    //    vResul = FlTraeDatosRCPersonas(int.Parse(TextBoxNroInspeccion.Text));
    //}

    protected void ButtonCancelPopRCPer_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      int vResul = 0;
      PBloqueaPersona(true);
      PLimpiaSeccionRCPersonas();
      vResul = FlTraeDatosRCPersonas(int.Parse(TextBoxNroInspeccion.Text));
      Session["PopupRCPerHabilitado"] = 0;
      this.ModalPopupRCPersonas.Hide();
    }

    #endregion

    #region Inspeccion Robo Parcial

    protected void PLimpiaSeccionRoboParcial()
    {
      DropDownListItemRP.SelectedIndex = 0;
      DropDownListItemRP.Enabled = true;
      DropDownListCompraRP.SelectedIndex = 0;
      DropDownListCompraRP.Enabled = true;
      //TextBoxCompraRP.Text = string.Empty;
      CheckBoxInstalacionRP.Checked = false;
      CheckBoxPinturaRP.Checked = false;
      CheckBoxMecanicoRP.Checked = false;
      DropDownListChaperioRP.SelectedIndex = 0;
      DropDownListChaperioRP.Enabled = true;
      //TextBoxChaperio.Text = string.Empty;
      DropDownListRepPreviaRP.SelectedIndex = 0;
      DropDownListRepPreviaRP.Enabled = true;
      //TextBoxRepPrevia.Text = string.Empty;
      //TextBoxChaperioRP.Text = string.Empty;
      //TextBoxRepPreviaRP.Text = string.Empty;
      TextBoxObservacionesRP.Text = string.Empty;
      ButtonGrabarRP.Enabled = false;
      ButtonBorrarRP.Enabled = false;
      ButtonNuevoRP.Enabled = true;
    }

    protected void GridViewRoboParcial_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      DropDownListItemRP.Enabled = false;
      TextBoxIdItemRP.Text = string.Empty;
      string vTextoTemporal = string.Empty;
      TextBoxIdItemRP.Text = GridViewRoboParcial.SelectedRow.Cells[1].Text.Substring(0, 8);
      string vTextoItemRP = GridViewRoboParcial.SelectedRow.Cells[2].Text;

      DropDownListItemRP.ClearSelection();
      DropDownListItemRP.Items.FindByValue(TextBoxIdItemRP.Text).Selected = true;

      string vTextoCompra = string.Empty;
      vTextoCompra = GridViewRoboParcial.SelectedRow.Cells[3].Text.Trim();
      DropDownListCompra.ClearSelection();
      DropDownListCompra.Items.FindByText(vTextoCompra).Selected = true;

      //TextBoxCompraRP.Text = GridViewRoboParcial.SelectedRow.Cells[3].Text;
      CheckBoxInstalacionRP.Checked = (GridViewRoboParcial.SelectedRow.Cells[4].Controls[1] as CheckBox).Checked;
      CheckBoxPinturaRP.Checked = (GridViewRoboParcial.SelectedRow.Cells[5].Controls[1] as CheckBox).Checked;
      CheckBoxMecanicoRP.Checked = (GridViewRoboParcial.SelectedRow.Cells[6].Controls[1] as CheckBox).Checked;

      string vTextoChaperio = string.Empty;
      vTextoChaperio = GridViewRoboParcial.SelectedRow.Cells[7].Text.Trim();
      DropDownListChaperioRP.ClearSelection();
      DropDownListChaperioRP.Items.FindByText(vTextoChaperio).Selected = true;

      string vTextoRepPrevia = string.Empty;
      vTextoRepPrevia = GridViewRoboParcial.SelectedRow.Cells[8].Text.Trim();
      DropDownListRepPreviaRP.ClearSelection();
      DropDownListRepPreviaRP.Items.FindByText(vTextoRepPrevia).Selected = true;

      //TextBoxChaperioRP.Text = GridViewRoboParcial.SelectedRow.Cells[7].Text.Trim();
      //TextBoxRepPreviaRP.Text = GridViewRoboParcial.SelectedRow.Cells[8].Text.Trim();
      vTextoTemporal = string.Empty;
      vTextoTemporal = GridViewRoboParcial.SelectedRow.Cells[9].Text.Trim();
      vTextoTemporal = vTextoTemporal.Replace("&#209;", "Ñ");
      vTextoTemporal = vTextoTemporal.Replace("&nbsp;", string.Empty);
      TextBoxObservacionesRP.Text = vTextoTemporal;
      TextBoxNroItemRP.Text = GridViewRoboParcial.SelectedRow.Cells[10].Text;
      ButtonNuevoRP.Enabled = false;
      ButtonGrabarRP.Enabled = true;
      ButtonBorrarRP.Enabled = true;
    }

    protected void GridViewRoboParcial_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      if (!VerificarPagina(true)) return;
      if (e.Row.RowType == DataControlRowType.DataRow)
      {
        //e.Row.Attributes["onmouseover"] = "this.style.backgroundColor='aquamarine';";
        //e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='white';";
        //e.Row.ToolTip = "Haz clic en la primera columna para seleccionar la fila.";
      }
    }

    protected void ButtonNuevoRP_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionRoboParcial vInspRoboParcial = new InspeccionRoboParcial();

      vInspRoboParcial.idInspeccion = int.Parse(TextBoxNroInspeccion.Text);
      vInspRoboParcial.item = DropDownListItemRP.SelectedValue;
      vInspRoboParcial.compra = DropDownListCompraRP.SelectedItem.Text;
      //vInspRoboParcial.compra = TextBoxCompraRP.Text;
      vInspRoboParcial.instalacion = CheckBoxInstalacionRP.Checked;
      vInspRoboParcial.pintura = CheckBoxPinturaRP.Checked;
      vInspRoboParcial.mecanico = CheckBoxMecanicoRP.Checked;
      vInspRoboParcial.chaperio = DropDownListChaperioRP.SelectedItem.Text;
      vInspRoboParcial.reparacionPrevia = DropDownListRepPreviaRP.SelectedItem.Text;
      //vInspRoboParcial.chaperio = TextBoxChaperioRP.Text;
      //vInspRoboParcial.reparacionPrevia = TextBoxRepPreviaRP.Text;
      vInspRoboParcial.observaciones = TextBoxObservacionesRP.Text.ToUpper().Trim();

      int vResultado = vAccesodatos.FGrabaInspRoboParcialICRL(vInspRoboParcial);

      if (vResultado > 0)
      {
        int vResul = FlTraeDatosRoboParcial(int.Parse(TextBoxNroInspeccion.Text));
        PLimpiaSeccionRoboParcial();
      }
    }

    protected void ButtonGrabarRP_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionRoboParcial vInspRoboParcial = new InspeccionRoboParcial();

      vInspRoboParcial.idInspeccion = int.Parse(TextBoxNroInspeccion.Text);
      vInspRoboParcial.item = TextBoxIdItemRP.Text;
      vInspRoboParcial.compra = DropDownListCompraRP.SelectedItem.Text;
      //vInspRoboParcial.compra = TextBoxCompraRP.Text;
      vInspRoboParcial.instalacion = CheckBoxInstalacionRP.Checked;
      vInspRoboParcial.pintura = CheckBoxPinturaRP.Checked;
      vInspRoboParcial.mecanico = CheckBoxMecanicoRP.Checked;
      vInspRoboParcial.chaperio = DropDownListChaperioRP.SelectedItem.Text;
      vInspRoboParcial.reparacionPrevia = DropDownListRepPreviaRP.SelectedItem.Text;
      //vInspRoboParcial.chaperio = TextBoxChaperioRP.Text;
      //vInspRoboParcial.reparacionPrevia = TextBoxRepPreviaRP.Text;
      vInspRoboParcial.observaciones = TextBoxObservacionesRP.Text.ToUpper().Trim();
      vInspRoboParcial.nro_item = long.Parse(TextBoxNroItemRP.Text);

      int vResultado = vAccesodatos.FActualizaInspRoboParcialICRL(vInspRoboParcial);

      if (vResultado > 0)
      {
        int vResul = FlTraeDatosRoboParcial(int.Parse(TextBoxNroInspeccion.Text));
        PLimpiaSeccionRoboParcial();
      }
    }

    protected void ButtonBorrarRP_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionRoboParcial vInspRoboParcial = new InspeccionRoboParcial();

      vInspRoboParcial.idInspeccion = int.Parse(TextBoxNroInspeccion.Text);
      vInspRoboParcial.item = TextBoxIdItemRP.Text;
      vInspRoboParcial.nro_item = long.Parse(TextBoxNroItemRP.Text);

      int vResultado = vAccesodatos.FBorrarInspRoboParcialICRL(vInspRoboParcial);

      if (vResultado > 0)
      {
        int vResul = FlTraeDatosRoboParcial(int.Parse(TextBoxNroInspeccion.Text));
        PLimpiaSeccionRoboParcial();
      }
    }

    private int FlTraeDatosRoboParcial(int pIdInspeccion)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vLst = from i in db.Inspeccion
                   join irp in db.InspRoboParcial on i.idInspeccion equals irp.idInspeccion
                   join n in db.Nomenclador on irp.idItem equals n.codigo
                   where i.idInspeccion == pIdInspeccion && n.categoriaNomenclador == "Item"
                   select new
                   {
                     irp.idItem,
                     n.descripcion,
                     irp.compra,
                     irp.instalacion,
                     irp.pintura,
                     irp.mecanico,
                     irp.chaperio,
                     irp.reparacionPrevia,
                     irp.observaciones,
                     irp.nro_item
                   };

        GridViewRoboParcial.DataSource = vLst.ToList();
        GridViewRoboParcial.DataBind();

      }
      return vResultado;
    }



    private void ValidaRoboParcialFlujo(int pIdFlujo)
    {
      AccesoDatos vAccesodatos = new AccesoDatos();

      bool vSeleccionado = false;
      int vResul = 0;
      int vIdInspeccion = 0;

      vIdInspeccion = vAccesodatos.FFlujoTieneRoboParcial(pIdFlujo);

      if (vIdInspeccion > 0)
      {
        vSeleccionado = vAccesodatos.FInspeccionTieneRPICRL(vIdInspeccion);
        if (vSeleccionado)
        {
          TabPanelRoboParcial.Enabled = true;
          TabPanelRoboParcial.Visible = true;
          CheckBoxRoboParcial.Checked = vSeleccionado;
          vResul = FlTraeDatosRoboParcial(vIdInspeccion);
          //TabContainerCoberturas.ActiveTabIndex = 0;
        }
      }
    }

    private int FlTraeItemsNomencladorRP()
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vLst = from n in db.Nomenclador
                   where n.categoriaNomenclador == "Item"
                   orderby n.descripcion
                   select new
                   {
                     n.codigo,
                     n.descripcion,
                   };

        DropDownListItemRP.DataValueField = "codigo";
        DropDownListItemRP.DataTextField = "descripcion";
        DropDownListItemRP.DataSource = vLst.ToList();
        DropDownListItemRP.DataBind();

      }
      return vResultado;
    }

    private int FlTraeNomenCompraRP()
    {
      int vResultado = 0;
      string vCategoria = "Compra Repuesto";
      int vOrdenCodigo = 2;
      AccesoDatos vAccesoDatos = new AccesoDatos();

      DropDownListCompraRP.DataValueField = "codigo";
      DropDownListCompraRP.DataTextField = "descripcion";
      DropDownListCompraRP.DataSource = vAccesoDatos.FlTraeNomenGenerico(vCategoria, vOrdenCodigo);
      DropDownListCompraRP.DataBind();

      return vResultado;
    }

    private int FlTraeNomenChaperioRP()
    {
      int vResultado = 0;
      string vCategoria = "Nivel de Daño";
      int vOrdenCodigo = 2;
      AccesoDatos vAccesoDatos = new AccesoDatos();

      DropDownListChaperioRP.DataValueField = "codigo";
      DropDownListChaperioRP.DataTextField = "descripcion";
      DropDownListChaperioRP.DataSource = vAccesoDatos.FlTraeNomenGenerico(vCategoria, vOrdenCodigo);
      DropDownListChaperioRP.DataBind();

      return vResultado;
    }

    private int FlTraeNomenRepPreviaRP()
    {
      int vResultado = 0;
      string vCategoria = "Nivel de Daño";
      int vOrdenCodigo = 2;
      AccesoDatos vAccesoDatos = new AccesoDatos();

      DropDownListRepPreviaRP.DataValueField = "codigo";
      DropDownListRepPreviaRP.DataTextField = "descripcion";
      DropDownListRepPreviaRP.DataSource = vAccesoDatos.FlTraeNomenGenerico(vCategoria, vOrdenCodigo);
      DropDownListRepPreviaRP.DataBind();

      return vResultado;
    }

    private int CreaInspRoboParcialFlujo(int pIdFlujo)
    {
      AccesoDatos vAccesoDatos = new AccesoDatos();
      InspeccionICRL vInspeccionICRL = new InspeccionICRL();
      string vCodUsuario = Session["IdUsr"].ToString();
      int vResultado = 0;
      int vIdInspeccion = 0;

      int vIdUsuario = vAccesoDatos.FValidaExisteUsuarioICRL(vCodUsuario);
      vIdInspeccion = vAccesoDatos.FFlujoTieneRoboParcial(pIdFlujo);
      if (0 == vIdInspeccion)
      {
        vInspeccionICRL.idFlujo = pIdFlujo;
        vInspeccionICRL.correlativo = vAccesoDatos.fObtieneContadorInspeccionFlujo(pIdFlujo);
        vInspeccionICRL.idUsuario = vIdUsuario;
        vInspeccionICRL.sucursalAtencion = string.Empty;
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
        vInspeccionICRL.tipoInspeccion = (int)ICRL.BD.AccesoDatos.TipoInspeccion.RoboParcial;
        int vRespuesta = vAccesoDatos.FGrabaInspeccionICRL(vInspeccionICRL);

        if (0 == vRespuesta)
        {
          LabelMensaje.Text = "Error al crear la Inspeccion Robo Parcial";
        }
        else
        {
          TextBoxNroInspeccion.Text = vRespuesta.ToString();
          vResultado = vRespuesta;
        }
      }
      return vResultado;
    }

    #endregion

    #region Inspeccion Perdida Total Danios Propios

    private int FlTraeItemsNomencladorCajaPTDP()
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vLst = from n in db.Nomenclador
                   where n.categoriaNomenclador == "Tipo de Caja"
                   orderby n.descripcion
                   select new
                   {
                     n.codigo,
                     n.descripcion,
                   };

        DropDownListCajaPTDP.DataValueField = "codigo";
        DropDownListCajaPTDP.DataTextField = "descripcion";
        DropDownListCajaPTDP.DataSource = vLst.ToList();
        DropDownListCajaPTDP.DataBind();

      }
      return vResultado;
    }

    private int FlTraeItemsNomencladorCombustiblePTDP()
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vLst = from n in db.Nomenclador
                   where n.categoriaNomenclador == "Combustible"
                   orderby n.descripcion
                   select new
                   {
                     n.codigo,
                     n.descripcion,
                   };

        DropDownListCombustiblePTDP.DataValueField = "codigo";
        DropDownListCombustiblePTDP.DataTextField = "descripcion";
        DropDownListCombustiblePTDP.DataSource = vLst.ToList();
        DropDownListCombustiblePTDP.DataBind();

      }
      return vResultado;
    }




    private void ValidaPerdidaTotalDPFlujo(int pIdFlujo)
    {
      AccesoDatos vAccesodatos = new AccesoDatos();

      bool vSeleccionado = false;
      int vResul = 0;
      int vIdInspeccion = 0;

      vIdInspeccion = vAccesodatos.FFlujoTienePerdidaTotDaniosPropios(pIdFlujo);

      if (vIdInspeccion > 0)
      {
        vSeleccionado = vAccesodatos.FInspeccionTienePTDPICRL(vIdInspeccion);
        if (vSeleccionado)
        {
          TabPanelPerdidaTotalDaniosPropios.Enabled = true;
          TabPanelPerdidaTotalDaniosPropios.Visible = true;
          CheckBoxPerdidaTotDanios.Checked = vSeleccionado;
          vResul = FlTraeDatosPerdidaTotalDP(vIdInspeccion);
          //TabContainerCoberturas.ActiveTabIndex = 0;
        }
      }
    }

    private int FlTraeDatosPerdidaTotalDP(int pIdInspeccion)
    {
      int vResultado = 0;
      string vTextoTemporal = string.Empty;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vTablaInspPerdidatotalDP = from ptDP in db.InspPerdidaTotalDanios
                                       where ptDP.idInspeccion == pIdInspeccion
                                       select ptDP;

        var vFilaTablaInspPerdidaTotDP = vTablaInspPerdidatotalDP.FirstOrDefault<InspPerdidaTotalDanios>();

        if (null != vFilaTablaInspPerdidaTotDP)
        {
          TextBoxVersionPTDP.Text = vFilaTablaInspPerdidaTotDP.version;
          TextBoxSeriePTDP.Text = vFilaTablaInspPerdidaTotDP.serie;
          TextBoxCilindradaPTDP.Text = vFilaTablaInspPerdidaTotDP.cilindrada.ToString();
          CheckBoxTechoSolarPTDP.Checked = (bool)vFilaTablaInspPerdidaTotDP.techoSolar;
          CheckBoxAsientosCueroPTDP.Checked = (bool)vFilaTablaInspPerdidaTotDP.asientosCuero;
          CheckBoxArosMagnesioPTDP.Checked = (bool)vFilaTablaInspPerdidaTotDP.arosMagnesio;
          CheckBoxConvertidoGnvPTDP.Checked = (bool)vFilaTablaInspPerdidaTotDP.convertidoGNV;
          vTextoTemporal = string.Empty;
          vTextoTemporal = vFilaTablaInspPerdidaTotDP.observaciones;
          vTextoTemporal = vTextoTemporal.Replace("&#209;", "Ñ");
          vTextoTemporal = vTextoTemporal.Replace("&nbsp;", string.Empty);
          TextBoxObservacionesPTDP.Text = vTextoTemporal;

          string vTempo = string.Empty;
          vTempo = vFilaTablaInspPerdidaTotDP.caja;

          DropDownListCajaPTDP.ClearSelection();
          DropDownListCajaPTDP.Items.FindByText(vTempo).Selected = true;

          vTempo = string.Empty;
          vTempo = vFilaTablaInspPerdidaTotDP.combustible;

          DropDownListCombustiblePTDP.ClearSelection();
          DropDownListCombustiblePTDP.Items.FindByText(vTempo).Selected = true;
          PBloqueoPerdidaTotalPTDP(false);
        }
        else
        {
          PLimpiaSeccionPerdidaTotalPTDP();
          PBloqueoPerdidaTotalPTDP(true);
        }

      }
      return vResultado;
    }

    protected void ButtonNuevoPTDP_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionPTDaniosPropios vInspPerdidaTotalPTDP = new InspeccionPTDaniosPropios();

      vInspPerdidaTotalPTDP.idInspeccion = int.Parse(TextBoxNroInspeccion.Text);
      vInspPerdidaTotalPTDP.version = TextBoxVersionPTDP.Text;
      vInspPerdidaTotalPTDP.serie = TextBoxSeriePTDP.Text;
      vInspPerdidaTotalPTDP.caja = DropDownListCajaPTDP.SelectedItem.Text;
      vInspPerdidaTotalPTDP.combustible = DropDownListCombustiblePTDP.SelectedItem.Text;
      vInspPerdidaTotalPTDP.cilindrada = int.Parse(TextBoxCilindradaPTDP.Text);
      vInspPerdidaTotalPTDP.techoSolar = CheckBoxTechoSolarPTDP.Checked;
      vInspPerdidaTotalPTDP.asientosCuero = CheckBoxAsientosCueroPTDP.Checked;
      vInspPerdidaTotalPTDP.arosMagnesio = CheckBoxArosMagnesioPTDP.Checked;
      vInspPerdidaTotalPTDP.convertidoGNV = CheckBoxConvertidoGnvPTDP.Checked;
      vInspPerdidaTotalPTDP.observaciones = TextBoxObservacionesPTDP.Text.ToUpper().Trim();

      int vResultado = vAccesodatos.FGrabaInspPerdidaTotalPTDPICRL(vInspPerdidaTotalPTDP);

      if (vResultado > 0)
      {
        int vResul = FlTraeDatosPerdidaTotalDP(int.Parse(TextBoxNroInspeccion.Text));
        PBloqueoPerdidaTotalPTDP(false);
      }
    }

    protected void ButtonGrabarPTDP_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionPTDaniosPropios vInspPerdidaTotalPTDP = new InspeccionPTDaniosPropios();

      vInspPerdidaTotalPTDP.idInspeccion = int.Parse(TextBoxNroInspeccion.Text);
      vInspPerdidaTotalPTDP.version = TextBoxVersionPTDP.Text;
      vInspPerdidaTotalPTDP.serie = TextBoxSeriePTDP.Text;
      vInspPerdidaTotalPTDP.caja = DropDownListCajaPTDP.SelectedItem.Text;
      vInspPerdidaTotalPTDP.combustible = DropDownListCombustiblePTDP.SelectedItem.Text;
      vInspPerdidaTotalPTDP.cilindrada = int.Parse(TextBoxCilindradaPTDP.Text);
      vInspPerdidaTotalPTDP.techoSolar = CheckBoxTechoSolarPTDP.Checked;
      vInspPerdidaTotalPTDP.asientosCuero = CheckBoxAsientosCueroPTDP.Checked;
      vInspPerdidaTotalPTDP.arosMagnesio = CheckBoxArosMagnesioPTDP.Checked;
      vInspPerdidaTotalPTDP.convertidoGNV = CheckBoxConvertidoGnvPTDP.Checked;
      vInspPerdidaTotalPTDP.observaciones = TextBoxObservacionesPTDP.Text.ToUpper().Trim();

      int vResultado = vAccesodatos.FActualizaInspPerdidaTotalPTDPICRL(vInspPerdidaTotalPTDP);

      if (vResultado > 0)
      {
        int vResul = FlTraeDatosPerdidaTotalDP(int.Parse(TextBoxNroInspeccion.Text));
        PBloqueoPerdidaTotalPTDP(false);
      }
    }

    protected void ButtonBorrarPTDP_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionPTDaniosPropios vInspPerdidaTotalPTDP = new InspeccionPTDaniosPropios();

      vInspPerdidaTotalPTDP.idInspeccion = int.Parse(TextBoxNroInspeccion.Text);

      int vResultado = vAccesodatos.FBorrarInspPerdidaTotalPTDPICRL(vInspPerdidaTotalPTDP);

      if (vResultado > 0)
      {
        int vResul = FlTraeDatosPerdidaTotalDP(int.Parse(TextBoxNroInspeccion.Text));
        PBloqueoPerdidaTotalPTDP(true);
        PLimpiaSeccionPerdidaTotalPTDP();
      }
    }

    protected void PBloqueoPerdidaTotalPTDP(bool pEstado)
    {
      ButtonNuevoPTDP.Enabled = pEstado;
      ButtonGrabarPTDP.Enabled = !pEstado;
      ButtonBorrarPTDP.Enabled = !pEstado;
    }
    protected void PLimpiaSeccionPerdidaTotalPTDP()
    {
      TextBoxVersionPTDP.Text = string.Empty;
      TextBoxSeriePTDP.Text = string.Empty;
      TextBoxCilindradaPTDP.Text = "0";
      CheckBoxTechoSolarPTDP.Checked = false;
      CheckBoxAsientosCueroPTDP.Checked = false;
      CheckBoxArosMagnesioPTDP.Checked = false;
      CheckBoxConvertidoGnvPTDP.Checked = false;
      TextBoxObservacionesPTDP.Text = string.Empty;
      DropDownListCajaPTDP.SelectedIndex = 0;
      DropDownListCombustiblePTDP.SelectedIndex = 0;
    }

    private int CreaInspPTDaniosPropiosFlujo(int pIdFlujo)
    {
      AccesoDatos vAccesoDatos = new AccesoDatos();
      InspeccionICRL vInspeccionICRL = new InspeccionICRL();
      string vCodUsuario = Session["IdUsr"].ToString();
      int vResultado = 0;
      int vIdInspeccion = 0;

      int vIdUsuario = vAccesoDatos.FValidaExisteUsuarioICRL(vCodUsuario);
      vIdInspeccion = vAccesoDatos.FFlujoTienePerdidaTotDaniosPropios(pIdFlujo);
      if (0 == vIdInspeccion)
      {
        vInspeccionICRL.idFlujo = pIdFlujo;
        vInspeccionICRL.correlativo = vAccesoDatos.fObtieneContadorInspeccionFlujo(pIdFlujo);
        vInspeccionICRL.idUsuario = vIdUsuario;
        vInspeccionICRL.sucursalAtencion = string.Empty;
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
        vInspeccionICRL.tipoInspeccion = (int)ICRL.BD.AccesoDatos.TipoInspeccion.PerdidaTotalDaniosPropios;
        int vRespuesta = vAccesoDatos.FGrabaInspeccionICRL(vInspeccionICRL);

        if (0 == vRespuesta)
        {
          LabelMensaje.Text = "Error al crear la Inspeccion Perdida Total Danios Propios";
        }
        else
        {
          TextBoxNroInspeccion.Text = vRespuesta.ToString();
          vResultado = vRespuesta;
        }
      }
      return vResultado;
    }

    #endregion

    #region Inspeccion Perdida Total por Robo

    private int FlTraeItemsNomencladorCajaPTRO()
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vLst = from n in db.Nomenclador
                   where n.categoriaNomenclador == "Tipo de Caja"
                   orderby n.descripcion
                   select new
                   {
                     n.codigo,
                     n.descripcion,
                   };

        DropDownListCajaPTRO.DataValueField = "codigo";
        DropDownListCajaPTRO.DataTextField = "descripcion";
        DropDownListCajaPTRO.DataSource = vLst.ToList();
        DropDownListCajaPTRO.DataBind();

      }
      return vResultado;
    }

    private int FlTraeItemsNomencladorCombustiblePTRO()
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vLst = from n in db.Nomenclador
                   where n.categoriaNomenclador == "Combustible"
                   orderby n.descripcion
                   select new
                   {
                     n.codigo,
                     n.descripcion,
                   };

        DropDownListCombustiblePTRO.DataValueField = "codigo";
        DropDownListCombustiblePTRO.DataTextField = "descripcion";
        DropDownListCombustiblePTRO.DataSource = vLst.ToList();
        DropDownListCombustiblePTRO.DataBind();

      }
      return vResultado;
    }




    private void ValidaPerdidaTotalROFlujo(int pIdFlujo)
    {
      AccesoDatos vAccesodatos = new AccesoDatos();

      bool vSeleccionado = false;
      int vResul = 0;
      int vIdInspeccion = 0;

      vIdInspeccion = vAccesodatos.FFlujoTienePerdidaTotRobo(pIdFlujo);

      if (vIdInspeccion > 0)
      {
        vSeleccionado = vAccesodatos.FInspeccionTienePTROICRL(vIdInspeccion);
        if (vSeleccionado)
        {
          TabPanelPerdidaTotalRobo.Enabled = true;
          TabPanelPerdidaTotalRobo.Visible = true;
          CheckBoxPerdidaTotRobo.Checked = vSeleccionado;
          vResul = FlTraeDatosPerdidaTotalRO(vIdInspeccion);
          //TabContainerCoberturas.ActiveTabIndex = 0;
        }
      }
    }

    private int FlTraeDatosPerdidaTotalRO(int pIdInspeccion)
    {
      int vResultado = 0;
      string vTextoTemporal = string.Empty;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vTablaInspPerdidatotalRO = from ptDP in db.InspPerdidaTotalRobo
                                       where ptDP.idInspeccion == pIdInspeccion
                                       select ptDP;

        var vFilaTablaInspPerdidaTotRO = vTablaInspPerdidatotalRO.FirstOrDefault<InspPerdidaTotalRobo>();

        if (null != vFilaTablaInspPerdidaTotRO)
        {
          TextBoxVersionPTRO.Text = vFilaTablaInspPerdidaTotRO.version;
          TextBoxSeriePTRO.Text = vFilaTablaInspPerdidaTotRO.serie;
          TextBoxCilindradaPTRO.Text = vFilaTablaInspPerdidaTotRO.cilindrada.ToString();
          CheckBoxTechoSolarPTRO.Checked = (bool)vFilaTablaInspPerdidaTotRO.techoSolar;
          CheckBoxAsientosCueroPTRO.Checked = (bool)vFilaTablaInspPerdidaTotRO.asientosCuero;
          CheckBoxArosMagnesioPTRO.Checked = (bool)vFilaTablaInspPerdidaTotRO.arosMagnesio;
          CheckBoxConvertidoGnvPTRO.Checked = (bool)vFilaTablaInspPerdidaTotRO.convertidoGNV;
          vTextoTemporal = string.Empty;
          vTextoTemporal = vFilaTablaInspPerdidaTotRO.observaciones;
          vTextoTemporal = vTextoTemporal.Replace("&#209;", "Ñ");
          vTextoTemporal = vTextoTemporal.Replace("&nbsp;", string.Empty);
          TextBoxObservacionesPTRO.Text = vTextoTemporal;

          string vTempo = string.Empty;
          vTempo = vFilaTablaInspPerdidaTotRO.caja;

          DropDownListCajaPTRO.ClearSelection();
          DropDownListCajaPTRO.Items.FindByText(vTempo).Selected = true;

          vTempo = string.Empty;
          vTempo = vFilaTablaInspPerdidaTotRO.combustible;

          DropDownListCombustiblePTRO.ClearSelection();
          DropDownListCombustiblePTRO.Items.FindByText(vTempo).Selected = true;
          PBloqueoPerdidaTotalPTRO(false);
        }
        else
        {
          PLimpiaSeccionPerdidaTotalPTRO();
          PBloqueoPerdidaTotalPTRO(true);
        }

      }
      return vResultado;
    }

    protected void ButtonNuevoPTRO_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionPTRobo vInspPerdidaTotalPTRO = new InspeccionPTRobo();

      vInspPerdidaTotalPTRO.idInspeccion = int.Parse(TextBoxNroInspeccion.Text);
      vInspPerdidaTotalPTRO.version = TextBoxVersionPTRO.Text;
      vInspPerdidaTotalPTRO.serie = TextBoxSeriePTRO.Text;
      vInspPerdidaTotalPTRO.caja = DropDownListCajaPTRO.SelectedItem.Text;
      vInspPerdidaTotalPTRO.combustible = DropDownListCombustiblePTRO.SelectedItem.Text;
      vInspPerdidaTotalPTRO.cilindrada = int.Parse(TextBoxCilindradaPTRO.Text);
      vInspPerdidaTotalPTRO.techoSolar = CheckBoxTechoSolarPTRO.Checked;
      vInspPerdidaTotalPTRO.asientosCuero = CheckBoxAsientosCueroPTRO.Checked;
      vInspPerdidaTotalPTRO.arosMagnesio = CheckBoxArosMagnesioPTRO.Checked;
      vInspPerdidaTotalPTRO.convertidoGNV = CheckBoxConvertidoGnvPTRO.Checked;
      vInspPerdidaTotalPTRO.observaciones = TextBoxObservacionesPTRO.Text.ToUpper().Trim();

      int vResultado = vAccesodatos.FGrabaInspPerdidaTotalPTROICRL(vInspPerdidaTotalPTRO);

      if (vResultado > 0)
      {
        int vResul = FlTraeDatosPerdidaTotalRO(int.Parse(TextBoxNroInspeccion.Text));
        PBloqueoPerdidaTotalPTRO(false);
      }
    }

    protected void ButtonGrabarPTRO_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionPTRobo vInspPerdidaTotalPTRO = new InspeccionPTRobo();

      vInspPerdidaTotalPTRO.idInspeccion = int.Parse(TextBoxNroInspeccion.Text);
      vInspPerdidaTotalPTRO.version = TextBoxVersionPTRO.Text;
      vInspPerdidaTotalPTRO.serie = TextBoxSeriePTRO.Text;
      vInspPerdidaTotalPTRO.caja = DropDownListCajaPTRO.SelectedItem.Text;
      vInspPerdidaTotalPTRO.combustible = DropDownListCombustiblePTRO.SelectedItem.Text;
      vInspPerdidaTotalPTRO.cilindrada = int.Parse(TextBoxCilindradaPTRO.Text);
      vInspPerdidaTotalPTRO.techoSolar = CheckBoxTechoSolarPTRO.Checked;
      vInspPerdidaTotalPTRO.asientosCuero = CheckBoxAsientosCueroPTRO.Checked;
      vInspPerdidaTotalPTRO.arosMagnesio = CheckBoxArosMagnesioPTRO.Checked;
      vInspPerdidaTotalPTRO.convertidoGNV = CheckBoxConvertidoGnvPTRO.Checked;
      vInspPerdidaTotalPTRO.observaciones = TextBoxObservacionesPTRO.Text.ToUpper().Trim();

      int vResultado = vAccesodatos.FActualizaInspPerdidaTotalPTROICRL(vInspPerdidaTotalPTRO);

      if (vResultado > 0)
      {
        int vResul = FlTraeDatosPerdidaTotalRO(int.Parse(TextBoxNroInspeccion.Text));
        PBloqueoPerdidaTotalPTRO(false);
        PLimpiaSeccionPerdidaTotalPTRO();
      }
    }

    protected void ButtonBorrarPTRO_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionPTRobo vInspPerdidaTotalPTRO = new InspeccionPTRobo();

      vInspPerdidaTotalPTRO.idInspeccion = int.Parse(TextBoxNroInspeccion.Text);

      int vResultado = vAccesodatos.FBorrarInspPerdidaTotalPTROICRL(vInspPerdidaTotalPTRO);

      if (vResultado > 0)
      {
        int vResul = FlTraeDatosPerdidaTotalRO(int.Parse(TextBoxNroInspeccion.Text));
        PBloqueoPerdidaTotalPTRO(true);
        PLimpiaSeccionPerdidaTotalPTRO();
      }
    }

    protected void PBloqueoPerdidaTotalPTRO(bool pEstado)
    {
      ButtonNuevoPTRO.Enabled = pEstado;
      ButtonGrabarPTRO.Enabled = !pEstado;
      ButtonBorrarPTRO.Enabled = !pEstado;
    }

    protected void PLimpiaSeccionPerdidaTotalPTRO()
    {
      TextBoxVersionPTRO.Text = string.Empty;
      TextBoxSeriePTRO.Text = string.Empty;
      TextBoxCilindradaPTRO.Text = string.Empty;
      CheckBoxTechoSolarPTRO.Checked = false;
      CheckBoxAsientosCueroPTRO.Checked = false;
      CheckBoxArosMagnesioPTRO.Checked = false;
      CheckBoxConvertidoGnvPTRO.Checked = false;
      TextBoxObservacionesPTRO.Text = string.Empty;
      DropDownListCajaPTRO.SelectedIndex = 0;
      DropDownListCombustiblePTRO.SelectedIndex = 0;
    }

    private int CreaInspPTRoboFlujo(int pIdFlujo)
    {
      AccesoDatos vAccesoDatos = new AccesoDatos();
      InspeccionICRL vInspeccionICRL = new InspeccionICRL();
      string vCodUsuario = Session["IdUsr"].ToString();
      int vResultado = 0;
      int vIdInspeccion = 0;

      int vIdUsuario = vAccesoDatos.FValidaExisteUsuarioICRL(vCodUsuario);
      vIdInspeccion = vAccesoDatos.FFlujoTienePerdidaTotRobo(pIdFlujo);
      if (0 == vIdInspeccion)
      {
        vInspeccionICRL.idFlujo = pIdFlujo;
        vInspeccionICRL.correlativo = vAccesoDatos.fObtieneContadorInspeccionFlujo(pIdFlujo);
        vInspeccionICRL.idUsuario = vIdUsuario;
        vInspeccionICRL.sucursalAtencion = string.Empty;
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
        vInspeccionICRL.tipoInspeccion = (int)ICRL.BD.AccesoDatos.TipoInspeccion.PerdidaTotalRobo;
        int vRespuesta = vAccesoDatos.FGrabaInspeccionICRL(vInspeccionICRL);

        if (0 == vRespuesta)
        {
          LabelMensaje.Text = "Error al crear la Inspeccion Perdida Total por Robo";
        }
        else
        {
          TextBoxNroInspeccion.Text = vRespuesta.ToString();
          vResultado = vRespuesta;
        }
      }
      return vResultado;
    }

    #endregion

    #region RC Vehicular 01



    private void ValidaRCVehicular01Flujo(int pIdFlujo)
    {
      AccesoDatos vAccesodatos = new AccesoDatos();

      bool vSeleccionado = false;
      int vResul = 0;
      int vIdInspeccion = 0;

      vIdInspeccion = vAccesodatos.FFlujoTieneRCVehicular(pIdFlujo);

      if (vIdInspeccion > 0)
      {
        vSeleccionado = vAccesodatos.FInspeccionTieneRCVehicularICRL(vIdInspeccion);
        if (vSeleccionado)
        {
          TabPanelRCV01.Enabled = true;
          TabPanelRCV01.Visible = true;
          CheckBoxRCVehicular01.Checked = vSeleccionado;
          vResul = FlTraeDatosRCV01(vIdInspeccion);
          //TabContainerCoberturas.ActiveTabIndex = 0;
          PBloqueoRCVehicular01(true);
        }
      }
    }

    protected void PBloqueoRCVehicular01(bool pEstado)
    {
      ButtonMNuevoRCV01.Enabled = pEstado;
      ButtonMGrabarRCV01.Enabled = !pEstado;
      ButtonMBorrarRCV01.Enabled = !pEstado;
      ButtonMDetalleRCV01.Enabled = !pEstado;
    }

    protected void PLimpiaSeccionRCVehicular01()
    {
      TextBoxNombreTerceroRCV01.Text = string.Empty;
      TextBoxDocIdTerceroRCV01.Text = string.Empty;
      TextBoxTelfTerceroRCV01.Text = string.Empty;
      DropDownListMarcaRCV01.SelectedIndex = 0;
      TextBoxModeloRCV01.Text = string.Empty;
      TextBoxAnioRCV01.Text = "1980";
      TextBoxPlacaRCV01.Text = string.Empty;
      DropDownListColorRCV01.SelectedIndex = 0;
      TextBoxNroChasisRCV01.Text = string.Empty;
      TextBoxKilometrajeRCV01.Text = "0";
    }

    protected void GridViewRCV01_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionRCVehicular vInspeccionRCVehicular01 = new InspeccionRCVehicular();
      int vSecuencial = 0;
      int vIdInspeccion = 0;

      vSecuencial = int.Parse(GridViewRCV01.SelectedRow.Cells[3].Text);
      vIdInspeccion = int.Parse(TextBoxNroInspeccion.Text);
      vInspeccionRCVehicular01 = vAccesodatos.FTraeInspRCVehicularICRL(vSecuencial, vIdInspeccion);

      //string vTextoItemDP = GridViewRCV01.SelectedRow.Cells[2].Text;
      if (null != vInspeccionRCVehicular01)
      {
        #region revisar
        //TextBoxSecuencialRCV01.Text = GridViewRCV01.SelectedRow.Cells[2].Text;
        //TextBoxNombreTerceroRCV01.Text = GridViewRCV01.SelectedRow.Cells[3].Text;
        //string vTextoMarca = string.Empty;
        //vTextoMarca = GridViewRCV01.SelectedRow.Cells[4].Text.Trim();
        //DropDownListMarcaRCV01.ClearSelection();
        //DropDownListMarcaRCV01.Items.FindByText(vTextoMarca).Selected = true;
        //TextBoxModeloRCV01.Text = GridViewRCV01.SelectedRow.Cells[5].Text;
        //string vTextoColor = string.Empty;
        //vTextoColor = GridViewRCV01.SelectedRow.Cells[6].Text.Trim();
        //DropDownListColorRCV01.ClearSelection();
        //DropDownListColorRCV01.Items.FindByText(vTextoColor).Selected = true;
        //TextBoxAnioRCV01.Text = GridViewRCV01.SelectedRow.Cells[7].Text;
        //TextBoxNroChasisRCV01.Text = GridViewRCV01.SelectedRow.Cells[8].Text;
        #endregion

        TextBoxSecuencialRCV01.Text = vInspeccionRCVehicular01.secuencial.ToString();
        TextBoxNombreTerceroRCV01.Text = vInspeccionRCVehicular01.nombreTercero;
        TextBoxDocIdTerceroRCV01.Text = vInspeccionRCVehicular01.docIdentidadTercero;
        TextBoxTelfTerceroRCV01.Text = vInspeccionRCVehicular01.telefonoTercero;
        string vTextoMarca = string.Empty;
        vTextoMarca = vInspeccionRCVehicular01.marca.Trim();
        DropDownListMarcaRCV01.ClearSelection();
        DropDownListMarcaRCV01.Items.FindByText(vTextoMarca).Selected = true;
        TextBoxModeloRCV01.Text = vInspeccionRCVehicular01.modelo;
        TextBoxAnioRCV01.Text = vInspeccionRCVehicular01.anio.ToString();
        string vTextoColor = string.Empty;
        vTextoColor = vInspeccionRCVehicular01.color.Trim();
        DropDownListColorRCV01.ClearSelection();
        DropDownListColorRCV01.Items.FindByText(vTextoColor).Selected = true;
        TextBoxPlacaRCV01.Text = vInspeccionRCVehicular01.placa;
        TextBoxNroChasisRCV01.Text = vInspeccionRCVehicular01.chasis;
        TextBoxKilometrajeRCV01.Text = vInspeccionRCVehicular01.kilometraje.ToString();

        string vTempoCadena = string.Empty;
        vTempoCadena = vInspeccionRCVehicular01.tipoTaller.Trim();
        DropDownListTipoTallerRCVeh.ClearSelection();
        DropDownListTipoTallerRCVeh.Items.FindByText(vTempoCadena).Selected = true;

        //FlTraeDatosRCVehicularDet(vSecuencial);

      }
      PBloqueoRCVehicular01(false);
    }

    protected void GridViewRCV01_RowCommand(object sender, GridViewCommandEventArgs e)
    {
      if (!VerificarPagina(true)) return;
      if (0 == e.CommandName.CompareTo("ImprimirFormularioInsp"))
      {
        string vTextoSecuencial = string.Empty;
        int vIndex = 0;
        int vSecuencial = 0;

        vIndex = Convert.ToInt32(e.CommandArgument);
        vSecuencial = Convert.ToInt32(GridViewRCV01.DataKeys[vIndex].Value);
        PImprimeFormularioInspRCVehicular(vSecuencial);
      }

      if (0 == e.CommandName.CompareTo("FinalizarInsp"))
      {
        string vTextoSecuencial = string.Empty;
        int vIndex = 0;
        int vSecuencial = 0;

        vIndex = Convert.ToInt32(e.CommandArgument);
        vSecuencial = Convert.ToInt32(GridViewRCV01.DataKeys[vIndex].Value);
        //proceso que copia los datos de Inps a Coti
        AccesoDatos vAccesoDatos = new AccesoDatos();
        int vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
        int vidInspecccion = int.Parse(TextBoxNroInspeccion.Text);
        vAccesoDatos.fCopiaRCVehicularInspACotizacion(vIdFlujo, vidInspecccion, vSecuencial);
        //cambiar estado de la cobertura para que no se pueda volver a ejecutar
        BD.InspeccionRCVehicular vInspeccionRCVehicular = new InspeccionRCVehicular();
        vInspeccionRCVehicular.idInspeccion = vidInspecccion;
        vInspeccionRCVehicular.secuencial = vSecuencial;
        int vResultado = 0;
        vResultado = vAccesoDatos.FRCVehicularCambiaEstado(vInspeccionRCVehicular);
      }
      int vResul = 0;
      vResul = FlTraeDatosRCV01(int.Parse(TextBoxNroInspeccion.Text));
    }

    protected void GridViewRCV01_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      if (!VerificarPagina(true)) return;
      if (e.Row.RowType == DataControlRowType.DataRow)
      {
        //e.Row.Attributes["onmouseover"] = "this.style.backgroundColor='aquamarine';";
        //e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='white';";

        //verificar el estado del registro
        string vEstadoCadena = string.Empty;
        int vEstado = 0;
        vEstadoCadena = e.Row.Cells[10].Text;
        vEstado = int.Parse(vEstadoCadena);
        if (1 == vEstado)
        {
          (e.Row.Cells[11].Controls[0] as LinkButton).Enabled = true;
          //ConfirmarFinalizarInspeccion
          (e.Row.Cells[11].Controls[0] as LinkButton).Attributes.Add("OnClick", "javascript:return ConfirmarFinalizarInspeccion()");
        }
        else
        {
          (e.Row.Cells[11].Controls[0] as LinkButton).Enabled = false;
        }

        //generamos la consulta para cada fila de la grilla maestra persona
        string vTextoSecuencial = string.Empty;
        int vSecuencial = 0;

        vTextoSecuencial = e.Row.Cells[3].Text;
        vSecuencial = int.Parse(vTextoSecuencial);

        AccesoDatos vAccesoDatos = new AccesoDatos();
        var gvRCVDet = (GridView)e.Row.FindControl("gvRCVDet");

        using (LBCDesaEntities db = new LBCDesaEntities())
        {
          var vLst = from ircv in db.InspRCVehicular
                     join ircvdet in db.InspRCVehicularDetalle on ircv.secuencial equals ircvdet.secuencial
                     join n in db.Nomenclador on ircvdet.idItem equals n.codigo
                     where ircv.secuencial == vSecuencial && n.categoriaNomenclador == "Item"
                     orderby n.descripcion
                     select new
                     {
                       ircvdet.idItem,
                       n.descripcion,
                       ircvdet.compra,
                       ircvdet.instalacion,
                       ircvdet.pintura,
                       ircvdet.mecanico,
                       ircvdet.chaperio,
                       ircvdet.reparacionPrevia,
                       ircvdet.observaciones
                     };


          gvRCVDet.DataSource = vLst.ToList();
          gvRCVDet.DataBind();
        }
      }
    }

    protected void ButtonMNuevoRCV01_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionRCVehicular vInspRCVehicular01 = new InspeccionRCVehicular();

      vInspRCVehicular01.idInspeccion = int.Parse(TextBoxNroInspeccion.Text);
      vInspRCVehicular01.nombreTercero = TextBoxNombreTerceroRCV01.Text;
      vInspRCVehicular01.docIdentidadTercero = TextBoxDocIdTerceroRCV01.Text;
      vInspRCVehicular01.telefonoTercero = TextBoxTelfTerceroRCV01.Text;
      vInspRCVehicular01.marca = DropDownListMarcaRCV01.SelectedItem.Text;
      vInspRCVehicular01.modelo = TextBoxModeloRCV01.Text;
      vInspRCVehicular01.estado = 1;
      int vTempo = 0;
      vInspRCVehicular01.anio = vTempo;
      if (int.TryParse(TextBoxAnioRCV01.Text, out vTempo))
      {
        vInspRCVehicular01.anio = vTempo;
      }
      //vInspRCVehicular01.anio = TextBoxAnioRCV01.Text;

      vInspRCVehicular01.placa = TextBoxPlacaRCV01.Text;
      vInspRCVehicular01.color = DropDownListColorRCV01.SelectedItem.Text;
      vInspRCVehicular01.chasis = TextBoxNroChasisRCV01.Text;
      vTempo = 0;
      vInspRCVehicular01.kilometraje = vTempo;
      if (int.TryParse(TextBoxKilometrajeRCV01.Text, out vTempo))
      {
        vInspRCVehicular01.kilometraje = vTempo;
      }
      //vInspRCVehicular01.kilometraje = TextBoxKilometrajeRCV01.Text;

      vInspRCVehicular01.importacionDirecta = false;

      vInspRCVehicular01.tipoTaller = DropDownListTipoTallerRCVeh.SelectedItem.Text;

      int vResultado = vAccesodatos.FGrabaInspRCVehicularICRL(vInspRCVehicular01);

      if (vResultado > 0)
      {
        int vResul = FlTraeDatosRCV01(int.Parse(TextBoxNroInspeccion.Text));
        PBloqueoRCVehicular01(true);
      }
    }

    protected void ButtonMGrabarRCV01_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionRCVehicular vInspRCVehicular01 = new InspeccionRCVehicular();

      vInspRCVehicular01.secuencial = int.Parse(TextBoxSecuencialRCV01.Text);
      vInspRCVehicular01.idInspeccion = int.Parse(TextBoxNroInspeccion.Text);
      vInspRCVehicular01.nombreTercero = TextBoxNombreTerceroRCV01.Text;
      vInspRCVehicular01.docIdentidadTercero = TextBoxDocIdTerceroRCV01.Text;
      vInspRCVehicular01.telefonoTercero = TextBoxTelfTerceroRCV01.Text;
      vInspRCVehicular01.marca = DropDownListMarcaRCV01.SelectedItem.Text;
      vInspRCVehicular01.modelo = TextBoxModeloRCV01.Text;
      int vTempo = 0;
      vInspRCVehicular01.anio = vTempo;
      if (int.TryParse(TextBoxAnioRCV01.Text, out vTempo))
      {
        vInspRCVehicular01.anio = vTempo;
      }
      vInspRCVehicular01.placa = TextBoxPlacaRCV01.Text;
      vInspRCVehicular01.color = DropDownListColorRCV01.SelectedItem.Text;
      vInspRCVehicular01.chasis = TextBoxNroChasisRCV01.Text;
      vTempo = 0;
      vInspRCVehicular01.kilometraje = vTempo;
      if (int.TryParse(TextBoxKilometrajeRCV01.Text, out vTempo))
      {
        vInspRCVehicular01.kilometraje = vTempo;
      }

      vInspRCVehicular01.importacionDirecta = false;

      vInspRCVehicular01.tipoTaller = DropDownListTipoTallerRCVeh.SelectedItem.Text;

      int vResultado = vAccesodatos.FActualizaInspRCVehicularICRL(vInspRCVehicular01);

      if (vResultado > 0)
      {
        int vResul = FlTraeDatosRCV01(int.Parse(TextBoxNroInspeccion.Text));
        PBloqueoRCVehicular01(true);
        PLimpiaSeccionRCVehicular01();

      }
    }

    protected void ButtonMBorrarRCV01_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionRCVehicular vInspRCVehicular01 = new InspeccionRCVehicular();

      vInspRCVehicular01.secuencial = int.Parse(TextBoxSecuencialRCV01.Text);
      vInspRCVehicular01.idInspeccion = int.Parse(TextBoxNroInspeccion.Text);

      int vResultado = vAccesodatos.FBorrarInspRCVehicularICRL(vInspRCVehicular01);

      if (vResultado > 0)
      {
        int vResul = FlTraeDatosRCV01(int.Parse(TextBoxNroInspeccion.Text));
        PBloqueoRCVehicular01(true);
        PLimpiaSeccionRCVehicular01();
      }
    }

    protected void ButtonMDetalleRCV01_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      PBloqueoRCVehicular01(true);
      PLimpiaSeccionRCVehicular01();
      int vSecuencial = int.Parse(TextBoxSecuencialRCV01.Text);
      FlTraeDatosRCVehicularDet(vSecuencial);
      Session["PopupHabilitado"] = 1;
      this.ModalPopupRCV01.Show();
    }

    private int FlTraeDatosRCV01(int pIdInspeccion)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vLst = from i in db.Inspeccion
                   join ircv in db.InspRCVehicular on i.idInspeccion equals ircv.idInspeccion
                   where i.idInspeccion == pIdInspeccion
                   select new
                   {
                     //ircv.idInspeccion,
                     ircv.secuencial,
                     ircv.nombreTercero,
                     ircv.docIdentidadTercero,
                     ircv.telefonoTercero,
                     ircv.marca,
                     ircv.modelo,
                     ircv.anio,
                     ircv.placa,
                     ircv.color,
                     ircv.chasis,
                     ircv.kilometraje,
                     ircv.estado
                   };

        GridViewRCV01.DataSource = vLst.ToList();
        GridViewRCV01.DataBind();

      }

      return vResultado;
    }



    private int FlTraeItemsMarcaRCV01()
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vLst = from n in db.Nomenclador
                   where n.categoriaNomenclador == "Marca"
                   orderby n.descripcion
                   select new
                   {
                     n.codigo,
                     n.descripcion,
                   };

        DropDownListMarcaRCV01.DataValueField = "codigo";
        DropDownListMarcaRCV01.DataTextField = "descripcion";
        DropDownListMarcaRCV01.DataSource = vLst.ToList();
        DropDownListMarcaRCV01.DataBind();

      }

      return vResultado;
    }

    private int FlTraeItemsColorRCV01()
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vLst = from n in db.Nomenclador
                   where n.categoriaNomenclador == "Color"
                   orderby n.descripcion
                   select new
                   {
                     n.codigo,
                     n.descripcion,
                   };

        DropDownListColorRCV01.DataValueField = "codigo";
        DropDownListColorRCV01.DataTextField = "descripcion";
        DropDownListColorRCV01.DataSource = vLst.ToList();
        DropDownListColorRCV01.DataBind();

      }

      return vResultado;
    }

    private int CreaInspRCVehicularFlujo(int pIdFlujo)
    {
      AccesoDatos vAccesoDatos = new AccesoDatos();
      InspeccionICRL vInspeccionICRL = new InspeccionICRL();
      string vCodUsuario = Session["IdUsr"].ToString();
      int vResultado = 0;
      int vIdInspeccion = 0;

      int vIdUsuario = vAccesoDatos.FValidaExisteUsuarioICRL(vCodUsuario);
      vIdInspeccion = vAccesoDatos.FFlujoTieneRCVehicular(pIdFlujo);
      if (0 == vIdInspeccion)
      {
        vInspeccionICRL.idFlujo = pIdFlujo;
        vInspeccionICRL.correlativo = vAccesoDatos.fObtieneContadorInspeccionFlujo(pIdFlujo);
        vInspeccionICRL.idUsuario = vIdUsuario;
        vInspeccionICRL.sucursalAtencion = string.Empty;
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
        vInspeccionICRL.tipoInspeccion = (int)ICRL.BD.AccesoDatos.TipoInspeccion.RCVEhicular;
        vInspeccionICRL.tipoTaller = string.Empty;
        int vRespuesta = vAccesoDatos.FGrabaInspeccionICRL(vInspeccionICRL);

        if (0 == vRespuesta)
        {
          LabelMensaje.Text = "Error al crear la Inspeccion RC Vehicular";
        }
        else
        {
          TextBoxNroInspeccion.Text = vRespuesta.ToString();
          vResultado = vRespuesta;
        }
      }
      return vResultado;
    }

    #endregion

    #region RC Vehicular Detalle

    protected void PBloqueoRCVehicularDet01(bool pEstado)
    {
      ButtonDNuevoRCV01.Enabled = pEstado;
      ButtonDGrabarRCV01.Enabled = !pEstado;
      ButtonDBorrarRCV01.Enabled = !pEstado;
    }

    protected void PLimpiaSeccionRCV01Det()
    {
      DropDownListItemRCV01.SelectedIndex = 0;
      DropDownListItemRCV01.Enabled = true;
      DropDownListCompraRCV01.SelectedIndex = 0;
      DropDownListCompraRCV01.Enabled = true;
      //TextBoxCompraRCV01.Text = string.Empty;
      CheckBoxInstalacionRCV01.Checked = false;
      CheckBoxPinturaRCV01.Checked = false;
      CheckBoxMecanicoRCV01.Checked = false;
      DropDownListChaperioRCV01.SelectedIndex = 0;
      DropDownListRepPreviaRCV01.SelectedIndex = 0;
      TextBoxObservacionesRCV01.Text = string.Empty;
      PBloqueoRCVehicularDet01(true);
    }

    protected void GridViewRCV01Det_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      DropDownListItemRCV01.Enabled = false;
      TextBoxIdItemRCV01.Text = string.Empty;
      string vTextoTemporal = string.Empty;
      TextBoxIdItemRCV01.Text = GridViewRCV01Det.SelectedRow.Cells[1].Text.Substring(0, 8);
      string vTextoItemRCV01 = GridViewRCV01Det.SelectedRow.Cells[2].Text;

      DropDownListItemRCV01.ClearSelection();
      DropDownListItemRCV01.Items.FindByValue(TextBoxIdItemRCV01.Text).Selected = true;

      string vTextoCompra = string.Empty;
      vTextoCompra = GridViewRCV01Det.SelectedRow.Cells[3].Text.Trim();
      DropDownListCompraRCV01.ClearSelection();
      DropDownListCompraRCV01.Items.FindByText(vTextoCompra).Selected = true;

      //TextBoxCompraRCV01.Text = GridViewRCV01Det.SelectedRow.Cells[3].Text;
      CheckBoxInstalacionRCV01.Checked = (GridViewRCV01Det.SelectedRow.Cells[4].Controls[1] as CheckBox).Checked;
      CheckBoxPinturaRCV01.Checked = (GridViewRCV01Det.SelectedRow.Cells[5].Controls[1] as CheckBox).Checked;
      CheckBoxMecanicoRCV01.Checked = (GridViewRCV01Det.SelectedRow.Cells[6].Controls[1] as CheckBox).Checked;

      string vTempoCadena = string.Empty;
      vTempoCadena = GridViewRCV01Det.SelectedRow.Cells[7].Text.Trim();
      DropDownListChaperioRCV01.ClearSelection();
      DropDownListChaperioRCV01.Items.FindByText(vTempoCadena).Selected = true;

      vTempoCadena = string.Empty;
      vTempoCadena = GridViewRCV01Det.SelectedRow.Cells[8].Text.Trim();
      DropDownListRepPreviaRCV01.ClearSelection();
      DropDownListRepPreviaRCV01.Items.FindByText(vTempoCadena).Selected = true;

      vTextoTemporal = string.Empty;
      vTextoTemporal = GridViewRCV01Det.SelectedRow.Cells[9].Text;
      vTextoTemporal = vTextoTemporal.Replace("&#209;", "Ñ");
      vTextoTemporal = vTextoTemporal.Replace("&nbsp;", string.Empty);
      TextBoxObservacionesRCV01.Text = vTextoTemporal;

      TextBoxNroItemRCV01.Text = GridViewRCV01Det.SelectedRow.Cells[10].Text;
      PBloqueoRCVehicularDet01(false);
    }

    protected void GridViewRCV01Det_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      if (!VerificarPagina(true)) return;
      if (e.Row.RowType == DataControlRowType.DataRow)
      {
        e.Row.Attributes["onmouseover"] = "this.style.backgroundColor='aquamarine';";
        e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='white';";
        //e.Row.ToolTip = "Haz clic en la primera columna para seleccionar la fila.";



      }
    }

    protected void gvRCVDet_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      string vFilaFlujo = string.Empty;
      string vNroFlujo = string.Empty;

      //var gvOrders = (GridView)sender;
      //vFilaFlujo = gvOrders.SelectedRow.Cells[0].Text;


      //int vIdInspeccion = int.Parse(vFilaFlujo);
      //AccesoDatos vAccesoDatos = new AccesoDatos();
      //vNroFlujo = vAccesoDatos.FTraeNroFlujoInspeccion(vIdInspeccion);

      //Session["NumFlujo"] = vNroFlujo;

      //Response.Redirect("~/Presentacion/Inspeccion.aspx?nroInsp=" + vIdInspeccion.ToString());
    }

    public string MyNewRowDet(object pIdSecuencial)
    {
      string vTempo = string.Empty;
      vTempo = String.Format(@"</td></tr><tr id ='trvh{0}' class='collapsed-row'><td></td><td colspan='100' style='padding:0px; margin:0px;'>", pIdSecuencial);
      return vTempo;
    }

    private int FlTraeDatosRCVehicularDet(int pSecuencial)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vLst = from ircv in db.InspRCVehicular
                   join ircvdet in db.InspRCVehicularDetalle on ircv.secuencial equals ircvdet.secuencial
                   join n in db.Nomenclador on ircvdet.idItem equals n.codigo
                   where ircv.secuencial == pSecuencial && n.categoriaNomenclador == "Item"
                   select new
                   {
                     ircvdet.idItem,
                     n.descripcion,
                     ircvdet.compra,
                     ircvdet.instalacion,
                     ircvdet.pintura,
                     ircvdet.mecanico,
                     ircvdet.chaperio,
                     ircvdet.reparacionPrevia,
                     ircvdet.observaciones,
                     ircvdet.nro_item
                   };

        GridViewRCV01Det.DataSource = vLst.ToList();
        GridViewRCV01Det.DataBind();

      }

      return vResultado;
    }

    private int FlTraeNomenChaperio()
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vLst = from n in db.Nomenclador
                   where n.categoriaNomenclador == "Nivel de Daño"
                   orderby n.descripcion
                   select new
                   {
                     n.codigo,
                     n.descripcion,
                   };

        DropDownListChaperioRCV01.DataValueField = "codigo";
        DropDownListChaperioRCV01.DataTextField = "descripcion";
        DropDownListChaperioRCV01.DataSource = vLst.ToList();
        DropDownListChaperioRCV01.DataBind();

      }

      return vResultado;
    }

    private int FlTraeNomenRepPrevia()
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vLst = from n in db.Nomenclador
                   where n.categoriaNomenclador == "Nivel de Daño"
                   orderby n.descripcion
                   select new
                   {
                     n.codigo,
                     n.descripcion,
                   };

        DropDownListRepPreviaRCV01.DataValueField = "codigo";
        DropDownListRepPreviaRCV01.DataTextField = "descripcion";
        DropDownListRepPreviaRCV01.DataSource = vLst.ToList();
        DropDownListRepPreviaRCV01.DataBind();

      }

      return vResultado;
    }

    private int FlTraeNomenCompraDetRCV01()
    {
      int vResultado = 0;
      string vCategoria = "Compra Repuesto";
      int vOrdenCodigo = 2;
      AccesoDatos vAccesoDatos = new AccesoDatos();

      DropDownListCompraRCV01.DataValueField = "codigo";
      DropDownListCompraRCV01.DataTextField = "descripcion";
      DropDownListCompraRCV01.DataSource = vAccesoDatos.FlTraeNomenGenerico(vCategoria, vOrdenCodigo);
      DropDownListCompraRCV01.DataBind();

      return vResultado;
    }

    private int FlTraeNomenTipoTallerInsp()
    {
      int vResultado = 0;
      string vCategoria = "Tipo Taller";
      int vOrdenCodigo = 2;
      AccesoDatos vAccesoDatos = new AccesoDatos();

      DropDownListTipoTallerInsp.DataValueField = "codigo";
      DropDownListTipoTallerInsp.DataTextField = "descripcion";
      DropDownListTipoTallerInsp.DataSource = vAccesoDatos.FlTraeNomenGenerico(vCategoria, vOrdenCodigo);
      DropDownListTipoTallerInsp.DataBind();

      return vResultado;
    }

    private int FlTraeNomenTipoTallerRCVeh()
    {
      int vResultado = 0;
      string vCategoria = "Tipo Taller";
      int vOrdenCodigo = 2;
      AccesoDatos vAccesoDatos = new AccesoDatos();

      DropDownListTipoTallerRCVeh.DataValueField = "codigo";
      DropDownListTipoTallerRCVeh.DataTextField = "descripcion";
      DropDownListTipoTallerRCVeh.DataSource = vAccesoDatos.FlTraeNomenGenerico(vCategoria, vOrdenCodigo);
      DropDownListTipoTallerRCVeh.DataBind();

      return vResultado;
    }

    private int FlTraeNomenItemRCV01()
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vLst = from n in db.Nomenclador
                   where n.categoriaNomenclador == "Item"
                   orderby n.descripcion
                   select new
                   {
                     n.codigo,
                     n.descripcion,
                   };

        DropDownListItemRCV01.DataValueField = "codigo";
        DropDownListItemRCV01.DataTextField = "descripcion";
        DropDownListItemRCV01.DataSource = vLst.ToList();
        DropDownListItemRCV01.DataBind();

      }

      return vResultado;
    }



    protected void ButtonDNuevoRCV01_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionRCVehicularDet vInspRCVDet = new InspeccionRCVehicularDet();
      //DropDownListMarcaRCV01.SelectedItem.Text;

      vInspRCVDet.idItem = DropDownListItemRCV01.SelectedValue;
      vInspRCVDet.secuencial = int.Parse(TextBoxSecuencialRCV01.Text);
      vInspRCVDet.compra = DropDownListCompraRCV01.SelectedItem.Text;
      //vInspRCVDet.compra = TextBoxCompraRCV01.Text;
      vInspRCVDet.instalacion = CheckBoxInstalacionRCV01.Checked;
      vInspRCVDet.pintura = CheckBoxPinturaRCV01.Checked;
      vInspRCVDet.mecanico = CheckBoxMecanicoRCV01.Checked;
      vInspRCVDet.chaperio = DropDownListChaperioRCV01.SelectedItem.Text.Trim();
      vInspRCVDet.reparacionPrevia = DropDownListRepPreviaRCV01.SelectedItem.Text.Trim();
      vInspRCVDet.observaciones = TextBoxObservacionesRCV01.Text.ToUpper().Trim();

      int vResultado = vAccesodatos.FGrabaInspRCV01DetICRL(vInspRCVDet);

      if (vResultado > 0)
      {
        int vResul = FlTraeDatosRCVehicularDet(int.Parse(TextBoxSecuencialRCV01.Text));
        PLimpiaSeccionRCV01Det();
      }
    }

    protected void ButtonDGrabarRCV01_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionRCVehicularDet vInspRCVDet = new InspeccionRCVehicularDet();

      vInspRCVDet.idItem = TextBoxIdItemRCV01.Text;
      vInspRCVDet.secuencial = int.Parse(TextBoxSecuencialRCV01.Text);
      vInspRCVDet.compra = DropDownListCompraRCV01.SelectedItem.Text;
      //vInspRCVDet.compra = TextBoxCompraRCV01.Text;
      vInspRCVDet.instalacion = CheckBoxInstalacionRCV01.Checked;
      vInspRCVDet.pintura = CheckBoxPinturaRCV01.Checked;
      vInspRCVDet.mecanico = CheckBoxMecanicoRCV01.Checked;
      vInspRCVDet.chaperio = DropDownListChaperioRCV01.SelectedItem.Text.Trim();
      vInspRCVDet.reparacionPrevia = DropDownListRepPreviaRCV01.SelectedItem.Text.Trim();
      vInspRCVDet.observaciones = TextBoxObservacionesRCV01.Text.ToUpper().Trim();
      vInspRCVDet.nro_item = long.Parse(TextBoxNroItemRCV01.Text);

      int vResultado = vAccesodatos.FActualizaInspRCV01DetICRL(vInspRCVDet);

      if (vResultado > 0)
      {
        int vResul = FlTraeDatosRCVehicularDet(int.Parse(TextBoxSecuencialRCV01.Text));
        PLimpiaSeccionRCV01Det();
      }
    }

    protected void ButtonDBorrarRCV01_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesodatos = new AccesoDatos();
      InspeccionRCVehicularDet vInspRCVDet = new InspeccionRCVehicularDet();

      vInspRCVDet.idItem = TextBoxIdItemRCV01.Text;
      vInspRCVDet.secuencial = int.Parse(TextBoxSecuencialRCV01.Text);
      vInspRCVDet.nro_item = long.Parse(TextBoxNroItemRCV01.Text);

      int vResultado = vAccesodatos.FBorrarInspRCV01DetICRL(vInspRCVDet);

      if (vResultado > 0)
      {
        int vResul = FlTraeDatosRCVehicularDet(int.Parse(TextBoxSecuencialRCV01.Text));
        PLimpiaSeccionRCV01Det();
      }
    }


    protected void ButtonCancelPop_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      int vResul = 0;
      PBloqueoRCVehicular01(true);
      PLimpiaSeccionRCVehicular01();
      vResul = FlTraeDatosRCV01(int.Parse(TextBoxNroInspeccion.Text));
      Session["PopupHabilitado"] = 0;
      this.ModalPopupRCV01.Hide();
    }


    #endregion

    #region Reportes

    protected void PImprimeFormularioInspDaniosPropios(int pIdSecuencial)
    {
      AccesoDatos vAccesoDatos = new AccesoDatos();
      LBCDesaEntities db = new LBCDesaEntities();

      int vIdFlujo = 0;
      int vIdInspeccion = 0;

      Warning[] warnings;
      string[] streamIds;
      string mimeType = string.Empty;
      string encoding = string.Empty;
      string extension = "pdf";
      string fileName = "RepFormInspeccionDaniosPropios" + pIdSecuencial.ToString();

      vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
      vIdInspeccion = int.Parse(TextBoxNroInspeccion.Text);

      var vListaFlujo = from i in db.Inspeccion
                        join f in db.Flujo on i.idFlujo equals f.idFlujo
                        where (i.idFlujo == vIdFlujo)
                           && (i.tipoCobertura == (int)ICRL.BD.AccesoDatos.TipoInspeccion.DaniosPropios)
                        orderby f.flujoOnBase, i.idInspeccion
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
                          i.descripcionSiniestro
                        };

      var vListaInspDaniosPropios = from i in db.Inspeccion
                                    join f in db.Flujo on i.idFlujo equals f.idFlujo
                                    join idpp in db.InspDaniosPropiosPadre on i.idInspeccion equals idpp.idInspeccion
                                    join idp in db.InspDaniosPropios on idpp.secuencial equals idp.secuencial
                                    join n in db.Nomenclador on idp.idItem equals n.codigo
                                    where (n.categoriaNomenclador == "Item")
                                    && (i.idFlujo == vIdFlujo)
                                    && (i.tipoCobertura == (int)ICRL.BD.AccesoDatos.TipoInspeccion.DaniosPropios)
                                    && (idp.secuencial == pIdSecuencial)
                                    orderby i.idInspeccion, n.descripcion
                                    select new
                                    {
                                      idInspeccion = idp.secuencial,
                                      idp.idItem,
                                      n.descripcion,
                                      idp.compra,
                                      idp.instalacion,
                                      idp.pintura,
                                      idp.mecanico,
                                      idp.chaperio,
                                      idp.reparacionPrevia,
                                      idp.observaciones
                                    };

      ReportViewerInsp.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
      ReportViewerInsp.LocalReport.ReportPath = "Reportes\\RepFormularioInspDaniosPropios.rdlc";
      ReportDataSource datasource1 = new ReportDataSource("DataSet1", vListaFlujo);
      ReportDataSource datasource2 = new ReportDataSource("DataSet2", vListaInspDaniosPropios);

      ReportViewerInsp.LocalReport.DataSources.Clear();
      ReportViewerInsp.LocalReport.DataSources.Add(datasource1);
      ReportViewerInsp.LocalReport.DataSources.Add(datasource2);

      ReportViewerInsp.LocalReport.Refresh();
      byte[] VArrayBytes = ReportViewerInsp.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

      //enviar el array de bytes a cliente
      Response.Buffer = true;
      Response.Clear();
      Response.ContentType = mimeType;
      Response.AddHeader("content-disposition", "attachment; filename=" + fileName + "." + extension);
      Response.BinaryWrite(VArrayBytes); // se crea el archivo
      Response.Flush(); // se envia al cliente para su descarga
    }

    protected void PImprimeFormularioInspRCObjeto(int pIdSecuencial)
    {
      AccesoDatos vAccesoDatos = new AccesoDatos();
      LBCDesaEntities db = new LBCDesaEntities();

      int vIdFlujo = 0;
      int vIdInspeccion = 0;

      Warning[] warnings;
      string[] streamIds;
      string mimeType = string.Empty;
      string encoding = string.Empty;
      string extension = "pdf";
      string fileName = "RepFormInspeccionRCObjetos" + pIdSecuencial.ToString();

      vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
      vIdInspeccion = int.Parse(TextBoxNroInspeccion.Text);

      var vListaFlujo = from i in db.Inspeccion
                        join f in db.Flujo on i.idFlujo equals f.idFlujo
                        where (i.idFlujo == vIdFlujo)
                           && (i.tipoCobertura == (int)ICRL.BD.AccesoDatos.TipoInspeccion.DaniosPropios)
                        orderby f.flujoOnBase, i.idInspeccion
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
                          i.descripcionSiniestro
                        };

      var vListaInspRCObj = from i in db.Inspeccion
                            join f in db.Flujo on i.idFlujo equals f.idFlujo
                            join irco in db.InspRCObjeto on i.idInspeccion equals irco.idInspeccion
                            join ircodet in db.InspRCObjetoDetalle on irco.secuencial equals ircodet.secuencial
                            where (i.idFlujo == vIdFlujo)
                               && (i.tipoCobertura == (int)ICRL.BD.AccesoDatos.TipoInspeccion.RCObjetos)
                               && (irco.secuencial == pIdSecuencial)
                            orderby i.idInspeccion, irco.secuencial
                            select new
                            {
                              irco.idInspeccion,
                              ircodet.secuencial,
                              irco.nombreObjeto,
                              irco.docIdentidadObjeto,
                              irco.observacionesObjeto,
                              ircodet.idItem,
                              ircodet.costoReferencial,
                              ircodet.descripcion
                            };

      ReportViewerInsp.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
      ReportViewerInsp.LocalReport.ReportPath = "Reportes\\RepFormularioInspRCObjetos.rdlc";
      ReportDataSource datasource1 = new ReportDataSource("DataSet1", vListaFlujo);
      ReportDataSource datasource4 = new ReportDataSource("DataSet4", vListaInspRCObj);

      ReportViewerInsp.LocalReport.DataSources.Clear();
      ReportViewerInsp.LocalReport.DataSources.Add(datasource1);
      ReportViewerInsp.LocalReport.DataSources.Add(datasource4);

      ReportViewerInsp.LocalReport.Refresh();
      byte[] VArrayBytes = ReportViewerInsp.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

      //enviar el array de bytes a cliente
      Response.Buffer = true;
      Response.Clear();
      Response.ContentType = mimeType;
      Response.AddHeader("content-disposition", "attachment; filename=" + fileName + "." + extension);
      Response.BinaryWrite(VArrayBytes); // se crea el archivo
      Response.Flush(); // se envia al cliente para su descarga
    }

    protected void PImprimeFormularioInspRCPersona(int pIdSecuencial)
    {
      AccesoDatos vAccesoDatos = new AccesoDatos();
      LBCDesaEntities db = new LBCDesaEntities();

      int vIdFlujo = 0;
      int vIdInspeccion = 0;

      Warning[] warnings;
      string[] streamIds;
      string mimeType = string.Empty;
      string encoding = string.Empty;
      string extension = "pdf";
      string fileName = "RepFormInspeccionRCPersonas" + pIdSecuencial.ToString();

      vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
      vIdInspeccion = int.Parse(TextBoxNroInspeccion.Text);

      var vListaFlujo = from i in db.Inspeccion
                        join f in db.Flujo on i.idFlujo equals f.idFlujo
                        where (i.idFlujo == vIdFlujo)
                           && (i.tipoCobertura == (int)ICRL.BD.AccesoDatos.TipoInspeccion.DaniosPropios)
                        orderby f.flujoOnBase, i.idInspeccion
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
                          i.descripcionSiniestro
                        };

      var vListaInspRCPer = from i in db.Inspeccion
                            join f in db.Flujo on i.idFlujo equals f.idFlujo
                            join ircp in db.InspRCPersona on i.idInspeccion equals ircp.idInspeccion
                            join ircpdet in db.InspRCPersonaDetalle on ircp.secuencial equals ircpdet.secuencial
                            where (i.idFlujo == vIdFlujo)
                               && (i.tipoCobertura == (int)ICRL.BD.AccesoDatos.TipoInspeccion.RCPersonas)
                               && (ircp.secuencial == pIdSecuencial)
                            orderby i.idInspeccion, ircp.secuencial
                            select new
                            {
                              ircp.idInspeccion,
                              ircpdet.secuencial,
                              ircp.nombrePersona,
                              ircp.docIdentidadPersona,
                              ircp.observacionesPersona,
                              ircpdet.tipo,
                              ircpdet.montoGasto,
                              ircpdet.descripcion
                            };

      ReportViewerInsp.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
      ReportViewerInsp.LocalReport.ReportPath = "Reportes\\RepFormularioInspRCPersonas.rdlc";
      ReportDataSource datasource1 = new ReportDataSource("DataSet1", vListaFlujo);
      ReportDataSource datasource3 = new ReportDataSource("DataSet3", vListaInspRCPer);

      ReportViewerInsp.LocalReport.DataSources.Clear();
      ReportViewerInsp.LocalReport.DataSources.Add(datasource1);
      ReportViewerInsp.LocalReport.DataSources.Add(datasource3);

      ReportViewerInsp.LocalReport.Refresh();
      byte[] VArrayBytes = ReportViewerInsp.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

      //enviar el array de bytes a cliente
      Response.Buffer = true;
      Response.Clear();
      Response.ContentType = mimeType;
      Response.AddHeader("content-disposition", "attachment; filename=" + fileName + "." + extension);
      Response.BinaryWrite(VArrayBytes); // se crea el archivo
      Response.Flush(); // se envia al cliente para su descarga
    }

    protected void PImprimeFormularioInspRCVehicular(int pIdSecuencial)
    {
      AccesoDatos vAccesoDatos = new AccesoDatos();
      LBCDesaEntities db = new LBCDesaEntities();

      int vIdFlujo = 0;
      int vIdInspeccion = 0;

      Warning[] warnings;
      string[] streamIds;
      string mimeType = string.Empty;
      string encoding = string.Empty;
      string extension = "pdf";
      string fileName = "RepFormInspeccionRCVehicular" + pIdSecuencial.ToString();

      vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
      vIdInspeccion = int.Parse(TextBoxNroInspeccion.Text);

      var vListaFlujo = from i in db.Inspeccion
                        join f in db.Flujo on i.idFlujo equals f.idFlujo
                        where (i.idFlujo == vIdFlujo)
                           && (i.tipoCobertura == (int)ICRL.BD.AccesoDatos.TipoInspeccion.DaniosPropios)
                        orderby f.flujoOnBase, i.idInspeccion
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
                          i.descripcionSiniestro
                        };

      var vListaInspRCVeh = from i in db.Inspeccion
                            join f in db.Flujo on i.idFlujo equals f.idFlujo
                            join ircv in db.InspRCVehicular on i.idInspeccion equals ircv.idInspeccion
                            join ircvdet in db.InspRCVehicularDetalle on ircv.secuencial equals ircvdet.secuencial
                            join n in db.Nomenclador on ircvdet.idItem equals n.codigo
                            where (n.categoriaNomenclador == "Item")
                               && (i.idFlujo == vIdFlujo)
                               && (i.tipoCobertura == (int)ICRL.BD.AccesoDatos.TipoInspeccion.RCVEhicular)
                               && (ircv.secuencial == pIdSecuencial)
                            orderby i.idInspeccion, ircv.secuencial
                            select new
                            {
                              ircv.idInspeccion,
                              ircvdet.secuencial,
                              ircv.nombreTercero,
                              ircv.docIdentidadTercero,
                              ircv.marca,
                              ircv.modelo,
                              ircv.placa,
                              ircv.chasis,
                              ircv.color,
                              ircvdet.idItem,
                              n.descripcion,
                              ircvdet.compra,
                              ircvdet.instalacion,
                              ircvdet.pintura,
                              ircvdet.chaperio,
                              ircvdet.mecanico,
                              ircvdet.reparacionPrevia,
                              ircvdet.observaciones
                            };



      ReportViewerInsp.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
      ReportViewerInsp.LocalReport.ReportPath = "Reportes\\RepFormularioInspRCVehicular.rdlc";
      ReportDataSource datasource1 = new ReportDataSource("DataSet1", vListaFlujo);
      ReportDataSource datasource5 = new ReportDataSource("DataSet5", vListaInspRCVeh);

      ReportViewerInsp.LocalReport.DataSources.Clear();
      ReportViewerInsp.LocalReport.DataSources.Add(datasource1);
      ReportViewerInsp.LocalReport.DataSources.Add(datasource5);

      ReportViewerInsp.LocalReport.Refresh();
      byte[] VArrayBytes = ReportViewerInsp.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

      //enviar el array de bytes a cliente
      Response.Buffer = true;
      Response.Clear();
      Response.ContentType = mimeType;
      Response.AddHeader("content-disposition", "attachment; filename=" + fileName + "." + extension);
      Response.BinaryWrite(VArrayBytes); // se crea el archivo
      Response.Flush(); // se envia al cliente para su descarga
    }

    protected void ImgButtonExportPdf_Click(object sender, ImageClickEventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesoDatos = new AccesoDatos();
      LBCDesaEntities db = new LBCDesaEntities();

      Warning[] warnings;
      string[] streamIds;
      string mimeType = string.Empty;
      string encoding = string.Empty;
      string extension = "pdf";
      string fileName = "RepFormInspeccion";

      int vInspeccion = 0;
      int vIdFlujo = 0;
      vInspeccion = int.Parse(TextBoxNroInspeccion.Text);
      vIdFlujo = int.Parse(TextBoxIdFlujo.Text);

      var vListaFlujo = from i in db.Inspeccion
                        join f in db.Flujo on i.idFlujo equals f.idFlujo
                        where (i.idFlujo == vIdFlujo)
                           && (i.tipoCobertura == (int)ICRL.BD.AccesoDatos.TipoInspeccion.DaniosPropios)
                        orderby f.flujoOnBase, i.idInspeccion
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
                          i.descripcionSiniestro
                        };

      var vListaInspDaniosPropios = from i in db.Inspeccion
                                    join f in db.Flujo on i.idFlujo equals f.idFlujo
                                    join idpp in db.InspDaniosPropiosPadre on i.idInspeccion equals idpp.idInspeccion
                                    join idp in db.InspDaniosPropios on idpp.secuencial equals idp.secuencial
                                    join n in db.Nomenclador on idp.idItem equals n.codigo
                                    where (n.categoriaNomenclador == "Item")
                                    && (i.idFlujo == vIdFlujo)
                                    && (i.tipoCobertura == (int)ICRL.BD.AccesoDatos.TipoInspeccion.DaniosPropios)
                                    orderby i.idInspeccion, n.descripcion
                                    select new
                                    {
                                      idInspeccion = idpp.secuencial,
                                      idp.idItem,
                                      n.descripcion,
                                      idp.compra,
                                      idp.instalacion,
                                      idp.pintura,
                                      idp.mecanico,
                                      idp.chaperio,
                                      idp.reparacionPrevia,
                                      idp.observaciones
                                    };

      var vListaInspRCPer = from i in db.Inspeccion
                            join f in db.Flujo on i.idFlujo equals f.idFlujo
                            join ircp in db.InspRCPersona on i.idInspeccion equals ircp.idInspeccion
                            join ircpdet in db.InspRCPersonaDetalle on ircp.secuencial equals ircpdet.secuencial
                            where (i.idFlujo == vIdFlujo)
                               && (i.tipoCobertura == (int)ICRL.BD.AccesoDatos.TipoInspeccion.RCPersonas)
                            orderby i.idInspeccion, ircp.secuencial
                            select new
                            {
                              ircp.idInspeccion,
                              ircpdet.secuencial,
                              ircp.nombrePersona,
                              ircp.docIdentidadPersona,
                              ircp.observacionesPersona,
                              ircpdet.tipo,
                              ircpdet.montoGasto,
                              ircpdet.descripcion
                            };

      var vListaInspRCObj = from i in db.Inspeccion
                            join f in db.Flujo on i.idFlujo equals f.idFlujo
                            join irco in db.InspRCObjeto on i.idInspeccion equals irco.idInspeccion
                            join ircodet in db.InspRCObjetoDetalle on irco.secuencial equals ircodet.secuencial
                            where (i.idFlujo == vIdFlujo)
                               && (i.tipoCobertura == (int)ICRL.BD.AccesoDatos.TipoInspeccion.RCObjetos)
                            orderby i.idInspeccion, irco.secuencial
                            select new
                            {
                              irco.idInspeccion,
                              ircodet.secuencial,
                              irco.nombreObjeto,
                              irco.docIdentidadObjeto,
                              irco.observacionesObjeto,
                              ircodet.idItem,
                              ircodet.costoReferencial,
                              ircodet.descripcion
                            };

      var vListaInspRCVeh = from i in db.Inspeccion
                            join f in db.Flujo on i.idFlujo equals f.idFlujo
                            join ircv in db.InspRCVehicular on i.idInspeccion equals ircv.idInspeccion
                            join ircvdet in db.InspRCVehicularDetalle on ircv.secuencial equals ircvdet.secuencial
                            join n in db.Nomenclador on ircvdet.idItem equals n.codigo
                            where (n.categoriaNomenclador == "Item")
                               && (i.idFlujo == vIdFlujo)
                               && (i.tipoCobertura == (int)ICRL.BD.AccesoDatos.TipoInspeccion.RCVEhicular)
                            orderby i.idInspeccion, ircv.secuencial
                            select new
                            {
                              ircv.idInspeccion,
                              ircvdet.secuencial,
                              ircv.nombreTercero,
                              ircv.docIdentidadTercero,
                              ircv.marca,
                              ircv.modelo,
                              ircv.placa,
                              ircv.chasis,
                              ircv.color,
                              ircvdet.idItem,
                              n.descripcion,
                              ircvdet.compra,
                              ircvdet.instalacion,
                              ircvdet.pintura,
                              ircvdet.chaperio,
                              ircvdet.mecanico,
                              ircvdet.reparacionPrevia,
                              ircvdet.observaciones
                            };

      var vListaInspRoboParcial = from i in db.Inspeccion
                                  join f in db.Flujo on i.idFlujo equals f.idFlujo
                                  join irp in db.InspRoboParcial on i.idInspeccion equals irp.idInspeccion
                                  join n in db.Nomenclador on irp.idItem equals n.codigo
                                  where (n.categoriaNomenclador == "Item")
                                     && (i.idFlujo == vIdFlujo)
                                     && (i.tipoCobertura == (int)ICRL.BD.AccesoDatos.TipoInspeccion.RoboParcial)
                                  orderby i.idInspeccion
                                  select new
                                  {
                                    irp.idInspeccion,
                                    irp.idItem,
                                    n.descripcion,
                                    irp.compra,
                                    irp.instalacion,
                                    irp.pintura,
                                    irp.mecanico,
                                    irp.chaperio,
                                    irp.reparacionPrevia,
                                    irp.observaciones
                                  };

      var vListaInspPTDaniosP = from i in db.Inspeccion
                                join f in db.Flujo on i.idFlujo equals f.idFlujo
                                join iptdp in db.InspPerdidaTotalDanios on i.idInspeccion equals iptdp.idInspeccion
                                where (i.idFlujo == vIdFlujo)
                                   && (i.tipoCobertura == (int)ICRL.BD.AccesoDatos.TipoInspeccion.PerdidaTotalDaniosPropios)
                                orderby iptdp.idInspeccion
                                select new
                                {
                                  iptdp.idInspeccion,
                                  iptdp.version,
                                  iptdp.serie,
                                  iptdp.caja,
                                  iptdp.combustible,
                                  iptdp.cilindrada,
                                  iptdp.techoSolar,
                                  iptdp.asientosCuero,
                                  iptdp.arosMagnesio,
                                  iptdp.convertidoGNV,
                                  iptdp.observaciones
                                };

      var vListaInspPTRobo = from i in db.Inspeccion
                             join f in db.Flujo on i.idFlujo equals f.idFlujo
                             join iptrobo in db.InspPerdidaTotalRobo on i.idInspeccion equals iptrobo.idInspeccion
                             where (i.idFlujo == vIdFlujo)
                                && (i.tipoCobertura == (int)ICRL.BD.AccesoDatos.TipoInspeccion.PerdidaTotalRobo)
                             orderby iptrobo.idInspeccion
                             select new
                             {
                               iptrobo.idInspeccion,
                               iptrobo.version,
                               iptrobo.serie,
                               iptrobo.caja,
                               iptrobo.combustible,
                               iptrobo.cilindrada,
                               iptrobo.techoSolar,
                               iptrobo.asientosCuero,
                               iptrobo.arosMagnesio,
                               iptrobo.convertidoGNV,
                               iptrobo.observaciones
                             };




      ReportViewerInsp.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
      ReportDataSource datasource1 = new ReportDataSource("DataSet1", vListaFlujo);
      ReportDataSource datasource2 = new ReportDataSource("DataSet2", vListaInspDaniosPropios);
      ReportDataSource datasource3 = new ReportDataSource("DataSet3", vListaInspRCPer);
      ReportDataSource datasource4 = new ReportDataSource("DataSet4", vListaInspRCObj);
      ReportDataSource datasource5 = new ReportDataSource("DataSet5", vListaInspRCVeh);
      ReportDataSource datasource6 = new ReportDataSource("DataSet6", vListaInspRoboParcial);
      ReportDataSource datasource7 = new ReportDataSource("DataSet7", vListaInspPTDaniosP);
      ReportDataSource datasource8 = new ReportDataSource("DataSet8", vListaInspPTRobo);

      ReportViewerInsp.LocalReport.DataSources.Clear();
      ReportViewerInsp.LocalReport.DataSources.Add(datasource1);
      ReportViewerInsp.LocalReport.DataSources.Add(datasource2);
      ReportViewerInsp.LocalReport.DataSources.Add(datasource3);
      ReportViewerInsp.LocalReport.DataSources.Add(datasource4);
      ReportViewerInsp.LocalReport.DataSources.Add(datasource5);
      ReportViewerInsp.LocalReport.DataSources.Add(datasource6);
      ReportViewerInsp.LocalReport.DataSources.Add(datasource7);
      ReportViewerInsp.LocalReport.DataSources.Add(datasource8);
      ReportViewerInsp.LocalReport.Refresh();
      byte[] VArrayBytes = ReportViewerInsp.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

      //enviar el array de bytes a cliente
      Response.Buffer = true;
      Response.Clear();
      Response.ContentType = mimeType;
      Response.AddHeader("content-disposition", "attachment; filename=" + fileName + "." + extension);
      Response.BinaryWrite(VArrayBytes); // create the file
      Response.Flush(); // send it to the client to download

    }

    protected void ImgButtonExportPdfPTRO_Click(object sender, ImageClickEventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesoDatos = new AccesoDatos();
      LBCDesaEntities db = new LBCDesaEntities();

      Warning[] warnings;
      string[] streamIds;
      string mimeType = string.Empty;
      string encoding = string.Empty;
      string extension = "pdf";
      string fileName = "RepFormInspeccion";

      int vInspeccion = 0;
      int vIdFlujo = 0;
      vInspeccion = int.Parse(TextBoxNroInspeccion.Text);
      vIdFlujo = int.Parse(TextBoxIdFlujo.Text);

      var vListaFlujo = from i in db.Inspeccion
                        join f in db.Flujo on i.idFlujo equals f.idFlujo
                        where (i.idFlujo == vIdFlujo)
                           && (i.tipoCobertura == (int)ICRL.BD.AccesoDatos.TipoInspeccion.DaniosPropios)
                        orderby f.flujoOnBase, i.idInspeccion
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
                          i.descripcionSiniestro
                        };

      var vListaInspPTRobo = from i in db.Inspeccion
                             join f in db.Flujo on i.idFlujo equals f.idFlujo
                             join iptrobo in db.InspPerdidaTotalRobo on i.idInspeccion equals iptrobo.idInspeccion
                             where (i.idFlujo == vIdFlujo)
                                && (i.tipoCobertura == (int)ICRL.BD.AccesoDatos.TipoInspeccion.PerdidaTotalRobo)
                             orderby iptrobo.idInspeccion
                             select new
                             {
                               iptrobo.idInspeccion,
                               iptrobo.version,
                               iptrobo.serie,
                               iptrobo.caja,
                               iptrobo.combustible,
                               iptrobo.cilindrada,
                               iptrobo.techoSolar,
                               iptrobo.asientosCuero,
                               iptrobo.arosMagnesio,
                               iptrobo.convertidoGNV,
                               iptrobo.observaciones
                             };




      ReportViewerInsp.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
      ReportViewerInsp.LocalReport.ReportPath = "Reportes\\RepFormularioInspPTRobo.rdlc";
      ReportDataSource datasource1 = new ReportDataSource("DataSet1", vListaFlujo);
      ReportDataSource datasource8 = new ReportDataSource("DataSet8", vListaInspPTRobo);

      ReportViewerInsp.LocalReport.DataSources.Clear();
      ReportViewerInsp.LocalReport.DataSources.Add(datasource1);
      ReportViewerInsp.LocalReport.DataSources.Add(datasource8);
      ReportViewerInsp.LocalReport.Refresh();
      byte[] VArrayBytes = ReportViewerInsp.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

      //enviar el array de bytes a cliente
      Response.Buffer = true;
      Response.Clear();
      Response.ContentType = mimeType;
      Response.AddHeader("content-disposition", "attachment; filename=" + fileName + "." + extension);
      Response.BinaryWrite(VArrayBytes); // create the file
      Response.Flush(); // send it to the client to download
    }

    protected void ButtonFinPTRobo_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      int vIdInspeccion = 0;
      int vIdFlujo = 0;
      vIdInspeccion = int.Parse(TextBoxNroInspeccion.Text);
      vIdFlujo = int.Parse(TextBoxIdFlujo.Text);

      //proceso que copia los datos de Inps a Coti
      AccesoDatos vAccesoDatos = new AccesoDatos();
      vAccesoDatos.fCopiaPTRoboInspACotizacion (vIdFlujo, vIdInspeccion);
      int vResultado = 0;
      vResultado = vAccesoDatos.FPTRoboCambiaEstado(vIdInspeccion);
      ButtonFinPTRobo.Enabled = false;
    }

    protected void ImgButtonExportPdfPTDP_Click(object sender, ImageClickEventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesoDatos = new AccesoDatos();
      LBCDesaEntities db = new LBCDesaEntities();

      Warning[] warnings;
      string[] streamIds;
      string mimeType = string.Empty;
      string encoding = string.Empty;
      string extension = "pdf";
      string fileName = "RepFormInspeccion";

      int vInspeccion = 0;
      int vIdFlujo = 0;
      vInspeccion = int.Parse(TextBoxNroInspeccion.Text);
      vIdFlujo = int.Parse(TextBoxIdFlujo.Text);

      var vListaFlujo = from i in db.Inspeccion
                        join f in db.Flujo on i.idFlujo equals f.idFlujo
                        where (i.idFlujo == vIdFlujo)
                           && (i.tipoCobertura == (int)ICRL.BD.AccesoDatos.TipoInspeccion.DaniosPropios)
                        orderby f.flujoOnBase, i.idInspeccion
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
                          i.descripcionSiniestro
                        };



      var vListaInspPTDaniosP = from i in db.Inspeccion
                                join f in db.Flujo on i.idFlujo equals f.idFlujo
                                join iptdp in db.InspPerdidaTotalDanios on i.idInspeccion equals iptdp.idInspeccion
                                where (i.idFlujo == vIdFlujo)
                                   && (i.tipoCobertura == (int)ICRL.BD.AccesoDatos.TipoInspeccion.PerdidaTotalDaniosPropios)
                                orderby iptdp.idInspeccion
                                select new
                                {
                                  iptdp.idInspeccion,
                                  iptdp.version,
                                  iptdp.serie,
                                  iptdp.caja,
                                  iptdp.combustible,
                                  iptdp.cilindrada,
                                  iptdp.techoSolar,
                                  iptdp.asientosCuero,
                                  iptdp.arosMagnesio,
                                  iptdp.convertidoGNV,
                                  iptdp.observaciones
                                };


      ReportViewerInsp.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
      ReportViewerInsp.LocalReport.ReportPath = "Reportes\\RepFormularioInspPTDaniosPropios.rdlc";
      ReportDataSource datasource1 = new ReportDataSource("DataSet1", vListaFlujo);
      ReportDataSource datasource7 = new ReportDataSource("DataSet7", vListaInspPTDaniosP);

      ReportViewerInsp.LocalReport.DataSources.Clear();
      ReportViewerInsp.LocalReport.DataSources.Add(datasource1);
      ReportViewerInsp.LocalReport.DataSources.Add(datasource7);
      ReportViewerInsp.LocalReport.Refresh();
      byte[] VArrayBytes = ReportViewerInsp.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

      //enviar el array de bytes a cliente
      Response.Buffer = true;
      Response.Clear();
      Response.ContentType = mimeType;
      Response.AddHeader("content-disposition", "attachment; filename=" + fileName + "." + extension);
      Response.BinaryWrite(VArrayBytes); // create the file
      Response.Flush(); // send it to the client to download
    }

    protected void ButtonFinPTDaniosP_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      int vIdInspeccion = 0;
      int vIdFlujo = 0;
      vIdInspeccion = int.Parse(TextBoxNroInspeccion.Text);
      vIdFlujo = int.Parse(TextBoxIdFlujo.Text);

      //proceso que copia los datos de Inps a Coti
      AccesoDatos vAccesoDatos = new AccesoDatos();
      vAccesoDatos.fCopiaPTDaniosPropiosInspACotizacion(vIdFlujo, vIdInspeccion);

      int vResultado = 0;
      vResultado = vAccesoDatos.FPTDaniosPCambiaEstado(vIdInspeccion);
      ButtonFinPTDaniosP.Enabled = false;
    }

    protected void ImgButtonExportPdfRoboP_Click(object sender, ImageClickEventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesoDatos = new AccesoDatos();
      LBCDesaEntities db = new LBCDesaEntities();

      Warning[] warnings;
      string[] streamIds;
      string mimeType = string.Empty;
      string encoding = string.Empty;
      string extension = "pdf";
      string fileName = "RepFormInspeccion";

      int vInspeccion = 0;
      int vIdFlujo = 0;
      vInspeccion = int.Parse(TextBoxNroInspeccion.Text);
      vIdFlujo = int.Parse(TextBoxIdFlujo.Text);

      var vListaFlujo = from i in db.Inspeccion
                        join f in db.Flujo on i.idFlujo equals f.idFlujo
                        where (i.idFlujo == vIdFlujo)
                           && (i.tipoCobertura == (int)ICRL.BD.AccesoDatos.TipoInspeccion.DaniosPropios)
                        orderby f.flujoOnBase, i.idInspeccion
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
                          i.descripcionSiniestro
                        };

      var vListaInspRoboParcial = from i in db.Inspeccion
                                  join f in db.Flujo on i.idFlujo equals f.idFlujo
                                  join irp in db.InspRoboParcial on i.idInspeccion equals irp.idInspeccion
                                  join n in db.Nomenclador on irp.idItem equals n.codigo
                                  where (n.categoriaNomenclador == "Item")
                                     && (i.idFlujo == vIdFlujo)
                                     && (i.tipoCobertura == (int)ICRL.BD.AccesoDatos.TipoInspeccion.RoboParcial)
                                  orderby i.idInspeccion
                                  select new
                                  {
                                    irp.idInspeccion,
                                    irp.idItem,
                                    n.descripcion,
                                    irp.compra,
                                    irp.instalacion,
                                    irp.pintura,
                                    irp.mecanico,
                                    irp.chaperio,
                                    irp.reparacionPrevia,
                                    irp.observaciones
                                  };




      ReportViewerInsp.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
      ReportViewerInsp.LocalReport.ReportPath = "Reportes\\RepFormularioInspRoboParcial.rdlc";
      ReportDataSource datasource1 = new ReportDataSource("DataSet1", vListaFlujo);
      ReportDataSource datasource6 = new ReportDataSource("DataSet6", vListaInspRoboParcial);

      ReportViewerInsp.LocalReport.DataSources.Clear();
      ReportViewerInsp.LocalReport.DataSources.Add(datasource1);
      ReportViewerInsp.LocalReport.DataSources.Add(datasource6);
      ReportViewerInsp.LocalReport.Refresh();
      byte[] VArrayBytes = ReportViewerInsp.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

      //enviar el array de bytes a cliente
      Response.Buffer = true;
      Response.Clear();
      Response.ContentType = mimeType;
      Response.AddHeader("content-disposition", "attachment; filename=" + fileName + "." + extension);
      Response.BinaryWrite(VArrayBytes); // create the file
      Response.Flush(); // send it to the client to download
    }

    protected void ButtonFinRoboP_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      int vIdInspeccion = 0;
      int vIdFlujo = 0;
      vIdInspeccion = int.Parse(TextBoxNroInspeccion.Text);
      vIdFlujo = int.Parse(TextBoxIdFlujo.Text);

      //proceso que copia los datos de Inps a Coti
      AccesoDatos vAccesoDatos = new AccesoDatos();
      vAccesoDatos.fCopiaRoboParcialInspACotizacion(vIdFlujo, vIdInspeccion);
      int vResultado = 0;
      vResultado = vAccesoDatos.FRoboParcialICRLCambiaEstado(vIdInspeccion);
      ButtonFinRoboP.Enabled = false;
    }

    protected void ImgButtonExportPdfDaniosP_Click(object sender, ImageClickEventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesoDatos = new AccesoDatos();
      LBCDesaEntities db = new LBCDesaEntities();

      Warning[] warnings;
      string[] streamIds;
      string mimeType = string.Empty;
      string encoding = string.Empty;
      string extension = "pdf";
      string fileName = "RepFormInspeccion";

      int vInspeccion = 0;
      int vIdFlujo = 0;
      vInspeccion = int.Parse(TextBoxNroInspeccion.Text);
      vIdFlujo = int.Parse(TextBoxIdFlujo.Text);

      var vListaFlujo = from i in db.Inspeccion
                        join f in db.Flujo on i.idFlujo equals f.idFlujo
                        where (i.idFlujo == vIdFlujo)
                           && (i.tipoCobertura == (int)ICRL.BD.AccesoDatos.TipoInspeccion.DaniosPropios)
                        orderby f.flujoOnBase, i.idInspeccion
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
                          i.descripcionSiniestro
                        };

      var vListaInspDaniosPropios = from i in db.Inspeccion
                                    join f in db.Flujo on i.idFlujo equals f.idFlujo
                                    join idpp in db.InspDaniosPropiosPadre on i.idInspeccion equals idpp.idInspeccion
                                    join idp in db.InspDaniosPropios on idpp.secuencial equals idp.secuencial
                                    join n in db.Nomenclador on idp.idItem equals n.codigo
                                    where (n.categoriaNomenclador == "Item")
                                    && (i.idFlujo == vIdFlujo)
                                    && (i.tipoCobertura == (int)ICRL.BD.AccesoDatos.TipoInspeccion.DaniosPropios)
                                    orderby i.idInspeccion, n.descripcion
                                    select new
                                    {
                                      idInspeccion = idp.secuencial,
                                      idp.idItem,
                                      n.descripcion,
                                      idp.compra,
                                      idp.instalacion,
                                      idp.pintura,
                                      idp.mecanico,
                                      idp.chaperio,
                                      idp.reparacionPrevia,
                                      idp.observaciones
                                    };



      ReportViewerInsp.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
      ReportViewerInsp.LocalReport.ReportPath = "Reportes\\RepFormularioInspDaniosPropios.rdlc";
      ReportDataSource datasource1 = new ReportDataSource("DataSet1", vListaFlujo);
      ReportDataSource datasource2 = new ReportDataSource("DataSet2", vListaInspDaniosPropios);


      ReportViewerInsp.LocalReport.DataSources.Clear();
      ReportViewerInsp.LocalReport.DataSources.Add(datasource1);
      ReportViewerInsp.LocalReport.DataSources.Add(datasource2);

      ReportViewerInsp.LocalReport.Refresh();
      byte[] VArrayBytes = ReportViewerInsp.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

      //enviar el array de bytes a cliente
      Response.Buffer = true;
      Response.Clear();
      Response.ContentType = mimeType;
      Response.AddHeader("content-disposition", "attachment; filename=" + fileName + "." + extension);
      Response.BinaryWrite(VArrayBytes); // create the file
      Response.Flush(); // send it to the client to download

    }

    #endregion

    protected void ButtonActualizaDesdeOnBase_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      int vResultado = 0;
      var vAccesoDatos = new AccesoDatos();
      var vFlujoICRL = new FlujoICRL();
      LabelMensaje.Text = string.Empty;

      vResultado = vAccesoDatos.FValidaExisteFlujoOnBase(TextBoxNroFlujo.Text);
      if (1 == vResultado)
      {
        vFlujoICRL = vAccesoDatos.FTraeDatosFlujoOnBase(TextBoxNroFlujo.Text);
        int vRespuesta = vAccesoDatos.FValidaExisteFlujoICRL(TextBoxNroFlujo.Text);
        if (0 == vRespuesta)
        {
          int vGrabacion = vAccesoDatos.FGrabaFlujoICRL(vFlujoICRL);
          if (0 == vGrabacion)
          {
            LabelMensaje.Text = "Error de Grabacion Flujo ICRL";
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
            LabelMensaje.Text = "Error de actualizacion Flujo ICRL";
          }
        }

      }
      else
      {
        LabelMensaje.Text = "Flujo No encontrado";
      }

      FlTraeDatosInspeccion(int.Parse(TextBoxNroInspeccion.Text), TextBoxNroFlujo.Text);
    }

    protected void TabContainerCoberturas_ActiveTabChanged(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      AccesoDatos vAccesoDatos = new AccesoDatos();
      int vIdInspeccion = 0;

      switch (TabContainerCoberturas.ActiveTab.TabIndex)
      {
        case 1:
          //traer datos de la inspeccion Danios Propios y actualizar valores globales
          vIdInspeccion = vAccesoDatos.FFlujoTieneDaniosPropios(int.Parse(TextBoxIdFlujo.Text));
          if (vIdInspeccion > 0)
          {
            ICRL.ModeloDB.Inspeccion vFilaInspeccion = new ICRL.ModeloDB.Inspeccion();
            vFilaInspeccion = vAccesoDatos.FTraeDatosBasicosInspeccion(vIdInspeccion);
            TextBoxCorrelativo.Text = vFilaInspeccion.correlativo.ToString();
            TextBoxNroInspeccion.Text = vFilaInspeccion.idInspeccion.ToString();
          }
          DropDownListTipoTallerInsp.Enabled = false;
          PanelDatosTaller.Visible = false;
          break;
        case 2:
          //traer datos de la inspeccion RC Objetos y actualizar valores globales
          vIdInspeccion = vAccesoDatos.FFlujoTieneRCObjetos(int.Parse(TextBoxIdFlujo.Text));
          if (vIdInspeccion > 0)
          {
            ICRL.ModeloDB.Inspeccion vFilaInspeccion = new ICRL.ModeloDB.Inspeccion();
            vFilaInspeccion = vAccesoDatos.FTraeDatosBasicosInspeccion(vIdInspeccion);
            TextBoxCorrelativo.Text = vFilaInspeccion.correlativo.ToString();
            TextBoxNroInspeccion.Text = vFilaInspeccion.idInspeccion.ToString();
          }
          DropDownListTipoTallerInsp.Enabled = false;
          PanelDatosTaller.Visible = false;
          break;
        case 3:
          //traer datos de la inspeccion RC Personas y actualizar valores globales
          vIdInspeccion = vAccesoDatos.FFlujoTieneRCPersonas(int.Parse(TextBoxIdFlujo.Text));
          if (vIdInspeccion > 0)
          {
            ICRL.ModeloDB.Inspeccion vFilaInspeccion = new ICRL.ModeloDB.Inspeccion();
            vFilaInspeccion = vAccesoDatos.FTraeDatosBasicosInspeccion(vIdInspeccion);
            TextBoxCorrelativo.Text = vFilaInspeccion.correlativo.ToString();
            TextBoxNroInspeccion.Text = vFilaInspeccion.idInspeccion.ToString();
          }
          DropDownListTipoTallerInsp.Enabled = false;
          PanelDatosTaller.Visible = false;
          break;
        case 4:
          //traer datos de la inspeccion Robo parcial y actualizar valores globales
          vIdInspeccion = vAccesoDatos.FFlujoTieneRoboParcial(int.Parse(TextBoxIdFlujo.Text));
          if (vIdInspeccion > 0)
          {
            ICRL.ModeloDB.Inspeccion vFilaInspeccion = new ICRL.ModeloDB.Inspeccion();
            vFilaInspeccion = vAccesoDatos.FTraeDatosBasicosInspeccion(vIdInspeccion);
            TextBoxCorrelativo.Text = vFilaInspeccion.correlativo.ToString();
            TextBoxNroInspeccion.Text = vFilaInspeccion.idInspeccion.ToString();
          }
          DropDownListTipoTallerInsp.Enabled = true;
          PanelDatosTaller.Visible = true;
          break;
        case 5:
          //traer datos de la inspeccion Perdida Total Danios Propios y actualizar valores globales
          vIdInspeccion = vAccesoDatos.FFlujoTienePerdidaTotDaniosPropios(int.Parse(TextBoxIdFlujo.Text));
          if (vIdInspeccion > 0)
          {
            ICRL.ModeloDB.Inspeccion vFilaInspeccion = new ICRL.ModeloDB.Inspeccion();
            vFilaInspeccion = vAccesoDatos.FTraeDatosBasicosInspeccion(vIdInspeccion);
            TextBoxCorrelativo.Text = vFilaInspeccion.correlativo.ToString();
            TextBoxNroInspeccion.Text = vFilaInspeccion.idInspeccion.ToString();
          }
          DropDownListTipoTallerInsp.Enabled = false;
          PanelDatosTaller.Visible = false;
          break;
        case 6:
          //traer datos de la inspeccion Perdida Total por Robo y actualizar valores globales
          vIdInspeccion = vAccesoDatos.FFlujoTienePerdidaTotRobo(int.Parse(TextBoxIdFlujo.Text));
          if (vIdInspeccion > 0)
          {
            ICRL.ModeloDB.Inspeccion vFilaInspeccion = new ICRL.ModeloDB.Inspeccion();
            vFilaInspeccion = vAccesoDatos.FTraeDatosBasicosInspeccion(vIdInspeccion);
            TextBoxCorrelativo.Text = vFilaInspeccion.correlativo.ToString();
            TextBoxNroInspeccion.Text = vFilaInspeccion.idInspeccion.ToString();
          }
          DropDownListTipoTallerInsp.Enabled = false;
          PanelDatosTaller.Visible = false;
          break;
        case 7:
          //traer datos de la inspeccion RC Vehicular y actualizar valores globales
          vIdInspeccion = vAccesoDatos.FFlujoTieneRCVehicular(int.Parse(TextBoxIdFlujo.Text));
          if (vIdInspeccion > 0)
          {
            ICRL.ModeloDB.Inspeccion vFilaInspeccion = new ICRL.ModeloDB.Inspeccion();
            vFilaInspeccion = vAccesoDatos.FTraeDatosBasicosInspeccion(vIdInspeccion);
            TextBoxCorrelativo.Text = vFilaInspeccion.correlativo.ToString();
            TextBoxNroInspeccion.Text = vFilaInspeccion.idInspeccion.ToString();
          }
          DropDownListTipoTallerInsp.Enabled = false;
          PanelDatosTaller.Visible = false;
          break;
        default:
          //situacion anomala no deberia generarse en ninguna condicion
          LabelMensaje.Text = "Error al seleccionar el panel";
          break;
      }

      if (vIdInspeccion > 0)
      {
        string vIdFlujo = TextBoxIdFlujo.Text;
        FlTraeDatosInspeccion(vIdInspeccion, vIdFlujo);
      }
    }

    protected void ButtonGrabarDatosInspeccion_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      int vResul = FGrabaCambiosInspeccion();
      vResul = FlTraeDatosDaniosPropios(int.Parse(TextBoxNroInspeccion.Text));
    }

    protected void ButtonFinalizarInspeccion_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      string vNombreUsuario = string.Empty;
      vNombreUsuario = Session["IdUsr"].ToString();
      string vBandejaEntrada = "REC – INSPECCION - INSPECCION PENDIENTE DE ATENCION";
      string vBandejaSalida = "REC – COTIZACION - INICIO";
      string vNumeroFlujo = TextBoxNroFlujo.Text;

      AccesoDatos vAccesoDatos = new AccesoDatos();
      int vResultado = 0;
      vResultado = vAccesoDatos.FCambiaEstadoOnBase(vNumeroFlujo, vNombreUsuario, vBandejaEntrada, vBandejaSalida);
    }
  }
}