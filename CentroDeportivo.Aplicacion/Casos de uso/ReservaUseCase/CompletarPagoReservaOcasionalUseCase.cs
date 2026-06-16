using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase
{
    public class CompletarPagoReservaOcasionalUseCase(
        IReservaRepositorio repoReserva,
        IPagoRepositorio repoPago,
        IPagoServicio pagoServicio 
    )
    {
        public async Task Ejecutar(int idReserva, string tarjetaToken)
        {
            var reserva = await repoReserva.ObtenerPorIdAsync(idReserva);

            if (reserva == null)
            {
                throw new Exception("Error: reserva inexistente.");
            }

            if (reserva.Estado != EstadoReserva.Reservado)
            {
                throw new Exception("Error: esta reserva no se encuentra pendiente de confirmación/pago restante.");
            }

            decimal montoRestante = reserva.PrecioPagado;

            bool cobroExitoso = await pagoServicio.ProcesarCobroAsync(reserva.Id_Usuario, montoRestante, tarjetaToken);

            if (!cobroExitoso)
            {
                throw new Exception("El cobro del saldo restante fue rechazado por la entidad bancaria. No se pudo confirmar la reserva.");
            }

   
            reserva.Estado = EstadoReserva.Confirmado;
            reserva.PrecioPagado = montoRestante * 2; 

            var pago = new Pago(reserva.Id_Usuario, montoRestante, reserva.Id, reserva.Id_Turno, null);

            await repoReserva.ActualizarAsync(reserva);
            await repoPago.AgregarAsync(pago);
        }
    }
}