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
                Console.WriteLine($"[CrearReservaListaEspera] idUsuario: {idUsuario}, idTurno: {idTurno}, paymentId: {paymentId}");

                var inscripcion = await repoListaEspera.ObtenerPorUsuarioYTurno(idUsuario, idTurno);
                Console.WriteLine($"[CrearReservaListaEspera] inscripcion: {(inscripcion == null ? "NULL" : $"Id={inscripcion.Id}, Estado={inscripcion.Estado}")}");

                if (inscripcion == null || inscripcion.Estado != EstadoListaEspera.Notificado)
                {
                    throw new Exception("No hay una invitación activa para este turno.");
                }

                var turno = await repoTurno.ObtenerPorIdAsync(idTurno);

               

                var reserva = new Reserva
                {
                    Id_Turno = idTurno,
                    Id_Usuario = idUsuario,
                    PrecioPagado = turno.PrecioTurno,
                    FechaReserva = DateOnly.FromDateTime(DateTime.Now),
                    Estado = EstadoReserva.Confirmado
                };
                await repoReserva.AgregarAsync(reserva);

                var pago = new Pago
                {
                    Id_Usuario = idUsuario,
                    Id_Reserva = reserva.Id,
                    Monto = turno.PrecioTurno,
                    MercadoPagoTransactionId = paymentId,
                    Fecha = DateTime.Now
                };
                await repoPago.AgregarAsync(pago);

                inscripcion.Estado = EstadoListaEspera.Confirmado;
                await repoListaEspera.ActualizarAsync(inscripcion);

            }

        }
    }
}

