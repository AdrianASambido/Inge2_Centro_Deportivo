using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Infraestructura.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;

namespace CentroDeportivo.Infraestructura.Persistencia.Repositorios;

public class ListaEsperaRepositorio(CentroDeportivoContext contexto) : IListaEsperaRepositorio
{
    public async Task ActualizarAsync(ListaEsperaEntrada entrada)
    {
        contexto.ListaEsperaEntradas.Update(entrada);
        await contexto.SaveChangesAsync();
    }

    public async Task AgregarAsync(ListaEsperaEntrada entrada)
    {
        await contexto.ListaEsperaEntradas.AddAsync(entrada);
        await contexto.SaveChangesAsync();
    }

    public async Task<ListaEsperaEntrada?> ObtenerPorIdAsync(int id)
    {
        return await contexto.ListaEsperaEntradas
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<ListaEsperaEntrada?> ObtenerPrimeraEnEsperaAsync(int turnoId)
    {
        return await contexto.ListaEsperaEntradas
            .Where(e => e.Id_Turno == turnoId && e.Estado == EstadoListaEspera.EnEspera)
            .OrderBy(e => e.FechaAltaUtc)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ListaEsperaEntrada>> ObtenerPorTurnoOrdenadasAsync(int turnoId)
    {
        return await contexto.ListaEsperaEntradas
            .AsNoTracking()
            .Where(e => e.Id_Turno == turnoId)
            .OrderBy(e => e.FechaAltaUtc)
            .ToListAsync();
    }

    public async Task<bool> UsuarioYaEnListaActivaAsync(int usuarioId, int turnoId)
    {
        return await contexto.ListaEsperaEntradas.AnyAsync(e =>
            e.Id_Turno == turnoId &&
            e.Id_Usuario == usuarioId &&
            (e.Estado == EstadoListaEspera.EnEspera || e.Estado == EstadoListaEspera.OfertaEnviada));
    }

    public async Task<IEnumerable<ListaEsperaEntrada>> ObtenerOfertasVencidasAsync(DateTime ahoraUtc)
    {
        return await contexto.ListaEsperaEntradas
            .Where(e => e.Estado == EstadoListaEspera.OfertaEnviada &&
                        e.OfertaExpiraUtc != null &&
                        e.OfertaExpiraUtc < ahoraUtc)
            .ToListAsync();
    }

    public async Task<ListaEsperaEntrada?> ObtenerOfertaActivaUsuarioAsync(int usuarioId, int turnoId)
    {
        var ahora = DateTime.UtcNow;
        return await contexto.ListaEsperaEntradas
            .FirstOrDefaultAsync(e =>
                e.Id_Turno == turnoId &&
                e.Id_Usuario == usuarioId &&
                e.Estado == EstadoListaEspera.OfertaEnviada &&
                e.OfertaExpiraUtc != null &&
                e.OfertaExpiraUtc >= ahora);
    }

    public async Task<IReadOnlyList<ListaEsperaEntrada>> ObtenerOfertasActivasPorTurnoAsync(int turnoId)
    {
        var ahora = DateTime.UtcNow;
        return await contexto.ListaEsperaEntradas
            .Where(e =>
                e.Id_Turno == turnoId &&
                e.Estado == EstadoListaEspera.OfertaEnviada &&
                e.OfertaExpiraUtc != null &&
                e.OfertaExpiraUtc >= ahora)
            .ToListAsync();
    }
}
