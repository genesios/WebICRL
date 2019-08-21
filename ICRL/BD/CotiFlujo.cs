using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICRL.BD
{
  public class CotiFlujo
  {
    public int idFlujo { get; set; }
    public int idUsuario { get; set; }
    public DateTime fechaCreacion { get; set; }
    public string observacionesSiniestro { get; set; }
     public string inspector { get; set; }
    public string nombreContacto { get; set; }
    public string telefonoContacto { get; set; }
    public string correosDeEnvio { get; set; }
    public int estado { get; set; }
    public DateTime fechaSiniestro { get; set; }
    public int correlativo { get; set; }
    public int usuario_modificacion { get; set; }
    public DateTime fechaModificacion { get; set; }

  }
}