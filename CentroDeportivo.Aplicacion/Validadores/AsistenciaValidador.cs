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
        public async Task<(bool esValido, string mensaje)> ValidarAsistencia(int idReserva)
        {


            var reserva = await repoReserva.ObtenerPorIdAsync(idReserva);
            if (reserva == null) {
                return (false, "Error: reserva inexistente");
            }

            var turno = reserva.Turno;

            if (turno == null) {
                return (false, "Error: turno inexistente");
            }

            if (turno.Estado == EstadoTurno.Cancelado || turno.Estado == EstadoTurno.Finalizado)
            {
                return (false, "Error: este turno no esta disponible");
            }

            if (reserva.Estado != EstadoReserva.Confirmado)
            {
                return (false, "Error: debe pagar la reserva para poder generar el QR");
            }

            var ahora = DateTime.Now;
            var inicioTurno = turno.Fecha.ToDateTime(turno.HoraInicio);
            TimeSpan tiempoRestante = inicioTurno - ahora;

            if (tiempoRestante.TotalMinutes  > 15)
            {
                return (false, "Error: puede generar el QR 15 minutos antes de la clase.");
            }

            return (true, "");
        }
    }
}