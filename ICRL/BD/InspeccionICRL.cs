using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICRL.BD
{
    public class InspeccionICRL
    {
        public int idInspeccion { get; set; }
        public int idFlujo { get; set; }
        public string flujoOnBase { get; set; }
        public int idUsuario { get; set; }
        public string codUsuario { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string sucursalAtencion { get; set; }
        public string direccion { get; set; }
        public string zona { get; set; }
        public DateTime fechaSiniestro { get; set; }
        public string causaSiniestro { get; set; }
        public string descripcionSiniestro { get; set; }
        public string observacionesInspec { get; set; }
        public string idInspector { get; set; }
        public string nombreContacto { get; set; }
        public string telefonoContacto { get; set; }
        public string correosDeEnvio { get; set; }
        public bool recomendacionPerdidaTotal { get; set; }
        public int estado { get; set; }
        public int tipoInspeccion { get; set;}
        public int correlativo { get; set; }
        public string tipoTaller { get; set; }
    }
}