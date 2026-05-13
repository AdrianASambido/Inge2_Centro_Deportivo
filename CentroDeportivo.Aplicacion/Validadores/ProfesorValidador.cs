using System;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;

namespace CentroDeportivo.Aplicacion.Validadores
{
    public class ProfesorValidador
    {
        private readonly IProfesorRepositorio _repo;

        public ProfesorValidador(IProfesorRepositorio repo)
        {
            _repo = repo;
        }

        public async Task<(bool esValido, string mensaje)> Validar(Profesor p)
        {
            var (camposOk, msg) = await ValidarDatosComunes(p);
            string mensaje = msg;
            if (!string.IsNullOrWhiteSpace(p.Dni))
            {
                if (await _repo.YaExiste(p.Dni))
                {
                    mensaje += "Error: ya existe una persona con ese DNI.\n";
                }
            }
            return (string.IsNullOrEmpty(mensaje), mensaje);
        }

        public async Task<(bool esValido, string mensaje)> ValidarEdicion(Profesor p)
        {
            var (camposOk, msg) = await ValidarDatosComunes(p);

            string mensaje = msg;

            if (!string.IsNullOrWhiteSpace(p.Dni))
            {
                if (await _repo.YaExisteDniParaEditar(p.Dni, p.Id))
                {
                    mensaje += "Error: ya existe un profesor con ese DNI.\n";
                }
            }

            return (string.IsNullOrEmpty(mensaje), mensaje);
        }

        private Task<(bool esValido, string mensaje)> ValidarDatosComunes(Profesor p)
        {
            string mensaje = "";

            if (string.IsNullOrWhiteSpace(p.Nombre) ||
                string.IsNullOrWhiteSpace(p.Apellido) ||
                string.IsNullOrWhiteSpace(p.Dni))
            {
                mensaje += "Error: Debe completar todos los campos obligatorios.\n";
            }

            return Task.FromResult((string.IsNullOrEmpty(mensaje), mensaje));
        }


        public async Task<(bool esValido, string mensaje)> ValidarEliminacion(int idProfesor)
        {
            var profesor = await _repo.ObtenerPorIdAsync(idProfesor);

            if (profesor == null)
                return (false, "Error: el profesor no existe.");

            if (await _repo.TieneTurnosAsignadosAsync(idProfesor))
                return (false, "Error al eliminar, el profesor posee turnos agendados.");

            return (true, "");
        }
    }
}