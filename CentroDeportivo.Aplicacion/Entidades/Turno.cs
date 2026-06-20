using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Entidades
{
    public class Turno
    {
        public int Id { get; private set; }
        public DateOnly Fecha { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
        public decimal PrecioTurno { get; set; } 
        public int CupoMaximo { get; set; }
        public int CupoDisponible { get; set; }
        public EstadoTurno Estado { get; set; }
        public int Id_Actividad { get; set; }
        public int Id_Profesor { get; set; }
        public int Id_Cancha { get; set; }
        public Actividad Actividad { get; set; } = null!;
        public Cancha Cancha { get; set; } = null!;
        public Profesor Profesor { get; set; } = null!;

        public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
        public ICollection<InscripcionListaEspera> ListaEspera { get; set; } = new List<InscripcionListaEspera>();

        public Turno() { }

    }
}