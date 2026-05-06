using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Validadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ProfesorUseCase
{
    public class EliminarProfesorUseCase (IProfesorRepositorio repo, ProfesorValidador validador)
    {
        public async Task ejecutar(int id) { 
            var (esValido, mensaje) = await validador.ValidarEliminacion(id);

            if (!esValido) {
                throw new Exception(mensaje);
            }

            await repo.EliminarAsync(id);
        }
    }
}
