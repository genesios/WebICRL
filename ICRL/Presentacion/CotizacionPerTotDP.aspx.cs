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
using System.Text;
using Microsoft.Reporting.WebForms;

namespace ICRL.Presentacion
{
  public partial class CotizacionPerTotDP : System.Web.UI.Page
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
          FlTraeItemsNomencladorCajaPTDP();
          FlTraeItemsNomencladorCombustiblePTDP();

          PCreaDataTableBenef();

          vIdFlujo = int.Parse(TextBoxIdFlujo.Text);

          FlTraeDatosCotizacion(vIdCotizacion, vlNumFlujo);

          //Cargar Datos Objeto
          vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);

          long vIdItem = 0;
          vIdItem = FValidaTienePTDP(vIdFlujo, vIdCotizacion);
          if (0 == vIdItem)
          {
            //No existe registro se debe crear la base
            PCreaBeneficiarioPrincipal();
          }
          PCargarGrillaBeneficiarios(vIdFlujo, vIdCotizacion);
          vIdItem = FValidaTienePTDP(vIdFlujo, vIdCotizacion);
          PCargarGrillaReferencias(vIdFlujo, vIdCotizacion, vIdItem);
          PBloqueaDatosEspeciales(false);
          FlCargaDatosPerdidaTotalDP(vIdFlujo, vIdCotizacion, vIdItem);
          pCargaOrdenes(vIdFlujo, vIdCotizacion);
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

    long FValidaTienePTDP(int pIdFlujo, int pIdCotizacion)
    {
      long vResultado = 0;

      CotizacionICRL.TipoPerdidaTotalPropioTraer vTipoPerdidaTotalPropioTraer;
      vTipoPerdidaTotalPropioTraer = CotizacionICRL.PerdidaTotalPropioTraer(pIdFlujo, pIdCotizacion);

      if (vTipoPerdidaTotalPropioTraer.Correcto)
      {
        var vFilaTabla = vTipoPerdidaTotalPropioTraer.PerdidasTotalesPropios.FirstOrDefault();
        if (vFilaTabla != null)
        {
          vResultado = vFilaTabla.id_item;
        }
      }

      return vResultado;
    }

    #endregion

    #region Datos Especiales


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
      CotizacionICRL.TipoPerdidaTotalPropio vTipoPerdidaTotalPropio = new CotizacionICRL.TipoPerdidaTotalPropio();

      vTipoPerdidaTotalPropio.id_flujo = vIdFlujo;
      vTipoPerdidaTotalPropio.id_cotizacion = vIdCotizacion;
      vTipoPerdidaTotalPropio.condiciones_especiales = false;
      vTipoPerdidaTotalPropio.duenio_nombres_1 = TextBoxNombreAsegurado.Text;
      vTipoPerdidaTotalPropio.duenio_documento_1 = string.Empty;
      vTipoPerdidaTotalPropio.duenio_monto_1 = 0;
      vTipoPerdidaTotalPropio.duenio_descripcion_1 = "Pago Asegurado";

      vResultado = CotizacionICRL.PerdidaTotalPropioRegistrar(vTipoPerdidaTotalPropio);
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

    protected void CheckBoxPTDPTotPagado_CheckedChanged(object sender, EventArgs e)
    {
      if (CheckBoxPTDPTotPagado.Checked)
      {
        //el checkbox chequeado indica que no esta completamente pagado
        //hay mas de un beneficiario
        ButtonPTDPAgregar.Visible = true;
        ButtonPTDPGrabar.Visible = false;
        ButtonPTDPCancelar.Visible = false;
      }
      else
      {
        //el checkbox NO chequeado indica que esta completamente pagado
        //solo hay un beneficiario
        ButtonPTDPAgregar.Visible = false;
        ButtonPTDPGrabar.Visible = false;
        ButtonPTDPCancelar.Visible = false;
      }
    }


    #endregion

    #region GrillaDueniosBeneficiarios

    protected void PCargarGrillaBeneficiarios(int pIdFlujo, int pIdCotizacion)
    {
      CotizacionICRL.TipoPerdidaTotalPropioTraer vTipoPerdidaTotalPropioTraer;
      vTipoPerdidaTotalPropioTraer = CotizacionICRL.PerdidaTotalPropioTraer(pIdFlujo, pIdCotizacion);

      if (vTipoPerdidaTotalPropioTraer.Correcto)
      {
        var vFilaTabla = vTipoPerdidaTotalPropioTraer.PerdidasTotalesPropios.FirstOrDefault();
        if (vFilaTabla != null)
        {
          DataTable dsTemp = new DataTable();
          dsTemp = null;

          GridViewPTDPDueniosaPagar.DataSource = dsTemp;
          GridViewPTDPDueniosaPagar.DataBind();
          dtBeneficiario.Rows.Clear();

          TextBoxPTDPIdItem.Text = vFilaTabla.id_item.ToString();

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
          if (string.Empty != vFilaTabla.duenio_nombres_1)
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
          if (string.Empty != vFilaTabla.duenio_nombres_2)
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
          if (string.Empty != vFilaTabla.duenio_nombres_3)
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
          if (string.Empty != vFilaTabla.duenio_nombres_4)
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
          if (string.Empty != vFilaTabla.duenio_nombres_5)
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

          GridViewPTDPDueniosaPagar.DataSource = dtBeneficiario;
          GridViewPTDPDueniosaPagar.DataBind();
        }
      }
    }

    protected void PHabilitarBeneficiarioPrincipal(bool pEstado)
    {
      if (pEstado)
      {
        TextBoxPTDPNombres.Enabled = false;
        TextBoxPTDPDocumentoId.Enabled = true;
        TextBoxPTDPMontoPago.Enabled = true;
        TextBoxPTDPDescripcion.Enabled = false;
      }
      else
      {
        TextBoxPTDPNombres.Enabled = false;
        TextBoxPTDPDocumentoId.Enabled = false;
        TextBoxPTDPMontoPago.Enabled = false;
        TextBoxPTDPDescripcion.Enabled = false;
      }
    }

    protected void PHabilitarBeneficiarioOtro(bool pEstado)
    {
      TextBoxPTDPNombres.Enabled = pEstado;
      TextBoxPTDPDocumentoId.Enabled = pEstado;
      TextBoxPTDPMontoPago.Enabled = pEstado;
      TextBoxPTDPDescripcion.Enabled = pEstado;
    }

    protected void PLimpiarBeneficiario()
    {
      TextBoxPTDPNombres.Text = string.Empty;
      TextBoxPTDPDocumentoId.Text = string.Empty;
      TextBoxPTDPMontoPago.Text = string.Empty;
      TextBoxPTDPDescripcion.Text = string.Empty;
    }

    protected void GridViewPTDPDueniosaPagar_SelectedIndexChanged(object sender, EventArgs e)
    {
      string vTextoTemporal = string.Empty;
      int vIndice = 0;

      //Leer Registro de la grilla y cargar los valores a la ventana.
      vIndice = int.Parse(GridViewPTDPDueniosaPagar.SelectedRow.Cells[1].Text);
      TextBoxPTDPIndice.Text = GridViewPTDPDueniosaPagar.SelectedRow.Cells[1].Text;
      TextBoxPTDPNombres.Text = GridViewPTDPDueniosaPagar.SelectedRow.Cells[2].Text;
      vTextoTemporal = GridViewPTDPDueniosaPagar.SelectedRow.Cells[3].Text;
      vTextoTemporal = vTextoTemporal.Replace("&nbsp;", string.Empty);
      TextBoxPTDPDocumentoId.Text = vTextoTemporal;
      TextBoxPTDPMontoPago.Text = GridViewPTDPDueniosaPagar.SelectedRow.Cells[4].Text;
      TextBoxPTDPDescripcion.Text = GridViewPTDPDueniosaPagar.SelectedRow.Cells[5].Text;
      if (1 == vIndice)
      {
        PHabilitarBeneficiarioPrincipal(true);
      }
      else
      {
        PHabilitarBeneficiarioOtro(true);
      }
      //PModificarPersonaDet(true);
      ButtonPTDPAgregar.Visible = false;
      ButtonPTDPGrabar.Visible = true;
      ButtonPTDPCancelar.Visible = true;
    }

    protected void GridViewPTDPDueniosaPagar_RowDeleting(object sender, GridViewDeleteEventArgs e)
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
      vIndice = Convert.ToInt32(GridViewPTDPDueniosaPagar.DataKeys[vIndex].Value);
      vIdItem = long.Parse(TextBoxPTDPIdItem.Text);

      CotizacionICRL.TipoPerdidaTotalPropioTraer vTipoPerdidaTotalPropioTraer;
      vTipoPerdidaTotalPropioTraer = CotizacionICRL.PerdidaTotalPropioTraer(vIdFlujo, vIdCotizacion);

      if (vTipoPerdidaTotalPropioTraer.Correcto)
      {
        var vFilaTabla = vTipoPerdidaTotalPropioTraer.PerdidasTotalesPropios.FirstOrDefault();
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

            vResultado = BD.CotizacionICRL.PerdidaTotalPropioModificar(vFilaTabla);
            if (vResultado)
            {
              LabelDatosDueniosMsj.Text = "Registro Borrado exitosamente";

              ButtonPTDPGrabar.Visible = false;
              ButtonPTDPCancelar.Visible = false;
            }
            else
            {
              LabelDatosDueniosMsj.Text = "El Registro no pudo ser Borrado";
            }
          }

          if (CheckBoxPTDPTotPagado.Checked)
          {
            ButtonPTDPAgregar.Visible = true;
          }
          else
          {
            ButtonPTDPAgregar.Visible = false;
          }
          ButtonPTDPGrabar.Visible = false;
          ButtonPTDPCancelar.Visible = false;
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
      CotizacionICRL.TipoPerdidaTotalPropioTraer vTipoPerdidaTotalPropioTraer;
      vTipoPerdidaTotalPropioTraer = CotizacionICRL.PerdidaTotalPropioTraer(pIdFlujo, pIdCotizacion);

      if (vTipoPerdidaTotalPropioTraer.Correcto)
      {
        var vFilaTabla = vTipoPerdidaTotalPropioTraer.PerdidasTotalesPropios.FirstOrDefault();
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

    protected void ButtonPTDPAgregar_Click(object sender, EventArgs e)
    {
      int vCantidadFilasGrid = 0;
      vCantidadFilasGrid = GridViewPTDPDueniosaPagar.Rows.Count;

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
          TextBoxPTDPIndice.Text = vIndiceLibre.ToString();
          PLimpiarBeneficiario();
          PHabilitarBeneficiarioOtro(true);
          ButtonPTDPGrabar.Visible = true;
          ButtonPTDPCancelar.Visible = true;
          ButtonPTDPAgregar.Visible = false;

        }
      }
    }

    protected void ButtonPTDPGrabar_Click(object sender, EventArgs e)
    {
      int vIdFlujo = 0;
      int vIdCotizacion = 0;

      vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
      vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);

      CotizacionICRL.TipoPerdidaTotalPropioTraer vTipoPerdidaTotalPropioTraer;
      vTipoPerdidaTotalPropioTraer = CotizacionICRL.PerdidaTotalPropioTraer(vIdFlujo, vIdCotizacion);

      if (vTipoPerdidaTotalPropioTraer.Correcto)
      {
        var vFilaTabla = vTipoPerdidaTotalPropioTraer.PerdidasTotalesPropios.FirstOrDefault();
        int vIndice = 0;
        vIndice = int.Parse(TextBoxPTDPIndice.Text);
        if (1 == vIndice)
        {
          vFilaTabla.duenio_nombres_1 = TextBoxPTDPNombres.Text;
          vFilaTabla.duenio_documento_1 = TextBoxPTDPDocumentoId.Text;
          vFilaTabla.duenio_monto_1 = double.Parse(TextBoxPTDPMontoPago.Text);
          vFilaTabla.duenio_descripcion_1 = TextBoxPTDPDescripcion.Text;
        }

        if (2 == vIndice)
        {
          vFilaTabla.duenio_nombres_2 = TextBoxPTDPNombres.Text;
          vFilaTabla.duenio_documento_2 = TextBoxPTDPDocumentoId.Text;
          vFilaTabla.duenio_monto_2 = double.Parse(TextBoxPTDPMontoPago.Text);
          vFilaTabla.duenio_descripcion_2 = TextBoxPTDPDescripcion.Text;
        }

        if (3 == vIndice)
        {
          vFilaTabla.duenio_nombres_3 = TextBoxPTDPNombres.Text;
          vFilaTabla.duenio_documento_3 = TextBoxPTDPDocumentoId.Text;
          vFilaTabla.duenio_monto_3 = double.Parse(TextBoxPTDPMontoPago.Text);
          vFilaTabla.duenio_descripcion_3 = TextBoxPTDPDescripcion.Text;
        }

        if (4 == vIndice)
        {
          vFilaTabla.duenio_nombres_4 = TextBoxPTDPNombres.Text;
          vFilaTabla.duenio_documento_4 = TextBoxPTDPDocumentoId.Text;
          vFilaTabla.duenio_monto_4 = double.Parse(TextBoxPTDPMontoPago.Text);
          vFilaTabla.duenio_descripcion_4 = TextBoxPTDPDescripcion.Text;
        }

        if (5 == vIndice)
        {
          vFilaTabla.duenio_nombres_5 = TextBoxPTDPNombres.Text;
          vFilaTabla.duenio_documento_5 = TextBoxPTDPDocumentoId.Text;
          vFilaTabla.duenio_monto_5 = double.Parse(TextBoxPTDPMontoPago.Text);
          vFilaTabla.duenio_descripcion_5 = TextBoxPTDPDescripcion.Text;
        }

        bool vResultado = false;
        vResultado = CotizacionICRL.PerdidaTotalPropioModificar(vFilaTabla);
        if (vResultado)
        {
          if (CheckBoxPTDPTotPagado.Checked)
          {
            ButtonPTDPAgregar.Visible = true;
          }
          else
          {
            ButtonPTDPAgregar.Visible = false;
          }
          ButtonPTDPGrabar.Visible = false;
          ButtonPTDPCancelar.Visible = false;
          PLimpiarBeneficiario();
          PHabilitarBeneficiarioOtro(false);
        }

        PCargarGrillaBeneficiarios(vIdFlujo, vIdCotizacion);
      }
    }

    protected void ButtonPTDPCancelar_Click(object sender, EventArgs e)
    {
      int vIdFlujo = 0;
      int vIdCotizacion = 0;

      vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
      vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);
      ButtonPTDPGrabar.Visible = false;
      ButtonPTDPCancelar.Visible = false;

      if (CheckBoxPTDPTotPagado.Checked)
      {
        ButtonPTDPAgregar.Visible = true;
      }

      PLimpiarBeneficiario();
      PHabilitarBeneficiarioOtro(false);
      ButtonPTDPGrabar.Visible = false;
      ButtonPTDPCancelar.Visible = false;
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
      dtReferencia = CotizacionICRL.PerdidaTotalPropioTraerReferencias(pIdFlujo, pIdCotizacion, pIdItem);

      GridViewPTDPReferencias.DataSource = dtReferencia;
      GridViewPTDPReferencias.DataBind();

    }

    protected void GridViewPTDPReferencias_SelectedIndexChanged(object sender, EventArgs e)
    {
      string vTextoTemporal = string.Empty;
      int vIndice = 0;

      //Leer Registro de la grilla y cargar los valores a la ventana.
      vIndice = int.Parse(GridViewPTDPReferencias.SelectedRow.Cells[1].Text);

      TextBoxReferIndice.Text = GridViewPTDPReferencias.SelectedRow.Cells[1].Text;
      CheckboxReferUtilizada.Checked = (GridViewPTDPReferencias.SelectedRow.Cells[2].Controls[1] as CheckBox).Checked;
      TextBoxReferMedioCoti.Text = GridViewPTDPReferencias.SelectedRow.Cells[3].Text;
      TextBoxReferDescripcion.Text = GridViewPTDPReferencias.SelectedRow.Cells[4].Text;
      TextBoxReferMontoCoti.Text = GridViewPTDPReferencias.SelectedRow.Cells[5].Text;

      PHabilitarReferencias(true);

      ButtonReferAgregar.Visible = false;
      ButtonReferGrabar.Visible = true;
      ButtonReferCancelar.Visible = true;
    }

    protected void GridViewPTDPReferencias_RowDeleting(object sender, GridViewDeleteEventArgs e)
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
      vIndice = Convert.ToInt32(GridViewPTDPReferencias.DataKeys[vIndex].Value);

      vIdItem = FValidaTienePTDP(vIdFlujo, vIdCotizacion);
      if (vIdItem > 0)
      {
        dtReferencia = CotizacionICRL.PerdidaTotalPropioTraerReferencias(vIdFlujo, vIdCotizacion, vIdItem);
        //se borra una fila del dataset
        dtReferencia.Rows[vIndice - 1].Delete();
        vResultado = CotizacionICRL.PerdidaTotalPropioActualizarReferencias(vIdFlujo, vIdCotizacion, vIdItem, dtReferencia);
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


      vIdItem = FValidaTienePTDP(vIdFlujo, vIdCotizacion);
      if (vIdItem > 0)
      {
        dtReferencia = CotizacionICRL.PerdidaTotalPropioTraerReferencias(vIdFlujo, vIdCotizacion, vIdItem);

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

      vIdItem = FValidaTienePTDP(vIdFlujo, vIdCotizacion);
      if (vIdItem > 0)
      {
        vIndice = int.Parse(TextBoxReferIndice.Text);
        dtReferencia = CotizacionICRL.PerdidaTotalPropioTraerReferencias(vIdFlujo, vIdCotizacion, vIdItem);
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
        vResultado = CotizacionICRL.PerdidaTotalPropioActualizarReferencias(vIdFlujo, vIdCotizacion, vIdItem, dtReferencia);
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

    #region DatosEspeciales

    private int FlTraeItemsNomencladorCajaPTDP()
    {
      int vResultado = 0;
      string vCategoria = "Tipo de Caja";
      int vOrdenCodigo = 2;
      AccesoDatos vAccesoDatos = new AccesoDatos();

      DropDownListCajaPTDP.DataValueField = "codigo";
      DropDownListCajaPTDP.DataTextField = "descripcion";
      DropDownListCajaPTDP.DataSource = vAccesoDatos.FlTraeNomenGenerico(vCategoria, vOrdenCodigo);
      DropDownListCajaPTDP.DataBind();

      return vResultado;

    }

    private int FlTraeItemsNomencladorCombustiblePTDP()
    {
      int vResultado = 0;
      string vCategoria = "Combustible";
      int vOrdenCodigo = 2;
      AccesoDatos vAccesoDatos = new AccesoDatos();

      DropDownListCombustiblePTDP.DataValueField = "codigo";
      DropDownListCombustiblePTDP.DataTextField = "descripcion";
      DropDownListCombustiblePTDP.DataSource = vAccesoDatos.FlTraeNomenGenerico(vCategoria, vOrdenCodigo);
      DropDownListCombustiblePTDP.DataBind();


      return vResultado;
    }

    private int FlCargaDatosPerdidaTotalDP(int pIdFlujo, int pIdCotizacion, long pIdItem)
    {
      int vResultado = 0;

      int vIdFlujo = 0;
      int vIdCotizacion = 0;
      long vIdItem = 0;

      vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
      vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);

      vIdItem = FValidaTienePTDP(vIdFlujo, vIdCotizacion);
      if (vIdItem > 0)
      {
        CotizacionICRL.TipoPerdidaTotalPropioTraer vTipoPerdidaTotalPropioTraer;
        vTipoPerdidaTotalPropioTraer = CotizacionICRL.PerdidaTotalPropioTraer(vIdFlujo, vIdCotizacion);
        if (vTipoPerdidaTotalPropioTraer.Correcto)
        {
          var vFilaTablaCotiPerdidaTotDP = vTipoPerdidaTotalPropioTraer.PerdidasTotalesPropios.FirstOrDefault();
          if (vFilaTablaCotiPerdidaTotDP != null)
          {
            TextBoxVersionPTDP.Text = vFilaTablaCotiPerdidaTotDP.version;
            TextBoxSeriePTDP.Text = vFilaTablaCotiPerdidaTotDP.serie;

            string vTempo = string.Empty;
            vTempo = vFilaTablaCotiPerdidaTotDP.caja;
            DropDownListCajaPTDP.ClearSelection();
            DropDownListCajaPTDP.Items.FindByText(vTempo).Selected = true;

            vTempo = string.Empty;
            vTempo = vFilaTablaCotiPerdidaTotDP.combustible;
            DropDownListCombustiblePTDP.ClearSelection();
            DropDownListCombustiblePTDP.Items.FindByText(vTempo).Selected = true;

            TextBoxCilindradaPTDP.Text = vFilaTablaCotiPerdidaTotDP.cilindrada.ToString();
            CheckBoxTechoSolarPTDP.Checked = (bool)vFilaTablaCotiPerdidaTotDP.techo_solar;
            CheckBoxAsientosCueroPTDP.Checked = (bool)vFilaTablaCotiPerdidaTotDP.asientos_cuero;
            CheckBoxArosMagnesioPTDP.Checked = (bool)vFilaTablaCotiPerdidaTotDP.aros_magnesio;
            TextBoxObservacionesPTDP.Text = vFilaTablaCotiPerdidaTotDP.observaciones_vehiculo;
          }
        }

      }

      return vResultado;
    }

    protected void PLimpiaSeccionPerdidaTotalPTDP()
    {
      TextBoxVersionPTDP.Text = string.Empty;
      TextBoxSeriePTDP.Text = string.Empty;
      TextBoxCilindradaPTDP.Text = string.Empty;
      CheckBoxTechoSolarPTDP.Checked = false;
      CheckBoxAsientosCueroPTDP.Checked = false;
      CheckBoxArosMagnesioPTDP.Checked = false;
      TextBoxObservacionesPTDP.Text = string.Empty;
      DropDownListCajaPTDP.SelectedIndex = 0;
      DropDownListCombustiblePTDP.SelectedIndex = 0;
    }

    protected void PBotonesDatosEspecialesPTDP(bool pEstado)
    {
      ButtonActualizarPTDP.Visible = pEstado;
      ButtonGrabarPTDP.Visible = !pEstado;
      ButtonCancelarPTDP.Visible = !pEstado;
    }

    protected void PBloqueaDatosEspeciales(bool pEstado)
    {
      TextBoxVersionPTDP.Enabled = pEstado;
      TextBoxSeriePTDP.Enabled = pEstado;
      TextBoxCilindradaPTDP.Enabled = pEstado;
      CheckBoxTechoSolarPTDP.Enabled = pEstado;
      CheckBoxAsientosCueroPTDP.Enabled = pEstado;
      CheckBoxArosMagnesioPTDP.Enabled = pEstado;
      TextBoxObservacionesPTDP.Enabled = pEstado;
      DropDownListCajaPTDP.Enabled = pEstado;
      DropDownListCombustiblePTDP.Enabled = pEstado;
    }

    #endregion

    protected void ButtonActualizarPTDP_Click(object sender, EventArgs e)
    {
      PBloqueaDatosEspeciales(true);
      PBotonesDatosEspecialesPTDP(false);

    }

    protected void ButtonGrabarPTDP_Click(object sender, EventArgs e)
    {
      bool vResultado = false;
      int vIdFlujo = 0;
      int vIdCotizacion = 0;
      long vIdItem = 0;

      vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
      vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);

      vIdItem = FValidaTienePTDP(vIdFlujo, vIdCotizacion);
      if (vIdItem > 0)
      {
        CotizacionICRL.TipoPerdidaTotalPropioTraer vTipoPerdidaTotalPropioTraer;
        vTipoPerdidaTotalPropioTraer = CotizacionICRL.PerdidaTotalPropioTraer(vIdFlujo, vIdCotizacion);
        if (vTipoPerdidaTotalPropioTraer.Correcto)
        {
          var vFilaTablaCotiPerdidaTotDP = vTipoPerdidaTotalPropioTraer.PerdidasTotalesPropios.FirstOrDefault();
          if (vFilaTablaCotiPerdidaTotDP != null)
          {
            vFilaTablaCotiPerdidaTotDP.version = TextBoxVersionPTDP.Text.ToUpper();
            vFilaTablaCotiPerdidaTotDP.serie = TextBoxSeriePTDP.Text.ToUpper();
            vFilaTablaCotiPerdidaTotDP.caja = DropDownListCajaPTDP.SelectedItem.Text;
            vFilaTablaCotiPerdidaTotDP.combustible = DropDownListCombustiblePTDP.SelectedItem.Text;
            vFilaTablaCotiPerdidaTotDP.cilindrada = TextBoxCilindradaPTDP.Text.ToUpper();
            vFilaTablaCotiPerdidaTotDP.techo_solar = CheckBoxTechoSolarPTDP.Checked;
            vFilaTablaCotiPerdidaTotDP.asientos_cuero = CheckBoxAsientosCueroPTDP.Checked;
            vFilaTablaCotiPerdidaTotDP.aros_magnesio = CheckBoxTechoSolarPTDP.Checked;
            vFilaTablaCotiPerdidaTotDP.observaciones_vehiculo = TextBoxObservacionesPTDP.Text.ToUpper();
            vResultado = BD.CotizacionICRL.PerdidaTotalPropioModificar(vFilaTablaCotiPerdidaTotDP);
            if (vResultado)
            {
              LabelDatosReferMsj.Text = "Registro Actualizado exitosamente";

              ButtonPTDPGrabar.Visible = false;
              ButtonPTDPCancelar.Visible = false;
            }
            else
            {
              LabelDatosReferMsj.Text = "El Registro no pudo ser Actualizado";
            }
          }
        }
        FlCargaDatosPerdidaTotalDP(vIdFlujo, vIdCotizacion, vIdItem);
        PBloqueaDatosEspeciales(false);
        PBotonesDatosEspecialesPTDP(true);
      }
    }

    protected void ButtonCancelarPTDP_Click(object sender, EventArgs e)
    {
      int vIdFlujo = 0;
      int vIdCotizacion = 0;
      long vIdItem = 0;

      vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
      vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);

      PLimpiaSeccionPerdidaTotalPTDP();

      vIdItem = FValidaTienePTDP(vIdFlujo, vIdCotizacion);
      if (vIdItem > 0)
      {
        FlCargaDatosPerdidaTotalDP(vIdFlujo, vIdCotizacion, vIdItem);
      }

      PBloqueaDatosEspeciales(false);
      PBotonesDatosEspecialesPTDP(true);
    }

    #region Generar Orden

    protected void ButtonGenerarOrden_Click(object sender, EventArgs e)
    {
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
      vNumeroOrden = vNumeroOrden.PadLeft(7, '0');
      vSBNumeroOrden.Append(vNumeroOrden);
      vSBNumeroOrden.Append("-TP-");
      vNumeroOrden = vContador.ToString();
      vSBNumeroOrden.Append(vNumeroOrden.PadLeft(2, '0'));
      vNumeroOrden = vSBNumeroOrden.ToString();

      CotizacionICRL.TipoOrden vTipoOrden = new CotizacionICRL.TipoOrden();
      vTipoOrden.id_flujo = vIdFlujo;
      vTipoOrden.id_cotizacion = vIdCotizacion;
      vTipoOrden.tipo_origen = (short)AccesoDatos.TipoInspeccion.PerdidaTotalDaniosPropios;
      vTipoOrden.numero_orden = vNumeroOrden;
      vTipoOrden.fecha_orden = DateTime.Today;
      vTipoOrden.descripcion = TextBoxNombreAsegurado.Text.ToUpper();
      vTipoOrden.monto_us = 0;
      vTipoOrden.id_estado = 1;

      //Sumar los gastos del detalle


      CotizacionICRL.TipoPerdidaTotalPropioTraer vTipoPerdidaTotalPropioTraer;
      vTipoPerdidaTotalPropioTraer = CotizacionICRL.PerdidaTotalPropioTraer(vIdFlujo, vIdCotizacion);

      double vSumaGastos = 0;

      if (vTipoPerdidaTotalPropioTraer.Correcto)
      {
        var vFilaTabla = vTipoPerdidaTotalPropioTraer.PerdidasTotalesPropios.FirstOrDefault();

        if (string.Empty != vFilaTabla.duenio_nombres_1)
        {
          vSumaGastos = vSumaGastos + vFilaTabla.duenio_monto_1;
        }

        if (string.Empty != vFilaTabla.duenio_nombres_2)
        {
          vSumaGastos = vSumaGastos + vFilaTabla.duenio_monto_2;
        }

        if (string.Empty != vFilaTabla.duenio_nombres_3)
        {
          vSumaGastos = vSumaGastos + vFilaTabla.duenio_monto_3;
        }

        if (string.Empty != vFilaTabla.duenio_nombres_4)
        {
          vSumaGastos = vSumaGastos + vFilaTabla.duenio_monto_4;
        }

        if (string.Empty != vFilaTabla.duenio_nombres_5)
        {
          vSumaGastos = vSumaGastos + vFilaTabla.duenio_monto_5;
        }
      }

      vTipoOrden.monto_bs = vSumaGastos;
      vResultado = CotizacionICRL.OrdenRegistrar(vTipoOrden);
      if (vResultado)
      {
        LabelDatosDueniosMsj.Text = "Orden creada exitosamente";
      }
      else
      {
        LabelDatosDueniosMsj.Text = "la Orden no se pudo crear";
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
        Ordenes.monto_bs
      }).ToList();
      GridViewOrdenes.DataBind();
    }
    protected void GridViewOrdenes_RowCommand(object sender, GridViewCommandEventArgs e)
    {
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
        PImprimeFormularioCotiPTDaniosP(vNumeroOrden);
      }

      if (0 == e.CommandName.CompareTo("Ver"))
      {
        string vTextoSecuencial = string.Empty;
        vIndex = 0;

        vIndex = Convert.ToInt32(e.CommandArgument);
        vNumeroOrden = (string)GridViewOrdenes.DataKeys[vIndex].Value;
        vProveedor = GridViewOrdenes.Rows[vIndex].Cells[2].Text;
        PVerFormularioCotiPTDaniosP(vNumeroOrden);
      }

      if (0 == e.CommandName.CompareTo("SubirOnBase"))
      {
        int vResultado = 0;
        string vTextoSecuencial = string.Empty;


        vIndex = Convert.ToInt32(e.CommandArgument);
        vNumeroOrden = (string)GridViewOrdenes.DataKeys[vIndex].Value;
        vProveedor = GridViewOrdenes.Rows[vIndex].Cells[2].Text;


        //Grabar en la tabla
        int vIdFlujo = 0;
        int vIdCotizacion = 0;
        double vTipoCambio = 0;

        vIdFlujo = int.Parse(TextBoxIdFlujo.Text);
        vIdCotizacion = int.Parse(TextBoxNroCotizacion.Text);
        vTipoCambio = double.Parse(TextBoxTipoCambio.Text);

        vResultado = vAccesoDatos.fActualizaLiquidacionTP(vIdFlujo, vIdCotizacion, vNumeroOrden, vTipoCambio);
        PSubeFormularioCotiPTDaniosP(vNumeroOrden);
      }
    }

    protected void PImprimeFormularioCotiPTDaniosP(string pNroOrden)
    {
      AccesoDatos vAccesoDatos = new AccesoDatos();
      LBCDesaEntities db = new LBCDesaEntities();


      Warning[] warnings;
      string[] streamIds;
      string mimeType = string.Empty;
      string encoding = string.Empty;
      string extension = "pdf";
      string fileName = "RepFormCotiPTDaniosP" + pNroOrden;

      var vListaFlujo = from f in db.Flujo
                        join s in db.cotizacion_ordenes on f.idFlujo equals s.id_flujo
                        where (s.numero_orden == pNroOrden)
                        select new
                        {
                          f.nombreAsegurado,
                          f.telefonocelAsegurado,
                          cobertura = "PT DAÑOSP",
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

      var vListaCotiPTDaniosP = (from c in db.cotizacion_perdida_total_propio
                                 join d in db.cotizacion_ordenes
                                 on new { c.id_flujo, c.id_cotizacion } equals new { d.id_flujo, d.id_cotizacion }
                                 where (d.numero_orden == pNroOrden) && (string.Empty != c.duenio_nombres_1)
                                 select new
                                 {
                                   d.numero_orden,
                                   duenio_nombres = c.duenio_nombres_1,
                                   duenio_documento = c.duenio_documento_1,
                                   duenio_descripcion = c.duenio_descripcion_1,
                                   duenio_monto = c.duenio_monto_1
                                 }).Union
                                (from c in db.cotizacion_perdida_total_propio
                                 join d in db.cotizacion_ordenes
                                 on new { c.id_flujo, c.id_cotizacion } equals new { d.id_flujo, d.id_cotizacion }
                                 where (d.numero_orden == pNroOrden) && (string.Empty != c.duenio_nombres_2)
                                 select new
                                 {
                                   d.numero_orden,
                                   duenio_nombres = c.duenio_nombres_2,
                                   duenio_documento = c.duenio_documento_2,
                                   duenio_descripcion = c.duenio_descripcion_2,
                                   duenio_monto = c.duenio_monto_2
                                 }).Union
                                (from c in db.cotizacion_perdida_total_propio
                                 join d in db.cotizacion_ordenes
                                 on new { c.id_flujo, c.id_cotizacion } equals new { d.id_flujo, d.id_cotizacion }
                                 where (d.numero_orden == pNroOrden) && (string.Empty != c.duenio_nombres_3)
                                 select new
                                 {
                                   d.numero_orden,
                                   duenio_nombres = c.duenio_nombres_3,
                                   duenio_documento = c.duenio_documento_3,
                                   duenio_descripcion = c.duenio_descripcion_3,
                                   duenio_monto = c.duenio_monto_3
                                 }).Union
                                (from c in db.cotizacion_perdida_total_propio
                                 join d in db.cotizacion_ordenes
                                 on new { c.id_flujo, c.id_cotizacion } equals new { d.id_flujo, d.id_cotizacion }
                                 where (d.numero_orden == pNroOrden) && (string.Empty != c.duenio_nombres_4)
                                 select new
                                 {
                                   d.numero_orden,
                                   duenio_nombres = c.duenio_nombres_4,
                                   duenio_documento = c.duenio_documento_4,
                                   duenio_descripcion = c.duenio_descripcion_4,
                                   duenio_monto = c.duenio_monto_4
                                 }).Union
                                (from c in db.cotizacion_perdida_total_propio
                                 join d in db.cotizacion_ordenes
                                 on new { c.id_flujo, c.id_cotizacion } equals new { d.id_flujo, d.id_cotizacion }
                                 where (d.numero_orden == pNroOrden) && (string.Empty != c.duenio_nombres_5)
                                 select new
                                 {
                                   d.numero_orden,
                                   duenio_nombres = c.duenio_nombres_5,
                                   duenio_documento = c.duenio_documento_5,
                                   duenio_descripcion = c.duenio_descripcion_5,
                                   duenio_monto = c.duenio_monto_5
                                 });



      ReportViewerCoti.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
      ReportViewerCoti.LocalReport.ReportPath = "Reportes\\RepFormularioCotiPTDaniosPropios.rdlc";
      ReportDataSource datasource1 = new ReportDataSource("DataSet1", vListaFlujo);
      ReportDataSource datasource2 = new ReportDataSource("DataSet2", vListaCotiPTDaniosP);


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

    protected void PVerFormularioCotiPTDaniosP(string pNroOrden)
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
                          cobertura = "PT DAÑOSP",
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

      var vListaCotiPTDaniosP = (from c in db.cotizacion_perdida_total_propio
                                 join d in db.cotizacion_ordenes
                                 on new { c.id_flujo, c.id_cotizacion } equals new { d.id_flujo, d.id_cotizacion }
                                 where (d.numero_orden == pNroOrden) && (string.Empty != c.duenio_nombres_1)
                                 select new
                                 {
                                   d.numero_orden,
                                   duenio_nombres = c.duenio_nombres_1,
                                   duenio_documento = c.duenio_documento_1,
                                   duenio_descripcion = c.duenio_descripcion_1,
                                   duenio_monto = c.duenio_monto_1
                                 }).Union
                                 (from c in db.cotizacion_perdida_total_propio
                                  join d in db.cotizacion_ordenes
                                  on new { c.id_flujo, c.id_cotizacion } equals new { d.id_flujo, d.id_cotizacion }
                                  where (d.numero_orden == pNroOrden) && (string.Empty != c.duenio_nombres_2)
                                  select new
                                  {
                                    d.numero_orden,
                                    duenio_nombres = c.duenio_nombres_2,
                                    duenio_documento = c.duenio_documento_2,
                                    duenio_descripcion = c.duenio_descripcion_2,
                                    duenio_monto = c.duenio_monto_2
                                  }).Union
                                 (from c in db.cotizacion_perdida_total_propio
                                  join d in db.cotizacion_ordenes
                                  on new { c.id_flujo, c.id_cotizacion } equals new { d.id_flujo, d.id_cotizacion }
                                  where (d.numero_orden == pNroOrden) && (string.Empty != c.duenio_nombres_3)
                                  select new
                                  {
                                    d.numero_orden,
                                    duenio_nombres = c.duenio_nombres_3,
                                    duenio_documento = c.duenio_documento_3,
                                    duenio_descripcion = c.duenio_descripcion_3,
                                    duenio_monto = c.duenio_monto_3
                                  }).Union
                                 (from c in db.cotizacion_perdida_total_propio
                                  join d in db.cotizacion_ordenes
                                  on new { c.id_flujo, c.id_cotizacion } equals new { d.id_flujo, d.id_cotizacion }
                                  where (d.numero_orden == pNroOrden) && (string.Empty != c.duenio_nombres_4)
                                  select new
                                  {
                                    d.numero_orden,
                                    duenio_nombres = c.duenio_nombres_4,
                                    duenio_documento = c.duenio_documento_4,
                                    duenio_descripcion = c.duenio_descripcion_4,
                                    duenio_monto = c.duenio_monto_4
                                  }).Union
                                 (from c in db.cotizacion_perdida_total_propio
                                  join d in db.cotizacion_ordenes
                                  on new { c.id_flujo, c.id_cotizacion } equals new { d.id_flujo, d.id_cotizacion }
                                  where (d.numero_orden == pNroOrden) && (string.Empty != c.duenio_nombres_5)
                                  select new
                                  {
                                    d.numero_orden,
                                    duenio_nombres = c.duenio_nombres_5,
                                    duenio_documento = c.duenio_documento_5,
                                    duenio_descripcion = c.duenio_descripcion_5,
                                    duenio_monto = c.duenio_monto_5
                                  });



      ReportViewerCoti.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
      ReportViewerCoti.LocalReport.ReportPath = "Reportes\\RepFormularioCotiPTDaniosPropios.rdlc";
      ReportDataSource datasource1 = new ReportDataSource("DataSet1", vListaFlujo);
      ReportDataSource datasource2 = new ReportDataSource("DataSet2", vListaCotiPTDaniosP);


      ReportViewerCoti.LocalReport.DataSources.Clear();
      ReportViewerCoti.LocalReport.DataSources.Add(datasource1);
      ReportViewerCoti.LocalReport.DataSources.Add(datasource2);

      ReportViewerCoti.LocalReport.Refresh();
      ReportViewerCoti.ShowToolBar = false;
      ReportViewerCoti.Visible = true;

    }

    protected void PSubeFormularioCotiPTDaniosP(string pNroOrden)
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
      string fileName = "RepFormCotiPTDaniosP" + pNroOrden;

      vTipoDocumental = "RE - Orden de Indemnizacion";

      vNombreUsuario = Session["IdUsr"].ToString();

      var vListaFlujo = from f in db.Flujo
                        join s in db.cotizacion_ordenes on f.idFlujo equals s.id_flujo
                        where (s.numero_orden == pNroOrden)
                        select new
                        {
                          f.nombreAsegurado,
                          f.telefonocelAsegurado,
                          cobertura = "PT DAÑOSP",
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

      var vListaCotiPTDaniosP = (from c in db.cotizacion_perdida_total_propio
                                 join d in db.cotizacion_ordenes
                                 on new { c.id_flujo, c.id_cotizacion } equals new { d.id_flujo, d.id_cotizacion }
                                 where (d.numero_orden == pNroOrden) && (string.Empty != c.duenio_nombres_1)
                                 select new
                                 {
                                   d.numero_orden,
                                   duenio_nombres = c.duenio_nombres_1,
                                   duenio_documento = c.duenio_documento_1,
                                   duenio_descripcion = c.duenio_descripcion_1,
                                   duenio_monto = c.duenio_monto_1
                                 }).Union
                                 (from c in db.cotizacion_perdida_total_propio
                                  join d in db.cotizacion_ordenes
                                  on new { c.id_flujo, c.id_cotizacion } equals new { d.id_flujo, d.id_cotizacion }
                                  where (d.numero_orden == pNroOrden) && (string.Empty != c.duenio_nombres_2)
                                  select new
                                  {
                                    d.numero_orden,
                                    duenio_nombres = c.duenio_nombres_2,
                                    duenio_documento = c.duenio_documento_2,
                                    duenio_descripcion = c.duenio_descripcion_2,
                                    duenio_monto = c.duenio_monto_2
                                  }).Union
                                 (from c in db.cotizacion_perdida_total_propio
                                  join d in db.cotizacion_ordenes
                                  on new { c.id_flujo, c.id_cotizacion } equals new { d.id_flujo, d.id_cotizacion }
                                  where (d.numero_orden == pNroOrden) && (string.Empty != c.duenio_nombres_3)
                                  select new
                                  {
                                    d.numero_orden,
                                    duenio_nombres = c.duenio_nombres_3,
                                    duenio_documento = c.duenio_documento_3,
                                    duenio_descripcion = c.duenio_descripcion_3,
                                    duenio_monto = c.duenio_monto_3
                                  }).Union
                                 (from c in db.cotizacion_perdida_total_propio
                                  join d in db.cotizacion_ordenes
                                  on new { c.id_flujo, c.id_cotizacion } equals new { d.id_flujo, d.id_cotizacion }
                                  where (d.numero_orden == pNroOrden) && (string.Empty != c.duenio_nombres_4)
                                  select new
                                  {
                                    d.numero_orden,
                                    duenio_nombres = c.duenio_nombres_4,
                                    duenio_documento = c.duenio_documento_4,
                                    duenio_descripcion = c.duenio_descripcion_4,
                                    duenio_monto = c.duenio_monto_4
                                  }).Union
                                 (from c in db.cotizacion_perdida_total_propio
                                  join d in db.cotizacion_ordenes
                                  on new { c.id_flujo, c.id_cotizacion } equals new { d.id_flujo, d.id_cotizacion }
                                  where (d.numero_orden == pNroOrden) && (string.Empty != c.duenio_nombres_5)
                                  select new
                                  {
                                    d.numero_orden,
                                    duenio_nombres = c.duenio_nombres_5,
                                    duenio_documento = c.duenio_documento_5,
                                    duenio_descripcion = c.duenio_descripcion_5,
                                    duenio_monto = c.duenio_monto_5
                                  });



      ReportViewerCoti.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
      ReportViewerCoti.LocalReport.ReportPath = "Reportes\\RepFormularioCotiPTDaniosPropios.rdlc";
      ReportDataSource datasource1 = new ReportDataSource("DataSet1", vListaFlujo);
      ReportDataSource datasource2 = new ReportDataSource("DataSet2", vListaCotiPTDaniosP);


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
      ReportViewerCoti.Visible = false;
      ButtonCierraVerRep.Visible = false;
    }

    #endregion

  }
}