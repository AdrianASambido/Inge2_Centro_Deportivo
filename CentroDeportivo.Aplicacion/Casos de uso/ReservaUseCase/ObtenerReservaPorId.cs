using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase
{
    public class ObtenerReservaPorId (IReservaRepositorio repo)
    {
        public async Task<Reserva> Ejecutar (int idReserva)
        {
            var reserva = await repo.ObtenerPorIdAsync (idReserva);
            if (reserva == null) {
                throw new Exception("Error: reserva inexistente");
            }
            return reserva;
        }
    }
}
