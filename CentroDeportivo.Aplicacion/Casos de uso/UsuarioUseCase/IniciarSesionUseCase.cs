using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Validadores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.UsuarioUseCase
{
    public class IniciarSesionUseCase(LoginValidador validador)
    {
        public async Task<Usuario> ejecutar(string email, string contra)
        {
            var (esValido, mensaje, usuario) = await validador.Validar(email, contra);

            if (!esValido)
            {
                throw new Exception(mensaje);
            }

            return usuario;
        }
    }
}
