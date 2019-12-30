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
  public partial class MantenimientoFirma : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void ButtonBuscarUsuario_Click(object sender, EventArgs e)
    {
      AccesoDatos vAccesodatos = new AccesoDatos();
      string vCadenaAux = string.Empty;
      int vResultado = 0;
      LabelIdUsuario.Text = string.Empty;
      LabelNombreUsuario.Text = string.Empty;

      vResultado = vAccesodatos.FValidaExisteUsuarioICRL(txtboxCodUsuario.Text);
      if (vResultado > 0)
      {
        LabelIdUsuario.Text = vResultado.ToString();
        UsuarioICRL vUsuarioICRL = null;
        vUsuarioICRL = vAccesodatos.FTraeUsuarioICRL(vResultado);
        LabelNombreUsuario.Text = vUsuarioICRL.nombreVisible;
        btnGrabaFirma.Enabled = true;
      }
    }

    protected void btnGrabaFirma_Click(object sender, EventArgs e)
    {
      AccesoDatos vAccesodatos = new AccesoDatos();
      string vRutaArchivo = string.Empty;
      int vResultado = 0;

      vRutaArchivo = FileUploadImagen.FileName;
      if (vRutaArchivo.Length == 0)
      {
        lblMensaje.Text = "La ruta del Archivo esta vacía";
      }
      else
      {
        byte[] vbytesArchivo;

        vbytesArchivo = FileUploadImagen.FileBytes;
        ManteFirma vManteFirma = new ManteFirma();
        vManteFirma.idUsuario = int.Parse(LabelIdUsuario.Text);
        string vIdUsuarioAux = string.Empty;
        vIdUsuarioAux = Session["IdUsr"].ToString();
        vManteFirma.usuarioCreacion = vAccesodatos.FValidaExisteUsuarioICRL(vIdUsuarioAux);
        vManteFirma.fechaCreacion = DateTime.Now;
        vManteFirma.estado = 1;
        vManteFirma.firmaSello = vbytesArchivo;
        vResultado = vAccesodatos.FUsuarioFirmaGrabaRegistro(vManteFirma);
        vResultado = 1;

        MuestraFirmaSello(vManteFirma.idUsuario);
      }
    }

    protected void MuestraFirmaSello(int pIdUsuarioFirmaSello)
    {
      AccesoDatos vAccesodatos = new AccesoDatos();
      ManteFirma vManteFirma = null;

      vManteFirma = vAccesodatos.FTraeFirmaSelloUsuario(pIdUsuarioFirmaSello);
      if (vManteFirma != null)
      {
        //ImageFirmaSelloActual;
        //Image rImage = null;
        //using (MemoryStream ms = new MemoryStream(arr))
        //{
        //  rImage = Image.FromStream(ms);
        //}
        string base64String = Convert.ToBase64String(vManteFirma.firmaSello, 0, vManteFirma.firmaSello.Length);
        ImageFirmaSelloActual.ImageUrl = "data:image/jpg;base64," + base64String;
        ImageFirmaSelloActual.DataBind();
      }
    }
  }
}