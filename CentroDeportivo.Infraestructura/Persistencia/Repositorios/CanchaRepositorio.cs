using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Infraestructura.Persistencia.Repositorios
{
    public class CanchaRepositorio : ICanchaRepositorio
    {

        public Task ActualizarAsync(Cancha cancha)
        {
            throw new NotImplementedException();
        }

        public Task AgregarAsync(Cancha cancha)
        {
            throw new NotImplementedException();
        }

        public Task EliminarAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Cancha>> ObtenerDisponiblesAsync(DateOnly fecha, TimeOnly horarioInicio)
        {
            throw new NotImplementedException();
        }

        public Task<Cancha?> ObtenerPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Cancha?> ObtenerPorNumeroAsync(int numeroCancha)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Cancha>> ObtenerTodasAsync()
        {
            throw new NotImplementedException();
        }
    }
}

