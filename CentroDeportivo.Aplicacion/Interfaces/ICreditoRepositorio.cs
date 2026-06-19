using CentroDeportivo.Aplicacion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Interfaces
{
    public interface ICreditoRepositorio
    {
        Task AgregarAsync(Credito credito);
        Task ActualizarAsync(Credito credito);
        Task EliminarAsync(int idCredito); //  lo dejo por las dudas. de todas maneras el borrado sera logico con el estado enum .
        Task<IEnumerable<Credito>> ObtenerTodosAsync();
        Task AgregarMuchosAsync(IEnumerable<Credito> creditos);
        Task<IEnumerable<Credito>> ObtenerTodosPorUsuarioAsync(int idUsuario); //para que el usuario pueda consultar sus creditos 
        Task<Credito?> ObtenerPorActividadAsync(int idActividad); // porr las dudas 
        Task<IEnumerable<Credito?>> ObtenerDisponiblesAsync(int idUsuario, int idActividad); //para mostrarle creditos disponibles al momento de reservar
    }
}
