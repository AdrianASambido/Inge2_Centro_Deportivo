using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Casos_de_uso.TurnoUseCase;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase;

public class CrearReservaUseCase(
    ITurnoRepositorio turnoRepositorio,
    IReservaRepositorio reservaRepositorio,
    IListaEsperaRepositorio listaEsperaRepositorio,
    ProcesarOfertasListaEsperaVencidasUseCase procesarOfertasListaEsperaVencidas)
{
    public async Task<(bool exito, string mensaje, int? reservaId, bool enListaEspera)> ejecutar(int usuarioId, int turnoId)
    {
        await procesarOfertasListaEsperaVencidas.ejecutar();

        var turno = await turnoRepositorio.ObtenerPorIdAsync(turnoId)
            ?? throw new Exception("Turno no encontrado.");

        if (turno.Estado == EstadoTurno.Cancelado)
            return (false, "Este turno fue cancelado.", null, false);

        if (await reservaRepositorio.ExisteReservaActivaAsync(usuarioId, turnoId))
            return (false, "Ya tiene una reserva activa en este turno.", null, false);

        if (await reservaRepositorio.TieneConflictoHorarioAsync(usuarioId, turno.Fecha, turno.HoraInicio))
            return (false, "Ya tiene otra reserva en el mismo horario.", null, false);

        if (turno.CupoDisponible <= 0)
        {
            if (await listaEsperaRepositorio.UsuarioYaEnListaActivaAsync(usuarioId, turnoId))
                return (false, "Ya está en la lista de espera de este turno.", null, false);

            await listaEsperaRepositorio.AgregarAsync(new ListaEsperaEntrada
            {
                Id_Turno = turnoId,
                Id_Usuario = usuarioId,
                FechaAltaUtc = DateTime.UtcNow,
                Estado = EstadoListaEspera.EnEspera
            });
            return (false, "No hay cupo en este horario. Quedó en lista de espera; le avisaremos si se libera un lugar.", null, true);
        }

        turno.CupoDisponible--;
        if (turno.CupoDisponible <= 0)
            turno.Estado = EstadoTurno.Lleno;
        await turnoRepositorio.ActualizarAsync(turno);

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

        return (true, "Reserva exitosa", reserva.Id, false);
    }
}
