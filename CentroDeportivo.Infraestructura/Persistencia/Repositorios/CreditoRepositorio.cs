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
    public class CreditoRepositorio (CentroDeportivoContext contexto): ICreditoRepositorio
    {
        public async Task ActualizarAsync(Credito credito)
        {
            contexto.Creditos.Update(credito);
            await contexto.SaveChangesAsync();
        }

        public async Task AgregarAsync(Credito credito)
        {
            await contexto.Creditos.AddAsync(credito);
            await contexto.SaveChangesAsync();
        }

        public async Task EliminarAsync(int idCredito)
        {
            throw new NotImplementedException();
        }

        
        public async Task<IEnumerable<Credito?>> ObtenerDisponiblesAsync(int idUsuario, int idActividad)
        {
            return await contexto.Creditos.Include(c => c.Usuario)
                                   .Include(c => c.Actividad)
                                    .Where(c => c.Id_Actividad == idActividad && c.Id_Usuario == idUsuario && c.Estado == EstadoCredito.Disponible)
                                    .AsNoTracking().ToListAsync();
        }

        public Task<Credito?> ObtenerPorActividadAsync(int idActividad)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Credito>> ObtenerTodosAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Credito>> ObtenerTodosPorUsuarioAsync(int idUsuario)
        {
            return await contexto.Creditos
                         .Include(c => c.Usuario)
                         .Include(c => c.Actividad)
                         .Where(c => c.Id_Usuario == idUsuario)
                         .AsNoTracking().ToListAsync();
        }
    }
}
