using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Validadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Entidades;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.TurnoUseCase
{
    public class CrearTurnoUseCase (ITurnoRepositorio repo, IActividadRepositorio repoActividad, TurnoValidador validador)
    {
        public async Task ejecutar(Turno turno) {
            var (esValido, mensaje) = await validador.validarTurno(turno);

            if (!esValido) {
                throw new Exception(mensaje);
            }

            var actividad = await repoActividad.ObtenerPorIdAsync(turno.Id_Actividad);
            turno.PrecioTurno = actividad!.Precio;

            turno.Estado = EstadoTurno.Disponible;
            turno.CupoDisponible = turno.CupoMaximo;
            await repo.AgregarAsync(turno);
        }
    }
}
