using CentroDeportivo.Aplicacion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Interfaces
{
    public interface IProfesorRepositorio
    {
        Task<Profesor?> ObtenerPorIdAsync(int id);
        Task<Profesor?> ObtenerPorDniAsync(string dni);
        Task<IEnumerable<Profesor>> ObtenerTodosAsync();
        Task<IEnumerable<Profesor>> ObtenerDisponiblesAsync(DateOnly fecha, TimeOnly horarioInicio);
        Task AgregarAsync(Profesor profesor);
        Task ActualizarAsync(Profesor profesor);
        Task EliminarAsync(int id);
        Task<bool> YaExiste(int dni);
    }
}
