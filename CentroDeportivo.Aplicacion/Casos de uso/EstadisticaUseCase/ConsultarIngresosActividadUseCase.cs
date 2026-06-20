using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.EstadisticaUseCase
{
    public class ConsultarIngresosActividadUseCase(IPagoRepositorio repoPago)
    {
        public async Task<decimal> Ejecutar(int idActividad, DateOnly desde, DateOnly hasta)
        {
            return await repoPago.ObtenerIngresosPorActividadAsync(idActividad, desde, hasta);
        }
    }
}