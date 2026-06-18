using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CentroDeportivo.Infraestructura.Servicios
{
    public class ListaEsperaWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _tiempoDePeriodo = TimeSpan.FromMinutes(1); 

        public ListaEsperaWorker(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
   
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcesarExpiracionesListaEsperaAsync();
                }
                catch (Exception ex)
                {
 
                    Console.WriteLine($"Error en el Worker de Lista de Espera: {ex.Message}");
                }

                await Task.Delay(_tiempoDePeriodo, stoppingToken);
            }
        }

        private async Task ProcesarExpiracionesListaEsperaAsync()
        {
            using var scope = _scopeFactory.CreateScope();

            var repoLista = scope.ServiceProvider.GetRequiredService<IListaDeEsperaRepositorio>();
            var repoTurno = scope.ServiceProvider.GetRequiredService<ITurnoRepositorio>();
            var emailServicio = scope.ServiceProvider.GetRequiredService<IEmailServicio>();

            DateTime ahora = DateTime.Now;

            var todos = await repoLista.ObtenerTodosAsync();

            var inscriptosExpirados = todos
                .Where(x => x.Estado == EstadoListaEspera.Notificado
                         && x.FechaNotificacion.HasValue
                         && (ahora - x.FechaNotificacion.Value).TotalMinutes >= 10)
                .ToList();

            foreach (var expirado in inscriptosExpirados)
            {

                expirado.Estado = EstadoListaEspera.Expirado;
                await repoLista.ActualizarAsync(expirado);

                var siguienteEnFila = await repoLista.ObtenerPrimeroEnFilaAsync(expirado.Id_Turno);

                var turno = await repoTurno.ObtenerPorIdAsync(expirado.Id_Turno);

                if (siguienteEnFila != null)
                {
      
                    siguienteEnFila.Estado = EstadoListaEspera.Notificado;
                    siguienteEnFila.FechaNotificacion = ahora;

     
                    await emailServicio.EnviarAvisoVacanteListaEsperaAsync(siguienteEnFila.Usuario.Email, turno!);
                    await repoLista.ActualizarAsync(siguienteEnFila);

                    Console.WriteLine($" [Worker] Tiempo expirado para usuario {expirado.Id_Usuario}. Notificado el siguiente: {siguienteEnFila.Id_Usuario}");
                }
                else
                {
  
                    if (turno != null)
                    {
                        turno.CupoDisponible++;
                        if (turno.Estado == EstadoTurno.Lleno)
                        {
                            turno.Estado = EstadoTurno.Disponible;
                        }
                        await repoTurno.ActualizarAsync(turno);
                    }

                    Console.WriteLine($" [Worker] Tiempo expirado para usuario {expirado.Id_Usuario}. Lista vacía, cupo devuelto al turno.");
                }
            }
        }
    }
}