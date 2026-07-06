using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ProfesorUseCase
{
    public class ObtenerProfesoresDisponiblesUseCase(IProfesorRepositorio repoProfesor)
    {
        public async Task<List<Profesor>> Ejecutar(
       DateOnly fecha, TimeOnly horaInicio, TimeOnly horaFin, int? excluirTurnoId = null)
        {
            return await repoProfesor.ObtenerDisponiblesEdicionTurnoAsync(fecha, horaInicio, horaFin, excluirTurnoId);
        }
    }
}
