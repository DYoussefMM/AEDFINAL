using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Metodos
{
    public class MonografiaProfesorMCD
    {
        //// Agrega un rol de profesor en una monografía.
        //public bool AgregarRol(Monografia_Profesor monografiaProfesor)
        //{
        //    using (var db = new DatosMonEntities())
        //    {
        //        db.Monografia_Profesor.Add(monografiaProfesor);
        //        db.SaveChanges();
        //        return true;
        //    }
        //}

        // Lista todos los datos de la relación Monografía_Profesor.
        public List<Monografia_Profesor> ListarDatos()
        {
            using (var db = new DatosMonEntities())
            {
                return db.Monografia_Profesor.ToList();
            }
        }

        // Lista las monografías en las que un profesor actúa como tutor.
        public List<Monografia> ListarMonografiasPorTutor(int idTutor)
        {
            using (var db = new DatosMonEntities())
            {
                // Verifica que solo se seleccionen monografías con el rol de tutor
                var titulo = (from proMon in db.Monografia_Profesor
                                   join mon in db.Monografia
                                   on proMon.Idmonografia equals mon.Idmonografia
                                   where proMon.Idprofesor == idTutor
                                         && proMon.Rol.Trim().Equals("Tutor", StringComparison.OrdinalIgnoreCase)
                                   select mon).ToList();

                return titulo;
            }
        }


        // Lista las monografías en las que un profesor actúa como tutor en un rango de fechas.
        public List<Monografia> MonografiasPorTutor(int idTutor, DateTime fechaInicio, DateTime fechaFin)
        {
            using (var db = new DatosMonEntities())
            {
                var monografias = (from proMon in db.Monografia_Profesor
                                   join mon in db.Monografia on proMon.Idmonografia equals mon.Idmonografia
                                   where proMon.Idprofesor == idTutor
                                         && proMon.Rol == "Tutor"
                                         && mon.FechaDefensa >= fechaInicio
                                         && mon.FechaDefensa <= fechaFin
                                   select mon).ToList();

                return monografias;
            }
        }
    }
}
