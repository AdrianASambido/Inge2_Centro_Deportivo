using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.EstadisticaUseCase.DTOs
{
    public record HistorialUsuarioActividadDTO
    (
        string NombreActividad,
        int TotalAsistencias,
        int TotalInasistencias,
        int TotalCancelaciones
    );
}
