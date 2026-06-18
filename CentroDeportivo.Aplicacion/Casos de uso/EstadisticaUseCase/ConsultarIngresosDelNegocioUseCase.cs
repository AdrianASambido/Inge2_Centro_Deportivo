using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.EstadisticaUseCase
{
    public class ConsultarIngresosDelNegocioUseCase(IPagoRepositorio repoPago)
    {
        public async Task<decimal> Ejecutar(DateOnly desde, DateOnly hasta) { 
            return await repoPago.ObtenerIngresosGeneralesAsync(desde, hasta);
        }
    }
}
