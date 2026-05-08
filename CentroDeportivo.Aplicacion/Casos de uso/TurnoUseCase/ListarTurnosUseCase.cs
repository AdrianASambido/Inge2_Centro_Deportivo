using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Entidades;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.TurnoUseCase
{
    public class ListarTurnosUseCase(ITurnoRepositorio repo)
    {
        public async Task<IEnumerable<Turno>> Ejecutar(DateOnly? fecha, int? id_Actividad, int? idCancha, int? idProfe, EstadoTurno? estado) {
            return await repo.BuscarTurnosAsync(fecha, id_Actividad, idCancha, idProfe, estado);
        }
    }
}
