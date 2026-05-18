using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroDeportivo.Infraestructura.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;

namespace CentroDeportivo.Infraestructura.Persistencia.Repositorios
{
    public class DevolucionRepositorio(CentroDeportivoContext contexto) : IDevolucionRepositorio
    {
        public async Task ActualizarAsync(Devolucion devolucion)
        {
             contexto.Devoluciones.Update(devolucion);
             await contexto.SaveChangesAsync();
        }

        public async Task AgregarAsync(Devolucion devolucion)
        {
            await contexto.Devoluciones.AddAsync(devolucion);
            await contexto.SaveChangesAsync();
        }

        public async Task<IEnumerable<Devolucion>> ObtenerPendientesAsync()
        {
            return await contexto.Devoluciones
                        .Include(d => d.Usuario) // Trae l9s datos personales del socio
                        .Include(d => d.Reserva) // Trae los datos de la reserva 
                        .ThenInclude(r => r.Turno)
                        .ThenInclude(t => t.Actividad)
                        .Where(d => d.Estado == DevolucionEstado.Pendiente)
                        .AsNoTracking()
                        .ToListAsync();
        }

        public async Task<Devolucion?> ObtenerPorIdAsync(int id)
        {
            return await contexto.Devoluciones.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Devolucion?> ObtenerPorReservaIdAsync(int reservaId)
        {
            return await contexto.Devoluciones.FirstOrDefaultAsync(x => x.Id_Reserva == reservaId);
        }

        public async Task<Devolucion?> ObtenerPorUsuarioIdAsync(int idUsuario)
        {
            return await contexto.Devoluciones.FirstOrDefaultAsync(x => x.Id_Usuario == idUsuario);
        }
    }
}
