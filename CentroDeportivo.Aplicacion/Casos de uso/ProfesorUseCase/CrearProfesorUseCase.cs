using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Validadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Entidades;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ProfesorUseCase
{
    public class CrearProfesorUseCase(IProfesorRepositorio repo, ProfesorValidador validador)
    {
        public async Task ejecutar(Profesor p) { 
            var (esValido,mensaje) = await validador.Validar(p);

            if (!esValido) {
                throw new Exception(mensaje);
            }

            await repo.AgregarAsync(p);
        }
    }
}
