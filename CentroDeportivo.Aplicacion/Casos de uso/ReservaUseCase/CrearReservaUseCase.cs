using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Validadores;
using System;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Entidades;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase
{ ///cuando vuelve de pagar en mercado pago, reserva ocasional/ 
    public class CrearReservaUseCase(
        IReservaRepositorio repoReserva,
        ITurnoRepositorio repoTurno,
        IPagoRepositorio repoPago)
    {


        public async Task Ejecutar(Reserva reserva, string idPayment)
        {

            var turno = await repoTurno.ObtenerPorIdAsync(reserva.Id_Turno);
            if (turno == null)
            {
                throw new Exception("Error crítico: El turno de la seña pagada no existe en el sistema.");
            }


            decimal montoSena = turno.PrecioTurno / 2;

            turno.CupoDisponible--;
            if (turno.CupoDisponible <= 0)
            {
                turno.Estado = EstadoTurno.Lleno;
            }


            reserva.Estado = EstadoReserva.Reservado;
            reserva.Asistencia = Asistencia.Ausente;
            reserva.FechaReserva = DateOnly.FromDateTime(DateTime.Today);
            reserva.PrecioPagado = montoSena;
            reserva.ConCredito = false;
            reserva.TipoReserva = TipoReserva.Ocasional;

           

            var pago = new Pago(reserva.Id_Usuario, montoSena, null, turno.Id, null);
            pago.MercadoPagoTransactionId = idPayment;

            await repoReserva.AgregarAsync(reserva);
            await repoTurno.ActualizarAsync(turno);

            pago.Id_Reserva = reserva.Id;
            await repoPago.AgregarAsync(pago);
        }
    }
}