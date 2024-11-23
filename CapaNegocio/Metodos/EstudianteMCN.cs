using CapaDatos.Metodos.Datos;
using CapaDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace CapaNegocio.Metodos
{
    public class EstudianteMCN
    {
        private readonly EstudianteMCD _estudianteMCD;

        public EstudianteMCN()
        {
            _estudianteMCD = new EstudianteMCD(); // Inicialización de la capa de datos
        }


        // Agrega un estudiante al sistema, validando que no exista previamente.
        public bool InsertarEstudiante(EstudianteCN estudianteCN)
        {
            if (estudianteCN == null)
                throw new ArgumentNullException(nameof(estudianteCN), "El objeto estudiante no puede ser nulo.");

            if (string.IsNullOrEmpty(estudianteCN.Carnet) || string.IsNullOrEmpty(estudianteCN.Nombre))
                throw new ArgumentException("El carnet y los nombres del estudiante son obligatorios.");

            // Convertir EstudianteCN a Estudiante
            Estudiante estudiante = new Estudiante
            {
                Carnet = estudianteCN.Carnet,
                Nombre = estudianteCN.Nombre,
                Apellido = estudianteCN.Apellido,
                Direccion = estudianteCN.Direccion,
                Telefono = estudianteCN.Telefono,
                FechaNacimiento = (DateTime)estudianteCN.FechaNacimiento,
                Idmonografia = estudianteCN.Idmonografia
            };

            return _estudianteMCD.AgregarEstudiante(estudiante);
        }


        // Devuelve una lista de todos los estudiantes registrados.
        public List<EstudianteCN> ListarEstudiantes()
        {
            var estudiantes = _estudianteMCD.ListarEstudiantes().
                Select(e => new EstudianteCN
            {
                Carnet = e.Carnet,
                Nombre = e.Nombre,
                Apellido = e.Apellido,
                Direccion = e.Direccion,
                Telefono = e.Telefono.HasValue ? (int?)e.Telefono.Value : null,
                FechaNacimiento = e.FechaNacimiento,
                Idmonografia = e.Idmonografia
            }).ToList();

            return estudiantes;
        }

        // Lista los estudiantes asociados a una monografía específica.
        public List<EstudianteCN> ListarEstudiantesPorMonografia(int idMonografia)
        {
            var estudiantes = _estudianteMCD.EstudiantesPorMonografia(idMonografia)
                                             .Select(e => new EstudianteCN
                                             {
                                                 Carnet = e.Carnet,
                                                 Nombre = e.Nombre,
                                                 Apellido = e.Apellido,
                                                 Direccion = e.Direccion,
                                                 Telefono = e.Telefono.HasValue ? (int?)e.Telefono.Value : null,
                                                 FechaNacimiento = e.FechaNacimiento

                                             })
                                             .ToList();
            return estudiantes;
        }



        // Obtiene la monografía asociada a un estudiante específico.
        public DataTable ObtenerMonografiaPorEstudiante(string carnet)
        {
            var monografia = _estudianteMCD.MonografiaPorEstudiante(carnet); // Llama a la Capa de Datos
            DataTable dt = new DataTable();
            dt.Columns.Add("IdMonografia");
            dt.Columns.Add("Titulo");
            dt.Columns.Add("FechaDefensa");
            dt.Columns.Add("NotaDefensa");
            dt.Columns.Add("TiempoOtorgado");
            dt.Columns.Add("TiempoDefensa");
            dt.Columns.Add("TiempoPreguntas");

            if (monografia != null)
            {
                dt.Rows.Add(monografia.Idmonografia, monografia.Titulo, monografia.FechaDefensa,
                            monografia.Notadefensa, monografia.Tiempo_Otorgado, monografia.Tiempo_Defensa,
                            monografia.Tiempo_Preguntas);
            }

            return dt; // Devuelve el DataTable con la monografía
        }

        // Busca estudiantes por nombre o carnet.
        public List<EstudianteCN> BuscarEstudiantePorNombre(string criterio)

        {
            if (string.IsNullOrEmpty(criterio))
                throw new ArgumentException("El criterio de búsqueda no puede estar vacío.");

            // Llamar al método de la Capa de Datos y mapear a EstudianteDTO
            var estudiante = _estudianteMCD.BuscarEstudiantePorNombre(criterio);
            return estudiante.Select(e => new EstudianteCN
            {
                Carnet = e.Carnet,
                Nombre = e.Nombre,
                Apellido = e.Apellido
            }).ToList();
        }
    }
}
