using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Validadores;
using System;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Entidades;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase
{
    public class IniciarReservaOcasionalUseCase(
        ITurnoRepositorio repoTurno,
        IPagoServicio pagoServicio,
        ReservaValidador validador)
    {
        public async Task<string> Ejecutar(Reserva reserva)
        {

            var (esValido, mensaje) = await validador.ValidarReserva(reserva);
            if (!esValido)
            {
                throw new Exception(mensaje);
            }

            var turno = await repoTurno.ObtenerPorIdAsync(reserva.Id_Turno);
            if (turno == null)
            {
                throw new Exception("El turno especificado no existe.");
            }

            if (turno.CupoDisponible <= 0)
            {
                throw new Exception("No hay cupos disponibles para este turno.");
            }

            decimal montoSena = turno.PrecioTurno / 2;
            string urlExito = $"https://localhost:7001/pago-exitoso?tipo=ocasional&turnoId={reserva.Id_Turno}";

            string nombreActividad = $"Seña Turno Ocasional Nro {turno.Id}";

            string urlMercadoPago = await pagoServicio.CrearPreferenciaPagoAsync(
                reserva.Id_Usuario,
                montoSena,
                nombreActividad,
                urlExito
            );

            return urlMercadoPago;
        }
    }
}