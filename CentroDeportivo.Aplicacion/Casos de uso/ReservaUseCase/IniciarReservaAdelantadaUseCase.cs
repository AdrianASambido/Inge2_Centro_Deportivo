using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase
{

    public class IniciarReservaAdelantadaUseCase(
        ITurnoRepositorio repoTurno,
        IPagoServicio pagoServicio
    )
    {
        public async Task<string> Ejecutar(int idUsuario, List<Turno> clasesDisponibles)
        {
            if (clasesDisponibles == null || !clasesDisponibles.Any())
                throw new Exception("No se proporcionaron clases válidas para reservar.");

            if (clasesDisponibles.Any(t => t.CupoDisponible <= 0))
                throw new Exception("Una o más clases del mes ya no cuentan con cupo disponible. Operación cancelada.");

            decimal precioBaseTurno = clasesDisponibles.First().PrecioTurno;
            decimal precioConDescuento = precioBaseTurno * 0.80m;
            decimal montoTotalAPagar = precioConDescuento * clasesDisponibles.Count;

            string stringIdsTurnos = string.Join(",", clasesDisponibles.Select(t => t.Id));

            string urlExito = $"https://localhost:5001/Turno/adelantada?volviMP=true&pagoExitoso=true&turnosId={stringIdsTurnos}&userId={idUsuario}&ActividadId={clasesDisponibles.First().Id_Actividad}&DiaSemana={(int)clasesDisponibles.First().Fecha.DayOfWeek}";
            string urlFallo = $"https://localhost:5001/Turno/adelantada?volviMP=true&pagoExitoso=false&turnosId={stringIdsTurnos}&userId={idUsuario}&ActividadId={clasesDisponibles.First().Id_Actividad}&DiaSemana={(int)clasesDisponibles.First().Fecha.DayOfWeek}";

            string nombreProducto = $"Abono Mensual Adelantado ({clasesDisponibles.Count} Clases)";

            string urlMercadoPago = await pagoServicio.CrearPreferenciaPagoAsync(
                idUsuario,
                montoTotalAPagar,
                nombreProducto,
                urlExito,
                urlFallo
            );

            return urlMercadoPago;
        }
    }
}