using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase;

public class CancelarReservaUseCase(
    IReservaRepositorio reservaRepositorio,
    ITurnoRepositorio turnoRepositorio)
{
    public async Task ejecutar(int reservaId)
    {
        var reserva = await reservaRepositorio.ObtenerPorIdAsync(reservaId)
            ?? throw new Exception("Reserva no encontrada.");

        if (reserva.Estado == EstadoReserva.Cancelado)
            return;

        reserva.Estado = EstadoReserva.Cancelado;
        await reservaRepositorio.ActualizarAsync(reserva);

        var turno = await turnoRepositorio.ObtenerPorIdAsync(reserva.Id_Turno);
        if (turno == null || turno.Estado == EstadoTurno.Cancelado)
            return;

        turno.CupoDisponible++;
        if (turno.CupoDisponible > 0 && turno.Estado == EstadoTurno.Lleno)
            turno.Estado = EstadoTurno.Disponible;

        await turnoRepositorio.ActualizarAsync(turno);
    }
}
