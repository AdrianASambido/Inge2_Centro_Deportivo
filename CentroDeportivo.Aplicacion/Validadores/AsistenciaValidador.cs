using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Validadores
{
    public class AsistenciaValidador(IReservaRepositorio repoReserva)
    {
        public async Task<(bool esValido, string mensaje)> ValidarAsistencia(Reserva reserva)
        {


            var fechaHoraClase = reserva.Turno!.Fecha.ToDateTime(reserva.Turno.HoraInicio);
            var diferencia = fechaHoraClase - DateTime.Now;
            if (diferencia.TotalMinutes > 15)
                return (false, "Error: la generación de QR se habilita 15 minutos antes del comienzo de la clase.");

            if (reserva.Turno!.Estado == EstadoTurno.Finalizado)
                return (false, "Error: la clase ya finalizó.");

            if (reserva.Estado == EstadoReserva.Cancelado)
                return (false, "Error: esta reserva se encuentra cancelada.");

            if (reserva.Estado == EstadoReserva.PendienteDePago)
                return (false, "Error: la reserva se encuentra pendiente de pago.");

            if (reserva.Asistencia == Asistencia.Presente)
                return (false, "Error: el inscripto ya registra asistencia en esta reserva.");

            return (true, "");
        }
    }
}