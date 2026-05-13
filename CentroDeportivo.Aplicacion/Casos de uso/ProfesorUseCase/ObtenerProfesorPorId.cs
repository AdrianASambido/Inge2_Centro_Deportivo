using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ProfesorUseCase
{
    public class ObtenerProfesorPorId(IProfesorRepositorio repo)
    {
        public async Task<Profesor> Ejecutar(int idProfesor)
        {
            var profesor = await repo.ObtenerPorIdAsync(idProfesor);
            if (profesor == null)
            {
                throw new Exception("Error: profesor inexistente.");
            }
            return profesor;
        }
    }
}