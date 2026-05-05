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
    public class ProfesorRepositorio(CentroDeportivoContext contexto) : IProfesorRepositorio
    {
        public Task ActualizarAsync(Profesor profesor)
        {
            throw new NotImplementedException();
        }

        public async Task AgregarAsync(Profesor profesor)
        {
            await contexto.Profesores.AddAsync(profesor);
            await contexto.SaveChangesAsync();
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

        public async Task<bool> YaExiste(string dni)
        {
            return await contexto.Profesores.AnyAsync(p => p.Dni == dni);
        }
    }
}
