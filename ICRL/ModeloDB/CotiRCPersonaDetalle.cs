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
    
    public partial class CotiRCPersonaDetalle
    {
        public int secuencial { get; set; }
        public string tipo { get; set; }
        public string monedaGasto { get; set; }
        public Nullable<decimal> montoGasto { get; set; }
        public string descripcion { get; set; }
    
        public virtual CotiRCPersona CotiRCPersona { get; set; }
    }
}
