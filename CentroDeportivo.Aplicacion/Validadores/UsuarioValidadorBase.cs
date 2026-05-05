using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Entidades;

namespace CentroDeportivo.Aplicacion.Validadores
{
    public abstract class UsuarioValidadorBase(IUsuarioRepositorio repo)
    {
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

        protected abstract (bool esValidoPass, string mensajePass) ValidarPassword(Usuario u);
    }
}
