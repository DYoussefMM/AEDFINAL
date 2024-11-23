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
    public class ProfesorMCN
    {
        private readonly ProfesorMCD _profesorMCD;

        public ProfesorMCN()
        {
            _profesorMCD = new ProfesorMCD(); // Inicializa la capa de datos
        }


        // Agrega un profesor al sistema, validando que no exista previamente.
        public bool InsertarProfesor(ProfesorCN pro)
        {
            //if (pro == null)
            //    throw new ArgumentNullException(nameof(pro), "El objeto profesor no puede ser nulo.");

            //if (pro.Idprofesor <= 0 || string.IsNullOrEmpty(pro.Nombre))
            //    throw new ArgumentException("El ID del profesor debe ser mayor que 0 y el nombre no puede estar vacío.");

            Profesor profesorOriginal = new Profesor
            {
                Idprofesor = pro.Idprofesor,
                Nombre = pro.Nombre,
                Apellido = pro.Apellido,
                FechaNacimiento = pro.FechaNacimiento,
                Direccion = pro.Direccion,
                Telefono = pro.Telefono
            };
            return _profesorMCD.AgregarProfesor(profesorOriginal);
        }

        // Devuelve una lista con todos los profesores registrados.
        public List<ProfesorCN> ListarProfesores()
        {
            var consulta = _profesorMCD.ListarProfesores().
                            Select(e => new ProfesorCN
                            {
                                Idprofesor = e.Idprofesor,
                                Nombre = e.Nombre,
                                Apellido = e.Apellido,
                                Direccion = e.Direccion,
                                Telefono = e.Telefono.HasValue ? (int?)e.Telefono.Value : null,
                                FechaNacimiento = e.FechaNacimiento
                            }).ToList();

            return consulta;
        }

        // Obtiene un profesor por su ID.
        public ProfesorCN ListarProfesor(int idProfesor)
        {
             var profesor = _profesorMCD.ListarProfesor(idProfesor); // Llama a la Capa de Datos

            if (profesor == null)
                return null;

            // Mapear Profesor a ProfesorDTO
            return new ProfesorCN
            {
                Idprofesor = profesor.Idprofesor,
                Nombre = profesor.Nombre,
                Apellido = profesor.Apellido
            };
        }
    }
}
