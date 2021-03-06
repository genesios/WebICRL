﻿using System;
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
  public partial class CotizacionDP : System.Web.UI.Page
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

          int vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
          short vTipoItem = (short)CotizacionICRL.TipoItem.Reparacion;

          FlTraeDatosDPReparacion(vIdCotizacion);
          FlTraeDatosDPRepuesto(vIdCotizacion);
          FlTraeDatosSumatoriaReparaciones(vIdFlujo, vIdCotizacion, vTipoItem);
          FLlenarGrillaOrdenes(vIdFlujo, vIdCotizacion, vTipoItem);

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

        if (Session["PopupTallerGen"] != null)
        {
          int vPopup = -1;
          vPopup = int.Parse(Session["PopupTallerGen"].ToString());
          if (1 == vPopup)
            this.ModalPopupTallerGen.Show();
          else
            this.ModalPopupTallerGen.Hide();
        }

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
                       u.correoElectronico
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

    //Anterior método de llenar la grilla Reparacion
    //private int FlTraeDatosDPReparacion(int pIdCotizacion)
    //{
    //    int vResultado = 0;

    //    using (LBCDesaEntities db = new LBCDesaEntities())
    //    {
    //        var vLst = from c in db.Cotizacion
    //                   join cotrepa in db.CotiReparacion on c.idCotizacion equals cotrepa.idCotizacion
    //                   join n in db.Nomenclador on cotrepa.idItem equals n.codigo
    //                   where c.idCotizacion == pIdCotizacion && n.categoriaNomenclador == "Item"
    //                   select new
    //                   {
    //                       n.descripcion,
    //                       cotrepa.chaperio,
    //                       cotrepa.reparacionPrevia,
    //                       cotrepa.mecanico,
    //                       cotrepa.moneda,
    //                       cotrepa.precioCotizado,
    //                       cotrepa.descFijoPorcentaje,
    //                       cotrepa.montoDescuento,
    //                       cotrepa.precioFinal,
    //                       cotrepa.proveedor
    //                   };

    //        GridViewReparaciones.DataSource = vLst.ToList();
    //        GridViewReparaciones.DataBind();

    //    }

    //    return vResultado;
    //}
    private int FlTraeDatosDPReparacion(int pIdCotizacion)
    {
      int vResultado = 0;
      int vIdFlujo = int.Parse(TextBoxIdFlujo.Text);

      BD.CotizacionICRL.TipoDaniosPropiosTraer vTipoDaniosPropiosTraer;
      vTipoDaniosPropiosTraer = CotizacionICRL.DaniosPropiosTraer(vIdFlujo, pIdCotizacion);

      GridViewReparaciones.DataSource = vTipoDaniosPropiosTraer.DaniosPropios.Select(DaniosPropios => new
      {
        DaniosPropios.id_item,
        DaniosPropios.item_descripcion,
        DaniosPropios.chaperio,
        DaniosPropios.reparacion_previa,
        DaniosPropios.mecanico,
        DaniosPropios.id_moneda,
        DaniosPropios.precio_cotizado,
        DaniosPropios.id_tipo_descuento,
        DaniosPropios.descuento,
        DaniosPropios.precio_final,
        DaniosPropios.proveedor,
        DaniosPropios.id_tipo_item

      }).Where(DaniosPropios => DaniosPropios.id_tipo_item == 1).ToList();
      GridViewReparaciones.DataBind();

      return vResultado;
    }

    //Anterior método de llenar la grilla Repuesto
    //private int FlTraeDatosDPRepuesto(int pIdCotizacion)
    //{
    //    int vResultado = 0;

    //    using (LBCDesaEntities db = new LBCDesaEntities())
    //    {
    //        var vLst = from c in db.Cotizacion
    //                   join cotrepu in db.CotiRepuesto on c.idCotizacion equals cotrepu.idCotizacion
    //                   join n in db.Nomenclador on cotrepu.idItem equals n.codigo
    //                   where c.idCotizacion == pIdCotizacion && n.categoriaNomenclador == "Item"
    //                   select new
    //                   {
    //                       n.descripcion,
    //                       cotrepu.pintura,
    //                       cotrepu.instalacion,
    //                       cotrepu.moneda,
    //                       cotrepu.precioCotizado,
    //                       cotrepu.descFijoPorcentaje,
    //                       cotrepu.montoDescuento,
    //                       cotrepu.precioFinal,
    //                       cotrepu.proveedor
    //                   };

    //        GridViewRepuestos.DataSource = vLst.ToList();
    //        GridViewRepuestos.DataBind();

    //    }

    //    return vResultado;
    //}

    private int FlTraeDatosDPRepuesto(int pIdCotizacion)
    {
      int vResultado = 0;
      int vIdFlujo = int.Parse(TextBoxIdFlujo.Text);

      BD.CotizacionICRL.TipoDaniosPropiosTraer vTipoDaniosPropiosTraer;
      vTipoDaniosPropiosTraer = CotizacionICRL.DaniosPropiosTraer(vIdFlujo, pIdCotizacion);

      GridViewRepuestos.DataSource = vTipoDaniosPropiosTraer.DaniosPropios.Select(DaniosPropios => new
      {
        DaniosPropios.id_item,
        DaniosPropios.item_descripcion,
        DaniosPropios.pintura,
        DaniosPropios.instalacion,
        DaniosPropios.id_moneda,
        DaniosPropios.precio_cotizado,
        DaniosPropios.id_tipo_descuento,
        DaniosPropios.descuento,
        DaniosPropios.precio_final,
        DaniosPropios.proveedor,
        DaniosPropios.id_tipo_item

      }).Where(DaniosPropios => DaniosPropios.id_tipo_item == 2).ToList();
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

    protected void PLimpiarCamposRepaAgregar()
    {
      DropDownListRepaItem.SelectedIndex = 0;
      DropDownListRepaChaperio.SelectedIndex = 0;
      DropDownListRepaRepPrevia.SelectedIndex = 0;
      CheckBoxRepaMecanico.Checked = false;
      //DropDownListRepaMoneda.SelectedIndex = 0;
      TextBoxRepaPrecioCotizado.Text = "0";
      DropDownListRepaTipoDesc.SelectedIndex = 0;
      TextBoxRepaMontoDesc.Text = "0";
      TextBoxRepaPrecioFinal.Text = "0";
      TextBoxRepaItem.Text = string.Empty;
      //DropDownListRepaProveedor.SelectedIndex = 0;
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
      BD.CotizacionICRL.TipoDaniosPropios vTipoDaniosPropios = new CotizacionICRL.TipoDaniosPropios();

      //Completar los elementos del objeto y grabar el registro.
      vTipoDaniosPropios.id_flujo = int.Parse(TextBoxIdFlujo.Text);
      vTipoDaniosPropios.id_cotizacion = int.Parse(TextBoxNroCotizacion.Text);

      //tipo_item:  1 = Reparacion  2 = Repuesto
      vTipoDaniosPropios.id_tipo_item = (int)CotizacionICRL.TipoItem.Reparacion;
      if ("A" == TextBoxRepaFlagEd.Text)
      {
        vTipoDaniosPropios.item_descripcion = DropDownListRepaItem.SelectedItem.Text.Trim();

        if (("OTRO (S)" == vTipoDaniosPropios.item_descripcion) && (!string.IsNullOrWhiteSpace(TextBoxRepaItem.Text)) )
        {
          vTipoDaniosPropios.item_descripcion = TextBoxRepaItem.Text.ToUpper();
        }
      }
      else
      {
        vTipoDaniosPropios.item_descripcion = TextBoxRepaItem.Text;
      }
      vTipoDaniosPropios.chaperio = DropDownListRepaChaperio.SelectedItem.Text.Trim();
      vTipoDaniosPropios.reparacion_previa = DropDownListRepaRepPrevia.SelectedItem.Text.Trim();
      vTipoDaniosPropios.mecanico = CheckBoxRepaMecanico.Checked;
      vTipoDaniosPropios.id_moneda = DropDownListRepaMoneda.SelectedItem.Text.Trim();
      vTipoDaniosPropios.precio_cotizado = double.Parse(TextBoxRepaPrecioCotizado.Text);
      vTipoDaniosPropios.id_tipo_descuento = DropDownListRepaTipoDesc.SelectedItem.Text.Trim();
      vTipoDaniosPropios.descuento = double.Parse(TextBoxRepaMontoDesc.Text);
      switch (vTipoDaniosPropios.id_tipo_descuento)
      {
        case "Fijo":
          vTipoDaniosPropios.precio_final = vTipoDaniosPropios.precio_cotizado - vTipoDaniosPropios.descuento;
          break;
        case "Porcentaje":
          vTipoDaniosPropios.precio_final = vTipoDaniosPropios.precio_cotizado - (vTipoDaniosPropios.precio_cotizado * (vTipoDaniosPropios.descuento / 100));
          break;
        default:
          vTipoDaniosPropios.precio_final = vTipoDaniosPropios.precio_cotizado;
          break;
      }
      vTipoDaniosPropios.proveedor = DropDownListRepaProveedor.SelectedItem.Text.Trim();
      vTipoDaniosPropios.id_estado = 1;

      double vTipoCambio = 0;
      vTipoCambio = double.Parse(TextBoxTipoCambio.Text);
      vTipoDaniosPropios.tipo_cambio = vTipoCambio;

      bool vResultado = false;
      if (string.Empty != TextBoxRepaIdItem.Text)
      {
        vTipoDaniosPropios.id_item = int.Parse(TextBoxRepaIdItem.Text);
        vResultado = BD.CotizacionICRL.DaniosPropiosModificar(vTipoDaniosPropios);
        if (vResultado)
        {
          LabelRepaRegistroItems.Text = "Registro modificado exitosamente";
          PLimpiarCamposRepa();
          //PanelABMReparaciones.Enabled = false;
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
        vResultado = BD.CotizacionICRL.DaniosPropiosRegistrar(vTipoDaniosPropios);
        if (vResultado)
        {
          LabelRepaRegistroItems.Text = "Registro añadido exitosamente";
          PLimpiarCamposRepaAgregar();
          //PanelABMReparaciones.Enabled = false;
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
      vResultado = BD.CotizacionICRL.DaniosPropiosBorrar(vIdFlujo, vIdCotizacion, vIdItem);
      if (vResultado)
      {
        LabelRepaRegistroItems.Text = "Registro Borrado exitosamente";
        PLimpiarCamposRepa();
        //PanelABMReparaciones.Enabled = false;
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
      TextBoxRepaIdItem.Text = GridViewReparaciones.SelectedRow.Cells[1].Text;
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
      vTextoTemporal = GridViewReparaciones.SelectedRow.Cells[6].Text.Trim();

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

    protected void PLimpiarCamposRepuAgregar()
    {
      DropDownListRepuItem.SelectedIndex = 0;
      CheckBoxRepuPintura.Checked = false;
      CheckBoxRepuInstalacion.Checked = false;
      //DropDownListRepuMoneda.SelectedIndex = 0;
      TextBoxRepuPrecioCotizado.Text = "0";
      DropDownListRepuTipoDesc.SelectedIndex = 0;
      TextBoxRepuMontoDesc.Text = "0";
      TextBoxRepuPrecioFinal.Text = "0";
      TextBoxRepuItem.Text = string.Empty;
      //DropDownListRepuProveedor.SelectedIndex = 0;
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
      BD.CotizacionICRL.TipoDaniosPropios vTipoDaniosPropios = new CotizacionICRL.TipoDaniosPropios();

      //Completar los elementos del objeto y grabar el registro.
      vTipoDaniosPropios.id_flujo = int.Parse(TextBoxIdFlujo.Text);
      vTipoDaniosPropios.id_cotizacion = int.Parse(TextBoxNroCotizacion.Text);

      //tipo_item:  1 = Repuracion  2 = Repuesto
      vTipoDaniosPropios.id_tipo_item = (int)CotizacionICRL.TipoItem.Repuesto;
      if ("A" == TextBoxRepuFlagEd.Text)
      {
        vTipoDaniosPropios.item_descripcion = DropDownListRepuItem.SelectedItem.Text.Trim();

        if (("OTRO (S)" == vTipoDaniosPropios.item_descripcion) && (!string.IsNullOrWhiteSpace(TextBoxRepuItem.Text)))
        {
          vTipoDaniosPropios.item_descripcion = TextBoxRepuItem.Text.ToUpper();
        }
      }
      else
      {
        vTipoDaniosPropios.item_descripcion = TextBoxRepuItem.Text;
      }
      vTipoDaniosPropios.pintura = CheckBoxRepuPintura.Checked;
      vTipoDaniosPropios.instalacion = CheckBoxRepuInstalacion.Checked;
      vTipoDaniosPropios.id_moneda = DropDownListRepuMoneda.SelectedItem.Text.Trim();
      vTipoDaniosPropios.precio_cotizado = double.Parse(TextBoxRepuPrecioCotizado.Text);
      vTipoDaniosPropios.id_tipo_descuento = DropDownListRepuTipoDesc.SelectedItem.Text.Trim();
      vTipoDaniosPropios.descuento = double.Parse(TextBoxRepuMontoDesc.Text);
      switch (vTipoDaniosPropios.id_tipo_descuento)
      {
        case "Fijo":
          vTipoDaniosPropios.precio_final = vTipoDaniosPropios.precio_cotizado - vTipoDaniosPropios.descuento;
          break;
        case "Porcentaje":
          vTipoDaniosPropios.precio_final = vTipoDaniosPropios.precio_cotizado - (vTipoDaniosPropios.precio_cotizado * (vTipoDaniosPropios.descuento / 100));
          break;
        default:
          vTipoDaniosPropios.precio_final = vTipoDaniosPropios.precio_cotizado;
          break;
      }
      vTipoDaniosPropios.proveedor = DropDownListRepuProveedor.SelectedItem.Text.Trim();
      vTipoDaniosPropios.id_estado = 1;

      double vTipoCambio = 0;
      vTipoCambio = double.Parse(TextBoxTipoCambio.Text);
      vTipoDaniosPropios.tipo_cambio = vTipoCambio;


      bool vResultado = false;
      if (string.Empty != TextBoxRepuIdItem.Text)
      {
        vTipoDaniosPropios.id_item = int.Parse(TextBoxRepuIdItem.Text);
        vResultado = BD.CotizacionICRL.DaniosPropiosModificar(vTipoDaniosPropios);
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
        vResultado = BD.CotizacionICRL.DaniosPropiosRegistrar(vTipoDaniosPropios);
        if (vResultado)
        {
          LabelRepuRegistroItems.Text = "Registro añadido exitosamente";
          PLimpiarCamposRepuAgregar();
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
      FlTraeDatosRecepRepu(vIdCotizacion);

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
      vResultado = BD.CotizacionICRL.DaniosPropiosBorrar(vIdFlujo, vIdCotizacion, vIdItem);
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
      FlTraeDatosRecepRepu(vIdCotizacion);
    }

    protected void GridViewRepuestos_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      string vTextoTemporal = string.Empty;

      //Leer Registro de la grilla y cargar los valores a la ventana.
      TextBoxRepuIdItem.Text = GridViewRepuestos.SelectedRow.Cells[1].Text;
      //tipo_item:  1 = Repuracion  2 = Repuesto
      vTextoTemporal = string.Empty;
      vTextoTemporal = GridViewRepuestos.SelectedRow.Cells[2].Text.Trim();
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

      BD.CotizacionICRL.TipoDaniosPropiosSumatoriaTraer vTipoDaniosPropiosSumatoriaTraer;
      vTipoDaniosPropiosSumatoriaTraer = CotizacionICRL.DaniosPropiosSumatoriaTraer(pIdFlujo, pIdCotizacion, pTipoItem);

      //ajustes 2020/02/02 wap se añade el campo cotizacion_proveedor

      GridViewSumaReparaciones.DataSource = vTipoDaniosPropiosSumatoriaTraer.DaniosPropiosSumatoria.Select(DaniosPropiosSumatoria => new
      {
        DaniosPropiosSumatoria.proveedor,
        DaniosPropiosSumatoria.monto_orden,
        DaniosPropiosSumatoria.id_tipo_descuento_orden,
        DaniosPropiosSumatoria.descuento_proveedor,
        DaniosPropiosSumatoria.deducible,
        DaniosPropiosSumatoria.monto_final,
        DaniosPropiosSumatoria.moneda_orden,
        DaniosPropiosSumatoria.cotizacion_proveedor
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
      vProveedor = vAccesoDatos.fValidaMonedasSumatoriaDP(vIdFlujo, vIdCotizacion, vTipoItem);

      if (string.Empty != vProveedor)
      {
        LabelMsjReparaciones.Text = "No se puede procesar, El proveedor " + vProveedor + " tiene registros de ambas monedas, revise por favor";
        ButtonRepaGenerarOrdenes.Enabled = false;
      }
      else
      {
        LabelMsjReparaciones.Text = string.Empty;
        vResultado = CotizacionICRL.DaniosPropiosSumatoriaGenerar(vIdFlujo, vIdCotizacion, vTipoItem);
        if (vResultado)
        {
          LabelMsjReparaciones.Text = "Reparaciones sumarizadas exitosamente";
          ButtonRepaGenerarOrdenes.Enabled = true;
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
      CotizacionICRL.TipoDanioPropioSumatoria vTipoDanioPropioSumatoria = new CotizacionICRL.TipoDanioPropioSumatoria();

      //Completar los elementos del objeto y grabar el registro.
      vTipoDanioPropioSumatoria.id_flujo = int.Parse(TextBoxIdFlujo.Text);
      vTipoDanioPropioSumatoria.id_cotizacion = int.Parse(TextBoxNroCotizacion.Text);
      short vTipoItem = 0;
      vTipoItem = short.Parse(Session["TipoItem"].ToString());

      vTipoDanioPropioSumatoria.id_tipo_item = vTipoItem;

      vTipoDanioPropioSumatoria.proveedor = DropDownListSumaProveedor.SelectedItem.Text.Trim();
      vTipoDanioPropioSumatoria.monto_orden = double.Parse(TextBoxSumaMontoOrden.Text);
      vTipoDanioPropioSumatoria.id_tipo_descuento_orden = DropDownListSumaTipoDesc.SelectedItem.Text.Trim();
      vTipoDanioPropioSumatoria.descuento_proveedor = double.Parse(TextBoxSumaMontoDescProv.Text);
      vTipoDanioPropioSumatoria.deducible = double.Parse(TextBoxSumaDeducible.Text);
      switch (vTipoDanioPropioSumatoria.id_tipo_descuento_orden)
      {
        case "Fijo":
          vTipoDanioPropioSumatoria.monto_final = vTipoDanioPropioSumatoria.monto_orden - vTipoDanioPropioSumatoria.descuento_proveedor;
          break;
        case "Porcentaje":
          vTipoDanioPropioSumatoria.monto_final = vTipoDanioPropioSumatoria.monto_orden - (vTipoDanioPropioSumatoria.monto_orden * (vTipoDanioPropioSumatoria.descuento_proveedor / 100));
          break;
        default:
          vTipoDanioPropioSumatoria.monto_final = vTipoDanioPropioSumatoria.monto_orden;
          break;
      }

      //ajustes 2020/02/02 wap se añade el campo cotizacion_proveedor
      vTipoDanioPropioSumatoria.cotizacion_proveedor = TextBoxCotizacionProveedor.Text.ToUpper().Trim();

      bool vResultado = false;

      vResultado = BD.CotizacionICRL.DaniosPropiosSumatoriaModificar(vTipoDanioPropioSumatoria);
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

      //ajustes 2020/02/02 wap se añade el campo cotizacion_proveedor
      TextBoxCotizacionProveedor.Text = GridViewSumaReparaciones.SelectedRow.Cells[7].Text;

      Session["PopupABMSumasHabilitado"] = 1;
      this.ModalPopupSumatorias.Show();
    }

    private int FlTraeDatosSumatoriaRepuestos(int pIdFlujo, int pIdCotizacion, short pTipoItem)
    {
      int vResultado = 0;

      BD.CotizacionICRL.TipoDaniosPropiosSumatoriaTraer vTipoDaniosPropiosSumatoriaTraer;
      vTipoDaniosPropiosSumatoriaTraer = CotizacionICRL.DaniosPropiosSumatoriaTraer(pIdFlujo, pIdCotizacion, pTipoItem);

      //ajustes 2020/02/02 wap se añade el campo cotizacion_proveedor

      GridViewSumaRepuestos.DataSource = vTipoDaniosPropiosSumatoriaTraer.DaniosPropiosSumatoria.Select(DaniosPropiosSumatoria => new
      {
        DaniosPropiosSumatoria.proveedor,
        DaniosPropiosSumatoria.monto_orden,
        DaniosPropiosSumatoria.id_tipo_descuento_orden,
        DaniosPropiosSumatoria.descuento_proveedor,
        DaniosPropiosSumatoria.deducible,
        DaniosPropiosSumatoria.monto_final,
        DaniosPropiosSumatoria.moneda_orden,
        DaniosPropiosSumatoria.cotizacion_proveedor
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
      vProveedor = vAccesoDatos.fValidaMonedasSumatoriaDP(vIdFlujo, vIdCotizacion, vTipoItem);

      if (string.Empty != vProveedor)
      {
        LabelMsjRepuestos.Text = "No se puede procesar, El proveedor " + vProveedor + " tiene registros de ambas monedas, revise por favor";
        ButtonRepuGenerarOrdenes.Enabled = false;
      }
      else
      {
        LabelMsjRepuestos.Text = string.Empty;
        vResultado = CotizacionICRL.DaniosPropiosSumatoriaGenerar(vIdFlujo, vIdCotizacion, vTipoItem);
        if (vResultado)
        {
          LabelMsjRepuestos.Text = "Repuestos sumarizados exitosamente";
          ButtonRepuGenerarOrdenes.Enabled = true;
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

      //ajustes 2020/02/02 wap se añade el campo cotizacion_proveedor
      TextBoxCotizacionProveedor.Text = GridViewSumaRepuestos.SelectedRow.Cells[7].Text;

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
      TextBoxRecepDiasEntrega.Text = "0";

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

      BD.CotizacionICRL.TipoDaniosPropiosTraer vTipoDaniosPropiosTraer;
      vTipoDaniosPropiosTraer = CotizacionICRL.DaniosPropiosTraer(vIdFlujo, pIdCotizacion);

      GridViewRecepRepuestos.DataSource = vTipoDaniosPropiosTraer.DaniosPropios.Select(DaniosPropios => new
      {
        DaniosPropios.id_item,
        DaniosPropios.item_descripcion,
        DaniosPropios.recepcion,
        DaniosPropios.dias_entrega,
        DaniosPropios.id_tipo_item
      }).Where(DaniosPropios => DaniosPropios.id_tipo_item == 2).ToList();
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
      vTextoTemporal = GridViewRecepRepuestos.SelectedRow.Cells[1].Text;
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
      BD.CotizacionICRL.TipoDaniosPropios vTipoDaniosPropios = new CotizacionICRL.TipoDaniosPropios();

      vIdFlujo = int.Parse(TextBoxIdFlujo.Text); ;
      vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);
      vIdItem = long.Parse(TextBoxRecepIdItem.Text);

      //Para no perder los datos originales recuperamos el registro afectado primero y luego actualizamos los datos correspondientes
      BD.CotizacionICRL.TipoDaniosPropiosTraer vTipoDaniosPropiosTraer;
      vTipoDaniosPropiosTraer = CotizacionICRL.DaniosPropiosTraer(vIdFlujo, vIdCotizacion, vIdItem);

      //Proceder solo si se trajo correctamente la información.
      if (vTipoDaniosPropiosTraer.Correcto)
      {
        vTipoDaniosPropios = vTipoDaniosPropiosTraer.DaniosPropios[0];

        //Solo actualizar los datos que se pudieron modificar
        vTipoDaniosPropios.recepcion = CheckBoxRecepRecibido.Checked;
        vTipoDaniosPropios.dias_entrega = int.Parse(TextBoxRecepDiasEntrega.Text);

        bool vResultado = false;

        vResultado = BD.CotizacionICRL.DaniosPropiosModificar(vTipoDaniosPropios);
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

    private int FLlenarGrillaOrdenes(int pIdFlujo, int pIdCotizacion, short pTipoItem)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vLst = from cdps in db.cotizacion_danios_propios_sumatoria
                   where (cdps.id_flujo == pIdFlujo)
                      && (cdps.id_cotizacion == pIdCotizacion)
                   select new
                   {
                     cdps.numero_orden,
                     cdps.id_estado,
                     cdps.proveedor,
                     cdps.moneda_orden,
                     cdps.monto_orden,
                     cdps.id_tipo_descuento_orden,
                     cdps.descuento_proveedor,
                     cdps.deducible,
                     cdps.monto_final,
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
        vEstadoCadena = e.Row.Cells[1].Text.Trim();
        vEstado = int.Parse(vEstadoCadena);
        if (1 == vEstado)
        {
          (e.Row.Cells[11].Controls[0] as LinkButton).Enabled = true;
          //validar si el Proveedor es Beneficiario
          string vNombreProveedor = string.Empty;
          vNombreProveedor = e.Row.Cells[2].Text.Trim().ToUpper();
          vNombreProveedor = vNombreProveedor.Replace("&NBSP;", string.Empty);
          vNombreProveedor = vNombreProveedor.Replace("&#209;", "Ñ");
          if ("BENEFICIARIO" == vNombreProveedor.ToUpper())
          {
            (e.Row.Cells[11].Controls[0] as LinkButton).Enabled = false;
          }
          else if ("TGENERICO" == vNombreProveedor.ToUpper())
          {
            (e.Row.Cells[11].Controls[0] as LinkButton).Enabled = false;
          }
          else if (string.Empty == vNombreProveedor.ToUpper())
          {
            (e.Row.Cells[11].Controls[0] as LinkButton).Enabled = false;
          }
          else
          {
            (e.Row.Cells[11].Controls[0] as LinkButton).Enabled = true;
            //ConfirmarFinalizarCotizacion
            (e.Row.Cells[11].Controls[0] as LinkButton).Attributes.Add("OnClick", "javascript:return ConfirmarFinalizarCotizacion()");
          }
        }
        else
        {
          (e.Row.Cells[11].Controls[0] as LinkButton).Enabled = false;
        }
        //validar que el campo nro_orden no este vacio, en ese caso deshabilitar los botones
        string vNroOrden = string.Empty;
        vNroOrden = e.Row.Cells[0].Text.Trim().ToUpper();
        vNroOrden = vNroOrden.Replace("&NBSP;", string.Empty);
        if (string.Empty == vNroOrden)
        {
          (e.Row.Cells[9].Controls[0] as LinkButton).Enabled = false;
          (e.Row.Cells[10].Controls[0] as LinkButton).Enabled = false;
          (e.Row.Cells[11].Controls[0] as LinkButton).Enabled = false;
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
        PImprimeFormularioCotiDaniosPropios(vNumeroOrden);
      }

      if (0 == e.CommandName.CompareTo("Ver"))
      {
        string vTextoSecuencial = string.Empty;
        vIndex = 0;

        vIndex = Convert.ToInt32(e.CommandArgument);
        vNumeroOrden = (string)GridViewOrdenes.DataKeys[vIndex].Value;
        vProveedor = GridViewOrdenes.Rows[vIndex].Cells[2].Text;
        ButtonCierraVerRep.Visible = true;
        PVerFormularioCotiDaniosPropios(vNumeroOrden);
      }

      if (0 == e.CommandName.CompareTo("SubirOnBase"))
      {
        int vResultado = 0;
        string vTextoSecuencial = string.Empty;


        vIndex = Convert.ToInt32(e.CommandArgument);
        vNumeroOrden = (string)GridViewOrdenes.DataKeys[vIndex].Value;
        vProveedor = GridViewOrdenes.Rows[vIndex].Cells[2].Text;
        vProveedor = vProveedor.Replace("&#209;", "Ñ");


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

        vResultado = vAccesoDatos.fActualizaLiquidacionDP(vIdFlujo, vIdCotizacion, vProveedor, vTipoItem);
        PSubeFormularioCotiDaniosPropios(vNumeroOrden);
        vResultado = vAccesoDatos.FCotizacionDaniosPropiosCambiaEstadoSumatoria(vIdFlujo, vIdCotizacion, (short)vTipoItem, vProveedor);
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
      ButtonRepuCambioTallerGen.Visible = false;

      //verificar si existe un Beneficiario
      int vIndiceBenef = FValidaRepuBeneficiario();

      //verificar si existe un TallerGenerico
      int vIndiceTallerGen = FValidaRepuTallerGen();

      if ((vIndiceBenef >= 0) && (vIndiceTallerGen >= 0))
      {
        LabelMsjRepuestos .Text = "NO se puede tener registros BENEFICIARIO y TGENERICO al mismo tiempo, revise por favor";
        ButtonRepuGenerarOrdenes.Enabled = false;
        ButtonRepuCambioBenef.Enabled = false;
        ButtonRepuCambioTallerGen.Enabled = false;
      }
      else
      {
        ButtonRepuGenerarOrdenes.Enabled = true;
        ButtonRepuCambioBenef.Enabled = true;
        ButtonRepuCambioTallerGen.Enabled = true;

        vIdFlujo = int.Parse(TextBoxIdFlujo.Text); ;
        vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);
        vTipoItem = (short)CotizacionICRL.TipoItem.Repuesto;

        BD.CotizacionICRL.TipoDaniosPropiosSumatoriaTraer vTipoDaniosPropiosSumatoriaTraer;
        vTipoDaniosPropiosSumatoriaTraer = CotizacionICRL.DaniosPropiosSumatoriaTraer(vIdFlujo, vIdCotizacion, vTipoItem);
        DataSet vDatasetOrdenes = vTipoDaniosPropiosSumatoriaTraer.dsDaniosPropiosSumatoria;
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
            vSBNumeroOrden.Append("-DP-");
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

        BD.CotizacionICRL.DaniosPropiosSumatoriaModificarTodos(vDatasetOrdenes);

        //actualizamos el detalle de las ordenes generadas
        vTipoDaniosPropiosSumatoriaTraer = CotizacionICRL.DaniosPropiosSumatoriaTraer(vIdFlujo, vIdCotizacion, vTipoItem);
        vDatasetOrdenes = vTipoDaniosPropiosSumatoriaTraer.dsDaniosPropiosSumatoria;
        vIndiceDataTable = vDatasetOrdenes.Tables.Count - 1;

        if (vIndiceDataTable >= 0)
        {
          AccesoDatos vAccesoDatos = new AccesoDatos();
          for (int i = 0; i < vDatasetOrdenes.Tables[vIndiceDataTable].Rows.Count; i++)
          {
            string vProveedor = string.Empty;
            vProveedor = vDatasetOrdenes.Tables[vIndiceDataTable].Rows[i][3].ToString();
            vProveedor = vProveedor.Replace("&#209;", "Ñ");
            vAccesoDatos.fActualizaOrdenesCotiDP(vIdFlujo, vIdCotizacion, vProveedor, vTipoItem);
            vContador++;
          }
        }

        FLlenarGrillaOrdenes(vIdFlujo, vIdCotizacion, vTipoItem);
      }

    }



    protected void ButtonRepaGenerarOrdenes_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      int vIdFlujo = 0;
      int vIdCotizacion = 0;
      short vTipoItem = 0;
      int vContador = 1;

      ButtonRepaCambioBenef.Visible = false;
      ButtonRepaCambioTallerGen.Visible = false;

      //verificar si existe un Beneficiario
      int vIndiceBenef = FValidaRepaBeneficiario();

      //verificar si existe un TallerGenerico
      int vIndiceTallerGen = FValidaRepaTallerGen();

      if ((vIndiceBenef >= 0) && (vIndiceTallerGen >= 0))
      {
        LabelMsjReparaciones.Text = "NO se puede tener registros BENEFICIARIO y TGENERICO al mismo tiempo, revise por favor";
        ButtonRepaGenerarOrdenes.Enabled = false;
        ButtonRepaCambioBenef.Enabled = false;
        ButtonRepaCambioTallerGen.Enabled = false;
      }
      else
      {
        ButtonRepaGenerarOrdenes.Enabled = true;
        ButtonRepaCambioBenef.Enabled = true;
        ButtonRepaCambioTallerGen.Enabled = true;


        vIdFlujo = int.Parse(TextBoxIdFlujo.Text); ;
        vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);
        vTipoItem = (short)CotizacionICRL.TipoItem.Reparacion;

        BD.CotizacionICRL.TipoDaniosPropiosSumatoriaTraer vTipoDaniosPropiosSumatoriaTraer;
        vTipoDaniosPropiosSumatoriaTraer = CotizacionICRL.DaniosPropiosSumatoriaTraer(vIdFlujo, vIdCotizacion, vTipoItem);
        DataSet vDatasetOrdenes = vTipoDaniosPropiosSumatoriaTraer.dsDaniosPropiosSumatoria;
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
            vSBNumeroOrden.Append("-DP-");
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

        BD.CotizacionICRL.DaniosPropiosSumatoriaModificarTodos(vDatasetOrdenes);

        //actualizamos el detalle de las ordenes generadas
        vTipoDaniosPropiosSumatoriaTraer = CotizacionICRL.DaniosPropiosSumatoriaTraer(vIdFlujo, vIdCotizacion, vTipoItem);
        vDatasetOrdenes = vTipoDaniosPropiosSumatoriaTraer.dsDaniosPropiosSumatoria;
        vIndiceDataTable = vDatasetOrdenes.Tables.Count - 1;

        if (vIndiceDataTable >= 0)
        {
          AccesoDatos vAccesoDatos = new AccesoDatos();
          for (int i = 0; i < vDatasetOrdenes.Tables[vIndiceDataTable].Rows.Count; i++)
          {
            string vProveedor = string.Empty;
            vProveedor = vDatasetOrdenes.Tables[vIndiceDataTable].Rows[i][3].ToString();
            vProveedor = vProveedor.Replace("&#209;", "Ñ");
            vAccesoDatos.fActualizaOrdenesCotiDP(vIdFlujo, vIdCotizacion, vProveedor, vTipoItem);
            vContador++;
          }
        }

        FLlenarGrillaOrdenes(vIdFlujo, vIdCotizacion, vTipoItem);
      }

    }

    protected void PImprimeFormularioCotiDaniosPropios(string pNroOrden)
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
      string fileName = "RepFormCotiDaniosPropios" + pNroOrden;

      var vListaFlujo = from f in db.Flujo
                        join s in db.cotizacion_danios_propios_sumatoria on f.idFlujo equals s.id_flujo
                        where (s.numero_orden == pNroOrden)
                        select new
                        {
                          f.nombreAsegurado,
                          f.telefonocelAsegurado,
                          cobertura = "DAÑOS PROPIOS",
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

      var vListaCotiDaniosPropios = from c in db.cotizacion_danios_propios
                                    join f in db.Flujo on c.id_flujo equals f.idFlujo
                                    where (c.numero_orden == pNroOrden)
                                    orderby c.id_item
                                    select new
                                    {
                                      f.nombreAsegurado,
                                      f.telefonocelAsegurado,
                                      cobertura = "DAÑOS PROPIOS",
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

      //ajustes 2020/02/02 wap se añade el campo cotizacion_proveedor

      var vListaCotiSumaDaniosPropios = from c in db.cotizacion_danios_propios_sumatoria
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
                                          c.monto_final,
                                          c.cotizacion_proveedor
                                        };

      var vListaCotizacionUsuario = from cc in db.cotizacion_danios_propios
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

      var vListaUsuarioFirma =      from cc in db.cotizacion_danios_propios
                                    join c in db.Cotizacion on cc.id_cotizacion equals c.idCotizacion
                                    join uf in db.UsuarioFirma on c.idUsuario equals uf.idUsuario
                                    where (cc.numero_orden == pNroOrden)
                                    select new
                                    {
                                      uf.idUsuario,
                                      uf.estado,
                                      uf.usuarioCreacion,
                                      uf.fechaCreacion,
                                      uf.firmaSello
                                    };

      ReportViewerCoti.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;

      if ("OT" == pNroOrden.Substring(0, 2))
      {
        ReportViewerCoti.LocalReport.ReportPath = "Reportes\\RepFormularioCotiOrdenTrabajoFirma.rdlc";
      }
      else
      {
        if ("OC" == pNroOrden.Substring(0, 2))
        {
          ReportViewerCoti.LocalReport.ReportPath = "Reportes\\RepFormularioCotiOrdenCompraFirma.rdlc";
        }
        else
        {
          ReportViewerCoti.LocalReport.ReportPath = "Reportes\\RepFormularioCotiOrdenPagoCambioFirma.rdlc";
        }
      }

      //ReportViewerCoti.LocalReport.ReportPath = "Reportes\\RepFormularioCotiDaniosPropios.rdlc";
      ReportDataSource datasource1 = new ReportDataSource("DataSet1", vListaFlujo);
      ReportDataSource datasource2 = new ReportDataSource("DataSet2", vListaCotiDaniosPropios);
      ReportDataSource datasource3 = new ReportDataSource("DataSet3", vListaCotiSumaDaniosPropios);
      ReportDataSource datasource4 = new ReportDataSource("DataSet4", vListaCotizacionUsuario);
      ReportDataSource datasource5 = new ReportDataSource("DataSet5", vListaUsuarioFirma);

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

    protected void PVerFormularioCotiDaniosPropios(string pNroOrden)
    {
      AccesoDatos vAccesoDatos = new AccesoDatos();
      LBCDesaEntities db = new LBCDesaEntities();

      var vListaFlujo = from f in db.Flujo
                        join s in db.cotizacion_danios_propios_sumatoria on f.idFlujo equals s.id_flujo
                        where (s.numero_orden == pNroOrden)
                        select new
                        {
                          f.nombreAsegurado,
                          f.telefonocelAsegurado,
                          cobertura = "DAÑOS PROPIOS",
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

      var vListaCotiDaniosPropios = from c in db.cotizacion_danios_propios
                                    join f in db.Flujo on c.id_flujo equals f.idFlujo
                                    where (c.numero_orden == pNroOrden)
                                    orderby c.id_item
                                    select new
                                    {
                                      f.nombreAsegurado,
                                      f.telefonocelAsegurado,
                                      cobertura = "DAÑOS PROPIOS",
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
      //ajustes 2020/02/02 wap se añade el campo cotizacion_proveedor

      var vListaCotiSumaDaniosPropios = from c in db.cotizacion_danios_propios_sumatoria
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
                                          c.monto_final,
                                          c.cotizacion_proveedor
                                        };

      var vListaCotizacionUsuario = from cc in db.cotizacion_danios_propios
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

      var vListaUsuarioFirma = from cc in db.cotizacion_danios_propios
                               join c in db.Cotizacion on cc.id_cotizacion equals c.idCotizacion
                               join uf in db.UsuarioFirma on c.idUsuario equals uf.idUsuario
                               where (cc.numero_orden == pNroOrden)
                               select new
                               {
                                 uf.idUsuario,
                                 uf.estado,
                                 uf.usuarioCreacion,
                                 uf.fechaCreacion,
                                 uf.firmaSello
                               };

      ReportViewerCoti.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;

      if ("OT" == pNroOrden.Substring(0, 2))
      {
        ReportViewerCoti.LocalReport.ReportPath = "Reportes\\RepFormularioCotiOrdenTrabajoFirma.rdlc";
      }
      else
      {
        if ("OC" == pNroOrden.Substring(0, 2))
        {
          ReportViewerCoti.LocalReport.ReportPath = "Reportes\\RepFormularioCotiOrdenCompraFirma.rdlc";
        }
        else
        {
          ReportViewerCoti.LocalReport.ReportPath = "Reportes\\RepFormularioCotiOrdenPagoCambioFirma.rdlc";
        }
      }

      ReportDataSource datasource1 = new ReportDataSource("DataSet1", vListaFlujo);
      ReportDataSource datasource2 = new ReportDataSource("DataSet2", vListaCotiDaniosPropios);
      ReportDataSource datasource3 = new ReportDataSource("DataSet3", vListaCotiSumaDaniosPropios);
      ReportDataSource datasource4 = new ReportDataSource("DataSet4", vListaCotizacionUsuario);
      ReportDataSource datasource5 = new ReportDataSource("DataSet5", vListaUsuarioFirma);


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

    protected void PSubeFormularioCotiDaniosPropios(string pNroOrden)
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
      string fileName = "RepFormCotiDaniosPropios" + pNroOrden;

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
                        join s in db.cotizacion_danios_propios_sumatoria on f.idFlujo equals s.id_flujo
                        where (s.numero_orden == pNroOrden)
                        select new
                        {
                          f.nombreAsegurado,
                          f.telefonocelAsegurado,
                          cobertura = "DAÑOS PROPIOS",
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

      var vListaCotiDaniosPropios = from c in db.cotizacion_danios_propios
                                    join f in db.Flujo on c.id_flujo equals f.idFlujo
                                    where (c.numero_orden == pNroOrden)
                                    orderby c.id_item
                                    select new
                                    {
                                      f.nombreAsegurado,
                                      f.telefonocelAsegurado,
                                      cobertura = "DAÑOS PROPIOS",
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

      //ajustes 2020/02/02 wap se añade el campo cotizacion_proveedor

      var vListaCotiSumaDaniosPropios = from c in db.cotizacion_danios_propios_sumatoria
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
                                          c.monto_final,
                                          c.cotizacion_proveedor
                                        };

      var vListaCotizacionUsuario = from cc in db.cotizacion_danios_propios
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

      var vListaUsuarioFirma = from cc in db.cotizacion_danios_propios
                               join c in db.Cotizacion on cc.id_cotizacion equals c.idCotizacion
                               join uf in db.UsuarioFirma on c.idUsuario equals uf.idUsuario
                               where (cc.numero_orden == pNroOrden)
                               select new
                               {
                                 uf.idUsuario,
                                 uf.estado,
                                 uf.usuarioCreacion,
                                 uf.fechaCreacion,
                                 uf.firmaSello
                               };

      ReportViewerCoti.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;

      if ("OT" == pNroOrden.Substring(0, 2))
      {
        ReportViewerCoti.LocalReport.ReportPath = "Reportes\\RepFormularioCotiOrdenTrabajoFirma.rdlc";
      }
      else
      {
        if ("OC" == pNroOrden.Substring(0, 2))
        {
          ReportViewerCoti.LocalReport.ReportPath = "Reportes\\RepFormularioCotiOrdenCompraFirma.rdlc";
        }
        else
        {
          ReportViewerCoti.LocalReport.ReportPath = "Reportes\\RepFormularioCotiOrdenPagoCambioFirma.rdlc";
        }
      }

      ReportDataSource datasource1 = new ReportDataSource("DataSet1", vListaFlujo);
      ReportDataSource datasource2 = new ReportDataSource("DataSet2", vListaCotiDaniosPropios);
      ReportDataSource datasource3 = new ReportDataSource("DataSet3", vListaCotiSumaDaniosPropios);
      ReportDataSource datasource4 = new ReportDataSource("DataSet4", vListaCotizacionUsuario);
      ReportDataSource datasource5 = new ReportDataSource("DataSet5", vListaUsuarioFirma);

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

      BD.CotizacionICRL.TipoDaniosPropiosSumatoriaTraer vTipoDaniosPropiosSumatoriaTraer;
      vTipoDaniosPropiosSumatoriaTraer = CotizacionICRL.DaniosPropiosSumatoriaTraer(vIdFlujo, vIdCotizacion, vTipoItem);
      DataSet vDatasetOrdenes = vTipoDaniosPropiosSumatoriaTraer.dsDaniosPropiosSumatoria;
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

      BD.CotizacionICRL.TipoDaniosPropiosSumatoriaTraer vTipoDaniosPropiosSumatoriaTraer;
      vTipoDaniosPropiosSumatoriaTraer = CotizacionICRL.DaniosPropiosSumatoriaTraer(vIdFlujo, vIdCotizacion, vTipoItem);
      DataSet vDatasetOrdenes = vTipoDaniosPropiosSumatoriaTraer.dsDaniosPropiosSumatoria;
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
        BD.CotizacionICRL.TipoDaniosPropiosSumatoriaTraer vTipoDaniosPropiosSumatoriaTraer;
        vTipoDaniosPropiosSumatoriaTraer = CotizacionICRL.DaniosPropiosSumatoriaTraer(vIdFlujo, vIdCotizacion, vTipoItem);
        DataSet vDatasetOrdenes = vTipoDaniosPropiosSumatoriaTraer.dsDaniosPropiosSumatoria;
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
          vSBNumeroOrden.Append("-DP-");
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
          BD.CotizacionICRL.DaniosPropiosSumatoriaModificarTodos(vDatasetOrdenes);

          //Actualizar el numero de orden en la tabla de Daños Propios
          vAccesoDatos.fActualizaOrdenesCotiDP(vIdFlujo, vIdCotizacion, "BENEFICIARIO", vTipoItem);

          //Actualizar un registro de Beneficiario
          vTipoDaniosPropiosSumatoriaTraer = CotizacionICRL.DaniosPropiosSumatoriaTraer(vIdFlujo, vIdCotizacion, vTipoItem);
          vDatasetOrdenes = vTipoDaniosPropiosSumatoriaTraer.dsDaniosPropiosSumatoria;
          vIndiceDataTable = vDatasetOrdenes.Tables.Count - 1;

          if (vIndiceDataTable >= 0)
          {
            string vProveedor = vDatasetOrdenes.Tables[vIndiceDataTable].Rows[vIndiceBenef][3].ToString();
            string vNombreBeneficiario = TextBoxBeneficiario.Text.ToUpper().Trim();
            //if ("Beneficiario" == vProveedor)
            //{
            //  vDatasetOrdenes.Tables[vIndiceDataTable].Rows[vIndiceBenef][3] = vNombreBeneficiario;
            //}

            BD.CotizacionICRL.DaniosPropiosSumatoriaModificarTodos(vDatasetOrdenes, vNombreBeneficiario);
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

          vTipoItem = (short)(CotizacionICRL.TipoItem.Reparacion);
          FlTraeDatosSumatoriaReparaciones(vIdFlujo, vIdCotizacion, vTipoItem);
          vTipoItem = (short)(CotizacionICRL.TipoItem.Repuesto);
          FlTraeDatosSumatoriaRepuestos(vIdFlujo, vIdCotizacion, vTipoItem);

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

    #region TallerGenerico

    private int FValidaRepaTallerGen()
    {
      int vResultado = -1;
      int vIdFlujo = 0;
      int vIdCotizacion = 0;
      short vTipoItem = 0;

      vIdFlujo = int.Parse(TextBoxIdFlujo.Text); ;
      vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);
      vTipoItem = (short)CotizacionICRL.TipoItem.Reparacion;

      BD.CotizacionICRL.TipoDaniosPropiosSumatoriaTraer vTipoDaniosPropiosSumatoriaTraer;
      vTipoDaniosPropiosSumatoriaTraer = CotizacionICRL.DaniosPropiosSumatoriaTraer(vIdFlujo, vIdCotizacion, vTipoItem);
      DataSet vDatasetOrdenes = vTipoDaniosPropiosSumatoriaTraer.dsDaniosPropiosSumatoria;
      int vIndiceDataTable = vDatasetOrdenes.Tables.Count - 1;

      if (vIndiceDataTable >= 0)
      {
        //Buscar un registro de Taller Generico
        for (int i = 0; i < vDatasetOrdenes.Tables[vIndiceDataTable].Rows.Count; i++)
        {
          string vTaller = vDatasetOrdenes.Tables[vIndiceDataTable].Rows[i][3].ToString();
          vTaller = vTaller.Replace("&#209;", "Ñ");
          if ("TGENERICO" == vTaller.ToUpper())
          {
            vResultado = i;
            ButtonRepaCambioTallerGen.Visible = true;
            break;
          }
        }
      }

      return vResultado;
    }

    private int FValidaRepuTallerGen()
    {
      int vResultado = -1;
      int vIdFlujo = 0;
      int vIdCotizacion = 0;
      short vTipoItem = 0;

      vIdFlujo = int.Parse(TextBoxIdFlujo.Text); ;
      vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);
      vTipoItem = (short)CotizacionICRL.TipoItem.Repuesto;

      BD.CotizacionICRL.TipoDaniosPropiosSumatoriaTraer vTipoDaniosPropiosSumatoriaTraer;
      vTipoDaniosPropiosSumatoriaTraer = CotizacionICRL.DaniosPropiosSumatoriaTraer(vIdFlujo, vIdCotizacion, vTipoItem);
      DataSet vDatasetOrdenes = vTipoDaniosPropiosSumatoriaTraer.dsDaniosPropiosSumatoria;
      int vIndiceDataTable = vDatasetOrdenes.Tables.Count - 1;

      if (vIndiceDataTable >= 0)
      {
        //Buscar un registro de Taller Generico
        for (int i = 0; i < vDatasetOrdenes.Tables[vIndiceDataTable].Rows.Count; i++)
        {
          string vTaller = vDatasetOrdenes.Tables[vIndiceDataTable].Rows[i][3].ToString();
          vTaller = vTaller.Replace("&#209;", "Ñ");
          if ("TGENERICO" == vTaller.ToUpper())
          {
            vResultado = i;
            ButtonRepuCambioTallerGen.Visible = true;
            break;
          }
        }
      }

      return vResultado;
    }

    protected void ButtonTallerGenCambiar_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      int vIdFlujo = 0;
      int vIdCotizacion = 0;
      short vTipoItem = 0;
      int vIndiceTallerGen = -1;
      AccesoDatos vAccesoDatos = new AccesoDatos();

      vIdFlujo = int.Parse(TextBoxIdFlujo.Text); ;
      vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);
      vTipoItem = short.Parse(TextBoxTallerGenTipoItem.Text);
      vIndiceTallerGen = int.Parse(TextBoxTallerGenIndice.Text);

      if (string.Empty != TextBoxTallerGen.Text)
      {
        //Cambiar el nombre del Taller Genérico en la tabla de Sumatorias
        BD.CotizacionICRL.TipoDaniosPropiosSumatoriaTraer vTipoDaniosPropiosSumatoriaTraer;
        vTipoDaniosPropiosSumatoriaTraer = CotizacionICRL.DaniosPropiosSumatoriaTraer(vIdFlujo, vIdCotizacion, vTipoItem);
        DataSet vDatasetOrdenes = vTipoDaniosPropiosSumatoriaTraer.dsDaniosPropiosSumatoria;
        int vIndiceDataTable = vDatasetOrdenes.Tables.Count - 1;

        if (vIndiceDataTable >= 0)
        {
          //Actualizar el registro de TallerGen

          string vProveedor = vDatasetOrdenes.Tables[vIndiceDataTable].Rows[vIndiceTallerGen][3].ToString();
          string vNombreTallerGen = TextBoxTallerGen.Text.ToUpper().Trim();
          vNombreTallerGen = vNombreTallerGen.Replace("&#209;", "Ñ");

          BD.CotizacionICRL.DaniosPropiosSumatoriaModificarTodos(vDatasetOrdenes, vNombreTallerGen);

          if (1 == vTipoItem)
          {
            ButtonRepaCambioTallerGen.Visible = false;
          }
          else
          {
            ButtonRepuCambioTallerGen.Visible = false;
          }
          FLlenarGrillaOrdenes(vIdFlujo, vIdCotizacion, vTipoItem);

          vTipoItem = (short)(CotizacionICRL.TipoItem.Reparacion);
          FlTraeDatosSumatoriaReparaciones(vIdFlujo, vIdCotizacion, vTipoItem);
          vTipoItem = (short)(CotizacionICRL.TipoItem.Repuesto);
          FlTraeDatosSumatoriaRepuestos(vIdFlujo, vIdCotizacion, vTipoItem);

          Session["PopupTallerGen"] = 0;
          this.ModalPopupTallerGen.Hide();
        }
      }
      else
      {
        //Si el nombre del Taller esta vacio no se hace el cambio
        LabelMsjTallerGen.Text = "El nombre del Taller no puede estar vacío";
      }
    }

    protected void ButtonTallerGenCancelar_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      ButtonTallerGenCambiar.Enabled = false;
      ButtonTallerGenCancelar.Enabled = false;
    }

    protected void ButtonCancelPopTallerGen_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      Session["PopupTallerGen"] = 0;
      this.ModalPopupTallerGen.Hide();
    }

    protected void ButtonRepaCambioTallerGen_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      int vIndiceTallerGen = -1;
      int vTipoItem = (int)CotizacionICRL.TipoItem.Reparacion;

      LabelMsjTallerGen.Text = string.Empty;

      vIndiceTallerGen = FValidaRepaTallerGen();
      if (vIndiceTallerGen >= 0)
      {
        TextBoxTallerGenIndice.Text = vIndiceTallerGen.ToString();
        TextBoxTallerGenTipoItem.Text = vTipoItem.ToString();
        ButtonTallerGenCambiar.Enabled = true;
        ButtonTallerGenCancelar.Enabled = true;
        Session["PopupTallerGen"] = 1;
        this.ModalPopupTallerGen.Show();
      }
    }

    protected void ButtonRepuCambioTallerGen_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      int vIndiceTallerGen = -1;
      int vTipoItem = (int)CotizacionICRL.TipoItem.Repuesto;

      LabelMsjTallerGen.Text = string.Empty;

      vIndiceTallerGen = FValidaRepuTallerGen();
      if (vIndiceTallerGen >= 0)
      {
        TextBoxTallerGenIndice.Text = vIndiceTallerGen.ToString();
        TextBoxTallerGenTipoItem.Text = vTipoItem.ToString();
        ButtonTallerGenCambiar.Enabled = true;
        ButtonTallerGenCancelar.Enabled = true;
        Session["PopupTallerGen"] = 1;
        this.ModalPopupTallerGen.Show();
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
        vAccesoDatos.fActualizaPrecioItemDaniosPropios(vIdFlujo, vIdCotizacion, vTipoCambio);
      }
      //Actualizar datos de las grillas Reparacion y Repuestos
      FlTraeDatosDPReparacion(vIdCotizacion);
      FlTraeDatosDPRepuesto(vIdCotizacion);
      FlTraeDatosRecepRepu(vIdCotizacion);
    }


    protected void DropDownListRepaItem_SelectedIndexChanged(object sender, EventArgs e)
    {
      string vItemSelecconado = string.Empty;
      vItemSelecconado = DropDownListRepaItem.SelectedItem.Text.Trim();
      if ("OTRO (S)" == vItemSelecconado.ToUpper())
      {
        TextBoxRepaItem.Visible = true;
      }
    }

    protected void DropDownListRepuItem_SelectedIndexChanged(object sender, EventArgs e)
    {
      string vItemSelecconado = string.Empty;
      vItemSelecconado = DropDownListRepuItem.SelectedItem.Text.Trim();
      if ("OTRO (S)" == vItemSelecconado.ToUpper())
      {
        TextBoxRepuItem.Visible = true;
      }
    }
  }
}