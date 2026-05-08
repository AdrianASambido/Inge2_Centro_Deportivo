using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;


namespace CentroDeportivo.Aplicacion.Validadores
{
    public class TurnoValidador (IProfesorRepositorio repoProfe, ICanchaRepositorio repoCancha)
    {
        public async Task<(bool esValido, string mensaje)> validarTurno(Turno turno) {

            string mensaje = "";

            var cancha = await repoCancha.ObtenerPorIdAsync(turno.Id_Cancha);

            if (cancha == null)
            {
                mensaje = "Error: cancha inexistente.\n";

            }

            var profesor = await repoProfe.ObtenerPorIdAsync(turno.Id_Profesor);

            if (profesor == null)
            {
                mensaje += "Error: profesor inexistente.\n";
            }

            if (!string.IsNullOrWhiteSpace(mensaje)) {
                return (false, mensaje);
            }

            if (turno.CupoMaximo > cancha.Capacidad)
            {
                mensaje = "Error: el cupo ingresado supera la capacidad de la cancha.\n";
            }

            TimeSpan duracion = turno.HoraFin - turno.HoraInicio;

            if (duracion.TotalHours != 1) {
                mensaje += "Error : los turnos deben durar 1 hora. \n";
            }

            if (!await repoProfe.EstaDisponibleAsync(turno.Id_Profesor, turno.Fecha, turno.HoraInicio))
            {
                mensaje += "Error : el profesor seleccionado no esta disponible. \n";
            }

            if (!await repoCancha.EstaDisponibleAsync(turno.Id_Cancha, turno.Fecha, turno.HoraInicio)){

                mensaje += "Error : la cancha seleccionada no esta disponbile. \n";
            }
            
            return (string.IsNullOrWhiteSpace(mensaje), mensaje);
        }

       
    }
}