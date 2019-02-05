using System;

namespace Libreria
{
    public class ORegistro
    {
        public string CodEmpleado { set; get; }
        public ETipo Tipo { set; get; } = ETipo.Entrada;
        public DateTime Hora { set; get; } = new DateTime(1900, 01, 01);

        public enum ETipo
        {
            Entrada,
            Salida
        }

        public enum EPrecio
        {
            p50,
            p100,

        }
    }
}
