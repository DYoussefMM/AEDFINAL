//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CapaDatos
{
    using System;
    using System.Collections.Generic;
    
    public partial class Monografia
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Monografia()
        {
            this.Estudiante = new HashSet<Estudiante>();
            this.Monografia_Profesor = new HashSet<Monografia_Profesor>();
        }
    
        public int Idmonografia { get; set; }
        public string Titulo { get; set; }
        public int Tiempo_Preguntas { get; set; }
        public int Tiempo_Defensa { get; set; }
        public int Tiempo_Otorgado { get; set; }
        public int Notadefensa { get; set; }
        public System.DateTime FechaDefensa { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Estudiante> Estudiante { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Monografia_Profesor> Monografia_Profesor { get; set; }
    }
}
