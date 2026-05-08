using CentroDeportivo.Aplicacion.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Infraestructura.Servicios
{
    public class TurnosBackgroundServicio(IServiceScopeFactory scopeFactory) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Este ciclo corre mientras la app esté encendida
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = scopeFactory.CreateScope())
                {
                    
                    var repo = scope.ServiceProvider.GetRequiredService<ITurnoRepositorio>();

                    try
                    {
                        await repo.FinalizarTurnosVencidosAsync();
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }

                // Espera 10 minutos para revisar d nuevo
                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
            }
        }
    }
}
