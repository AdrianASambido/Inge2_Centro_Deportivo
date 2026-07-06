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
    public class PagoRepositorio(CentroDeportivoContext contexto) : IPagoRepositorio
    {
        public async Task AgregarAsync(Pago pago)
        {
            await contexto.Pagos.AddAsync(pago);
            await contexto.SaveChangesAsync();
        }

        public async Task<decimal> ObtenerIngresosGeneralesAsync(DateOnly desde, DateOnly hasta)
        {
            DateTime fechaDesde = desde.ToDateTime(TimeOnly.MinValue);
            DateTime fechaHasta = hasta.ToDateTime(TimeOnly.MaxValue);

  
            var montos = await contexto.Pagos
                .Where(p => p.Fecha >= fechaDesde && p.Fecha <= fechaHasta)
                .Select(p => p.Monto)
                .ToListAsync();

            
            return montos.Sum();
        }

        public async Task<decimal> ObtenerIngresosPorActividadAsync(int idActividad, DateOnly desde, DateOnly hasta)
        {
            // 1. Pagos de reservas ocasionales (tienen Id_Turno)
            var montosDirectos = await contexto.Pagos
                .Where(p => p.Id_Turno != null
                         && p.Turno!.Id_Actividad == idActividad
                         && p.Turno.Fecha >= desde
                         && p.Turno.Fecha <= hasta)
                .Select(p => p.Monto)
                .ToListAsync();

            decimal totalDirectos = montosDirectos.Sum();

            // 2. Pagos de reservas adelantadas (tienen CodigoPaqueteAdelantado)
            // Obtenemos los códigos de paquetes que corresponden a la actividad y rango
            var codigosPaquetes = await contexto.Reservas
                .Where(r => r.CodigoPaqueteAdelantado != null
                         && r.Turno!.Id_Actividad == idActividad
                         && r.Turno.Fecha >= desde
                         && r.Turno.Fecha <= hasta)
                .Select(r => r.CodigoPaqueteAdelantado)
                .Distinct()
                .ToListAsync();

            // Luego buscamos los pagos que tienen esos códigos
            decimal totalAdelantados = 0;
            if (codigosPaquetes.Any())
            {
                var montosAdelantados = await contexto.Pagos
                    .Where(p => p.CodigoPaqueteAdelantado != null
                             && codigosPaquetes.Contains(p.CodigoPaqueteAdelantado))
                    .Select(p => p.Monto)
                    .ToListAsync();

                totalAdelantados = montosAdelantados.Sum();
            }

            return totalDirectos + totalAdelantados;
        }

        public async Task<Pago?> ObtenerPorIdAsync(int idPago)
        {
            return await contexto.Pagos
                         .Include(p => p.Usuario)
                         .Include(p => p.Reserva)
                            .ThenInclude(r => r.Turno)
                                .ThenInclude(t => t.Actividad)
                         .Include(p => p.Turno)
                            .ThenInclude(t => t.Actividad)
                         .Where(p => p.Id == idPago)
                         .FirstOrDefaultAsync();
        }

        public async Task<Pago?> ObtenerPorPaqueteAsync(Guid codigo)
        {
            return await contexto.Pagos
                         .Include(p => p.Usuario)
                         .Include(p => p.Reserva)
                            .ThenInclude(r => r.Turno)
                                .ThenInclude(t => t.Actividad)
                         .Include(p => p.Turno)
                            .ThenInclude(t => t.Actividad)
                         .Where(p => p.CodigoPaqueteAdelantado == codigo)
                         .FirstOrDefaultAsync();
        }

        public async Task<Pago?> ObtenerPorReservaAsync(int idReserva)
        {
            return await contexto.Pagos
                         .Include(p => p.Usuario)
                         .Include(p => p.Reserva)
                            .ThenInclude(r => r.Turno)
                                .ThenInclude(t => t.Actividad)
                         .Where(p => p.Id_Reserva == idReserva)
                         .FirstOrDefaultAsync();
        }

        public async Task<Pago?> ObtenerPorUsuarioAsync(int idUsuario)
        {
            return await contexto.Pagos
                         .Include(p => p.Usuario)
                         .Include(p => p.Reserva)
                            .ThenInclude(r => r.Turno)
                                .ThenInclude(t => t.Actividad)
                         .Include(p => p.Turno)
                            .ThenInclude(t => t.Actividad)
                         .Where(p => p.Id_Usuario == idUsuario)
                         .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Pago>> ObtenerTodosAsync()
        {
            return await contexto.Pagos
                         .Include(p => p.Usuario)
                         .Include(p => p.Reserva)
                            .ThenInclude(r => r.Turno)
                                .ThenInclude(t => t.Actividad)
                         .Include(p => p.Turno)
                            .ThenInclude(t => t.Actividad)
                         .AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Pago>> ObtenerTodosPorReservaAsync(int idReserva)
        {
            return await contexto.Pagos
        .Where(p => p.Id_Reserva == idReserva)
        .ToListAsync();
        }

        public async Task<IEnumerable<Pago>> ObtenerTodosPorTurnoAsync(int idTurno)
        {
            return await contexto.Pagos
                        .Include(p => p.Usuario)
                        .Include(p => p.Turno)
                          .ThenInclude(t => t.Actividad)
                        .Where(p => p.Id_Turno == idTurno)
                        .AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Pago>> ObtenerTodosPorUsuarioAsync(int idUsuario)
        {
            return await contexto.Pagos
                         .Include(p => p.Usuario)
                         .Include(p => p.Reserva)
                            .ThenInclude(r => r.Turno)
                                .ThenInclude(t => t.Actividad)
                         .Include(p => p.Turno)
                            .ThenInclude(t => t.Actividad)
                         .Where(p => p.Id_Usuario == idUsuario)
                         .AsNoTracking().ToListAsync();
        }
    }
}