using CentroDeportivo.Aplicacion.Validadores;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase
{
    public class GenerarQrUseCase(IQrServicio repo, IReservaRepositorio repoReserva, AsistenciaValidador validador)
    {
        public async Task<byte[]> Ejecutar(int idReserva) {
            var reserva = await repoReserva.ObtenerPorIdAsync(idReserva);
            if (reserva == null)
            {
                throw new Exception("Error: reserva inexistente");
            }

            var (esValido, mensaje) = await validador.ValidarAsistencia(reserva);

            if (!esValido) {
                throw new Exception(mensaje);
            }

            

            string token = repo.GenerarToken();
            reserva!.TokenQr = token;

            await repoReserva.ActualizarAsync(reserva);
            
            return repo.GenerarImagenQr(token);
        }
    }
}