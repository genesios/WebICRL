using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IRCL
{
  public partial class SitioICRL : System.Web.UI.MasterPage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (Session["IdUsr"] != null)
      {
        Usuario.Text = Session["IdUsr"].ToString();
      }

      if (Session["NomUsr"] != null)
      {
        Nombre.Text = Session["NomUsr"].ToString();
      }

      if (Session["CorreoUsr"] != null)
      {
        CorreoElectronico.Text = Session["CorreoUsr"].ToString();
      }

      if (Session["SucursalUsr"] != null)
      {
        Sucursal.Text = Session["SucursalUsr"].ToString();
      }

      if (!IsPostBack)
      {
        TreeNode vNodoNuevo;
        TreeNode vSubNodoNuevo;
        bool vRolInspeccion = false;
        bool vRolCotizacion = false;
        bool vRolLiquidacion = false;

        //nodo Inicio
        vNodoNuevo = new TreeNode
        {
          Value = "Inicio",
          Text = "Inicio",
          NavigateUrl = "~/Presentacion/Inicio.aspx"
        };

        TreeViewMenu.Nodes.Add(vNodoNuevo);

        if (null != Session["RolesUsr"])
        {
          foreach (var vRol in (string[])Session["RolesUsr"])
          {
            if (("ICRLInspeccion" == vRol.Substring(0, 14)) && (!vRolInspeccion))
            {
              //nodo Inspecciones
              vNodoNuevo = new TreeNode
              {
                Value = "Inspecciones",
                Text = "Inspecciones",
              };

              TreeViewMenu.Nodes.Add(vNodoNuevo);
              //cargar los subnodos
              vSubNodoNuevo = new TreeNode
              {
                Value = "GestionInspeccion",
                Text = "Gestión de Inspecciones",
                NavigateUrl = "~/Presentacion/GestionInspeccion.aspx"
              };

              vNodoNuevo.ChildNodes.Add(vSubNodoNuevo);
              vRolInspeccion = true;
            }

            if (("ICRLCotizacion" == vRol.Substring(0, 14)) && (!vRolCotizacion))
            {
              //nodo Cotizaciones
              vNodoNuevo = new TreeNode
              {
                Value = "Cotizaciones",
                Text = "Cotizaciones",
              };

              TreeViewMenu.Nodes.Add(vNodoNuevo);
              //cargar los subnodos
              vSubNodoNuevo = new TreeNode
              {
                Value = "GestionCotizacion",
                Text = "Gestión de de Cotizaciones",
                NavigateUrl = "~/Presentacion/GestionCotizacion.aspx"
              };

              vNodoNuevo.ChildNodes.Add(vSubNodoNuevo);
              vSubNodoNuevo = new TreeNode
              {
                Value = "CotizacionAnalista",
                Text = "Cotización Analista",
                NavigateUrl = "~/Presentacion/CotizacionAnalista.aspx"
              };

              vNodoNuevo.ChildNodes.Add(vSubNodoNuevo);
              vSubNodoNuevo = new TreeNode
              {
                Value = "MantenimientoFirma",
                Text = "Actualización de Firma y Sello",
                NavigateUrl = "~/Presentacion/MantenimientoFirma.aspx"
              };

              vNodoNuevo.ChildNodes.Add(vSubNodoNuevo);
              vRolCotizacion = true;
            }

            if (("ICRLLiquidacion" == vRol.Substring(0, 15)) && (!vRolLiquidacion))
            {
              //nodo Liquidaciones
              vNodoNuevo = new TreeNode
              {
                Value = "Liquidaciones",
                Text = "Liquidaciones",
                //NavigateUrl = "~/Presentacion/Inicio.asp"
              };

              TreeViewMenu.Nodes.Add(vNodoNuevo);
              //cargar los subnodos
              vSubNodoNuevo = new TreeNode
              {
                Value = "GestionLiquidacion",
                Text = "Gestión de Liquidaciones",
                NavigateUrl = "~/Presentacion/GestionLiquidacion.aspx"
              };

              vNodoNuevo.ChildNodes.Add(vSubNodoNuevo);
              vRolLiquidacion = true;
            }
          }
        }
      }
    }
  }
}