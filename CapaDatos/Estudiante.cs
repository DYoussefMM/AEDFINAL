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
    
    public partial class Estudiante
    {
        public string Carnet { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Direccion { get; set; }
        public Nullable<long> Telefono { get; set; }
        public System.DateTime FechaNacimiento { get; set; }
        public Nullable<int> Idmonografia { get; set; }
    
        public virtual Monografia Monografia { get; set; }
    }
}
