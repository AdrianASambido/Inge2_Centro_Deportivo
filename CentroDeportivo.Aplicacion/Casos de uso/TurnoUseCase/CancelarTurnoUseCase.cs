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
        IEmailServicio emailServicio,
        IPagoRepositorio pagoRepo
    )
    {
        public async Task Ejecutar(int idTurno)
        {
            Console.WriteLine("HOLAAAAAA");
            var turno = await repoTurno.ObtenerPorIdAsync(idTurno);
            if (turno == null) throw new Exception("El turno que intenta cancelar no existe.");
            if (turno.Estado == EstadoTurno.Cancelado) throw new Exception("El turno ya se encuentra cancelado.");
            if (turno.Estado == EstadoTurno.Finalizado) throw new Exception("El turno se encuentra finalizado.");

            turno.Estado = EstadoTurno.Cancelado;

            var reservasActivas = turno.Reservas.Where(r => r.Estado != EstadoReserva.Cancelado).ToList();

            List<Credito> nuevosCreditos = new List<Credito>();
            List<string> emailsDestinatarios = new List<string>();

            foreach (var reserva in reservasActivas)
            {
                if (reserva.Usuario != null && !string.IsNullOrEmpty(reserva.Usuario.Email))
                    emailsDestinatarios.Add(reserva.Usuario.Email);

                reserva.Estado = EstadoReserva.Cancelado;

                if (reserva.TipoReserva == TipoReserva.Ocasional)
                {
  
                    if (reserva.ConCredito)
                    {

                        var credito = new Credito(reserva.Id_Usuario, turno.Id_Actividad);
                        nuevosCreditos.Add(credito);
                    }
                    else
                    {
       
                        var pago = await pagoRepo.ObtenerPorReservaAsync(reserva.Id);

                        if (pago != null && !string.IsNullOrEmpty(pago.MercadoPagoTransactionId))
                        {
                            bool exito = await pagoServicio.RealizarReembolsoAsync(pago.MercadoPagoTransactionId);
                            if (!exito)
                            {
                                throw new Exception($"No se pudo procesar el reembolso de la reserva {reserva.Id}. Cancelación abortada.");
                            }
                        }
                    }
                }
                else if (reserva.TipoReserva == TipoReserva.Adelantado)
                {
 
                    var credito = new Credito(reserva.Id_Usuario, turno.Id_Actividad);
                    nuevosCreditos.Add(credito);
                }
            }

            await repoTurno.ActualizarAsync(turno);
            if (reservasActivas.Any()) await repoReserva.ActualizarMuchasAsync(reservasActivas);
            if (nuevosCreditos.Any()) await repoCredito.AgregarMuchosAsync(nuevosCreditos);

           /* if (emailsDestinatarios.Any())
            {
                try { await emailServicio.EnviarAvisoCancelacionMasivo(emailsDestinatarios, turno); }
                catch { }
            }*/
        }
    }
}