using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ICRL.ModeloDB;
using ICRL.BD;

namespace ICRL.Presentacion
{
    public partial class RCVehicularPopup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["PopSecuencial"] != null)
                {
                    TextBoxSecuencialPop.Text = Session["PopSecuencial"].ToString();
                }

                if (Session["PopIdInspeccion"] != null)
                {
                    TextBoxIdInspeccionPop.Text = Session["PopIdInspeccion"].ToString();
                }

                FlTraeNomenChaperio();
                FlTraeNomenRepPrevia();
                FlTraeNomenItemRCV01();
                int vSecuencial = 3;
                //vSecuencial = int.Parse(TextBoxSecuencialPop.Text);
                FlTraeDatosRCVehicularDet(vSecuencial);

            }

        }


        #region RC Inspeccion Vehicular Detalle

        protected void PLimpiaSeccionRCV01Det()
        {
            DropDownListItemRCV01.SelectedIndex = 0;
            DropDownListItemRCV01.Enabled = true;
            TextBoxCompraRCV01.Text = string.Empty;
            CheckBoxInstalacionRCV01.Checked = false;
            CheckBoxPinturaRCV01.Checked = false;
            CheckBoxMecanicoRCV01.Checked = false;
            DropDownListChaperioRCV01.SelectedIndex = 0;
            DropDownListRepPreviaRCV01.SelectedIndex = 0;
            TextBoxObservacionesRCV01.Text = string.Empty;
            ButtonGrabarRCV01.Enabled = false;
            ButtonBorrarRCV01.Enabled = false;
            ButtonNuevoRCV01.Enabled = true;
        }

        protected void GridViewRCV01Det_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownListItemRCV01.Enabled = false;
            TextBoxIdItemRCV01.Text = string.Empty;
            TextBoxIdItemRCV01.Text = GridViewRCV01Det.SelectedRow.Cells[1].Text.Substring(0, 8);
            string vTextoItemRCV01 = GridViewRCV01Det.SelectedRow.Cells[2].Text;

            DropDownListItemRCV01.ClearSelection();
            DropDownListItemRCV01.Items.FindByValue(TextBoxIdItemRCV01.Text).Selected = true;

            TextBoxCompraRCV01.Text = GridViewRCV01Det.SelectedRow.Cells[3].Text;
            CheckBoxInstalacionRCV01.Checked = (GridViewRCV01Det.SelectedRow.Cells[4].Controls[0] as CheckBox).Checked;
            CheckBoxPinturaRCV01.Checked = (GridViewRCV01Det.SelectedRow.Cells[5].Controls[0] as CheckBox).Checked;
            CheckBoxMecanicoRCV01.Checked = (GridViewRCV01Det.SelectedRow.Cells[6].Controls[0] as CheckBox).Checked;

            string vTempoCadena = string.Empty;
            vTempoCadena = GridViewRCV01Det.SelectedRow.Cells[7].Text.Trim();
            DropDownListChaperioRCV01.ClearSelection();
            DropDownListChaperioRCV01.Items.FindByText(vTempoCadena).Selected = true;

            vTempoCadena = string.Empty;
            vTempoCadena = GridViewRCV01Det.SelectedRow.Cells[8].Text.Trim();
            DropDownListRepPreviaRCV01.ClearSelection();
            DropDownListRepPreviaRCV01.Items.FindByText(vTempoCadena).Selected = true;
            TextBoxObservacionesRCV01.Text = GridViewRCV01Det.SelectedRow.Cells[9].Text;
            ButtonNuevoRCV01.Enabled = false;
            ButtonGrabarRCV01.Enabled = true;
            ButtonBorrarRCV01.Enabled = true;
        }

        protected void GridViewRCV01Det_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.backgroundColor='aquamarine';";
                e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='white';";
                e.Row.ToolTip = "Haz clic en la primera columna para seleccionar la fila.";
            }
        }

        protected void ButtonNuevoRCV01_Click(object sender, EventArgs e)
        {
            AccesoDatos vAccesodatos = new AccesoDatos();
            InspeccionRCVehicularDet vInspRCVDet = new InspeccionRCVehicularDet();

            vInspRCVDet.idItem = DropDownListItemRCV01.SelectedValue;
            vInspRCVDet.compra = TextBoxCompraRCV01.Text;
            vInspRCVDet.instalacion = CheckBoxInstalacionRCV01.Checked;
            vInspRCVDet.pintura = CheckBoxPinturaRCV01.Checked;
            vInspRCVDet.mecanico = CheckBoxMecanicoRCV01.Checked;
            vInspRCVDet.chaperio = DropDownListChaperioRCV01.SelectedValue;
            vInspRCVDet.reparacionPrevia = DropDownListRepPreviaRCV01.SelectedValue;
            vInspRCVDet.observaciones = TextBoxObservacionesRCV01.Text; ;

            int vResultado = vAccesodatos.FGrabaInspRCV01DetICRL(vInspRCVDet);

            if (vResultado > 0)
            {
                int vResul = FlTraeDatosRCVehicularDet(int.Parse(TextBoxSecuencialPop.Text));
                PLimpiaSeccionRCV01Det();
            }
        }

        protected void ButtonGrabarRCV01_Click(object sender, EventArgs e)
        {
            AccesoDatos vAccesodatos = new AccesoDatos();
            InspeccionRCVehicularDet vInspRCVDet = new InspeccionRCVehicularDet();

            vInspRCVDet.secuencial = int.Parse(TextBoxSecuencialPop.Text);
            vInspRCVDet.idItem = TextBoxIdItemRCV01.Text;
            vInspRCVDet.compra = TextBoxCompraRCV01.Text;
            vInspRCVDet.instalacion = CheckBoxInstalacionRCV01.Checked;
            vInspRCVDet.pintura = CheckBoxPinturaRCV01.Checked;
            vInspRCVDet.mecanico = CheckBoxMecanicoRCV01.Checked;
            vInspRCVDet.chaperio = DropDownListChaperioRCV01.SelectedValue;
            vInspRCVDet.reparacionPrevia = DropDownListRepPreviaRCV01.SelectedValue;
            vInspRCVDet.observaciones = TextBoxObservacionesRCV01.Text; ;

            int vResultado = vAccesodatos.FActualizaInspRCV01DetICRL(vInspRCVDet);

            if (vResultado > 0)
            {
                int vResul = FlTraeDatosRCVehicularDet(int.Parse(TextBoxSecuencialPop.Text));
                PLimpiaSeccionRCV01Det();
            }
        }

        protected void ButtonBorrarRCV01_Click(object sender, EventArgs e)
        {
            AccesoDatos vAccesodatos = new AccesoDatos();
            InspeccionRCVehicularDet vInspRCVDet = new InspeccionRCVehicularDet();

            vInspRCVDet.secuencial = int.Parse(TextBoxSecuencialPop.Text);
            vInspRCVDet.idItem = TextBoxIdItemRCV01.Text;

            int vResultado = vAccesodatos.FBorrarInspRCV01DetICRL(vInspRCVDet);

            if (vResultado > 0)
            {
                int vResul = FlTraeDatosRCVehicularDet(int.Parse(TextBoxSecuencialPop.Text));
                PLimpiaSeccionRCV01Det();
            }
        }

        private int FlTraeDatosRCVehicularDet(int pSecuencial)
        {
            int vResultado = 0;

            using (LBCDesaEntities db = new LBCDesaEntities())
            {
                var vLst = from ircv in db.InspRCVehicular
                           join ircvdet in db.InspRCVehicularDetalle on ircv.secuencial equals ircvdet.secuencial
                           join n in db.Nomenclador on ircvdet.idItem equals n.codigo
                           where ircv.idInspeccion == pSecuencial && n.categoriaNomenclador == "Item"
                           select new
                           {
                               ircvdet.idItem,
                               n.descripcion,
                               ircvdet.compra,
                               ircvdet.instalacion,
                               ircvdet.pintura,
                               ircvdet.mecanico,
                               ircvdet.chaperio,
                               ircvdet.reparacionPrevia,
                               ircvdet.observaciones,
                           };

                GridViewRCV01Det.DataSource = vLst.ToList();
                GridViewRCV01Det.DataBind();

            }

            return vResultado;
        }

        //private void ValidaRoboParcial(int pIdInspeccion)
        //{
        //    AccesoDatos vAccesodatos = new AccesoDatos();

        //    bool vSeleccionado = false;
        //    int vResul = 0;
        //    vSeleccionado = vAccesodatos.FInspeccionTieneRCV01ICRL(pIdInspeccion);

        //    if (vSeleccionado)
        //    {
        //        TabPanelRoboParcial.Enabled = true;
        //        TabPanelRoboParcial.Visible = true;
        //        CheckBoxRoboParcial.Checked = vSeleccionado;
        //        vResul = FlTraeDatosRoboParcial(pIdInspeccion);
        //    }
        //}

        private int FlTraeNomenChaperio()
        {
            int vResultado = 0;

            using (LBCDesaEntities db = new LBCDesaEntities())
            {
                var vLst = from n in db.Nomenclador
                           where n.categoriaNomenclador == "Nivel de Daño"
                           orderby n.descripcion
                           select new
                           {
                               n.codigo,
                               n.descripcion,
                           };

                DropDownListChaperioRCV01.DataValueField = "codigo";
                DropDownListChaperioRCV01.DataTextField = "descripcion";
                DropDownListChaperioRCV01.DataSource = vLst.ToList();
                DropDownListChaperioRCV01.DataBind();

            }

            return vResultado;
        }

        private int FlTraeNomenRepPrevia()
        {
            int vResultado = 0;

            using (LBCDesaEntities db = new LBCDesaEntities())
            {
                var vLst = from n in db.Nomenclador
                           where n.categoriaNomenclador == "Nivel de Daño"
                           orderby n.descripcion
                           select new
                           {
                               n.codigo,
                               n.descripcion,
                           };

                DropDownListRepPreviaRCV01.DataValueField = "codigo";
                DropDownListRepPreviaRCV01.DataTextField = "descripcion";
                DropDownListRepPreviaRCV01.DataSource = vLst.ToList();
                DropDownListRepPreviaRCV01.DataBind();

            }

            return vResultado;
        }



        private int FlTraeNomenItemRCV01()
        {
            int vResultado = 0;

            using (LBCDesaEntities db = new LBCDesaEntities())
            {
                var vLst = from n in db.Nomenclador
                           where n.categoriaNomenclador == "Item"
                           orderby n.descripcion
                           select new
                           {
                               n.codigo,
                               n.descripcion,
                           };

                DropDownListItemRCV01.DataValueField = "codigo";
                DropDownListItemRCV01.DataTextField = "descripcion";
                DropDownListItemRCV01.DataSource = vLst.ToList();
                DropDownListItemRCV01.DataBind();

            }

            return vResultado;
        }


        #endregion


    }
}