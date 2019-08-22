using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICRL.BD
{
    public class FlujoICRL
    {
        public int idFlujo { get; set; }
        public string flujoOnBase { get; set; }
        public int estado { get; set; }
        public int numeroReclamo { get; set; }
        public string numeroPoliza { get; set; }
        public string marcaVehiculo { get; set; }
        public string modeloVehiculo { get; set; }
        public int anioVehiculo { get; set; }
        public string colorVehiculo { get; set; }
        public string placaVehiculo { get; set; }
        public string chasisVehiculo { get; set; }
        public decimal valorAsegurado { get; set; }
        public bool importacionDirecta { get; set; }
        public string nombreAsegurado { get; set; }
        public string docIdAsegurado { get; set; }
        public string telefonoCelAsegurado { get; set; }
        public string causaSiniestro { get; set; }
        public int contador { get; set; }
        public string descripcionSiniestro { get; set; }
        public string direccionInspeccion { get; set; }
        public string agenciaAtencion { get; set; }
        public DateTime fechaSiniestro { get; set; }
        public string tipoTaller { get; set; }
    }
}