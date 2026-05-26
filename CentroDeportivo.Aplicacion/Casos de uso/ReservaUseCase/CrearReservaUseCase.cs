using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Validadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Entidades;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase
{
    public class CrearReservaUseCase(IReservaRepositorio repoReserva, ITurnoRepositorio repoTurno, ReservaValidador validador)
    {
        public async Task Ejecutar(Reserva reserva)
        {
            var (esValido, mensaje) = await validador.ValidarReserva(reserva);

            if (!esValido)
            {
                throw new Exception(mensaje);
            }

            var turno = await repoTurno.ObtenerPorIdAsync(reserva.Id_Turno);

            turno!.CupoDisponible--;///

            if (turno.CupoDisponible == 0)
            {
                turno.Estado = EstadoTurno.Lleno;
            }

            reserva.Estado = EstadoReserva.PendienteDePago;
            reserva.Asistencia = Asistencia.Ausente;
            reserva.FechaReserva = DateOnly.FromDateTime(DateTime.Today);
            reserva.PrecioPagado = turno.PrecioTurno;

            await repoReserva.AgregarAsync(reserva);
            await repoTurno.ActualizarAsync(turno);
        }
    }
}