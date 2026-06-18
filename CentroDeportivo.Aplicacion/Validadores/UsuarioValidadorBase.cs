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
                msg += "Error: el DNI ingresado ya existe.\n";

            if (!string.IsNullOrWhiteSpace(u.Dni) && await _repoProfesor.YaExiste(u.Dni))
                msg += "Error: el DNI ingresado ya existe.\n";

            
            if (!string.IsNullOrWhiteSpace(u.Email) && !EsEmailValidoEstructuralmente(u.Email))
            {
                msg += "Error: el formato de email no es válido.\n";
            }
            
            else if (!string.IsNullOrWhiteSpace(u.Email) && await _repo.YaExisteEmail(u.Email))
            {
                msg += "Error: el email ingresado ya existe.\n";
            }

            return (string.IsNullOrEmpty(msg), msg);
        }

        public async Task<(bool esValido, string mensaje)> ValidarEdicion(Usuario u, int idUsuario)
        {
            string mensaje = "";

            
            var (camposOk, msgCampos) = await ValidarCamposObligatorios(u);
            if (!camposOk) mensaje += msgCampos;

            
            if (!string.IsNullOrWhiteSpace(u.Dni))
            {
                if (await _repo.YaExisteDniParaEditar(u.Dni, idUsuario))
                {
                    mensaje += "Error: el DNI ingresado ya se encuentra registrado.\n";
                }

                
                if (await _repoProfesor.YaExiste(u.Dni))
                {
                    mensaje += "Error: el DNI ingresado ya se encuentra registrado.\n";
                }
            }

            // 3. Validar Email (Excluyendo al usuario actual)
            if (!string.IsNullOrWhiteSpace(u.Email))
            {
                if (!EsEmailValidoEstructuralmente(u.Email))
                {
                    mensaje += "Error: el formato de email no es válido.\n";
                }
                else if (await _repo.YaExisteEmailParaEditar(u.Email, idUsuario))
                {
                    mensaje += "Error: el email ya se encuentra registrado.\n";
                }
            }

            return (string.IsNullOrEmpty(mensaje), mensaje.Trim());
        }

        // Método estático para que el Caso de Uso lo use directamente
        public static (bool esValido, string mensaje) ValidarFormatoPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return (false, "La contraseña es obligatoria. \n");

            if (!Regex.IsMatch(password, @"^(?=.*[A-Z])(?=.*\d).{8,}$"))
            {
                return (false, "Error: La contraseña debe tener al menos 8 caracteres, una mayúscula y un número. \n");
            }
            return (true, "");
        }

        private bool EsEmailValidoEstructuralmente(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

            // Esta expresión regular valida formato de email y restringe el dominio a gmail u outlook
            string patron = @"^[a-zA-Z0-9._%+-]+@(gmail|outlook)\.[a-zA-Z]{2,}(?:\.[a-zA-Z]{2,})?$";

            return Regex.IsMatch(email, patron, RegexOptions.IgnoreCase);
        }

        protected abstract (bool esValidoPass, string mensajePass) ValidarPassword(Usuario u);
    }
}