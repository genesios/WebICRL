using System;
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
    public partial class CotizacionRCVehicular : System.Web.UI.Page
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

                    int vIdFlujo = int.Parse(TextBoxIdFlujo.Text);

                    FlTraeDatosDPReparacion(vIdCotizacion);
                    FlTraeDatosDPRepuesto(vIdCotizacion);
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
                               join cdp in db.CotiDaniosPropios on c.idCotizacion equals cdp.idCotizacion
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
                                   cdp.tipoTaller,
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
        }

        protected void PCargaCombosRepa()
        {
            FlTraeNomenItemRepa();
            FlTraeNomenChaperioRepa();
            FlTraeNomenRepPreviaRepa();
            FlTraeNomenMonedaRepa();
            FlTraeNomenTipoDescRepa();
            FlTraeNomenProveedorRepa();
        }

        protected void ButtonRepaAgregarItem_Click(object sender, EventArgs e)
        {
            PCargaCombosRepa();
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
            PCargaCombosRepa();
            //PanelABMReparaciones.Enabled = true;
            DropDownListRepaItem.Enabled = false;
            ButtonRepaGrabar.Enabled = true;
            ButtonRepaCancelar.Enabled = true;
        }

        protected void ButtonRepaGrabar_Click(object sender, EventArgs e)
        {
            LabelRepaRegistroItems.Text = "Items";
            BD.CotizacionICRL.TipoDaniosPropios vTipoDaniosPropios = new CotizacionICRL.TipoDaniosPropios();

            //Completar los elementos del objeto y grabar el registro.
            vTipoDaniosPropios.id_flujo = int.Parse(TextBoxIdFlujo.Text);
            vTipoDaniosPropios.id_cotizacion = int.Parse(TextBoxNroCotizacion.Text);

            //tipo_item:  1 = Reparacion  2 = Repuesto
            vTipoDaniosPropios.id_tipo_item = (int)CotizacionICRL.TipoItem.Reparacion;
            vTipoDaniosPropios.item_descripcion = DropDownListRepaItem.SelectedItem.Text.Trim();
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
                }
                else
                {
                    LabelRepaRegistroItems.Text = "El Registro no pudo ser añadido";
                }
            }
            else
            {
                vResultado = BD.CotizacionICRL.DaniosPropiosRegistrar(vTipoDaniosPropios);
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
            DropDownListRepaChaperio.ClearSelection();
            DropDownListRepaChaperio.Items.FindByText(vTextoTemporal).Selected = true;

            vTextoTemporal = string.Empty;
            vTextoTemporal = GridViewReparaciones.SelectedRow.Cells[4].Text;
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
        }

        protected void PCargaCombosRepu()
        {
            FlTraeNomenItemRepu();
            FlTraeNomenMonedaRepu();
            FlTraeNomenTipoDescRepu();
            FlTraeNomenProveedorRepu();
        }

        protected void ButtonRepuAgregarItem_Click(object sender, EventArgs e)
        {
            PCargaCombosRepu();
            TextBoxRepuIdItem.Text = string.Empty;
            DropDownListRepuItem.Enabled = true;
            PanelABMRepuestos.Enabled = true;
            ButtonRepuGrabar.Enabled = true;
            ButtonRepuCancelar.Enabled = true;
        }

        protected void PRepuModificarItem()
        {
            PCargaCombosRepu();
            PanelABMRepuestos.Enabled = true;
            DropDownListRepuItem.Enabled = false;
            ButtonRepuGrabar.Enabled = true;
            ButtonRepuCancelar.Enabled = true;
        }

        protected void ButtonRepuGrabar_Click(object sender, EventArgs e)
        {
            LabelRepuRegistroItems.Text = "Items";
            BD.CotizacionICRL.TipoDaniosPropios vTipoDaniosPropios = new CotizacionICRL.TipoDaniosPropios();

            //Completar los elementos del objeto y grabar el registro.
            vTipoDaniosPropios.id_flujo = int.Parse(TextBoxIdFlujo.Text);
            vTipoDaniosPropios.id_cotizacion = int.Parse(TextBoxNroCotizacion.Text);

            //tipo_item:  1 = Repuracion  2 = Repuesto
            vTipoDaniosPropios.id_tipo_item = (int)CotizacionICRL.TipoItem.Repuesto;
            vTipoDaniosPropios.item_descripcion = DropDownListRepuItem.SelectedItem.Text.Trim();
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
                    PanelABMRepuestos.Enabled = false;
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
                vResultado = BD.CotizacionICRL.DaniosPropiosRegistrar(vTipoDaniosPropios);
                if (vResultado)
                {
                    LabelRepuRegistroItems.Text = "Registro añadido exitosamente";
                    PLimpiarCamposRepu();
                    PanelABMRepuestos.Enabled = false;
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
            PanelABMRepuestos.Enabled = false;
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
            vResultado = BD.CotizacionICRL.DaniosPropiosBorrar(vIdFlujo, vIdCotizacion, vIdItem);
            if (vResultado)
            {
                LabelRepuRegistroItems.Text = "Registro Borrado exitosamente";
                PLimpiarCamposRepu();
                PanelABMRepuestos.Enabled = false;
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
            PRepuModificarItem();
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

        }
        #endregion
    }



}