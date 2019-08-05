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
            

        }
    }
}