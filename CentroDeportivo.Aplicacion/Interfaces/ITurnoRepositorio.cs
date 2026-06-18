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
        Task<IEnumerable<Turno>> ObtenerParaCalendarioAsync(int idUsuario, DateOnly fecha, int actividad);
        Task FinalizarTurnosVencidosAsync();
        Task AgregarAsync(Turno turno);
        Task ActualizarAsync(Turno turno);
        Task EliminarAsync(int id);
        Task<List<Turno>> ObtenerTurnosDisponiblesRangoAsync(int idTurno, int idUsuario, DateOnly desde, DateOnly hasta);
        Task<bool> TieneInscriptosAsync(int turnoId);
        Task ActualizarMuchosAsync(List<Turno> turnos);
    }
}
