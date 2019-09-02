using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICRL.BD
{
    public class InspeccionRCPersona
    {
        public int secuencial { get; set; }
        public int idInspeccion { get; set; }
        public string nombrePersona { get; set; }
        public string docIdentidadPersona { get; set; }
        public string telefonoPersona { get; set; }
        public string observacionesPersona { get; set; }
        public int estado { get; set; }
    }
}