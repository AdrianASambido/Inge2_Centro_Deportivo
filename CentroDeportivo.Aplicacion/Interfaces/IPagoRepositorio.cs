using CentroDeportivo.Aplicacion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Interfaces
{
    public interface IPagoRepositorio
    {
        Task AgregarAsync(Pago pago);
        Task<IEnumerable<Pago>> ObtenerTodosPorTurnoAsync(int idTurno);
        Task<Pago?> ObtenerPorReservaAsync(int idReserva);
        Task<Pago?> ObtenerPorIdAsync (int  idPago);
        Task<Pago?> ObtenerPorPaqueteAsync(Guid codigo); //codigo guid
        Task<Pago?> ObtenerPorUsuarioAsync(int idUsuario);
        Task<IEnumerable<Pago>> ObtenerTodosPorUsuarioAsync(int idUsuario);
        Task<IEnumerable<Pago>> ObtenerTodosAsync();
        Task<decimal> ObtenerIngresosPorActividadAsync(int idActividad, DateOnly desde, DateOnly hasta);
        Task<decimal> ObtenerIngresosGeneralesAsync(DateOnly desde, DateOnly hasta);


    }
}