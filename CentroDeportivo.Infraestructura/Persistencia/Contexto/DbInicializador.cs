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

        // Asegurar reservas/pagos sobre turnos finalizados para que las estadísticas los contabilicen
        var turnosFinalizados = context.Turnos.Where(t => t.Estado == EstadoTurno.Finalizado).ToList();
        if (turnosFinalizados.Any())
        {
            // si no hay reservas asociadas a finalizados, creamos algunas
            var existenSobreFinalizados = context.Reservas
                                        .Include(r => r.Turno)
                                        .Any(r => r.Turno != null && r.Turno.Estado == EstadoTurno.Finalizado);
            if (!existenSobreFinalizados)
            {
                var usuarios = context.Usuarios.Take(5).ToList();
                var turnoFinal = turnosFinalizados.First();

                // Reserva confirmada y presente (pagada)
                if (usuarios.Count > 0)
                {
                    var rc = new Reserva
                    {
                        Id_Usuario = usuarios[0].Id,
                        Id_Turno = turnoFinal.Id,
                        PrecioPagado = turnoFinal.PrecioTurno,
                        Estado = EstadoReserva.Confirmado,
                        TipoReserva = TipoReserva.Ocasional,
                        FechaReserva = DateOnly.FromDateTime(DateTime.Now.AddDays(-20)),
                        FechaAsistencia = turnoFinal.Fecha,
                        Asistencia = Asistencia.Presente
                    };
                    context.Reservas.Add(rc);
                    context.SaveChanges();

                    var pc = new Pago(rc.Id_Usuario, rc.PrecioPagado, rc.Id, turnoFinal.Id, null) { Fecha = DateTime.Now.AddDays(-19) };
                    context.Pagos.Add(pc);
                    context.SaveChanges();
                }

                // Reserva confirmada pero ausente (pagada)
                if (usuarios.Count > 1)
                {
                    var ra = new Reserva
                    {
                        Id_Usuario = usuarios[1].Id,
                        Id_Turno = turnoFinal.Id,
                        PrecioPagado = turnoFinal.PrecioTurno,
                        Estado = EstadoReserva.Confirmado,
                        TipoReserva = TipoReserva.Ocasional,
                        FechaReserva = DateOnly.FromDateTime(DateTime.Now.AddDays(-18)),
                        FechaAsistencia = turnoFinal.Fecha,
                        Asistencia = Asistencia.Ausente
                    };
                    context.Reservas.Add(ra);
                    context.SaveChanges();

                    var pa = new Pago(ra.Id_Usuario, ra.PrecioPagado, ra.Id, turnoFinal.Id, null) { Fecha = DateTime.Now.AddDays(-17) };
                    context.Pagos.Add(pa);
                    context.SaveChanges();
                }

                // Reserva cancelada (sin pago)
                if (usuarios.Count > 2)
                {
                    var rcan = new Reserva
                    {
                        Id_Usuario = usuarios[2].Id,
                        Id_Turno = turnoFinal.Id,
                        PrecioPagado = 0,
                        Estado = EstadoReserva.Cancelado,
                        TipoReserva = TipoReserva.Ocasional,
                        FechaReserva = DateOnly.FromDateTime(DateTime.Now.AddDays(-16)),
                        FechaAsistencia = turnoFinal.Fecha,
                        Asistencia = Asistencia.Ausente
                    };
                    context.Reservas.Add(rcan);
                    context.SaveChanges();
                }
            }
        }

        // Seed ejemplo: actividades, canchas, profesores, usuarios y turnos para pruebas de estadísticas
        if (!context.Actividades.Any())
        {
            var actividades = new List<Actividad>
            {
                new Actividad("Futbol","Entrenamiento de futbol", 800),
                new Actividad("Padel","Clase de padel", 700),
                new Actividad("Voley","Clase de voley", 700),
                new Actividad("Basquet","Clase de basquet", 750)
            };
            context.Actividades.AddRange(actividades);
            context.SaveChanges();
        }

        if (!context.Canchas.Any())
        {
            var canchas = new List<Cancha>
            {
                new Cancha(1, 10),
                new Cancha(2, 10),
                new Cancha(3, 10)
            };
            context.Canchas.AddRange(canchas);
            context.SaveChanges();
        }

        if (!context.Profesores.Any())
        {
            var profes = new List<Profesor>
            {
                new Profesor("Ana","Gomez","30111222"),
                new Profesor("Luis","Martinez","30999888"),
                new Profesor("Mariana","Lopez","30777666")
            };
            context.Profesores.AddRange(profes);
            context.SaveChanges();
        }

        // Usuarios de prueba
        if (context.Usuarios.Count() < 5)
        {
            var usuarios = new List<Usuario>
            {
                new Usuario { Nombre = "Sofia", Apellido = "Perez", Domicilio = "Calle A 12", Dni = "40111222", Email = "sofia@example.com", Password = BCrypt.Net.BCrypt.HashPassword("User1234"), Rol = Rol.Cliente },
                new Usuario { Nombre = "Mateo", Apellido = "Rodriguez", Domicilio = "Calle B 34", Dni = "40111223", Email = "mateo@example.com", Password = BCrypt.Net.BCrypt.HashPassword("User1234"), Rol = Rol.Cliente },
                new Usuario { Nombre = "Lucia", Apellido = "Fernandez", Domicilio = "Calle C 56", Dni = "40111224", Email = "lucia@example.com", Password = BCrypt.Net.BCrypt.HashPassword("User1234"), Rol = Rol.Cliente },
                new Usuario { Nombre = "Diego", Apellido = "Santos", Domicilio = "Calle D 78", Dni = "40111225", Email = "diego@example.com", Password = BCrypt.Net.BCrypt.HashPassword("User1234"), Rol = Rol.Cliente }
            };
            context.Usuarios.AddRange(usuarios);
            context.SaveChanges();
        }

        // Turnos de prueba en distintas fechas y horas
        if (!context.Turnos.Any())
        {
            // recuperar entidades para referenciar ids
            var actividadFutbol = context.Actividades.First(a => a.Nombre == "Futbol");
            var actividadPadel = context.Actividades.First(a => a.Nombre == "Padel");
            var actividadVoley = context.Actividades.First(a => a.Nombre == "Voley");
            var actividadBasquet = context.Actividades.First(a => a.Nombre == "Basquet");

            var cancha1 = context.Canchas.First(c => c.Numero == 1);
            var cancha2 = context.Canchas.First(c => c.Numero == 2);

            var profAna = context.Profesores.First(p => p.Nombre == "Ana");
            var profLuis = context.Profesores.First(p => p.Nombre == "Luis");

            var hoy = DateOnly.FromDateTime(DateTime.Today);

            var turnos = new List<Turno>
            {
                new Turno { Fecha = hoy.AddDays(1), HoraInicio = new TimeOnly(9,0), HoraFin = new TimeOnly(10,0), PrecioTurno = actividadPadel.Precio, CupoMaximo = 12, CupoDisponible = 10, Estado = EstadoTurno.Disponible, Id_Actividad = actividadPadel.Id, Id_Profesor = profAna.Id, Id_Cancha = cancha1.Id },
                new Turno { Fecha = hoy.AddDays(2), HoraInicio = new TimeOnly(10,0), HoraFin = new TimeOnly(11,0), PrecioTurno = actividadVoley.Precio, CupoMaximo = 16, CupoDisponible = 16, Estado = EstadoTurno.Disponible, Id_Actividad = actividadVoley.Id, Id_Profesor = profLuis.Id, Id_Cancha = cancha2.Id },
                new Turno { Fecha = hoy.AddDays(3), HoraInicio = new TimeOnly(18,0), HoraFin = new TimeOnly(19,0), PrecioTurno = actividadFutbol.Precio, CupoMaximo = 20, CupoDisponible = 5, Estado = EstadoTurno.Lleno, Id_Actividad = actividadFutbol.Id, Id_Profesor = profAna.Id, Id_Cancha = cancha1.Id },
                new Turno { Fecha = hoy.AddDays(-2), HoraInicio = new TimeOnly(8,0), HoraFin = new TimeOnly(9,0), PrecioTurno = actividadBasquet.Precio, CupoMaximo = 18, CupoDisponible = 0, Estado = EstadoTurno.Finalizado, Id_Actividad = actividadBasquet.Id, Id_Profesor = profLuis.Id, Id_Cancha = cancha2.Id },
                new Turno { Fecha = hoy.AddDays(7), HoraInicio = new TimeOnly(19,0), HoraFin = new TimeOnly(20,0), PrecioTurno = actividadFutbol.Precio, CupoMaximo = 20, CupoDisponible = 18, Estado = EstadoTurno.Disponible, Id_Actividad = actividadFutbol.Id, Id_Profesor = profLuis.Id, Id_Cancha = cancha1.Id }
            };

            context.Turnos.AddRange(turnos);
            context.SaveChanges();
        }

        // Reservas y Pagos de prueba: confirmados (pagados) y cancelados para estadísticas
        if (!context.Reservas.Any())
        {
            var usuariosList = context.Usuarios.Take(5).ToList();
            var turnosList = context.Turnos.OrderBy(t => t.Fecha).ToList();
            var hoy = DateOnly.FromDateTime(DateTime.Today);

            // elegir algunos turnos pasados y futuros
            var pasado1 = turnosList.FirstOrDefault(t => t.Fecha <= hoy.AddDays(0));
            var pasado2 = turnosList.FirstOrDefault(t => t.Fecha < hoy);
            var futuro1 = turnosList.FirstOrDefault(t => t.Fecha > hoy);

            var reservas = new List<Reserva>();

            if (pasado1 != null && usuariosList.Count > 0)
            {
                var r = new Reserva
                {
                    Id_Usuario = usuariosList[0].Id,
                    Id_Turno = pasado1.Id,
                    PrecioPagado = pasado1.PrecioTurno,
                    Estado = EstadoReserva.Confirmado,
                    TipoReserva = TipoReserva.Ocasional,
                    FechaReserva = DateOnly.FromDateTime(DateTime.Now.AddDays(-10)),
                    FechaAsistencia = pasado1.Fecha,
                    Asistencia = Asistencia.Presente
                };
                reservas.Add(r);
                context.Reservas.Add(r);
                context.SaveChanges();

                var pago = new Pago(r.Id_Usuario, r.PrecioPagado, r.Id, pasado1.Id, null) { Fecha = DateTime.Now.AddDays(-9) };
                context.Pagos.Add(pago);
                context.SaveChanges();
            }

            if (pasado2 != null && usuariosList.Count > 1)
            {
                var r2 = new Reserva
                {
                    Id_Usuario = usuariosList[1].Id,
                    Id_Turno = pasado2.Id,
                    PrecioPagado = pasado2.PrecioTurno,
                    Estado = EstadoReserva.Confirmado,
                    TipoReserva = TipoReserva.Ocasional,
                    FechaReserva = DateOnly.FromDateTime(DateTime.Now.AddDays(-8)),
                    FechaAsistencia = pasado2.Fecha,
                    Asistencia = Asistencia.Ausente
                };
                context.Reservas.Add(r2);
                context.SaveChanges();

                var pago2 = new Pago(r2.Id_Usuario, r2.PrecioPagado, r2.Id, pasado2.Id, null) { Fecha = DateTime.Now.AddDays(-7) };
                context.Pagos.Add(pago2);
                context.SaveChanges();
            }

            if (futuro1 != null && usuariosList.Count > 2)
            {
                var r3 = new Reserva
                {
                    Id_Usuario = usuariosList[2].Id,
                    Id_Turno = futuro1.Id,
                    PrecioPagado = futuro1.PrecioTurno,
                    Estado = EstadoReserva.PendienteDePago,
                    TipoReserva = TipoReserva.Ocasional,
                    FechaReserva = DateOnly.FromDateTime(DateTime.Now.AddDays(-2)),
                    FechaAsistencia = futuro1.Fecha,
                    Asistencia = Asistencia.Ausente
                };
                context.Reservas.Add(r3);
                context.SaveChanges();
            }

            // Reserva cancelada sin pago
            if (turnosList.Count > 0 && usuariosList.Count > 3)
            {
                var anyTurno = turnosList.Last();
                var r4 = new Reserva
                {
                    Id_Usuario = usuariosList[3].Id,
                    Id_Turno = anyTurno.Id,
                    PrecioPagado = 0,
                    Estado = EstadoReserva.Cancelado,
                    TipoReserva = TipoReserva.Ocasional,
                    FechaReserva = DateOnly.FromDateTime(DateTime.Now.AddDays(-5)),
                    FechaAsistencia = anyTurno.Fecha,
                    Asistencia = Asistencia.Ausente
                };
                context.Reservas.Add(r4);
                context.SaveChanges();
            }
        }
    }
   
}
