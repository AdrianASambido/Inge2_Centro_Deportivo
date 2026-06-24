using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Entidades;
using System.Net.WebSockets;
using CentroDeportivo.Aplicacion.Validadores;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase
{
    public class CrearReservaConCreditoUseCase(IReservaRepositorio repoReserva, ITurnoRepositorio repoTurno, ICreditoRepositorio repoCredito, ReservaValidador validador)
    {
        public async Task Ejecutar(Reserva reserva)
        {
            var (esValido, mensaje) = await validador.ValidarReserva(reserva);

            if (!esValido)
            {
                throw new Exception(mensaje);
            }

            var turno = await repoTurno.ObtenerPorIdAsync(reserva.Id_Turno);
            if (turno == null)
            {
                throw new Exception("Error, turno inexistente.");
            }

            var creditos = await repoCredito.ObtenerDisponiblesAsync(reserva.Id_Usuario, turno.Id_Actividad);
            if (creditos == null || !creditos.Any())
            {
                throw new Exception("Error: no dispone de creditos para esta actividad");
            }

            var creditoAConsumir = creditos.OrderBy(c => c.FechaVencimiento).First();
            creditoAConsumir!.Estado = EstadoCredito.Utilizado;

            turno.CupoDisponible--;
            if (turno.CupoDisponible == 0)
            {
                turno.Estado = EstadoTurno.Lleno;
            }

            reserva.Asistencia = Asistencia.Ausente;
            reserva.Estado = EstadoReserva.Confirmado;
            reserva.FechaReserva = DateOnly.FromDateTime(DateTime.Today);
            reserva.PrecioPagado = 0;
            reserva.ConCredito = true;
            reserva.TipoReserva = TipoReserva.Ocasional;

            await repoReserva.AgregarAsync(reserva);
            await repoTurno.ActualizarAsync(turno);
            await repoCredito.ActualizarAsync(creditoAConsumir);
        }
    }
}