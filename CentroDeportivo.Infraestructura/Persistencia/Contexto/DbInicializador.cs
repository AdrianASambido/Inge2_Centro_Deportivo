using CentroDeportivo.Aplicacion.Entidades;
using Microsoft.EntityFrameworkCore;

namespace CentroDeportivo.Infraestructura.Persistencia.Contexto;

public static class DbInicializador
{
    public static void Initialize(CentroDeportivoContext context)
    {
        context.Database.EnsureCreated();

        if (!context.Usuarios.Any())
        {
            var admin = new Usuario
            {
                Nombre = "Carlos",
                Apellido = "Tevez",
                Domicilio = "Calle 123",
                Dni = "112345534",
                Email = "admin@gmail.com",
                Password = BCrypt.Net.BCrypt.HashPassword("Admin1234"),
                Rol = Rol.Administrador,
                DebeCambiarPassword = false
            };
            context.Usuarios.Add(admin);
            context.SaveChanges();
        }

        
    }
   
}
