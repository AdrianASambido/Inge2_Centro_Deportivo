using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.TurnoUseCase;

public class ListarTurnosCalendarioUseCase(
    AsegurarTurnosDelDiaUseCase asegurarTurnosDelDia,
    ITurnoRepositorio turnoRepositorio)
{
    public async Task<IReadOnlyList<Turno>> ejecutar(int actividadId, DateOnly fecha)
    {
        await asegurarTurnosDelDia.ejecutar(actividadId, fecha);
        return await turnoRepositorio.ObtenerPorActividadYFechaAsync(actividadId, fecha);
    }
}
