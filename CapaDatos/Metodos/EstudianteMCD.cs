using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Metodos 
{ 
        using System.Collections.Generic;
        using System.Linq;

namespace Datos
{
    public class EstudianteMCD
    {
  

        //Agrega un nuevo estudiante a la base de datos.
        public bool AgregarEstudiante(Estudiante estudiante)
        {
            using (var db = new DatosMonEntities())
            {
                if (db.Estudiante.Any(e => e.Carnet == estudiante.Carnet))
                    return false;

                db.Estudiante.Add(estudiante);
                db.SaveChanges();
                return true;
            }
        }

            // Asocia una monografía a un estudiante existente.
            public bool AgregarMonografia(Estudiante estudiante)
        {
            using (var db = new DatosMonEntities())
                { // Verificar que el estudiante exista
                    var estudianteExistente = db.Estudiante.FirstOrDefault(e => e.Carnet == estudiante.Carnet);
                    if (estudianteExistente == null)
                        throw new Exception($"El estudiante con carnet {estudiante.Carnet} no existe.");

                    // Verificar que la monografía exista
                    var monografiaExistente = db.Monografia.FirstOrDefault(m => m.Idmonografia == estudiante.Idmonografia);
                    if (monografiaExistente == null)
                        throw new Exception($"La monografía con ID {estudiante.Idmonografia} no existe.");

                    // Asociar la monografía al estudiante
                    estudianteExistente.Idmonografia = estudiante.Idmonografia;

                    db.SaveChanges();
                    return true;
                }
        }

        // Lista todos los estudiantes en la base de datos.
        public List<Estudiante> ListarEstudiantes()
        {
            using (var db = new DatosMonEntities())
            {
                    var consulta = (from e in db.Estudiante select e).ToList();
                    return consulta;

            }
        }


            // Devuelve un estudiante específico por su carnet.

            public List<Estudiante> BuscarEstudiantePorNombre(string carnetNombre)
            {
                using (var db = new DatosMonEntities())
                {
                    return db.Estudiante
                        .Where(es => es.Carnet.Contains(carnetNombre) || es.Nombre.Contains(carnetNombre))
                        .ToList();
                }
            }

            // Lista estudiantes asociados a una monografía específica.
            public List<Estudiante> EstudiantesPorMonografia(int idMonografia)
            {
                using (var db = new DatosMonEntities())
                {
                    return db.Estudiante.Where(e => e.Idmonografia == idMonografia).ToList();
                }
            }

        //Devuelve la monografía asociada a un estudiante específico.
        public Monografia MonografiaPorEstudiante(string carnet)
        {
            using (var db = new DatosMonEntities())
            {
                var monografia = (from est in db.Estudiante
                                  join mon in db.Monografia
                                  on est.Idmonografia equals mon.Idmonografia
                                  where est.Carnet == carnet
                                  select mon).FirstOrDefault();

                return monografia;
            }
        }
    }
}

}
