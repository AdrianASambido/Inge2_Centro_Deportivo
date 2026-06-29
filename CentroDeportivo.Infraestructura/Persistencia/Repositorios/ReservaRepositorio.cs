using CentroDeportivo.Aplicacion.Casos_de_uso.EstadisticaUseCase.DTOs;
using CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase;
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

        public async Task ActualizarMuchasAsync(IEnumerable<Reserva> reservas)
        {
            contexto.Reservas.UpdateRange(reservas);
            await contexto.SaveChangesAsync();
        }

        public async Task AgregarAsync(Reserva reserva)
        {
            await contexto.Reservas.AddAsync(reserva);
            await contexto.SaveChangesAsync();
        }

        public async Task<int> ContarCancelacionesUsuarioMesAsync(int idUsuario, int anio, int mes)
        {
            return await contexto.Reservas
                .Where(r => r.Id_Usuario == idUsuario
                         && r.Estado == EstadoReserva.Cancelado
                         && r.FechaReserva.Year == anio
                         && r.FechaReserva.Month == mes)
                .CountAsync();
        }

        public async Task<bool> ExisteReservaActivaAsync(int usuarioId, int turnoId)
        {
            throw new NotImplementedException();
        }

        public async Task GuardarMuchasReservasAsync(List<Reserva> reservas)
        {
            if (reservas == null || !reservas.Any()) return;

            await contexto.Reservas.AddRangeAsync(reservas);

            await contexto.SaveChangesAsync();
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
                        .Include(r => r.Turno) 
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
            var query = contexto.Reservas
                .Include(r => r.Turno)
                    .ThenInclude(t => t!.Actividad)
                .Include(r => r.Turno)
                    .ThenInclude(t => t!.Profesor)
                .Include(r => r.Turno)
                    .ThenInclude(t => t!.Cancha)
                .Where(r => r.Id_Usuario == usuarioId)
                .AsQueryable();

            
            if (!incluirPasadas)
            {
                query = query.Where(r =>
                    r.Turno!.Estado != EstadoTurno.Finalizado &&
                    r.Turno.Estado != EstadoTurno.Cancelado &&
                    r.Estado != EstadoReserva.Cancelado
                );
            }

            
            if (estado.HasValue)
            {
                query = query.Where(r => r.Estado == estado.Value);
            }

           
            if (actividadId.HasValue)
            {
                query = query.Where(r => r.Turno!.Id_Actividad == actividadId.Value);
            }

            return await query
                .OrderBy(r => r.Turno!.Fecha)
                .ThenBy(r => r.Turno!.HoraInicio)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ReporteIndicesActividadDTO> ObtenerTotalesPorActividadAsync(int idActividad, DateOnly desde, DateOnly hasta)
        {
            var filtradas = await contexto.Reservas
                .Include(r => r.Turno)
                .Where(r => r.Turno != null
                         && r.Turno.Id_Actividad == idActividad
                         && r.Turno.Estado != EstadoTurno.Cancelado
                         && r.Turno.Estado == EstadoTurno.Finalizado
                         && r.Turno.Fecha >= desde
                         && r.Turno.Fecha <= hasta)
                .Select(r => new { r.Estado, r.Asistencia })
                .ToListAsync();

            int total = filtradas.Count;

            if (total == 0)
            {
                return new ReporteIndicesActividadDTO(0, 0, 0, 0);
            }

            int asistencias = filtradas.Count(r => r.Estado != EstadoReserva.Cancelado && r.Asistencia == Asistencia.Presente);
            int inasistencias = filtradas.Count(r => r.Estado != EstadoReserva.Cancelado && r.Asistencia == Asistencia.Ausente);
            int canceladas = filtradas.Count(r => r.Estado == EstadoReserva.Cancelado);

            return new ReporteIndicesActividadDTO(total, asistencias, inasistencias, canceladas);
        }

        public async Task<IEnumerable<HistorialUsuarioActividadDTO>> ObtenerTotalesPorUsuarioAsync(int idUsuario, DateOnly? desde=null, DateOnly? hasta=null)
        {
            var reservas = await contexto.Reservas
                           .Include(r => r.Turno)
                           .ThenInclude(t => t.Actividad)
                           .Where(r => r.Id_Usuario == idUsuario
                                 && r.Turno != null
                                 && r.Turno.Estado != EstadoTurno.Cancelado
                                 && r.Turno.Estado == EstadoTurno.Finalizado
                                 && (desde == null || r.Turno.Fecha >= desde.Value)
                                 && (hasta == null || r.Turno.Fecha <= hasta.Value))
                           .ToListAsync();

            var historial = reservas.GroupBy(r => r.Turno.Id_Actividad)
                            .Select(grupo =>
                            {
                                var primeraReserva = grupo.First();
                                string nombreActividad = primeraReserva.Turno.Actividad.Nombre;
                                int asistencias = grupo.Count(r => r.Estado != EstadoReserva.Cancelado && r.Asistencia == Asistencia.Presente);
                                int inasistencias = grupo.Count(r => r.Estado != EstadoReserva.Cancelado && r.Asistencia == Asistencia.Ausente);
                                int cancelaciones = grupo.Count(r => r.Estado == EstadoReserva.Cancelado);
                                return new HistorialUsuarioActividadDTO(nombreActividad, asistencias, inasistencias, cancelaciones);
                            })
                            .ToList();
            return historial;
        }

        public async Task<bool> TieneConflictoHorarioAsync(int usuarioId, DateOnly fecha, TimeOnly horarioInicio)
        {
            return await contexto.Reservas
                       .AnyAsync(r => r.Id_Usuario == usuarioId &&
                       r.Turno!.Fecha == fecha &&
                       r.Turno.HoraInicio == horarioInicio &&
                       (r.Estado == EstadoReserva.Confirmado || r.Estado == EstadoReserva.Reservado));
        }

        public async Task<bool> TieneExcesoCancelacionesAsync(int idUsuario, DateOnly fechaLimite)
        {
            int cantidad = await contexto.Reservas
                            .Where(r => r.Id_Usuario == idUsuario && r.Estado == EstadoReserva.Cancelado && r.FechaReserva >= fechaLimite).CountAsync();

            return cantidad >= 3;
        }
    }
    }
