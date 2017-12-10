namespace AplicacionWebMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Usuario
    {
        public string nombreUsuario { get; set; }
        public string contrasena { get; set; }
        public int idUsuario { get; set; }
        public string idEmpleado { get; set; }
        public string registradoPor { get; set; }
        public string Role { get; set; }
    }
}