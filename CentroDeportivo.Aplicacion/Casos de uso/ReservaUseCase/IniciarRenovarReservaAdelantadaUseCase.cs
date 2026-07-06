using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase
{
    public class IniciarRenovarReservaAdelantadaUseCase(
    IReservaRepositorio repoReserva,
    ITurnoRepositorio repoTurno,
    IUsuarioRepositorio repoUsuario,
    IPagoServicio pagoServicio)
    {


        public async Task<string> Ejecutar(int idUsuario, Guid codigoPaquete)
        {

            var reservasPaquete = await repoReserva.ObtenerPorCodigoPaqueteAsync(codigoPaquete);
            if (!reservasPaquete.Any())
                throw new Exception("No se encontró el paquete de reservas.");

            var primeraReserva = reservasPaquete.First();
            var turnoReferencia = primeraReserva.Turno
                ?? throw new Exception("No se pudo obtener el turno de referencia.");

   
            var fechaReferencia = turnoReferencia.Fecha;
            int anioSiguiente = fechaReferencia.Month == 12 ? fechaReferencia.Year + 1 : fechaReferencia.Year;
            int mesSiguiente = fechaReferencia.Month == 12 ? 1 : fechaReferencia.Month + 1;

            var turnosSiguienteMes = await repoTurno.ObtenerTurnosSiguienteMesPorClaseAsync(
                turnoReferencia.Id_Actividad,
                turnoReferencia.Fecha.DayOfWeek,
                turnoReferencia.HoraInicio,
                turnoReferencia.Id_Profesor,
                turnoReferencia.Id_Cancha,
                anioSiguiente,
                mesSiguiente);

            var nombreMes = new DateTime(anioSiguiente, mesSiguiente, 1)
            .ToString("MMMM yyyy", new CultureInfo("es-AR"));

            if (!turnosSiguienteMes.Any())
                throw new Exception($"No hay turnos disponibles para esta clase en {nombreMes}. Es posible que aún no estén cargados.");


            foreach (var turno in turnosSiguienteMes)
            {
                bool hayConflictoHorario = await repoReserva.ExisteReservaActivaEnFechaYHoraAsync(
                    idUsuario, turno.Fecha, turno.HoraInicio);

                if (hayConflictoHorario)
                    throw new Exception("No es posible renovar porque tenés una reserva activa que se superpone con alguna clase del próximo mes.");

                if (turno.CupoDisponible <= 0)
                    throw new Exception("No es posible renovar porque alguna clase del próximo mes no tiene cupo disponible.");
            }


            var turnosDisponibles = turnosSiguienteMes;


            var usuario = await repoUsuario.ObtenerPorIdAsync(idUsuario)
                ?? throw new Exception("Usuario no encontrado.");

            decimal precioBase = turnosDisponibles.First().PrecioTurno;
            decimal factor = usuario.TieneSancionDescuento ? 1.0m : 0.80m;
            decimal precioConDescuento = precioBase * factor;
            decimal montoTotal = precioConDescuento * turnosDisponibles.Count;

            string stringIdsTurnos = string.Join(",", turnosDisponibles.Select(t => t.Id));
            string nombreProducto = $"Renovación Mensual - {turnoReferencia.Actividad?.Nombre} {turnoReferencia.Fecha.DayOfWeek} {turnoReferencia.HoraInicio:HH\\:mm}";

            string urlExito = $"https://localhost:5001/MisReservas?pagoExitoso=true&tipo=renovacion&turnosId={stringIdsTurnos}&userId={idUsuario}";
            string urlFallo = $"https://localhost:5001/MisReservas?pagoExitoso=false&tipo=renovacion&turnosId={stringIdsTurnos}&userId={idUsuario}";

            return await pagoServicio.CrearPreferenciaPagoAsync(
                idUsuario, montoTotal, nombreProducto, urlExito, urlFallo);
        }
    }
}
