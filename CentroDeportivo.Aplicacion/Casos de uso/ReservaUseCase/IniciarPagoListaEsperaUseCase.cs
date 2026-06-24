using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase
{
    public class IniciarPagoListaEsperaUseCase(
     ITurnoRepositorio repoTurno,
     IPagoServicio pagoServicio)
    {
        public async Task<string> Ejecutar(int idUsuario, int idTurno)
        {
            var turno = await repoTurno.ObtenerPorIdAsync(idTurno);
            if (turno == null || turno.CupoDisponible <= 0)
            {
                throw new Exception("El turno ya no está disponible.");
            }

            // Aquí cobramos el 100%
            decimal montoTotal = turno.PrecioTurno;

            // IMPORTANTE: Un nuevo tipo de URL para que el sistema sepa que es 100%
            string urlExito = $"https://localhost:7001/Turnos/lista-espera?pagoExitoso=true&turnoId={idTurno}";
            string urlFallo = $"https://localhost:7001/Turnos/lista-espera?pagoExitoso=false&turnoId={idTurno}";

            string nombreActividad = $"Pago Total Turno Nro {turno.Id} (Lista de Espera)";

            return await pagoServicio.CrearPreferenciaPagoAsync(
                idUsuario,
                montoTotal,
                nombreActividad,
                urlExito,
                urlFallo
            );
        }
    }
}