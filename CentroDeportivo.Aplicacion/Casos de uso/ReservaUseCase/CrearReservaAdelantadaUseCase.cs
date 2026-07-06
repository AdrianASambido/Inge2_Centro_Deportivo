using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase
{
    public class CrearReservaAdelantadaUseCase(
     IReservaRepositorio repoReserva,
     ITurnoRepositorio repoTurno,
     IPagoRepositorio repoPago,
     IUsuarioRepositorio repoUsuario
 )
    {
        public async Task Ejecutar(int idUsuario, List<Turno> clasesDisponibles, string idPayment)
        {
            DateOnly hoy = DateOnly.FromDateTime(DateTime.Now);
            Guid codigoPaquete = Guid.NewGuid();

            var usuario = await repoUsuario.ObtenerPorIdAsync(idUsuario)
                ?? throw new Exception("Usuario no encontrado.");

            decimal precioBase = clasesDisponibles.First().PrecioTurno;
            decimal factor = usuario.TieneSancionDescuento ? 1.0m : 0.80m;
            decimal precioConDescuento = precioBase * factor;
            decimal montoTotal = precioConDescuento * clasesDisponibles.Count;

            // si tenía sanción se levanta al reservar por adelantado
            if (usuario.TieneSancionDescuento)
            {
                usuario.TieneSancionDescuento = false;
                await repoUsuario.ActualizarAsync(usuario);
            }

            List<Reserva> nuevasReservas = new();
            foreach (var turnoClase in clasesDisponibles)
            {
                nuevasReservas.Add(new Reserva
                {
                    Id_Usuario = idUsuario,
                    Id_Turno = turnoClase.Id,
                    FechaReserva = hoy,
                    FechaAsistencia = turnoClase.Fecha,
                    Estado = EstadoReserva.Confirmado,
                    Asistencia = Asistencia.Ausente,
                    TipoReserva = TipoReserva.Adelantado,
                    PrecioPagado = precioConDescuento,
                    ConCredito = false,
                    CodigoPaqueteAdelantado = codigoPaquete,
                    TokenQr = null
                });

                turnoClase.CupoDisponible--;
                if (turnoClase.CupoDisponible <= 0)
                    turnoClase.Estado = EstadoTurno.Lleno;
            }

            var nuevoPago = new Pago(idUsuario, montoTotal, null, null, codigoPaquete);
            nuevoPago.MercadoPagoTransactionId = idPayment;

            await repoReserva.GuardarMuchasReservasAsync(nuevasReservas);
            await repoTurno.ActualizarMuchosAsync(clasesDisponibles);
            await repoPago.AgregarAsync(nuevoPago);
        }
    }
}