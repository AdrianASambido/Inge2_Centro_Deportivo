using CentroDeportivo.Aplicacion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Interfaces
{
    public interface IUsuarioRepositorio
    {
        Task<Usuario?> ObtenerPorIdAsync(int id);
        Task<Usuario?> ObtenerPorDniAsync(string dni);
        Task<Usuario?> ObtenerPorEmail(string email);
        Task<Usuario?> ObtenerPorToken(string token); 
        Task<IEnumerable<Usuario>> ObtenerTodosAsync();
        Task<IEnumerable<Usuario>> ObtenerEmpleadosAsync();
        Task<IEnumerable<Usuario>> ObtenerClientesAsync();
        Task AgregarAsync(Usuario usuario);
        Task ActualizarAsync(Usuario usuario);
        Task EliminarAsync(int id);
        Task<bool> YaExiste(string dni);
        Task<bool> YaExisteEmail(string email);
        Task<bool> YaExisteDniParaEditar(string dni, int idUsuario);
        Task<bool> YaExisteEmailParaEditar(string email, int idUsuario);
       
    }
}
