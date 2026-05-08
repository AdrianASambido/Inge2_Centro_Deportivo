using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Casos_de_uso.TurnoUseCase;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase;

public class AceptarOfertaListaEsperaUseCase(
    ITurnoRepositorio turnoRepositorio,
    IReservaRepositorio reservaRepositorio,
    IListaEsperaRepositorio listaEsperaRepositorio,
    ProcesarOfertasListaEsperaVencidasUseCase procesarOfertasListaEsperaVencidas)
{
    /// <summary>Confirma la reserva cuando el cupo fue retenido por una oferta de lista de espera.</summary>
    public async Task<(bool exito, string mensaje, int? reservaId)> ejecutar(int usuarioId, int turnoId)
    {
        await procesarOfertasListaEsperaVencidas.ejecutar();

        var entrada = await listaEsperaRepositorio.ObtenerOfertaActivaUsuarioAsync(usuarioId, turnoId);
        if (entrada == null)
            return (false, "No tiene una oferta vigente para este turno o el plazo de 15 minutos venció.", null);

        if (await reservaRepositorio.ExisteReservaActivaAsync(usuarioId, turnoId))
            return (false, "Ya registramos una reserva activa en este turno.", null);

        var turno = await turnoRepositorio.ObtenerPorIdAsync(turnoId)
            ?? throw new Exception("Turno no encontrado.");

        var reserva = new Reserva
        {
            Id_Usuario = usuarioId,
            Id_Turno = turnoId,
            Estado = EstadoReserva.PendienteDePago,
            FechaReserva = DateOnly.FromDateTime(DateTime.Today),
            FechaAsistencia = turno.Fecha,
            PrecioPagado = 0
        };
        await reservaRepositorio.AgregarAsync(reserva);

        entrada.Estado = EstadoListaEspera.Cubierta;
        entrada.OfertaExpiraUtc = null;
        await listaEsperaRepositorio.ActualizarAsync(entrada);

        return (true, "Reserva exitosa", reserva.Id);
    }
}
