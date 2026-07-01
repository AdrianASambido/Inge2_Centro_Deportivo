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

            // MercadoPagoConfig.AccessToken = "";
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
                    
                    
                    AutoReturn = "all"
                };

                Preference preference = await client.CreateAsync(request);

                Console.WriteLine("InitPoint:");
                Console.WriteLine(preference.InitPoint);

                Console.WriteLine("Sandbox:");
                Console.WriteLine(preference.SandboxInitPoint);

                // Usamos InitPoint porque el Access Token ya pertenece a un entorno de pruebas cerrado
               // return preference.SandboxInitPoint;
                return preference.InitPoint;

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

        public async Task ProbarRefundHttp(string paymentId)
        {
            using var http = new HttpClient();

            http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer",
                    MercadoPagoConfig.AccessToken);

            
           http.DefaultRequestHeaders.Add("X-Sandbox-Mode", "true");

            var response = await http.PostAsync(
                $"https://api.mercadopago.com/v1/payments/{paymentId}/refunds",
                null);

            Console.WriteLine($"HTTP Status: {response.StatusCode}");

            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine(body);
        }

       /* public async Task<bool> RealizarReembolsoAsync(string idTransaccion)
        {
            Console.WriteLine($"[Reembolso] Intentando reembolsar payment_id: {idTransaccion}");

            var client = new PaymentClient();
            var payment = await client.GetAsync(long.Parse(idTransaccion));

            Console.WriteLine($"PaymentId API: {payment.Id}");
            Console.WriteLine($"CollectorId: {payment.CollectorId}");
            Console.WriteLine($"Status: {payment.Status}");
            Console.WriteLine($"LiveMode: {payment.LiveMode}");

           
           // await ProbarRefundHttp(idTransaccion);

            
            var refund = await client.RefundAsync(long.Parse(idTransaccion));

            return refund != null;
        }*/

         public async Task<bool> RealizarReembolsoAsync(string idTransaccion)
           {
               Console.WriteLine($"[Reembolso SIMULADO] payment_id: {idTransaccion}");
               await Task.CompletedTask;
               return true;
           } 
    }
}