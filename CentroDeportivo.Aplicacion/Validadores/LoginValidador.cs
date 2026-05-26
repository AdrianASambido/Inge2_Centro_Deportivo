using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CentroDeportivo.Aplicacion.Validadores
{
    public class LoginValidador(IUsuarioRepositorio repoUsuario, IHashServicio repoHash)
    {
        public async Task<(bool esValido, string mensaje, Usuario? usuario)> Validar(string email, string contra)
        {


            var usuario = await repoUsuario.ObtenerPorEmail(email);
            if (usuario == null)
            {
                return (false, "Error: el email ingresado no existe.", null);
            }

            if (!repoHash.Verificar(contra, usuario.Password))
            {
                return (false, "Error: contraseña incorrecta.", null);
            }

            return (true, string.Empty, usuario);
        }
    }
}