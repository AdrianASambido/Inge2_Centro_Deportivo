using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Infraestructura.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Infraestructura.Persistencia.Repositorios
{
    public class TurnoRepositorio(CentroDeportivoContext contexto) : ITurnoRepositorio
    {
        public async Task ActualizarAsync(Turno turno)
        {
            contexto.Turnos.Update(turno);
            await contexto.SaveChangesAsync();
        }

        public async Task AgregarAsync(Turno turno)
        {
            await contexto.AddAsync(turno);
            await contexto.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var turno = await contexto.Turnos.FindAsync(id);
            if (turno != null)
            {
                contexto.Turnos.Remove(turno);
                await contexto.SaveChangesAsync();
            }
        }

        //para el empleado 
        public async Task<IEnumerable<Turno>> BuscarTurnosAsync(DateOnly? fecha, int? actividadId, int? profeId, int? canchaId, EstadoTurno? estado)
        {
            var query = contexto.Turnos.Where(t => t.Estado != EstadoTurno.Finalizado && t.Estado != EstadoTurno.Cancelado)
                .Include(t => t.Actividad)
                .Include(t => t.Profesor)
                .Include(t => t.Cancha)
                .Include(t => t.Reservas)
                .AsQueryable(); // encadena filtros

            if (fecha.HasValue)
                query = query.Where(t => t.Fecha == fecha.Value);

            if (actividadId.HasValue)
                query = query.Where(t => t.Id_Actividad == actividadId.Value);

            if (profeId.HasValue)
                query = query.Where(t => t.Id_Profesor == profeId.Value);

            if (canchaId.HasValue)
            {
                query = query.Where(t => t.Id_Cancha == canchaId.Value);
            }

            if (estado.HasValue)
                query = query.Where(t => t.Estado == estado.Value);

            return await query
                .OrderByDescending(t => t.Fecha) // Los más recientes primero
                .ThenBy(t => t.HoraInicio)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Turno?> ObtenerPorIdAsync(int id)
        {
            return await contexto.Turnos
                .Include(t => t.Actividad)
                .Include(t => t.Profesor)
                .Include(t => t.Cancha)
                .Include(t => t.Reservas)
                    .ThenInclude(r => r.Usuario) 
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<bool> TieneInscriptosAsync(int turnoId)
        {
            return await contexto.Turnos
                .AnyAsync(t => t.Id == turnoId
                            && t.Reservas.Any(r => r.Estado == EstadoReserva.Confirmado
                                               || r.Estado == EstadoReserva.Reservado));
        }


        public async Task<IEnumerable<Turno>> ObtenerParaCalendarioAsync(int usuarioId, DateOnly fecha, int actividadId)
        {
            var hoy = DateOnly.FromDateTime(DateTime.Now);

            var reservasCliente = await contexto.Reservas
                .Where(r => r.Id_Usuario == usuarioId &&
                            r.Turno.Fecha == fecha &&
                            (r.Estado == EstadoReserva.Confirmado || r.Estado == EstadoReserva.PendienteDePago || r.Estado == EstadoReserva.Reservado))
                .Select(r => new { r.Turno.HoraInicio, r.Turno.HoraFin })
                .ToListAsync();

            var turnosEnListaEspera = await contexto.InscripcionListaEsperas
       .Where(i => i.Id_Usuario == usuarioId &&
                   (i.Estado == EstadoListaEspera.Esperando ||
                    i.Estado == EstadoListaEspera.Notificado))
       .Select(i => i.Id_Turno)
       .ToListAsync();

            var turnosCandidatos = await contexto.Turnos
                .Include(t => t.Actividad)
                .Include(t => t.Profesor)
                .Include(t => t.Cancha)
                .Where(t => t.Id_Actividad == actividadId
                         && t.Fecha == fecha
                         && (t.Estado == EstadoTurno.Disponible || t.Estado == EstadoTurno.Lleno)
                         && !turnosEnListaEspera.Contains(t.Id)
                         && t.Fecha >= hoy)
                .ToListAsync();

           

            var filtrados = turnosCandidatos.Where(t =>
                !reservasCliente.Any(r =>

                    t.HoraInicio < r.HoraFin && t.HoraFin > r.HoraInicio
                )
            );

            return filtrados;
        }

        public async Task FinalizarTurnosVencidosAsync()
        {
            var hoy = DateOnly.FromDateTime(DateTime.Now);
            var ahora = TimeOnly.FromDateTime(DateTime.Now);

            // Se buscan turnos que
            //  Sean de días anteriores o
            //  Sean de hoy pero la hora de fin ya pasó
            //  Y que todavía no estén en estado Finalizado o Cancelado

            var turnosAFinalizar = await contexto.Turnos
                .Where(t => (t.Fecha < hoy || (t.Fecha == hoy && t.HoraFin < ahora))
                       && t.Estado != EstadoTurno.Finalizado
                       && t.Estado != EstadoTurno.Cancelado)
                .ToListAsync();

            foreach (var turno in turnosAFinalizar)
            {
                turno.Estado = EstadoTurno.Finalizado;
            }

            await contexto.SaveChangesAsync();
        }

        public async Task<List<Turno>> ObtenerTurnosDisponiblesRangoAsync(
      int idActividad,
      DayOfWeek diaSemana,
      int idUsuario,
      DateOnly desde,
      DateOnly hasta)
        {
            return await contexto.Turnos
                .Include(t => t.Profesor)
                .Include(t => t.Cancha)
                .Where(t => t.Id_Actividad == idActividad
                         && t.Estado == EstadoTurno.Disponible
                         && t.Fecha >= desde
                         && t.Fecha <= hasta
                         && t.Fecha.DayOfWeek == diaSemana
                         && !contexto.Reservas.Any(r => r.Id_Usuario == idUsuario
                                                     && r.Estado != EstadoReserva.Cancelado
                                                     && r.Turno.Fecha == t.Fecha
                                                     && r.Turno.HoraInicio < t.HoraFin
                                                     && r.Turno.HoraFin > t.HoraInicio))
                .OrderBy(t => t.Fecha)
                .ToListAsync();
        }

        public async Task ActualizarMuchosAsync(List<Turno> turnos)
        {
            if (turnos == null || !turnos.Any()) return;

            contexto.Turnos.UpdateRange(turnos);

            await contexto.SaveChangesAsync();
        }

        public async Task<List<Turno>> ObtenerTurnosSiguienteMesPorClaseAsync(
    int idActividad, DayOfWeek diaSemana, TimeOnly horaInicio,
    int idProfesor, int idCancha, int anio, int mes)
        {
            DateOnly inicio = new DateOnly(anio, mes, 1);
            DateOnly fin = new DateOnly(anio, mes, DateTime.DaysInMonth(anio, mes));

            return await contexto.Turnos
                .Include(t => t.Actividad)
                .Include(t => t.Profesor)
                .Include(t => t.Cancha)
                .Where(t => t.Id_Actividad == idActividad
                         && t.Id_Profesor == idProfesor
                         && t.Id_Cancha == idCancha
                         && t.HoraInicio == horaInicio
                         && t.Fecha >= inicio
                         && t.Fecha <= fin
                         && t.Fecha.DayOfWeek == diaSemana
                         && t.Estado == EstadoTurno.Disponible)
                .OrderBy(t => t.Fecha)
                .ToListAsync();
        }
    }
}