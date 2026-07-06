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
        public decimal PrecioPagado { get; set; }
        public EstadoReserva Estado { get; set; }
        public Asistencia Asistencia { get; set; }
        public DateOnly FechaReserva { get; set; }
        public DateOnly? FechaCancelacion { get; set; }
        public DateOnly FechaAsistencia { get; set; }
        public TipoReserva TipoReserva { get; set; } ///Tipo Ocasional, Adelantada
        public bool ConCredito { get; set; } ///Si hizo la reserva ocasional pagando con credito es TRUE
        public Guid? CodigoPaqueteAdelantado { get; set; } //Guid para enlazar el pago con las reservas por adelantado de una clase
        public string? TokenQr { get; set; } //token para registrar la asistencia
        public int Id_Usuario { get; set; }
        public int Id_Turno { get; set; }
        public Usuario? Usuario { get; set; } = null;
        public Turno? Turno { get; set; } = null;

        public Reserva() { }
    }


}