using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ProfesorUseCase
{
    public class ListarProfesoresUseCase(IProfesorRepositorio repo)
    {
        public async Task<IEnumerable<Profesor>> ejecutar() {
            return await repo.ObtenerTodosAsync();
        }
    }
}
