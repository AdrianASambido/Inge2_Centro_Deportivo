using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase
{
    public class CancelarReservaUseCase(
        IReservaRepositorio repoReserva,
        IDevolucionRepositorio repoDevolucion,
        ITurnoRepositorio repoTurno,
        IEmailServicio repoEmail,
        IPagoServicio pagoServicio,
        IListaDeEsperaRepositorio repoLista,
        IPagoRepositorio pagoRepo)
    {
        public async Task<(bool conDevolucion, decimal monto)> Ejecutar(int idReserva)
        {
  
            var reserva = await repoReserva.ObtenerPorIdAsync(idReserva);
            if (reserva == null) throw new Exception("Error: reserva inexistente.");
            if (reserva.Turno!.Estado == EstadoTurno.Finalizado) throw new Exception("Error: la clase ya transcurrió.");
            if (reserva.Estado == EstadoReserva.Cancelado) throw new Exception("Error: la reserva ya se encuentra cancelada.");

            var ahora = DateTime.Now;
            var fechaHoraTurno = reserva.Turno.Fecha.ToDateTime(reserva.Turno.HoraInicio);
            var diferencia = fechaHoraTurno - ahora;

            bool aplicaDevolucion = false;
            decimal montoADevolver = 0;

  
            if (diferencia.TotalHours >= 24 && !reserva.ConCredito)
            {
                aplicaDevolucion = true;
                var pago = await pagoRepo.ObtenerPorReservaAsync(idReserva);

                if (pago == null) throw new Exception("No se encontró el registro de pago para esta reserva.");

                montoADevolver = (reserva.Estado == EstadoReserva.Reservado)
                                 ? reserva.PrecioPagado
                                 : reserva.PrecioPagado / 2;


                bool transaccionExitosa = await pagoServicio.RealizarReembolsoAsync(pago.MercadoPagoTransactionId);

                if (!transaccionExitosa)
                {
                    throw new Exception("Error: No se pudo procesar el reembolso en Mercado Pago. La cancelación fue abortada.");
                }


                var devolucion = new Devolucion
                {
                    MontoADevolver = montoADevolver,
                    Estado = DevolucionEstado.Confirmado,
                    Id_Usuario = reserva.Id_Usuario,
                    Id_Reserva = reserva.Id,
                    FechaGeneracion = ahora
                };
                await repoDevolucion.AgregarAsync(devolucion);
            }


            reserva.Estado = EstadoReserva.Cancelado;
            reserva.FechaCancelacion = DateTime.Now; // con hora exacta
            await repoReserva.ActualizarAsync(reserva);

            var turno = await repoTurno.ObtenerPorIdAsync(reserva.Id_Turno);
            if (turno == null) throw new Exception("Error: turno inexistente.");


            var primeroEnEspera = await repoLista.ObtenerPrimeroEnFilaAsync(turno.Id);

            if (primeroEnEspera != null)
            {
                primeroEnEspera.Estado = EstadoListaEspera.Notificado;
                primeroEnEspera.FechaNotificacion = ahora;
                await repoEmail.EnviarAvisoVacanteListaEsperaAsync(primeroEnEspera.Usuario.Email, turno);
                await repoLista.ActualizarAsync(primeroEnEspera);
            }
            else
            {
                turno.CupoDisponible++;
                if (turno.Estado == EstadoTurno.Lleno)
                {
                    turno.Estado = EstadoTurno.Disponible;
                }
                await repoTurno.ActualizarAsync(turno);
            }

            return (aplicaDevolucion, montoADevolver);
        }
    }
}