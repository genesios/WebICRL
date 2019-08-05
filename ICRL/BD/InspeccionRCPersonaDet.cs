using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICRL.BD
{
    public class InspeccionRCPersonaDet
    {
        public int secuencial { get; set; }
        public string tipoPerDet { get; set; }
        public decimal montoGastoPerDet { get; set; }
        public string descripPerDet { get; set; }
    }
}