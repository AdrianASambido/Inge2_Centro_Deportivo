using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase
{
    public class CancelarReservaAdelantadaUseCase(
        IReservaRepositorio repoReserva,
        IUsuarioRepositorio repoUsuario,
        ICreditoRepositorio repoCredito,
        ITurnoRepositorio repoTurno,
        IEmailServicio emailServicio,
        IListaDeEsperaRepositorio listaRepo
    )
    {
        public async Task<(bool SeOtorgoCredito, string Mensaje)> Ejecutar(int idUsuario, int idReserva)
        {
            var usuario = await repoUsuario.ObtenerPorIdAsync(idUsuario);
            if (usuario == null) throw new Exception("Error: usuario inexistente");

            var reserva = await repoReserva.ObtenerPorIdAsync(idReserva);
            if (reserva == null) throw new Exception("Error: reserva inexistente");
            if (reserva.Estado == EstadoReserva.Cancelado) throw new Exception("Error: la reserva ya se encuentra cancelada");

            var turno = await repoTurno.ObtenerPorIdAsync(reserva.Id_Turno);
            if (turno == null) throw new Exception("Error: turno inexistente");

            DateTime ahora = DateTime.Now;
            DateTime fechaHoraClase = turno.Fecha.ToDateTime(turno.HoraInicio);
            TimeSpan diferenciaTiempo = fechaHoraClase - ahora;
            bool cumpleAnticipacion = diferenciaTiempo.TotalHours >= 48;



            int cancelacionesMes = await repoReserva.ContarCancelacionesUsuarioMesAsync(idUsuario, ahora.Year, ahora.Month);

            int totalCancelaciones = cancelacionesMes + 1;
            bool aplicarSancion = totalCancelaciones >= 3;

            reserva.Estado = EstadoReserva.Cancelado;

            var primeroEnEspera = await listaRepo.ObtenerPrimeroEnFilaAsync(turno.Id);
            if (primeroEnEspera != null)
            {
                primeroEnEspera.Estado = EstadoListaEspera.Notificado;
                primeroEnEspera.FechaNotificacion = ahora;
                await emailServicio.EnviarAvisoVacanteListaEsperaAsync(primeroEnEspera.Usuario.Email, turno);
                await listaRepo.ActualizarAsync(primeroEnEspera);
            }
            else
            {
                turno.CupoDisponible++;
                if (turno.Estado == EstadoTurno.Lleno)
                {
                    turno.Estado = EstadoTurno.Disponible;
                }
            }

            if (aplicarSancion)
            {
                usuario.TieneSancionDescuento = true;
            }

            bool otorgaCredito = cumpleAnticipacion && !usuario.TieneSancionDescuento;
            if (otorgaCredito)
            {
                Credito credito = new Credito(idUsuario, turno.Id_Actividad);
                await repoCredito.AgregarAsync(credito);
            }

            await repoReserva.ActualizarAsync(reserva);
            await repoUsuario.ActualizarAsync(usuario);
            await repoTurno.ActualizarAsync(turno);

            string mensaje;
            if (aplicarSancion)
            {
                mensaje = otorgaCredito
                    ? "Clase cancelada a tiempo. Se reintegró el crédito, pero alcanzaste las 3 cancelaciones mensuales y fuiste sancionado."
                    : "Clase cancelada fuera de término. No se otorgó crédito y, además, alcanzaste las 3 cancelaciones mensuales: fuiste sancionado.";
            }
            else
            {
                if (otorgaCredito)
                {
                    mensaje = "Clase cancelada con éxito. El crédito correspondiente fue acreditado en tu cuenta.";
                }
                else
                {

                    mensaje = !cumpleAnticipacion
                        ? "Clase cancelada fuera de término (menos de 48hs de anticipación). No se te otorga un credito."
                        : "Clase cancelada a tiempo, pero no se otorgó crédito por poseer una sanción vigente.";
                }
            }

            return (otorgaCredito, mensaje);
        }
    }
}