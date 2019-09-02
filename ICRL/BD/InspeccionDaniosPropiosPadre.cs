using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICRL.BD
{
    public class InspeccionDaniosPropiosPadre
    {
        public int secuencial { get; set; }
        public int idInspeccion { get; set; }
        public string tipoTaller { get; set; }
        public bool cambioAPerdidaTotal { get; set; }
        public int estado { get; set; }
    }
}