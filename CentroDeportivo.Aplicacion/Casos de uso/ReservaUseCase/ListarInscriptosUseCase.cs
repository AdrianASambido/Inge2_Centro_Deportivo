using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Entidades;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase
{
    public class ListarInscriptosUseCase (IReservaRepositorio repo, ITurnoRepositorio repoTurno)
    {
        public async Task<IEnumerable<Reserva>> Ejecutar(int idTurno, string? dniCliente = null) { 
            var turno = await repoTurno.ObtenerPorIdAsync(idTurno);

            if (turno == null) {
                throw new Exception("Error: turno inexistente");
            }

            var reservas = await repo.ObtenerPorTurnoAsync(idTurno, dniCliente);

            return reservas;
        }
    }
}
