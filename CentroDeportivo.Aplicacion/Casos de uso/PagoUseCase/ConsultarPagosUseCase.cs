using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.PagoUseCase
{
    public class ConsultarPagosUseCase(IPagoRepositorio repoPago)
    {
        public async Task<IEnumerable<Pago>> Ejecutar(int idUsuario)
        {
            return await repoPago.ObtenerTodosPorUsuarioAsync(idUsuario);
        }
    }
}