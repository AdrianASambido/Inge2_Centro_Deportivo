using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.EstadisticaUseCase.DTOs
{
    public record ReporteIndicesActividadDTO
    {
        public int TotalReservas { get; }
        public int TotalAsistencias { get; }
        public int TotalCancelaciones { get; }
        public int TotalInasistencias { get; }

        public ReporteIndicesActividadDTO(int total, int presentes, int ausentes, int canceladas)
        {
            TotalReservas = total;
            TotalAsistencias = presentes;
            TotalCancelaciones = ausentes;
            TotalInasistencias = canceladas;
        }
    }
}