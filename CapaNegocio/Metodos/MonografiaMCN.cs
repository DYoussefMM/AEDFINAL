using CapaDatos.Metodos;
using CapaDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos.Metodos.Datos;

namespace CapaNegocio.Metodos
{
    public class MonografiaMCN
    {
        private readonly MonografiaManager _monografiaManager;
        private readonly EstudianteMCD _monografiaStd;


        public MonografiaMCN()
        {
            _monografiaManager = new MonografiaManager(); // Inicializa la capa de datos
            _monografiaStd = new EstudianteMCD();

        }

        // Agrega una nueva monografía al sistema.
        public bool InsertarMonografia(MonografiaCN mon, Monografia_ProfesorCN[] promon, EstudianteCN[] estudiantes)
        {
            // Mapeo de la Monografía desde CN a la entidad de datos
            Monografia monOriginal = new Monografia
            {
                Titulo = mon.Titulo,
                FechaDefensa = mon.FechaDefensa,
                Notadefensa = mon.Notadefensa,
                Tiempo_Otorgado = mon.Tiempo_Otorgado,
                Tiempo_Defensa = mon.Tiempo_Defensa,
                Tiempo_Preguntas = mon.Tiempo_Preguntas
            };

            // Mapeo de Monografía_Profesor desde CN a la entidad de datos
            Monografia_Profesor[] promonOriginal = promon.Select(p => new Monografia_Profesor
            {
                Idprofesor = p.Idprofesor,
                Rol = p.Rol
            }).ToArray();

            // Inserta la Monografía y Monografía_Profesor
            bool resultado = _monografiaManager.InsertarMonografia(monOriginal, promonOriginal);

            if (!resultado)
                return false;

            // Asociar los estudiantes a la Monografía
            foreach (var estudianteCN in estudiantes)
            {
                // Mapear el estudiante desde CN a la entidad de datos
                Estudiante estudiante = new Estudiante
                {
                    Carnet = estudianteCN.Carnet,
                    Idmonografia = monOriginal.Idmonografia // Se usa el ID de la monografía recién insertada
                };

                // Agregar la monografía al estudiante
                if (!_monografiaStd.AgregarMonografia(estudiante))
                {
                    // Si falla la asociación, devuelve falso
                    return false;
                }
            }

            return true; // Si todo es exitoso, devuelve true
        }



        // Lista todas las monografías registradas.
        public List<MonografiaCN> ListarMonografias()
        {
            var monografias = _monografiaManager.ListarMonografia();
            return monografias.Select(m => new MonografiaCN
            {
                Idmonografia = m.Idmonografia,
                Titulo = m.Titulo,
                FechaDefensa = m.FechaDefensa,
                Notadefensa = m.Notadefensa,
                Tiempo_Otorgado = m.Tiempo_Otorgado,
                Tiempo_Defensa = m.Tiempo_Defensa,
                Tiempo_Preguntas = m.Tiempo_Preguntas
            }).ToList();
        }

        // Filtra las monografías defendidas en un rango de fechas y devuelve detalles.
        public IEnumerable<dynamic> FiltrarDatosMonografia(DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaInicio > fechaFin)
                throw new ArgumentException("La fecha de inicio no puede ser mayor que la fecha final.");

            return _monografiaManager.FiltrarDatosMonografia(fechaInicio, fechaFin);
        }
    }
}
