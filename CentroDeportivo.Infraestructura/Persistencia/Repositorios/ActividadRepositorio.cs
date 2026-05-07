using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Infraestructura.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;

namespace CentroDeportivo.Infraestructura.Persistencia.Repositorios
{
    public class ActividadRepositorio(CentroDeportivoContext context) : IActividadRepositorio
    {
        public async Task ActualizarAsync(Actividad actividad)
        {
            context.Actividades.Update(actividad);
            await context.SaveChangesAsync();
        }

        public async Task AgregarAsync(Actividad actividad)
        {
           await context.Actividades.AddAsync(actividad);
           await context.SaveChangesAsync();

        }

        public async Task EliminarAsync(int id)
        {
            var actividad = await ObtenerPorIdAsync(id);
            if (actividad != null) {
                context.Actividades.Remove(actividad);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Actividad?> ObtenerPorIdAsync(int id)
        {
            return await context.Actividades.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Actividad>> ObtenerTodasAsync()
        {
            return await context.Actividades.AsNoTracking().ToListAsync();
        }

        public async Task<bool> TieneInscriptosAsync(int actividadId)
        {
            return await context.Turnos.AnyAsync(t => t.Id_Actividad == actividadId && (t.Estado == EstadoTurno.Disponible || t.Estado == EstadoTurno.Lleno));
        }

        public async Task<bool> YaExiste(string nombre)
        {
            return await context.Actividades.AnyAsync(a => a.Nombre == nombre);
        }
        public async Task<bool> YaExisteParaEditar(string nombre, int idActual)
        {
            // Devuelve true solo si hay otra actividad con ese nombre, pero que NO sea la que estamos editando
            return await context.Actividades.AnyAsync(a => a.Nombre == nombre && a.Id != idActual);
        }
    }
}
