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
        // agregado
        // ====================================================================
        // NUEVO BLOQUE: Inicializar Reservas de Prueba para los 3 usuarios
        // ====================================================================
        if (!context.Reservas.Any()) // Usamos tu DbSet de Reservas
        {
            // 1. Buscamos el primer turno de fútbol (7 de Julio)
            var turnoFutbol = context.Turnos.FirstOrDefault(t => t.Fecha == new DateOnly(2026, 7, 7));

            // 2. Buscamos a tus 3 usuarios clientes reales de la BD
            var juan = context.Usuarios.FirstOrDefault(u => u.Email == "juan@gmail.com");
            var maria = context.Usuarios.FirstOrDefault(u => u.Email == "maria@gmail.com");
            var nelson = context.Usuarios.FirstOrDefault(u => u.Email == "nelsonjp1999@gmail.com");

            if (turnoFutbol != null && juan != null && maria != null && nelson != null)
            {
                // El turno sale $5000. Dividimos por 3 para simular la porción de cada uno.
                decimal montoPorUsuario = Math.Round(5000m / 3m, 2); // 1666.67

                var reservasDePrueba = new List<Reserva>
                {
                    // USUARIO 1: Juan (Simulamos que ya pagó exitosamente)
                    new Reserva
                    {
                        Id_Turno = turnoFutbol.Id,
                        Id_Usuario = juan.Id,
                        PrecioPagado = montoPorUsuario,
                        Estado = EstadoReserva.Confirmado, // Tu enum (asumiendo que tenés 'Pagado' o 'Confirmado')
                        Asistencia = Asistencia.Ausente, // Por defecto al inicializar
                        FechaReserva = new DateOnly(2026, 6, 29), // Fecha de hoy simulada
                        TipoReserva = TipoReserva.Ocasional, // Tu enum para reservas comunes
                        ConCredito = false,    
                    //    ExternalReferenceMP = $"turno_{turnoFutbol.Id}_user_{juan.Id}"

                    },

                    // USUARIO 2: María (Simulamos reserva pendiente de pago)
                    new Reserva
                    {
                        Id_Turno = turnoFutbol.Id,
                        Id_Usuario = maria.Id,
                        PrecioPagado = 0, // Aún no pagó
                        Estado = EstadoReserva.PendienteDePago, // Tu enum para estados pendientes
                        Asistencia = Asistencia.Ausente,
                        FechaReserva = new DateOnly(2026, 6, 29),
                        TipoReserva = TipoReserva.Ocasional,
                        ConCredito = false,
                  //      ExternalReferenceMP = $"turno_{turnoFutbol.Id}_user_{maria.Id}"
                    },

                    // USUARIO 3: Nelson (Simulamos reserva pendiente de pago)
                    new Reserva
                    {
                        Id_Turno = turnoFutbol.Id,
                        Id_Usuario = nelson.Id,
                        PrecioPagado = 0, // Aún no pagó
                        Estado = EstadoReserva.Reservado,
                        Asistencia = Asistencia.Ausente,
                        FechaReserva = new DateOnly(2026, 6, 29),
                        TipoReserva = TipoReserva.Ocasional,
                        ConCredito = false,
                  //      ExternalReferenceMP = $"turno_{turnoFutbol.Id}_user_{nelson.Id}"
                    }
                };

                // Descontamos 3 lugares del cupo disponible del turno
                turnoFutbol.CupoDisponible -= 3;

                context.Reservas.AddRange(reservasDePrueba);
                context.SaveChanges();
            }
        }
        //hasta acá
        context.SaveChanges();
    }
}