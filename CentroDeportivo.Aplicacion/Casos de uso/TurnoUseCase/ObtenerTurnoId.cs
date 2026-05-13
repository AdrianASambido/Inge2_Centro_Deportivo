using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.TurnoUseCase
{
    public class ObtenerTurnoId(ITurnoRepositorio repo)
    {
        public async Task<Turno> Ejecutar(int idTurno)
        {
            var turno = await repo.ObtenerPorIdAsync(idTurno);
            if (turno == null)
            {
                throw new Exception("Error: turno inexistente");
            }
            return turno;
        }
    }
}