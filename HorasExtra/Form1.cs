using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Libreria;

namespace HorasExtra
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Creamos la lista
                List<ORegistro> listRegistro = new List<ORegistro>();

                // Obtenemos los registros
                //for (int i = 0; i < textbox1.Lines.Length; i++)
                //{
                //    textbox2.Text += textbox1.Lines[i] + "\r\n";
                //}
                foreach (var row in textBox1.Lines)
                {
                    ORegistro oRegistro = new ORegistro();
                    oRegistro.CodEmpleado = row.Substring(0, 3);
                    oRegistro.Hora = new DateTime();
                    string auxLectura = row.Substring(4, 19);
                }

                MessageBox.Show($"Finalizado");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ha ocurrido un error; {ex.Message}");
            }
        }
    }
}
