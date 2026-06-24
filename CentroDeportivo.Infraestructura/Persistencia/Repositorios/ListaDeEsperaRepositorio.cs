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
    public class ListaDeEsperaRepositorio(CentroDeportivoContext contexto) : IListaDeEsperaRepositorio
    {
        public async Task ActualizarAsync(InscripcionListaEspera lista)
        {
            contexto.InscripcionListaEsperas.Update(lista);
            await contexto.SaveChangesAsync();
        }

        public async Task AgregarAsync(InscripcionListaEspera lista)
        {
            await contexto.InscripcionListaEsperas.AddAsync(lista);
            await contexto.SaveChangesAsync();
        }

        public async Task EliminarAsync(int idLista)
        {
            var lista = await this.ObtenerPorIdAsync(idLista);
            if (lista != null)
            {
                contexto.InscripcionListaEsperas.Remove(lista);
                await contexto.SaveChangesAsync();
            }
        }

        public async Task<InscripcionListaEspera?> ObtenerPorIdAsync(int id)
        {
            return await contexto.InscripcionListaEsperas
                .Include(x => x.Usuario)
                .Include(x => x.Turno)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<InscripcionListaEspera?> ObtenerPorUsuarioAsync(int idUsuario)
        {
            return await contexto.InscripcionListaEsperas
                                 .Include(x => x.Usuario)
                                 .Include(x => x.Turno)
                                 .FirstOrDefaultAsync(x => x.Id_Usuario == idUsuario);
        }

        public async Task<InscripcionListaEspera?> ObtenerPrimeroEnFilaAsync(int idTurno)
        {
            return await contexto.InscripcionListaEsperas
                .Include(x => x.Usuario)
                .Include(x => x.Turno)
                .Where(x => x.Id_Turno == idTurno && x.Estado == EstadoListaEspera.Esperando)
                .OrderBy(x => x.FechaInscripcion)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<InscripcionListaEspera>> ObtenerTodosAsync()
        {
            return await contexto.InscripcionListaEsperas
                                 .Include(x => x.Usuario)
                                 .Include(x => x.Turno)
                                 .AsNoTracking().ToListAsync();
        }

        //para mostrarle al empleado los inscriptos en una lista de espera de un turno
        public async Task<IEnumerable<InscripcionListaEspera>> ObtenerTodosPorTurnoAsync(int idTurno)
        {
            return await contexto.InscripcionListaEsperas
                            .Include(x => x.Usuario)
                            .Include(x => x.Turno)
                            .Where(x => x.Id_Turno == idTurno && (x.Estado == EstadoListaEspera.Esperando || x.Estado == EstadoListaEspera.Notificado))
                            .AsNoTracking().ToListAsync();
        }

        //para mostrarle al cliente sus inscripciones en lista de espera activas
        public async Task<IEnumerable<InscripcionListaEspera>> ObtenerTodosPorUsuarioAsync(int idUsuario)
        {
            return await contexto.InscripcionListaEsperas
                                 .Include(x => x.Usuario)
                                 .Include(x => x.Turno)
                                 .Where(x => x.Id_Usuario == idUsuario && (x.Estado == EstadoListaEspera.Esperando || x.Estado == EstadoListaEspera.Notificado))
                                 .AsNoTracking().ToListAsync();
        }

        public async Task<InscripcionListaEspera?> ObtenerPorUsuarioYTurno(int idUsuario, int idTurno)
        {
            return await contexto.InscripcionListaEsperas
                .FirstOrDefaultAsync(i => i.Id_Usuario == idUsuario
                                       && i.Id_Turno == idTurno);
        }
    }
}