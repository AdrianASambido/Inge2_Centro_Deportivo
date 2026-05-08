using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Entidades;

public class ListarTurnosCalendarioUseCase(
    AsegurarTurnosDelDiaUseCase asegurarTurnosDelDia,
    ITurnoRepositorio turnoRepositorio)
{
    public class ListarTurnosCalendarioUseCase (ITurnoRepositorio repo)
    {
        public async Task<IEnumerable<Turno>> ejecutar(int idUsuario, int? idActividad = null) {
           return await repo.ObtenerParaCalendarioAsync(idUsuario,idActividad);
        }
    }
}
