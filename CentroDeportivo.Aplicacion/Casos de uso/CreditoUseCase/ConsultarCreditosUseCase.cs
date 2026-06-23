using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.CreditoUseCase
{
    public class ConsultarCreditosUseCase(ICreditoRepositorio repoCredito)
    {
        public async Task<IEnumerable<Credito>> Ejecutar (int idUsuario)
        {
            return await repoCredito.ObtenerTodosPorUsuarioAsync(idUsuario);
        }
    }
}