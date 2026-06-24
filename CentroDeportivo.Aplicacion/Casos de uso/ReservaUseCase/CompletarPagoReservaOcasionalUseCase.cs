using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase
{
    public class CompletarPagoReservaOcasionalUseCase(
        IReservaRepositorio repoReserva,
        IPagoRepositorio repoPago,
        IPagoServicio pagoServicio
    ) //este se lo llama para mandarlo a mercado pago a pagar el 50 restante de la reserva ocasional
    {
        public async Task<string> Ejecutar(int idReserva)
        {
            var reserva = await repoReserva.ObtenerPorIdAsync(idReserva);

            if (reserva == null)
            {
                throw new Exception("Error: reserva inexistente.");
            }

            if (reserva.Estado != EstadoReserva.Reservado)
            {
                throw new Exception("Error: esta reserva no se encuentra pendiente de confirmación/pago restante.");
            }

            decimal montoRestante = reserva.PrecioPagado;

            bool cobroExitoso = await pagoServicio.ProcesarCobroAsync(reserva.Id_Usuario, montoRestante, tarjetaToken);

            string urlExito = $"https://localhost:7001/MisReservas?pagoExitoso=true&reservaId={reserva.Id}&tipo=confirmarOcasional";
            string urlFallo = $"https://localhost:7001/MisReservas?pagoExitoso=false&reservaId={reserva.Id}&tipo=confirmarOcasional";

            string tituloPago = $"Saldo Restante - Turno Ocasional Nro {reserva.Id_Turno}";

            string urlRedireccion = await pagoServicio.CrearPreferenciaPagoAsync(
                reserva.Id_Usuario,
                montoRestante,
                tituloPago,
                urlExito,
                urlFallo
            );

            return urlRedireccion;
        }
    }
}