using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LbcOnBaseWS;
using ICRL.ModeloDB;
using ICRL.BD;
using System.Data;
using System.Text;
using Microsoft.Reporting.WebForms;

namespace ICRL.Presentacion
{
  public partial class CotizacionRCVehicular : System.Web.UI.Page
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
      try
      {
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
          ButtonActualizaDesdeOnBase.Enabled = false;
          ButtonFinalizarCotizacion.Enabled = false;
          ButtonActualizaTaller.Enabled = false;
          ButtonVehActualizar.Enabled = false;
          ButtonRepaAgregarItem.Enabled = false;
          ButtonRepaGenerarResumen.Enabled = false;
          ButtonRepaCambioBenef.Enabled = false;
          ButtonRepaGenerarOrdenes.Enabled = false;
          ButtonRepuAgregarItem.Enabled = false;
          ButtonRepuGenerarResumen.Enabled = false;
          ButtonRepuCambioBenef.Enabled = false;
          ButtonRepuGenerarOrdenes.Enabled = false;
          ButtonRecepcionRepu.Enabled = false;
          ButtonRepaGrabar.Enabled = false;
          ButtonRepuGrabar.Enabled = false;
          ButtonSumaGrabar.Enabled = false;
          ButtonRecepGrabar.Enabled = false;
          ButtonBenefCambiar.Enabled = false;
        }

        int vIdCotizacion = 0;
        string vlNumFlujo = string.Empty;
        if (Request.QueryString["nroCoti"] != null)
        {
          vIdCotizacion = int.Parse(Request.QueryString["nroCoti"]);
        }


        if (Session["NumFlujo"] != null)
        {
          vlNumFlujo = Session["NumFlujo"].ToString();
          TextBoxIdFlujo.Text = Session["NumFlujo"].ToString();
        }

        if (!IsPostBack)
        {
          FlTraeNomenTipoTallerCoti();
          FlTraeDatosCotizacion(vIdCotizacion, vlNumFlujo);

          //Cargar combos de Reparaciones
          FlTraeNomenItemRepa();
          FlTraeNomenChaperioRepa();
          FlTraeNomenRepPreviaRepa();
          FlTraeNomenMonedaRepa();
          FlTraeNomenTipoDescRepa();
          FlTraeNomenProveedorRepa();

          //Cargar combos de Repuestos
          FlTraeNomenItemRepu();
          FlTraeNomenMonedaRepu();
          FlTraeNomenTipoDescRepu();
          FlTraeNomenProveedorRepu();

          //Cargar combos de Sumatorias
          FlTraeNomenProveedorSuma();
          FlTraeNomenTipoDescSuma();

          //Cargar combos de Recepcion Repuestos
          FlTraeNomenItemRecep();

          //Cargar combos de Vehicular Tercero
          FlTraeNomenVehMarca();
          FlTraeNomenVehAnio();
          FlTraeNomenVehColor();
          FlTraeNomenVehTaller();
          PCargaDatosTer();
          PModificarVehTer(false);

          int vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
          short vTipoItem = (short)CotizacionICRL.TipoItem.Reparacion;

          FlTraeDatosDPReparacion(vIdCotizacion);
          FlTraeDatosDPRepuesto(vIdCotizacion);
          FlTraeDatosSumatoriaReparaciones(vIdFlujo, vIdCotizacion, vTipoItem);

          vTipoItem = (short)CotizacionICRL.TipoItem.Repuesto;
          FlTraeDatosSumatoriaRepuestos(vIdFlujo, vIdCotizacion, vTipoItem);
          FlTraeDatosRecepRepu(vIdCotizacion);

          FLlenarGrillaOrdenes(vIdFlujo, vIdCotizacion, vTipoItem);
        }

        LabelMsjReparaciones.Text = string.Empty;
        LabelMsjRepuestos.Text = string.Empty;

        if (Session["PopupABMReparacionesHabilitado"] != null)
        {
          int vPopup = -1;
          vPopup = int.Parse(Session["PopupABMReparacionesHabilitado"].ToString());
          if (1 == vPopup)
            this.ModalPopupReparaciones.Show();
          else
            this.ModalPopupReparaciones.Hide();
        }

        if (Session["PopupABMRepuestosHabilitado"] != null)
        {
          int vPopup = -1;
          vPopup = int.Parse(Session["PopupABMRepuestosHabilitado"].ToString());
          if (1 == vPopup)
            this.ModalPopupRepuestos.Show();
          else
            this.ModalPopupRepuestos.Hide();
        }

        if (Session["PopupABMSumasHabilitado"] != null)
        {
          int vPopup = -1;
          vPopup = int.Parse(Session["PopupABMSumasHabilitado"].ToString());
          if (1 == vPopup)
            this.ModalPopupSumatorias.Show();
          else
            this.ModalPopupSumatorias.Hide();
        }

        if (Session["PopupRecepRepuHabilitado"] != null)
        {
          int vPopup = -1;
          vPopup = int.Parse(Session["PopupRecepRepuHabilitado"].ToString());
          if (1 == vPopup)
            this.ModalPopupRecepRepuestos.Show();
          else
            this.ModalPopupRecepRepuestos.Hide();
        }

        if (Session["PopupBeneficiario"] != null)
        {
          int vPopup = -1;
          vPopup = int.Parse(Session["PopupBeneficiario"].ToString());
          if (1 == vPopup)
            this.ModalPopupBeneficiario.Show();
          else
            this.ModalPopupBeneficiario.Hide();
        }

      }
      catch (Exception ex)
      {
        if (Session["MsjEstado"] != null)
        {
          Session["MsjEstado"] = string.Empty;
        }
        Session["MsjEstado"] = ex.Message;
        LabelMsjGeneral.Text = ex.Message;
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

    #region Principal formulario

    private void FlTraeDatosCotizacion(int pIdCotizacion, string pNumFlujo)
    {
      if (string.Empty == pNumFlujo)
      {
        using (LBCDesaEntities db = new LBCDesaEntities())
        {
          var vLst = from c in db.Cotizacion
                     join u in db.Usuario on c.idUsuario equals u.idUsuario
                     join f in db.Flujo on c.idFlujo equals f.idFlujo
                     join cf in db.CotizacionFlujo on c.idFlujo equals cf.idFlujo
                     where c.idCotizacion == pIdCotizacion
                     select new
                     {
                       c.idInspeccion,
                       f.causaSiniestro,
                       f.descripcionSiniestro,
                       zona = "",
                       cf.observacionesSiniestro,
                       c.sucursal,
                       cf.fecha_siniestro,
                       cf.nombreContacto,
                       cf.telefonoContacto,
                       c.correlativo,
                       u.nombreVisible,
                       u.correoElectronico
                     };
          var vFilaTabla = vLst.FirstOrDefault();

          if (null != vFilaTabla)
          {
            TextBoxNroCotizacion.Text = vFilaTabla.idInspeccion.ToString();
            TextBoxCorrelativo.Text = vFilaTabla.correlativo.ToString();
            TextBoxSucAtencion.Text = vFilaTabla.sucursal;
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
          var vLst = from c in db.Cotizacion
                     join u in db.Usuario on c.idUsuario equals u.idUsuario
                     join f in db.Flujo on c.idFlujo equals f.idFlujo
                     //join cdp in db.CotiDaniosPropios on c.idCotizacion equals cdp.idCotizacion
                     join cf in db.CotizacionFlujo on c.idFlujo equals cf.idFlujo
                     where c.idCotizacion == pIdCotizacion
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
                       c.idCotizacion,
                       f.descripcionSiniestro,
                       f.direccionInspeccion,
                       f.agenciaAtencion,
                       zona = "",
                       cf.observacionesSiniestro,
                       c.sucursal,
                       cf.fecha_siniestro,
                       cf.nombreContacto,
                       cf.telefonoContacto,
                       cf.correosDeEnvio,
                       c.correlativo,
                       c.tipoTaller,
                       //tipoTaller = "Taller Tipo B",
                       u.nombreVisible,
                       u.correoElectronico,
                     };
          var vFilaTabla = vLst.FirstOrDefault();

          if (null != vFilaTabla)
          {
            TextBoxNroFlujo.Text = vFilaTabla.flujoOnBase;
            TextBoxNroCotizacion.Text = vFilaTabla.idCotizacion.ToString();
            TextBoxCorrelativo.Text = vFilaTabla.correlativo.ToString();
            TextBoxNroReclamo.Text = vFilaTabla.numeroReclamo.ToString();
            TextBoxSucAtencion.Text = vFilaTabla.agenciaAtencion.Trim();
            TextBoxDirecInspeccion.Text = vFilaTabla.direccionInspeccion.Trim();
            TextBoxCausaSiniestro.Text = vFilaTabla.causaSiniestro.Trim();
            TextBoxDescripSiniestro.Text = vFilaTabla.descripcionSiniestro.Trim();
            TextBoxObservacionesInspec.Text = vFilaTabla.observacionesSiniestro.Trim();
            TextBoxNombreAsegurado.Text = vFilaTabla.nombreAsegurado;
            TextBoxTelefonoAsegurado.Text = vFilaTabla.telefonocelAsegurado;
            TextBoxNombreInspector.Text = vFilaTabla.nombreVisible;
            TextBoxCorreoInspector.Text = vFilaTabla.correoElectronico;
            TextBoxNombreContacto.Text = vFilaTabla.nombreContacto.Trim();
            TextBoxMarca.Text = vFilaTabla.marcaVehiculo;
            TextBoxModelo.Text = vFilaTabla.modeloVehiculo;
            TextBoxPlaca.Text = vFilaTabla.placaVehiculo;
            TextBoxColor.Text = vFilaTabla.colorVehiculo;
            TextBoxNroChasis.Text = vFilaTabla.chasisVehiculo;
            TextBoxAnio.Text = vFilaTabla.anioVehiculo.ToString();
            TextBoxValorAsegurado.Text = vFilaTabla.valorAsegurado.ToString();

            string vTempoCadena = string.Empty;
            vTempoCadena = vFilaTabla.tipoTaller.Trim();
            DropDownListTipoTallerCoti.ClearSelection();
            DropDownListTipoTallerCoti.Items.FindByText(vTempoCadena).Selected = true;
          }

        }
      }
    }

    private int FlTraeNomenTipoTallerCoti()
    {
      int vResultado = 0;
      string vCategoria = "Tipo Taller";
      int vOrdenCodigo = 2;
      AccesoDatos vAccesoDatos = new AccesoDatos();

      DropDownListTipoTallerCoti.DataValueField = "codigo";
      DropDownListTipoTallerCoti.DataTextField = "descripcion";
      DropDownListTipoTallerCoti.DataSource = vAccesoDatos.FlTraeNomenGenerico(vCategoria, vOrdenCodigo);
      DropDownListTipoTallerCoti.DataBind();

      return vResultado;
    }

    private int FlTraeDatosDPReparacion(int pIdCotizacion)
    {
      int vResultado = 0;
      int vIdFlujo = int.Parse(TextBoxIdFlujo.Text);

      BD.CotizacionICRL.TipoRCVehicularTraer vTipoRCVehicularTraer;
      vTipoRCVehicularTraer = CotizacionICRL.RCVehicularesTraer(vIdFlujo, pIdCotizacion);

      GridViewReparaciones.DataSource = vTipoRCVehicularTraer.RCVehiculares.Select(RCVehiculares => new
      {
        RCVehiculares.id_item,
        RCVehiculares.item_descripcion,
        RCVehiculares.chaperio,
        RCVehiculares.reparacion_previa,
        RCVehiculares.mecanico,
        RCVehiculares.id_moneda,
        RCVehiculares.precio_cotizado,
        RCVehiculares.id_tipo_descuento,
        RCVehiculares.descuento,
        RCVehiculares.precio_final,
        RCVehiculares.proveedor,
        RCVehiculares.id_tipo_item

      }).Where(RCVehiculares => RCVehiculares.id_tipo_item == 1).ToList();
      GridViewReparaciones.DataBind();

      return vResultado;
    }

    private int FlTraeDatosDPRepuesto(int pIdCotizacion)
    {
      int vResultado = 0;
      int vIdFlujo = int.Parse(TextBoxIdFlujo.Text);

      BD.CotizacionICRL.TipoRCVehicularTraer vTipoRCVehicularTraer;
      vTipoRCVehicularTraer = CotizacionICRL.RCVehicularesTraer(vIdFlujo, pIdCotizacion);

      GridViewRepuestos.DataSource = vTipoRCVehicularTraer.RCVehiculares.Select(RCVehiculares => new
      {
        RCVehiculares.id_item,
        RCVehiculares.item_descripcion,
        RCVehiculares.pintura,
        RCVehiculares.instalacion,
        RCVehiculares.id_moneda,
        RCVehiculares.precio_cotizado,
        RCVehiculares.id_tipo_descuento,
        RCVehiculares.descuento,
        RCVehiculares.precio_final,
        RCVehiculares.proveedor,
        RCVehiculares.id_tipo_item

      }).Where(RCVehiculares => RCVehiculares.id_tipo_item == 2).ToList();
      GridViewRepuestos.DataBind();

      return vResultado;
    }

    #endregion

    #region ABMReparaciones

    private int FlTraeNomenItemRepa()
    {
      int vResultado = 0;
      string vCategoria = "Item";
      int vOrdenCodigo = 2;
      AccesoDatos vAccesoDatos = new AccesoDatos();

      DropDownListRepaItem.DataValueField = "codigo";
      DropDownListRepaItem.DataTextField = "descripcion";
      DropDownListRepaItem.DataSource = vAccesoDatos.FlTraeNomenGenerico(vCategoria, vOrdenCodigo);
      DropDownListRepaItem.DataBind();

      return vResultado;
    }

    private int FlTraeNomenChaperioRepa()
    {
      int vResultado = 0;
      string vCategoria = "Nivel de Daño";
      int vOrdenCodigo = 2;
      AccesoDatos vAccesoDatos = new AccesoDatos();

      DropDownListRepaChaperio.DataValueField = "codigo";
      DropDownListRepaChaperio.DataTextField = "descripcion";
      DropDownListRepaChaperio.DataSource = vAccesoDatos.FlTraeNomenGenerico(vCategoria, vOrdenCodigo);
      DropDownListRepaChaperio.DataBind();

      return vResultado;
    }

    private int FlTraeNomenRepPreviaRepa()
    {
      int vResultado = 0;
      string vCategoria = "Nivel de Daño";
      int vOrdenCodigo = 2;
      AccesoDatos vAccesoDatos = new AccesoDatos();

      DropDownListRepaRepPrevia.DataValueField = "codigo";
      DropDownListRepaRepPrevia.DataTextField = "descripcion";
      DropDownListRepaRepPrevia.DataSource = vAccesoDatos.FlTraeNomenGenerico(vCategoria, vOrdenCodigo);
      DropDownListRepaRepPrevia.DataBind();

      return vResultado;
    }

    private int FlTraeNomenMonedaRepa()
    {
      int vResultado = 0;
      string vCategoria = "Moneda";
      int vOrdenCodigo = 2;
      AccesoDatos vAccesoDatos = new AccesoDatos();

      DropDownListRepaMoneda.DataValueField = "codigo";
      DropDownListRepaMoneda.DataTextField = "descripcion";
      DropDownListRepaMoneda.DataSource = vAccesoDatos.FlTraeNomenGenerico(vCategoria, vOrdenCodigo);
      DropDownListRepaMoneda.DataBind();

      return vResultado;
    }

    private int FlTraeNomenTipoDescRepa()
    {
      int vResultado = 0;
      string vCategoria = "Tipo Descuento";
      int vOrdenCodigo = 2;
      AccesoDatos vAccesoDatos = new AccesoDatos();

      DropDownListRepaTipoDesc.DataValueField = "codigo";
      DropDownListRepaTipoDesc.DataTextField = "descripcion";
      DropDownListRepaTipoDesc.DataSource = vAccesoDatos.FlTraeNomenGenerico(vCategoria, vOrdenCodigo);
      DropDownListRepaTipoDesc.DataBind();

      return vResultado;
    }

    private int FlTraeNomenProveedorRepa()
    {
      int vResultado = 0;
      string vCategoria = "Proveedor";
      int vOrdenCodigo = 2;
      AccesoDatos vAccesoDatos = new AccesoDatos();

      DropDownListRepaProveedor.DataValueField = "codigo";
      DropDownListRepaProveedor.DataTextField = "descripcion";
      DropDownListRepaProveedor.DataSource = vAccesoDatos.FlTraeNomenGenerico(vCategoria, vOrdenCodigo);
      DropDownListRepaProveedor.DataBind();

      return vResultado;
    }

    protected void PLimpiarCamposRepa()
    {
      DropDownListRepaItem.SelectedIndex = 0;
      DropDownListRepaChaperio.SelectedIndex = 0;
      DropDownListRepaRepPrevia.SelectedIndex = 0;
      CheckBoxRepaMecanico.Checked = false;
      DropDownListRepaMoneda.SelectedIndex = 0;
      TextBoxRepaPrecioCotizado.Text = "0";
      DropDownListRepaTipoDesc.SelectedIndex = 0;
      TextBoxRepaMontoDesc.Text = "0";
      TextBoxRepaPrecioFinal.Text = "0";
      DropDownListRepaProveedor.SelectedIndex = 0;
    }

    protected void ButtonRepaAgregarItem_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      TextBoxRepaIdItem.Text = string.Empty;
      DropDownListRepaItem.Enabled = true;
      //PanelABMReparaciones.Enabled = true;
      ButtonRepaGrabar.Enabled = true;
      ButtonRepaCancelar.Enabled = true;
      TextBoxRepaFlagEd.Text = "A";
      DropDownListRepaItem.Visible = true;
      TextBoxRepaItem.Visible = false;
      Session["PopupABMReparacionesHabilitado"] = 1;
      this.ModalPopupReparaciones.Show();
    }

    protected void PRepaModificarItem()
    {
      //PanelABMReparaciones.Enabled = true;
      DropDownListRepaItem.Enabled = false;
      DropDownListRepaItem.Visible = false;
      TextBoxRepaItem.Visible = true;
      ButtonRepaGrabar.Enabled = true;
      ButtonRepaCancelar.Enabled = true;
    }

    protected void ButtonRepaGrabar_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      LabelRepaRegistroItems.Text = "Items";
      BD.CotizacionICRL.TipoRCVehicular vTipoRCVehicular = new CotizacionICRL.TipoRCVehicular();

      //Completar los elementos del objeto y grabar el registro.
      vTipoRCVehicular.id_flujo = int.Parse(TextBoxIdFlujo.Text);
      vTipoRCVehicular.id_cotizacion = int.Parse(TextBoxNroCotizacion.Text);

      //tipo_item:  1 = Reparacion  2 = Repuesto
      vTipoRCVehicular.id_tipo_item = (int)CotizacionICRL.TipoItem.Reparacion;
      if ("A" == TextBoxRepaFlagEd.Text)
      {
        vTipoRCVehicular.item_descripcion = DropDownListRepaItem.SelectedItem.Text.Trim();
      }
      else
      {
        vTipoRCVehicular.item_descripcion = TextBoxRepaItem.Text;
      }
      vTipoRCVehicular.chaperio = DropDownListRepaChaperio.SelectedItem.Text.Trim();
      vTipoRCVehicular.reparacion_previa = DropDownListRepaRepPrevia.SelectedItem.Text.Trim();
      vTipoRCVehicular.mecanico = CheckBoxRepaMecanico.Checked;
      vTipoRCVehicular.id_moneda = DropDownListRepaMoneda.SelectedItem.Text.Trim();
      vTipoRCVehicular.precio_cotizado = double.Parse(TextBoxRepaPrecioCotizado.Text);
      vTipoRCVehicular.id_tipo_descuento = DropDownListRepaTipoDesc.SelectedItem.Text.Trim();
      vTipoRCVehicular.descuento = double.Parse(TextBoxRepaMontoDesc.Text);
      switch (vTipoRCVehicular.id_tipo_descuento)
      {
        case "Fijo":
          vTipoRCVehicular.precio_final = vTipoRCVehicular.precio_cotizado - vTipoRCVehicular.descuento;
          break;
        case "Porcentaje":
          vTipoRCVehicular.precio_final = vTipoRCVehicular.precio_cotizado - (vTipoRCVehicular.precio_cotizado * (vTipoRCVehicular.descuento / 100));
          break;
        default:
          vTipoRCVehicular.precio_final = vTipoRCVehicular.precio_cotizado;
          break;
      }
      vTipoRCVehicular.proveedor = DropDownListRepaProveedor.SelectedItem.Text.Trim();
      vTipoRCVehicular.id_estado = 1;

      double vTipoCambio = 0;
      vTipoCambio = double.Parse(TextBoxTipoCambio.Text);
      vTipoRCVehicular.tipo_cambio = vTipoCambio;

      bool vResultado = false;
      if (string.Empty != TextBoxRepaIdItem.Text)
      {
        vTipoRCVehicular.id_item = int.Parse(TextBoxRepaIdItem.Text);
        vResultado = BD.CotizacionICRL.RCVehicularesModificar(vTipoRCVehicular);
        if (vResultado)
        {
          LabelRepaRegistroItems.Text = "Registro modificado exitosamente";
          PLimpiarCamposRepa();
          ButtonRepaGrabar.Enabled = false;
          ButtonRepaCancelar.Enabled = false;
          DropDownListRepaItem.Visible = true;
          TextBoxRepaItem.Visible = false;
          //Cerrar el popup cuando se ejcute una modificacion exitosa
          Session["PopupABMReparacionesHabilitado"] = 0;
          this.ModalPopupReparaciones.Hide();
        }
        else
        {
          LabelRepaRegistroItems.Text = "El Registro no pudo ser modificado";
        }
      }
      else
      {
        vResultado = BD.CotizacionICRL.RCVehicularRegistrar(vTipoRCVehicular);
        if (vResultado)
        {
          LabelRepaRegistroItems.Text = "Registro añadido exitosamente";
          PLimpiarCamposRepa();

          ButtonRepaGrabar.Enabled = true;
          ButtonRepaCancelar.Enabled = true;
          DropDownListRepaItem.Visible = true;
          TextBoxRepaItem.Visible = false;
        }
        else
        {
          LabelRepaRegistroItems.Text = "El Registro no pudo ser añadido";
        }
      }

      int vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);

      FlTraeDatosDPReparacion(vIdCotizacion);

    }

    protected void ButtonRepaCancelar_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      ButtonRepaGrabar.Enabled = false;
      ButtonRepaCancelar.Enabled = false;
      PLimpiarCamposRepa();
      //Cerrar el popup cuando se ejcute una cancelación de alta o Modificacion
      Session["PopupABMReparacionesHabilitado"] = 0;
      this.ModalPopupReparaciones.Hide();
    }

    protected void GridViewReparaciones_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
      if (!VerificarPagina(true)) return;
      bool vResultado = false;
      int vIdFlujo = 0;
      int vIdCotizacion = 0;
      long vIdItem = 0;
      int vIndex = 0;

      vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
      vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);
      vIndex = e.RowIndex;
      vIdItem = long.Parse(GridViewReparaciones.Rows[vIndex].Cells[1].Text);
      vResultado = BD.CotizacionICRL.RCVehicularBorrar(vIdFlujo, vIdCotizacion, vIdItem);
      if (vResultado)
      {
        LabelRepaRegistroItems.Text = "Registro Borrado exitosamente";
        PLimpiarCamposRepa();
        ButtonRepaGrabar.Enabled = false;
        ButtonRepaCancelar.Enabled = false;
      }
      else
      {
        LabelRepaRegistroItems.Text = "El Registro no pudo ser Borrado";
      }

      FlTraeDatosDPReparacion(vIdCotizacion);
    }

    protected void GridViewReparaciones_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      string vTextoTemporal = string.Empty;

      //Leer Registro de la grilla y cargar los valores a la ventana.
      TextBoxRepaIdItem.Text = GridViewReparaciones.SelectedRow.Cells[1].Text.Trim();
      //tipo_item:  1 = Reparacion  2 = Repuesto
      vTextoTemporal = string.Empty;
      vTextoTemporal = GridViewReparaciones.SelectedRow.Cells[2].Text.Trim();
      vTextoTemporal = vTextoTemporal.Replace("&#209;", "Ñ");
      vTextoTemporal = vTextoTemporal.Replace("&nbsp;", string.Empty);
      //cuando se modifica ya no se utiliza el combo
      //DropDownListRepaItem.ClearSelection();
      //DropDownListRepaItem.Items.FindByText(vTextoTemporal).Selected = true;
      TextBoxRepaItem.Text = vTextoTemporal;

      vTextoTemporal = string.Empty;
      vTextoTemporal = GridViewReparaciones.SelectedRow.Cells[3].Text.Trim();
      vTextoTemporal = vTextoTemporal.Replace("&#209;", "Ñ");
      vTextoTemporal = vTextoTemporal.Replace("&nbsp;", string.Empty);
      DropDownListRepaChaperio.ClearSelection();
      DropDownListRepaChaperio.Items.FindByText(vTextoTemporal).Selected = true;

      vTextoTemporal = string.Empty;
      vTextoTemporal = GridViewReparaciones.SelectedRow.Cells[4].Text.Trim();
      vTextoTemporal = vTextoTemporal.Replace("&#209;", "Ñ");
      vTextoTemporal = vTextoTemporal.Replace("&nbsp;", string.Empty);
      DropDownListRepaRepPrevia.ClearSelection();
      DropDownListRepaRepPrevia.Items.FindByText(vTextoTemporal).Selected = true;

      CheckBoxRepaMecanico.Checked = (GridViewReparaciones.SelectedRow.Cells[5].Controls[1] as CheckBox).Checked;

      vTextoTemporal = string.Empty;
      vTextoTemporal = GridViewReparaciones.SelectedRow.Cells[6].Text;

      DropDownListRepaMoneda.ClearSelection();
      DropDownListRepaMoneda.Items.FindByText(vTextoTemporal).Selected = true;

      float vMontoTemp = float.Parse(GridViewReparaciones.SelectedRow.Cells[7].Text);
      TextBoxRepaPrecioCotizado.Text = vMontoTemp.ToString();

      vTextoTemporal = string.Empty;
      vTextoTemporal = GridViewReparaciones.SelectedRow.Cells[8].Text.Trim();
      vTextoTemporal = vTextoTemporal.Replace("&#209;", "Ñ");
      vTextoTemporal = vTextoTemporal.Replace("&nbsp;", string.Empty);
      DropDownListRepaTipoDesc.ClearSelection();
      DropDownListRepaTipoDesc.Items.FindByText(vTextoTemporal).Selected = true;

      vMontoTemp = float.Parse(GridViewReparaciones.SelectedRow.Cells[9].Text);
      TextBoxRepaMontoDesc.Text = vMontoTemp.ToString();

      vMontoTemp = float.Parse(GridViewReparaciones.SelectedRow.Cells[10].Text);
      TextBoxRepaPrecioFinal.Text = vMontoTemp.ToString();

      vTextoTemporal = string.Empty;
      vTextoTemporal = GridViewReparaciones.SelectedRow.Cells[11].Text.Trim();
      vTextoTemporal = vTextoTemporal.Replace("&#209;", "Ñ");
      vTextoTemporal = vTextoTemporal.Replace("&nbsp;", string.Empty);
      DropDownListRepaProveedor.ClearSelection();
      DropDownListRepaProveedor.Items.FindByText(vTextoTemporal).Selected = true;
      TextBoxRepaFlagEd.Text = "M";

      PRepaModificarItem();
      Session["PopupABMReparacionesHabilitado"] = 1;
      this.ModalPopupReparaciones.Show();
    }

    protected void ButtonCancelPopReparaciones_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      ButtonRepaGrabar.Enabled = false;
      ButtonRepaCancelar.Enabled = false;
      PLimpiarCamposRepa();

      Session["PopupABMReparacionesHabilitado"] = 0;
      this.ModalPopupReparaciones.Hide();
    }

    #endregion

    #region ABMRepuestos

    private int FlTraeNomenItemRepu()
    {
      int vResultado = 0;
      string vCategoria = "Item";
      int vOrdenCodigo = 2;
      AccesoDatos vAccesoDatos = new AccesoDatos();

      DropDownListRepuItem.DataValueField = "codigo";
      DropDownListRepuItem.DataTextField = "descripcion";
      DropDownListRepuItem.DataSource = vAccesoDatos.FlTraeNomenGenerico(vCategoria, vOrdenCodigo);
      DropDownListRepuItem.DataBind();

      return vResultado;
    }

    private int FlTraeNomenMonedaRepu()
    {
      int vResultado = 0;
      string vCategoria = "Moneda";
      int vOrdenCodigo = 2;
      AccesoDatos vAccesoDatos = new AccesoDatos();

      DropDownListRepuMoneda.DataValueField = "codigo";
      DropDownListRepuMoneda.DataTextField = "descripcion";
      DropDownListRepuMoneda.DataSource = vAccesoDatos.FlTraeNomenGenerico(vCategoria, vOrdenCodigo);
      DropDownListRepuMoneda.DataBind();

      return vResultado;
    }

    private int FlTraeNomenTipoDescRepu()
    {
      int vResultado = 0;
      string vCategoria = "Tipo Descuento";
      int vOrdenCodigo = 2;
      AccesoDatos vAccesoDatos = new AccesoDatos();

      DropDownListRepuTipoDesc.DataValueField = "codigo";
      DropDownListRepuTipoDesc.DataTextField = "descripcion";
      DropDownListRepuTipoDesc.DataSource = vAccesoDatos.FlTraeNomenGenerico(vCategoria, vOrdenCodigo);
      DropDownListRepuTipoDesc.DataBind();

      return vResultado;
    }

    private int FlTraeNomenProveedorRepu()
    {
      int vResultado = 0;
      string vCategoria = "Proveedor";
      int vOrdenCodigo = 2;
      AccesoDatos vAccesoDatos = new AccesoDatos();

      DropDownListRepuProveedor.DataValueField = "codigo";
      DropDownListRepuProveedor.DataTextField = "descripcion";
      DropDownListRepuProveedor.DataSource = vAccesoDatos.FlTraeNomenGenerico(vCategoria, vOrdenCodigo);
      DropDownListRepuProveedor.DataBind();

      return vResultado;
    }

    protected void PLimpiarCamposRepu()
    {
      DropDownListRepuItem.SelectedIndex = 0;
      CheckBoxRepuPintura.Checked = false;
      CheckBoxRepuInstalacion.Checked = false;
      DropDownListRepuMoneda.SelectedIndex = 0;
      TextBoxRepuPrecioCotizado.Text = "0";
      DropDownListRepuTipoDesc.SelectedIndex = 0;
      TextBoxRepuMontoDesc.Text = "0";
      TextBoxRepuPrecioFinal.Text = "0";
      DropDownListRepuProveedor.SelectedIndex = 0;
    }

    protected void ButtonRepuAgregarItem_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      TextBoxRepuIdItem.Text = string.Empty;
      DropDownListRepuItem.Enabled = true;
      //PanelABMRepuestos.Enabled = true;
      ButtonRepuGrabar.Enabled = true;
      ButtonRepuCancelar.Enabled = true;
      TextBoxRepuFlagEd.Text = "A";
      DropDownListRepuItem.Visible = true;
      TextBoxRepuItem.Visible = false;
      Session["PopupABMRepuestosHabilitado"] = 1;
      this.ModalPopupRepuestos.Show();
    }

    protected void PRepuModificarItem()
    {
      //PanelABMRepuestos.Enabled = true;
      DropDownListRepuItem.Enabled = false;
      DropDownListRepuItem.Visible = false;
      TextBoxRepuItem.Visible = true;
      ButtonRepuGrabar.Enabled = true;
      ButtonRepuCancelar.Enabled = true;
    }

    protected void ButtonRepuGrabar_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      LabelRepuRegistroItems.Text = "Items";
      BD.CotizacionICRL.TipoRCVehicular vTipoRCVehicular = new CotizacionICRL.TipoRCVehicular();

      //Completar los elementos del objeto y grabar el registro.
      vTipoRCVehicular.id_flujo = int.Parse(TextBoxIdFlujo.Text);
      vTipoRCVehicular.id_cotizacion = int.Parse(TextBoxNroCotizacion.Text);

      //tipo_item:  1 = Repuracion  2 = Repuesto
      vTipoRCVehicular.id_tipo_item = (int)CotizacionICRL.TipoItem.Repuesto;
      if ("A" == TextBoxRepuFlagEd.Text)
      {
        vTipoRCVehicular.item_descripcion = DropDownListRepuItem.SelectedItem.Text.Trim();
      }
      else
      {
        vTipoRCVehicular.item_descripcion = TextBoxRepuItem.Text;
      }
      vTipoRCVehicular.pintura = CheckBoxRepuPintura.Checked;
      vTipoRCVehicular.instalacion = CheckBoxRepuInstalacion.Checked;
      vTipoRCVehicular.id_moneda = DropDownListRepuMoneda.SelectedItem.Text.Trim();
      vTipoRCVehicular.precio_cotizado = double.Parse(TextBoxRepuPrecioCotizado.Text);
      vTipoRCVehicular.id_tipo_descuento = DropDownListRepuTipoDesc.SelectedItem.Text.Trim();
      vTipoRCVehicular.descuento = double.Parse(TextBoxRepuMontoDesc.Text);
      switch (vTipoRCVehicular.id_tipo_descuento)
      {
        case "Fijo":
          vTipoRCVehicular.precio_final = vTipoRCVehicular.precio_cotizado - vTipoRCVehicular.descuento;
          break;
        case "Porcentaje":
          vTipoRCVehicular.precio_final = vTipoRCVehicular.precio_cotizado - (vTipoRCVehicular.precio_cotizado * (vTipoRCVehicular.descuento / 100));
          break;
        default:
          vTipoRCVehicular.precio_final = vTipoRCVehicular.precio_cotizado;
          break;
      }
      vTipoRCVehicular.proveedor = DropDownListRepuProveedor.SelectedItem.Text.Trim();
      vTipoRCVehicular.id_estado = 1;

      double vTipoCambio = 0;
      vTipoCambio = double.Parse(TextBoxTipoCambio.Text);
      vTipoRCVehicular.tipo_cambio = vTipoCambio;

      bool vResultado = false;
      if (string.Empty != TextBoxRepuIdItem.Text)
      {
        vTipoRCVehicular.id_item = int.Parse(TextBoxRepuIdItem.Text);
        vResultado = BD.CotizacionICRL.RCVehicularesModificar(vTipoRCVehicular);
        if (vResultado)
        {
          LabelRepuRegistroItems.Text = "Registro modificado exitosamente";
          PLimpiarCamposRepu();
          //PanelABMRepuestos.Enabled = false;
          ButtonRepuGrabar.Enabled = false;
          ButtonRepuCancelar.Enabled = false;
          DropDownListRepuItem.Visible = true;
          TextBoxRepuItem.Visible = false;
          //Cerrar el popup cuando se ejcute una modificacion exitosa
          Session["PopupABMRepuestosHabilitado"] = 0;
          this.ModalPopupRepuestos.Hide();
        }
        else
        {
          LabelRepuRegistroItems.Text = "El Registro no pudo ser modificado";
        }
      }
      else
      {
        vResultado = BD.CotizacionICRL.RCVehicularRegistrar(vTipoRCVehicular);
        if (vResultado)
        {
          LabelRepuRegistroItems.Text = "Registro añadido exitosamente";
          PLimpiarCamposRepu();
          //PanelABMRepuestos.Enabled = false;
          ButtonRepuGrabar.Enabled = true;
          ButtonRepuCancelar.Enabled = true;
          DropDownListRepuItem.Visible = true;
          TextBoxRepuItem.Visible = false;
        }
        else
        {
          LabelRepuRegistroItems.Text = "El Registro no pudo ser añadido";
        }
      }

      int vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);

      FlTraeDatosDPRepuesto(vIdCotizacion);

    }

    protected void ButtonRepuCancelar_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      //PanelABMRepuestos.Enabled = false;
      ButtonRepuGrabar.Enabled = false;
      ButtonRepuCancelar.Enabled = false;
      PLimpiarCamposRepu();
      //Cerrar el popup cuando se ejecute una cancelación de alta o Modificacion
      Session["PopupABMRepuestosHabilitado"] = 0;
      this.ModalPopupRepuestos.Hide();
    }

    protected void GridViewRepuestos_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
      if (!VerificarPagina(true)) return;
      bool vResultado = false;
      int vIdFlujo = 0;
      int vIdCotizacion = 0;
      long vIdItem = 0;
      int vIndex = 0;

      vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
      vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);
      vIndex = e.RowIndex;
      vIdItem = long.Parse(GridViewRepuestos.Rows[vIndex].Cells[1].Text);
      vResultado = BD.CotizacionICRL.RCVehicularBorrar(vIdFlujo, vIdCotizacion, vIdItem);
      if (vResultado)
      {
        LabelRepuRegistroItems.Text = "Registro Borrado exitosamente";
        PLimpiarCamposRepu();
        //PanelABMRepuestos.Enabled = false;
        ButtonRepuGrabar.Enabled = false;
        ButtonRepuCancelar.Enabled = false;
      }
      else
      {
        LabelRepuRegistroItems.Text = "El Registro no pudo ser Borrado";
      }

      FlTraeDatosDPRepuesto(vIdCotizacion);
    }

    protected void GridViewRepuestos_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      string vTextoTemporal = string.Empty;

      //Leer Registro de la grilla y cargar los valores a la ventana.
      TextBoxRepuIdItem.Text = GridViewRepuestos.SelectedRow.Cells[1].Text.Trim();
      //tipo_item:  1 = Repuracion  2 = Repuesto
      vTextoTemporal = string.Empty;
      vTextoTemporal = GridViewRepuestos.SelectedRow.Cells[2].Text;
      vTextoTemporal = vTextoTemporal.Replace("&#209;", "Ñ");
      vTextoTemporal = vTextoTemporal.Replace("&nbsp;", string.Empty);
      //Cuando se modifica ya no se utiliza el combo
      //DropDownListRepuItem.ClearSelection();
      //DropDownListRepuItem.Items.FindByText(vTextoTemporal).Selected = true;
      TextBoxRepuItem.Text = vTextoTemporal;

      CheckBoxRepuPintura.Checked = (GridViewRepuestos.SelectedRow.Cells[3].Controls[1] as CheckBox).Checked;

      CheckBoxRepuInstalacion.Checked = (GridViewRepuestos.SelectedRow.Cells[4].Controls[1] as CheckBox).Checked;

      vTextoTemporal = string.Empty;
      vTextoTemporal = GridViewRepuestos.SelectedRow.Cells[5].Text.Trim();
      vTextoTemporal = vTextoTemporal.Replace("&#209;", "Ñ");
      vTextoTemporal = vTextoTemporal.Replace("&nbsp;", string.Empty);
      DropDownListRepuMoneda.ClearSelection();
      DropDownListRepuMoneda.Items.FindByText(vTextoTemporal).Selected = true;

      float vMontoTemp = float.Parse(GridViewRepuestos.SelectedRow.Cells[6].Text);
      TextBoxRepuPrecioCotizado.Text = vMontoTemp.ToString();

      vTextoTemporal = string.Empty;
      vTextoTemporal = GridViewRepuestos.SelectedRow.Cells[7].Text.Trim();
      vTextoTemporal = vTextoTemporal.Replace("&#209;", "Ñ");
      vTextoTemporal = vTextoTemporal.Replace("&nbsp;", string.Empty);
      DropDownListRepuTipoDesc.ClearSelection();
      DropDownListRepuTipoDesc.Items.FindByText(vTextoTemporal).Selected = true;

      vMontoTemp = float.Parse(GridViewRepuestos.SelectedRow.Cells[8].Text);
      TextBoxRepuMontoDesc.Text = vMontoTemp.ToString();

      vMontoTemp = float.Parse(GridViewRepuestos.SelectedRow.Cells[9].Text);
      TextBoxRepuPrecioFinal.Text = vMontoTemp.ToString();

      vTextoTemporal = string.Empty;
      vTextoTemporal = GridViewRepuestos.SelectedRow.Cells[10].Text.Trim();
      vTextoTemporal = vTextoTemporal.Replace("&#209;", "Ñ");
      vTextoTemporal = vTextoTemporal.Replace("&nbsp;", string.Empty);
      DropDownListRepuProveedor.ClearSelection();
      DropDownListRepuProveedor.Items.FindByText(vTextoTemporal).Selected = true;
      TextBoxRepuFlagEd.Text = "M";

      PRepuModificarItem();
      Session["PopupABMRepuestosHabilitado"] = 1;
      this.ModalPopupRepuestos.Show();
    }

    protected void ButtonCancelPopRepuestos_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      ButtonRepuGrabar.Enabled = false;
      ButtonRepuCancelar.Enabled = false;
      PLimpiarCamposRepu();

      Session["PopupABMRepuestosHabilitado"] = 0;
      this.ModalPopupRepuestos.Hide();
    }

    #endregion

    #region SumaReparaciones

    private int FlTraeNomenProveedorSuma()
    {
      int vResultado = 0;
      string vCategoria = "Proveedor";
      int vOrdenCodigo = 2;
      AccesoDatos vAccesoDatos = new AccesoDatos();

      DropDownListSumaProveedor.DataValueField = "codigo";
      DropDownListSumaProveedor.DataTextField = "descripcion";
      DropDownListSumaProveedor.DataSource = vAccesoDatos.FlTraeNomenGenerico(vCategoria, vOrdenCodigo);
      DropDownListSumaProveedor.DataBind();

      return vResultado;
    }

    private int FlTraeNomenTipoDescSuma()
    {
      int vResultado = 0;
      string vCategoria = "Tipo Descuento";
      int vOrdenCodigo = 2;
      AccesoDatos vAccesoDatos = new AccesoDatos();

      DropDownListSumaTipoDesc.DataValueField = "codigo";
      DropDownListSumaTipoDesc.DataTextField = "descripcion";
      DropDownListSumaTipoDesc.DataSource = vAccesoDatos.FlTraeNomenGenerico(vCategoria, vOrdenCodigo);
      DropDownListSumaTipoDesc.DataBind();

      return vResultado;
    }

    protected void PSumaModificarItem(short pTipoItem)
    {
      //PanelABMReparaciones.Enabled = true;
      DropDownListSumaProveedor.Enabled = false;
      if ((short)CotizacionICRL.TipoItem.Repuesto == pTipoItem)
      {
        TextBoxSumaDeducible.Enabled = false;
      }
      ButtonSumaGrabar.Enabled = true;
      ButtonSumaCancelar.Enabled = true;
    }

    private int FlTraeDatosSumatoriaReparaciones(int pIdFlujo, int pIdCotizacion, short pTipoItem)
    {
      int vResultado = 0;

      BD.CotizacionICRL.TipoRCVehicularSumatoriaTraer vTipoVehicularSumatoriaTraer;
      vTipoVehicularSumatoriaTraer = CotizacionICRL.RCVehicularSumatoriaTraer(pIdFlujo, pIdCotizacion, pTipoItem);

      GridViewSumaReparaciones.DataSource = vTipoVehicularSumatoriaTraer.RCVehicularesSumatoria.Select(RCVehicularesSumatoria => new
      {
        RCVehicularesSumatoria.proveedor,
        RCVehicularesSumatoria.monto_orden,
        RCVehicularesSumatoria.id_tipo_descuento_orden,
        RCVehicularesSumatoria.descuento_proveedor,
        RCVehicularesSumatoria.deducible,
        RCVehicularesSumatoria.monto_final,
        RCVehicularesSumatoria.moneda_orden
      }).ToList();
      GridViewSumaReparaciones.DataBind();

      return vResultado;
    }

    protected void ButtonRepaGenerarResumen_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      int vIdFlujo = 0;
      int vIdCotizacion = 0;
      short vTipoItem = 0;
      bool vResultado = false;

      //Completar los elementos del objeto y grabar el registro.
      vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
      vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);
      vTipoItem = (short)CotizacionICRL.TipoItem.Reparacion;

      //validar si para un proveedor hay dos monedas
      string vProveedor = string.Empty;
      AccesoDatos vAccesoDatos = new AccesoDatos();
      vProveedor = vAccesoDatos.fValidaMonedasSumatoriaVE(vIdFlujo, vIdCotizacion, vTipoItem);

      if (string.Empty != vProveedor)
      {
        LabelMsjReparaciones.Text = "No se puede procesar, El proveedor " + vProveedor + " tiene registros de ambas monedas, revise por favor";
      }
      else
      {
        LabelMsjReparaciones.Text = string.Empty;
        vResultado = CotizacionICRL.RCVehicularSumatoriaGenerar(vIdFlujo, vIdCotizacion, vTipoItem);
        if (vResultado)
        {
          LabelMsjReparaciones.Text = "Reparaciones sumarizadas exitosamente";
        }
        else
        {
          LabelMsjReparaciones.Text = "Reparaciones NO sumarizadas exitosamente";
        }
      }
      FlTraeDatosSumatoriaReparaciones(vIdFlujo, vIdCotizacion, vTipoItem);
    }

    protected void ButtonSumaGrabar_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      LabelSumaRegistroItems.Text = "Items - Sumatoria";
      CotizacionICRL.TipoRCVehicularSumatoria vTipoRCVehicular = new CotizacionICRL.TipoRCVehicularSumatoria();

      //Completar los elementos del objeto y grabar el registro.
      vTipoRCVehicular.id_flujo = int.Parse(TextBoxIdFlujo.Text);
      vTipoRCVehicular.id_cotizacion = int.Parse(TextBoxNroCotizacion.Text);
      short vTipoItem = 0;
      vTipoItem = short.Parse(Session["TipoItem"].ToString());

      vTipoRCVehicular.id_tipo_item = vTipoItem;

      vTipoRCVehicular.proveedor = DropDownListSumaProveedor.SelectedItem.Text.Trim();
      vTipoRCVehicular.monto_orden = double.Parse(TextBoxSumaMontoOrden.Text);
      vTipoRCVehicular.id_tipo_descuento_orden = DropDownListSumaTipoDesc.SelectedItem.Text.Trim();
      vTipoRCVehicular.descuento_proveedor = double.Parse(TextBoxSumaMontoDescProv.Text);
      vTipoRCVehicular.deducible = double.Parse(TextBoxSumaDeducible.Text);
      switch (vTipoRCVehicular.id_tipo_descuento_orden)
      {
        case "Fijo":
          vTipoRCVehicular.monto_final = vTipoRCVehicular.monto_orden - vTipoRCVehicular.descuento_proveedor;
          break;
        case "Porcentaje":
          vTipoRCVehicular.monto_final = vTipoRCVehicular.monto_orden - (vTipoRCVehicular.monto_orden * (vTipoRCVehicular.descuento_proveedor / 100));
          break;
        default:
          vTipoRCVehicular.monto_final = vTipoRCVehicular.monto_orden;
          break;
      }

      bool vResultado = false;

      vResultado = BD.CotizacionICRL.RCVehicularSumatoriaModificar(vTipoRCVehicular);
      if (vResultado)
      {
        LabelSumaRegistroItems.Text = "Registro modificado exitosamente";
        //PLimpiarCamposRepa();
        //PanelABMReparaciones.Enabled = false;
        ButtonSumaGrabar.Enabled = false;
        ButtonSumaCancelar.Enabled = false;
        Session["PopupABMSumasHabilitado"] = 0;
        this.ModalPopupSumatorias.Hide();
      }
      else
      {
        LabelSumaRegistroItems.Text = "El Registro no pudo ser añadido";
      }

      int vIdFlujo = 0;
      int vIdCotizacion = 0;

      //Completar los elementos del objeto y grabar el registro.
      vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
      vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);

      if ((short)CotizacionICRL.TipoItem.Reparacion == vTipoItem)
      {
        FlTraeDatosSumatoriaReparaciones(vIdFlujo, vIdCotizacion, vTipoItem);
      }

      if ((short)CotizacionICRL.TipoItem.Repuesto == vTipoItem)
      {
        FlTraeDatosSumatoriaRepuestos(vIdFlujo, vIdCotizacion, vTipoItem);
      }

    }

    protected void ButtonSumaCancelar_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      //PanelABMReparaciones.Enabled = false;
      ButtonSumaGrabar.Enabled = false;
      ButtonSumaCancelar.Enabled = false;
      //PLimpiarCamposRepa();
      Session["PopupABMSumasHabilitado"] = 0;
      this.ModalPopupSumatorias.Hide();
    }

    protected void ButtonCancelPopSumatorias_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      int vResul = 0;
      //PLimpiaSeccionDaniosPropiosPadre();
      //PLimpiaSeccionDatosPropios();
      //PBloqueaDPPadreEdicion(false);
      //PBloqueaDPEdicion(false);
      //vResul = FlTraeDatosDaniosPropiosPadre(int.Parse(TextBoxNroInspeccion.Text));

      Session["PopupABMSumasHabilitado"] = 0;
      this.ModalPopupSumatorias.Hide();
    }

    protected void GridViewSumaReparaciones_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      string vTextoTemporal = string.Empty;

      short vTipoItem = (short)CotizacionICRL.TipoItem.Reparacion;
      Session["TipoItem"] = vTipoItem;

      PSumaModificarItem(vTipoItem);

      //Leer Registro de la grilla y cargar los valores a la ventana.
      //Proveedor
      vTextoTemporal = string.Empty;
      vTextoTemporal = GridViewSumaReparaciones.SelectedRow.Cells[0].Text;
      vTextoTemporal = vTextoTemporal.Replace("&#209;", "Ñ");
      vTextoTemporal = vTextoTemporal.Replace("&nbsp;", string.Empty);
      DropDownListSumaProveedor.ClearSelection();
      DropDownListSumaProveedor.Items.FindByText(vTextoTemporal).Selected = true;

      //Monto Orden
      TextBoxSumaMontoOrden.Text = GridViewSumaReparaciones.SelectedRow.Cells[1].Text;

      //Tipo Descuento
      vTextoTemporal = string.Empty;
      vTextoTemporal = GridViewSumaReparaciones.SelectedRow.Cells[2].Text;
      vTextoTemporal = vTextoTemporal.Replace("&#209;", "Ñ");
      vTextoTemporal = vTextoTemporal.Replace("&nbsp;", string.Empty);
      DropDownListSumaTipoDesc.ClearSelection();
      DropDownListSumaTipoDesc.Items.FindByText(vTextoTemporal).Selected = true;

      //Monto Descuento Orden
      TextBoxSumaMontoDescProv.Text = GridViewSumaReparaciones.SelectedRow.Cells[3].Text;

      //Monto Deducible Franquicia COA
      TextBoxSumaDeducible.Text = GridViewSumaReparaciones.SelectedRow.Cells[4].Text;

      TextBoxSumaMontoFinal.Text = GridViewSumaReparaciones.SelectedRow.Cells[5].Text;

      Session["PopupABMSumasHabilitado"] = 1;
      this.ModalPopupSumatorias.Show();
    }

    private int FlTraeDatosSumatoriaRepuestos(int pIdFlujo, int pIdCotizacion, short pTipoItem)
    {
      int vResultado = 0;

      BD.CotizacionICRL.TipoRCVehicularSumatoriaTraer vTipoRCVehicularSumatoriaTraer;
      vTipoRCVehicularSumatoriaTraer = CotizacionICRL.RCVehicularSumatoriaTraer(pIdFlujo, pIdCotizacion, pTipoItem);

      GridViewSumaRepuestos.DataSource = vTipoRCVehicularSumatoriaTraer.RCVehicularesSumatoria.Select(RCVehicularesSumatoria => new
      {
        RCVehicularesSumatoria.proveedor,
        RCVehicularesSumatoria.monto_orden,
        RCVehicularesSumatoria.id_tipo_descuento_orden,
        RCVehicularesSumatoria.descuento_proveedor,
        RCVehicularesSumatoria.deducible,
        RCVehicularesSumatoria.monto_final,
        RCVehicularesSumatoria.moneda_orden
      }).ToList();
      GridViewSumaRepuestos.DataBind();

      return vResultado;
    }

    protected void ButtonRepuGenerarResumen_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      int vIdFlujo = 0;
      int vIdCotizacion = 0;
      short vTipoItem = 0;
      bool vResultado = false;

      //Completar los elementos del objeto y grabar el registro.
      vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
      vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);
      vTipoItem = (short)CotizacionICRL.TipoItem.Repuesto;

      //validar si para un proveedor hay dos monedas
      string vProveedor = string.Empty;
      AccesoDatos vAccesoDatos = new AccesoDatos();
      vProveedor = vAccesoDatos.fValidaMonedasSumatoriaVE(vIdFlujo, vIdCotizacion, vTipoItem);

      if (string.Empty != vProveedor)
      {
        LabelMsjRepuestos.Text = "No se puede procesar, El proveedor " + vProveedor + " tiene registros de ambas monedas, revise por favor";
      }
      else
      {
        LabelMsjRepuestos.Text = string.Empty;
        vResultado = CotizacionICRL.RCVehicularSumatoriaGenerar(vIdFlujo, vIdCotizacion, vTipoItem);
        if (vResultado)
        {
          LabelMsjRepuestos.Text = "Repuestos sumarizados exitosamente";
        }
        else
        {
          LabelMsjRepuestos.Text = "Repuestos NO sumarizados exitosamente";
        }
      }
      FlTraeDatosSumatoriaRepuestos(vIdFlujo, vIdCotizacion, vTipoItem);
    }

    protected void GridViewSumaRepuestos_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      string vTextoTemporal = string.Empty;

      short vTipoItem = (short)CotizacionICRL.TipoItem.Repuesto;
      Session["TipoItem"] = vTipoItem;

      PSumaModificarItem(vTipoItem);

      //Leer Registro de la grilla y cargar los valores a la ventana.
      //Proveedor
      vTextoTemporal = string.Empty;
      vTextoTemporal = GridViewSumaRepuestos.SelectedRow.Cells[0].Text;
      vTextoTemporal = vTextoTemporal.Replace("&#209;", "Ñ");
      vTextoTemporal = vTextoTemporal.Replace("&nbsp;", string.Empty);
      DropDownListSumaProveedor.ClearSelection();
      DropDownListSumaProveedor.Items.FindByText(vTextoTemporal).Selected = true;

      //Monto Orden
      TextBoxSumaMontoOrden.Text = GridViewSumaRepuestos.SelectedRow.Cells[1].Text;

      //Tipo Descuento
      vTextoTemporal = string.Empty;
      vTextoTemporal = GridViewSumaRepuestos.SelectedRow.Cells[2].Text;
      vTextoTemporal = vTextoTemporal.Replace("&#209;", "Ñ");
      vTextoTemporal = vTextoTemporal.Replace("&nbsp;", string.Empty);
      DropDownListSumaTipoDesc.ClearSelection();
      DropDownListSumaTipoDesc.Items.FindByText(vTextoTemporal).Selected = true;

      //Monto Descuento Orden
      TextBoxSumaMontoDescProv.Text = GridViewSumaRepuestos.SelectedRow.Cells[3].Text;

      //Monto Deducible Franquicia COA
      TextBoxSumaDeducible.Text = GridViewSumaRepuestos.SelectedRow.Cells[4].Text;

      TextBoxSumaMontoFinal.Text = GridViewSumaRepuestos.SelectedRow.Cells[5].Text;

      Session["PopupABMSumasHabilitado"] = 1;
      this.ModalPopupSumatorias.Show();
    }

    #endregion

    #region Recepcion Repuestos

    private int FlTraeNomenItemRecep()
    {
      int vResultado = 0;
      string vCategoria = "Item";
      int vOrdenCodigo = 2;
      AccesoDatos vAccesoDatos = new AccesoDatos();

      DropDownListRecepItem.DataValueField = "codigo";
      DropDownListRecepItem.DataTextField = "descripcion";
      DropDownListRecepItem.DataSource = vAccesoDatos.FlTraeNomenGenerico(vCategoria, vOrdenCodigo);
      DropDownListRecepItem.DataBind();

      return vResultado;
    }

    protected void PLimpiarCamposRecep()
    {
      DropDownListRecepItem.SelectedIndex = 0;
      CheckBoxRecepRecibido.Checked = false;
      TextBoxRecepDiasEntrega.Text = string.Empty;

    }

    protected void PRecepModificarItem()
    {
      DropDownListRecepItem.Enabled = false;
      ButtonRecepGrabar.Enabled = true;
      ButtonRecepCancelar.Enabled = true;
    }

    private int FlTraeDatosRecepRepu(int pIdCotizacion)
    {
      int vResultado = 0;
      int vIdFlujo = int.Parse(TextBoxIdFlujo.Text);

      BD.CotizacionICRL.TipoRCVehicularTraer vTipoRCVehicularTraer;
      vTipoRCVehicularTraer = CotizacionICRL.RCVehicularesTraer(vIdFlujo, pIdCotizacion);

      GridViewRecepRepuestos.DataSource = vTipoRCVehicularTraer.RCVehiculares.Select(RCVehiculares => new
      {
        RCVehiculares.id_item,
        RCVehiculares.item_descripcion,
        RCVehiculares.recepcion,
        RCVehiculares.dias_entrega,
        RCVehiculares.id_tipo_item
      }).Where(RCVehiculares => RCVehiculares.id_tipo_item == 2).ToList();
      GridViewRecepRepuestos.DataBind();

      return vResultado;
    }

    protected void GridViewRecepRepuestos_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      string vTextoTemporal = string.Empty;

      short vTipoItem = (short)CotizacionICRL.TipoItem.Repuesto;
      Session["TipoItem"] = vTipoItem;

      PRecepModificarItem();

      //Leer Registro de la grilla y cargar los valores a la ventana.
      TextBoxRecepIdItem.Text = GridViewRecepRepuestos.SelectedRow.Cells[0].Text;
      //tipo_item:  1 = Repuracion  2 = Repuesto
      vTextoTemporal = string.Empty;
      vTextoTemporal = GridViewRecepRepuestos.SelectedRow.Cells[1].Text.Trim();
      vTextoTemporal = vTextoTemporal.Replace("&#209;", "Ñ");
      vTextoTemporal = vTextoTemporal.Replace("&nbsp;", string.Empty);

      DropDownListRecepItem.ClearSelection();
      DropDownListRecepItem.Items.FindByText(vTextoTemporal).Selected = true;

      CheckBoxRecepRecibido.Checked = (GridViewRecepRepuestos.SelectedRow.Cells[2].Controls[1] as CheckBox).Checked;

      TextBoxRecepDiasEntrega.Text = GridViewRecepRepuestos.SelectedRow.Cells[3].Text;

      Session["PopupRecepRepuHabilitado"] = 1;
      this.ModalPopupRecepRepuestos.Show();
    }

    protected void ButtonCancelPopRecepRepuestos_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      Session["PopupRecepRepuHabilitado"] = 0;
      this.ModalPopupRecepRepuestos.Hide();
    }

    protected void ButtonRecepGrabar_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      int vIdFlujo = 0;
      int vIdCotizacion = 0;
      long vIdItem = 0;

      LabelRecepRegistroItems.Text = "Items";
      BD.CotizacionICRL.TipoRCVehicular vTipoRCVehicular = new CotizacionICRL.TipoRCVehicular();

      vIdFlujo = int.Parse(TextBoxIdFlujo.Text); ;
      vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);
      vIdItem = long.Parse(TextBoxRecepIdItem.Text);

      //Para no perder los datos originales recuperamos el registro afectado primero y luego actualizamos los datos correspondientes
      BD.CotizacionICRL.TipoRCVehicularTraer vTipoRCVehicularTraer;
      vTipoRCVehicularTraer = CotizacionICRL.RCVehicularesTraer(vIdFlujo, vIdCotizacion, vIdItem);

      //Proceder solo si se trajo correctamente la información.
      if (vTipoRCVehicularTraer.Correcto)
      {
        vTipoRCVehicular = vTipoRCVehicularTraer.RCVehiculares[0];

        //Solo actualizar los datos que se pudieron modificar
        vTipoRCVehicular.recepcion = CheckBoxRecepRecibido.Checked;
        vTipoRCVehicular.dias_entrega = int.Parse(TextBoxRecepDiasEntrega.Text);

        bool vResultado = false;

        vResultado = BD.CotizacionICRL.RCVehicularesModificar(vTipoRCVehicular);
        if (vResultado)
        {
          LabelRecepRegistroItems.Text = "Registro modificado exitosamente";
          PLimpiarCamposRecep();
          ButtonRecepGrabar.Enabled = false;
          ButtonRecepCancelar.Enabled = false;
        }
        else
        {
          LabelRecepRegistroItems.Text = "El Registro no pudo ser añadido";
        }
      }

      FlTraeDatosRecepRepu(vIdCotizacion);

    }

    protected void ButtonRecepCancelar_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      ButtonRepuGrabar.Enabled = false;
      ButtonRepuCancelar.Enabled = false;
      PLimpiarCamposRecep();
    }

    #endregion

    #region Vehicular Tercero

    private int FlTraeNomenVehMarca()
    {
      int vResultado = 0;
      string vCategoria = "Marca";
      int vOrdenCodigo = 2;
      AccesoDatos vAccesoDatos = new AccesoDatos();

      DropDownListVehMarca.DataValueField = "codigo";
      DropDownListVehMarca.DataTextField = "descripcion";
      DropDownListVehMarca.DataSource = vAccesoDatos.FlTraeNomenGenerico(vCategoria, vOrdenCodigo);
      DropDownListVehMarca.DataBind();

      return vResultado;
    }

    private int FlTraeNomenVehAnio()
    {
      int vResultado = 0;
      string vCategoria = "Año";
      int vOrdenCodigo = 2;
      AccesoDatos vAccesoDatos = new AccesoDatos();

      DropDownListVehAnio.DataValueField = "codigo";
      DropDownListVehAnio.DataTextField = "descripcion";
      DropDownListVehAnio.DataSource = vAccesoDatos.FlTraeNomenGenericoDesc(vCategoria, vOrdenCodigo);
      DropDownListVehAnio.DataBind();

      return vResultado;
    }

    private int FlTraeNomenVehColor()
    {
      int vResultado = 0;
      string vCategoria = "Color";
      int vOrdenCodigo = 2;
      AccesoDatos vAccesoDatos = new AccesoDatos();

      DropDownListVehColor.DataValueField = "codigo";
      DropDownListVehColor.DataTextField = "descripcion";
      DropDownListVehColor.DataSource = vAccesoDatos.FlTraeNomenGenerico(vCategoria, vOrdenCodigo);
      DropDownListVehColor.DataBind();

      return vResultado;
    }

    private int FlTraeNomenVehTaller()
    {
      int vResultado = 0;
      string vCategoria = "Tipo Taller";
      int vOrdenCodigo = 2;
      AccesoDatos vAccesoDatos = new AccesoDatos();

      DropDownListVehTipoTaller.DataValueField = "codigo";
      DropDownListVehTipoTaller.DataTextField = "descripcion";
      DropDownListVehTipoTaller.DataSource = vAccesoDatos.FlTraeNomenGenerico(vCategoria, vOrdenCodigo);
      DropDownListVehTipoTaller.DataBind();

      return vResultado;
    }

    protected void PModificarVehTer(bool pEstado)
    {
      TextBoxVehNombreTer.Enabled = pEstado;
      TextBoxVehDocId.Enabled = pEstado;
      TextBoxVehTelefono.Enabled = pEstado;
      DropDownListVehMarca.Enabled = pEstado;
      TextBoxVehModelo.Enabled = pEstado;
      DropDownListVehAnio.Enabled = pEstado;
      TextBoxVehPlaca.Enabled = pEstado;
      DropDownListVehColor.Enabled = pEstado;
      TextBoxVehNroChasis.Enabled = pEstado;
      DropDownListVehTipoTaller.Enabled = pEstado;
    }

    #endregion

    long FValidaTieneVehTercero(int pIdFlujo, int pIdCotizacion)
    {
      long vResultado = 0;

      BD.CotizacionICRL.TipoRCVehicularTerceroTraer vTipoRCVehicularTerceroTraer;
      vTipoRCVehicularTerceroTraer = CotizacionICRL.RCVehicularTerceroTraer(pIdFlujo, pIdCotizacion);

      if (vTipoRCVehicularTerceroTraer.Correcto)
      {
        var vFilaTabla = vTipoRCVehicularTerceroTraer.RCVehicularesTerceros.FirstOrDefault();
        if (vFilaTabla != null)
        {
          vResultado = vFilaTabla.id_item;
        }
      }

      return vResultado;
    }

    protected void PCargaDatosTer()
    {
      long vIdItem = 0;
      int vIdFlujo = 0;
      int vIdCotizacion = 0;
      string vTextoTemporal = string.Empty;

      vIdFlujo = int.Parse(TextBoxIdFlujo.Text); ;
      vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);

      vIdItem = FValidaTieneVehTercero(vIdFlujo, vIdCotizacion);
      if (vIdItem > 0)
      {
        BD.CotizacionICRL.TipoRCVehicularTerceroTraer vTipoRCVehicularTerceroTraer;
        vTipoRCVehicularTerceroTraer = CotizacionICRL.RCVehicularTerceroTraer(vIdFlujo, vIdCotizacion);
        var vFilaTabla = vTipoRCVehicularTerceroTraer.RCVehicularesTerceros.FirstOrDefault();
        TextBoxVehNombreTer.Text = vFilaTabla.nombre;
        TextBoxVehDocId.Text = vFilaTabla.documento;
        TextBoxVehTelefono.Text = vFilaTabla.telefono;

        vTextoTemporal = string.Empty;
        vTextoTemporal = vFilaTabla.marca.Trim();
        vTextoTemporal = vTextoTemporal.Replace("&#209;", "Ñ");
        vTextoTemporal = vTextoTemporal.Replace("&nbsp;", string.Empty);
        DropDownListVehMarca.ClearSelection();
        DropDownListVehMarca.Items.FindByText(vTextoTemporal).Selected = true;

        TextBoxVehModelo.Text = vFilaTabla.modelo;

        vTextoTemporal = string.Empty;
        vTextoTemporal = vFilaTabla.anio.Trim();
        DropDownListVehAnio.ClearSelection();
        DropDownListVehAnio.Items.FindByText(vTextoTemporal).Selected = true;

        TextBoxVehPlaca.Text = vFilaTabla.placa;

        vTextoTemporal = string.Empty;
        vTextoTemporal = vFilaTabla.color.Trim();
        vTextoTemporal = vTextoTemporal.Replace("&#209;", "Ñ");
        vTextoTemporal = vTextoTemporal.Replace("&nbsp;", string.Empty);
        DropDownListVehColor.ClearSelection();
        DropDownListVehColor.Items.FindByText(vTextoTemporal).Selected = true;

        TextBoxVehNroChasis.Text = vFilaTabla.chasis;

        vTextoTemporal = string.Empty;
        vTextoTemporal = vFilaTabla.taller.Trim();
        vTextoTemporal = vTextoTemporal.Replace("&#209;", "Ñ");
        vTextoTemporal = vTextoTemporal.Replace("&nbsp;", string.Empty);
        DropDownListVehTipoTaller.ClearSelection();
        DropDownListVehTipoTaller.Items.FindByText(vTextoTemporal).Selected = true;

        TextBoxVehIdItem.Text = vFilaTabla.id_item.ToString();
      }

    }

    protected void ButtonVehActualizar_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      LabelDatosVehicularMsj.Text = string.Empty;
      PModificarVehTer(true);
      ButtonVehActualizar.Visible = false;
      ButtonVehGrabar.Visible = true;
      ButtonVehCancelar.Visible = true;
    }

    protected void ButtonVehGrabar_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      int vIdFlujo = 0;
      int vIdCotizacion = 0;
      long vIdItem = 0;
      bool vResultado = false;

      vIdFlujo = int.Parse(TextBoxIdFlujo.Text); ;
      vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);

      //cargar los datos del panel al objeto correspondiente
      CotizacionICRL.TipoRCVehicularTercero vTipoRCVehicularTercero = new CotizacionICRL.TipoRCVehicularTercero();
      vTipoRCVehicularTercero.id_flujo = vIdFlujo;
      vTipoRCVehicularTercero.id_cotizacion = vIdCotizacion;
      vTipoRCVehicularTercero.nombre = TextBoxVehNombreTer.Text.Trim().ToUpper();
      vTipoRCVehicularTercero.documento = TextBoxVehDocId.Text.Trim().ToUpper();
      vTipoRCVehicularTercero.telefono = TextBoxVehTelefono.Text.Trim().ToUpper();
      vTipoRCVehicularTercero.marca = DropDownListVehMarca.SelectedItem.Text.Trim();
      vTipoRCVehicularTercero.modelo = TextBoxVehModelo.Text.Trim().ToUpper();
      vTipoRCVehicularTercero.anio = DropDownListVehAnio.SelectedItem.Text.Trim();
      vTipoRCVehicularTercero.placa = TextBoxVehPlaca.Text.Trim().ToUpper();
      vTipoRCVehicularTercero.color = DropDownListVehColor.SelectedItem.Text.Trim();
      vTipoRCVehicularTercero.chasis = TextBoxVehNroChasis.Text.Trim().ToUpper();
      vTipoRCVehicularTercero.taller = DropDownListVehTipoTaller.SelectedItem.Text.Trim();

      //validar si existe el registro del tercero
      if (string.Empty != TextBoxVehIdItem.Text)
      {
        //Existe el registro del tercero
        vIdItem = long.Parse(TextBoxVehIdItem.Text);
        vTipoRCVehicularTercero.id_item = vIdItem;
        vResultado = CotizacionICRL.RCVehicularTerceroModificar(vTipoRCVehicularTercero);
      }
      else
      {
        //NO Existe el registro del tercero
        vResultado = CotizacionICRL.RCVehicularTerceroRegistrar(vTipoRCVehicularTercero);
      }

      if (vResultado)
      {
        LabelDatosVehicularMsj.Text = "Registro Actualizado Exitosamente";
        vIdItem = FValidaTieneVehTercero(vIdFlujo, vIdCotizacion);
        TextBoxVehIdItem.Text = vIdItem.ToString();
      }
      else
      {
        LabelDatosVehicularMsj.Text = "El Registro no se actualizo correctamente";
      }
      PModificarVehTer(false);
      ButtonVehActualizar.Visible = true;
      ButtonVehGrabar.Visible = false;
      ButtonVehCancelar.Visible = false;
    }

    protected void ButtonVehCancelar_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      PModificarVehTer(false);
      ButtonVehActualizar.Visible = true;
      ButtonVehGrabar.Visible = false;
      ButtonVehCancelar.Visible = false;
    }

    private int FLlenarGrillaOrdenes(int pIdFlujo, int pIdCotizacion, short pTipoItem)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vLst = from cvhs in db.cotizacion_rc_vehicular_sumatoria
                   where (cvhs.id_flujo == pIdFlujo)
                      && (cvhs.id_cotizacion == pIdCotizacion)
                   select new
                   {
                     cvhs.numero_orden,
                     cvhs.id_estado,
                     cvhs.proveedor,
                     cvhs.moneda_orden,
                     cvhs.monto_orden,
                     cvhs.id_tipo_descuento_orden,
                     cvhs.descuento_proveedor,
                     cvhs.deducible,
                     cvhs.monto_final,
                   };

        GridViewOrdenes.DataSource = vLst.ToList();
        GridViewOrdenes.DataBind();

      }

      return vResultado;
    }

    protected void GridViewOrdenes_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      //verificar esa rutina
      if (!VerificarPagina(true)) return;
      if (e.Row.RowType == DataControlRowType.DataRow)
      {
        //verificar el estado del registro
        string vEstadoCadena = string.Empty;
        int vEstado = 0;
        vEstadoCadena = e.Row.Cells[1].Text;
        vEstado = int.Parse(vEstadoCadena);
        if (1 == vEstado)
        {
          (e.Row.Cells[11].Controls[0] as Button).Enabled = true;
          //validar si el Proveedor es Benficiario
          string vNombreProveedor = string.Empty;
          vNombreProveedor = e.Row.Cells[2].Text.Trim().ToUpper();
          if ("BENEFICIARIO" == vNombreProveedor.ToUpper())
          {
            (e.Row.Cells[11].Controls[0] as Button).Enabled = false;
          }
          else
          {
            (e.Row.Cells[11].Controls[0] as Button).Enabled = true;
          }
        }
        else
        {
          (e.Row.Cells[11].Controls[0] as Button).Enabled = false;
        }
      }
    }

    protected void GridViewOrdenes_RowCommand(object sender, GridViewCommandEventArgs e)
    {
      if (!VerificarPagina(true)) return;
      int vIdFlujo = 0;
      int vIdCotizacion = 0;
      int vTipoItem = 0;
      int vIndex = 0;
      string vNumeroOrden = string.Empty;
      string vProveedor = string.Empty;
      AccesoDatos vAccesoDatos = new AccesoDatos();

      if (0 == e.CommandName.CompareTo("Imprimir"))
      {
        string vTextoSecuencial = string.Empty;
        vIndex = 0;

        vIndex = Convert.ToInt32(e.CommandArgument);
        vNumeroOrden = (string)GridViewOrdenes.DataKeys[vIndex].Value;
        vProveedor = GridViewOrdenes.Rows[vIndex].Cells[2].Text;
        PImprimeFormularioCotiRCVehicular(vNumeroOrden);
      }

      if (0 == e.CommandName.CompareTo("Ver"))
      {
        string vTextoSecuencial = string.Empty;
        vIndex = 0;

        vIndex = Convert.ToInt32(e.CommandArgument);
        vNumeroOrden = (string)GridViewOrdenes.DataKeys[vIndex].Value;
        vProveedor = GridViewOrdenes.Rows[vIndex].Cells[2].Text;
        ButtonCierraVerRep.Visible = true;
        PVerFormularioCotiRCVehicular(vNumeroOrden);
      }

      if (0 == e.CommandName.CompareTo("SubirOnBase"))
      {
        int vResultado = 0;
        string vTextoSecuencial = string.Empty;


        vIndex = Convert.ToInt32(e.CommandArgument);
        vNumeroOrden = (string)GridViewOrdenes.DataKeys[vIndex].Value;
        vProveedor = GridViewOrdenes.Rows[vIndex].Cells[2].Text;


        //Grabar en la tabla
        

        vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
        vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);
        if ("OT" == vNumeroOrden.Substring(0, 2))
        {
          vTipoItem = (int)CotizacionICRL.TipoItem.Reparacion;
        }
        else
        {
          if ("OC" == vNumeroOrden.Substring(0, 2))
          {
            vTipoItem = (int)CotizacionICRL.TipoItem.Repuesto;
          }
          else
          {
            if ("T" == vNumeroOrden.Substring(20, 1))
            {
              vTipoItem = (int)CotizacionICRL.TipoItem.Reparacion;
            }
            else
            {
              vTipoItem = (int)CotizacionICRL.TipoItem.Repuesto;
            }
          }
        }

        vResultado = vAccesoDatos.fActualizaLiquidacionVE(vIdFlujo, vIdCotizacion, vProveedor, vTipoItem);
        PSubeFormularioCotiRCVehicular(vNumeroOrden);
        vResultado = vAccesoDatos.FCotizacionRCVehicularCambiaEstadoSumatoria(vIdFlujo, vIdCotizacion, (short)vTipoItem, vProveedor);
        FLlenarGrillaOrdenes(vIdFlujo, vIdCotizacion, (short)vTipoItem);
      }
    }

    protected void ButtonRepuGenerarOrdenes_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      int vIdFlujo = 0;
      int vIdCotizacion = 0;
      short vTipoItem = 0;
      int vContador = 1;

      ButtonRepuCambioBenef.Visible = false;

      //verificar si existe un Beneficiario
      int vIndiceBenef = FValidaRepuBeneficiario();

      vIdFlujo = int.Parse(TextBoxIdFlujo.Text); ;
      vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);
      vTipoItem = (short)CotizacionICRL.TipoItem.Repuesto;

      BD.CotizacionICRL.TipoRCVehicularSumatoriaTraer vTipoRCVehicularSumatoriaTraer;
      vTipoRCVehicularSumatoriaTraer = CotizacionICRL.RCVehicularSumatoriaTraer(vIdFlujo, vIdCotizacion, vTipoItem);
      DataSet vDatasetOrdenes = vTipoRCVehicularSumatoriaTraer.dsRCVehicularSumatoria;
      int vIndiceDataTable = vDatasetOrdenes.Tables.Count - 1;

      if (vIndiceDataTable >= 0)
      {
        StringBuilder vSBNumeroOrden = new StringBuilder();
        string vNumeroOrden = string.Empty;
        //Completar el campo numero_orden por cada registro del dataset.
        for (int i = 0; i < vDatasetOrdenes.Tables[vIndiceDataTable].Rows.Count; i++)
        {
          vNumeroOrden = string.Empty;
          vSBNumeroOrden.Clear();
          vSBNumeroOrden.Append("OC-");
          vNumeroOrden = TextBoxNroFlujo.Text.Trim();
          vNumeroOrden = vNumeroOrden.PadLeft(6, '0');
          vSBNumeroOrden.Append(vNumeroOrden);
          vSBNumeroOrden.Append("-VE-");
          vNumeroOrden = vIdCotizacion.ToString();
          vNumeroOrden = vNumeroOrden.PadLeft(6, '0');
          vSBNumeroOrden.Append(vNumeroOrden);
          vSBNumeroOrden.Append("-");
          vNumeroOrden = vContador.ToString();
          vSBNumeroOrden.Append(vNumeroOrden.PadLeft(2, '0'));
          vNumeroOrden = vSBNumeroOrden.ToString();
          vDatasetOrdenes.Tables[vIndiceDataTable].Rows[i][9] = vNumeroOrden;
          //inicializar el estado a 1
          vDatasetOrdenes.Tables[vIndiceDataTable].Rows[i][10] = 1;
          vContador++;
        }
      }

      BD.CotizacionICRL.RCVehicularSumatoriaModificarTodos(vDatasetOrdenes);

      //actualizamos el detalle de las ordenes generadas
      vTipoRCVehicularSumatoriaTraer = CotizacionICRL.RCVehicularSumatoriaTraer(vIdFlujo, vIdCotizacion, vTipoItem);
      vDatasetOrdenes = vTipoRCVehicularSumatoriaTraer.dsRCVehicularSumatoria;
      vIndiceDataTable = vDatasetOrdenes.Tables.Count - 1;

      if (vIndiceDataTable >= 0)
      {
        AccesoDatos vAccesoDatos = new AccesoDatos();
        for (int i = 0; i < vDatasetOrdenes.Tables[vIndiceDataTable].Rows.Count; i++)
        {
          string vProveedor = string.Empty;
          vProveedor = vDatasetOrdenes.Tables[vIndiceDataTable].Rows[i][3].ToString();
          vAccesoDatos.fActualizaOrdenesCotiVE(vIdFlujo, vIdCotizacion, vProveedor, vTipoItem);
          vContador++;
        }
      }

      FLlenarGrillaOrdenes(vIdFlujo, vIdCotizacion, vTipoItem);

    }

    protected void ButtonRepaGenerarOrdenes_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      int vIdFlujo = 0;
      int vIdCotizacion = 0;
      short vTipoItem = 0;
      int vContador = 1;

      ButtonRepaCambioBenef.Visible = false;

      //verificar si existe un Beneficiario
      int vIndiceBenef = FValidaRepaBeneficiario();

      vIdFlujo = int.Parse(TextBoxIdFlujo.Text); ;
      vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);
      vTipoItem = (short)CotizacionICRL.TipoItem.Reparacion;

      BD.CotizacionICRL.TipoRCVehicularSumatoriaTraer vTipoRCVehicularSumatoriaTraer;
      vTipoRCVehicularSumatoriaTraer = CotizacionICRL.RCVehicularSumatoriaTraer(vIdFlujo, vIdCotizacion, vTipoItem);
      DataSet vDatasetOrdenes = vTipoRCVehicularSumatoriaTraer.dsRCVehicularSumatoria;
      int vIndiceDataTable = vDatasetOrdenes.Tables.Count - 1;

      if (vIndiceDataTable >= 0)
      {
        StringBuilder vSBNumeroOrden = new StringBuilder();
        string vNumeroOrden = string.Empty;
        //Completar el campo numero_orden por cada registro del dataset.
        for (int i = 0; i < vDatasetOrdenes.Tables[vIndiceDataTable].Rows.Count; i++)
        {
          vNumeroOrden = string.Empty;
          vSBNumeroOrden.Clear();
          vSBNumeroOrden.Append("OT-");
          vNumeroOrden = TextBoxNroFlujo.Text.Trim();
          vNumeroOrden = vNumeroOrden.PadLeft(6, '0');
          vSBNumeroOrden.Append(vNumeroOrden);
          vSBNumeroOrden.Append("-VE-");
          vNumeroOrden = vIdCotizacion.ToString();
          vNumeroOrden = vNumeroOrden.PadLeft(6, '0');
          vSBNumeroOrden.Append(vNumeroOrden);
          vSBNumeroOrden.Append("-");
          vNumeroOrden = vContador.ToString();
          vSBNumeroOrden.Append(vNumeroOrden.PadLeft(2, '0'));
          vNumeroOrden = vSBNumeroOrden.ToString();
          vDatasetOrdenes.Tables[vIndiceDataTable].Rows[i][9] = vNumeroOrden;
          //inicializar el estado a 1
          vDatasetOrdenes.Tables[vIndiceDataTable].Rows[i][10] = 1;
          vContador++;
        }
      }

      BD.CotizacionICRL.RCVehicularSumatoriaModificarTodos(vDatasetOrdenes);

      //actualizamos el detalle de las ordenes generadas
      vTipoRCVehicularSumatoriaTraer = CotizacionICRL.RCVehicularSumatoriaTraer(vIdFlujo, vIdCotizacion, vTipoItem);
      vDatasetOrdenes = vTipoRCVehicularSumatoriaTraer.dsRCVehicularSumatoria;
      vIndiceDataTable = vDatasetOrdenes.Tables.Count - 1;

      if (vIndiceDataTable >= 0)
      {
        AccesoDatos vAccesoDatos = new AccesoDatos();
        for (int i = 0; i < vDatasetOrdenes.Tables[vIndiceDataTable].Rows.Count; i++)
        {
          string vProveedor = string.Empty;
          vProveedor = vDatasetOrdenes.Tables[vIndiceDataTable].Rows[i][3].ToString();
          vAccesoDatos.fActualizaOrdenesCotiVE(vIdFlujo, vIdCotizacion, vProveedor, vTipoItem);
          vContador++;
        }
      }

      FLlenarGrillaOrdenes(vIdFlujo, vIdCotizacion, vTipoItem);

    }

    protected void PImprimeFormularioCotiRCVehicular(string pNroOrden)
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
      string fileName = "RepFormCotiRCVehicular" + pNroOrden;

      var vListaFlujo = from f in db.Flujo
                        join s in db.cotizacion_rc_vehicular_sumatoria on f.idFlujo equals s.id_flujo
                        where (s.numero_orden == pNroOrden)
                        select new
                        {
                          f.nombreAsegurado,
                          f.telefonocelAsegurado,
                          cobertura = "RC VEHICULAR",
                          f.fechaSiniestro,
                          f.flujoOnBase,
                          f.numeroReclamo,
                          f.numeroPoliza,
                          f.marcaVehiculo,
                          f.modeloVehiculo,
                          f.anioVehiculo,
                          f.placaVehiculo,
                          f.chasisVehiculo
                        };

      var vListaCotiRCVehicular = from c in db.cotizacion_rc_vehicular
                                  join f in db.Flujo on c.id_flujo equals f.idFlujo
                                  where (c.numero_orden == pNroOrden)
                                  orderby c.item_descripcion
                                  select new
                                  {
                                    f.nombreAsegurado,
                                    f.telefonocelAsegurado,
                                    cobertura = "RC VEHICULAR",
                                    f.fechaSiniestro,
                                    f.flujoOnBase,
                                    f.numeroReclamo,
                                    f.numeroPoliza,
                                    f.marcaVehiculo,
                                    f.modeloVehiculo,
                                    f.anioVehiculo,
                                    f.placaVehiculo,
                                    f.chasisVehiculo,
                                    c.numero_orden,
                                    c.proveedor,
                                    c.item_descripcion,
                                    c.id_moneda,
                                    c.precio_final,
                                    c.tipo_cambio
                                  };

      var vListaCotiSumaRCVehicular = from c in db.cotizacion_rc_vehicular_sumatoria
                                      where (c.numero_orden == pNroOrden)
                                      select new
                                      {
                                        c.numero_orden,
                                        c.proveedor,
                                        c.moneda_orden,
                                        c.monto_orden,
                                        c.id_tipo_descuento_orden,
                                        c.descuento_proveedor,
                                        c.deducible,
                                        c.monto_final
                                      };

      var vListaCotiRCVehicularTercero = from c in db.cotizacion_rc_vehicular
                                         join f in db.Flujo on c.id_flujo equals f.idFlujo
                                         join t in db.cotizacion_rc_vehicular_tercero on c.id_flujo equals t.id_flujo
                                         where (c.numero_orden == pNroOrden)
                                         select new
                                         {
                                           f.flujoOnBase,
                                           t.nombre,
                                           t.telefono,
                                           t.marca,
                                           t.modelo,
                                           t.anio,
                                           t.placa,
                                           t.chasis
                                         };

      var vListaCotizacionUsuario = from cc in db.cotizacion_rc_vehicular
                                    join c in db.Cotizacion on cc.id_cotizacion equals c.idCotizacion
                                    join u in db.Usuario on c.idUsuario equals u.idUsuario
                                    where (cc.numero_orden == pNroOrden)
                                    select new
                                    {
                                      cc.numero_orden,
                                      c.idUsuario,
                                      u.nombres,
                                      u.apellidos,
                                      u.codUsuario,
                                      u.nombreVisible
                                    };

      ReportViewerCoti.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
      ReportViewerCoti.LocalReport.ReportPath = "Reportes\\RepFormularioCotiRCVehicular.rdlc";
      ReportDataSource datasource1 = new ReportDataSource("DataSet1", vListaFlujo);
      ReportDataSource datasource2 = new ReportDataSource("DataSet2", vListaCotiRCVehicular);
      ReportDataSource datasource3 = new ReportDataSource("DataSet3", vListaCotiSumaRCVehicular);
      ReportDataSource datasource4 = new ReportDataSource("DataSet4", vListaCotiRCVehicularTercero.Distinct().ToList());
      ReportDataSource datasource5 = new ReportDataSource("DataSet5", vListaCotizacionUsuario);

      ReportViewerCoti.LocalReport.DataSources.Clear();
      ReportViewerCoti.LocalReport.DataSources.Add(datasource1);
      ReportViewerCoti.LocalReport.DataSources.Add(datasource2);
      ReportViewerCoti.LocalReport.DataSources.Add(datasource3);
      ReportViewerCoti.LocalReport.DataSources.Add(datasource4);
      ReportViewerCoti.LocalReport.DataSources.Add(datasource5);

      ReportViewerCoti.LocalReport.Refresh();
      byte[] VArrayBytes = ReportViewerCoti.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

      //enviar el array de bytes a cliente
      Response.Buffer = true;
      Response.Clear();
      Response.ContentType = mimeType;
      Response.AddHeader("content-disposition", "attachment; filename=" + fileName + "." + extension);
      Response.BinaryWrite(VArrayBytes); // se crea el archivo
      Response.Flush(); // se envia al cliente para su descarga
    }

    protected void PVerFormularioCotiRCVehicular(string pNroOrden)
    {
      AccesoDatos vAccesoDatos = new AccesoDatos();
      LBCDesaEntities db = new LBCDesaEntities();

      var vListaFlujo = from f in db.Flujo
                        join s in db.cotizacion_rc_vehicular_sumatoria on f.idFlujo equals s.id_flujo
                        where (s.numero_orden == pNroOrden)
                        select new
                        {
                          f.nombreAsegurado,
                          f.telefonocelAsegurado,
                          cobertura = "RC VEHICULAR",
                          f.fechaSiniestro,
                          f.flujoOnBase,
                          f.numeroReclamo,
                          f.numeroPoliza,
                          f.marcaVehiculo,
                          f.modeloVehiculo,
                          f.anioVehiculo,
                          f.placaVehiculo,
                          f.chasisVehiculo
                        };

      var vListaCotiRCVehicular = from c in db.cotizacion_rc_vehicular
                                  join f in db.Flujo on c.id_flujo equals f.idFlujo
                                  where (c.numero_orden == pNroOrden)
                                  orderby c.item_descripcion
                                  select new
                                  {
                                    f.nombreAsegurado,
                                    f.telefonocelAsegurado,
                                    cobertura = "RC VEHICULAR",
                                    f.fechaSiniestro,
                                    f.flujoOnBase,
                                    f.numeroReclamo,
                                    f.numeroPoliza,
                                    f.marcaVehiculo,
                                    f.modeloVehiculo,
                                    f.anioVehiculo,
                                    f.placaVehiculo,
                                    f.chasisVehiculo,
                                    c.numero_orden,
                                    c.proveedor,
                                    c.item_descripcion,
                                    c.id_moneda,
                                    c.precio_final,
                                    c.tipo_cambio
                                  };

      var vListaCotiSumaRCVehicular = from c in db.cotizacion_rc_vehicular_sumatoria
                                      where (c.numero_orden == pNroOrden)
                                      select new
                                      {
                                        c.numero_orden,
                                        c.proveedor,
                                        c.moneda_orden,
                                        c.monto_orden,
                                        c.id_tipo_descuento_orden,
                                        c.descuento_proveedor,
                                        c.deducible,
                                        c.monto_final
                                      };

      var vListaCotiRCVehicularTercero = from c in db.cotizacion_rc_vehicular
                                         join f in db.Flujo on c.id_flujo equals f.idFlujo
                                         join t in db.cotizacion_rc_vehicular_tercero on c.id_flujo equals t.id_flujo
                                         where (c.numero_orden == pNroOrden)
                                         select new
                                         {
                                           f.flujoOnBase,
                                           t.nombre,
                                           t.telefono,
                                           t.marca,
                                           t.modelo,
                                           t.anio,
                                           t.placa,
                                           t.chasis
                                         };

      var vListaCotizacionUsuario = from cc in db.cotizacion_rc_vehicular
                                    join c in db.Cotizacion on cc.id_cotizacion equals c.idCotizacion
                                    join u in db.Usuario on c.idUsuario equals u.idUsuario
                                    where (cc.numero_orden == pNroOrden)
                                    select new
                                    {
                                      cc.numero_orden,
                                      c.idUsuario,
                                      u.nombres,
                                      u.apellidos,
                                      u.codUsuario,
                                      u.nombreVisible
                                    };

      ReportViewerCoti.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
      ReportViewerCoti.LocalReport.ReportPath = "Reportes\\RepFormularioCotiRCVehicular.rdlc";
      ReportDataSource datasource1 = new ReportDataSource("DataSet1", vListaFlujo);
      ReportDataSource datasource2 = new ReportDataSource("DataSet2", vListaCotiRCVehicular);
      ReportDataSource datasource3 = new ReportDataSource("DataSet3", vListaCotiSumaRCVehicular);
      ReportDataSource datasource4 = new ReportDataSource("DataSet4", vListaCotiRCVehicularTercero.Distinct().ToList());
      ReportDataSource datasource5 = new ReportDataSource("DataSet5", vListaCotizacionUsuario);

      ReportViewerCoti.LocalReport.DataSources.Clear();
      ReportViewerCoti.LocalReport.DataSources.Add(datasource1);
      ReportViewerCoti.LocalReport.DataSources.Add(datasource2);
      ReportViewerCoti.LocalReport.DataSources.Add(datasource3);
      ReportViewerCoti.LocalReport.DataSources.Add(datasource4);
      ReportViewerCoti.LocalReport.DataSources.Add(datasource5);

      ReportViewerCoti.LocalReport.Refresh();
      ReportViewerCoti.ShowToolBar = false;
      ReportViewerCoti.Visible = true;

    }

    protected void PSubeFormularioCotiRCVehicular(string pNroOrden)
    {
      AccesoDatos vAccesoDatos = new AccesoDatos();
      LBCDesaEntities db = new LBCDesaEntities();

      string vNumFlujo = TextBoxNroFlujo.Text;
      string vTipoDocumental = string.Empty;
      string vNombreUsuario = string.Empty;

      Warning[] warnings;
      string[] streamIds;
      string mimeType = string.Empty;
      string encoding = string.Empty;
      string extension = "pdf";
      string fileName = "RepFormCotiRCVehicular" + pNroOrden;

      if ("OT" == pNroOrden.Substring(0, 2))
      {
        vTipoDocumental = "RE - Orden de Trabajo";
      }
      else
      {
        if ("OC" == pNroOrden.Substring(0, 2))
        {
          vTipoDocumental = "RE - Orden de Compra";
        }
        else
        {
          vTipoDocumental = "RE - Orden de Indemnizacion";
        }
      }

      vNombreUsuario = Session["IdUsr"].ToString();

      var vListaFlujo = from f in db.Flujo
                        join s in db.cotizacion_rc_vehicular_sumatoria on f.idFlujo equals s.id_flujo
                        where (s.numero_orden == pNroOrden)
                        select new
                        {
                          f.nombreAsegurado,
                          f.telefonocelAsegurado,
                          cobertura = "RC VEHICULAR",
                          f.fechaSiniestro,
                          f.flujoOnBase,
                          f.numeroReclamo,
                          f.numeroPoliza,
                          f.marcaVehiculo,
                          f.modeloVehiculo,
                          f.anioVehiculo,
                          f.placaVehiculo,
                          f.chasisVehiculo
                        };

      var vListaCotiRCVehicular = from c in db.cotizacion_rc_vehicular
                                  join f in db.Flujo on c.id_flujo equals f.idFlujo
                                  where (c.numero_orden == pNroOrden)
                                  orderby c.item_descripcion
                                  select new
                                  {
                                    f.nombreAsegurado,
                                    f.telefonocelAsegurado,
                                    cobertura = "RC VEHICULAR",
                                    f.fechaSiniestro,
                                    f.flujoOnBase,
                                    f.numeroReclamo,
                                    f.numeroPoliza,
                                    f.marcaVehiculo,
                                    f.modeloVehiculo,
                                    f.anioVehiculo,
                                    f.placaVehiculo,
                                    f.chasisVehiculo,
                                    c.numero_orden,
                                    c.proveedor,
                                    c.item_descripcion,
                                    c.id_moneda,
                                    c.precio_final,
                                    c.tipo_cambio
                                  };

      var vListaCotiSumaRCVehicular = from c in db.cotizacion_rc_vehicular_sumatoria
                                      where (c.numero_orden == pNroOrden)
                                      select new
                                      {
                                        c.numero_orden,
                                        c.proveedor,
                                        c.moneda_orden,
                                        c.monto_orden,
                                        c.id_tipo_descuento_orden,
                                        c.descuento_proveedor,
                                        c.deducible,
                                        c.monto_final
                                      };

      var vListaCotiRCVehicularTercero = from c in db.cotizacion_rc_vehicular
                                         join f in db.Flujo on c.id_flujo equals f.idFlujo
                                         join t in db.cotizacion_rc_vehicular_tercero on c.id_flujo equals t.id_flujo
                                         where (c.numero_orden == pNroOrden)
                                         select new
                                         {
                                           f.flujoOnBase,
                                           t.nombre,
                                           t.telefono,
                                           t.marca,
                                           t.modelo,
                                           t.anio,
                                           t.placa,
                                           t.chasis
                                         };

      var vListaCotizacionUsuario = from cc in db.cotizacion_rc_vehicular
                                    join c in db.Cotizacion on cc.id_cotizacion equals c.idCotizacion
                                    join u in db.Usuario on c.idUsuario equals u.idUsuario
                                    where (cc.numero_orden == pNroOrden)
                                    select new
                                    {
                                      cc.numero_orden,
                                      c.idUsuario,
                                      u.nombres,
                                      u.apellidos,
                                      u.codUsuario,
                                      u.nombreVisible
                                    };

      ReportViewerCoti.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
      ReportViewerCoti.LocalReport.ReportPath = "Reportes\\RepFormularioCotiRCVehicular.rdlc";
      ReportDataSource datasource1 = new ReportDataSource("DataSet1", vListaFlujo);
      ReportDataSource datasource2 = new ReportDataSource("DataSet2", vListaCotiRCVehicular);
      ReportDataSource datasource3 = new ReportDataSource("DataSet3", vListaCotiSumaRCVehicular);
      ReportDataSource datasource4 = new ReportDataSource("DataSet4", vListaCotiRCVehicularTercero.Distinct().ToList());
      ReportDataSource datasource5 = new ReportDataSource("DataSet5", vListaCotiSumaRCVehicular);

      ReportViewerCoti.LocalReport.DataSources.Clear();
      ReportViewerCoti.LocalReport.DataSources.Add(datasource1);
      ReportViewerCoti.LocalReport.DataSources.Add(datasource2);
      ReportViewerCoti.LocalReport.DataSources.Add(datasource3);
      ReportViewerCoti.LocalReport.DataSources.Add(datasource4);
      ReportViewerCoti.LocalReport.DataSources.Add(datasource5);

      ReportViewerCoti.LocalReport.Refresh();
      byte[] vArrayBytes = ReportViewerCoti.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

      //enviar el array de bytes a OnBase
      int vResultado = 0;
      vResultado = vAccesoDatos.FEnviaArchivoOnBase(vNumFlujo, vTipoDocumental, vNombreUsuario, vArrayBytes);
      if (vResultado > 0)
      {
        LabelMensaje.Text = "Documento subido exitosamente a OnBase";
      }
      else
      {
        LabelMensaje.Text = "error, El Documento no fue subido a OnBase";
      }
    }

    protected void ButtonCierraVerRep_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      ReportViewerCoti.Visible = false;
      ButtonCierraVerRep.Visible = false;
    }

    protected void ButtonFinalizarCotizacion_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      string vNombreUsuario = string.Empty;
      vNombreUsuario = Session["IdUsr"].ToString();
      string vBandejaEntrada = "REC – COTIZACION – PENDIENTE DE COTIZACION";
      string vBandejaSalida = "REC – PRONUNCIAMIENTO - INICIO";
      string vNumeroFlujo = TextBoxNroFlujo.Text;

      AccesoDatos vAccesoDatos = new AccesoDatos();
      int vResultado = 0;
      vResultado = vAccesoDatos.FCambiaEstadoOnBase(vNumeroFlujo, vNombreUsuario, vBandejaEntrada, vBandejaSalida);

      int vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
      vAccesoDatos.FCambiaEstadoFlujoCotiLiq(vIdFlujo);
    }

    #region CambioBeneficiario Orden Pago

    private int FValidaRepaBeneficiario()
    {
      int vResultado = -1;
      int vIdFlujo = 0;
      int vIdCotizacion = 0;
      short vTipoItem = 0;

      vIdFlujo = int.Parse(TextBoxIdFlujo.Text); ;
      vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);
      vTipoItem = (short)CotizacionICRL.TipoItem.Reparacion;

      BD.CotizacionICRL.TipoRCVehicularSumatoriaTraer vTipoRCVehicularSumatoriaTraer;
      vTipoRCVehicularSumatoriaTraer = CotizacionICRL.RCVehicularSumatoriaTraer(vIdFlujo, vIdCotizacion, vTipoItem);
      DataSet vDatasetOrdenes = vTipoRCVehicularSumatoriaTraer.dsRCVehicularSumatoria;
      int vIndiceDataTable = vDatasetOrdenes.Tables.Count - 1;

      if (vIndiceDataTable >= 0)
      {
        //Buscar un registro de Beneficiario
        for (int i = 0; i < vDatasetOrdenes.Tables[vIndiceDataTable].Rows.Count; i++)
        {
          string vProveedor = vDatasetOrdenes.Tables[vIndiceDataTable].Rows[i][3].ToString();
          if ("BENEFICIARIO" == vProveedor.ToUpper())
          {
            vResultado = i;
            ButtonRepaCambioBenef.Visible = true;
            break;
          }
        }
      }

      return vResultado;
    }

    private int FValidaRepuBeneficiario()
    {
      int vResultado = -1;
      int vIdFlujo = 0;
      int vIdCotizacion = 0;
      short vTipoItem = 0;

      vIdFlujo = int.Parse(TextBoxIdFlujo.Text); ;
      vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);
      vTipoItem = (short)CotizacionICRL.TipoItem.Repuesto;

      BD.CotizacionICRL.TipoRCVehicularSumatoriaTraer vTipoRCVehicularSumatoriaTraer;
      vTipoRCVehicularSumatoriaTraer = CotizacionICRL.RCVehicularSumatoriaTraer(vIdFlujo, vIdCotizacion, vTipoItem);
      DataSet vDatasetOrdenes = vTipoRCVehicularSumatoriaTraer.dsRCVehicularSumatoria;
      int vIndiceDataTable = vDatasetOrdenes.Tables.Count - 1;

      if (vIndiceDataTable >= 0)
      {
        //Buscar un registro de Beneficiario
        for (int i = 0; i < vDatasetOrdenes.Tables[vIndiceDataTable].Rows.Count; i++)
        {
          string vProveedor = vDatasetOrdenes.Tables[vIndiceDataTable].Rows[i][3].ToString();
          if ("BENEFICIARIO" == vProveedor.ToUpper())
          {
            vResultado = i;
            ButtonRepuCambioBenef.Visible = true;
            break;
          }
        }
      }

      return vResultado;
    }

    protected void ButtonBenefCambiar_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      int vIdFlujo = 0;
      int vIdCotizacion = 0;
      short vTipoItem = 0;
      int vIndiceBenef = -1;
      AccesoDatos vAccesoDatos = new AccesoDatos();

      vIdFlujo = int.Parse(TextBoxIdFlujo.Text); ;
      vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);
      vTipoItem = short.Parse(TextBoxBenefTipoItem.Text);
      vIndiceBenef = int.Parse(TextBoxBenefIndice.Text);

      if (string.Empty != TextBoxBeneficiario.Text)
      {
        //Cambiar el nombre del Beneficiario en la tabla de Sumatorias
        BD.CotizacionICRL.TipoRCVehicularSumatoriaTraer vTipoRCVehicularSumatoriaTraer;
        vTipoRCVehicularSumatoriaTraer = CotizacionICRL.RCVehicularSumatoriaTraer(vIdFlujo, vIdCotizacion, vTipoItem);
        DataSet vDatasetOrdenes = vTipoRCVehicularSumatoriaTraer.dsRCVehicularSumatoria;
        int vIndiceDataTable = vDatasetOrdenes.Tables.Count - 1;

        if (vIndiceDataTable >= 0)
        {
          //Generar el nuevo numero de Orden como si fuera Orden de Pago
          //generar numero de orden
          StringBuilder vSBNumeroOrden = new StringBuilder();
          string vNumeroOrden = string.Empty;
          vSBNumeroOrden.Clear();
          vSBNumeroOrden.Append("OP-");
          vNumeroOrden = TextBoxNroFlujo.Text.Trim();
          vNumeroOrden = vNumeroOrden.PadLeft(6, '0');
          vSBNumeroOrden.Append(vNumeroOrden);
          vSBNumeroOrden.Append("-VE-");
          vNumeroOrden = vIdCotizacion.ToString();
          vNumeroOrden = vNumeroOrden.PadLeft(6, '0');
          vSBNumeroOrden.Append(vNumeroOrden);
          vSBNumeroOrden.Append("-");
          if ((short)CotizacionICRL.TipoItem.Reparacion == vTipoItem)
          {
            vSBNumeroOrden.Append("T1");
          }
          else
          {
            vSBNumeroOrden.Append("C1");
          }
          vNumeroOrden = vSBNumeroOrden.ToString();

          vDatasetOrdenes.Tables[vIndiceDataTable].Rows[vIndiceBenef][9] = vNumeroOrden;
          BD.CotizacionICRL.RCVehicularSumatoriaModificarTodos(vDatasetOrdenes);

          //Actualizar el numero de orden en la tabla de RCVehicular
          vAccesoDatos.fActualizaOrdenesCotiVE(vIdFlujo, vIdCotizacion, "BENEFICIARIO", vTipoItem);

          //Actualizar un registro de Beneficiario
          vTipoRCVehicularSumatoriaTraer = CotizacionICRL.RCVehicularSumatoriaTraer(vIdFlujo, vIdCotizacion, vTipoItem);
          vDatasetOrdenes = vTipoRCVehicularSumatoriaTraer.dsRCVehicularSumatoria;
          vIndiceDataTable = vDatasetOrdenes.Tables.Count - 1;

          if (vIndiceDataTable >= 0)
          {
            string vProveedor = vDatasetOrdenes.Tables[vIndiceDataTable].Rows[vIndiceBenef][3].ToString();
            string vNombreBeneficiario = TextBoxBeneficiario.Text.ToUpper().Trim();

            BD.CotizacionICRL.RCVehicularSumatoriaModificarTodos(vDatasetOrdenes, vNombreBeneficiario);
          }
          if (1 == vTipoItem)
          {
            ButtonRepaCambioBenef.Visible = false;
          }
          else
          {
            ButtonRepuCambioBenef.Visible = false;
          }
          FLlenarGrillaOrdenes(vIdFlujo, vIdCotizacion, vTipoItem);
          Session["PopupBeneficiario"] = 0;
          this.ModalPopupBeneficiario.Hide();
        }

      }
      else
      {
        //Si el nombre del Beneficiario esta vacio no se hace el cambio
        LabelMsjBenef.Text = "El nombre del Beneficiario no puede estar vacío";

      }

    }

    protected void ButtonBenefCancelar_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      ButtonBenefCambiar.Enabled = false;
      ButtonBenefCancelar.Enabled = false;
    }

    protected void ButtonCancelPopBeneficiario_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      Session["PopupBeneficiario"] = 0;
      this.ModalPopupBeneficiario.Hide();
    }

    protected void ButtonRepaCambioBenef_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      int vIndiceBenef = -1;
      int vTipoItem = (int)CotizacionICRL.TipoItem.Reparacion;

      LabelMsjBenef.Text = string.Empty;

      vIndiceBenef = FValidaRepaBeneficiario();
      if (vIndiceBenef >= 0)
      {
        TextBoxBenefIndice.Text = vIndiceBenef.ToString();
        TextBoxBenefTipoItem.Text = vTipoItem.ToString();
        ButtonBenefCambiar.Enabled = true;
        ButtonBenefCancelar.Enabled = true;
        Session["PopupBeneficiario"] = 1;
        this.ModalPopupBeneficiario.Show();
      }
    }

    protected void ButtonRepuCambioBenef_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      int vIndiceBenef = -1;
      int vTipoItem = (int)CotizacionICRL.TipoItem.Repuesto;

      LabelMsjBenef.Text = string.Empty;

      vIndiceBenef = FValidaRepuBeneficiario();
      if (vIndiceBenef >= 0)
      {
        TextBoxBenefIndice.Text = vIndiceBenef.ToString();
        TextBoxBenefTipoItem.Text = vTipoItem.ToString();
        ButtonBenefCambiar.Enabled = true;
        ButtonBenefCancelar.Enabled = true;
        Session["PopupBeneficiario"] = 1;
        this.ModalPopupBeneficiario.Show();
      }
    }

    #endregion

    //Actualizar los precios de los items
    protected void ButtonActualizaTaller_Click(object sender, EventArgs e)
    {
      AccesoDatos vAccesoDatos = new AccesoDatos();
      int vResultado = 0;
      int vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
      int vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);
      //Actualiza el Tipo de Taller 
      string vTipoTaller = string.Empty;
      vTipoTaller = DropDownListTipoTallerCoti.SelectedItem.Text.Trim();
      vResultado = vAccesoDatos.FActualizaTipoTallerCotizacion(vIdCotizacion, vTipoTaller);

      if (1 == vResultado)
      {
        //Obtener el Tipo de Cambio
        float vTipoCambioConv = 0;
        float vTipoCambio = 1;
        if (float.TryParse(TextBoxTipoCambio.Text, out vTipoCambioConv))
        {
          vTipoCambio = vTipoCambioConv;
        }
        vAccesoDatos.fActualizaPrecioItemRCVehicular(vIdFlujo, vIdCotizacion, vTipoCambio);
      }
      //Actualizar datos de las grillas Reparacion y Repuestos
      FlTraeDatosDPReparacion(vIdCotizacion);
      FlTraeDatosDPRepuesto(vIdCotizacion);
    }

  }
}