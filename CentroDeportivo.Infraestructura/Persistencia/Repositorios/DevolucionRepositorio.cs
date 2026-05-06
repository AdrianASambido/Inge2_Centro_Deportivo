using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Infraestructura.Persistencia.Repositorios
{
    public class DevolucionRepositorio : IDevolucionRepositorio
    {
        public Task ActualizarAsync(Devolucion devolucion)
        {
            throw new NotImplementedException();
        }

        public Task AgregarAsync(Devolucion devolucion)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Devolucion>> ObtenerPendientesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Devolucion?> ObtenerPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Devolucion?> ObtenerPorReservaIdAsync(int reservaId)
        {
            throw new NotImplementedException();
        }

        public Task<Devolucion?> ObtenerPorUsuarioIdAsync(int idUsuario)
        {
            throw new NotImplementedException();
        }
    }
}
