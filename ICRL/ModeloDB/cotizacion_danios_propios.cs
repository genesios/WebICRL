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
    
    public partial class cotizacion_danios_propios
    {
        public int id_flujo { get; set; }
        public int id_cotizacion { get; set; }
        public long id_item { get; set; }
        public short id_tipo_item { get; set; }
        public string item_descripcion { get; set; }
        public string chaperio { get; set; }
        public string reparacion_previa { get; set; }
        public Nullable<bool> mecanico { get; set; }
        public Nullable<bool> pintura { get; set; }
        public Nullable<bool> instalacion { get; set; }
        public string id_moneda { get; set; }
        public Nullable<double> precio_cotizado { get; set; }
        public string id_tipo_descuento { get; set; }
        public Nullable<double> descuento { get; set; }
        public Nullable<double> precio_final { get; set; }
        public string proveedor { get; set; }
        public Nullable<double> monto_orden { get; set; }
        public string id_tipo_descuento_orden { get; set; }
        public Nullable<double> descuento_proveedor { get; set; }
        public Nullable<double> deducible { get; set; }
        public Nullable<double> monto_final { get; set; }
        public Nullable<bool> recepcion { get; set; }
        public Nullable<int> dias_entrega { get; set; }
        public Nullable<System.DateTime> fecha_envio_efectivo { get; set; }
        public string numero_orden { get; set; }
        public Nullable<short> id_estado { get; set; }
        public Nullable<double> tipo_cambio { get; set; }
    }
}
