using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Entidades;

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