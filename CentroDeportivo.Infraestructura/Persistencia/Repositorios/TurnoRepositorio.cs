using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Infraestructura.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;

namespace CentroDeportivo.Infraestructura.Persistencia.Repositorios;

public class TurnoRepositorio(CentroDeportivoContext contexto) : ITurnoRepositorio
{
    public async Task ActualizarAsync(Turno turno)
    {
        contexto.Turnos.Update(turno);
        await contexto.SaveChangesAsync();
    }

    public async Task AgregarAsync(Turno turno)
    {
        await contexto.Turnos.AddAsync(turno);
        await contexto.SaveChangesAsync();
    }

    public async Task EliminarAsync(int id)
    {
        var t = await contexto.Turnos.FindAsync(id);
        if (t != null)
        {
            contexto.Turnos.Remove(t);
            await contexto.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Turno>> ObtenerDisponiblesAsync()
    {
        return await contexto.Turnos
            .AsNoTracking()
            .Include(t => t.Actividad)
            .Include(t => t.Cancha)
            .Include(t => t.Profesor)
            .Where(t => t.Estado == EstadoTurno.Disponible && t.CupoDisponible > 0)
            .ToListAsync();
    }

    public async Task<IEnumerable<Turno>> ObtenerPorActividadAsync(int actividadId)
    {
        return await contexto.Turnos
            .AsNoTracking()
            .Include(t => t.Actividad)
            .Include(t => t.Cancha)
            .Include(t => t.Profesor)
            .Where(t => t.Id_Actividad == actividadId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Turno>> ObtenerPorCanchaAsync(int canchaId)
    {
        return await contexto.Turnos
            .AsNoTracking()
            .Include(t => t.Actividad)
            .Include(t => t.Cancha)
            .Include(t => t.Profesor)
            .Where(t => t.Id_Cancha == canchaId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Turno>> ObtenerPorFechaAsync(DateOnly fecha)
    {
        return await contexto.Turnos
            .AsNoTracking()
            .Include(t => t.Actividad)
            .Include(t => t.Cancha)
            .Include(t => t.Profesor)
            .Where(t => t.Fecha == fecha)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Turno>> ObtenerPorActividadYFechaAsync(int actividadId, DateOnly fecha)
    {
        return await contexto.Turnos
            .AsNoTracking()
            .Include(t => t.Actividad)
            .Include(t => t.Cancha)
            .Include(t => t.Profesor)
            .Where(t => t.Id_Actividad == actividadId && t.Fecha == fecha)
            .OrderBy(t => t.HoraInicio)
            .ToListAsync();
    }

    public async Task<Turno?> ObtenerPorActividadFechaYHoraAsync(int actividadId, DateOnly fecha, TimeOnly horaInicio)
    {
        return await contexto.Turnos
            .Include(t => t.Actividad)
            .Include(t => t.Cancha)
            .Include(t => t.Profesor)
            .FirstOrDefaultAsync(t =>
                t.Id_Actividad == actividadId &&
                t.Fecha == fecha &&
                t.HoraInicio == horaInicio);
    }

    public async Task<Turno?> ObtenerPorIdAsync(int id)
    {
        return await contexto.Turnos
            .Include(t => t.Actividad)
            .Include(t => t.Cancha)
            .Include(t => t.Profesor)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Turno>> ObtenerPorProfesorAsync(int profesorId)
    {
        return await contexto.Turnos
            .AsNoTracking()
            .Include(t => t.Actividad)
            .Include(t => t.Cancha)
            .Include(t => t.Profesor)
            .Where(t => t.Id_Profesor == profesorId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Turno>> ObtenerTodosAsync()
    {
        return await contexto.Turnos
            .AsNoTracking()
            .Include(t => t.Actividad)
            .Include(t => t.Cancha)
            .Include(t => t.Profesor)
            .ToListAsync();
    }

    public async Task<bool> TieneInscriptosAsync(int turnoId)
    {
        return await contexto.Reservas.AnyAsync(r =>
            r.Id_Turno == turnoId &&
            r.Estado != EstadoReserva.Cancelado);
    }
}
