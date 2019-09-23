using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LbcOnBaseWS;
using ICRL.ModeloDB;
using ICRL.BD;
using System.Text;
using System.Data;
using Microsoft.Reporting.WebForms;

namespace ICRL.Presentacion
{
  public partial class CotizacionRCObj : System.Web.UI.Page
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
          ButtonObjAgregar.Enabled = false;
        }

        int vIdFlujo = 0;
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
          vIdFlujo = int.Parse(TextBoxIdFlujo.Text);

          FlTraeDatosCotizacion(vIdCotizacion, vlNumFlujo);


          //Cargar Datos Objeto
          vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);
          PModificarObjeto(false);
          PCargaDatosObj();
          pCargaGrillaObjetos(vIdFlujo, vIdCotizacion);
          pCargaOrdenes(vIdFlujo, vIdCotizacion);
          //PModificarVehTer(false);

          short vTipoItem = (short)CotizacionICRL.TipoItem.Reparacion;

          //FlTraeDatosDPReparacion(vIdCotizacion);
          //FlTraeDatosDPRepuesto(vIdCotizacion);
          //FlTraeDatosSumatoriaReparaciones(vIdFlujo, vIdCotizacion, vTipoItem);

          vTipoItem = (short)CotizacionICRL.TipoItem.Repuesto;
          //FlTraeDatosSumatoriaRepuestos(vIdFlujo, vIdCotizacion, vTipoItem);
          //FlTraeDatosRecepRepu(vIdCotizacion);
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
                       //cdp.tipoTaller,
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
          }

        }
      }
    }

    long FValidaTieneObjeto(int pIdFlujo, int pIdCotizacion)
    {
      long vResultado = 0;

      BD.CotizacionICRL.TipoRCObjetosTraer vTipoRCObjetosTraer;
      vTipoRCObjetosTraer = CotizacionICRL.RCObjetosTraer(pIdFlujo, pIdCotizacion);

      if (vTipoRCObjetosTraer.Correcto)
      {
        var vFilaTabla = vTipoRCObjetosTraer.RCObjetos.FirstOrDefault();
        if (vFilaTabla != null)
        {
          vResultado = vFilaTabla.id_item;
        }
      }

      return vResultado;
    }

    protected void PCargaDatosObj()
    {
      long vIdItem = 0;
      int vIdFlujo = 0;
      int vIdCotizacion = 0;
      string vTextoTemporal = string.Empty;

      vIdFlujo = int.Parse(TextBoxIdFlujo.Text); ;
      vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);

      vIdItem = FValidaTieneObjeto(vIdFlujo, vIdCotizacion);
      if (vIdItem > 0)
      {
        BD.CotizacionICRL.TipoRCObjetosTraer vTipoRCObjetosTraer;
        vTipoRCObjetosTraer = CotizacionICRL.RCObjetosTraer(vIdFlujo, vIdCotizacion);
        var vFilaTabla = vTipoRCObjetosTraer.RCObjetos.FirstOrDefault();

        TextBoxObjNombreTer.Text = vFilaTabla.nombre_apellido;
        TextBoxObjTelefono.Text = vFilaTabla.telefono_contacto;
        TextBoxObjDocId.Text = vFilaTabla.numero_documento;
        CheckBoxObjReembolso.Checked = vFilaTabla.rembolso;

        TextBoxObjIdItem.Text = vFilaTabla.id_item.ToString();
      }

    }

    //private int FlTraeDatosDPReparacion(int pIdCotizacion)
    //{
    //    int vResultado = 0;
    //    int vIdFlujo = int.Parse(TextBoxIdFlujo.Text);

    //    BD.CotizacionICRL.TipoRCVehicularTraer vTipoRCVehicularTraer;
    //    vTipoRCVehicularTraer = CotizacionICRL.RCVehicularesTraer(vIdFlujo, pIdCotizacion);

    //    GridViewReparaciones.DataSource = vTipoRCVehicularTraer.RCVehiculares.Select(RCVehiculares => new
    //    {
    //        RCVehiculares.id_item,
    //        RCVehiculares.item_descripcion,
    //        RCVehiculares.chaObjio,
    //        RCVehiculares.reparacion_previa,
    //        RCVehiculares.mecanico,
    //        RCVehiculares.id_moneda,
    //        RCVehiculares.precio_cotizado,
    //        RCVehiculares.id_tipo_descuento,
    //        RCVehiculares.descuento,
    //        RCVehiculares.precio_final,
    //        RCVehiculares.proveedor,
    //        RCVehiculares.id_tipo_item

    //    }).Where(RCVehiculares => RCVehiculares.id_tipo_item == 1).ToList();
    //    GridViewReparaciones.DataBind();

    //    return vResultado;
    //}

    //private int FlTraeDatosDPRepuesto(int pIdCotizacion)
    //{
    //    int vResultado = 0;
    //    int vIdFlujo = int.Parse(TextBoxIdFlujo.Text);

    //    BD.CotizacionICRL.TipoRCVehicularTraer vTipoRCVehicularTraer;
    //    vTipoRCVehicularTraer = CotizacionICRL.RCVehicularesTraer(vIdFlujo, pIdCotizacion);

    //    GridViewRepuestos.DataSource = vTipoRCVehicularTraer.RCVehiculares.Select(RCVehiculares => new
    //    {
    //        RCVehiculares.id_item,
    //        RCVehiculares.item_descripcion,
    //        RCVehiculares.pintura,
    //        RCVehiculares.instalacion,
    //        RCVehiculares.id_moneda,
    //        RCVehiculares.precio_cotizado,
    //        RCVehiculares.id_tipo_descuento,
    //        RCVehiculares.descuento,
    //        RCVehiculares.precio_final,
    //        RCVehiculares.proveedor,
    //        RCVehiculares.id_tipo_item

    //    }).Where(RCVehiculares => RCVehiculares.id_tipo_item == 2).ToList();
    //    GridViewRepuestos.DataBind();

    //    return vResultado;
    //}

    #endregion

    #region Datos Objeto
    protected void PModificarObjeto(bool pEstado)
    {
      TextBoxObjNombreTer.Enabled = pEstado;
      TextBoxObjDocId.Enabled = pEstado;
      TextBoxObjTelefono.Enabled = pEstado;
      CheckBoxObjReembolso.Enabled = pEstado;
      TextBoxObjItem.Enabled = pEstado;
      TextBoxObjMontoItemRef.Enabled = pEstado;
      TextBoxObjDescripcion.Enabled = pEstado;
    }

    protected void PModificarObjetoDet(bool pEstado)
    {
      TextBoxObjTelefono.Enabled = pEstado;
      CheckBoxObjReembolso.Enabled = pEstado;
      TextBoxObjItem.Enabled = pEstado;
      TextBoxObjMontoItemRef.Enabled = pEstado;
      TextBoxObjDescripcion.Enabled = pEstado;
    }

    protected void PLimpiarObjetoDet()
    {
      TextBoxObjItem.Text = string.Empty;
      TextBoxObjMontoItemRef.Text = "0";
      TextBoxObjDescripcion.Text = string.Empty;
    }

    protected void ButtonObjAgregar_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      LabelDatosObjetoMsj.Text = string.Empty;
      //validamos si existe el dato de Objeto
      int vIdFlujo = 0;
      int vIdCotizacion = 0;
      long vIdItem = 0;
      vIdFlujo = int.Parse(TextBoxIdFlujo.Text); ;
      vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);

      vIdItem = FValidaTieneObjeto(vIdFlujo, vIdCotizacion);
      if (vIdItem > 0)
      {
        //Si existe
        PModificarObjetoDet(true);
        ButtonObjAgregar.Visible = false;
        ButtonObjGrabar.Visible = true;
        ButtonObjCancelar.Visible = true;
      }
      else
      {
        //No existe
        PModificarObjeto(true);
        ButtonObjAgregar.Visible = false;
        ButtonObjGrabar.Visible = true;
        ButtonObjCancelar.Visible = true;
      }
    }

    protected void ButtonObjGrabar_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      int vIdFlujo = 0;
      int vIdCotizacion = 0;
      long vIdItem = 0;
      bool vResultado = false;

      vIdFlujo = int.Parse(TextBoxIdFlujo.Text); ;
      vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);

      //cargar los datos del panel al objeto correspondiente
      CotizacionICRL.TipoRCObjetos vTipoRCObjetos = new CotizacionICRL.TipoRCObjetos();
      vTipoRCObjetos.id_flujo = vIdFlujo;
      vTipoRCObjetos.id_cotizacion = vIdCotizacion;
      vTipoRCObjetos.nombre_apellido = TextBoxObjNombreTer.Text.ToUpper();
      vTipoRCObjetos.telefono_contacto = TextBoxObjTelefono.Text.ToUpper();
      vTipoRCObjetos.numero_documento = TextBoxObjDocId.Text.ToUpper();
      vTipoRCObjetos.rembolso = CheckBoxObjReembolso.Checked;
      vTipoRCObjetos.tipo_item = TextBoxObjItem.Text.ToUpper();
      vTipoRCObjetos.monto_item = double.Parse(TextBoxObjMontoItemRef.Text);
      vTipoRCObjetos.id_moneda = 1; //1 corresponde a Bs.
      vTipoRCObjetos.descripcion = TextBoxObjDescripcion.Text.ToUpper();
      vTipoRCObjetos.tipo_cambio = double.Parse(TextBoxTipoCambio.Text);
      vTipoRCObjetos.id_estado = 1;

      //validar si existe el registro del Objeto
      if (string.Empty != TextBoxObjIdItem.Text)
      {
        //Existe el registro del tercero
        vIdItem = long.Parse(TextBoxObjIdItem.Text);
        vTipoRCObjetos.id_item = vIdItem;
        vResultado = CotizacionICRL.RCObjetosModificar(vTipoRCObjetos);
      }
      else
      {
        //NO Existe el registro del tercero
        vResultado = CotizacionICRL.RCObjetosRegistrar(vTipoRCObjetos);
      }

      if (vResultado)
      {
        LabelDatosObjetoMsj.Text = "Registro Actualizado Exitosamente";
        vIdItem = FValidaTieneObjeto(vIdFlujo, vIdCotizacion);

      }
      else
      {
        LabelDatosObjetoMsj.Text = "El Registro no se actualizo correctamente";
      }

      TextBoxObjIdItem.Text = string.Empty;
      PModificarObjeto(false);
      ButtonObjAgregar.Visible = true;
      ButtonObjGrabar.Visible = false;
      ButtonObjCancelar.Visible = false;
      PLimpiarObjetoDet();

      pCargaGrillaObjetos(vIdFlujo, vIdCotizacion);
    }

    protected void ButtonObjCancelar_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      TextBoxObjIdItem.Text = string.Empty;
      PModificarObjeto(false);
      ButtonObjAgregar.Visible = true;
      ButtonObjGrabar.Visible = false;
      ButtonObjCancelar.Visible = false;
    }
    #endregion

    #region Grilla ObjetosDet

    protected void pCargaGrillaObjetos(int pIdFlujo, int pIdCotizacion)
    {
      BD.CotizacionICRL.TipoRCObjetosTraer vTipoRCObjetosTraer;
      vTipoRCObjetosTraer = CotizacionICRL.RCObjetosTraer(pIdFlujo, pIdCotizacion);

      GridViewObjDetalle.DataSource = vTipoRCObjetosTraer.RCObjetos.Select(RCObjetos => new
      {
        RCObjetos.id_item,
        RCObjetos.nombre_apellido,
        RCObjetos.numero_documento,
        RCObjetos.tipo_item,
        RCObjetos.monto_item,
        RCObjetos.descripcion,
      }).ToList();
      GridViewObjDetalle.DataBind();
    }

    protected void GridViewObjDetalle_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      string vTextoTemporal = string.Empty;

      //Leer Registro de la grilla y cargar los valores a la ventana.

      TextBoxObjItem.Text = GridViewObjDetalle.SelectedRow.Cells[4].Text;
      TextBoxObjMontoItemRef.Text = GridViewObjDetalle.SelectedRow.Cells[5].Text;
      TextBoxObjDescripcion.Text = GridViewObjDetalle.SelectedRow.Cells[6].Text;

      TextBoxObjIdItem.Text = GridViewObjDetalle.SelectedRow.Cells[1].Text;

      PModificarObjetoDet(true);
      ButtonObjAgregar.Visible = false;
      ButtonObjGrabar.Visible = true;
      ButtonObjCancelar.Visible = true;

    }

    protected void GridViewObjDetalle_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
      if (!VerificarPagina(true)) return;
      bool vResultado = false;
      int vIdFlujo = 0;
      int vIdCotizacion = 0;
      long vIdItem = 0;

      vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
      vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);

      string vTextoSecuencial = string.Empty;
      int vIndex = 0;
      int vSecuencial = 0;

      vIndex = Convert.ToInt32(e.RowIndex);
      vSecuencial = Convert.ToInt32(GridViewObjDetalle.DataKeys[vIndex].Value);
      vIdItem = Convert.ToInt64(vSecuencial);



      //vIdItem = long.Parse(GridViewObjDetalle.SelectedRow.Cells[1].Text);

      //CotizacionICRL.RCObjetoModificar

      vResultado = BD.CotizacionICRL.RCObjetosBorrar(vIdFlujo, vIdCotizacion, vIdItem);
      if (vResultado)
      {
        LabelDatosObjetoMsj.Text = "Registro Borrado exitosamente";
        PModificarObjeto(false);
        ButtonObjAgregar.Visible = true;
        ButtonObjGrabar.Visible = false;
        ButtonObjCancelar.Visible = false;
      }
      else
      {
        LabelDatosObjetoMsj.Text = "El Registro no pudo ser Borrado";
      }
      TextBoxObjIdItem.Text = string.Empty;
      pCargaGrillaObjetos(vIdFlujo, vIdCotizacion);
    }
    #endregion

    #region GenerarOrden

    protected void ButtonGenerarOrden_Click(object sender, EventArgs e)
    {
      if (!VerificarPagina(true)) return;
      bool vResultado = false;
      int vIdFlujo = 0;
      int vIdCotizacion = 0;
      int vContador = 1;
      StringBuilder vSBNumeroOrden = new StringBuilder();
      string vNumeroOrden = string.Empty;

      vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
      vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);

      //generar numero de orden
      vNumeroOrden = string.Empty;
      vSBNumeroOrden.Clear();
      vSBNumeroOrden.Append("OP-");
      vNumeroOrden = TextBoxNroFlujo.Text.Trim();
      vNumeroOrden = vNumeroOrden.PadLeft(6, '0');
      vSBNumeroOrden.Append(vNumeroOrden);
      vSBNumeroOrden.Append("-OB-");
      vNumeroOrden = vIdCotizacion.ToString();
      vNumeroOrden = vNumeroOrden.PadLeft(6, '0');
      vSBNumeroOrden.Append(vNumeroOrden);
      vSBNumeroOrden.Append("-");
      vNumeroOrden = vContador.ToString();
      vSBNumeroOrden.Append(vNumeroOrden.PadLeft(2, '0'));
      vNumeroOrden = vSBNumeroOrden.ToString();

      CotizacionICRL.TipoOrden vTipoOrden = new CotizacionICRL.TipoOrden();
      vTipoOrden.id_flujo = vIdFlujo;
      vTipoOrden.id_cotizacion = vIdCotizacion;
      vTipoOrden.tipo_origen = (short)AccesoDatos.TipoInspeccion.RCObjetos;
      vTipoOrden.numero_orden = vNumeroOrden;
      vTipoOrden.fecha_orden = DateTime.Today;
      vTipoOrden.descripcion = TextBoxObjNombreTer.Text.ToUpper();
      vTipoOrden.monto_us = 0;
      vTipoOrden.id_estado = 1;

      //Sumar los gastos del detalle
      BD.CotizacionICRL.TipoRCObjetosTraer vTipoRCObjetosTraer;
      vTipoRCObjetosTraer = CotizacionICRL.RCObjetosTraer(vIdFlujo, vIdCotizacion);

      double vSumaGastos = 0;
      DataSet vDataSetPer = vTipoRCObjetosTraer.dsRCObjetos;

      int vIndiceDataTable = vDataSetPer.Tables.Count - 1;

      if (vIndiceDataTable >= 0)
      {
        for (int i = 0; i < vDataSetPer.Tables[vIndiceDataTable].Rows.Count; i++)
        {
          double vMontoGasto = 0;
          string vTextoMontoGasto = string.Empty;
          vTextoMontoGasto = vDataSetPer.Tables[vIndiceDataTable].Rows[i][5].ToString();
          try
          {
            vMontoGasto = (double)vDataSetPer.Tables[vIndiceDataTable].Rows[i][5];
          }
          catch (Exception)
          {
            vMontoGasto = 0;
          }

          vSumaGastos = vSumaGastos + vMontoGasto;
        }
      }
      vTipoOrden.monto_bs = vSumaGastos;

      //ajuste ordenes
      CotizacionICRL.TipoOrdenTraer vTipoOrdenTraer;
      CotizacionICRL.TipoOrden vTipoOrdenAux = new CotizacionICRL.TipoOrden();
      vTipoOrdenTraer = CotizacionICRL.OrdenTraer(vIdFlujo,vIdCotizacion);
      
      if (vTipoOrdenTraer.Ordenes.Count > 0)
      {
        //si existe registro se borra
        vTipoOrdenAux = vTipoOrdenTraer.Ordenes.FirstOrDefault();
        vResultado = CotizacionICRL.OrdenBorrar(vTipoOrdenAux.id_flujo, vTipoOrdenAux.id_cotizacion, vTipoOrdenAux.id_item);
      }


      vResultado = CotizacionICRL.OrdenRegistrar(vTipoOrden);
      if (vResultado)
      {
        LabelDatosObjetoMsj.Text = "Orden creada exitosamente";
      }
      else
      {
        LabelDatosObjetoMsj.Text = "la Orden no se pudo crear";
      }
      pCargaOrdenes(vIdFlujo, vIdCotizacion);
    }

    protected void pCargaOrdenes(int pIdFlujo, int pIdCotizacion)
    {
      BD.CotizacionICRL.TipoOrdenTraer vTipoOrdenTraer;
      vTipoOrdenTraer = CotizacionICRL.OrdenTraer(pIdFlujo, pIdCotizacion);

      GridViewOrdenes.DataSource = vTipoOrdenTraer.Ordenes.Select(Ordenes => new
      {
        Ordenes.numero_orden,
        Ordenes.id_estado,
        Ordenes.descripcion,
        Ordenes.monto_bs,
        Ordenes.id_item
      }).ToList();
      GridViewOrdenes.DataBind();
    }

    protected void GridViewOrdenes_RowCommand(object sender, GridViewCommandEventArgs e)
    {
      if (!VerificarPagina(true)) return;
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
        PImprimeFormularioCotiRCObjetos(vNumeroOrden);
      }

      if (0 == e.CommandName.CompareTo("Ver"))
      {
        string vTextoSecuencial = string.Empty;
        vIndex = 0;

        vIndex = Convert.ToInt32(e.CommandArgument);
        vNumeroOrden = (string)GridViewOrdenes.DataKeys[vIndex].Value;
        vProveedor = GridViewOrdenes.Rows[vIndex].Cells[2].Text;
        PVerFormularioCotiRCObjetos(vNumeroOrden);
      }

      if (0 == e.CommandName.CompareTo("SubirOnBase"))
      {
        int vResultado = 0;
        string vTextoSecuencial = string.Empty;


        vIndex = Convert.ToInt32(e.CommandArgument);
        vNumeroOrden = (string)GridViewOrdenes.DataKeys[vIndex].Value;
        vProveedor = GridViewOrdenes.Rows[vIndex].Cells[2].Text;

        string vIdItemAux = string.Empty;
        vIdItemAux = GridViewOrdenes.Rows[vIndex].Cells[7].Text;
        long vIdItem = 0;
        vIdItem = long.Parse(vIdItemAux);

        //Grabar en la tabla
        int vIdFlujo = 0;
        int vIdCotizacion = 0;
        

        vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
        vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);
        

        vResultado = vAccesoDatos.fActualizaLiquidacionOB(vIdFlujo, vIdCotizacion, vNumeroOrden);
        PSubeFormularioCotiRCObjetos(vNumeroOrden);
        vResultado = vAccesoDatos.FCotizacionRCObjetosCambiaEstadoOrdenes(vIdFlujo, vIdCotizacion, vIdItem);
        pCargaOrdenes(vIdFlujo, vIdCotizacion);
      }
    }

    protected void PImprimeFormularioCotiRCObjetos(string pNroOrden)
    {
      AccesoDatos vAccesoDatos = new AccesoDatos();
      LBCDesaEntities db = new LBCDesaEntities();


      Warning[] warnings;
      string[] streamIds;
      string mimeType = string.Empty;
      string encoding = string.Empty;
      string extension = "pdf";
      string fileName = "RepFormCotiRCObjetos" + pNroOrden;

      var vListaFlujo = from f in db.Flujo
                        join s in db.cotizacion_ordenes on f.idFlujo equals s.id_flujo
                        where (s.numero_orden == pNroOrden)
                        select new
                        {
                          f.nombreAsegurado,
                          f.telefonocelAsegurado,
                          cobertura = "RC OBJETOS",
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

      var vListaCotiRCObjetos = from c in db.cotizacion_rc_objetos
                                 join d in db.cotizacion_ordenes
                                 on new { c.id_flujo, c.id_cotizacion } equals new { d.id_flujo, d.id_cotizacion }
                                 where (d.numero_orden == pNroOrden)
                                 select new
                                 {
                                   d.numero_orden,
                                   c.nombre_apellido,
                                   c.numero_documento,
                                   c.telefono_contacto,
                                   c.tipo_item,
                                   c.descripcion,
                                   c.monto_item,
                                   c.rembolso
                                 };



      ReportViewerCoti.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
      ReportViewerCoti.LocalReport.ReportPath = "Reportes\\RepFormularioCotiRCObjetos.rdlc";
      ReportDataSource datasource1 = new ReportDataSource("DataSet1", vListaFlujo);
      ReportDataSource datasource2 = new ReportDataSource("DataSet2", vListaCotiRCObjetos);


      ReportViewerCoti.LocalReport.DataSources.Clear();
      ReportViewerCoti.LocalReport.DataSources.Add(datasource1);
      ReportViewerCoti.LocalReport.DataSources.Add(datasource2);

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

    protected void PVerFormularioCotiRCObjetos(string pNroOrden)
    {
      AccesoDatos vAccesoDatos = new AccesoDatos();
      LBCDesaEntities db = new LBCDesaEntities();

      var vListaFlujo = from f in db.Flujo
                        join s in db.cotizacion_ordenes on f.idFlujo equals s.id_flujo
                        where (s.numero_orden == pNroOrden)
                        select new
                        {
                          f.nombreAsegurado,
                          f.telefonocelAsegurado,
                          cobertura = "RC OBJETOS",
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

      var vListaCotiRCObjetos = from c in db.cotizacion_rc_objetos
                                join d in db.cotizacion_ordenes
                                on new { c.id_flujo, c.id_cotizacion } equals new { d.id_flujo, d.id_cotizacion }
                                where (d.numero_orden == pNroOrden)
                                select new
                                {
                                  d.numero_orden,
                                  c.nombre_apellido,
                                  c.numero_documento,
                                  c.telefono_contacto,
                                  c.tipo_item,
                                  c.descripcion,
                                  c.monto_item,
                                  c.rembolso
                                };



      ReportViewerCoti.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
      ReportViewerCoti.LocalReport.ReportPath = "Reportes\\RepFormularioCotiRCObjetos.rdlc";
      ReportDataSource datasource1 = new ReportDataSource("DataSet1", vListaFlujo);
      ReportDataSource datasource2 = new ReportDataSource("DataSet2", vListaCotiRCObjetos);


      ReportViewerCoti.LocalReport.DataSources.Clear();
      ReportViewerCoti.LocalReport.DataSources.Add(datasource1);
      ReportViewerCoti.LocalReport.DataSources.Add(datasource2);

      ReportViewerCoti.LocalReport.Refresh();
      ReportViewerCoti.ShowToolBar = false;
      ReportViewerCoti.Visible = true;

    }

    protected void PSubeFormularioCotiRCObjetos(string pNroOrden)
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
      string fileName = "RepFormCotiRCObjetos" + pNroOrden;

      vTipoDocumental = "RE - Orden de Indemnizacion";

      vNombreUsuario = Session["IdUsr"].ToString();

      var vListaFlujo = from f in db.Flujo
                        join s in db.cotizacion_ordenes on f.idFlujo equals s.id_flujo
                        where (s.numero_orden == pNroOrden)
                        select new
                        {
                          f.nombreAsegurado,
                          f.telefonocelAsegurado,
                          cobertura = "RC OBJETOS",
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

      var vListaCotiRCObjetos = from c in db.cotizacion_rc_objetos
                                join d in db.cotizacion_ordenes
                                on new { c.id_flujo, c.id_cotizacion } equals new { d.id_flujo, d.id_cotizacion }
                                where (d.numero_orden == pNroOrden)
                                select new
                                {
                                  d.numero_orden,
                                  c.nombre_apellido,
                                  c.numero_documento,
                                  c.telefono_contacto,
                                  c.tipo_item,
                                  c.descripcion,
                                  c.monto_item,
                                  c.rembolso
                                };



      ReportViewerCoti.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
      ReportViewerCoti.LocalReport.ReportPath = "Reportes\\RepFormularioCotiRCObjetos.rdlc";
      ReportDataSource datasource1 = new ReportDataSource("DataSet1", vListaFlujo);
      ReportDataSource datasource2 = new ReportDataSource("DataSet2", vListaCotiRCObjetos);


      ReportViewerCoti.LocalReport.DataSources.Clear();
      ReportViewerCoti.LocalReport.DataSources.Add(datasource1);
      ReportViewerCoti.LocalReport.DataSources.Add(datasource2);

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

    #endregion

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
          (e.Row.Cells[6].Controls[0] as Button).Enabled = true;
        }
        else
        {
          (e.Row.Cells[6].Controls[0] as Button).Enabled = false;
        }
      }
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
  }
}