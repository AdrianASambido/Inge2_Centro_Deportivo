using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Entidades;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase
{//este ya no se usa
    public class ConfirmarPagoReservaUseCase(IReservaRepositorio repo)
    {
        public async Task Ejecutar(int idReserva) {
            var reserva = await repo.ObtenerPorIdAsync(idReserva);

            if (reserva == null) {
                throw new Exception("Reserva inexistente.");
            }

            if (reserva.Turno!.Estado == EstadoTurno.Finalizado) {
                throw new Exception("Error: la reserva corresponde a un turno ya finalizado.");
            }

            if (reserva.Estado != EstadoReserva.PendienteDePago)
            {
                throw new Exception($"No se puede confirmar el pago. La reserva está en estado: {reserva.Estado}");
            }

            reserva.Estado = EstadoReserva.Confirmado;
            await repo.ActualizarAsync(reserva);
        }
    }
}
