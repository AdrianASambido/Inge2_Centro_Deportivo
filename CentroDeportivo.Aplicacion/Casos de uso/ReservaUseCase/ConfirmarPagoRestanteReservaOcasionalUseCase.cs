using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase
{
    public class ConfirmarPagoRestanteReservaOcasionalUseCase(
        IReservaRepositorio repoReserva,
        IPagoRepositorio repoPago
    )
    {
        public async Task Ejecutar(int idReserva)
        {

            var reserva = await repoReserva.ObtenerPorIdAsync(idReserva);

            if (reserva == null)
            {
                throw new Exception("Error del sistema: No se encontró la reserva a confirmar.");
            }


            decimal montoSaldoRestante = reserva.PrecioPagado;


            reserva.Estado = EstadoReserva.Confirmado;
            reserva.PrecioPagado = reserva.PrecioPagado + montoSaldoRestante; 

            var pagoSegundo = new Pago(
                reserva.Id_Usuario,
                montoSaldoRestante,
                reserva.Id,
                reserva.Id_Turno,
                null
            );

            await repoReserva.ActualizarAsync(reserva);
            await repoPago.AgregarAsync(pagoSegundo);
        }
    }
}