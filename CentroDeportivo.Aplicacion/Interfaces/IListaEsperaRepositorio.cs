using CentroDeportivo.Aplicacion.Entidades;

namespace CentroDeportivo.Aplicacion.Interfaces;

public interface IListaEsperaRepositorio
{
    Task<ListaEsperaEntrada?> ObtenerPorIdAsync(int id);
    Task AgregarAsync(ListaEsperaEntrada entrada);
    Task ActualizarAsync(ListaEsperaEntrada entrada);
    Task<IEnumerable<ListaEsperaEntrada>> ObtenerPorTurnoOrdenadasAsync(int turnoId);
    Task<ListaEsperaEntrada?> ObtenerPrimeraEnEsperaAsync(int turnoId);
    Task<bool> UsuarioYaEnListaActivaAsync(int usuarioId, int turnoId);
    Task<IEnumerable<ListaEsperaEntrada>> ObtenerOfertasVencidasAsync(DateTime ahoraUtc);
    Task<ListaEsperaEntrada?> ObtenerOfertaActivaUsuarioAsync(int usuarioId, int turnoId);
    Task<IReadOnlyList<ListaEsperaEntrada>> ObtenerOfertasActivasPorTurnoAsync(int turnoId);
}
