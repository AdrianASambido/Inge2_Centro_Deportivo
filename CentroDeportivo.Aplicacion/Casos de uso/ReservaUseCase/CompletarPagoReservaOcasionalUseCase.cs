using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase
{
    public class CompletarPagoReservaOcasionalUseCase(
        IReservaRepositorio repoReserva,
        IPagoServicio pagoServicio
    )
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

            string urlExito = $"https://localhost:7001/pago-exitoso-restante?reservaId={reserva.Id}";

            string tituloPago = $"Seña Turno Ocasional Nro {reserva.Id_Turno} - Saldo Restante";

            string urlRedireccion = await pagoServicio.CrearPreferenciaPagoAsync(
                reserva.Id_Usuario,
                montoRestante,
                tituloPago,
                urlExito
            );

            return urlRedireccion;
        }
    }
}