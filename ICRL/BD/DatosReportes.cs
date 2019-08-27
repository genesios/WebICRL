using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICRL.BD
{
  public class DatosReportes
  {
    public List<LiquidacionICRL.TipoGrilla> ObtenerDatosGrilla(int estado, string proveedor, string sucursal, string flujoon, string placa, DateTime fechai, DateTime fechaf)
    {
      LiquidacionICRL.TipoRespuestaGrilla respuestaGrilla = LiquidacionICRL.LiquidacionGrilla(
        estado, proveedor, sucursal, flujoon, placa, fechai, fechaf);

      return respuestaGrilla.listaLiquidacionGrilla;
    }
  }
}