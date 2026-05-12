using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Entidades;

namespace CentroDeportivo.Aplicacion.Validadores
{
    public class ReservaValidador(IReservaRepositorio repoReserva, ITurnoRepositorio repoTurno)
    {
        public async Task<(bool esValido, string mensaje)> ValidarReserva(Reserva reserva) {
            var turno = await repoTurno.ObtenerPorIdAsync(reserva.Id_Turno);

            if (turno == null) {
                return (false,"Error: el turno seleccionado no existe");
            }

            if (turno.Estado != EstadoTurno.Disponible) {
                return(false,"Error: turno no disponible");
            }

            if (turno.CupoDisponible <= 0)
            {
                return (false, "Error: el turno ya completó su cupo máximo");
            }

            bool seSuperpone = await repoReserva.TieneConflictoHorarioAsync(reserva.Id_Usuario, turno.Fecha, turno.HoraInicio);
            if (seSuperpone) {
                return(false,"Error: ya posee un turno activo en fecha y horario seleccionado.");
            }

            return (true, "");
        }
    }
}