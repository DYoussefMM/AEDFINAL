using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class MonografiaCN
    {
        public int Idmonografia { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaDefensa { get; set; }
        public int Tiempo_Preguntas { get; set; }
        public int Tiempo_Defensa { get; set; }
        public int Tiempo_Otorgado { get; set; }
        public int Notadefensa { get; set; }

    }
}
