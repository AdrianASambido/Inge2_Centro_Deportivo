using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.UsuarioUseCase
{
    public class ObtenerUsuarioPorId(IUsuarioRepositorio repo)
    {
        public async Task<Usuario> Ejecutar(int idUsuario)
        {
            var usuario = await repo.ObtenerPorIdAsync(idUsuario);
            if (usuario == null)
            {
                throw new Exception("Error: usuario inexistente");
            }
            return usuario;
        }
    }
}