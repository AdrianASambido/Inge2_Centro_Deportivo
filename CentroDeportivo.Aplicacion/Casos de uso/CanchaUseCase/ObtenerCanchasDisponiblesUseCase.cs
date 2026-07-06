using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.CanchaUseCase
{
    public class ObtenerCanchasDisponiblesUseCase(ICanchaRepositorio repoCancha)
    {
        public async Task<List<Cancha>> Ejecutar(
        DateOnly fecha, TimeOnly horaInicio, TimeOnly horaFin, int? excluirTurnoId = null)
        {
            return await repoCancha.ObtenerDisponiblesEdicionTurnoAsync(fecha, horaInicio, horaFin, excluirTurnoId);
        }
    }
}
