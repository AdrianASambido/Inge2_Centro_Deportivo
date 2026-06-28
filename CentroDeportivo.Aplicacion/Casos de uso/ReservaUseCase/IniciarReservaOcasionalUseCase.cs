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
            string urlExito = $"https://localhost:5001/Turnos/dia?volviMP=true&pagoExitoso=true&turnoId={reserva.Id_Turno}&userId={reserva.Id_Usuario}&ActividadId={turno.Id_Actividad}&Fecha={turno.Fecha:yyyy-MM-dd}";
            string urlFallo = $"https://localhost:5001/Turnos/dia?volviMP=true&pagoExitoso=false&turnoId={reserva.Id_Turno}&userId={reserva.Id_Usuario}&ActividadId={turno.Id_Actividad}&Fecha={turno.Fecha:yyyy-MM-dd}";

            string nombreActividad = $"Seña Turno Ocasional Nro {turno.Id}";

            string urlMercadoPago = await pagoServicio.CrearPreferenciaPagoAsync(
                reserva.Id_Usuario,
                montoSena,
                nombreActividad,
                urlExito,
                urlFallo
            );

            return urlMercadoPago;
        }
    }
}