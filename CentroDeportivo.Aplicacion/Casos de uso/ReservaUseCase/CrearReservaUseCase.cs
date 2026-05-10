using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Entidades;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase;

public class CrearReservaUseCase(
    ITurnoRepositorio turnoRepositorio,
    IReservaRepositorio reservaRepositorio)
{
    public async Task<(bool exito, string mensaje, int? reservaId)> ejecutar(int usuarioId, int turnoId)
    {
        var turno = await turnoRepositorio.ObtenerPorIdAsync(turnoId)
            ?? throw new Exception("Turno no encontrado.");

        if (turno.Estado == EstadoTurno.Cancelado)
            return (false, "Este turno fue cancelado.", null);

        if (await reservaRepositorio.ExisteReservaActivaAsync(usuarioId, turnoId))
            return (false, "Ya tiene una reserva activa en este turno.", null);

        if (await reservaRepositorio.TieneConflictoHorarioAsync(usuarioId, turno.Fecha, turno.HoraInicio))
            return (false, "Ya tiene otra reserva en el mismo horario.", null);

        if (turno.CupoDisponible <= 0)
            return (false, "No hay cupo en este horario.", null);

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

        return (true, "Reserva exitosa", reserva.Id);
    }
}
