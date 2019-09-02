using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICRL.BD
{
    public class InspeccionRCObjeto
    {
        public int secuencial { get; set; }
        public int idInspeccion { get; set; }
        public string nombreObjeto { get; set; }
        public string docIdentidadObjeto { get; set; }
        public string telefonoObjeto { get; set; }
        public string observacionesObjeto { get; set; }
        public int estado { get; set; }
    }
}