using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase
{
    public class CrearReservaListaEsperaUseCase(
    IReservaRepositorio repoReserva,
    IListaDeEsperaRepositorio repoListaEspera,
    IPagoRepositorio repoPago,
    ITurnoRepositorio repoTurno)
    {
        public async Task Ejecutar(int idUsuario, int idTurno, string paymentId)
        {
            {

                var inscripcion = await repoListaEspera.ObtenerPorUsuarioYTurno(idUsuario, idTurno);
                if (inscripcion == null || inscripcion.Estado != EstadoListaEspera.Notificado)
                {
                    throw new Exception("No hay una invitación activa para este turno.");
                }

                var turno = await repoTurno.ObtenerPorIdAsync(idTurno);

                // 2. Crear la reserva confirmada
                var reserva = new Reserva
                {
                    Id_Turno = idTurno,
                    Id_Usuario = idUsuario,
                    FechaReserva = DateOnly.FromDateTime(DateTime.Now),
                    Estado = EstadoReserva.Confirmado // Ya pagó el 100%
                };
                await repoReserva.AgregarAsync(reserva);

                // 3. Registrar el Pago al 100%
                var pago = new Pago
                {
                    Id_Usuario = idUsuario,
                    Id_Reserva = reserva.Id,
                    Monto = turno.PrecioTurno,
                    MercadoPagoTransactionId = paymentId,
                    Fecha = DateTime.Now
                };
                await repoPago.AgregarAsync(pago);

                // 4. Actualizar estado de la lista de espera
                inscripcion.Estado = EstadoListaEspera.Confirmado;
                await repoListaEspera.ActualizarAsync(inscripcion);

            }

        }
    }
}

