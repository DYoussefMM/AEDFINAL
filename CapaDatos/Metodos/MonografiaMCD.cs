using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Metodos
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class MonografiaManager
    {

   

        // Agrega una nueva monografía si no existe.
        public bool InsertarMonografia(Monografia mon, Monografia_Profesor[] profesores)
        {
            using (var db = new DatosMonEntities())
            {
                var m = db.Monografia.FirstOrDefault(x => x.Idmonografia == mon.Idmonografia);
                if (m != null)
                    return false;

                db.Monografia.Add(mon);
                db.SaveChanges();

                int idMonografia = mon.Idmonografia;

                foreach (var relacion in profesores)
                {
                    relacion.Idmonografia = idMonografia;
                    db.Monografia_Profesor.Add(relacion);
                }

                db.SaveChanges();
                return true;
            }
        }


        // Lista todas las monografías.
        public List<Monografia> ListarMonografia()
        {
            using (var dbContext = new DatosMonEntities())
            {
                return dbContext.Monografia.ToList();
            }
        }

        // Lista monografías por código.
        public List<Monografia> ListarMonografia(int codigo)
        {
            using (var dbContext = new DatosMonEntities())
            {
                return dbContext.Monografia
                    .Where(e => e.Idmonografia == codigo)
                    .ToList();
            }
        }

        // Lista monografías defendidas en un rango de fechas.
        public IEnumerable<dynamic> FiltrarDatosMonografia(DateTime fechaInicio, DateTime fechaFinal)
        {
            using (var db = new DatosMonEntities())
            {
                var consultaFiltrada =
                from mon in db.Monografia
                join proMon in db.Monografia_Profesor
                on mon.Idmonografia equals proMon.Idmonografia
                join pro in db.Profesor
                on proMon.Idprofesor equals pro.Idprofesor
                where DbFunctions.TruncateTime(mon.FechaDefensa) >= DbFunctions.TruncateTime(fechaInicio)
                      && DbFunctions.TruncateTime(mon.FechaDefensa) <= DbFunctions.TruncateTime(fechaFinal)
                      && proMon.Rol.Trim().Equals("Tutor", StringComparison.OrdinalIgnoreCase)
                select new
                {
                    TituloMonografia = mon.Titulo,
                    Tutor = pro.Nombre,
                    FechaD = mon.FechaDefensa,
                    Estudiantes = db.Estudiante
                                    .Where(est => est.Idmonografia == mon.Idmonografia)
                                    .Take(3)
                                    .Select(est => est.Nombre)
                                    .ToList()
                };

                // Verificar si hay datos
                var consultaLista = consultaFiltrada.ToList();
                if (!consultaLista.Any())
                {
                    return Enumerable.Empty<dynamic>(); // Retorna una lista vacía
                }

                // Procesar los datos en memoria
                var resultado = consultaLista.Select(x => new
                {
                    x.TituloMonografia,
                    x.Tutor,
                    x.FechaD,
                    NombreEstudiantes = string.Join(", ", x.Estudiantes)
                });

                return resultado.ToList(); // Retorna la lista procesada
            }
        }



        // Filtra monografías por rango de fechas.
        public List<Monografia> MonografiasRangoFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            using (var dbContext = new DatosMonEntities())
            {
                return dbContext.Monografia
                    .Where(m => m.FechaDefensa >= fechaInicio && m.FechaDefensa <= fechaFin)
                    .ToList();
            }
        }
    }
}
