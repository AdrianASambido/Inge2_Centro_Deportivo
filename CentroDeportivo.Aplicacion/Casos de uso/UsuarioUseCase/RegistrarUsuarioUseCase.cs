using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Validadores;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.UsuarioUseCase;

public class RegistrarUsuarioUseCase (IUsuarioRepositorio repo, UsuarioClienteValidador validador, IHashServicio repoHash)
{
    public async Task ejecutar(Usuario u) {
        var (esValido, mensaje) = await validador.ValidarDatosComunes(u);

        if (!esValido) {
            throw new Exception(mensaje);
        }

        repoHash.Hashear(u.Password);
        u.Rol = Rol.Cliente;

       await repo.AgregarAsync(u);
    }
}
