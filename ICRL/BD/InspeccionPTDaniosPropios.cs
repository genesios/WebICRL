using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICRL.BD
{
    public class InspeccionPTDaniosPropios
    {
        public int idInspeccion { get; set; }
        public string version { get; set; }
        public string serie { get; set; }
        public string caja { get; set; }
        public string combustible { get; set; }
        public int cilindrada { get; set; }
        public bool techoSolar { get; set; }
        public bool asientosCuero { get; set; }
        public bool arosMagnesio { get; set; }
        public bool convertidoGNV { get; set; }
        public string observaciones { get; set; }
    }
}