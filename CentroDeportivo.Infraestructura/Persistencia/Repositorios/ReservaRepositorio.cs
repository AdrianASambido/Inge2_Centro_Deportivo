using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Infraestructura.Persistencia.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Infraestructura.Persistencia.Repositorios
{
    public class ReservaRepositorio(CentroDeportivoContext contexto) : IReservaRepositorio
    {
        public async Task ActualizarAsync(Reserva reserva)
        {
            throw new NotImplementedException();
        }

        public async Task AgregarAsync(Reserva reserva)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExisteReservaActivaAsync(int usuarioId, int turnoId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Reserva>> ObtenerConDevolucionPendienteAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Reserva?> ObtenerPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Reserva?> ObtenerPorQrTokenAsync(string qrToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Reserva>> ObtenerPorTurnoAsync(int turnoId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Reserva>> ObtenerPorUsuarioAsync(int usuarioId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> TieneConflictoHorarioAsync(int usuarioId, DateOnly fecha, TimeOnly horarioInicio)
        {
            throw new NotImplementedException();
        }
    }
}
