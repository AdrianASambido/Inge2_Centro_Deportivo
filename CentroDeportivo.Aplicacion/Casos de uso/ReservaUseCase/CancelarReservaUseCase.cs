using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase
{
    public class CancelarReservaUseCase(IReservaRepositorio repoReserva, IDevolucionRepositorio repoDevolucion, ITurnoRepositorio repoTurno)
    {
        public async Task<(bool conDevolucion, double monto)> Ejecutar(int idReserva)
        {
            var reserva = await repoReserva.ObtenerPorIdAsync(idReserva);

            if (reserva == null)
            {
                throw new Exception("Error: reserva inexistente.");
            }

            if (reserva.Turno.Estado == EstadoTurno.Finalizado) {
                throw new Exception("Error: la clase ya transcurrio.");
            }

            if (reserva.Estado == EstadoReserva.Cancelado)
            {
                throw new Exception("Error: la reserva ya se encuentra cancelada.");
            }
         
            var ahora = DateTime.Now;
            var fechaHoraTurno = reserva.Turno.Fecha.ToDateTime(reserva.Turno.HoraInicio);
            var diferencia = fechaHoraTurno - ahora;

            bool aplicaDevolucion = false;
            double montoADevolver = 0;

            

            if (diferencia.TotalHours > 24 && reserva.Estado == EstadoReserva.Confirmado)
            {
                aplicaDevolucion = true;
                montoADevolver = reserva.PrecioPagado / 2;

                var devolucion = new Devolucion
                {
                    MontoADevolver = montoADevolver,
                    Estado = DevolucionEstado.Pendiente,
                    Id_Usuario = reserva.Id_Usuario,
                    Id_Reserva = reserva.Id,
                    FechaGeneracion = ahora
                };
                await repoDevolucion.AgregarAsync(devolucion);
            }


            var turno = await repoTurno.ObtenerPorIdAsync(reserva.Id_Turno);
            turno!.CupoDisponible++;

            if (turno.Estado == EstadoTurno.Lleno)
            {
                turno.Estado = EstadoTurno.Disponible;
            }

            await repoTurno.ActualizarAsync(turno);

            reserva.Estado = EstadoReserva.Cancelado;
            await repoReserva.ActualizarAsync(reserva);

            return (aplicaDevolucion, montoADevolver);
        }
    }
}