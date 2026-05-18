using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Validadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase
{
    public class RegistrarAsistenciaManualUseCase(IReservaRepositorio repo, AsistenciaValidador validador)
    {
        public async Task Ejecutar(int idReserva) {

            var reserva = await repo.ObtenerPorIdAsync(idReserva);
            if (reserva == null)
            {
                throw new Exception("Error: reserva inexistente");
            }

            var (esValido, mensaje) = await validador.ValidarAsistencia(reserva);
            if (!esValido) {
                throw new Exception(mensaje);
            }

                    
            reserva.Asistencia = Asistencia.Presente;
            await repo.ActualizarAsync(reserva);
        }
    }
}
