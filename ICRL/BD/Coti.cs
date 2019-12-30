using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICRL.BD
{
  public class Coti
  {
    public int idCotizacion { get; set; }
    public int idUsuario { get; set; }
    public int idFlujo { get; set; }
    public DateTime fechaCreacion { get; set; }
    public string inspector { get; set; }
    public string sucursal { get; set; }
    public int idInspeccion { get; set; }
    public int estado { get; set; }
    public int tipoCobertura { get; set; }
    public int correlativo { get; set; }
    public string tipoTaller { get; set; }
    public string TextoNroCotizacion { get; set; }
  }
}