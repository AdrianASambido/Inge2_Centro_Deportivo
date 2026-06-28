using CentroDeportivo.Aplicacion.Interfaces;
using MercadoPago.Client.Payment;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Client.PaymentMethod;
using MercadoPago.Client.DisbursementRefund;
using MercadoPago.Resource.Preference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Infraestructura.Servicios
{
    public class MercadoPagoServicio : IPagoServicio
    {

        public MercadoPagoServicio()
        {

              //  MercadoPagoConfig.AccessToken = "";
        }

        public async Task<string> CrearPreferenciaPagoAsync(
                    int idUsuario,
                    decimal monto,
                    string nombreActividad,
                    string urlExito,
                    string urlFallo)
        {
            try
            {
                var client = new PreferenceClient();

                var request = new PreferenceRequest
                {
                    Items = new List<PreferenceItemRequest>
            {
                new PreferenceItemRequest
                {
                    Title = $"Reserva - {nombreActividad}",
                    Quantity = 1,
                    CurrencyId = "ARS",
                    UnitPrice = monto
                }
            },
                    BackUrls = new PreferenceBackUrlsRequest
                    {
                        Success = urlExito,
                        Failure = urlFallo,
                        Pending = urlFallo
                    },
                    AutoReturn = "approved"
                };

                Preference preference = await client.CreateAsync(request);
                return preference.SandboxInitPoint;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MercadoPago Error] Falló la creación de la preferencia: {ex.Message}");
                throw new Exception("Error al conectar con la pasarela de pagos.");
            }
        }

        public Task<bool> ProcesarCobroAsync(int idUsuario, decimal monto, string token)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ProcesarReembolsoAsync(int idUsuario, decimal montoADevolver)
        {
            throw new NotImplementedException();
        }

       /* public async Task<bool> RealizarReembolsoAsync(string idTransaccion)
        {
            Console.WriteLine($"[Reembolso] Intentando reembolsar payment_id: {idTransaccion}");
            try
            {
                var client = new MercadoPago.Client.Payment.PaymentClient();

                var requestOptions = new MercadoPago.Http.MercadoPagoRequest(
                    MercadoPago.Http.HttpMethod.Post,
                    $"/v1/payments/{idTransaccion}/refunds",
                    null
                );

                var refund = await client.RefundAsync(long.Parse(idTransaccion));
                return refund != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en reembolso: {ex.Message}");
                return false;
            }
        }*/

        public async Task<bool> RealizarReembolsoAsync(string idTransaccion)
          {
              Console.WriteLine($"[Reembolso SIMULADO] payment_id: {idTransaccion}");
              await Task.CompletedTask;
              return true;
          } 
    }
}