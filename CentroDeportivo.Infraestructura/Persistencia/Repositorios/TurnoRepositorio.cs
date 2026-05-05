using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Infraestructura.Persistencia.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Infraestructura.Persistencia.Repositorios
{
    public class TurnoRepositorio(CentroDeportivoContext contexto) : ITurnoRepositorio
    {
        public async Task ActualizarAsync(Turno turno)
        {
            throw new NotImplementedException();
        }

        public async Task AgregarAsync(Turno turno)
        {
            throw new NotImplementedException();
        }

        public async Task EliminarAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Turno>> ObtenerDisponiblesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Turno>> ObtenerPorActividadAsync(int actividadId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Turno>> ObtenerPorCanchaAsync(int canchaId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Turno>> ObtenerPorFechaAsync(DateOnly fecha)
        {
            throw new NotImplementedException();
        }

        public async Task<Turno?> ObtenerPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Turno>> ObtenerPorProfesorAsync(int profesorId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Turno>> ObtenerTodosAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> TieneInscriptosAsync(int turnoId)
        {
            throw new NotImplementedException();
        }
    }
}
