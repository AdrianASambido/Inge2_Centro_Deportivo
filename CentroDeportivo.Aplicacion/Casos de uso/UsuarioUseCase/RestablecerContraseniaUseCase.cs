using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Validadores;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.UsuarioUseCase
{
    public class RestablecerContraseniaUseCase(IUsuarioRepositorio repoUsuario, IHashServicio repoHash)
    {
        public async Task Ejecutar(string contra, string token) {
            var usuario = await repoUsuario.ObtenerPorToken(token);

            if (usuario == null) {
                throw new Exception("Error: link de recuperacion invalido");
            }

            if (DateTime.Now > usuario.TokenRecuperacionVencimiento) {
                throw new Exception("Este link de recuperación ha vencido. Solicita un nuevo enlace desde la pantalla de ingreso");
            }

            var (esValido, mensaje) = UsuarioValidadorBase.ValidarFormatoPassword(contra);
            if (!esValido) {
                throw new Exception(mensaje);
            }

            usuario.Password = repoHash.Hashear(contra);
            usuario.TokenRecuperacionVencimiento = null;
            usuario.TokenRecuperacion = null;
            await repoUsuario.ActualizarAsync(usuario);

        }
    }
}
