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
        public Task ActualizarAsync(Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public async Task AgregarAsync(Usuario usuario)
        {
            await context.Usuarios.AddAsync(usuario);
            await context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Usuario?> ObtenerPorDniAsync(string dni)
        {
            throw new NotImplementedException();
        }

        public async Task<Usuario?> ObtenerPorEmail(string email)
        {
            return await context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Usuario>> ObtenerTodosAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> YaExiste(string dni)
        {
            return await context.Usuarios.AnyAsync(u => u.Dni == dni);
        }

        public async Task<bool> YaExisteEmail(string email)
        {
            return await context.Usuarios.AnyAsync(u => u.Email == email);
        }
    }
}
