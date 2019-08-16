using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ICRL.BD;
using ICRL.ModeloDB;
using Microsoft.Reporting.WebForms;
using System.Globalization;

namespace IRCL.Presentacion
{
    public partial class GestionCotizacion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime vFechaIni = DateTime.Now;
            DateTime vFechaFin = DateTime.Now;
            if (null == TextBoxFechaIni_CalendarExtender.SelectedDate)
            {
                vFechaIni = new DateTime(vFechaIni.Year, vFechaIni.Month, 1, 0, 0, 0);
                TextBoxFechaIni_CalendarExtender.SelectedDate = vFechaIni;
                TextBoxFechaIni.Text = vFechaIni.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            if (null == TextBoxFechaFin_CalendarExtender.SelectedDate)
            {
                vFechaFin = vFechaIni.AddDays(30);
                TextBoxFechaFin_CalendarExtender.SelectedDate = vFechaFin;
                TextBoxFechaFin.Text = vFechaFin.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }


            Label4.Text = string.Empty;

            if (!IsPostBack)
            {
                PBusquedaInspecciones();
            }
        }

        protected void ButtonBuscarFlujo_Click(object sender, EventArgs e)
        {
            PBusquedaInspecciones();
        }

        protected void ButtonCreaCotizacion_Click(object sender, EventArgs e)
        {
            Label4.Text = string.Empty;
            TextBoxPlaca.Text = string.Empty;
            //if (string.Empty == TextBoxNroFlujo.Text)
            //{
            //    TextBoxPopupSiNo.Text = "No se tiene Flujo Asociado, se creara un Flujo Temporal";
            //    Session["PopupModalSiNo"] = 1;
            //    this.ModalPopupSiNo.Show();
            //}
            //else
            //{
            //    int vResultadoFlujo = 0;
            //    vResultadoFlujo = PTraeFlujoOnBase(TextBoxNroFlujo.Text);
            //    if (1 == vResultadoFlujo)
            //    {
            //        PCreaInspeccion();
            //    }
            //    else
            //    {
            //        Label4.Text = "NO existe el Flujo " + TextBoxNroFlujo.Text + " en OnBase, verifique";
            //    }
            //}
        }

        protected void GridViewMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.backgroundColor='aquamarine';";
                e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='white';";

                DateTime vFechaIni = DateTime.ParseExact(TextBoxFechaIni.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                TextBoxFechaIni_CalendarExtender.SelectedDate = vFechaIni;

                DateTime vFechaFin = DateTime.ParseExact(TextBoxFechaFin.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                TextBoxFechaFin_CalendarExtender.SelectedDate = vFechaFin;

                string vIdFlujo = string.Empty;
                int vIdInspeccion = 0;

                vIdFlujo = e.Row.Cells[2].Text;
                vIdInspeccion = int.Parse(vIdFlujo);

                AccesoDatos vAccesoDatos = new AccesoDatos();
                var gvInspecciones = (GridView)e.Row.FindControl("gvInspecciones");

                vFechaFin = vFechaFin.AddDays(1);
                using (LBCDesaEntities db = new LBCDesaEntities())
                {

                    var vLst = (from c in db.Cotizacion
                                join cf in db.CotizacionFlujo on c.idFlujo equals cf.idFlujo
                                where (c.idFlujo == vIdInspeccion)
                                && (c.fechaCreacion >= vFechaIni && c.fechaCreacion <= vFechaFin)
                                && (c.tipoCobertura == (int)AccesoDatos.TipoInspeccion.DaniosPropios)
                                orderby c.idInspeccion
                                select new
                                {
                                    c.idCotizacion,
                                    tipoCobertura = "Daños Propios",
                                    secuencialOrden = "DP - Pendiente",
                                    nombreProveedor = "N/A",
                                    correlativoInspeccion = c.correlativo,
                                    c.fechaCreacion,
                                    sumaCosto = 0,
                                    descEstado = "Coti.Pendiente"
                                }).Union
                                (from c in db.Cotizacion
                                 join cf in db.CotizacionFlujo on c.idFlujo equals cf.idFlujo
                                 where (c.idFlujo == vIdInspeccion)
                                 && (c.fechaCreacion >= vFechaIni && c.fechaCreacion <= vFechaFin)
                                 && (c.tipoCobertura == (int)AccesoDatos.TipoInspeccion.RoboParcial)
                                 orderby c.idInspeccion
                                 select new
                                 {
                                     c.idCotizacion,
                                     tipoCobertura = "Robo Parcial",
                                     secuencialOrden = "RP - Pendiente",
                                     nombreProveedor = "N/A",
                                     correlativoInspeccion = c.correlativo,
                                     c.fechaCreacion,
                                     sumaCosto = 0,
                                     descEstado = "Coti.Pendiente"
                                 }).Union
                                (from c in db.Cotizacion
                                 join cf in db.CotizacionFlujo on c.idFlujo equals cf.idFlujo
                                 where (c.idFlujo == vIdInspeccion)
                                 && (c.fechaCreacion >= vFechaIni && c.fechaCreacion <= vFechaFin)
                                 && (c.tipoCobertura == (int)AccesoDatos.TipoInspeccion.RCVEhicular)
                                 orderby c.idInspeccion
                                 select new
                                 {
                                     c.idCotizacion,
                                     tipoCobertura = "RC Vehicular",
                                     secuencialOrden = "RCV - Pendiente",
                                     nombreProveedor = "N/A",
                                     correlativoInspeccion = c.correlativo,
                                     c.fechaCreacion,
                                     sumaCosto = 0,
                                     descEstado = "Coti.Pendiente"
                                 }).Union
                                (from c in db.Cotizacion
                                 join cf in db.CotizacionFlujo on c.idFlujo equals cf.idFlujo
                                 where (c.idFlujo == vIdInspeccion)
                                 && (c.fechaCreacion >= vFechaIni && c.fechaCreacion <= vFechaFin)
                                 && (c.tipoCobertura == (int)AccesoDatos.TipoInspeccion.RCPersonas)
                                 orderby c.idInspeccion
                                 select new
                                 {
                                     c.idCotizacion,
                                     tipoCobertura = "RC Personas",
                                     secuencialOrden = "RCPer - Pendiente",
                                     nombreProveedor = "N/A",
                                     correlativoInspeccion = c.correlativo,
                                     c.fechaCreacion,
                                     sumaCosto = 0,
                                     descEstado = "Coti.Pendiente"
                                 }).Union
                                (from c in db.Cotizacion
                                 join cf in db.CotizacionFlujo on c.idFlujo equals cf.idFlujo
                                 where (c.idFlujo == vIdInspeccion)
                                 && (c.fechaCreacion >= vFechaIni && c.fechaCreacion <= vFechaFin)
                                 && (c.tipoCobertura == (int)AccesoDatos.TipoInspeccion.RCObjetos)
                                 orderby c.idInspeccion
                                 select new
                                 {
                                     c.idCotizacion,
                                     tipoCobertura = "RC Objetos",
                                     secuencialOrden = "RCObj - Pendiente",
                                     nombreProveedor = "N/A",
                                     correlativoInspeccion = c.correlativo,
                                     c.fechaCreacion,
                                     sumaCosto = 0,
                                     descEstado = "Coti.Pendiente"
                                 }).Union
                                (from c in db.Cotizacion
                                 join cf in db.CotizacionFlujo on c.idFlujo equals cf.idFlujo
                                 where (c.idFlujo == vIdInspeccion)
                                 && (c.fechaCreacion >= vFechaIni && c.fechaCreacion <= vFechaFin)
                                 && (c.tipoCobertura == (int)AccesoDatos.TipoInspeccion.PerdidaTotalRobo)
                                 orderby c.idInspeccion
                                 select new
                                 {
                                     c.idCotizacion,
                                     tipoCobertura = "PT Robo",
                                     secuencialOrden = "PT Robo - Pendiente",
                                     nombreProveedor = "N/A",
                                     correlativoInspeccion = c.correlativo,
                                     c.fechaCreacion,
                                     sumaCosto = 0,
                                     descEstado = "Coti.Pendiente"
                                 });

                    gvInspecciones.DataSource = vLst.ToList();
                    gvInspecciones.DataBind();
                }
            }
        }

        protected void GridViewMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //se realiza una búsqueda por prioridad
            //primero por flujo, después por placa y finalmente por fecha
            //validar los campos de busqueda
            DateTime vFechaIni = DateTime.Now;
            DateTime vFechaFin = DateTime.Now;

            string vNroFlujo = null;
            string vPlaca = null;

            if (TextBoxNroFlujo.Text != string.Empty)
                vNroFlujo = TextBoxNroFlujo.Text;

            if (TextBoxPlaca.Text != string.Empty)
                vPlaca = TextBoxPlaca.Text;

            if (null == TextBoxFechaIni_CalendarExtender.SelectedDate)
            {
                vFechaIni = new DateTime(vFechaIni.Year, vFechaIni.Month, 1, 0, 0, 0);
                TextBoxFechaIni_CalendarExtender.SelectedDate = vFechaIni;
            }
            else
            {
                vFechaIni = DateTime.ParseExact(TextBoxFechaIni.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            if (null == TextBoxFechaFin_CalendarExtender.SelectedDate)
            {
                vFechaFin = vFechaIni.AddDays(30);
                TextBoxFechaFin_CalendarExtender.SelectedDate = vFechaFin;
            }
            else
            {
                vFechaFin = DateTime.ParseExact(TextBoxFechaFin.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            vFechaFin = vFechaFin.AddDays(1);
            using (LBCDesaEntities db = new LBCDesaEntities())
            {
                var vLst = from c in db.Cotizacion
                           join u in db.Usuario on c.idUsuario equals u.idUsuario
                           join f in db.Flujo on c.idFlujo equals f.idFlujo
                           where (vNroFlujo == null || f.flujoOnBase == vNroFlujo)
                           && (vPlaca == null || f.placaVehiculo == vPlaca)
                           && (c.fechaCreacion >= vFechaIni && c.fechaCreacion <= vFechaFin)
                           orderby f.flujoOnBase
                           select new
                           {
                               f.idFlujo,
                               f.flujoOnBase,
                               f.nombreAsegurado,
                               f.numeroPoliza,
                               f.placaVehiculo,
                               c.fechaCreacion,
                               descEstado = "Pendiente"
                           };

                GridViewMaster.DataSource = vLst.Distinct().ToList();
                GridViewMaster.PageIndex = e.NewPageIndex;
                GridViewMaster.DataBind();
            }
        }

        public string MyNewRowCot(object pFlujoOnBase)
        {
            return String.Format(@"</td></tr><tr id ='tr{0}' class='collapsed-row'>
                                <td></td><td colspan='100' style='padding:0px; margin:0px;'>", pFlujoOnBase);
        }

        protected void GridViewgvInspecciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vFilaFlujo = string.Empty;
            string vCobertura = string.Empty;
            int vIdFlujo = 0;

            var gvInspecciones = (GridView)sender;
            vFilaFlujo = gvInspecciones.SelectedRow.Cells[0].Text;
            vCobertura = gvInspecciones.SelectedRow.Cells[1].Text;
            vCobertura = vCobertura.Replace("&#241;", "ñ");


            int vIdCotizacion = int.Parse(vFilaFlujo);
            AccesoDatos vAccesoDatos = new AccesoDatos();
            vIdFlujo = vAccesoDatos.FTraeIdFlujoCotizacion(vIdCotizacion);

            Session["NumFlujo"] = vIdFlujo;

            switch (vCobertura)
            {
                case "Daños Propios":
                    Response.Redirect("~/Presentacion/CotizacionDPRP.aspx?nroCoti=" + vIdCotizacion.ToString());
                    break;
                case "RC Objetos":
                    Response.Redirect("~/Presentacion/CotizacionRCObj.aspx?nroCoti=" + vIdCotizacion.ToString());
                    break;
                case "RC Personas":
                    Response.Redirect("~/Presentacion/CotizacionRCPer.aspx?nroCoti=" + vIdCotizacion.ToString());
                    break;
                case "Robo Parcial":
                    Response.Redirect("~/Presentacion/CotizacionRP.aspx?nroCoti=" + vIdCotizacion.ToString());
                    break;
                case "RC Vehicular":
                    Response.Redirect("~/Presentacion/CotizacionRCVehicular.aspx?nroCoti=" + vIdCotizacion.ToString());
                    break;
                case "PT Robo":
                    Response.Redirect("~/Presentacion/CotizacionPerTotRobo.aspx?nroCoti=" + vIdCotizacion.ToString());
                    break;
                default:
                    break;
            }

        }

        #region Grilla Maestra

        protected int PBusquedaInspecciones()
        {
            //se realiza una búsqueda por prioridad
            //primero por flujo, después por placa y finalmente por fecha
            //validar los campos de busqueda
            int vResul = 1;
            DateTime vFechaIni = DateTime.Now;
            DateTime vFechaFin = DateTime.Now;

            string vNroFlujo = null;
            string vPlaca = null;

            if (TextBoxNroFlujo.Text != string.Empty)
                vNroFlujo = TextBoxNroFlujo.Text;

            if (TextBoxPlaca.Text != string.Empty)
                vPlaca = TextBoxPlaca.Text;

            if (null == TextBoxFechaIni_CalendarExtender.SelectedDate)
            {
                vFechaIni = new DateTime(vFechaIni.Year, vFechaIni.Month, 1, 0, 0, 0);
                TextBoxFechaIni_CalendarExtender.SelectedDate = vFechaIni;
            }
            else
            {
                vFechaIni = DateTime.ParseExact(TextBoxFechaIni.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            if (null == TextBoxFechaFin_CalendarExtender.SelectedDate)
            {
                vFechaFin = vFechaIni.AddDays(30);
                TextBoxFechaFin_CalendarExtender.SelectedDate = vFechaFin;
            }
            else
            {
                vFechaFin = DateTime.ParseExact(TextBoxFechaFin.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            vFechaFin = vFechaFin.AddDays(1);
            using (LBCDesaEntities db = new LBCDesaEntities())
            {
                var vLst = from c in db.Cotizacion
                           join u in db.Usuario on c.idUsuario equals u.idUsuario
                           join f in db.Flujo on c.idFlujo equals f.idFlujo
                           where (vNroFlujo == null || f.flujoOnBase == vNroFlujo)
                           && (vPlaca == null || f.placaVehiculo == vPlaca)
                           && (c.fechaCreacion >= vFechaIni && c.fechaCreacion <= vFechaFin)
                           orderby f.flujoOnBase
                           select new
                           {
                               f.idFlujo,
                               f.flujoOnBase,
                               f.nombreAsegurado,
                               f.numeroPoliza,
                               f.placaVehiculo,
                               c.fechaCreacion,
                               descEstado = "Pendiente"
                           };

                GridViewMaster.DataSource = vLst.Distinct().ToList();
                GridViewMaster.DataBind();
            }

            return vResul;

        }
        #endregion
    }
}