using System;
using System.Dynamic;

namespace Libreria
{
    public class OCalculo
    {
        public string CodEmpleado { set; get; } = string.Empty;
        public ORegistro.ETipo Tipo { set; get; } = ORegistro.ETipo.Entrada;
        public DateTime Dia { set; get; } = new DateTime(1900, 01, 01);
        public double MinAtraso { set; get; } = 0;
        public double MinExtra50 { set; get; } = 0;
        public double MinExtra100 { set; get; } = 0;
    }
}
