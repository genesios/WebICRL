//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ICRL.ModeloDB
{
    using System;
    using System.Collections.Generic;
    
    public partial class CotizacionFlujo
    {
        public int idFlujo { get; set; }
        public int idUsuario { get; set; }
        public System.DateTime fechaCreacion { get; set; }
        public string observacionesSiniestro { get; set; }
        public string Inspector { get; set; }
        public string nombreContacto { get; set; }
        public string telefonoContacto { get; set; }
        public string correosDeEnvio { get; set; }
        public int estado { get; set; }
        public System.DateTime fecha_siniestro { get; set; }
        public int correlativo { get; set; }
        public int usuario_modificacion { get; set; }
        public System.DateTime fecha_modificacion { get; set; }
    }
}
