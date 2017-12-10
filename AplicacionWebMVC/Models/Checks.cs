using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplicacionWebMVC.Models
{
    public class Checks
    {
        public bool trabajoSindicato { get; set; }
        public bool retencionImpuesto { get; set; }
        public bool altura { get; set; }
        public bool espaciosConfinados { get; set; }
        public bool electrico { get; set; }
        public bool corte { get; set; }
        public bool soldadura { get; set; }
        public bool izajes { get; set; }
        public bool montacarga { get; set; }

    }
}