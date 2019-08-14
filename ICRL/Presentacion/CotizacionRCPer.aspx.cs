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
    public partial class CotizacionRCPer : System.Web.UI.Page
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


                    //Cargar Datos Persona
                    vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);
                    PModificarPersona(false);
                    PCargaDatosPer();
                    pCargaGrillaPersonas(vIdFlujo, vIdCotizacion);
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

        long FValidaTienePersona(int pIdFlujo, int pIdCotizacion)
        {
            long vResultado = 0;

            BD.CotizacionICRL.TipoRCPersonasTraer vTipoRCPersonasTraer;
            vTipoRCPersonasTraer = CotizacionICRL.RCPersonasTraer(pIdFlujo, pIdCotizacion);

            if (vTipoRCPersonasTraer.Correcto)
            {
                var vFilaTabla = vTipoRCPersonasTraer.RCPersonas.FirstOrDefault();
                if (vFilaTabla != null)
                {
                    vResultado = vFilaTabla.id_item;
                }
            }

            return vResultado;
        }

        protected void PCargaDatosPer()
        {
            long vIdItem = 0;
            int vIdFlujo = 0;
            int vIdCotizacion = 0;
            string vTextoTemporal = string.Empty;

            vIdFlujo = int.Parse(TextBoxIdFlujo.Text); ;
            vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);

            vIdItem = FValidaTienePersona(vIdFlujo, vIdCotizacion);
            if (vIdItem > 0)
            {
                BD.CotizacionICRL.TipoRCPersonasTraer vTipoRCPersonasTraer;
                vTipoRCPersonasTraer = CotizacionICRL.RCPersonasTraer(vIdFlujo, vIdCotizacion);
                var vFilaTabla = vTipoRCPersonasTraer.RCPersonas.FirstOrDefault();

                TextBoxPerNombreTer.Text = vFilaTabla.nombre_apellido;
                TextBoxPerTelefono.Text = vFilaTabla.telefono_contacto;
                TextBoxPerDocId.Text = vFilaTabla.numero_documento;
                CheckBoxPerReembolso.Checked = vFilaTabla.rembolso;

                TextBoxPerIdItem.Text = vFilaTabla.id_item.ToString();
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
        //        RCVehiculares.chaperio,
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

        #region Datos Persona
        protected void PModificarPersona(bool pEstado)
        {
            TextBoxPerNombreTer.Enabled = pEstado;
            TextBoxPerDocId.Enabled = pEstado;
            TextBoxPerTelefono.Enabled = pEstado;
            CheckBoxPerReembolso.Enabled = pEstado;
            TextBoxPerTipoGasto.Enabled = pEstado;
            TextBoxPerMontoGasto.Enabled = pEstado;
            TextBoxPerDescripcion.Enabled = pEstado;
        }

        protected void PModificarPersonaDet(bool pEstado)
        {
            TextBoxPerTelefono.Enabled = pEstado;
            CheckBoxPerReembolso.Enabled = pEstado;
            TextBoxPerTipoGasto.Enabled = pEstado;
            TextBoxPerMontoGasto.Enabled = pEstado;
            TextBoxPerDescripcion.Enabled = pEstado;
        }

        protected void ButtonPerAgregar_Click(object sender, EventArgs e)
        {
            LabelDatosPersonaMsj.Text = string.Empty;
            //validamos si existe el dato de Persona
            int vIdFlujo = 0;
            int vIdCotizacion = 0;
            long vIdItem = 0;
            vIdFlujo = int.Parse(TextBoxIdFlujo.Text); ;
            vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);

            vIdItem = FValidaTienePersona(vIdFlujo, vIdCotizacion);
            if (vIdItem > 0)
            {
                //Si existe
                PModificarPersonaDet(true);
                ButtonPerAgregar.Visible = false;
                ButtonPerGrabar.Visible = true;
                ButtonPerCancelar.Visible = true;
            }
            else
            {
                //No existe
                PModificarPersona(true);
                ButtonPerAgregar.Visible = false;
                ButtonPerGrabar.Visible = true;
                ButtonPerCancelar.Visible = true;
            }
        }

        protected void ButtonPerGrabar_Click(object sender, EventArgs e)
        {
            int vIdFlujo = 0;
            int vIdCotizacion = 0;
            long vIdItem = 0;
            bool vResultado = false;

            vIdFlujo = int.Parse(TextBoxIdFlujo.Text); ;
            vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);

            //cargar los datos del panel al objeto correspondiente
            CotizacionICRL.TipoRCPersonas vTipoRCPersonas = new CotizacionICRL.TipoRCPersonas();
            vTipoRCPersonas.id_flujo = vIdFlujo;
            vTipoRCPersonas.id_cotizacion = vIdCotizacion;
            vTipoRCPersonas.nombre_apellido = TextBoxPerNombreTer.Text.ToUpper();
            vTipoRCPersonas.telefono_contacto = TextBoxPerTelefono.Text.ToUpper();
            vTipoRCPersonas.numero_documento = TextBoxPerDocId.Text.ToUpper();
            vTipoRCPersonas.rembolso = CheckBoxPerReembolso.Checked;
            vTipoRCPersonas.tipo_gasto = TextBoxPerTipoGasto.Text.ToUpper();
            vTipoRCPersonas.monto_gasto = double.Parse(TextBoxPerMontoGasto.Text);
            vTipoRCPersonas.descripcion = TextBoxPerDescripcion.Text.ToUpper();
            vTipoRCPersonas.tipo_cambio = double.Parse(TextBoxTipoCambio.Text);
            vTipoRCPersonas.id_estado = 1;

            //validar si existe el registro de la persona
            if (string.Empty != TextBoxPerIdItem.Text)
            {
                //Existe el registro del tercero
                vIdItem = long.Parse(TextBoxPerIdItem.Text);
                vTipoRCPersonas.id_item = vIdItem;
                vResultado = CotizacionICRL.RCPersonaModificar(vTipoRCPersonas);
            }
            else
            {
                //NO Existe el registro del tercero
                vResultado = CotizacionICRL.RCPersonaRegistrar(vTipoRCPersonas);
            }

            if (vResultado)
            {
                LabelDatosPersonaMsj.Text = "Registro Actualizado Exitosamente";
                vIdItem = FValidaTienePersona(vIdFlujo, vIdCotizacion);
                TextBoxPerIdItem.Text = string.Empty;
            }
            else
            {
                LabelDatosPersonaMsj.Text = "El Registro no se actualizo correctamente";
            }

            PModificarPersona(false);
            ButtonPerAgregar.Visible = true;
            ButtonPerGrabar.Visible = false;
            ButtonPerCancelar.Visible = false;
            pCargaGrillaPersonas(vIdFlujo, vIdCotizacion);
        }

        protected void ButtonPerCancelar_Click(object sender, EventArgs e)
        {
            PModificarPersona(false);
            ButtonPerAgregar.Visible = true;
            ButtonPerGrabar.Visible = false;
            ButtonPerCancelar.Visible = false;
        }
        #endregion

        #region Grilla PersonasDet

        protected void pCargaGrillaPersonas(int pIdFlujo, int pIdCotizacion)
        {
            BD.CotizacionICRL.TipoRCPersonasTraer vTipoRCPersonasTraer;
            vTipoRCPersonasTraer = CotizacionICRL.RCPersonasTraer(pIdFlujo, pIdCotizacion);

            GridViewPerDetalle.DataSource = vTipoRCPersonasTraer.RCPersonas.Select(RCPersonas => new
            {
                RCPersonas.id_item,
                RCPersonas.nombre_apellido,
                RCPersonas.numero_documento,
                RCPersonas.tipo_gasto,
                RCPersonas.monto_gasto,
                RCPersonas.descripcion,
            }).ToList();
            GridViewPerDetalle.DataBind();
        }

        #endregion
    }
}