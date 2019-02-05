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

                // Creamos la otra lista
                List<OCalculo> listCalculo = new List<OCalculo>();

                // Leemos los registros
                foreach (var row in textBox1.Lines)
                {
                    // Validamos el largo
                    if (row.Length == 25)
                    {
                        // Instanciamos
                        ORegistro oRegistro = new ORegistro();

                        // Obtenemos el codigo del empleado
                        oRegistro.CodEmpleado = row.Substring(0, 3);

                        // Fecha y hora del registro
                        oRegistro.Hora = new DateTime(
                            int.Parse(row.Substring(4, 4)),
                            int.Parse(row.Substring(9, 2)),
                            int.Parse(row.Substring(12, 2)),

                            int.Parse(row.Substring(15, 2)),
                            int.Parse(row.Substring(18, 2)),
                            int.Parse(row.Substring(21, 2)));

                        // Tipo de registro
                        oRegistro.Tipo = oRegistro.Hora.Hour < 11 ? ORegistro.ETipo.Entrada : ORegistro.ETipo.Salida;

                        // Agregamos a la lista
                        listRegistro.Add(oRegistro);
                    }
                }

                // Procesamos
                foreach (ORegistro row in listRegistro)
                {
                    // Instanciamos el calculo
                    OCalculo oCalculo = new OCalculo();

                    // Llenamos
                    oCalculo.CodEmpleado = row.CodEmpleado;
                    oCalculo.Hora = row.Hora;

                    // Obtenemos la difrencia
                    TimeSpan timeSpan = row.Hora - row.Hora.Date;

                    // Forma de trabajar
                    switch (row.Tipo)
                    {
                        // Atrasos
                        case ORegistro.ETipo.Entrada:
                            // Si superas las 8,5 horas es contado como atraso
                            if (timeSpan.TotalHours > 8.5)
                            {
                                oCalculo.MinAtraso = oCalculo.MinAtraso + timeSpan.TotalMinutes;
                            }
                            break;

                        case ORegistro.ETipo.Salida:

                            break;
                    }
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
