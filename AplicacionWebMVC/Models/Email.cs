using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplicacionWebMVC.Models
{
    public class Email
    {
        public string Subject { get; set; }
        public string fechaNecesitar { get; set; }
        public string fechaRequisicion { get; set; }
        public string uso { get; set; }
        public string observaciones { get; set; }
        public string departamento { get; set; }
    }
}