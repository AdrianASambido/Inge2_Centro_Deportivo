using System;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;

namespace CentroDeportivo.Aplicacion.Validadores
{
    public class ProfesorValidador
    {
        private readonly IProfesorRepositorio _repoProfe;
        private readonly IUsuarioRepositorio _repo;

        public ProfesorValidador(IProfesorRepositorio repoProfe, IUsuarioRepositorio repo)
        {
            _repoProfe = repoProfe;
            _repo = repo;
        }

        public async Task<(bool esValido, string mensaje)> Validar(Profesor p)
        {
            var (camposOk, msg) = await ValidarDatosComunes(p);

            string mensaje = msg;
            if (!string.IsNullOrWhiteSpace(p.Dni))
            {
                if (await _repoProfe.YaExiste(p.Dni))
                {
                    mensaje += "Error: el DNI ingresado ya existe.\n";
                }
                if (await _repo.YaExiste(p.Dni))
                {
                    mensaje += "Error: el DNI ingresado ya existe..\n";
                }
            }
            return (string.IsNullOrEmpty(mensaje), mensaje);
        }

        public async Task<(bool esValido, string mensaje)> ValidarEdicion(Profesor p, int id)
        {
            var (camposOk, msg) = await ValidarDatosComunes(p);

            string mensaje = msg;

            if (!string.IsNullOrWhiteSpace(p.Dni))
            {
                if (await _repoProfe.YaExisteDniParaEditar(p.Dni, id))
                {
                    mensaje += "Error: el DNI ingresado ya existe.\n";
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
            var profesor = await _repoProfe.ObtenerPorIdAsync(idProfesor);

            if (profesor == null)
                return (false, "Error: el profesor no existe.");

            if (await _repoProfe.TieneTurnosAsignadosAsync(idProfesor))
                return (false, "Error al eliminar, el profesor posee turnos agendados.");

            return (true, "");
        }
    }
}