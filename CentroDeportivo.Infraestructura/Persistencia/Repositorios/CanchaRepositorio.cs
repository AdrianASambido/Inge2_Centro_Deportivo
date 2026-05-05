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
    public class CanchaRepositorio(CentroDeportivoContext contexto) : ICanchaRepositorio
    {

        public async Task ActualizarAsync(Cancha cancha)
        {
            throw new NotImplementedException();
        }

        public async Task AgregarAsync(Cancha cancha)
        {
            await contexto.Canchas.AddAsync(cancha);
            await contexto.SaveChangesAsync();
        }

        public async Task<int> capacidad(int idCancha)
        {
            throw new NotImplementedException();
        }

        public async Task EliminarAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Cancha>> ObtenerDisponiblesAsync(DateOnly fecha, TimeOnly horarioInicio)
        {
            throw new NotImplementedException();
        }

        public async Task<Cancha?> ObtenerPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Cancha?> ObtenerPorNumeroAsync(int numeroCancha)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Cancha>> ObtenerTodasAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> YaExiste(int numero)
        {
            return await contexto.Canchas.AnyAsync(c => c.Numero == numero);
        }
    }
}

