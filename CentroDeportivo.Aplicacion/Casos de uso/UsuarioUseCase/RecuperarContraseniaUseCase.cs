using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Interfaces;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.UsuarioUseCase
{
    public class RecuperarContraseniaUseCase(IUsuarioRepositorio repo, IEmailServicio repoEmail)
    {
        public async Task Ejecutar(string email) { 
            var usuario = await repo.ObtenerPorEmail(email);

            if (usuario == null) {
                throw new Exception("Email incorrecto.");
            }

            var token = Guid.NewGuid().ToString();
            var vencimiento = DateTime.Now.AddHours(1);

            usuario.TokenRecuperacion = token;
            usuario.TokenRecuperacionVencimiento = vencimiento;

            var link = $"https://centrodeportivo.com/restablecer-contrasenia?token={token}";
            await repoEmail.EnviarLinkRecuperacionAsync(email, link);
        }
    }
}
