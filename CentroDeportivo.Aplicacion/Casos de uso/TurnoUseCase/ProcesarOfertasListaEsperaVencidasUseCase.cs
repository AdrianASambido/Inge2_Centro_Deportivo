using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.TurnoUseCase;

public class ProcesarOfertasListaEsperaVencidasUseCase(
    IListaEsperaRepositorio listaEsperaRepositorio,
    ITurnoRepositorio turnoRepositorio,
    OfertarSiguienteListaEsperaUseCase ofertarSiguienteListaEspera)
{
    public async Task ejecutar()
    {
        var ahora = DateTime.UtcNow;
        var vencidas = (await listaEsperaRepositorio.ObtenerOfertasVencidasAsync(ahora)).ToList();
        foreach (var entrada in vencidas)
        {
            entrada.Estado = EstadoListaEspera.OfertaVencida;
            entrada.OfertaExpiraUtc = null;
            await listaEsperaRepositorio.ActualizarAsync(entrada);

            var turno = await turnoRepositorio.ObtenerPorIdAsync(entrada.Id_Turno);
            if (turno == null)
                continue;

            turno.CupoDisponible++;
            if (turno.CupoDisponible > 0 && turno.Estado == EstadoTurno.Lleno)
                turno.Estado = EstadoTurno.Disponible;

            await turnoRepositorio.ActualizarAsync(turno);
            await ofertarSiguienteListaEspera.ejecutar(turno.Id);
        }
    }
}
