using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LbcConsultaUsuarioSistema;
using ICRL.BD;

namespace ICRL.Acceso
{
  public partial class Login : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
      try
      {
        string uid = TextBox1.Text;
        string pass = TextBox2.Text;
        string vUrlPagina = string.Empty;

        int vResultado = 0;
        var vAccesoDatos = new AccesoDatos();

        if (1 == vAccesoDatos.FValidaExisteUsuarioArgos(uid, pass))
        {
          Label4.Text = "Validación de Usuario Argos exitosa ...";

          //Buscar en la tabla de Usuarios de ICRL al usuario Argos
          //Si existe el usuario continuar acceso al Sistema
          //Si no existe el usuario crear el registro correspondiente en el ICRL
          int vIdUsuario = vAccesoDatos.FValidaExisteUsuarioICRL(uid);
          if (vIdUsuario > 0)
          {
            //Si existe el usuario, actualizar sus datos desde Argos
            UsuarioICRL vUsuarioICRL = new UsuarioICRL();
            vUsuarioICRL = vAccesoDatos.FTraerDatosUsuarioArgos(uid, pass);

            if (vUsuarioICRL != null)
            {
              //Actualizar datos UsuarioICRL con los datos de Argos
              vResultado = vAccesoDatos.FActualizaUsuarioICRL(vUsuarioICRL, vIdUsuario);
              if (1 == vResultado)
              {
                //los datos se actualizaron correctamente
                if (vUsuarioICRL.codUsuario != null)
                  Session["IdUsr"] = vUsuarioICRL.codUsuario;
                else
                  Session["IdUsr"] = "UsuarioArgos";
                if (vUsuarioICRL.nombreVisible != null)
                  Session["NomUsr"] = vUsuarioICRL.nombreVisible;
                else
                  Session["NomUsr"] = "NombreArgos";
                if (vUsuarioICRL.correoElectronico != null)
                  Session["CorreoUsr"] = vUsuarioICRL.correoElectronico;
                else
                  Session["CorreoUsr"] = "usuario@argos.bo";
                if (vUsuarioICRL.codSucursal != null)
                  Session["SucursalUsr"] = vUsuarioICRL.codSucursal;
                else
                  Session["SucursalUsr"] = "SucursalArgos";
                //se accede al sistema
                //se carga la lista de Roles a una variable de Sesión
                if (vUsuarioICRL.roles != null)
                {
                  Session["RolesUsr"] = vUsuarioICRL.roles;
                  foreach (var vRol in (string[])Session["RolesUsr"])
                  {
                    if ("ICRLInspeccion" == vRol.Substring(0,14))
                    {
                      vUrlPagina = "~/Presentacion/GestionInspeccion.aspx";
                      break;
                    }
                    if ("ICRLCotizacion" == vRol.Substring(0, 14))
                    {
                      vUrlPagina = "~/Presentacion/GestionCotizacion.aspx";
                      break;
                    }
                    if ("ICRLLiquidacion" == vRol.Substring(0, 15))
                    {
                      vUrlPagina = "~/Presentacion/GestionLiquidacion.aspx";
                      break;
                    }
                  }
                  Response.Redirect(vUrlPagina,false);

                }
                else
                {
                  Label4.Text = "El usuario, no tiene roles para el sistema ICRL";
                }
                
              }
            }
            else
            {
              //no se pudo recuperar los datos del usuario desde el ICRL
              Label4.Text = "NO se puede recuperar los datos de ICRL, para el usuario";
            }
          }
          else
          {
            //No existe el usuario, crear registro con los datos desde Argos
            UsuarioICRL vUsuarioICRL = new UsuarioICRL();
            vUsuarioICRL = vAccesoDatos.FTraerDatosUsuarioArgos(uid, pass);
            if (vUsuarioICRL != null)
            {
              //Actualizar datos UsuarioICRL con los datos de Argos
              vResultado = vAccesoDatos.FGrabaUsuarioICRL(vUsuarioICRL, uid);
              if (1 == vResultado)
              {
                //los datos se actualizaron correctamente
                Session["IdUsr"] = vUsuarioICRL.codUsuario;
                Session["NomUsr"] = vUsuarioICRL.nombreVisible;
                Session["CorreoUsr"] = vUsuarioICRL.correoElectronico;
                Session["SucursalUsr"] = vUsuarioICRL.codSucursal;
                //se accede al sistema
                //se carga la lista de Roles a una variable de Sesión
                if (vUsuarioICRL.roles != null)
                {
                  Session["RolesUsr"] = vUsuarioICRL.roles;
                  foreach (var vRol in (string[])Session["RolesUsr"])
                  {
                    if ("ICRLInspeccion" == vRol.Substring(0, 14))
                    {
                      vUrlPagina = "~/Presentacion/GestionInspeccion.aspx";
                      break;
                    }
                    if ("ICRLCotizacion" == vRol.Substring(0, 14))
                    {
                      vUrlPagina = "~/Presentacion/GestionCotizacion.aspx";
                      break;
                    }
                    if ("ICRLLiquidacion" == vRol.Substring(0, 15))
                    {
                      vUrlPagina = "~/Presentacion/GestionLiquidacion.aspx";
                      break;
                    }
                  }
                  Response.Redirect(vUrlPagina, false);
                }
                else
                {
                  Label4.Text = "El usuario, no tiene roles para el sistema ICRL";
                }
              }
            }
          }

        }
        else
        {
          Label4.Text = "Usuario o Contraseña incorrecta";
        }

        ////Ejecutar la llamada al web service Argos
        //if (1 == fiValidaAcceso(uid, pass))
        //{
        //    Label4.Text = "Acceso exitoso...";
        //    Response.Redirect("~/Presentacion/GestionInspeccion.aspx");
        //}
        //else
        //{
        //    Label4.Text = "Usuario o Contraseña incorrecta";
        //}
      }
      catch (Exception ex)
      {
        Response.Write(ex.Message);
      }
    }

    private int fiValidaAcceso(string pCodUsuario, string pContrasenia)
    {
      int vResultado = 0;
      string vSistema = "EXT101";

      UsuarioSistemaEntity vClienteResultado = new UsuarioSistemaEntity();
      ConsultaUsuarioSistema vClienteServicio = new ConsultaUsuarioSistema();

      try
      {
        // Llamada al WS Argos para validar la información de Usuario y Contrasenia
        vClienteResultado = vClienteServicio.ConsultarUsuarioSistema(pCodUsuario, vSistema, pContrasenia);
        if (1 == vClienteResultado.CodigoEstado)
        {
          Session["IdUsr"] = vClienteResultado.Usuario;
          Session["NomUsr"] = vClienteResultado.NombreLargo;
          Session["CorreoUsr"] = vClienteResultado.Correo;
          Session["SucursalUsr"] = vClienteResultado.Sucursal;
          vResultado = 1;

        }
      }
      catch (Exception ex)
      {
        Response.Write(ex.Message);
      }

      return (vResultado);
    }
  }
}