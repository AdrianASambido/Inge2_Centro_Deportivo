using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.TurnoUseCase
{
    public class ConsultarDisponibilidadUseCase (ICanchaRepositorio repoCancha, IProfesorRepositorio repoProfe)
    {
        public async Task<(IEnumerable<Profesor>, IEnumerable<Cancha>)> ejecutar(DateOnly fecha, TimeOnly horaInicio) {

            if (fecha < DateOnly.FromDateTime(DateTime.Today))
                throw new Exception("No se puede buscar disponibilidad para fechas pasadas.");

            var canchas = await repoCancha.ObtenerDisponiblesAsync(fecha, horaInicio);

            if (canchas == null || !canchas.Any()) {
                throw new Exception("No hay canchas disponibles en el horario seleccionado.");
            }

            var profesores = await repoProfe.ObtenerDisponiblesAsync(fecha, horaInicio);

            if (profesores == null || !profesores.Any()) {
                throw new Exception("No hay profesores disponibles en el horario seleccionado.");
            }

            return (profesores, canchas);
        }
    }
}
