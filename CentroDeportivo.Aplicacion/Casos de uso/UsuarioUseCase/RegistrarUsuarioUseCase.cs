using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Validadores;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.UsuarioUseCase;

public class RegistrarUsuarioUseCase
{
    private readonly IUsuarioRepositorio _repo;
    private readonly UsuarioClienteValidador _validador;
    private readonly IHashServicio _repoHash;

    // Constructor
    public RegistrarUsuarioUseCase(IUsuarioRepositorio repo, UsuarioClienteValidador validador, IHashServicio repoHash)
    {
        _repo = repo;
        _validador = validador;
        _repoHash = repoHash;
    }

    // Método principal
    public async Task ejecutar(Usuario u)
    {
        var (esValido, mensaje) = await _validador.ValidarDatosComunes(u);

        if (!esValido)
        {
            throw new Exception(mensaje);
        }

        u.Password = _repoHash.Hashear(u.Password);
        u.Rol = Rol.Cliente;

        await _repo.AgregarAsync(u);
    }
}
