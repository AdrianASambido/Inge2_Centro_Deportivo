using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Infraestructura.Persistencia.Repositorios
{
    public class ReservaRepositorio : IReservaRepositorio
    {
        public Task ActualizarAsync(Reserva reserva)
        {
            throw new NotImplementedException();
        }

        public Task AgregarAsync(Reserva reserva)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExisteReservaActivaAsync(int usuarioId, int turnoId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Reserva>> ObtenerConDevolucionPendienteAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Reserva?> ObtenerPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Reserva?> ObtenerPorQrTokenAsync(string qrToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Reserva>> ObtenerPorTurnoAsync(int turnoId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Reserva>> ObtenerPorUsuarioAsync(int usuarioId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TieneConflictoHorarioAsync(int usuarioId, DateOnly fecha, TimeOnly horarioInicio)
        {
            throw new NotImplementedException();
        }
    }
}
