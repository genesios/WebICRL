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
    
    public partial class CotiReparacionRepuesto
    {
        public int idCotizacion { get; set; }
        public string idOrden { get; set; }
        public string tipoOrden { get; set; }
        public string proveedor { get; set; }
        public Nullable<decimal> montoOrden { get; set; }
        public string monedaOrden { get; set; }
        public Nullable<int> estadoOrden { get; set; }
        public string descFijoPorcentaje { get; set; }
        public Nullable<decimal> montoDescuento { get; set; }
        public Nullable<decimal> montoDeducibleFraCoa { get; set; }
        public Nullable<decimal> montoFinal { get; set; }
    
        public virtual Cotizacion Cotizacion { get; set; }
    }
}
