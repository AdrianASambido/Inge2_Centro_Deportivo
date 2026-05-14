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
    public class ReservaRepositorio(CentroDeportivoContext contexto) : IReservaRepositorio
    {
        public async Task ActualizarAsync(Reserva reserva)
        {
            contexto.Reservas.Update(reserva);
            await contexto.SaveChangesAsync();
        }

        public async Task AgregarAsync(Reserva reserva)
        {
            await contexto.Reservas.AddAsync(reserva);
            await contexto.SaveChangesAsync();
        }

        public async Task<bool> ExisteReservaActivaAsync(int usuarioId, int turnoId)
        {
            throw new NotImplementedException();
        }

        public async Task<Reserva?> ObtenerPorIdAsync(int id)
        {
            return await contexto.Reservas
                .Include(r => r.Turno)   // Traigo los datos del horario, fecha
                .Include(r => r.Usuario) // Taigo los datos del cliente 
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Reserva?> ObtenerPorQrTokenAsync(string qrToken)
        {
            return await contexto.Reservas
                         .Include(r => r.Turno)   
                         .Include(r => r.Usuario) 
                         .FirstOrDefaultAsync(r => r.TokenQr == qrToken);
        }

        public async Task<IEnumerable<Reserva>> ObtenerPorTurnoAsync(int turnoId, string? dni = null)
        {
            var query = contexto.Reservas
                        .Include(r => r.Usuario)
                        .Where(r => r.Id_Turno == turnoId && r.Estado != EstadoReserva.Cancelado)
                        .AsQueryable();


            if (!string.IsNullOrWhiteSpace(dni))
                query = query.Where(r => r.Usuario!.Dni == dni);

            return await query
                        .OrderBy(r => r.Usuario!.Nombre)
                        .AsNoTracking()
                        .ToListAsync();
        }

        public async Task<IEnumerable<Reserva>> ObtenerPorUsuarioAsync(int usuarioId, int? actividadId = null, EstadoReserva? estado = null, bool incluirPasadas = false)
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);

            var query = contexto.Reservas
                .Include(r => r.Turno)
                    .ThenInclude(t => t!.Actividad)
                .Include(r => r.Turno)
                    .ThenInclude(t => t!.Profesor)
                .Include (r => r.Turno)
                    .ThenInclude(t => t!.Cancha) ///AGREGA ESTO
                .Where(r => r.Id_Usuario == usuarioId)
                .AsQueryable();

            // Por defecto solo muestra reservas activas 
            if (!incluirPasadas)
                query = query.Where(r => r.Turno!.Fecha >= hoy);

            // Filtro por estado (pendienteDePago, confirmado, cancelado) .
            if (estado.HasValue)
                query = query.Where(r => r.Estado == estado.Value);

            // Filtro por actividadr
            if (actividadId.HasValue)
                query = query.Where(r => r.Turno!.Id_Actividad == actividadId.Value);

            return await query
                .OrderBy(r => r.Turno!.Fecha)
                .ThenBy(r => r.Turno!.HoraInicio)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> TieneConflictoHorarioAsync(int usuarioId, DateOnly fecha, TimeOnly horarioInicio)
        {
            return await contexto.Reservas
                       .AnyAsync(r => r.Id_Usuario == usuarioId &&
                       r.Turno.Fecha == fecha &&
                       r.Turno.HoraInicio == horarioInicio &&
                       (r.Estado == EstadoReserva.Confirmado || r.Estado == EstadoReserva.PendienteDePago));
        }
    }
}