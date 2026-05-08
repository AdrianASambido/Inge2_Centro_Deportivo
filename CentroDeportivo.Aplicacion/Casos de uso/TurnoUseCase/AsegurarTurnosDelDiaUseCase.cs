using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.TurnoUseCase;

public class AsegurarTurnosDelDiaUseCase(
    ITurnoRepositorio turnoRepositorio,
    IActividadRepositorio actividadRepositorio,
    ICanchaRepositorio canchaRepositorio,
    IProfesorRepositorio profesorRepositorio)
{
    /// <summary>Garantiza turnos horarios (16–22) para la actividad y fecha indicadas.</summary>
    public async Task ejecutar(int actividadId, DateOnly fecha)
    {
        _ = await actividadRepositorio.ObtenerPorIdAsync(actividadId)
            ?? throw new Exception("Actividad no encontrada.");

        var cancha = (await canchaRepositorio.ObtenerTodasAsync()).OrderBy(c => c.Id).FirstOrDefault()
            ?? throw new Exception("No hay canchas cargadas. Agregue al menos una cancha.");
        var profe = (await profesorRepositorio.ObtenerTodosAsync()).OrderBy(p => p.Id).FirstOrDefault()
            ?? throw new Exception("No hay profesores cargados. Agregue al menos un profesor.");

        for (var h = 16; h <= 22; h++)
        {
            var inicio = new TimeOnly(h, 0);
            var existente = await turnoRepositorio.ObtenerPorActividadFechaYHoraAsync(actividadId, fecha, inicio);
            if (existente != null)
                continue;

            var fin = h < 23 ? new TimeOnly(h + 1, 0) : new TimeOnly(23, 0);
            var turno = new Turno
            {
                Fecha = fecha,
                HoraInicio = inicio,
                HoraFin = fin,
                CupoMaximo = 10,
                CupoDisponible = 10,
                Estado = EstadoTurno.Disponible,
                Id_Actividad = actividadId,
                Id_Cancha = cancha.Id,
                Id_Profesor = profe.Id
            };
            await turnoRepositorio.AgregarAsync(turno);
        }
    }
}
