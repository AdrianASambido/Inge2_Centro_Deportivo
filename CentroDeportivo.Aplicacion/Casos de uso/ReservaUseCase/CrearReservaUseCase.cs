using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Validadores;
using System;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Entidades;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase
{
    public class CrearReservaUseCase(
        IReservaRepositorio repoReserva,
        ITurnoRepositorio repoTurno,
        IPagoRepositorio repoPago,
        IPagoServicio pagoServicio,
        ReservaValidador validador)
    {

        public async Task Ejecutar(Reserva reserva, string tarjetaToken)
        {
            var (esValido, mensaje) = await validador.ValidarReserva(reserva);

            if (!esValido)
            {
                throw new Exception(mensaje);
            }

            var turno = await repoTurno.ObtenerPorIdAsync(reserva.Id_Turno);


            decimal montoSena = turno!.PrecioTurno / 2;


            bool cobroExitoso = await pagoServicio.ProcesarCobroAsync(reserva.Id_Usuario, montoSena, tarjetaToken);

            if (!cobroExitoso)
            {
                throw new Exception("El cobro de la seña fue rechazado por la entidad bancaria. Reserva cancelada.");
            }

            turno.CupoDisponible--;

            if (turno.CupoDisponible == 0)
            {
                turno.Estado = EstadoTurno.Lleno;
            }

            reserva.Estado = EstadoReserva.Reservado;
            reserva.Asistencia = Asistencia.Ausente;
            reserva.FechaReserva = DateOnly.FromDateTime(DateTime.Today);
            reserva.PrecioPagado = montoSena;
            reserva.ConCredito = false;
            reserva.TipoReserva = TipoReserva.Ocasional;
            reserva.TokenQr = null;

            var pago = new Pago(reserva.Id_Usuario, montoSena, null, turno.Id, null);

            await repoReserva.AgregarAsync(reserva);
            await repoTurno.ActualizarAsync(turno);

            pago.Id_Reserva = reserva.Id;
            await repoPago.AgregarAsync(pago);
        }
    }
}