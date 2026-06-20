using CentroDeportivo.Aplicacion.Casos_de_uso.EstadisticaUseCase.DTOs;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.EstadisticaUseCase
{
    public class ConsultarHistorialUsuarioUseCase(IReservaRepositorio repoReserva)
    {
        public async Task<IEnumerable<HistorialUsuarioActividadDTO>> Ejecutar(int idUsuario, DateOnly? desde = null, DateOnly? hasta = null)
        {
            return await repoReserva.ObtenerTotalesPorUsuarioAsync(idUsuario, desde, hasta);
        }
    }
}