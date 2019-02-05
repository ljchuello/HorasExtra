using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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
                    oCalculo.Dia = row.Hora;

                    // Obtenemos la difrencia
                    TimeSpan timeSpan = row.Hora - row.Hora.Date;

                    // Forma de trabajar
                    switch (row.Tipo)
                    {
                        // Atrasos
                        case ORegistro.ETipo.Entrada:
                            if (row.Hora.DayOfWeek != DayOfWeek.Saturday & row.Hora.DayOfWeek != DayOfWeek.Sunday)
                            {
                                // Si superas las 8,5 horas es contado como atraso
                                if (timeSpan.TotalHours > 8.5)
                                {
                                    oCalculo.MinAtraso = timeSpan.TotalMinutes-510;
                                    oCalculo.Tipo = ORegistro.ETipo.Entrada;
                                    listCalculo.Add(oCalculo);
                                }
                                else
                                {
                                    oCalculo.MinAtraso = 0;
                                    oCalculo.Tipo = ORegistro.ETipo.Entrada;
                                    listCalculo.Add(oCalculo);
                                }
                            }
                            break;

                        case ORegistro.ETipo.Salida:
                            if (row.Hora.DayOfWeek != DayOfWeek.Saturday & row.Hora.DayOfWeek != DayOfWeek.Sunday)
                            {
                                // Si supera los 1060 minutos cuenta como hora extra
                                if (timeSpan.TotalMinutes >= 1060)
                                {
                                    oCalculo.MinExtra50 = (row.Hora - row.Hora.Date.AddHours(17.50)).TotalMinutes;
                                    oCalculo.Tipo = ORegistro.ETipo.Salida;
                                    listCalculo.Add(oCalculo);
                                }
                                else
                                {
                                    oCalculo.MinExtra50 = 0;
                                    oCalculo.Tipo = ORegistro.ETipo.Salida;
                                    listCalculo.Add(oCalculo);
                                }
                            }
                            break;
                    }
                }

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("Dia", typeof(string));
                dataTable.Columns.Add("Entrada", typeof(string));
                dataTable.Columns.Add("Atraso", typeof(string));
                dataTable.Columns.Add("Salida", typeof(string));
                dataTable.Columns.Add("Extra", typeof(string));

                double Atraso = 0;
                double Extra = 0;

                var fecha = listCalculo.GroupBy(x => x.Dia.Date).Select(y => y.First()).ToList();

                foreach (var row in fecha)
                {
                    OCalculo entrada = listCalculo.FirstOrDefault(x => x.Dia.Date == row.Dia.Date & x.Tipo == ORegistro.ETipo.Entrada) ?? new OCalculo();
                    OCalculo salida = listCalculo.FirstOrDefault(x => x.Dia.Date == row.Dia.Date & x.Tipo == ORegistro.ETipo.Salida) ?? new OCalculo();

                    Atraso = Atraso + entrada.MinAtraso;
                    Extra = Extra + entrada.MinExtra50;

                    dataTable.Rows.Add(
                        $"{row.Dia:yyyy-MM-dd}",
                        $"{entrada.Dia:HH:mm:ss}",
                        $"{entrada.MinAtraso:n2}",
                        $"{salida.Dia:HH:mm:ss}",
                        $"{salida.MinExtra50:n2}");
                }

                dataGridView1.DataSource = dataTable;

                StringBuilder stringBuilder = new StringBuilder();

                stringBuilder.AppendLine($"Atrasos: {listCalculo.Select(x => x.MinAtraso).Sum():n2} min.");
                stringBuilder.AppendLine($"Extra: {listCalculo.Select(x => x.MinExtra50).Sum():n2} min. | {listCalculo.Select(x => x.MinExtra50).Sum()/60:n2} horas");

                MessageBox.Show($"{stringBuilder}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ha ocurrido un error; {ex.Message}");
            }
        }
    }
}
