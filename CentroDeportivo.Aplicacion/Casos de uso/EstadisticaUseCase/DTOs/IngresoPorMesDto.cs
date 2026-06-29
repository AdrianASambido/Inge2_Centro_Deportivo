namespace CentroDeportivo.Aplicacion.Casos_de_uso.EstadisticaUseCase.DTOs
{
    public class IngresoPorMesDto
    {
        public string Mes { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public decimal MontoTotal { get; set; }
    }
}
