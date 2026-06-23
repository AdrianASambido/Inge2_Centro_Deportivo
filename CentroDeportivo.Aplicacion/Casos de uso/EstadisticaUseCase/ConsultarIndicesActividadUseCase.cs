using CentroDeportivo.Aplicacion.Casos_de_uso.EstadisticaUseCase.DTOs;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.EstadisticaUseCase
{
    public class ConsultarIndicesActividadUseCase (IReservaRepositorio repoReserva)
    {
        public async Task<ReporteIndicesActividadResponseDTO> Ejecutar(int idActividad, DateOnly desde, DateOnly hasta) {
            var totales = await repoReserva.ObtenerTotalesPorActividadAsync(idActividad, desde, hasta);

            if (totales.TotalReservas == 0) {
                return new ReporteIndicesActividadResponseDTO();
            }

            double porcAsistencia = Math.Round((double)totales.TotalAsistencias / totales.TotalReservas * 100, 2);
            double porcInasistencia = Math.Round((double)totales.TotalInasistencias / totales.TotalReservas * 100, 2);
            double porcCancelaciones = Math.Round((double)totales.TotalCancelaciones / totales.TotalReservas * 100, 2);

            return new ReporteIndicesActividadResponseDTO
            {
                TotalReservas = totales.TotalReservas,
                CantidadAsistencias = totales.TotalAsistencias,
                CantidadInasistencias = totales.TotalInasistencias,
                CantidadCancelaciones = totales.TotalCancelaciones,
                PorcentajeAsistencia = porcAsistencia,
                PorcentajeInasistencia = porcInasistencia,
                PorcentajeCancelacion = porcCancelaciones,
            };
        }
    }
}