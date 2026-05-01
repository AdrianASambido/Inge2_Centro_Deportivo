using CentroDeportivo.Aplicacion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Interfaces
{
    public interface IReservaRepositorio
    {
        Task<Reserva?> ObtenerPorIdAsync(int id);
        Task<Reserva?> ObtenerPorQrTokenAsync(string qrToken);
        Task<IEnumerable<Reserva>> ObtenerPorUsuarioAsync(int usuarioId);
        Task<IEnumerable<Reserva>> ObtenerPorTurnoAsync(int turnoId);
        Task<IEnumerable<Reserva>> ObtenerConDevolucionPendienteAsync();
        Task<bool> ExisteReservaActivaAsync(int usuarioId, int turnoId);
        Task<bool> TieneConflictoHorarioAsync(int usuarioId, DateOnly fecha, TimeOnly horarioInicio);
        Task AgregarAsync(Reserva reserva);
        Task ActualizarAsync(Reserva reserva);
    }
}

