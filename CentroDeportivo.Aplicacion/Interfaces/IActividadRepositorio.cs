using CentroDeportivo.Aplicacion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Interfaces
{
    public interface IActividadRepositorio
    {
        Task<Actividad?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Actividad>> ObtenerTodasAsync();
        Task AgregarAsync(Actividad actividad);
        Task ActualizarAsync(Actividad actividad);
        Task EliminarAsync(int id);
        Task<bool> TieneInscriptosAsync(int actividadId);
        Task<bool> YaExiste(string nombre);
    }
}
