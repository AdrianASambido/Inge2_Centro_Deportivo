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

        // SeedCatalogoReservas(context);
    }
    /*
    /// <summary>Cancha, profesor y actividades base si aún no existen (útil tras primera ejecución sólo con admin).</summary>
    private static void SeedCatalogoReservas(CentroDeportivoContext context)
    {
        if (!context.Canchas.Any())
            context.Canchas.Add(new Cancha(1, 30));

        if (!context.Profesores.Any())
            context.Profesores.Add(new Profesor("Profe", "General", "00000000"));

        if (!context.Actividades.Any())
        {
            context.Actividades.Add(new Actividad("Pádel", "Clase grupal", 1500));
            context.Actividades.Add(new Actividad("Fútbol", "Clase grupal", 1200));
            context.Actividades.Add(new Actividad("Vóley", "Clase grupal", 1100));
            context.Actividades.Add(new Actividad("Básquet", "Clase grupal", 1100));
        }

        context.SaveChanges();
    }
    */
}
