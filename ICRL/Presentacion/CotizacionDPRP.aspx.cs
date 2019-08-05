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
    public partial class CotizacionDPRP : System.Web.UI.Page
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

        private int FlTraeDatosDPReparacion(int pIdCotizacion)
        {
            int vResultado = 0;

            using (LBCDesaEntities db = new LBCDesaEntities())
            {
                var vLst = from c in db.Cotizacion
                           join cotrepa in db.CotiReparacion on c.idCotizacion equals cotrepa.idCotizacion
                           join n in db.Nomenclador on cotrepa.idItem equals n.codigo
                           where c.idCotizacion == pIdCotizacion && n.categoriaNomenclador == "Item"
                           select new
                           {
                               n.descripcion,
                               cotrepa.chaperio,
                               cotrepa.reparacionPrevia,
                               cotrepa.mecanico,
                               cotrepa.moneda,
                               cotrepa.precioCotizado,
                               cotrepa.descFijoPorcentaje,
                               cotrepa.montoDescuento,
                               cotrepa.precioFinal,
                               cotrepa.proveedor
                           };

                GridViewReparaciones.DataSource = vLst.ToList();
                GridViewReparaciones.DataBind();

            }

            return vResultado;
        }

        private int FlTraeDatosDPRepuesto(int pIdCotizacion)
        {
            int vResultado = 0;

            using (LBCDesaEntities db = new LBCDesaEntities())
            {
                var vLst = from c in db.Cotizacion
                           join cotrepu in db.CotiRepuesto on c.idCotizacion equals cotrepu.idCotizacion
                           join n in db.Nomenclador on cotrepu.idItem equals n.codigo
                           where c.idCotizacion == pIdCotizacion && n.categoriaNomenclador == "Item"
                           select new
                           {
                               n.descripcion,
                               cotrepu.pintura,
                               cotrepu.instalacion,
                               cotrepu.moneda,
                               cotrepu.precioCotizado,
                               cotrepu.descFijoPorcentaje,
                               cotrepu.montoDescuento,
                               cotrepu.precioFinal,
                               cotrepu.proveedor
                           };

                GridViewRepuestos.DataSource = vLst.ToList();
                GridViewRepuestos.DataBind();

            }

            return vResultado;
        }

    }
}