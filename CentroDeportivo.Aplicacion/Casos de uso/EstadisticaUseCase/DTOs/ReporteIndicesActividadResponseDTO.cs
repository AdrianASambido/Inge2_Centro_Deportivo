using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.EstadisticaUseCase.DTOs
{
    public class ReporteIndicesActividadResponseDTO
    {
        public int TotalReservas { get; set; }
        public int CantidadAsistencias { get; set; }
        public int CantidadInasistencias { get; set; }
        public int CantidadCancelaciones { get; set; }

        public double PorcentajeAsistencia { get; set; }
        public double PorcentajeInasistencia { get; set; }
        public double PorcentajeCancelacion { get; set; }
    }
}
