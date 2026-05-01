using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Infraestructura.Persistencia.Repositorios
{
    public class TurnoRepositorio : ITurnoRepositorio
    {
        public Task ActualizarAsync(Turno turno)
        {
            throw new NotImplementedException();
        }

        public Task AgregarAsync(Turno turno)
        {
            throw new NotImplementedException();
        }

        public Task EliminarAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Turno>> ObtenerDisponiblesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Turno>> ObtenerPorActividadAsync(int actividadId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Turno>> ObtenerPorCanchaAsync(int canchaId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Turno>> ObtenerPorFechaAsync(DateOnly fecha)
        {
            throw new NotImplementedException();
        }

        public Task<Turno?> ObtenerPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Turno>> ObtenerPorProfesorAsync(int profesorId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Turno>> ObtenerTodosAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> TieneInscriptosAsync(int turnoId)
        {
            throw new NotImplementedException();
        }
    }
}
