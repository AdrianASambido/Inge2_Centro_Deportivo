using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.TurnoUseCase
{
    public class CancelarTurnoUseCase(
        ITurnoRepositorio repoTurno,
        IReservaRepositorio repoReserva,
        ICreditoRepositorio repoCredito,
        IPagoServicio pagoServicio,
        IEmailServicio emailServicio
    )
    {
        public async Task Ejecutar(int idTurno)
        {

            var turno = await repoTurno.ObtenerPorIdAsync(idTurno);
            if (turno == null)
            {
                throw new Exception("El turno que intenta cancelar no existe.");
            }

            if (turno.Estado == EstadoTurno.Cancelado)
            {
                throw new Exception("El turno ya se encuentra cancelado.");
            }

            if (turno.Estado == EstadoTurno.Finalizado)
            {
                throw new Exception("El turno se encuentra finalizado.");
            }

            turno.Estado = EstadoTurno.Cancelado;

            var reservasActivas = turno.Reservas
                .Where(r => r.Estado != EstadoReserva.Cancelado)
                .ToList();

            List<Credito> nuevosCreditos = new List<Credito>();
            List<string> emailsDestinatarios = new List<string>();

            foreach (var reserva in reservasActivas)
            {
                if (reserva.Usuario != null && !string.IsNullOrEmpty(reserva.Usuario.Email))
                {
                    emailsDestinatarios.Add(reserva.Usuario.Email);
                }

                reserva.Estado = EstadoReserva.Cancelado;

                if (reserva.TipoReserva == TipoReserva.Ocasional)
                {

                    bool devolucionExitosa = await pagoServicio.ProcesarReembolsoAsync(reserva.Id_Usuario, reserva.PrecioPagado);
                    if (!devolucionExitosa)
                    {

                        Console.WriteLine($"[Alerta] No se pudo procesar la devolución automática del usuario {reserva.Id_Usuario}");
                    }
                }
                else if (reserva.TipoReserva == TipoReserva.Adelantado)
                {

                    var credito = new Credito(reserva.Id_Usuario, turno.Id_Actividad);
                    nuevosCreditos.Add(credito);
                }
            }


            await repoTurno.ActualizarAsync(turno);

            if (reservasActivas.Any())
            {
                await repoReserva.ActualizarMuchasAsync(reservasActivas);
            }

            if (nuevosCreditos.Any())
            {
                await repoCredito.AgregarMuchosAsync(nuevosCreditos);
            }


            if (emailsDestinatarios.Any())
            {
                try
                {

                    await emailServicio.EnviarAvisoCancelacionMasivo(emailsDestinatarios, turno);
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"[Error Email] Las alertas no pudieron enviarse: {ex.Message}");
                }
            }
        }
    }
}