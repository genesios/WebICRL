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
    
    public partial class cotizacion_rc_objetos
    {
        public int id_flujo { get; set; }
        public int id_cotizacion { get; set; }
        public long id_item { get; set; }
        public string nombre_apellido { get; set; }
        public string telefono_contacto { get; set; }
        public string numero_documento { get; set; }
        public string tipo_item { get; set; }
        public Nullable<double> monto_item { get; set; }
        public Nullable<short> id_moneda { get; set; }
        public string descripcion { get; set; }
        public Nullable<bool> rembolso { get; set; }
        public Nullable<double> tipo_cambio { get; set; }
        public Nullable<short> id_estado { get; set; }
    }
}
