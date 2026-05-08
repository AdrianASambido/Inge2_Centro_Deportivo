using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Entidades
{
    public class Reserva
    {
        public int Id { get; private set; }
        public double PrecioPagado { get; set; }
        public EstadoReserva Estado { get; set; }
        public Asistencia Asistencia { get; set; }
        public DateOnly FechaReserva { get; set; }
        public DateOnly FechaAsistencia { get; set; }
        public string? TokenQr { get; set; }
        public int Id_Usuario { get; set; }
        public int Id_Turno { get; set; }
        public Usuario? Usuario { get; set; } = null;
        public Turno? Turno { get; set; } = null;

        public Reserva() { }
    }

    
}
