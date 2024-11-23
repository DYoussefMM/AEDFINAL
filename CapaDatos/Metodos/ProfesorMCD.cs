using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Metodos
{
    public class ProfesorMCD
    {

        // Agrega un profesor si no existe ya en la base de datos.
        public bool AgregarProfesor(Profesor profesor)
        {
            using (var db = new DatosMonEntities())
            {
                if (db.Profesor.Any(p => p.Idprofesor == profesor.Idprofesor))
                    return false;

                db.Profesor.Add(profesor);
                db.SaveChanges();
                return true;
            }
        }

        // Devuelve una lista con todos los profesores.
        public List<Profesor> ListarProfesores()
        {
            using (var db = new DatosMonEntities())
            {
                return db.Profesor.ToList();
            }
        }

        // Busca y devuelve un profesor específico por su ID.
        public Profesor ListarProfesor(int idProfesor)
        {
            using (var db = new DatosMonEntities())
            {
                return db.Profesor.FirstOrDefault(p => p.Idprofesor == idProfesor);
            }
        }

        // Busca y devuelve una lista de profesores específicos por su nombre.
        public List<Profesor> ListarProfesoresPorNombre(string nombreProfesor)
        {
            using (var db = new DatosMonEntities())
            {
                return db.Profesor
                         .Where(p => p.Nombre.Contains(nombreProfesor))
                         .ToList();
            }
        }

    }
}
