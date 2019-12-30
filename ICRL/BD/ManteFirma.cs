using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICRL.BD
{
  public class ManteFirma
  {
    public int idUsuario { get; set; }
    public int estado { get; set; }
    public int usuarioCreacion { get; set; }
    public DateTime fechaCreacion { get; set; }
    public byte[] firmaSello { get; set; }
  }
}