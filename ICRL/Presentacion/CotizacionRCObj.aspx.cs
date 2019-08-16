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
    public partial class CotizacionRCObj : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
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
            TextBoxObjMontoItemRef.Text = string.Empty;
            TextBoxObjDescripcion.Text = string.Empty;
        }

        protected void ButtonObjAgregar_Click(object sender, EventArgs e)
        {
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
    }
}