using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Infraestructura.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;

namespace CentroDeportivo.Infraestructura.Persistencia.Repositorios;

public class ReservaRepositorio(CentroDeportivoContext contexto) : IReservaRepositorio
{
    private static readonly EstadoReserva[] EstadosOcupanCupo = [EstadoReserva.Reservado, EstadoReserva.Confirmado, EstadoReserva.PendienteDePago];

    public async Task ActualizarAsync(Reserva reserva)
    {
        contexto.Reservas.Update(reserva);
        await contexto.SaveChangesAsync();
    }

    public async Task AgregarAsync(Reserva reserva)
    {
        await contexto.Reservas.AddAsync(reserva);
        await contexto.SaveChangesAsync();
    }

    public async Task<bool> ExisteReservaActivaAsync(int usuarioId, int turnoId)
    {
        return await contexto.Reservas.AnyAsync(r =>
            r.Id_Usuario == usuarioId &&
            r.Id_Turno == turnoId &&
            EstadosOcupanCupo.Contains(r.Estado));
    }

    public async Task<IEnumerable<Reserva>> ObtenerConDevolucionPendienteAsync()
    {
        return await contexto.Reservas
            .AsNoTracking()
            .Include(r => r.Turno)
            .Include(r => r.Usuario)
            .ToListAsync();
    }

    public async Task<Reserva?> ObtenerPorIdAsync(int id)
    {
        return await contexto.Reservas
            .Include(r => r.Turno)
            .Include(r => r.Usuario)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Reserva?> ObtenerPorQrTokenAsync(string qrToken)
    {
        return await contexto.Reservas
            .Include(r => r.Turno)
            .Include(r => r.Usuario)
            .FirstOrDefaultAsync(r => r.TokenQr == qrToken);
    }

    public async Task<IEnumerable<Reserva>> ObtenerPorTurnoAsync(int turnoId)
    {
        return await contexto.Reservas
            .AsNoTracking()
            .Include(r => r.Usuario)
            .Where(r => r.Id_Turno == turnoId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reserva>> ObtenerPorUsuarioAsync(int usuarioId)
    {
        return await contexto.Reservas
            .AsNoTracking()
            .Include(r => r.Turno)!.ThenInclude(t => t!.Actividad)
            .Where(r => r.Id_Usuario == usuarioId)
            .ToListAsync();
    }

    public async Task<bool> TieneConflictoHorarioAsync(int usuarioId, DateOnly fecha, TimeOnly horarioInicio)
    {
        return await contexto.Reservas
            .Where(r => r.Id_Usuario == usuarioId && EstadosOcupanCupo.Contains(r.Estado))
            .Join(
                contexto.Turnos,
                r => r.Id_Turno,
                t => t.Id,
                (r, t) => t)
            .AnyAsync(t => t.Fecha == fecha && t.HoraInicio == horarioInicio);
    }

    public async Task<int> ContarActivasPorTurnoAsync(int turnoId)
    {
        return await contexto.Reservas.CountAsync(r =>
            r.Id_Turno == turnoId &&
            EstadosOcupanCupo.Contains(r.Estado));
    }

    public async Task<IReadOnlyList<Reserva>> ObtenerPendientesDePagoAsync()
    {
        return await contexto.Reservas
            .AsNoTracking()
            .Include(r => r.Usuario)
            .Include(r => r.Turno)!.ThenInclude(t => t!.Actividad)
            .Where(r => r.Estado == EstadoReserva.PendienteDePago)
            .OrderBy(r => r.FechaReserva)
            .ToListAsync();
    }
}
