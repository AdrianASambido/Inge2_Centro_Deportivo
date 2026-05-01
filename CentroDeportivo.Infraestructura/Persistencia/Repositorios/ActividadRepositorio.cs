using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;

namespace CentroDeportivo.Infraestructura.Persistencia.Repositorios
{
    public class ActividadRepositorio : IActividadRepositorio
    {
        public Task ActualizarAsync(Actividad actividad)
        {
            throw new NotImplementedException();
        }

        public Task AgregarAsync(Actividad actividad)
        {
            throw new NotImplementedException();
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
    }
}
