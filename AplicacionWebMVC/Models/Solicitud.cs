using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplicacionWebMVC.Models
{
    public class Solicitud
    {
        public string preRequisicion { get; set; }
        public string idRequisicion { get; set; }
        public string fechaRequisicion { get; set; }
        public string uso { get; set; }
        public string fechaNecesitar{ get; set; }
        public int departamento { get; set; }
        public string observaciones { get; set; }
        public char ciclo { get; set; }
        public char area { get; set; }
        public string fechaRecepcion { get; set; }
        public int ejercicio { get; set; }
        public int solicitante { get; set; }
    }
}