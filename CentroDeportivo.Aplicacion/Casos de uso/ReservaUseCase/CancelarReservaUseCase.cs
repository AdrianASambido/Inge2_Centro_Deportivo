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
        IListaDeEsperaRepositorio repoLista)
    {
        public async Task<(bool conDevolucion, decimal monto)> Ejecutar(int idReserva)
        {
            var reserva = await repoReserva.ObtenerPorIdAsync(idReserva);

            if (reserva == null) throw new Exception("Error: reserva inexistente.");
            if (reserva.Turno!.Estado == EstadoTurno.Finalizado) throw new Exception("Error: la clase ya transcurrio.");
            if (reserva.Estado == EstadoReserva.Cancelado) throw new Exception("Error: la reserva ya se encuentra cancelada.");

            var ahora = DateTime.Now;
            var fechaHoraTurno = reserva.Turno.Fecha.ToDateTime(reserva.Turno.HoraInicio);
            var diferencia = fechaHoraTurno - ahora;

            bool aplicaDevolucion = false;
            decimal montoADevolver = 0;
            DevolucionEstado estadoInicialDevolucion = DevolucionEstado.Confirmado;

            if (diferencia.TotalHours >= 24 && !reserva.ConCredito)
            {
                aplicaDevolucion = true;

                if (reserva.Estado == EstadoReserva.Reservado)
                {
                    montoADevolver = reserva.PrecioPagado;
                }
                else if (reserva.Estado == EstadoReserva.Confirmado)
                {
                    montoADevolver = reserva.PrecioPagado / 2;
                }

                try
                {
                    bool transaccionExitosa = await pagoServicio.ProcesarReembolsoAsync(reserva.Id_Usuario, montoADevolver);
                    if (!transaccionExitosa)
                    {
                        estadoInicialDevolucion = DevolucionEstado.Pendiente;
                    }
                }
                catch (Exception)
                {
                    estadoInicialDevolucion = DevolucionEstado.Pendiente;
                }

                var devolucion = new Devolucion
                {
                    MontoADevolver = montoADevolver,
                    Estado = estadoInicialDevolucion,
                    Id_Usuario = reserva.Id_Usuario,
                    Id_Reserva = reserva.Id,
                    FechaGeneracion = ahora
                };

                await repoDevolucion.AgregarAsync(devolucion);
            }

            reserva.Estado = EstadoReserva.Cancelado;
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
                // ESCENARIO B: No hay nadie en la fila. El cupo se libera normalmente.
                turno.CupoDisponible++;
                if (turno.Estado == EstadoTurno.Lleno)
                {
                    turno.Estado = EstadoTurno.Disponible;
                }
            }

            // Persistimos el estado final del turno (haya cambiado el cupo o no)
            await repoTurno.ActualizarAsync(turno);

            return (aplicaDevolucion, montoADevolver);
        }
    }
}