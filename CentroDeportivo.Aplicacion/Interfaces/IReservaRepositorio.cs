using CentroDeportivo.Aplicacion.Casos_de_uso.EstadisticaUseCase.DTOs;
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
        Task<IEnumerable<Reserva>> ObtenerPorUsuarioAsync(int usuarioId, int? actividadId, EstadoReserva? estado, bool incluirPasadas);
        Task<IEnumerable<Reserva>> ObtenerPorTurnoAsync(int turnoId, string? dni);
        Task<bool> ExisteReservaActivaEnFechaYHoraAsync(int idUsuario, DateOnly fecha, TimeOnly horaInicio);
        Task<bool> TieneConflictoHorarioAsync(int usuarioId, DateOnly fecha, TimeOnly horarioInicio);
        Task AgregarAsync(Reserva reserva);
        Task<int> ContarCancelacionesUsuarioMesAsync(int idUsuario, int anio, int mes);
        Task ActualizarMuchasAsync(IEnumerable<Reserva> reservas);
        Task GuardarMuchasReservasAsync(List<Reserva> reservas);
        Task<bool> YaRenovoParaSiguienteMesAsync(int idUsuario, int idActividad, DayOfWeek diaSemana, TimeOnly horaInicio, int anioSiguiente,
    int mesSiguiente);

            Task ActualizarAsync(Reserva reserva);
        Task<bool> TieneExcesoCancelacionesAsync(int idUsuario, DateOnly fechaLimite);
        Task<ReporteIndicesActividadDTO> ObtenerTotalesPorActividadAsync(int idActividad, DateOnly desde, DateOnly hasta);
        // obtener reservas por código de paquete (para extraer datos de la clase)
        Task<List<Reserva>> ObtenerPorCodigoPaqueteAsync(Guid codigoPaquete);
        Task<List<Reserva>> ObtenerPaquetesAdelantatadosPorUsuarioAsync(int idUsuario);
        Task<IEnumerable<HistorialUsuarioActividadDTO>> ObtenerTotalesPorUsuarioAsync(int idUsuario, DateOnly? desde=null, DateOnly? hasta=null);
    }
}