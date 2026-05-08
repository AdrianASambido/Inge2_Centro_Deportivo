using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase
{
    public class RegistrarAsistenciaManualUseCase(IReservaRepositorio repo)
    {
        public async Task Ejecutar(int idReserva) {
            var reserva = await repo.ObtenerPorIdAsync(idReserva);

            if (reserva == null) {
                throw new Exception("Error: reserva inexistente");
            }

            if (reserva.Asistencia == Asistencia.Presente || reserva.Estado == EstadoReserva.Cancelado || reserva.Estado == EstadoReserva.PendienteDePago) {
                throw new Exception("Error: no se puede confirmar asistencia en esta reserva.");
            }

            reserva.Asistencia = Asistencia.Presente;
            await repo.ActualizarAsync(reserva);
        }
    }
}
