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
    public abstract class UsuarioValidadorBase(IUsuarioRepositorio repo)
    {
        protected readonly IUsuarioRepositorio _repo = repo;
        public async Task<(bool esValido, string mensaje)> ValidarDatosComunes(Usuario u) {
            string mensaje = "";

            if (string.IsNullOrWhiteSpace(u.Apellido) || string.IsNullOrWhiteSpace(u.Nombre) ||
            string.IsNullOrWhiteSpace(u.Domicilio) || string.IsNullOrWhiteSpace(u.Email) ||
            string.IsNullOrWhiteSpace(u.Dni))
            {
                mensaje += "Error: Debe completar todos los campos obligatorios.\n";
            }


            if (!string.IsNullOrWhiteSpace(u.Dni))
            {
                if (await repo.YaExiste(u.Dni))
                {
                    mensaje += "Error: El DNI ingresado ya existe. \n";
                }
            }

            if (!string.IsNullOrWhiteSpace(u.Email))
                {
                if (await repo.YaExisteEmail(u.Email)) {
                    mensaje += "Error : el email ingresado ya existe. \n";
                } 
            }

            var (valido, msjPass) = ValidarPassword(u);

            if (!valido) {
                mensaje += msjPass;
            }

            return (string.IsNullOrEmpty(mensaje), mensaje);
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
