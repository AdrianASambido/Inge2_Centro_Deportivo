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
        public async Task<IEnumerable<Turno>> Ejecutar(DateOnly? fecha = null, int? id_Actividad = null, int? idCancha = null, int? idProfe = null, EstadoTurno? estado = null) {
            return await repo.BuscarTurnosAsync(fecha, id_Actividad, idCancha, idProfe, estado);
        }
    }
}
