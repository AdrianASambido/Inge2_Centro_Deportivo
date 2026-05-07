using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Entidades
{
    public class Devolucion
    {
        public int Id { get; private set; }
        public double MontoADevolver { get; set; }
        public DevolucionEstado Estado { get; set; }
        public int Id_Usuario { get; set; }
        public Usuario Usuario { get; set; }
        public int Id_Reserva {  get; set; }
        public Reserva Reserva { get; set; }
        public DateTime FechaGeneracion { get; set; }

        public Devolucion() { }
    }
}
