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
    IPagoRepositorio repoPago

)
    { 
        public async Task Ejecutar(int idUsuario, List<Turno> clasesDisponibles, string idPayment)
        {
            DateOnly hoy = DateOnly.FromDateTime(DateTime.Now);

            Guid codigoPaquete = Guid.NewGuid();

            decimal precioBaseTurno = clasesDisponibles.First().PrecioTurno;
            decimal precioConDescuento = precioBaseTurno * 0.80m;
            decimal montoTotalAPagar = precioConDescuento * clasesDisponibles.Count;

            List<Reserva> nuevasReservas = new List<Reserva>();

            foreach (var turnoClase in clasesDisponibles)
            {
                var reserva = new Reserva
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
                };

                nuevasReservas.Add(reserva);

                turnoClase.CupoDisponible--;
                if (turnoClase.CupoDisponible <= 0)
                {
                    turnoClase.Estado = EstadoTurno.Lleno;
                }
            }

   
            var nuevoPago = new Pago(idUsuario, montoTotalAPagar, null, null, codigoPaquete);
            nuevoPago.MercadoPagoTransactionId = idPayment;
    
            await repoReserva.GuardarMuchasReservasAsync(nuevasReservas);
            await repoTurno.ActualizarMuchosAsync(clasesDisponibles);
            await repoPago.AgregarAsync(nuevoPago);
        }
    }
}