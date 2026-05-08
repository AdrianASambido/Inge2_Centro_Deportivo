using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.TurnoUseCase;

public class OfertarSiguienteListaEsperaUseCase(
    ITurnoRepositorio turnoRepositorio,
    IListaEsperaRepositorio listaEsperaRepositorio,
    IUsuarioRepositorio usuarioRepositorio,
    IEmailServicio emailServicio)
{
    public async Task ejecutar(int turnoId)
    {
        var ofertasActivas = await listaEsperaRepositorio.ObtenerOfertasActivasPorTurnoAsync(turnoId);
        if (ofertasActivas.Count > 0)
            return;

        var turno = await turnoRepositorio.ObtenerPorIdAsync(turnoId);
        if (turno == null || turno.Estado == EstadoTurno.Cancelado)
            return;

        if (turno.CupoDisponible <= 0)
            return;

        var siguiente = await listaEsperaRepositorio.ObtenerPrimeraEnEsperaAsync(turnoId);
        if (siguiente == null)
            return;

        siguiente.Estado = EstadoListaEspera.OfertaEnviada;
        siguiente.OfertaExpiraUtc = DateTime.UtcNow.AddMinutes(15);
        await listaEsperaRepositorio.ActualizarAsync(siguiente);

        turno.CupoDisponible--;
        if (turno.CupoDisponible <= 0)
            turno.Estado = EstadoTurno.Lleno;
        await turnoRepositorio.ActualizarAsync(turno);

        var usuario = await usuarioRepositorio.ObtenerPorIdAsync(siguiente.Id_Usuario);
        if (usuario != null && !string.IsNullOrWhiteSpace(usuario.Email))
            await emailServicio.EnviarCupoListaEsperaAsync(usuario.Email, turno, 15);
    }
}
