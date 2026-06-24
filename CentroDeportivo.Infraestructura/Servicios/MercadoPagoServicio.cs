using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Interfaces;
using MercadoPago.Config;
using MercadoPago.Client.Preference;
using MercadoPago.Resource.Preference;

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
    }
}