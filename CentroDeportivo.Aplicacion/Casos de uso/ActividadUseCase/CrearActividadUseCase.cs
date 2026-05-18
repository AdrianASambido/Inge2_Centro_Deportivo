using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Validadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Entidades;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ActividadUseCase
{
    public class CrearActividadUseCase(IActividadRepositorio repo, ActividadValidador validador)
    {
        public async Task ejecutar(Actividad actividad)
        {


            var (esValido, mensaje) = await validador.Validar(actividad);


            if (!esValido)
            {
                throw new Exception(mensaje);
            }

            actividad.Existe = true;
            await repo.AgregarAsync(actividad);
        }
    }
}