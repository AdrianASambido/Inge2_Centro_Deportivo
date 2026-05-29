using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Validadores;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.UsuarioUseCase
{
    public class EditarUsuarioUseCase
    {
        private readonly IUsuarioRepositorio _repo;

        private readonly UsuarioValidadorBase _validador;


        public EditarUsuarioUseCase(IUsuarioRepositorio repo, UsuarioValidadorBase validador)
        {
            _repo = repo;
            _validador = validador;
        }

        public async Task Ejecutar(Usuario usuario, int idUsuario)
        {

            var existente = await _repo.ObtenerPorIdAsync(idUsuario);
            if (existente == null)
                throw new Exception("Usuario no encontrado");


            var (esValido, mensaje) = await _validador.ValidarEdicion(usuario, idUsuario);
            if (!esValido)
                throw new Exception(mensaje);


            existente.Nombre = usuario.Nombre;
            existente.Apellido = usuario.Apellido;
            existente.Dni = usuario.Dni;
            existente.Email = usuario.Email;
            existente.Domicilio = usuario.Domicilio;


            await _repo.ActualizarAsync(existente);
        }
    }
}