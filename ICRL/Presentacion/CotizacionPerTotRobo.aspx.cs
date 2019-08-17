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

namespace ICRL.Presentacion
{
    public partial class CotizacionPerTotRobo : System.Web.UI.Page
    {
        public DataTable dtBeneficiario = new DataTable();
        public DataTable dtReferencia;

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
                    PCreaDataTableBenef();

                    vIdFlujo = int.Parse(TextBoxIdFlujo.Text);

                    FlTraeDatosCotizacion(vIdCotizacion, vlNumFlujo);

                    //Cargar Datos Objeto
                    vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);

                    long vIdItem = 0;
                    vIdItem = FValidaTienePTRobo(vIdFlujo, vIdCotizacion);
                    if (0 == vIdItem)
                    {
                        //No existe registro se debe crear la base
                        PCreaBeneficiarioPrincipal();
                    }
                    PCargarGrillaBeneficiarios(vIdFlujo, vIdCotizacion);
                    vIdItem = FValidaTienePTRobo(vIdFlujo, vIdCotizacion);
                    PCargarGrillaReferencias(vIdFlujo, vIdCotizacion, vIdItem);
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

        long FValidaTienePTRobo(int pIdFlujo, int pIdCotizacion)
        {
            long vResultado = 0;

            //CotizacionICRL.TipoPerdidaTotalRobo
            CotizacionICRL.TipoPerdidaTotalRoboTraer vTipoPerdidaTotalRoboTraer;
            vTipoPerdidaTotalRoboTraer = CotizacionICRL.PerdidaTotalRoboTraer(pIdFlujo, pIdCotizacion);

            if (vTipoPerdidaTotalRoboTraer.Correcto)
            {
                var vFilaTabla = vTipoPerdidaTotalRoboTraer.PerdidasTotalesRobos.FirstOrDefault();
                if (vFilaTabla != null)
                {
                    vResultado = vFilaTabla.id_item;
                }
            }

            return vResultado;
        }

        #region antiguo_codigo
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

        #endregion

        #region Datos Especiales

        protected void ButtonNuevoPTRO_Click(object sender, EventArgs e)
        {

        }

        protected void ButtonGrabarPTRO_Click(object sender, EventArgs e)
        {

        }

        protected void ButtonBorrarPTRO_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region Beneficiarios
        protected void PCreaDataTableBenef()
        {
            //DataTable dtBeneficiario = new DataTable();
            dtBeneficiario.Columns.Add("id_fila", typeof(int));
            dtBeneficiario.Columns.Add("duenio_nombres", typeof(string));
            dtBeneficiario.Columns.Add("duenio_documento", typeof(string));
            dtBeneficiario.Columns.Add("duenio_monto", typeof(float));
            dtBeneficiario.Columns.Add("duenio_descripcion", typeof(string));
        }

        protected void PCreaBeneficiarioPrincipal()
        {
            //grabar el registro en la tabla
            bool vResultado = false;
            int vIdFlujo = 0;
            int vIdCotizacion = 0;

            vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
            vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);
            CotizacionICRL.TipoPerdidaTotalRobo vTipoPerdidaTotalRobo = new CotizacionICRL.TipoPerdidaTotalRobo();

            vTipoPerdidaTotalRobo.id_flujo = vIdFlujo;
            vTipoPerdidaTotalRobo.id_cotizacion = vIdCotizacion;
            vTipoPerdidaTotalRobo.condiciones_especiales = false;
            vTipoPerdidaTotalRobo.duenio_nombres_1 = TextBoxNombreAsegurado.Text;
            vTipoPerdidaTotalRobo.duenio_documento_1 = string.Empty;
            vTipoPerdidaTotalRobo.duenio_monto_1 = 0;
            vTipoPerdidaTotalRobo.duenio_descripcion_1 = "Pago Asegurado";

            vResultado = CotizacionICRL.PerdidaTotalRoboRegistrar(vTipoPerdidaTotalRobo);
            if (vResultado)
            {
                //se Grabo exitosamente el registro
                LabelDatosDueniosMsj.Text = "El Registro De Beneficiario Principal se creo exitosamente";
            }
            else
            {
                LabelDatosDueniosMsj.Text = "El Registro De Beneficiario Principal no se pudo crear";
            }
        }

        protected void CheckBoxPTROTotPagado_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxPTROTotPagado.Checked)
            {
                //el checkbox chequeado indica que no esta completamente pagado
                //hay mas de un beneficiario
                ButtonPTROAgregar.Visible = true;
                ButtonPTROGrabar.Visible = false;
                ButtonPTROCancelar.Visible = false;
            }
            else
            {
                //el checkbox NO chequeado indica que esta completamente pagado
                //solo hay un beneficiario
                ButtonPTROAgregar.Visible = false;
                ButtonPTROGrabar.Visible = false;
                ButtonPTROCancelar.Visible = false;
            }
        }


        #endregion

        #region GrillaDueniosBeneficiarios

        protected void PCargarGrillaBeneficiarios(int pIdFlujo, int pIdCotizacion)
        {
            CotizacionICRL.TipoPerdidaTotalRoboTraer vTipoPerdidaTotalRoboTraer;
            vTipoPerdidaTotalRoboTraer = CotizacionICRL.PerdidaTotalRoboTraer(pIdFlujo, pIdCotizacion);

            if (vTipoPerdidaTotalRoboTraer.Correcto)
            {
                var vFilaTabla = vTipoPerdidaTotalRoboTraer.PerdidasTotalesRobos.FirstOrDefault();
                if (vFilaTabla != null)
                {
                    DataTable dsTemp = new DataTable();
                    dsTemp = null;

                    GridViewPTRODueniosaPagar.DataSource = dsTemp;
                    GridViewPTRODueniosaPagar.DataBind();
                    dtBeneficiario.Rows.Clear();

                    TextBoxPTROIdItem.Text = vFilaTabla.id_item.ToString();

                    //validar si se perdio la definicion de columnas del datatable
                    if (0 == dtBeneficiario.Columns.Count)
                    {
                        dtBeneficiario.Columns.Add("id_fila", typeof(int));
                        dtBeneficiario.Columns.Add("duenio_nombres", typeof(string));
                        dtBeneficiario.Columns.Add("duenio_documento", typeof(string));
                        dtBeneficiario.Columns.Add("duenio_monto", typeof(float));
                        dtBeneficiario.Columns.Add("duenio_descripcion", typeof(string));
                    }

                    //Beneficiario Principal, es el asegurado
                    if (string.Empty != vFilaTabla.duenio_descripcion_1)
                    {
                        DataRow vFila = null;
                        vFila = dtBeneficiario.NewRow();
                        vFila["id_fila"] = 1;
                        vFila["duenio_nombres"] = vFilaTabla.duenio_nombres_1;
                        vFila["duenio_documento"] = vFilaTabla.duenio_documento_1;
                        vFila["duenio_monto"] = vFilaTabla.duenio_monto_1;
                        vFila["duenio_descripcion"] = vFilaTabla.duenio_descripcion_1;

                        dtBeneficiario.Rows.Add(vFila);
                    }

                    //Segundo Beneficario si aplica
                    if (string.Empty != vFilaTabla.duenio_descripcion_2)
                    {
                        DataRow vFila = null;
                        vFila = dtBeneficiario.NewRow();
                        vFila["id_fila"] = 2;
                        vFila["duenio_nombres"] = vFilaTabla.duenio_nombres_2;
                        vFila["duenio_documento"] = vFilaTabla.duenio_documento_2;
                        vFila["duenio_monto"] = vFilaTabla.duenio_monto_2;
                        vFila["duenio_descripcion"] = vFilaTabla.duenio_descripcion_2;

                        dtBeneficiario.Rows.Add(vFila);
                    }

                    //Tercer Beneficario si aplica
                    if (string.Empty != vFilaTabla.duenio_descripcion_3)
                    {
                        DataRow vFila = null;
                        vFila = dtBeneficiario.NewRow();
                        vFila["id_fila"] = 3;
                        vFila["duenio_nombres"] = vFilaTabla.duenio_nombres_3;
                        vFila["duenio_documento"] = vFilaTabla.duenio_documento_3;
                        vFila["duenio_monto"] = vFilaTabla.duenio_monto_3;
                        vFila["duenio_descripcion"] = vFilaTabla.duenio_descripcion_3;

                        dtBeneficiario.Rows.Add(vFila);
                    }

                    //Cuarto Beneficario si aplica
                    if (string.Empty != vFilaTabla.duenio_descripcion_4)
                    {
                        DataRow vFila = null;
                        vFila = dtBeneficiario.NewRow();
                        vFila["id_fila"] = 4;
                        vFila["duenio_nombres"] = vFilaTabla.duenio_nombres_4;
                        vFila["duenio_documento"] = vFilaTabla.duenio_documento_4;
                        vFila["duenio_monto"] = vFilaTabla.duenio_monto_4;
                        vFila["duenio_descripcion"] = vFilaTabla.duenio_descripcion_4;

                        dtBeneficiario.Rows.Add(vFila);
                    }

                    //Quinto Beneficario si aplica
                    if (string.Empty != vFilaTabla.duenio_descripcion_5)
                    {
                        DataRow vFila = null;
                        vFila = dtBeneficiario.NewRow();
                        vFila["id_fila"] = 5;
                        vFila["duenio_nombres"] = vFilaTabla.duenio_nombres_5;
                        vFila["duenio_documento"] = vFilaTabla.duenio_documento_5;
                        vFila["duenio_monto"] = vFilaTabla.duenio_monto_5;
                        vFila["duenio_descripcion"] = vFilaTabla.duenio_descripcion_5;

                        dtBeneficiario.Rows.Add(vFila);
                    }

                    GridViewPTRODueniosaPagar.DataSource = dtBeneficiario;
                    GridViewPTRODueniosaPagar.DataBind();
                }
            }
        }

        protected void PHabilitarBeneficiarioPrincipal(bool pEstado)
        {
            if (pEstado)
            {
                TextBoxPTRONombres.Enabled = false;
                TextBoxPTRODocumentoId.Enabled = true;
                TextBoxPTROMontoPago.Enabled = true;
                TextBoxPTRODescripcion.Enabled = false;
            }
            else
            {
                TextBoxPTRONombres.Enabled = false;
                TextBoxPTRODocumentoId.Enabled = false;
                TextBoxPTROMontoPago.Enabled = false;
                TextBoxPTRODescripcion.Enabled = false;
            }
        }

        protected void PHabilitarBeneficiarioOtro(bool pEstado)
        {
            TextBoxPTRONombres.Enabled = pEstado;
            TextBoxPTRODocumentoId.Enabled = pEstado;
            TextBoxPTROMontoPago.Enabled = pEstado;
            TextBoxPTRODescripcion.Enabled = pEstado;
        }

        protected void PLimpiarBeneficiario()
        {
            TextBoxPTRONombres.Text = string.Empty;
            TextBoxPTRODocumentoId.Text = string.Empty;
            TextBoxPTROMontoPago.Text = string.Empty;
            TextBoxPTRODescripcion.Text = string.Empty;
        }

        protected void GridViewPTRODueniosaPagar_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vTextoTemporal = string.Empty;
            int vIndice = 0;

            //Leer Registro de la grilla y cargar los valores a la ventana.
            vIndice = int.Parse(GridViewPTRODueniosaPagar.SelectedRow.Cells[1].Text);
            TextBoxPTROIndice.Text = GridViewPTRODueniosaPagar.SelectedRow.Cells[1].Text;
            TextBoxPTRONombres.Text = GridViewPTRODueniosaPagar.SelectedRow.Cells[2].Text;
            vTextoTemporal = GridViewPTRODueniosaPagar.SelectedRow.Cells[3].Text;
            vTextoTemporal = vTextoTemporal.Replace("&nbsp;", string.Empty);
            TextBoxPTRODocumentoId.Text = vTextoTemporal;
            TextBoxPTROMontoPago.Text = GridViewPTRODueniosaPagar.SelectedRow.Cells[4].Text;
            TextBoxPTRODescripcion.Text = GridViewPTRODueniosaPagar.SelectedRow.Cells[5].Text;
            if (1 == vIndice)
            {
                PHabilitarBeneficiarioPrincipal(true);
            }
            else
            {
                PHabilitarBeneficiarioOtro(true);
            }
            //PModificarPersonaDet(true);
            ButtonPTROAgregar.Visible = false;
            ButtonPTROGrabar.Visible = true;
            ButtonPTROCancelar.Visible = true;
        }

        protected void GridViewPTRODueniosaPagar_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            bool vResultado = false;
            int vIdFlujo = 0;
            int vIdCotizacion = 0;
            long vIdItem = 0;

            vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
            vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);

            string vTextoSecuencial = string.Empty;
            int vIndex = 0;
            int vIndice = 0;

            vIndex = Convert.ToInt32(e.RowIndex);
            vIndice = Convert.ToInt32(GridViewPTRODueniosaPagar.DataKeys[vIndex].Value);
            vIdItem = long.Parse(TextBoxPTROIdItem.Text);

            CotizacionICRL.TipoPerdidaTotalRoboTraer vTipoPerdidaTotalRoboTraer;
            vTipoPerdidaTotalRoboTraer = CotizacionICRL.PerdidaTotalRoboTraer(vIdFlujo, vIdCotizacion);

            if (vTipoPerdidaTotalRoboTraer.Correcto)
            {
                var vFilaTabla = vTipoPerdidaTotalRoboTraer.PerdidasTotalesRobos.FirstOrDefault();
                if (vFilaTabla != null)
                {
                    //No se puede borrar el primer Beneficiario que es el asegurado
                    if (vIndice > 1)
                    {
                        if (2 == vIndice)
                        {
                            vFilaTabla.duenio_nombres_2 = string.Empty;
                            vFilaTabla.duenio_documento_2 = string.Empty;
                            vFilaTabla.duenio_monto_2 = 0;
                            vFilaTabla.duenio_descripcion_2 = string.Empty;
                        }

                        if (3 == vIndice)
                        {
                            vFilaTabla.duenio_nombres_3 = string.Empty;
                            vFilaTabla.duenio_documento_3 = string.Empty;
                            vFilaTabla.duenio_monto_3 = 0;
                            vFilaTabla.duenio_descripcion_3 = string.Empty;
                        }

                        if (4 == vIndice)
                        {
                            vFilaTabla.duenio_nombres_4 = string.Empty;
                            vFilaTabla.duenio_documento_4 = string.Empty;
                            vFilaTabla.duenio_monto_4 = 0;
                            vFilaTabla.duenio_descripcion_4 = string.Empty;
                        }

                        if (5 == vIndice)
                        {
                            vFilaTabla.duenio_nombres_5 = string.Empty;
                            vFilaTabla.duenio_documento_5 = string.Empty;
                            vFilaTabla.duenio_monto_5 = 0;
                            vFilaTabla.duenio_descripcion_5 = string.Empty;
                        }

                        vResultado = BD.CotizacionICRL.PerdidaTotalRoboModificar(vFilaTabla);
                        if (vResultado)
                        {
                            LabelDatosDueniosMsj.Text = "Registro Borrado exitosamente";

                            ButtonPTROGrabar.Visible = false;
                            ButtonPTROCancelar.Visible = false;
                        }
                        else
                        {
                            LabelDatosDueniosMsj.Text = "El Registro no pudo ser Borrado";
                        }
                    }

                    if (CheckBoxPTROTotPagado.Checked)
                    {
                        ButtonPTROAgregar.Visible = true;
                    }
                    else
                    {
                        ButtonPTROAgregar.Visible = false;
                    }
                    ButtonPTROGrabar.Visible = false;
                    ButtonPTROCancelar.Visible = false;
                    PLimpiarBeneficiario();
                    PHabilitarBeneficiarioOtro(false);
                    PCargarGrillaBeneficiarios(vIdFlujo, vIdCotizacion);
                }
            }
        }


        #endregion

        int FBuscaIndiceLibre(int pIdFlujo, int pIdCotizacion)
        {
            int vIndiceLibre = 0;
            CotizacionICRL.TipoPerdidaTotalRoboTraer vTipoPerdidaTotalRoboTraer;
            vTipoPerdidaTotalRoboTraer = CotizacionICRL.PerdidaTotalRoboTraer(pIdFlujo, pIdCotizacion);

            if (vTipoPerdidaTotalRoboTraer.Correcto)
            {
                var vFilaTabla = vTipoPerdidaTotalRoboTraer.PerdidasTotalesRobos.FirstOrDefault();
                if (vFilaTabla != null)
                {
                    if (string.Empty == vFilaTabla.duenio_nombres_2)
                    {
                        vIndiceLibre = 2;
                        return vIndiceLibre;
                    }
                    if (string.Empty == vFilaTabla.duenio_nombres_3)
                    {
                        vIndiceLibre = 3;
                        return vIndiceLibre;
                    }
                    if (string.Empty == vFilaTabla.duenio_nombres_4)
                    {
                        vIndiceLibre = 4;
                        return vIndiceLibre;
                    }
                    if (string.Empty == vFilaTabla.duenio_nombres_5)
                    {
                        vIndiceLibre = 5;
                        return vIndiceLibre;
                    }

                }
            }

            return vIndiceLibre;
        }

        protected void ButtonPTROAgregar_Click(object sender, EventArgs e)
        {
            int vCantidadFilasGrid = 0;
            vCantidadFilasGrid = GridViewPTRODueniosaPagar.Rows.Count;

            //Solo se puede tener 4 Beneficiarios adicionales al Beneficiario Principal
            if (vCantidadFilasGrid < 5)
            {
                int vIndiceLibre = 0;
                int vIdFlujo = 0;
                int vIdCotizacion = 0;

                vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
                vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);
                //Buscar el espacio libre en las referencias para agregar al beneficiario
                vIndiceLibre = FBuscaIndiceLibre(vIdFlujo, vIdCotizacion);
                if (vIndiceLibre > 0)
                {
                    TextBoxPTROIndice.Text = vIndiceLibre.ToString();
                    PLimpiarBeneficiario();
                    PHabilitarBeneficiarioOtro(true);
                    ButtonPTROGrabar.Visible = true;
                    ButtonPTROCancelar.Visible = true;
                    ButtonPTROAgregar.Visible = false;

                }
            }
        }

        protected void ButtonPTROGrabar_Click(object sender, EventArgs e)
        {
            int vIdFlujo = 0;
            int vIdCotizacion = 0;

            vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
            vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);

            CotizacionICRL.TipoPerdidaTotalRoboTraer vTipoPerdidaTotalRoboTraer;
            vTipoPerdidaTotalRoboTraer = CotizacionICRL.PerdidaTotalRoboTraer(vIdFlujo, vIdCotizacion);

            if (vTipoPerdidaTotalRoboTraer.Correcto)
            {
                var vFilaTabla = vTipoPerdidaTotalRoboTraer.PerdidasTotalesRobos.FirstOrDefault();
                int vIndice = 0;
                vIndice = int.Parse(TextBoxPTROIndice.Text);
                if (1 == vIndice)
                {
                    vFilaTabla.duenio_nombres_1 = TextBoxPTRONombres.Text;
                    vFilaTabla.duenio_documento_1 = TextBoxPTRODocumentoId.Text;
                    vFilaTabla.duenio_monto_1 = double.Parse(TextBoxPTROMontoPago.Text);
                    vFilaTabla.duenio_descripcion_1 = TextBoxPTRODescripcion.Text;
                }

                if (2 == vIndice)
                {
                    vFilaTabla.duenio_nombres_2 = TextBoxPTRONombres.Text;
                    vFilaTabla.duenio_documento_2 = TextBoxPTRODocumentoId.Text;
                    vFilaTabla.duenio_monto_2 = double.Parse(TextBoxPTROMontoPago.Text);
                    vFilaTabla.duenio_descripcion_2 = TextBoxPTRODescripcion.Text;
                }

                if (3 == vIndice)
                {
                    vFilaTabla.duenio_nombres_3 = TextBoxPTRONombres.Text;
                    vFilaTabla.duenio_documento_3 = TextBoxPTRODocumentoId.Text;
                    vFilaTabla.duenio_monto_3 = double.Parse(TextBoxPTROMontoPago.Text);
                    vFilaTabla.duenio_descripcion_3 = TextBoxPTRODescripcion.Text;
                }

                if (4 == vIndice)
                {
                    vFilaTabla.duenio_nombres_4 = TextBoxPTRONombres.Text;
                    vFilaTabla.duenio_documento_4 = TextBoxPTRODocumentoId.Text;
                    vFilaTabla.duenio_monto_4 = double.Parse(TextBoxPTROMontoPago.Text);
                    vFilaTabla.duenio_descripcion_4 = TextBoxPTRODescripcion.Text;
                }

                if (5 == vIndice)
                {
                    vFilaTabla.duenio_nombres_5 = TextBoxPTRONombres.Text;
                    vFilaTabla.duenio_documento_5 = TextBoxPTRODocumentoId.Text;
                    vFilaTabla.duenio_monto_5 = double.Parse(TextBoxPTROMontoPago.Text);
                    vFilaTabla.duenio_descripcion_5 = TextBoxPTRODescripcion.Text;
                }

                bool vResultado = false;
                vResultado = CotizacionICRL.PerdidaTotalRoboModificar(vFilaTabla);
                if (vResultado)
                {
                    if (CheckBoxPTROTotPagado.Checked)
                    {
                        ButtonPTROAgregar.Visible = true;
                    }
                    else
                    {
                        ButtonPTROAgregar.Visible = false;
                    }
                    ButtonPTROGrabar.Visible = false;
                    ButtonPTROCancelar.Visible = false;
                    PLimpiarBeneficiario();
                    PHabilitarBeneficiarioOtro(false);
                }

                PCargarGrillaBeneficiarios(vIdFlujo, vIdCotizacion);
            }
        }

        protected void ButtonPTROCancelar_Click(object sender, EventArgs e)
        {
            int vIdFlujo = 0;
            int vIdCotizacion = 0;

            vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
            vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);
            ButtonPTROGrabar.Visible = false;
            ButtonPTROCancelar.Visible = false;

            if (CheckBoxPTROTotPagado.Checked)
            {
                ButtonPTROAgregar.Visible = true;
            }

            PLimpiarBeneficiario();
            PHabilitarBeneficiarioOtro(false);
            ButtonPTROGrabar.Visible = false;
            ButtonPTROCancelar.Visible = false;
            PCargarGrillaBeneficiarios(vIdFlujo, vIdCotizacion);
        }

        #region Perdida Total Robo - Referencias

        protected void PLimpiarReferencias()
        {
            CheckboxReferUtilizada.Checked = false;
            TextBoxReferMedioCoti.Text = string.Empty;
            TextBoxReferDescripcion.Text = string.Empty;
            TextBoxReferMontoCoti.Text = string.Empty;
        }

        protected void PHabilitarReferencias(bool pEstado)
        {
            CheckboxReferUtilizada.Enabled = pEstado;
            TextBoxReferMedioCoti.Enabled = pEstado;
            TextBoxReferDescripcion.Enabled = pEstado;
            TextBoxReferMontoCoti.Enabled = pEstado;
        }

        protected void PCargarGrillaReferencias(int pIdFlujo, int pIdCotizacion, long pIdItem)
        {

            //DataTable dtReferencia;
            dtReferencia = CotizacionICRL.PerdidaTotalTraerReferencias(pIdFlujo, pIdCotizacion, pIdItem);

            GridViewPTROReferencias.DataSource = dtReferencia;
            GridViewPTROReferencias.DataBind();

        }

        protected void GridViewPTROReferencias_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vTextoTemporal = string.Empty;
            int vIndice = 0;

            //Leer Registro de la grilla y cargar los valores a la ventana.
            vIndice = int.Parse(GridViewPTROReferencias.SelectedRow.Cells[1].Text);

            TextBoxReferIndice.Text = GridViewPTROReferencias.SelectedRow.Cells[1].Text;
            CheckboxReferUtilizada.Checked = (GridViewPTROReferencias.SelectedRow.Cells[2].Controls[1] as CheckBox).Checked;
            TextBoxReferMedioCoti.Text = GridViewPTROReferencias.SelectedRow.Cells[3].Text;
            TextBoxReferDescripcion.Text = GridViewPTROReferencias.SelectedRow.Cells[4].Text;
            TextBoxReferMontoCoti.Text = GridViewPTROReferencias.SelectedRow.Cells[5].Text;

            PHabilitarReferencias(true);

            ButtonReferAgregar.Visible = false;
            ButtonReferGrabar.Visible = true;
            ButtonReferCancelar.Visible = true;


            ////dtReferencia.Rows[vIndice].ItemArray[2]
            //dtReferencia = CotizacionICRL.PerdidaTotalTraerReferencias(37, 6, 1);
            //dtReferencia.Rows[vIndice-1][1] = "PAGINA SIETE";
            //bool vResultado = false;

            //vResultado =   CotizacionICRL.PerdidaTotalActualizarReferencias(37, 6, 1, dtReferencia);


            //TextBoxPTROIndice.Text = GridViewPTRODueniosaPagar.SelectedRow.Cells[1].Text;
            //TextBoxPTRONombres.Text = GridViewPTRODueniosaPagar.SelectedRow.Cells[2].Text;
            //vTextoTemporal = GridViewPTRODueniosaPagar.SelectedRow.Cells[3].Text;
            //vTextoTemporal = vTextoTemporal.Replace("&nbsp;", string.Empty);
            //TextBoxPTRODocumentoId.Text = vTextoTemporal;
            //TextBoxPTROMontoPago.Text = GridViewPTRODueniosaPagar.SelectedRow.Cells[4].Text;
            //TextBoxPTRODescripcion.Text = GridViewPTRODueniosaPagar.SelectedRow.Cells[5].Text;
            //if (1 == vIndice)
            //{
            //    PHabilitarBeneficiarioPrincipal(true);
            //}
            //else
            //{
            //    PHabilitarBeneficiarioOtro(true);
            //}
            ////PModificarPersonaDet(true);



        }

        protected void GridViewPTROReferencias_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            bool vResultado = false;
            int vIdFlujo = 0;
            int vIdCotizacion = 0;
            long vIdItem = 0;
            int vIndice = 0;

            vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
            vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);

            string vTextoSecuencial = string.Empty;
            int vIndex = 0;

            vIndex = Convert.ToInt32(e.RowIndex);
            vIndice = Convert.ToInt32(GridViewPTROReferencias.DataKeys[vIndex].Value);

            vIdItem = FValidaTienePTRobo(vIdFlujo, vIdCotizacion);
            if (vIdItem > 0)
            {
                dtReferencia = CotizacionICRL.PerdidaTotalTraerReferencias(vIdFlujo, vIdCotizacion, vIdItem);
                //se borra una fila del dataset
                dtReferencia.Rows[vIndice - 1].Delete();
                vResultado = CotizacionICRL.PerdidaTotalActualizarReferencias(vIdFlujo, vIdCotizacion, vIdItem, dtReferencia);
                if (vResultado)
                {
                    LabelDatosReferMsj.Text = "Referencia borrada exitosamente";
                }
                else
                {
                    LabelDatosReferMsj.Text = "El Registro no pudo ser borrado";
                }
            }
            PCargarGrillaReferencias(vIdFlujo, vIdCotizacion, vIdItem);
        }

        protected void ButtonReferAgregar_Click(object sender, EventArgs e)
        {
            int vIdFlujo = 0;
            int vIdCotizacion = 0;
            long vIdItem = 0;
            int vIndice = 0;

            vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
            vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);


            vIdItem = FValidaTienePTRobo(vIdFlujo, vIdCotizacion);
            if (vIdItem > 0)
            {
                dtReferencia = CotizacionICRL.PerdidaTotalTraerReferencias(vIdFlujo, vIdCotizacion, vIdItem);

                if (dtReferencia.Rows.Count < 6)
                {
                    TextBoxReferIndice.Text = vIndice.ToString();
                    PHabilitarReferencias(true);
                    PLimpiarReferencias();
                    ButtonReferAgregar.Visible = false;
                    ButtonReferGrabar.Visible = true;
                    ButtonReferCancelar.Visible = true;
                }
            }

            PCargarGrillaReferencias(vIdFlujo, vIdCotizacion, vIdItem);
        }

        protected void ButtonReferGrabar_Click(object sender, EventArgs e)
        {
            int vIdFlujo = 0;
            int vIdCotizacion = 0;
            long vIdItem = 0;
            int vIndice = 0;

            vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
            vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);

            vIdItem = FValidaTienePTRobo(vIdFlujo, vIdCotizacion);
            if (vIdItem > 0)
            {
                vIndice = int.Parse(TextBoxReferIndice.Text);
                dtReferencia = CotizacionICRL.PerdidaTotalTraerReferencias(vIdFlujo, vIdCotizacion, vIdItem);
                if (0 == vIndice)
                {
                    //se agrega una fila al dataset
                    DataRow dtFilaRef = dtReferencia.NewRow();
                    dtFilaRef["usada"] = CheckboxReferUtilizada.Checked;
                    dtFilaRef["medios"] = TextBoxReferMedioCoti.Text;
                    dtFilaRef["descripcion"] = TextBoxReferDescripcion.Text;
                    dtFilaRef["monto"] = double.Parse(TextBoxReferMontoCoti.Text);

                    dtReferencia.Rows.Add(dtFilaRef);
                }
                else
                {
                    //se modifica una fila del dataset
                    //Referencia Utilizada
                    dtReferencia.Rows[vIndice - 1][0] = CheckboxReferUtilizada.Checked;
                    //Medio Cotizado
                    dtReferencia.Rows[vIndice - 1][1] = TextBoxReferMedioCoti.Text;
                    //Descripción
                    dtReferencia.Rows[vIndice - 1][2] = TextBoxReferDescripcion.Text;
                    //Monto
                    dtReferencia.Rows[vIndice - 1][3] = double.Parse(TextBoxReferMontoCoti.Text);
                }

                bool vResultado = false;
                vResultado = CotizacionICRL.PerdidaTotalActualizarReferencias(vIdFlujo, vIdCotizacion, vIdItem, dtReferencia);
                if (vResultado)
                {
                    LabelDatosReferMsj.Text = "Registro Procesado exitosamente";
                }
                else
                {
                    LabelDatosReferMsj.Text = "El Registro no pudo ser actualizado";
                }

            }

            PHabilitarReferencias(false);
            PLimpiarReferencias();
            ButtonReferAgregar.Visible = true;
            ButtonReferGrabar.Visible = false;
            ButtonReferCancelar.Visible = false;
            PCargarGrillaReferencias(vIdFlujo, vIdCotizacion, vIdItem);

        }

        protected void ButtonReferCancelar_Click(object sender, EventArgs e)
        {
            PHabilitarReferencias(false);
            PLimpiarReferencias();
            ButtonReferAgregar.Visible = true;
            ButtonReferGrabar.Visible = false;
            ButtonReferCancelar.Visible = false;
        }

        #endregion


    }
}