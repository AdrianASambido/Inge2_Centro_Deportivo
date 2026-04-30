using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Infraestructura.Persistencia.Repositorios
{
    public class ProfesorRepositorio : IProfesorRepositorio
    {
        public Task ActualizarAsync(Profesor profesor)
        {
            throw new NotImplementedException();
        }

        public Task AgregarAsync(Profesor profesor)
        {
            throw new NotImplementedException();
        }

        public Task EliminarAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Profesor>> ObtenerDisponiblesAsync(DateOnly fecha, TimeOnly horarioInicio)
        {
            throw new NotImplementedException();
        }

        public Task<Profesor?> ObtenerPorDniAsync(string dni)
        {
            throw new NotImplementedException();
        }

        public Task<Profesor?> ObtenerPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Profesor>> ObtenerTodosAsync()
        {
            throw new NotImplementedException();
        }
    }
}
