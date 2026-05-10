using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Validadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ActividadUseCase
{
    public class EliminarActividadUseCase(IActividadRepositorio repo, ActividadValidador validador)
    {
        public async Task Ejecutar(int idActividad)
        {
            var (esValido, mensaje) = await validador.validarEliminacion(idActividad);

            if (!esValido)
            {
                throw new Exception(mensaje);
            }

            await repo.EliminarAsync(idActividad);
        }
    }
}