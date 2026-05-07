using CentroDeportivo.Aplicacion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Interfaces
{
    public interface IDevolucionRepositorio
    {
        Task<IEnumerable<Devolucion>> ObtenerPendientesAsync();
        Task<Devolucion?> ObtenerPorIdAsync(int id);
        Task<Devolucion?> ObtenerPorReservaIdAsync(int reservaId);
        Task<Devolucion?> ObtenerPorUsuarioIdAsync(int idUsuario);
        Task AgregarAsync(Devolucion devolucion);
        Task ActualizarAsync(Devolucion devolucion);
    }
}
