using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.TurnoUseCase
{
    public class EliminarInscriptoUseCase(
      IReservaRepositorio repoReserva,
      IPagoRepositorio repoPago,
      ICreditoRepositorio repoCredito,
      ITurnoRepositorio repoTurno,
      IPagoServicio pagoServicio)
    {
        public async Task Ejecutar(int idReserva)
        {
            var reserva = await repoReserva.ObtenerPorIdAsync(idReserva);
            if (reserva == null) throw new Exception("Reserva no encontrada.");
            if (reserva.Estado == EstadoReserva.Cancelado) throw new Exception("La reserva ya está cancelada.");

        
            if (reserva.ConCredito || reserva.TipoReserva == TipoReserva.Adelantado)
            {
            
                var credito = new Credito(reserva.Id_Usuario, reserva.Turno.Id_Actividad);
                await repoCredito.AgregarAsync(credito);
            }
            else if (reserva.TipoReserva == TipoReserva.Ocasional)
            {
               
                var pagos = await repoPago.ObtenerTodosPorReservaAsync(reserva.Id);

                foreach (var pago in pagos)
                {
                    if (!string.IsNullOrEmpty(pago.MercadoPagoTransactionId))
                    {
              
                        bool exito = await pagoServicio.RealizarReembolsoAsync(pago.MercadoPagoTransactionId);
                        if (!exito)
                        {
                            throw new Exception($"Cancelación abortada: No se pudo reembolsar el pago {pago.Id}.");
                        }
                    }
                }
            }


            reserva.Estado = EstadoReserva.Cancelado;
            await repoReserva.ActualizarAsync(reserva);

            var turno = reserva.Turno;
            turno.CupoDisponible++;
            if (turno.Estado == EstadoTurno.Lleno)
                turno.Estado = EstadoTurno.Disponible;

            await repoTurno.ActualizarAsync(turno);
        }
    }
}
