using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Infraestructura.Persistencia.Contexto;

namespace CentroDeportivo.Infraestructura.Persistencia.Repositorios
{
    public class ActividadRepositorio(CentroDeportivoContext context) : IActividadRepositorio
    {
        public Task ActualizarAsync(Actividad actividad)
        {
            throw new NotImplementedException();
        }

        public async Task AgregarAsync(Actividad actividad)
        {
           await context.Actividades.AddAsync(actividad);
           await context.SaveChangesAsync();

        }

        public Task EliminarAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Actividad?> ObtenerPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Actividad>> ObtenerTodasAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> TieneInscriptosAsync(int actividadId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> YaExiste(string nombre)
        {
            throw new NotImplementedException();
        }
    }
}
