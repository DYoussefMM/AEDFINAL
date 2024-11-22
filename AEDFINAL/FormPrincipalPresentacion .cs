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
    public partial class FormPrincipalPresentacion : Form
    {
        public FormPrincipalPresentacion()
        {
            InitializeComponent(); 
        }

        private void CambiarColorBoton(Button botonSeleccionado)
        {
            // Restaurar el color de todos los botones a su color original
            foreach (Control control in this.panelSeleccion.Controls)
            {
                if (control is Button)
                {
                    control.BackColor = Color.FromArgb(156, 18, 18); // Color original del boton
                    control.ForeColor = Color.White; // Color de texto original (opcional)
                }
            }

            // Cambiamos el color del boton seleccionado
            botonSeleccionado.BackColor = Color.FromArgb(175, 76, 76); // Color de selección
            botonSeleccionado.ForeColor = Color.Black; // Color de texto para el botón seleccionado (opcional)
        }

        private void AbrirFormEnPanel(object Formulario, Button botonSeleccionado)
        {
            CambiarColorBoton(botonSeleccionado);
            //  Primeramente se pregunta si existe algun control dentro el panel, en caso que sea verdadero se  
            //  eliminara
            if (this.panelContenedor.Controls.Count > 0)
                this.panelContenedor.Controls.RemoveAt(0);

            //  Se crea un formulario con nombre "fh"
            Form fh = Formulario as Form;
            //  En sus propiedades se especifica que no sera un formulario de primer nivel, sino secundario
            fh.TopLevel = false;
            //  Se acoplara a todo el panel contenedor
            fh.Dock = DockStyle.Fill;
            //  Por ultimo se agrega al panel
            this.panelContenedor.Controls.Add(fh);
            //  Se establece la instancia como contenedor de datos del panel
            this.panelContenedor.Tag = fh;
            //  Por ultimo se muestra
            fh.Show();
        }

        private void BtnEstudiante_Click(object sender, EventArgs e)
        {
            AbrirFormEnPanel(new FormEstudiante(), BtnEstudiante);
        }

        private void BtnProfesor_Click(object sender, EventArgs e)
        {
            AbrirFormEnPanel(new FormProfesor(), BtnProfesor);
        }

        private void BtnMonografia_Click(object sender, EventArgs e)
        {
            AbrirFormEnPanel(new FormMonografia(), BtnMonografia);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AbrirFormEnPanel(new FormConsultas(), btnconsultas);
        }
    }
}
