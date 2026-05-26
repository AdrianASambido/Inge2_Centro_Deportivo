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
            if (actividad != null)
            {
                context.Actividades.Remove(actividad);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Actividad?> ObtenerPorIdAsync(int id)
        {
            return await context.Actividades.FirstOrDefaultAsync(x => x.Id == id && x.Existe);
        }

        public async Task<IEnumerable<Actividad>> ObtenerTodasAsync()
        {
            return await context.Actividades.Where(a => a.Existe).AsNoTracking().ToListAsync();
        }

        public async Task<bool> TieneInscriptosAsync(int actividadId)
        {
            return await context.Turnos.AnyAsync(t => t.Id_Actividad == actividadId && (t.Estado == EstadoTurno.Disponible || t.Estado == EstadoTurno.Lleno));
        }

        private string Normalizar(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return "";
            var temp = texto.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var c in temp)
            {
                if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }
            return sb.ToString().Normalize(NormalizationForm.FormC).ToUpper();
        }

        public async Task<bool> YaExiste(string nombreNuevo)
        {
            var nombreNuevoLimpio = Normalizar(nombreNuevo);

            // Traemos solo los nombres de actividades activas
            var nombresExistentes = await context.Actividades
                .Where(a => a.Existe)
                .Select(a => a.Nombre)
                .ToListAsync();

            return nombresExistentes.Any(n => Normalizar(n) == nombreNuevoLimpio);
        }

        public async Task<bool> YaExisteParaEditar(string nombre, int idActual)
        {
            var nombreNormalizado = Normalizar(nombre);

            var nombresExistentes = await context.Actividades
                .Where(a => a.Existe && a.Id != idActual)
                .Select(a => a.Nombre)
                .ToListAsync();

            return nombresExistentes.Any(n => Normalizar(n) == nombreNormalizado);
        }
    }
}