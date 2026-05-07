using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Infraestructura.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Infraestructura.Persistencia.Repositorios
{
    public class UsuarioRepositorio (CentroDeportivoContext context) : IUsuarioRepositorio 
    {
        public async Task ActualizarAsync(Usuario usuario)
        {
            context.Usuarios.Update(usuario);
            await context.SaveChangesAsync();
        }

        public async Task AgregarAsync(Usuario usuario)
        {
            await context.Usuarios.AddAsync(usuario);
            await context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var usuario = await context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                context.Usuarios.Remove(usuario);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Usuario>> ObtenerClientesAsync()
        {
            return await context.Usuarios
                         .AsNoTracking()
                         .Where(u => u.Rol == Rol.Cliente)
                         .ToListAsync();
        }

        public async Task<IEnumerable<Usuario>> ObtenerEmpleadosAsync()
        {
            return await context.Usuarios
                         .AsNoTracking()
                         .Where(u => u.Rol == Rol.Empleado) 
                         .ToListAsync();
        }

        public async Task<Usuario?> ObtenerPorDniAsync(string dni)
        {
            return await context.Usuarios.FirstOrDefaultAsync(u =>  u.Dni == dni);
        }

        public async Task<Usuario?> ObtenerPorEmail(string email)
        {
            return await context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            return await context.Usuarios.FirstOrDefaultAsync(u =>u.Id == id);
        }

        public async Task<IEnumerable<Usuario>> ObtenerTodosAsync()
        {
            return await context.Usuarios
                 .AsNoTracking()
                 .ToListAsync();
        }

        public async Task<bool> YaExiste(string dni)
        {
            return await context.Usuarios.AnyAsync(u => u.Dni == dni);
        }

        public async Task<bool> YaExisteEmail(string email)
        {
            return await context.Usuarios.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> YaExisteDniParaEditar(string dni, int idActual)
        {
            return await context.Usuarios.AnyAsync(u => u.Dni == dni && u.Id != idActual);
        }

        public async Task<bool> YaExisteEmailParaEditar(string email, int idActual)
        {
            return await context.Usuarios.AnyAsync(u => u.Email == email && u.Id != idActual);
        }
    }
}
