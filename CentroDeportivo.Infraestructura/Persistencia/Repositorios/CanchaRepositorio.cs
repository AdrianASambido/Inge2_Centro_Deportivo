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
    public class CanchaRepositorio(CentroDeportivoContext contexto) : ICanchaRepositorio
    {

        public async Task ActualizarAsync(Cancha cancha)
        {
            contexto.Canchas.Update(cancha);
            await contexto.SaveChangesAsync();
        }

        public async Task AgregarAsync(Cancha cancha)
        {
            await contexto.Canchas.AddAsync(cancha);
            await contexto.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var cancha = await this.ObtenerPorIdAsync(id);
            if (cancha != null)
            {
                contexto.Canchas.Remove(cancha);
                await contexto.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Cancha>> ObtenerDisponiblesAsync(DateOnly fecha, TimeOnly horarioInicio)
        {
            // 1. Calculamos los límites afuera de la consulta LINQ
            // Un turno de una hora que empiece a las 18:00 termina a las 19:00.
            // Buscamos cualquier turno que empiece después de las 17:00 y antes de las 19:00.
            var limiteInferior = horarioInicio.AddHours(-1);
            var limiteSuperior = horarioInicio.AddHours(1);

            var canchasOcupadas = await contexto.Turnos
                .Where(t => t.Fecha == fecha &&
                           (t.Estado == EstadoTurno.Disponible || t.Estado == EstadoTurno.Lleno))
                .Where(t =>
                    // Ahora comparamos solo contra variables, esto SÍ se traduce a SQL
                    t.HoraInicio > limiteInferior &&
                    t.HoraInicio < limiteSuperior
                )
                .Select(t => t.Id_Cancha)
                .Distinct()
                .ToListAsync();

            return await contexto.Canchas
                .Where(c => !canchasOcupadas.Contains(c.Id) && c.Existe)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Cancha?> ObtenerPorIdAsync(int id)
        {
            return await contexto.Canchas.FirstOrDefaultAsync(c => c.Id == id && c.Existe);
        }

        public async Task<Cancha?> ObtenerPorNumeroAsync(int numeroCancha)
        {
            return await contexto.Canchas.FirstOrDefaultAsync(c => c.Numero == numeroCancha && c.Existe);
        }

        public async Task<IEnumerable<Cancha>> ObtenerTodasAsync()
        {
            return await contexto.Canchas.Where(c => c.Existe).AsNoTracking().ToListAsync();
        }

        public async Task<bool> YaExiste(int numero)
        {
            return await contexto.Canchas.AnyAsync(c => c.Numero == numero && c.Existe);
        }

        public async Task<bool> YaExisteNumeroParaEditar(int numero, int idActual)
        {
            return await contexto.Canchas.AnyAsync(c => c.Numero == numero && c.Id != idActual && c.Existe);
        }

        //si tiene turnos asignados no se puede eliminar ni editar la cancha
        public async Task<bool> TieneTurnosAsignadosAsync(int canchaId)
        {
            return await contexto.Turnos.AnyAsync(t =>
                t.Id_Cancha == canchaId &&
                (t.Estado == EstadoTurno.Disponible || t.Estado == EstadoTurno.Lleno)
            );
        }

        public async Task<bool> EstaDisponibleAsync(int idCancha, DateOnly fecha, TimeOnly horarioInicio)
        {
            bool ocupado = await contexto.Turnos
                    .AnyAsync(t => t.Id_Cancha == idCancha
                    && t.Fecha == fecha
                    && t.HoraInicio == horarioInicio
                    && (t.Estado == EstadoTurno.Disponible || t.Estado == EstadoTurno.Lleno));

            return !ocupado;
        }
    }
}