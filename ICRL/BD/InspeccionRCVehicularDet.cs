﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICRL.BD
{
    public class InspeccionRCVehicularDet
    {
        public string idItem { get; set; }
        public int secuencial { get; set; }
        public string compra { get; set; }
        public bool instalacion { get; set; }
        public bool pintura { get; set; }
        public bool mecanico { get; set; }
        public string chaperio { get; set; }
        public string reparacionPrevia { get; set; }
        public string observaciones { get; set; }
        public long nro_item { get; set; }
  }
}