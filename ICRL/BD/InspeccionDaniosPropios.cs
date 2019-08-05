using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICRL.BD
{
    public class InspeccionDaniosPropios
    {
        public int idInspeccion { get; set; }
        public int idItem { get; set; }
        public string item { get; set; }
        public string compra { get; set; }
        public bool instalacion { get; set; }
        public bool pintura { get; set; }
        public bool mecanico { get; set; }
        public string chaperio { get; set; }
        public string reparacionPrevia { get; set; }
        public string observaciones { get; set; }
    }
}