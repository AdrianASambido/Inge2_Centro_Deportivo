using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Validadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Entidades;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.UsuarioUseCase;

public class CrearEmpleadoUseCase (IUsuarioRepositorio repo, UsuarioEmpleadoValidador validador, IHashServicio repoHash, IEmailServicio repoEmail)
{
    public async Task ejecutar(Usuario u) {
        u.Domicilio = "Calle 20";
        var (esValido, mensaje) = await validador.ValidarDatosComunes(u);

        if (!esValido)
        {
            throw new Exception(mensaje);
        }

        // "Temp" ( 1 Mayúscula) + los últimos 4 del DNI (Números) = 8 caracteres
        string passwordTemporal = $"Temp{u.Dni.Substring(u.Dni.Length - 4)}";
        u.DebeCambiarPassword = true;
        u.Password = repoHash.Hashear(passwordTemporal);

        u.Rol = Rol.Empleado;

        await repo.AgregarAsync(u);
        await repoEmail.EnviarContraseniaTemporalAsync(u.Email, passwordTemporal);
    }
}
