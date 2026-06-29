using CentroDeportivo.Aplicacion.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CentroDeportivo.Infraestructura.Persistencia.Contexto;

public static class DbInicializador
{
    public static void Initialize(CentroDeportivoContext context)
    {
        context.Database.EnsureCreated();

        if (!context.Usuarios.Any())
        {
            var usuarios = new List<Usuario>
            {
                new Usuario("Carlos", "Tevez", "Calle 123", "112345534", BCrypt.Net.BCrypt.HashPassword("Admin1234"), "admin@gmail.com", false, Rol.Administrador),
                new Usuario("Juan", "Perez", "Calle 456", "30111222", BCrypt.Net.BCrypt.HashPassword("Boca1999"), "juan@gmail.com", false, Rol.Cliente),
                new Usuario("Maria", "Gomez", "Calle 789", "32444555", BCrypt.Net.BCrypt.HashPassword("Boca1999"), "maria@gmail.com", false, Rol.Cliente),
                new Usuario("nel", "pin", "Calle10", "123456", BCrypt.Net.BCrypt.HashPassword("Boca1999"), "nelsonjp1999@gmail.com", false, Rol.Cliente)
            };
            context.Usuarios.AddRange(usuarios);
            context.SaveChanges();
        }

        if (!context.Canchas.Any())
        {
            var canchas = new List<Cancha>
            {
                new Cancha(1, 10),
                new Cancha(2, 10),
                new Cancha(3, 12)
            };
            context.Canchas.AddRange(canchas);
            context.SaveChanges();
        }

        if (!context.Profesores.Any())
        {
            var profesores = new List<Profesor>
            {
                new Profesor("Lionel", "Messi", "25000111"),
                new Profesor("Alberto", "Tevez", "26000222"),
                new Profesor("Juan", "Riquelme", "27000333")
            };
            context.Profesores.AddRange(profesores);
            context.SaveChanges();
        }

        if (!context.Actividades.Any())
        {
            var actividades = new List<Actividad>
            {
                new Actividad("Futbol", "Actividad recreativa de futbol 5", 50),
                new Actividad("Voley", "Entrenamiento de voley", 30)
            };
            context.Actividades.AddRange(actividades);
            context.SaveChanges();
        }

        if (!context.Turnos.Any())
        {
            
            var futbol = context.Actividades.FirstOrDefault(a => a.Nombre == "Futbol");
            var cancha1 = context.Canchas.FirstOrDefault(c => c.Numero == 1);
            var profeMessi = context.Profesores.FirstOrDefault(p => p.Nombre == "Lionel");

            var fechasMartesJulio = new List<DateOnly>
            {
                new DateOnly(2026, 7, 7),
                new DateOnly(2026, 7, 14),
                new DateOnly(2026, 7, 21),
                new DateOnly(2026, 7, 28)
            };

            if (futbol != null && cancha1 != null && profeMessi != null)
            {
                var turnos = fechasMartesJulio.Select(fecha => new Turno
                {
                    Fecha = fecha,
                    HoraInicio = new TimeOnly(18, 0),
                    HoraFin = new TimeOnly(19, 0),
                    PrecioTurno = 5000,
                    CupoMaximo = 10,
                    CupoDisponible = 10,
                    Estado = EstadoTurno.Disponible,
                    Actividad = futbol,
                    Cancha = cancha1,
                    Profesor = profeMessi
                }).ToList();
                context.Turnos.AddRange(turnos);
                context.SaveChanges();
            }
        }

        context.SaveChanges();
    }
}