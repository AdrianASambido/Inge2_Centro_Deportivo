using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Entidades
{
    public class Actividad
    {
        public int Id { get; private set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public double Precio { get; set; }
        public bool Existe { get; set; } = true;

        public Actividad(string nombre, string descripcion, double precio)
        {
            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.Precio = precio;
        }
        public Actividad() { }
    }
}
