using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICRL.BD
{
    public class UsuarioICRL
    {
        public int idUsuario { get; set; }
        public string codUsuario { get; set; }
        public string apellidos { get; set; }
        public string nombres { get; set; }
        public string nombreVisible { get; set; }
        public string codSucursal { get; set; }
        public string correoElectronico { get; set; }
        public int estado { get; set; }
        public string usuarioCreacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string[] roles { get; set; }
    }
}