using CapaNegocio;
using CapaNegocio.Metodos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AEDFINAL
{
    public partial class FormEstudiante : Form
    {
        private readonly EstudianteMCN estudianteService;

        public FormEstudiante()
        {
            InitializeComponent();
            estudianteService = new EstudianteMCN(); // Instancia de la capa de negocio
            CargarDgv();
        }

        // Limpia los campos de texto
        public void Limpiar()
        {
            txtCarnet.Clear();
            txtNombres.Clear();
            txtApellidos.Clear();
            txtDireccion.Clear();
            txtTelefono.Clear();
            dtpFechaNacimiento.Value = DateTime.Now;
        }

        // Convierte la lista de estudiantes a un DataTable para el DataGridView
        public DataTable Lista_DT_Estudiantes()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Carnet");
            dt.Columns.Add("Nombres");
            dt.Columns.Add("Apellidos");
            dt.Columns.Add("Telefono");
            dt.Columns.Add("Direccion");
            dt.Columns.Add("Año de Nacimiento");
            dt.Columns.Add("Idmonografia");


            var estudiantes = estudianteService.ListarEstudiantes(); // Llama a la capa de negocio
            estudiantes.ForEach(e =>
            {
                dt.Rows.Add(e.Carnet, e.Nombre, e.Apellido, e.Telefono, e.Direccion, e.FechaNacimiento.ToShortDateString(), e.Idmonografia);
            });

            return dt;
        }

        // Carga los datos en el DataGridView
        public void CargarDgv()
        {
            dgvMostrarEsts.DataSource = null;
            dgvMostrarEsts.DataSource = Lista_DT_Estudiantes();
        }

        // Registra un nuevo estudiante
        private void btnRegistrarEst_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Crear un objeto EstudianteCN (Capa de Negocio)
                EstudianteCN est = new EstudianteCN
                {
                    Carnet = txtCarnet.Text,
                    Nombre = txtNombres.Text,
                    Apellido = txtApellidos.Text,
                    Direccion = txtDireccion.Text,
                    Telefono = int.Parse(txtTelefono.Text),
                    FechaNacimiento = dtpFechaNacimiento.Value,
                    Idmonografia = null,
                };

                bool resultado = estudianteService.InsertarEstudiante(est); // Llama a la capa de negocio

                if (resultado)
                {
                    MessageBox.Show("Estudiante registrado con éxito.");
                    Limpiar();
                    CargarDgv();
                }
                else
                {
                    MessageBox.Show("El estudiante ya existe.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar estudiante: {ex.Message}");
            }
        }
    }
}
