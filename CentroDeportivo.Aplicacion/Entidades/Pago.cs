using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Entidades
{
    public class Pago
    {
        public int Id { get; private set; }
        public int Id_Usuario { get; set; }
        public int? Id_Reserva { get; set; }

        public int? Id_Turno { get; set; }
        public Guid? CodigoPaqueteAdelantado { get; set; }

        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public Reserva? Reserva { get; set; }
        public Usuario Usuario { get; set; }
        public Turno? Turno { get; set; }

        public Pago() { }

        public Pago(int idUsuario, decimal monto, int? idReserva = null, int? idTurno = null, Guid? codigoPaquete = null)
        {
            this.Id_Usuario = idUsuario;
            this.Monto = monto;
            this.Id_Reserva = idReserva;
            this.Id_Turno = idTurno;
            this.CodigoPaqueteAdelantado = codigoPaquete;
            this.Fecha = DateTime.Now;
        }
    }
}