using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.TurnoUseCase
{
    public class EditarTurnoUseCase(ITurnoRepositorio repoTurno)
    {
        public async Task Ejecutar(
            int idTurno,
            DateOnly nuevaFecha,
            TimeOnly nuevaHora,
            TimeOnly nuevaHoraFin,
            decimal nuevoPrecio,
            int nuevoCupo,
            int idProfesor,
            int idCancha)
        {
            var turno = await repoTurno.ObtenerPorIdAsync(idTurno);
            if (turno == null)
                throw new Exception("Error: El turno que intenta editar no existe.");

            bool tieneInscriptos = await repoTurno.TieneInscriptosAsync(idTurno);
            if (tieneInscriptos)
                throw new Exception("No se puede modificar el turno porque ya cuenta con usuarios inscriptos.");

            var horaMinima = new TimeOnly(18, 0);
            var horaMaxima = new TimeOnly(23, 0);
            if (nuevaHora < horaMinima || nuevaHora > horaMaxima)
                throw new Exception("El horario de inicio debe estar entre las 18:00 y las 23:00.");

            if (nuevoCupo <= 0)
                throw new Exception("El cupo del turno debe ser mayor a cero.");

            if (nuevoPrecio <= 0)
                throw new Exception("El precio del turno debe ser mayor a cero.");

            turno.Fecha = nuevaFecha;
            turno.HoraInicio = nuevaHora;
            turno.HoraFin = nuevaHoraFin;
            turno.PrecioTurno = nuevoPrecio;
            turno.CupoMaximo = nuevoCupo;
            turno.CupoDisponible = nuevoCupo;
            turno.Id_Profesor = idProfesor;
            turno.Id_Cancha = idCancha;

            await repoTurno.ActualizarAsync(turno);
        }
    }
}