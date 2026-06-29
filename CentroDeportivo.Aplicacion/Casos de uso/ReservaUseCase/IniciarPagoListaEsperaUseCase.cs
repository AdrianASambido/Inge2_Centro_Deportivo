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
            if (turno == null)
            {
                throw new Exception("El turno ya no está disponible.");
            }


            decimal montoTotal = turno.PrecioTurno;



            string urlExito = $"https://localhost:5001/MisReservas?pagoExitoso=true&tipo=listaEspera&turnoId={idTurno}&userId={idUsuario}";
            string urlFallo = $"https://localhost:5001/MisReservas?pagoExitoso=false&tipo=listaEspera&turnoId={idTurno}&userId={idUsuario}";

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