using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Validadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.UsuarioUseCase
{
<<<<<<< HEAD
    public class CambiarContraseniaUseCase
=======
    public class CambiarContraseniaUseCase(IUsuarioRepositorio repo, UsuarioValidadorBase validador, IHashServicio repoHash)
>>>>>>> d2f48e2d146661fe7a41a53c37d5e54b8d046751
    {
        public async Task ejecutar(string contraVieja, string contraNueva, int idUsuario) 
        {
            var usuario = await repo.ObtenerPorIdAsync(idUsuario);

            if (usuario == null) {
                throw new Exception("Usuario inexistente");
            }

            var (esValido, mensaje) = UsuarioValidadorBase.ValidarFormatoPassword(contraNueva);
            if (!esValido) {
                throw new Exception(mensaje);
            }

            if (!repoHash.Verificar(contraVieja, usuario.Password)){
                throw new Exception("La contraseña actual es incorrecta");
            }

            usuario.Password = repoHash.Hashear(contraNueva);
            await repo.ActualizarAsync(usuario);
        }
    }
}
