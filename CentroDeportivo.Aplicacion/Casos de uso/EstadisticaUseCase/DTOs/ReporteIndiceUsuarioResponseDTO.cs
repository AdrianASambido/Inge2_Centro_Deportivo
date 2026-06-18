using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.EstadisticaUseCase.DTOs
{
    public class ReporteIndiceUsuarioResponseDTO
    {
        public string NombreActividad { get; set; } = string.Empty;
        public int TotalAsistencias { get; set; }
        public int TotalInasistencias { get; set; }
        public int TotalCancelaciones { get; set; }
        public double PorcentajeAsistencia { get; set; }
        public double PorcentajeInasistencia { get; set; }
        public double PorcentajeCancelacion { get; set; }

    }
}
