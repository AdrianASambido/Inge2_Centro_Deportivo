using CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase;
using CentroDeportivo.Aplicacion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Interfaces
{
    public interface IListaDeEsperaRepositorio
    {
        Task AgregarAsync(InscripcionListaEspera lista);
        Task EliminarAsync(int idLista);
        Task ActualizarAsync(InscripcionListaEspera lista);
        Task<IEnumerable<InscripcionListaEspera>> ObtenerTodosAsync();
        Task<InscripcionListaEspera?> ObtenerPorIdAsync(int id);
        Task<InscripcionListaEspera?> ObtenerPorUsuarioAsync(int idUsuario);
        Task<IEnumerable<InscripcionListaEspera>> ObtenerTodosPorUsuarioAsync(int idUsuario);
        Task<IEnumerable<InscripcionListaEspera>> ObtenerTodosPorTurnoAsync(int idTurno);
        Task<InscripcionListaEspera?> ObtenerPrimeroEnFilaAsync(int idTurno);
        Task<InscripcionListaEspera?> ObtenerPorUsuarioYTurno(int idUsuario, int idTurno);
        Task<int> ObtenerPosicionEnListaAsync(int idUsuario, int idTurno);

    }
}