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
            decimal nuevoPrecio,
            int nuevoCupo)
        {
            var turno = await repoTurno.ObtenerPorIdAsync(idTurno);
            if (turno == null)
            {
                throw new Exception("Error: El turno que intenta editar no existe.");
            }

            bool tieneInscriptos = await repoTurno.TieneInscriptosAsync(idTurno);
            if (tieneInscriptos)
            {
                throw new Exception("No se puede modificar el turno porque ya cuenta con usuarios inscriptos.");
            }

            if (nuevoCupo <= 0)
            {
                throw new Exception("El cupo del turno debe ser mayor a cero.");
            }

            if (nuevoPrecio < 0)
            {
                throw new Exception("El precio del turno no puede ser un valor negativo.");
            }

            turno.Fecha = nuevaFecha;
            turno.HoraInicio = nuevaHora;
            turno.PrecioTurno = nuevoPrecio;
            turno.CupoMaximo = nuevoCupo;

            turno.CupoDisponible = nuevoCupo;

            await repoTurno.ActualizarAsync(turno);
        }
    }
}