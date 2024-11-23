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
    public partial class FormMonografia : Form
    {
        private readonly MonografiaMCN monografiaService;
        private readonly ProfesorMCN profesorService;
        private readonly EstudianteMCN estudianteService;

        public FormMonografia()
        {
            InitializeComponent();
            monografiaService = new MonografiaMCN();
            profesorService = new ProfesorMCN();
            estudianteService = new EstudianteMCN();

            OcultarLabels();
        }

        public void OcultarLabels()
        {
            lblNombreTutor.Visible = false;
            lblNombreEstudiante1.Visible = false;
            lblNombreEstudiante2.Visible = false;
            lblNombreEstudiante3.Visible = false;
            lblNombreJurado1.Visible = false;
            lblNombreJurado2.Visible = false;
            lblNombreJurado3.Visible = false;
        }

        public void Limpiar()
        {
            txtTitulo.Clear();
            txtNota.Clear();
            txtTiempoOtorgado.Clear();
            txtTiempoDefensa.Clear();
            txtTiempoPreguntas.Clear();
            txtidtutor.Clear();
            txtidjurado1.Clear();
            txtidjurado2.Clear();
            txtidjurado3.Clear();
        }

        public bool VerificarTb()
        {
            foreach (Control control in this.Controls)
            {
                if (control is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text))
                {
                    return false;
                }
            }
            return true;
        }

        private void btnRegistrarMono_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (VerificarTb())
                {
                    // Crear el objeto MonografiaCN
                    MonografiaCN monografia = new MonografiaCN
                    {
                        Titulo = txtTitulo.Text,
                        FechaDefensa = dtpFechaMono.Value,
                        Notadefensa = int.Parse(txtNota.Text),
                        Tiempo_Otorgado = int.Parse(txtTiempoOtorgado.Text),
                        Tiempo_Defensa = int.Parse(txtTiempoDefensa.Text),
                        Tiempo_Preguntas = int.Parse(txtTiempoPreguntas.Text)
                    };



                    // Crear las relaciones Monografia_ProfesorCN
                    Monografia_ProfesorCN[] promon = new Monografia_ProfesorCN[]
                    {
                        new Monografia_ProfesorCN { Idprofesor = int.Parse(txtidtutor.Text), Rol = "Tutor" },
                        new Monografia_ProfesorCN { Idprofesor = int.Parse(txtidjurado1.Text), Rol = "Jurado" },
                        new Monografia_ProfesorCN { Idprofesor = int.Parse(txtidjurado2.Text), Rol = "Jurado" },
                        new Monografia_ProfesorCN { Idprofesor = int.Parse(txtidjurado3.Text), Rol = "Jurado" }
                    };

                    EstudianteCN[] estudiantes = new[]
                    {
                        new EstudianteCN { Carnet = txtcarnetestudiante1.Text },
                        new EstudianteCN { Carnet = txtcarnetestudiante2.Text },
                        new EstudianteCN { Carnet = txtcarnetestudiante3.Text }
                    };

                    // Llamar al servicio de la capa de negocio
                    bool resultado = monografiaService.InsertarMonografia(monografia, promon, estudiantes);

                    if (resultado)
                    {
                        MessageBox.Show("Monografía registrada con éxito.");
                        Limpiar();
                        OcultarLabels();
                    }
                    else
                    {
                        MessageBox.Show("Error al registrar la monografía.");
                    }
                }
                else
                {
                    MessageBox.Show("Todos los campos son obligatorios.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void VerificarProfesor(TextBox textBox, Label label)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    errorProvider1.SetError(textBox, "Este campo no puede estar vacío.");
                    label.Visible = false;
                }
                else if (!VerificarIdentificaciones(textBox.Text, textBox))
                {
                    errorProvider1.SetError(textBox, "No pueden repetirse identificadores (Carnet o Identificación).");
                    label.Visible = false;
                }
                else
                {
                    var profesor = profesorService.ListarProfesor(int.Parse(textBox.Text));
                    if (profesor != null)
                    {
                        errorProvider1.SetError(textBox, "");
                        label.Text = $"{profesor.Nombre} {profesor.Apellido}";  // Concatenamos nombre y apellido
                        label.Visible = true;
                    }
                    else
                    {
                        errorProvider1.SetError(textBox, "No existe.");
                        label.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(textBox, $"Error: {ex.Message}");
                label.Visible = false;
            }
        }


        private void VerificarEstudiante(TextBox textBox, Label label)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    errorProvider1.SetError(textBox, "Este campo no puede estar vacío.");
                    label.Visible = false;
                }
                else if (!VerificarIdentificaciones(textBox.Text, textBox))
                {
                    errorProvider1.SetError(textBox, "No pueden repetirse identificadores (Carnet).");
                    label.Visible = false;
                }
                else
                {
                    var estudiantes = estudianteService.BuscarEstudiantePorNombre(textBox.Text);
                    var estudiante = estudiantes.FirstOrDefault(); // Obtener el primer estudiante o null si no hay coincidencias

                    if (estudiante != null)
                    {
                        errorProvider1.SetError(textBox, "");
                        label.Text = $"{estudiante.Nombre} {estudiante.Apellido}";  // Concatenamos nombre y apellido
                        label.Visible = true;
                    }
                    else
                    {
                        errorProvider1.SetError(textBox, "No existe.");
                        label.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(textBox, $"Error: {ex.Message}");
                label.Visible = false;
            }
        }


        private void txtidjurado1_TextChanged(object sender, EventArgs e)
        {
            VerificarProfesor(txtidjurado1, lblNombreJurado1);
        }

        private bool VerificarIdentificaciones(string textoABuscar, TextBox textBoxIgnorar)
        {
            foreach (Control control in this.Controls)
            {
                if (control is TextBox textBox && textBox != textBoxIgnorar && textBox.Text == textoABuscar)
                {
                    return false;
                }
            }
            return true;
        }

        private void txtidjurado3_TextChanged_1(object sender, EventArgs e)
        {
            VerificarProfesor(txtidjurado3, lblNombreJurado3);

        }

        private void txtidjurado2_TextChanged_1(object sender, EventArgs e)
        {
            VerificarProfesor(txtidjurado2, lblNombreJurado2);

        }

        private void txtcarnetestudiante1_TextChanged_1(object sender, EventArgs e)
        {
            VerificarEstudiante(txtcarnetestudiante1, lblNombreEstudiante1);
        }

        private void txtcarnetestudiante2_TextChanged_1(object sender, EventArgs e)
        {
            VerificarEstudiante(txtcarnetestudiante2, lblNombreEstudiante2);
        }

        private void txtcarnetestudiante3_TextChanged_1(object sender, EventArgs e)
        {
            VerificarEstudiante(txtcarnetestudiante3, lblNombreEstudiante3);

        }

        private void txtidtutor_TextChanged_1(object sender, EventArgs e)
        {
            VerificarProfesor(txtidtutor, lblNombreTutor);

        }
    }
}
