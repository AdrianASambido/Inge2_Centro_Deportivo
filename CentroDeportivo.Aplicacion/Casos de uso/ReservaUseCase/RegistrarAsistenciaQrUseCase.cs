using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase
{       public class RegistrarAsistenciaQrUseCase(IReservaRepositorio repoReserva)
        {
            public async Task Ejecutar(string tokenEscaneado)
            {
               
                var reserva = await repoReserva.ObtenerPorQrTokenAsync(tokenEscaneado);

                if (reserva == null)
                {
                    throw new Exception ("Error: Código QR inválido o inexistente.");
                }

                if (reserva.Turno.Estado == EstadoTurno.Finalizado) {
                    throw new Exception("Error: la clase ya finalizo.");
                }
                
                if (reserva.Asistencia == Asistencia.Presente)
                {
                    throw new Exception ("Error: Esta asistencia ya fue registrada anteriormente.");
                }
                
                if (reserva.Turno.Fecha != DateOnly.FromDateTime(DateTime.Now))
                {
                    throw new Exception ("Error: El QR corresponde a una reserva de otro día");
                }

               
                reserva.Asistencia = Asistencia.Presente;
                await repoReserva.ActualizarAsync(reserva);           
            }
        }
    }

