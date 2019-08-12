﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class LBCDesaEntities : DbContext
    {
        public LBCDesaEntities()
            : base("name=LBCDesaEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Asegurado> Asegurado { get; set; }
        public virtual DbSet<Contador> Contador { get; set; }
        public virtual DbSet<CotiDaniosPropios> CotiDaniosPropios { get; set; }
        public virtual DbSet<CotiPerdidaTotal> CotiPerdidaTotal { get; set; }
        public virtual DbSet<CotiPerdidaTotalDuenio> CotiPerdidaTotalDuenio { get; set; }
        public virtual DbSet<CotiPerdidaTotalReferencia> CotiPerdidaTotalReferencia { get; set; }
        public virtual DbSet<CotiRCObjeto> CotiRCObjeto { get; set; }
        public virtual DbSet<CotiRCObjetoDetalle> CotiRCObjetoDetalle { get; set; }
        public virtual DbSet<CotiRCPersona> CotiRCPersona { get; set; }
        public virtual DbSet<CotiRCPersonaDetalle> CotiRCPersonaDetalle { get; set; }
        public virtual DbSet<CotiReparacion> CotiReparacion { get; set; }
        public virtual DbSet<CotiReparacionRepuesto> CotiReparacionRepuesto { get; set; }
        public virtual DbSet<CotiRepuesto> CotiRepuesto { get; set; }
        public virtual DbSet<Cotizacion> Cotizacion { get; set; }
        public virtual DbSet<cotizacion_danios_propios> cotizacion_danios_propios { get; set; }
        public virtual DbSet<cotizacion_danios_propios_sumatoria> cotizacion_danios_propios_sumatoria { get; set; }
        public virtual DbSet<cotizacion_perdida_total_propio> cotizacion_perdida_total_propio { get; set; }
        public virtual DbSet<cotizacion_perdida_total_robo> cotizacion_perdida_total_robo { get; set; }
        public virtual DbSet<cotizacion_rc_objetos> cotizacion_rc_objetos { get; set; }
        public virtual DbSet<cotizacion_rc_personas> cotizacion_rc_personas { get; set; }
        public virtual DbSet<cotizacion_rc_vehicular> cotizacion_rc_vehicular { get; set; }
        public virtual DbSet<cotizacion_rc_vehicular_sumatoria> cotizacion_rc_vehicular_sumatoria { get; set; }
        public virtual DbSet<cotizacion_robo_parcial> cotizacion_robo_parcial { get; set; }
        public virtual DbSet<cotizacion_robo_parcial_sumatoria> cotizacion_robo_parcial_sumatoria { get; set; }
        public virtual DbSet<CotizacionFlujo> CotizacionFlujo { get; set; }
        public virtual DbSet<EstadoSesion> EstadoSesion { get; set; }
        public virtual DbSet<Flujo> Flujo { get; set; }
        public virtual DbSet<InspDaniosPropios> InspDaniosPropios { get; set; }
        public virtual DbSet<InspDaniosPropiosPadre> InspDaniosPropiosPadre { get; set; }
        public virtual DbSet<Inspeccion> Inspeccion { get; set; }
        public virtual DbSet<InspPerdidaTotalDanios> InspPerdidaTotalDanios { get; set; }
        public virtual DbSet<InspPerdidaTotalRobo> InspPerdidaTotalRobo { get; set; }
        public virtual DbSet<InspRCObjeto> InspRCObjeto { get; set; }
        public virtual DbSet<InspRCObjetoDetalle> InspRCObjetoDetalle { get; set; }
        public virtual DbSet<InspRCPersona> InspRCPersona { get; set; }
        public virtual DbSet<InspRCPersonaDetalle> InspRCPersonaDetalle { get; set; }
        public virtual DbSet<InspRCVehicular> InspRCVehicular { get; set; }
        public virtual DbSet<InspRCVehicularDetalle> InspRCVehicularDetalle { get; set; }
        public virtual DbSet<InspRoboParcial> InspRoboParcial { get; set; }
        public virtual DbSet<ItemTaller> ItemTaller { get; set; }
        public virtual DbSet<Liquidacion> Liquidacion { get; set; }
        public virtual DbSet<LiquidacionDetalle> LiquidacionDetalle { get; set; }
        public virtual DbSet<LiquidacionFacturas> LiquidacionFacturas { get; set; }
        public virtual DbSet<Nomenclador> Nomenclador { get; set; }
        public virtual DbSet<Proveedor> Proveedor { get; set; }
        public virtual DbSet<Rol> Rol { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<UsuarioRol> UsuarioRol { get; set; }
        public virtual DbSet<cotizacion_rc_vehicular_tercero> cotizacion_rc_vehicular_tercero { get; set; }
    
        public virtual int paIncFlujoContadorInsp(Nullable<int> iIdFlujo, ObjectParameter iValorContador)
        {
            var iIdFlujoParameter = iIdFlujo.HasValue ?
                new ObjectParameter("iIdFlujo", iIdFlujo) :
                new ObjectParameter("iIdFlujo", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("paIncFlujoContadorInsp", iIdFlujoParameter, iValorContador);
        }
    
        public virtual int paIncrementaContador(string cTipo, ObjectParameter biValorContador)
        {
            var cTipoParameter = cTipo != null ?
                new ObjectParameter("cTipo", cTipo) :
                new ObjectParameter("cTipo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("paIncrementaContador", cTipoParameter, biValorContador);
        }
    }
}
