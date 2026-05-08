using CentroDeportivo.Aplicacion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Interfaces
{
    public interface ICanchaRepositorio
    {
        Task<Cancha?> ObtenerPorIdAsync(int id);
        Task<Cancha?> ObtenerPorNumeroAsync(int numeroCancha);
        Task<IEnumerable<Cancha>> ObtenerTodasAsync();
        Task<IEnumerable<Cancha>> ObtenerDisponiblesAsync(DateOnly fecha, TimeOnly horarioInicio);
        Task AgregarAsync(Cancha cancha);
        Task ActualizarAsync(Cancha cancha);
        Task EliminarAsync(int id);
        Task<bool> YaExiste(int numero);
        Task<bool> YaExisteNumeroParaEditar(int numeroCancha, int idCancha);
        Task<bool> TieneTurnosAsignadosAsync(int idCancha);
        Task<bool> EstaDisponibleAsync(int idCancha, DateOnly fecha, TimeOnly horarioInicio);
    }
}
