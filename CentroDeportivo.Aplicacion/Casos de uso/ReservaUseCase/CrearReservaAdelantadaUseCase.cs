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
    IPagoServicio pagoServicio
)
    {
        public async Task Ejecutar(int idUsuario, List<Turno> clasesDisponibles, string tarjetaToken)
        {
            DateOnly hoy = DateOnly.FromDateTime(DateTime.Now);

            if (clasesDisponibles == null || !clasesDisponibles.Any())
            {
                throw new Exception("No se proporcionaron clases válidas para reservar.");
            }

  
            if (clasesDisponibles.Any(t => t.CupoDisponible <= 0))
            {
                throw new Exception("Una o más clases del mes ya no cuentan con cupo disponible. Operación cancelada.");
            }

            Guid codigoPaquete = Guid.NewGuid();
            decimal precioBaseTurno = clasesDisponibles.First().PrecioTurno;
            decimal precioConDescuento = precioBaseTurno * 0.80m;
            decimal montoTotalAPagar = precioConDescuento * clasesDisponibles.Count;


            bool cobroExitoso = await pagoServicio.ProcesarCobroAsync(idUsuario, montoTotalAPagar, tarjetaToken);
            if (!cobroExitoso)
            {
                throw new Exception("El pago del paquete adelantado fue rechazado por la entidad bancaria. Operación cancelada.");
            }

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
                if (turnoClase.CupoDisponible == 0)
                {
                    turnoClase.Estado = EstadoTurno.Lleno;
                }
            }

            var nuevoPago = new Pago
            {
                Id_Usuario = idUsuario,
                Monto = montoTotalAPagar,
                Fecha = DateTime.Now,
                CodigoPaqueteAdelantado = codigoPaquete
            };


            await repoReserva.GuardarMuchasReservasAsync(nuevasReservas);
            await repoTurno.ActualizarMuchosAsync(clasesDisponibles);
            await repoPago.AgregarAsync(nuevoPago);
        }
    }
}