using CentroDeportivo.Aplicacion.Casos_de_uso.EstadisticaUseCase.DTOs;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.EstadisticaUseCase
{
    public class ConsultarIndicesUsuarioUseCase  (IReservaRepositorio repoReserva)
    {
        public async Task<IEnumerable<ReporteIndiceUsuarioResponseDTO>> Ejecutar(int idUsuario, DateOnly desde, DateOnly hasta) {
            var totales = await repoReserva.ObtenerTotalesPorUsuarioAsync(idUsuario, desde, hasta);

            return totales.Select(t =>
            {
                int totalReservas = t.TotalAsistencias + t.TotalInasistencias + t.TotalCancelaciones;

                if (totalReservas == 0) return new ReporteIndiceUsuarioResponseDTO { NombreActividad = t.NombreActividad };

                return new ReporteIndiceUsuarioResponseDTO
                {
                    NombreActividad = t.NombreActividad,
                    TotalAsistencias = t.TotalAsistencias,
                    TotalInasistencias = t.TotalInasistencias,
                    TotalCancelaciones = t.TotalCancelaciones,
                    PorcentajeAsistencia = Math.Round((double)t.TotalAsistencias / totalReservas * 100, 2),
                    PorcentajeInasistencia = Math.Round((double)t.TotalInasistencias / totalReservas * 100, 2),
                    PorcentajeCancelacion = Math.Round((double)t.TotalCancelaciones / totalReservas * 100, 2)
                };
            }).ToList();
        }
    }
    }

