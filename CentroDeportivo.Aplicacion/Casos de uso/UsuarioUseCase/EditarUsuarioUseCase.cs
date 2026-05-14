using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Validadores;
using System;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Validadores;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.UsuarioUseCase
{
    public class EditarUsuarioUseCase
    {
        private readonly IUsuarioRepositorio _repo;
        private readonly UsuarioClienteValidador _validador;

        public EditarUsuarioUseCase(IUsuarioRepositorio repo, UsuarioClienteValidador validador)
        {
            _repo = repo;
            _validador = validador;
        }

        public async Task Ejecutar(Usuario usuario, int idUsuario)
        {
            // 1. Verificar que el usuario existe
            var existente = await _repo.ObtenerPorIdAsync(idUsuario);

            if (existente == null)
                throw new Exception("Usuario no encontrado");

            // 2. Validar datos usando el método de edición del validador
            var (esValido, mensaje) = await _validador.ValidarEdicion(usuario);
            if (!esValido)
                throw new Exception(mensaje);

            // 3. Aplicar cambios solo con los atributos existentes
            existente.Nombre = usuario.Nombre;
            existente.Apellido = usuario.Apellido;
            existente.Dni = usuario.Dni;
            existente.Email = usuario.Email;
            existente.Domicilio = usuario.Domicilio;
            existente.Password = usuario.Password;
            existente.DebeCambiarPassword = usuario.DebeCambiarPassword;
            existente.Rol = usuario.Rol;

            // 4. Guardar cambios
            await _repo.ActualizarAsync(existente);
        }
    }
}