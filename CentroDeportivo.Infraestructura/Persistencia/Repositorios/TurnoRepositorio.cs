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
        public async Task<IEnumerable<Turno>> BuscarTurnosAsync(DateOnly? fecha, int? actividadId, int? profeId, int? canchaId,EstadoTurno? estado)
        {
            var query = contexto.Turnos
                .Include(t => t.Actividad)
                .Include(t => t.Profesor)
                .Include(t => t.Cancha)
                .AsQueryable(); // encadena filtros

            // Filtros dinámicos, solo se aplican si el empleado los elige
            if (fecha.HasValue)
                query = query.Where(t => t.Fecha == fecha.Value);

            if (actividadId.HasValue)
                query = query.Where(t => t.Id_Actividad == actividadId.Value);

            if (profeId.HasValue)
                query = query.Where(t => t.Id_Profesor == profeId.Value);

            if (canchaId.HasValue) { 
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
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<bool> TieneInscriptosAsync(int turnoId)
        {
            return await contexto.Turnos
                .AnyAsync(t => t.Id == turnoId && t.Reservas.Any());
        }

        public async Task<IEnumerable<Turno>> ObtenerParaCalendarioAsync(int usuarioId, int? actividadId = null)
        {
            var hoy = DateOnly.FromDateTime(DateTime.Now);

            //  Fecha y Hora de sus reservas activas
            var horariosOcupados = await contexto.Reservas
                .Where(r => r.Id_Usuario == usuarioId &&
                           (r.Estado == EstadoReserva.Confirmado || r.Estado == EstadoReserva.PendienteDePago))
                .Select(r => new {r.Turno.Fecha, r.Turno.HoraInicio })
                .ToListAsync();

            //  Query principal
            var query = contexto.Turnos
                        .Include(t => t.Actividad)
                        .Include(t => t.Profesor)
                        .Include(t => t.Cancha)
                        .Where(t => t.Estado == EstadoTurno.Disponible && t.Fecha >= hoy)
                        .AsQueryable();

            //  Filtra los turnos, solo mostramos si NO hay coincidencia de Fecha y HoraInicio
            var todosLosDisponibles = await query.ToListAsync();

            var filtrados = todosLosDisponibles.Where(t =>
                !horariosOcupados.Any(h => h.Fecha == t.Fecha && h.HoraInicio == t.HoraInicio)
            );

            if (actividadId.HasValue)
                filtrados = filtrados.Where(t => t.Id_Actividad == actividadId.Value);

            return filtrados.OrderBy(t => t.Fecha).ThenBy(t => t.HoraInicio);
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

        
    }
}
