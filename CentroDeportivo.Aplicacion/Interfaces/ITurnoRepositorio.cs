using CentroDeportivo.Aplicacion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Interfaces
{
    public interface ITurnoRepositorio
    {
        Task<Turno?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Turno>> BuscarTurnosAsync(DateOnly? fecha, int? actividadId, int? profeId, int? canchaId,EstadoTurno? estado);
        Task<IEnumerable<Turno>> ObtenerParaCalendarioAsync(int idUsuario, int? actividad);
        Task FinalizarTurnosVencidosAsync();
        Task<IEnumerable<Turno>> ObtenerTodosAsync();
        Task<IEnumerable<Turno>> ObtenerPorActividadAsync(int actividadId);
        Task<IEnumerable<Turno>> ObtenerPorFechaAsync(DateOnly fecha);
        Task<Turno?> ObtenerPorActividadFechaYHoraAsync(int actividadId, DateOnly fecha, TimeOnly horaInicio);
        Task<IReadOnlyList<Turno>> ObtenerPorActividadYFechaAsync(int actividadId, DateOnly fecha);
        Task<IEnumerable<Turno>> ObtenerPorProfesorAsync(int profesorId);
        Task<IEnumerable<Turno>> ObtenerPorCanchaAsync(int canchaId);
        Task<IEnumerable<Turno>> ObtenerDisponiblesAsync();
        Task AgregarAsync(Turno turno);
        Task ActualizarAsync(Turno turno);
        Task EliminarAsync(int id);
        Task<bool> TieneInscriptosAsync(int turnoId);
    }
}
