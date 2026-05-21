using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Validadores;
using CentroDeportivo.Aplicacion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.UsuarioUseCase
{
    public class CambiarContraseniaUseCase(IUsuarioRepositorio repo, IHashServicio repoHash)
    {
        public async Task ejecutar(string contraVieja, string contraNueva, int idUsuario)
        {
            var usuario = await repo.ObtenerPorIdAsync(idUsuario);

            if (usuario == null)
            {
                throw new Exception("Usuario inexistente");
            }

            if (!repoHash.Verificar(contraVieja, usuario.Password))
            {
                throw new Exception("Error contraseña actual invalida");
            }

            var (esValido, mensaje) = UsuarioValidadorBase.ValidarFormatoPassword(contraNueva);
            if (!esValido)
            {
                throw new Exception(mensaje);
            }

            
            if (usuario.Rol == Rol.Empleado && usuario.DebeCambiarPassword)
            {
                usuario.DebeCambiarPassword = false;
            }
            usuario.Password = repoHash.Hashear(contraNueva);
            await repo.ActualizarAsync(usuario);
        }
    }
}