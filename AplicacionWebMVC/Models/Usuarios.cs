//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AplicacionWebMVC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Usuarios
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Usuarios()
        {
            this.DetallesUsuarios = new HashSet<DetallesUsuarios>();
            this.DetallesUsuarios2 = new HashSet<DetallesUsuarios2>();
            this.UsuarioDepartamento = new HashSet<UsuarioDepartamento>();
        }
    
        public int idUsuario { get; set; }
        public string nombreUsuario { get; set; }
        public string contrasena { get; set; }
        public string idEmpleado { get; set; }
        public string registradoPor { get; set; }
        public string Role { get; set; }
        public Nullable<int> departamento { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetallesUsuarios> DetallesUsuarios { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetallesUsuarios2> DetallesUsuarios2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UsuarioDepartamento> UsuarioDepartamento { get; set; }
    }
}
