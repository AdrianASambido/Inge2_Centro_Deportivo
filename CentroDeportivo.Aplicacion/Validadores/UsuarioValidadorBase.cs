using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Validadores
{
    public abstract class UsuarioValidadorBase(IUsuarioRepositorio repo, IProfesorRepositorio repoProfe)
    {
        protected readonly IUsuarioRepositorio _repo = repo;
        protected readonly IProfesorRepositorio _repoProfesor = repoProfe;

        //Validacion en comun ambos csos
        private async Task<(bool esValido, string mensaje)> ValidarCamposObligatorios(Usuario u)
        {
            string mensaje = "";

            if (string.IsNullOrWhiteSpace(u.Apellido) ||
                string.IsNullOrWhiteSpace(u.Nombre) ||
                string.IsNullOrWhiteSpace(u.Domicilio) ||
                string.IsNullOrWhiteSpace(u.Email) ||
                string.IsNullOrWhiteSpace(u.Dni))
            {
                mensaje += "Debe completar todos los campos obligatorios.\n";
            }

            return (string.IsNullOrEmpty(mensaje), mensaje);
        }

        // Validacion para crear
        public async Task<(bool esValido, string mensaje)> ValidarDatosComunes(Usuario u)
        {
            var (camposOk, msg) = await ValidarCamposObligatorios(u);

            if (!string.IsNullOrWhiteSpace(u.Dni) && await _repo.YaExiste(u.Dni))
                msg += "El DNI pertenece a un usuario registrado.\n";

            if (!string.IsNullOrWhiteSpace(u.Dni) && await _repoProfesor.YaExiste(u.Dni))
                msg += "El DNI pertenece a un usuario registrado.\n";

            if (!string.IsNullOrWhiteSpace(u.Email) && await _repo.YaExisteEmail(u.Email))
                msg += "El correo ingresado pertenece a un usuario registrado.\n";

            return (string.IsNullOrEmpty(msg), msg);
        }

        // Validacion para editar
        public async Task<(bool esValido, string mensaje)> ValidarEdicionBase(Usuario u)
        {
            var (camposOk, msg) = await ValidarCamposObligatorios(u);

            if (!string.IsNullOrWhiteSpace(u.Dni) && await _repo.YaExisteDniParaEditar(u.Dni, u.Id))
                msg += "El DNI ingresado ya se encuentra registrado en otro usuario.\n";

            if (!string.IsNullOrWhiteSpace(u.Email) && await _repo.YaExisteEmailParaEditar(u.Email, u.Id))
                msg += "El correo ingresado ya se encuentra registrado en otro usuario.\n";

            return (string.IsNullOrEmpty(msg), msg);
        }

        // Método estático para que el Caso de Uso lo use directamente
        public static (bool esValido, string mensaje) ValidarFormatoPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return (false, "La contraseña es obligatoria. \n");

            if (!Regex.IsMatch(password, @"^(?=.*[A-Z])(?=.*\d).{8,}$"))
            {
                return (false, "La contraseña debe tener al menos 8 caracteres, una mayúscula y un número. \n");
            }
            return (true, "");
        }

        protected abstract (bool esValidoPass, string mensajePass) ValidarPassword(Usuario u);
    }
}