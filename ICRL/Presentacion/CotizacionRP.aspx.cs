﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LbcOnBaseWS;
using ICRL.ModeloDB;
using ICRL.BD;

namespace ICRL.Presentacion
{
    public partial class CotizacionRP : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
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

                    vTipoItem = (short)CotizacionICRL.TipoItem.Repuesto;
                    FlTraeDatosSumatoriaRepuestos(vIdFlujo, vIdCotizacion, vTipoItem);
                    FlTraeDatosRecepRepu(vIdCotizacion);
                }

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
                                   tipoTaller = "Taller Tipo B",
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

            BD.CotizacionICRL.TipoRoboParcialTraer vTipoRoboParcialTraer;
            vTipoRoboParcialTraer = CotizacionICRL.RoboParcialTraer(vIdFlujo, pIdCotizacion);

            GridViewReparaciones.DataSource = vTipoRoboParcialTraer.RobosParciales.Select(RobosParciales => new
            {
                RobosParciales.id_item,
                RobosParciales.item_descripcion,
                RobosParciales.chaperio,
                RobosParciales.reparacion_previa,
                RobosParciales.mecanico,
                RobosParciales.id_moneda,
                RobosParciales.precio_cotizado,
                RobosParciales.id_tipo_descuento,
                RobosParciales.descuento,
                RobosParciales.precio_final,
                RobosParciales.proveedor,
                RobosParciales.id_tipo_item

            }).Where(RobosParciales => RobosParciales.id_tipo_item == 1).ToList();
            GridViewReparaciones.DataBind();

            return vResultado;
        }

        private int FlTraeDatosDPRepuesto(int pIdCotizacion)
        {
            int vResultado = 0;
            int vIdFlujo = int.Parse(TextBoxIdFlujo.Text);

            BD.CotizacionICRL.TipoRoboParcialTraer vTipoRoboParcialTraer;
            vTipoRoboParcialTraer = CotizacionICRL.RoboParcialTraer(vIdFlujo, pIdCotizacion);

            GridViewRepuestos.DataSource = vTipoRoboParcialTraer.RobosParciales.Select(RobosParciales => new
            {
                RobosParciales.id_item,
                RobosParciales.item_descripcion,
                RobosParciales.pintura,
                RobosParciales.instalacion,
                RobosParciales.id_moneda,
                RobosParciales.precio_cotizado,
                RobosParciales.id_tipo_descuento,
                RobosParciales.descuento,
                RobosParciales.precio_final,
                RobosParciales.proveedor,
                RobosParciales.id_tipo_item

            }).Where(RobosParciales => RobosParciales.id_tipo_item == 2).ToList();
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
            TextBoxRepaPrecioCotizado.Text = string.Empty;
            DropDownListRepaTipoDesc.SelectedIndex = 0;
            TextBoxRepaMontoDesc.Text = string.Empty;
            TextBoxRepuPrecioFinal.Text = string.Empty;
            DropDownListRepuProveedor.SelectedIndex = 0;
        }

        protected void ButtonRepaAgregarItem_Click(object sender, EventArgs e)
        {
            TextBoxRepaIdItem.Text = string.Empty;
            DropDownListRepaItem.Enabled = true;
            //PanelABMReparaciones.Enabled = true;
            ButtonRepaGrabar.Enabled = true;
            ButtonRepaCancelar.Enabled = true;
            Session["PopupABMReparacionesHabilitado"] = 1;
            this.ModalPopupReparaciones.Show();
        }

        protected void PRepaModificarItem()
        {
            //PanelABMReparaciones.Enabled = true;
            DropDownListRepaItem.Enabled = false;
            ButtonRepaGrabar.Enabled = true;
            ButtonRepaCancelar.Enabled = true;
        }

        protected void ButtonRepaGrabar_Click(object sender, EventArgs e)
        {
            LabelRepaRegistroItems.Text = "Items";
            BD.CotizacionICRL.TipoRoboParcial vTipoRoboParcial = new CotizacionICRL.TipoRoboParcial();

            //Completar los elementos del objeto y grabar el registro.
            vTipoRoboParcial.id_flujo = int.Parse(TextBoxIdFlujo.Text);
            vTipoRoboParcial.id_cotizacion = int.Parse(TextBoxNroCotizacion.Text);

            //tipo_item:  1 = Reparacion  2 = Repuesto
            vTipoRoboParcial.id_tipo_item = (int)CotizacionICRL.TipoItem.Reparacion;
            vTipoRoboParcial.item_descripcion = DropDownListRepaItem.SelectedItem.Text.Trim();
            vTipoRoboParcial.chaperio = DropDownListRepaChaperio.SelectedItem.Text.Trim();
            vTipoRoboParcial.reparacion_previa = DropDownListRepaRepPrevia.SelectedItem.Text.Trim();
            vTipoRoboParcial.mecanico = CheckBoxRepaMecanico.Checked;
            vTipoRoboParcial.id_moneda = DropDownListRepaMoneda.SelectedItem.Text.Trim();
            vTipoRoboParcial.precio_cotizado = double.Parse(TextBoxRepaPrecioCotizado.Text);
            vTipoRoboParcial.id_tipo_descuento = DropDownListRepaTipoDesc.SelectedItem.Text.Trim();
            vTipoRoboParcial.descuento = double.Parse(TextBoxRepaMontoDesc.Text);
            switch (vTipoRoboParcial.id_tipo_descuento)
            {
                case "Fijo":
                    vTipoRoboParcial.precio_final = vTipoRoboParcial.precio_cotizado - vTipoRoboParcial.descuento;
                    break;
                case "Porcentaje":
                    vTipoRoboParcial.precio_final = vTipoRoboParcial.precio_cotizado - (vTipoRoboParcial.precio_cotizado * (vTipoRoboParcial.descuento / 100));
                    break;
                default:
                    vTipoRoboParcial.precio_final = vTipoRoboParcial.precio_cotizado;
                    break;
            }
            vTipoRoboParcial.proveedor = DropDownListRepaProveedor.SelectedItem.Text.Trim();
            vTipoRoboParcial.id_estado = 1;

            double vTipoCambio = 0;
            vTipoCambio = double.Parse(TextBoxTipoCambio.Text);
            vTipoRoboParcial.tipo_cambio = vTipoCambio;

            bool vResultado = false;
            if (string.Empty != TextBoxRepaIdItem.Text)
            {
                vTipoRoboParcial.id_item = int.Parse(TextBoxRepaIdItem.Text);
                vResultado = BD.CotizacionICRL.RoboParcialModificar(vTipoRoboParcial);
                if (vResultado)
                {
                    LabelRepaRegistroItems.Text = "Registro modificado exitosamente";
                    PLimpiarCamposRepa();
                    //PanelABMReparaciones.Enabled = false;
                    ButtonRepaGrabar.Enabled = false;
                    ButtonRepaCancelar.Enabled = false;
                }
                else
                {
                    LabelRepaRegistroItems.Text = "El Registro no pudo ser añadido";
                }
            }
            else
            {
                vResultado = BD.CotizacionICRL.RoboParcialRegistrar(vTipoRoboParcial);
                if (vResultado)
                {
                    LabelRepaRegistroItems.Text = "Registro añadido exitosamente";
                    PLimpiarCamposRepa();
                    //PanelABMReparaciones.Enabled = false;
                    ButtonRepaGrabar.Enabled = false;
                    ButtonRepaCancelar.Enabled = false;
                }
                else
                {
                    LabelRepaRegistroItems.Text = "El Registro no pudo ser modificado";
                }
            }

            int vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);

            FlTraeDatosDPReparacion(vIdCotizacion);

        }

        protected void ButtonRepaCancelar_Click(object sender, EventArgs e)
        {
            //PanelABMReparaciones.Enabled = false;
            ButtonRepaGrabar.Enabled = false;
            ButtonRepaCancelar.Enabled = false;
            PLimpiarCamposRepa();
        }

        protected void GridViewReparaciones_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            bool vResultado = false;
            int vIdFlujo = 0;
            int vIdCotizacion = 0;
            long vIdItem = 0;

            vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
            vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);
            vIdItem = long.Parse(GridViewReparaciones.SelectedRow.Cells[1].Text);
            vResultado = BD.CotizacionICRL.RoboParcialBorrar(vIdFlujo, vIdCotizacion, vIdItem);
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
            string vTextoTemporal = string.Empty;

            //Leer Registro de la grilla y cargar los valores a la ventana.
            TextBoxRepaIdItem.Text = GridViewReparaciones.SelectedRow.Cells[1].Text;
            //tipo_item:  1 = Reparacion  2 = Repuesto
            vTextoTemporal = string.Empty;
            vTextoTemporal = GridViewReparaciones.SelectedRow.Cells[2].Text;
            DropDownListRepaItem.ClearSelection();
            DropDownListRepaItem.Items.FindByText(vTextoTemporal).Selected = true;

            vTextoTemporal = string.Empty;
            vTextoTemporal = GridViewReparaciones.SelectedRow.Cells[3].Text;
            vTextoTemporal = vTextoTemporal.Replace("&nbsp;", string.Empty);
            DropDownListRepaChaperio.ClearSelection();
            DropDownListRepaChaperio.Items.FindByText(vTextoTemporal).Selected = true;

            vTextoTemporal = string.Empty;
            vTextoTemporal = GridViewReparaciones.SelectedRow.Cells[4].Text;
            vTextoTemporal = vTextoTemporal.Replace("&nbsp;", string.Empty);
            DropDownListRepaRepPrevia.ClearSelection();
            DropDownListRepaRepPrevia.Items.FindByText(vTextoTemporal).Selected = true;

            CheckBoxRepaMecanico.Checked = (GridViewReparaciones.SelectedRow.Cells[5].Controls[1] as CheckBox).Checked;

            vTextoTemporal = string.Empty;
            vTextoTemporal = GridViewReparaciones.SelectedRow.Cells[6].Text;
            DropDownListRepaMoneda.ClearSelection();
            DropDownListRepaMoneda.Items.FindByText(vTextoTemporal).Selected = true;

            TextBoxRepaPrecioCotizado.Text = GridViewReparaciones.SelectedRow.Cells[7].Text;

            vTextoTemporal = string.Empty;
            vTextoTemporal = GridViewReparaciones.SelectedRow.Cells[8].Text;
            DropDownListRepaTipoDesc.ClearSelection();
            DropDownListRepaTipoDesc.Items.FindByText(vTextoTemporal).Selected = true;

            TextBoxRepaMontoDesc.Text = GridViewReparaciones.SelectedRow.Cells[9].Text;

            TextBoxRepaPrecioFinal.Text = GridViewReparaciones.SelectedRow.Cells[10].Text;

            vTextoTemporal = string.Empty;
            vTextoTemporal = GridViewReparaciones.SelectedRow.Cells[11].Text;
            DropDownListRepaProveedor.ClearSelection();
            DropDownListRepaProveedor.Items.FindByText(vTextoTemporal).Selected = true;

            PRepaModificarItem();
            Session["PopupABMReparacionesHabilitado"] = 1;
            this.ModalPopupReparaciones.Show();
        }

        protected void ButtonCancelPopReparaciones_Click(object sender, EventArgs e)
        {
            int vResul = 0;
            //PLimpiaSeccionDaniosPropiosPadre();
            //PLimpiaSeccionDatosPropios();
            //PBloqueaDPPadreEdicion(false);
            //PBloqueaDPEdicion(false);
            //vResul = FlTraeDatosDaniosPropiosPadre(int.Parse(TextBoxNroInspeccion.Text));

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
            TextBoxRepuPrecioCotizado.Text = string.Empty;
            DropDownListRepuTipoDesc.SelectedIndex = 0;
            TextBoxRepuMontoDesc.Text = string.Empty;
            TextBoxRepuPrecioFinal.Text = string.Empty;
            DropDownListRepuProveedor.SelectedIndex = 0;
        }

        protected void ButtonRepuAgregarItem_Click(object sender, EventArgs e)
        {
            TextBoxRepuIdItem.Text = string.Empty;
            DropDownListRepuItem.Enabled = true;
            //PanelABMRepuestos.Enabled = true;
            ButtonRepuGrabar.Enabled = true;
            ButtonRepuCancelar.Enabled = true;
            Session["PopupABMRepuestosHabilitado"] = 1;
            this.ModalPopupRepuestos.Show();
        }

        protected void PRepuModificarItem()
        {
            //PanelABMRepuestos.Enabled = true;
            DropDownListRepuItem.Enabled = false;
            ButtonRepuGrabar.Enabled = true;
            ButtonRepuCancelar.Enabled = true;
        }

        protected void ButtonRepuGrabar_Click(object sender, EventArgs e)
        {
            LabelRepuRegistroItems.Text = "Items";
            BD.CotizacionICRL.TipoRoboParcial vTipoRoboParcial = new CotizacionICRL.TipoRoboParcial();

            //Completar los elementos del objeto y grabar el registro.
            vTipoRoboParcial.id_flujo = int.Parse(TextBoxIdFlujo.Text);
            vTipoRoboParcial.id_cotizacion = int.Parse(TextBoxNroCotizacion.Text);

            //tipo_item:  1 = Repuracion  2 = Repuesto
            vTipoRoboParcial.id_tipo_item = (int)CotizacionICRL.TipoItem.Repuesto;
            vTipoRoboParcial.item_descripcion = DropDownListRepuItem.SelectedItem.Text.Trim();
            vTipoRoboParcial.pintura = CheckBoxRepuPintura.Checked;
            vTipoRoboParcial.instalacion = CheckBoxRepuInstalacion.Checked;
            vTipoRoboParcial.id_moneda = DropDownListRepuMoneda.SelectedItem.Text.Trim();
            vTipoRoboParcial.precio_cotizado = double.Parse(TextBoxRepuPrecioCotizado.Text);
            vTipoRoboParcial.id_tipo_descuento = DropDownListRepuTipoDesc.SelectedItem.Text.Trim();
            vTipoRoboParcial.descuento = double.Parse(TextBoxRepuMontoDesc.Text);
            switch (vTipoRoboParcial.id_tipo_descuento)
            {
                case "Fijo":
                    vTipoRoboParcial.precio_final = vTipoRoboParcial.precio_cotizado - vTipoRoboParcial.descuento;
                    break;
                case "Porcentaje":
                    vTipoRoboParcial.precio_final = vTipoRoboParcial.precio_cotizado - (vTipoRoboParcial.precio_cotizado * (vTipoRoboParcial.descuento / 100));
                    break;
                default:
                    vTipoRoboParcial.precio_final = vTipoRoboParcial.precio_cotizado;
                    break;
            }
            vTipoRoboParcial.proveedor = DropDownListRepuProveedor.SelectedItem.Text.Trim();
            vTipoRoboParcial.id_estado = 1;

            double vTipoCambio = 0;
            vTipoCambio = double.Parse(TextBoxTipoCambio.Text);
            vTipoRoboParcial.tipo_cambio = vTipoCambio;

            bool vResultado = false;
            if (string.Empty != TextBoxRepuIdItem.Text)
            {
                vTipoRoboParcial.id_item = int.Parse(TextBoxRepuIdItem.Text);
                vResultado = BD.CotizacionICRL.RoboParcialModificar(vTipoRoboParcial);
                if (vResultado)
                {
                    LabelRepuRegistroItems.Text = "Registro modificado exitosamente";
                    PLimpiarCamposRepu();
                    //PanelABMRepuestos.Enabled = false;
                    ButtonRepuGrabar.Enabled = false;
                    ButtonRepuCancelar.Enabled = false;
                }
                else
                {
                    LabelRepuRegistroItems.Text = "El Registro no pudo ser añadido";
                }
            }
            else
            {
                vResultado = BD.CotizacionICRL.RoboParcialRegistrar(vTipoRoboParcial);
                if (vResultado)
                {
                    LabelRepuRegistroItems.Text = "Registro añadido exitosamente";
                    PLimpiarCamposRepu();
                    //PanelABMRepuestos.Enabled = false;
                    ButtonRepuGrabar.Enabled = false;
                    ButtonRepuCancelar.Enabled = false;
                }
                else
                {
                    LabelRepuRegistroItems.Text = "El Registro no pudo ser modificado";
                }
            }

            int vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);

            FlTraeDatosDPRepuesto(vIdCotizacion);

        }

        protected void ButtonRepuCancelar_Click(object sender, EventArgs e)
        {
            //PanelABMRepuestos.Enabled = false;
            ButtonRepuGrabar.Enabled = false;
            ButtonRepuCancelar.Enabled = false;
            PLimpiarCamposRepu();
        }

        protected void GridViewRepuestos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            bool vResultado = false;
            int vIdFlujo = 0;
            int vIdCotizacion = 0;
            long vIdItem = 0;

            vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
            vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);
            vIdItem = long.Parse(GridViewRepuestos.SelectedRow.Cells[1].Text);
            vResultado = BD.CotizacionICRL.RoboParcialBorrar(vIdFlujo, vIdCotizacion, vIdItem);
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
            string vTextoTemporal = string.Empty;

            //Leer Registro de la grilla y cargar los valores a la ventana.
            TextBoxRepuIdItem.Text = GridViewRepuestos.SelectedRow.Cells[1].Text;
            //tipo_item:  1 = Repuracion  2 = Repuesto
            vTextoTemporal = string.Empty;
            vTextoTemporal = GridViewRepuestos.SelectedRow.Cells[2].Text;
            DropDownListRepuItem.ClearSelection();
            DropDownListRepuItem.Items.FindByText(vTextoTemporal).Selected = true;

            CheckBoxRepuPintura.Checked = (GridViewRepuestos.SelectedRow.Cells[3].Controls[1] as CheckBox).Checked;

            CheckBoxRepuInstalacion.Checked = (GridViewRepuestos.SelectedRow.Cells[4].Controls[1] as CheckBox).Checked;

            vTextoTemporal = string.Empty;
            vTextoTemporal = GridViewRepuestos.SelectedRow.Cells[5].Text;
            DropDownListRepuMoneda.ClearSelection();
            DropDownListRepuMoneda.Items.FindByText(vTextoTemporal).Selected = true;

            TextBoxRepuPrecioCotizado.Text = GridViewRepuestos.SelectedRow.Cells[6].Text;

            vTextoTemporal = string.Empty;
            vTextoTemporal = GridViewRepuestos.SelectedRow.Cells[7].Text;
            DropDownListRepuTipoDesc.ClearSelection();
            DropDownListRepuTipoDesc.Items.FindByText(vTextoTemporal).Selected = true;

            TextBoxRepuMontoDesc.Text = GridViewRepuestos.SelectedRow.Cells[8].Text;

            TextBoxRepuPrecioFinal.Text = GridViewRepuestos.SelectedRow.Cells[9].Text;

            vTextoTemporal = string.Empty;
            vTextoTemporal = GridViewRepuestos.SelectedRow.Cells[10].Text;
            DropDownListRepuProveedor.ClearSelection();
            DropDownListRepuProveedor.Items.FindByText(vTextoTemporal).Selected = true;

            PRepuModificarItem();
            Session["PopupABMRepuestosHabilitado"] = 1;
            this.ModalPopupRepuestos.Show();
        }

        protected void ButtonCancelPopRepuestos_Click(object sender, EventArgs e)
        {
            int vResul = 0;
            //PLimpiaSeccionDaniosPropiosPadre();
            //PLimpiaSeccionDatosPropios();
            //PBloqueaDPPadreEdicion(false);
            //PBloqueaDPEdicion(false);
            //vResul = FlTraeDatosDaniosPropiosPadre(int.Parse(TextBoxNroInspeccion.Text));

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

            BD.CotizacionICRL.TipoRoboParcialSumatoriaTraer vTipoRoboParcialSumatoriaTraer;
            vTipoRoboParcialSumatoriaTraer = CotizacionICRL.RoboParcialSumatoriaTraer(pIdFlujo, pIdCotizacion, pTipoItem);

            GridViewSumaReparaciones.DataSource = vTipoRoboParcialSumatoriaTraer.RoboParcialSumatoria.Select(RoboParcialSumatoria => new
            {
                RoboParcialSumatoria.proveedor,
                RoboParcialSumatoria.monto_orden,
                RoboParcialSumatoria.id_tipo_descuento_orden,
                RoboParcialSumatoria.descuento_proveedor,
                RoboParcialSumatoria.deducible,
                RoboParcialSumatoria.monto_final
            }).ToList();
            GridViewSumaReparaciones.DataBind();

            return vResultado;
        }

        protected void ButtonRepaGenerarResumen_Click(object sender, EventArgs e)
        {
            int vIdFlujo = 0;
            int vIdCotizacion = 0;
            short vTipoItem = 0;
            bool vResultado = false;

            //Completar los elementos del objeto y grabar el registro.
            vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
            vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);
            vTipoItem = (short)CotizacionICRL.TipoItem.Reparacion;


            vResultado = CotizacionICRL.RoboParcialSumatoriaGenerar(vIdFlujo, vIdCotizacion, vTipoItem);
            if (vResultado)
            {
                LabelRepaRegistroItems.Text = "Reparaciones sumarizadas exitosamente";
            }
            else
            {
                LabelRepaRegistroItems.Text = "Reparaciones NO sumarizadas exitosamente";
            }

            FlTraeDatosSumatoriaReparaciones(vIdFlujo, vIdCotizacion, vTipoItem);
        }

        protected void ButtonSumaGrabar_Click(object sender, EventArgs e)
        {
            LabelSumaRegistroItems.Text = "Items - Sumatoria";
            CotizacionICRL.TipoRoboParcialSumatoria vTipoRoboParcialSumatoria = new CotizacionICRL.TipoRoboParcialSumatoria();

            //Completar los elementos del objeto y grabar el registro.
            vTipoRoboParcialSumatoria.id_flujo = int.Parse(TextBoxIdFlujo.Text);
            vTipoRoboParcialSumatoria.id_cotizacion = int.Parse(TextBoxNroCotizacion.Text);
            short vTipoItem = 0;
            vTipoItem = short.Parse(Session["TipoItem"].ToString());

            vTipoRoboParcialSumatoria.id_tipo_item = vTipoItem;

            vTipoRoboParcialSumatoria.proveedor = DropDownListSumaProveedor.SelectedItem.Text.Trim();
            vTipoRoboParcialSumatoria.monto_orden = double.Parse(TextBoxSumaMontoOrden.Text);
            vTipoRoboParcialSumatoria.id_tipo_descuento_orden = DropDownListSumaTipoDesc.SelectedItem.Text.Trim();
            vTipoRoboParcialSumatoria.descuento_proveedor = double.Parse(TextBoxSumaMontoDescProv.Text);
            vTipoRoboParcialSumatoria.deducible = double.Parse(TextBoxSumaDeducible.Text);
            switch (vTipoRoboParcialSumatoria.id_tipo_descuento_orden)
            {
                case "Fijo":
                    vTipoRoboParcialSumatoria.monto_final = vTipoRoboParcialSumatoria.monto_orden - vTipoRoboParcialSumatoria.descuento_proveedor;
                    break;
                case "Porcentaje":
                    vTipoRoboParcialSumatoria.monto_final = vTipoRoboParcialSumatoria.monto_orden - (vTipoRoboParcialSumatoria.monto_orden * (vTipoRoboParcialSumatoria.descuento_proveedor / 100));
                    break;
                default:
                    vTipoRoboParcialSumatoria.monto_final = vTipoRoboParcialSumatoria.monto_orden;
                    break;
            }

            bool vResultado = false;

            vResultado = BD.CotizacionICRL.RoboParcialSumatoriaModificar(vTipoRoboParcialSumatoria);
            if (vResultado)
            {
                LabelSumaRegistroItems.Text = "Registro modificado exitosamente";
                //PLimpiarCamposRepa();
                //PanelABMReparaciones.Enabled = false;
                ButtonSumaGrabar.Enabled = false;
                ButtonSumaCancelar.Enabled = false;
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
            //PanelABMReparaciones.Enabled = false;
            ButtonSumaGrabar.Enabled = false;
            ButtonSumaCancelar.Enabled = false;
            //PLimpiarCamposRepa();
        }

        protected void ButtonCancelPopSumatorias_Click(object sender, EventArgs e)
        {
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
            string vTextoTemporal = string.Empty;

            short vTipoItem = (short)CotizacionICRL.TipoItem.Reparacion;
            Session["TipoItem"] = vTipoItem;

            PSumaModificarItem(vTipoItem);

            //Leer Registro de la grilla y cargar los valores a la ventana.
            //Proveedor
            vTextoTemporal = string.Empty;
            vTextoTemporal = GridViewSumaReparaciones.SelectedRow.Cells[0].Text;
            DropDownListSumaProveedor.ClearSelection();
            DropDownListSumaProveedor.Items.FindByText(vTextoTemporal).Selected = true;

            //Monto Orden
            TextBoxSumaMontoOrden.Text = GridViewSumaReparaciones.SelectedRow.Cells[1].Text;

            //Tipo Descuento
            vTextoTemporal = string.Empty;
            vTextoTemporal = GridViewSumaReparaciones.SelectedRow.Cells[2].Text;
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

            BD.CotizacionICRL.TipoRoboParcialSumatoriaTraer vTipoRoboParcialSumatoriaTraer;
            vTipoRoboParcialSumatoriaTraer = CotizacionICRL.RoboParcialSumatoriaTraer(pIdFlujo, pIdCotizacion, pTipoItem);

            GridViewSumaRepuestos.DataSource = vTipoRoboParcialSumatoriaTraer.RoboParcialSumatoria.Select(RoboParcialSumatoria => new
            {
                RoboParcialSumatoria.proveedor,
                RoboParcialSumatoria.monto_orden,
                RoboParcialSumatoria.id_tipo_descuento_orden,
                RoboParcialSumatoria.descuento_proveedor,
                RoboParcialSumatoria.deducible,
                RoboParcialSumatoria.monto_final
            }).ToList();
            GridViewSumaRepuestos.DataBind();

            return vResultado;
        }

        protected void ButtonRepuGenerarResumen_Click(object sender, EventArgs e)
        {
            int vIdFlujo = 0;
            int vIdCotizacion = 0;
            short vTipoItem = 0;
            bool vResultado = false;

            //Completar los elementos del objeto y grabar el registro.
            vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
            vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);
            vTipoItem = (short)CotizacionICRL.TipoItem.Repuesto;


            vResultado = CotizacionICRL.RoboParcialSumatoriaGenerar(vIdFlujo, vIdCotizacion, vTipoItem);
            if (vResultado)
            {
                LabelRepuRegistroItems.Text = "Reparaciones sumarizadas exitosamente";
            }
            else
            {
                LabelRepuRegistroItems.Text = "Reparaciones NO sumarizadas exitosamente";
            }

            FlTraeDatosSumatoriaRepuestos(vIdFlujo, vIdCotizacion, vTipoItem);
        }

        protected void GridViewSumaRepuestos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vTextoTemporal = string.Empty;

            short vTipoItem = (short)CotizacionICRL.TipoItem.Repuesto;
            Session["TipoItem"] = vTipoItem;

            PSumaModificarItem(vTipoItem);

            //Leer Registro de la grilla y cargar los valores a la ventana.
            //Proveedor
            vTextoTemporal = string.Empty;
            vTextoTemporal = GridViewSumaRepuestos.SelectedRow.Cells[0].Text;
            DropDownListSumaProveedor.ClearSelection();
            DropDownListSumaProveedor.Items.FindByText(vTextoTemporal).Selected = true;

            //Monto Orden
            TextBoxSumaMontoOrden.Text = GridViewSumaRepuestos.SelectedRow.Cells[1].Text;

            //Tipo Descuento
            vTextoTemporal = string.Empty;
            vTextoTemporal = GridViewSumaRepuestos.SelectedRow.Cells[2].Text;
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

            BD.CotizacionICRL.TipoRoboParcialTraer vTipoRoboParcialTraer;
            vTipoRoboParcialTraer = CotizacionICRL.RoboParcialTraer(vIdFlujo, pIdCotizacion);

            GridViewRecepRepuestos.DataSource = vTipoRoboParcialTraer.RobosParciales.Select(RobosParciales => new
            {
                RobosParciales.id_item,
                RobosParciales.item_descripcion,
                RobosParciales.recepcion,
                RobosParciales.dias_entrega,
                RobosParciales.id_tipo_item
            }).Where(RobosParciales => RobosParciales.id_tipo_item == 2).ToList();
            GridViewRecepRepuestos.DataBind();

            return vResultado;
        }

        protected void GridViewRecepRepuestos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vTextoTemporal = string.Empty;

            short vTipoItem = (short)CotizacionICRL.TipoItem.Repuesto;
            Session["TipoItem"] = vTipoItem;

            PRecepModificarItem();

            //Leer Registro de la grilla y cargar los valores a la ventana.
            TextBoxRecepIdItem.Text = GridViewRecepRepuestos.SelectedRow.Cells[0].Text;
            //tipo_item:  1 = Repuracion  2 = Repuesto
            vTextoTemporal = string.Empty;
            vTextoTemporal = GridViewRecepRepuestos.SelectedRow.Cells[1].Text;
            DropDownListRecepItem.ClearSelection();
            DropDownListRecepItem.Items.FindByText(vTextoTemporal).Selected = true;

            CheckBoxRecepRecibido.Checked = (GridViewRecepRepuestos.SelectedRow.Cells[2].Controls[1] as CheckBox).Checked;

            TextBoxRecepDiasEntrega.Text = GridViewRecepRepuestos.SelectedRow.Cells[3].Text;

            Session["PopupRecepRepuHabilitado"] = 1;
            this.ModalPopupRecepRepuestos.Show();
        }

        protected void ButtonCancelPopRecepRepuestos_Click(object sender, EventArgs e)
        {
            Session["PopupRecepRepuHabilitado"] = 0;
            this.ModalPopupRecepRepuestos.Hide();
        }

        protected void ButtonRecepGrabar_Click(object sender, EventArgs e)
        {
            int vIdFlujo = 0;
            int vIdCotizacion = 0;
            long vIdItem = 0;

            LabelRecepRegistroItems.Text = "Items";
            BD.CotizacionICRL.TipoRoboParcial vTipoRoboParcial = new CotizacionICRL.TipoRoboParcial();

            vIdFlujo = int.Parse(TextBoxIdFlujo.Text); ;
            vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);
            vIdItem = long.Parse(TextBoxRecepIdItem.Text);

            //Para no perder los datos originales recuperamos el registro afectado primero y luego actualizamos los datos correspondientes
            BD.CotizacionICRL.TipoRoboParcialTraer vTipoRoboParcialTraer;
            vTipoRoboParcialTraer = CotizacionICRL.RoboParcialTraer(vIdFlujo, vIdCotizacion, vIdItem);

            //Proceder solo si se trajo correctamente la información.
            if (vTipoRoboParcialTraer.Correcto)
            {
                vTipoRoboParcial = vTipoRoboParcialTraer.RobosParciales[0];

                //Solo actualizar los datos que se pudieron modificar
                vTipoRoboParcial.recepcion = CheckBoxRecepRecibido.Checked;
                vTipoRoboParcial.dias_entrega = int.Parse(TextBoxRecepDiasEntrega.Text);

                bool vResultado = false;

                vResultado = BD.CotizacionICRL.RoboParcialModificar(vTipoRoboParcial);
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
            ButtonRepuGrabar.Enabled = false;
            ButtonRepuCancelar.Enabled = false;
            PLimpiarCamposRecep();
        }

        #endregion
    }
}