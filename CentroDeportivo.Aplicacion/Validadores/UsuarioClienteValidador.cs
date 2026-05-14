using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using CentroDeportivo.Aplicacion.Validadores;
using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Validadores
{
    public class UsuarioClienteValidador : UsuarioValidadorBase
    {
        public UsuarioClienteValidador(IUsuarioRepositorio repo, IProfesorRepositorio repoProfe) : base(repo, repoProfe)
        {
        }

        // En edición no validamos password
        protected override (bool esValidoPass, string mensajePass) ValidarPassword(Usuario u)
        {
            return (true, "");
        }

        public async Task<(bool esValido, string mensaje)> ValidarEdicion(Usuario u)
        {
            string mensaje = "";

            // Campos obligatorios
            var (datosValido, datosMsg) = await ValidarEdicionBase(u);
            if (!datosValido)
                mensaje += datosMsg;

            // Validar que el DNI no exista en otro usuario
            if (!string.IsNullOrWhiteSpace(u.Dni))
            {
                if (await _repo.YaExisteDniParaEditar(u.Dni, u.Id)) // Nota: excluye el propio usuario
                    mensaje += "El DNI ingresado ya se encuentra registrado en el sistema.\n";
                if (await _repoProfesor.YaExiste(u.Dni))
                    mensaje += "El DNI ingresado ya se encuentra registrado en el sistema.\n";
            }

            // Validar que el email no exista en otro usuario
            if (!string.IsNullOrWhiteSpace(u.Email))
            {
                if (await _repo.YaExisteEmailParaEditar(u.Email, u.Id))
                    mensaje += "El correo ingresado no se encuentra disponible.\n";
            }

            return (string.IsNullOrEmpty(mensaje), mensaje.Trim());
        }
    }
}