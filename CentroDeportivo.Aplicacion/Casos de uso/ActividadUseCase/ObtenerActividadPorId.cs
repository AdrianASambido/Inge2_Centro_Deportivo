using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Entidades;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ActividadUseCase
{
    public class ObtenerActividadPorId(IActividadRepositorio repo)
    {
        public async Task<Actividad> Ejecutar(int idActividad)
        {
            var actividad = await repo.ObtenerPorIdAsync(idActividad);
            if (actividad == null)
            {
                throw new Exception("Error: actividad inexistente");
            }
            return actividad;
        }
    }
}
