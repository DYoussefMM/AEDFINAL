using CapaDatos.Metodos;
using CapaDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Metodos
{
    public class MonografiaProfesorMCN
    {
        private readonly MonografiaProfesorMCD _monografiaProfesorMCD;

        public MonografiaProfesorMCN()
        {
            _monografiaProfesorMCD = new MonografiaProfesorMCD(); // Inicializa la capa de datos
        }

        //// Agrega un rol de profesor en una monografía.
        //public bool AgregarRol(Monografia_Profesor monografiaProfesor)
        //{
        //    if (monografiaProfesor == null)
        //        throw new ArgumentNullException(nameof(monografiaProfesor), "El objeto Monografia_Profesor no puede ser nulo.");

        //    if (string.IsNullOrEmpty(monografiaProfesor.Rol))
        //        throw new ArgumentException("El rol no puede estar vacío.");

        //    return _monografiaProfesorMCD.AgregarRol(monografiaProfesor);
        //}

        // Lista las monografías en las que un profesor actúa como tutor en un rango de fechas.
        public List<MonografiaCN> MonografiasPorTutor(int idTutor, DateTime fechaInicio, DateTime fechaFin)
        {
            var monografias = _monografiaProfesorMCD.MonografiasPorTutor(idTutor, fechaInicio, fechaFin); ; // Llamada a la Capa de Datos

            // Mapea las monografías a DTOs para ser devueltos
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
    }
}
