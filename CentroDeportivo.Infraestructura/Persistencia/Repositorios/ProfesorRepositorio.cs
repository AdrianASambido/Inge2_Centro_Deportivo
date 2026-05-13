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
    public class ProfesorRepositorio(CentroDeportivoContext contexto) : IProfesorRepositorio
    {
        public async Task ActualizarAsync(Profesor profesor)
        {
            contexto.Profesores.Update(profesor);
            await contexto.SaveChangesAsync();
        }

        public async Task AgregarAsync(Profesor profesor)
        {
            await contexto.Profesores.AddAsync(profesor);
            await contexto.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var profesor = await this.ObtenerPorIdAsync(id);
            if(profesor != null)
            {
                contexto.Profesores.Remove(profesor);
                await contexto.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Profesor>> ObtenerDisponiblesAsync(DateOnly fecha, TimeOnly horarioInicio)
        {
            // 1. Calculamos los límites afuera para que EF los entienda como constantes
            // Buscamos cualquier turno que empiece entre una hora antes y una hora después
            var limiteInferior = horarioInicio.AddHours(-1);
            var limiteSuperior = horarioInicio.AddHours(1);

            var profesOcupados = await contexto.Turnos
                .Where(t => t.Fecha == fecha &&
                           (t.Estado == EstadoTurno.Disponible || t.Estado == EstadoTurno.Lleno))
                .Where(t =>
                    // Validamos el solapamiento de 1 hora
                    t.HoraInicio > limiteInferior &&
                    t.HoraInicio < limiteSuperior
                )
                .Select(t => t.Id_Profesor)
                .Distinct()
                .ToListAsync();

            return await contexto.Profesores
                .Where(p => !profesOcupados.Contains(p.Id))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Profesor?> ObtenerPorDniAsync(string dni)
        {
            return await contexto.Profesores.FirstOrDefaultAsync(p => p.Dni == dni);
        }

        public async Task<Profesor?> ObtenerPorIdAsync(int id)
        {
            return await contexto.Profesores.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Profesor>> ObtenerTodosAsync()
        {
            return await contexto.Profesores.AsNoTracking().ToListAsync();

        }

        //al momento de dar de alta un profesor, ver que ya no este registrado
        public async Task<bool> YaExiste(string dni)
        {
            return await contexto.Profesores.AnyAsync(p => p.Dni == dni);
        }

        public async Task<bool> YaExisteDniParaEditar(string dni, int idActual)
        {
            return await contexto.Profesores.AnyAsync(p => p.Dni == dni && p.Id != idActual);
        }

        //si tiene turnos asignados no se puede eliminar el profesor
        public async Task<bool> TieneTurnosAsignadosAsync(int profesorId)
        {
            return await contexto.Turnos.AnyAsync(t =>
                t.Id_Profesor == profesorId &&
                (t.Estado == EstadoTurno.Disponible || t.Estado == EstadoTurno.Lleno)
            );
        }

        public async Task<bool> EstaDisponibleAsync(int idProfesor, DateOnly fecha, TimeOnly horarioInicio)
        {
           bool ocupado = await contexto.Turnos
                    .AnyAsync(t => t.Id_Profesor == idProfesor
                    && t.Fecha == fecha
                    && t.HoraInicio == horarioInicio
                    && (t.Estado == EstadoTurno.Disponible || t.Estado == EstadoTurno.Lleno));

            return !ocupado;
        }
    }
}
