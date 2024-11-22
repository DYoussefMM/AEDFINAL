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
    public partial class FormProfesor : Form
    {

        private readonly ProfesorMCN profesorService;
        public FormProfesor()
        {
            InitializeComponent();
            profesorService = new ProfesorMCN(); // Inicializa la capa de negocio
            CargarDgvProfesor();
        }

        // Método para limpiar los campos del formulario
        public void Limpiar()
        {
            txtNombres.Clear();
            txtApellidos.Clear();
            txtDireccion.Clear();
            txtTelefono.Clear();
            dtpFechaNacimiento.Value = DateTime.Now;
        }

        // Convierte la lista de profesores en un DataTable para el DataGridView
        public DataTable Lista_DT_Profesores()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("Nombres");
            dt.Columns.Add("Apellidos");
            dt.Columns.Add("Telefono");
            dt.Columns.Add("Direccion");
            dt.Columns.Add("Año de Nacimiento");

            var profesores = profesorService.ListarProfesores(); // Llama a la capa de negocio
            profesores.ForEach(e =>
            {
                dt.Rows.Add( e.Idprofesor, e.Nombre, e.Apellido, e.Telefono, e.Direccion, e.FechaNacimiento.ToShortDateString());
            });

            return dt;
        }

        // Carga los datos de los profesores en el DataGridView
        public void CargarDgvProfesor()
        {
            dgvMostrarProf.DataSource = null;
            dgvMostrarProf.DataSource = Lista_DT_Profesores();
        }

        private void btnRegistrarProf_Click_1(object sender, EventArgs e)
        {
            try
            {
                ProfesorCN prof = new ProfesorCN
                {
                    Nombre = txtNombres.Text,
                    Apellido = txtApellidos.Text,
                    Direccion = txtDireccion.Text,
                    Telefono = int.Parse(txtTelefono.Text),
                    FechaNacimiento = dtpFechaNacimiento.Value
                };

                bool resultado = profesorService.InsertarProfesor(prof); // Llama a la capa de negocio

                if (resultado)
                {
                    MessageBox.Show("Profesor registrado con éxito.");
                    Limpiar();
                    CargarDgvProfesor();
                }
                else
                {
                    MessageBox.Show("El profesor ya existe.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar profesor: {ex.Message}");
            }
        }
    }
}
