using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Infraestructura.Persistencia.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        public Task ActualizarAsync(Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public Task AgregarAsync(Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public Task EliminarAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Usuario?> ObtenerPorDniAsync(string dni)
        {
            throw new NotImplementedException();
        }

        public Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Usuario>> ObtenerTodosAsync()
        {
            throw new NotImplementedException();
        }
    }
}
