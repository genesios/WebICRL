using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICRL.BD
{
    public class InspeccionRCVehicular
    {
        public int secuencial { get; set; }
        public int idInspeccion { get; set; }
        public string nombreTercero { get; set; }
        public string docIdentidadTercero { get; set; }
        public string telefonoTercero { get; set; }
        public string marca { get; set; }
        public string modelo { get; set; }
        public int anio { get; set; }
        public string placa { get; set; }
        public string color { get; set; }
        public string chasis { get; set; }
        public int kilometraje { get; set; }
        public bool importacionDirecta { get; set; }
        public string tipoTaller { get; set; }
        public int estado { get; set; }
    }
}