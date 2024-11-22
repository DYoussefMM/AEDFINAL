using CapaNegocio;
using CapaNegocio.Metodos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AEDFINAL
{
        public partial class FormConsultas : Form
        {
            private readonly MonografiaMCN monografiaService;
            private readonly EstudianteMCN estudianteService;
            private readonly ProfesorMCN profesorService;
            private readonly MonografiaProfesorMCN monService;

            public FormConsultas()
            {
                InitializeComponent();
                monografiaService = new MonografiaMCN();
                estudianteService = new EstudianteMCN();
                profesorService = new ProfesorMCN();
                monService = new MonografiaProfesorMCN();
            }

        private void CargarDatos(string tipoConsulta, string filtro = "", DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            DataTable dt = new DataTable();

            switch (tipoConsulta)
            {
                case "Monografias":
                    dt = ObtenerMonografias();
                    break;
                case "MonografiasPorFecha":
                    if (fechaInicio.HasValue && fechaFin.HasValue)
                    {
                        var monografias = monografiaService.FiltrarDatosMonografia(fechaInicio.Value, fechaFin.Value); // IEnumerable<dynamic>
                        dt = ConvertToDataTable(monografias); // Convertir a DataTable
                    }
                    break;
                case "MonografiasPorTutor":
                    int tutorId;
                    if (int.TryParse(filtro, out tutorId))
                    {
                        var monografiasPorTutor = monService.MonografiasPorTutor(tutorId, fechaInicio.Value, fechaFin.Value); // List<MonografiaCN>
                        dt = ConvertToDataTable(monografiasPorTutor); // Convertir a DataTable
                    }
                    break;
                case "EstudiantesPorMonografia":
                    int tutorId2;
                    if (int.TryParse(filtro, out tutorId2))
                    {
                        var estudiantepormono = estudianteService.ListarEstudiantesPorMonografia(tutorId2);
                        dt = ConvertToDataTable(estudiantepormono);
                    }
                    break;
                case "MonografiaPorEstudiante":
                    dt = estudianteService.ObtenerMonografiaPorEstudiante(filtro);
                    break;
                default:
                    MessageBox.Show("Consulta no reconocida.");
                    break;
            }

            dgvMostrarMonos.DataSource = dt;
            AjustarColumnas();
        }

        // Método para obtener todas las monografías desde la Capa de Negocio
        private DataTable ObtenerMonografias()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("indMonografia");
            dt.Columns.Add("Titulo");
            dt.Columns.Add("Fecha Defendida");
            dt.Columns.Add("Nota Defensa");
            dt.Columns.Add("Tiempo Otorgado");
            dt.Columns.Add("Tiempo de Defensa");
            dt.Columns.Add("Tiempo de Pregunta y Respuesta");

            var monografias = monografiaService.ListarMonografias();  // Llamada a la Capa de Negocio
            monografias.ForEach(m =>
            {
                dt.Rows.Add(m.Idmonografia, m.Titulo, m.FechaDefensa, m.Notadefensa, m.Tiempo_Otorgado, m.Tiempo_Defensa, m.Tiempo_Preguntas);
            });

            return dt;
        }

        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            DataTable table = new DataTable(typeof(T).Name);

            // Verificar si hay datos
            if (data == null || data.Count == 0)
                return table; // Retornar un DataTable vacío si no hay datos

            // Obtener las propiedades públicas de la clase T
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Usar un HashSet para evitar duplicados
            HashSet<string> columnsAdded = new HashSet<string>();

            // Agregar columnas solo si hay datos
            foreach (T item in data)
            {
                foreach (PropertyInfo prop in properties)
                {
                    var value = prop.GetValue(item);
                    if (value != null && !columnsAdded.Contains(prop.Name))
                    {
                        table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                        columnsAdded.Add(prop.Name); // Marcar la columna como agregada
                    }
                }
            }

            // Llenar el DataTable con los datos
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyInfo prop in properties)
                {
                    if (table.Columns.Contains(prop.Name))
                    {
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    }
                }
                table.Rows.Add(row);
            }

            return table;
        }

        private DataTable ConvertToDataTable(IEnumerable<dynamic> data)
        {
            DataTable dt = new DataTable();

            // Verificar si hay datos
            if (data == null || !data.Any())
                return dt; // Devuelve un DataTable vacío si no hay datos

            // Obtener las propiedades dinámicas del primer elemento
            var firstElement = data.First();
            var properties = firstElement.GetType().GetProperties();

            // Usar un HashSet para evitar duplicados
            HashSet<string> columnsAdded = new HashSet<string>();

            // Agregar columnas solo si hay datos
            foreach (var item in data)
            {
                foreach (var property in properties)
                {
                    var value = property.GetValue(item);
                    if (value != null && !columnsAdded.Contains(property.Name))
                    {
                        dt.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
                        columnsAdded.Add(property.Name); // Marcar la columna como agregada
                    }
                }
            }

            // Llenar el DataTable con los datos
            foreach (var item in data)
            {
                DataRow row = dt.NewRow();
                foreach (var property in properties)
                {
                    if (dt.Columns.Contains(property.Name))
                    {
                        row[property.Name] = property.GetValue(item) ?? DBNull.Value;
                    }
                }
                dt.Rows.Add(row);
            }

            return dt;
        }


        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            CargarDatos("MonografiasPorTutor", filtro: txtBuscar.Text, fechaInicio: dtpFechaInicio.Value, fechaFin: dtpFechaFin.Value);
        }

        private void txtidmonografia_TextChanged(object sender, EventArgs e)
        {
            CargarDatos("EstudiantesPorMonografia", filtro: txtidmonografia.Text);
        }

        private void txtcarnet_TextCha0nged(object sender, EventArgs e)
        {
            // Llamar al método CargarDatos con el tipo de consulta "MonografiaPorEstudiante" y el filtro del carnet
            CargarDatos("MonografiaPorEstudiante", filtro: txtcarnet.Text);

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (dtpFechaInicio.Value <= dtpFechaFin.Value)
                CargarDatos("MonografiasPorFecha", fechaInicio: dtpFechaInicio.Value, fechaFin: dtpFechaFin.Value);
            else
                MessageBox.Show("La fecha de inicio no puede ser mayor que la fecha final.");
        }

        private void btnListar_Click(object sender, EventArgs e)
        {
            CargarDatos("Monografias");
        }

        private void dtpFechaInicio_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnLimpiar_Click_1(object sender, EventArgs e)
        {
            dgvMostrarMonos.DataSource = null;
            dgvMostrarMonos.Rows.Clear();

            // Limpiar los TextBox
            txtBuscar.Clear();
            txtidmonografia.Clear();
            txtcarnet.Clear();

            // Limpiar los DateTimePicker (si los estás usando)
            dtpFechaInicio.Value = DateTime.Now;
            dtpFechaFin.Value = DateTime.Now;
        }
        private void AjustarColumnas()
        {
            dgvMostrarMonos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Las columnas llenarán el ancho disponible
            dgvMostrarMonos.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;  // Ajusta las filas al contenido

            // Opcional: Configuración individual para columnas específicas
            foreach (DataGridViewColumn columna in dgvMostrarMonos.Columns)
            {
                columna.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; // Cada columna se ajusta a su contenido
            }
        }


        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
