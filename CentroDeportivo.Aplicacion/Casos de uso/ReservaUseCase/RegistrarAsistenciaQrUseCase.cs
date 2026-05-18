using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Validadores;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase
{       public class RegistrarAsistenciaQrUseCase(IReservaRepositorio repoReserva, AsistenciaValidador validador)
        {
            public async Task Ejecutar(string tokenEscaneado)
            {
            
                var reserva = await repoReserva.ObtenerPorQrTokenAsync(tokenEscaneado);

                if (reserva == null)
                {
                    throw new Exception ("Error: Código QR inválido o inexistente.");
                }

                var (esValido, mensaje) = await validador.ValidarAsistencia(reserva);
                if (!esValido) {
                    throw new Exception(mensaje);
                }
       
                
                reserva.Asistencia = Asistencia.Presente;
                await repoReserva.ActualizarAsync(reserva);           
            }
        }
    }

