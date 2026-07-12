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
            var usuarios = new List<Usuario>
            {
                new Usuario("Mario", "Tevez", "Calle 123", "112345534", BCrypt.Net.BCrypt.HashPassword("Admin1234"), "admin@gmail.com", false, Rol.Administrador),
                new Usuario("Joaquin", "Pinanelli", "Calle 456", "30111222", BCrypt.Net.BCrypt.HashPassword("Boca1999"), "joa64919@gmail.com", false, Rol.Cliente),
                new Usuario("Maria", "Gomez", "Calle 789", "32444555", BCrypt.Net.BCrypt.HashPassword("Boca1999"), "maria@gmail.com", false, Rol.Cliente),
                new Usuario("nel", "pinanelli", "Calle10", "123456", BCrypt.Net.BCrypt.HashPassword("Boca1999"), "nelsonjp1999@gmail.com", false, Rol.Cliente)
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
                new Profesor("Carlos", "Tevez", "26000222"),
                new Profesor("Juan", "Riquelme", "27000333")
            };
            context.Profesores.AddRange(profesores);
            context.SaveChanges();
        }

        if (!context.Actividades.Any())
        {
            var actividades = new List<Actividad>
            {
                new Actividad("Futbol", "Actividad recreativa de futbol 5", 500),
                new Actividad("Voley", "Entrenamiento de voley", 300),
                new Actividad("Paddle", "Clase grupal de Paddle", 200),
                new Actividad("Basquet", "Clase grupal de Basquet", 100)
            };
            context.Actividades.AddRange(actividades);
            context.SaveChanges();
        }

        if (!context.Turnos.Any())
        {
            
            var futbol = context.Actividades.FirstOrDefault(a => a.Nombre == "Futbol");
            var cancha1 = context.Canchas.FirstOrDefault(c => c.Numero == 1);
            var profeMessi = context.Profesores.FirstOrDefault(p => p.Nombre == "Lionel");

            var voley = context.Actividades.FirstOrDefault(a => a.Nombre == "Voley");
            var cancha2 = context.Canchas.FirstOrDefault(c => c.Numero == 2);
            var profeJuan = context.Profesores.FirstOrDefault(p => p.Nombre == "Juan");

            var basquet = context.Actividades.FirstOrDefault(a => a.Nombre == "Basquet");

            var fechasMartesJulio = new List<DateOnly>
            {
                new DateOnly(2026, 7, 7),
                new DateOnly(2026, 7, 14),
                new DateOnly(2026, 7, 21),
                new DateOnly(2026, 7, 28)
            };

            var fechasMiercolesJulio = new List<DateOnly>
            {
                new DateOnly(2026,7,8),
                new DateOnly(2026,7,15),
                new DateOnly(2026,7,22),
                new DateOnly(2026,7,29)
            };

            var fechasMiercolesAgosto = new List<DateOnly> { 
                new DateOnly(2026,8,5),
                new DateOnly(2026,8,12),
                new DateOnly(2026,8,19),
                new DateOnly(2026,8,26)
            };

            var fechasJuevesJulio = new List<DateOnly> {
                new DateOnly(2026,7,9),
                new DateOnly(2026,7,16),
                new DateOnly(2026,7,23),
                new DateOnly(2026,7,30)
            };

            var fechasJuevesAgosto = new List<DateOnly> {
                new DateOnly(2026,8,6),
                new DateOnly(2026,8,13),
                new DateOnly(2026,8,20),
                new DateOnly(2026,8,27)
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

                var turnosMiercolesJulio = fechasMiercolesJulio.Select(fecha => new Turno
                {
                    Fecha = fecha,
                    HoraInicio = new TimeOnly(20, 0),
                    HoraFin = new TimeOnly(21, 0),
                    PrecioTurno = 5000,
                    CupoMaximo = 10,
                    CupoDisponible = 10,
                    Estado = EstadoTurno.Disponible,
                    Actividad = futbol,
                    Cancha = cancha1,
                    Profesor = profeMessi
                }).ToList();

                var turnosMiercolesAgosto = fechasMiercolesAgosto.Select(fecha => new Turno
                {
                    Fecha = fecha,
                    HoraInicio = new TimeOnly(20, 0),
                    HoraFin = new TimeOnly(21, 0),
                    PrecioTurno = 5000,
                    CupoMaximo = 10,
                    CupoDisponible = 10,
                    Estado = EstadoTurno.Disponible,
                    Actividad = futbol,
                    Cancha = cancha1,
                    Profesor = profeMessi
                }).ToList();

                var turnosJuevesJulio = fechasJuevesJulio.Select(fecha => new Turno
                {
                    Fecha = fecha,
                    HoraInicio = new TimeOnly(19, 0),
                    HoraFin = new TimeOnly(20, 0),
                    PrecioTurno = 2000,
                    CupoMaximo = 10,
                    CupoDisponible = 10,
                    Estado = EstadoTurno.Disponible,
                    Actividad = voley,
                    Cancha = cancha2,
                    Profesor = profeJuan
                }).ToList();

                var turnosJuevesAgosto = fechasJuevesAgosto.Select(fecha => new Turno
                {
                    Fecha = fecha,
                    HoraInicio = new TimeOnly(19, 0),
                    HoraFin = new TimeOnly(20, 0),
                    PrecioTurno = 2000,
                    CupoMaximo = 10,
                    CupoDisponible = 10,
                    Estado = EstadoTurno.Disponible,
                    Actividad = voley,
                    Cancha = cancha2,
                    Profesor = profeJuan
                }).ToList();

                var turnoSuelto1 = new Turno
                {
                    Fecha = new DateOnly(2026, 7, 31),
                    HoraInicio = new TimeOnly(22, 0),
                    HoraFin = new TimeOnly(23, 0),
                    PrecioTurno = 1000,
                    CupoMaximo = 1,
                    CupoDisponible = 1,
                    Estado = EstadoTurno.Disponible,
                    Actividad = futbol,
                    Cancha = cancha1,
                    Profesor = profeMessi
                };


                var turnoSuelto2 = new Turno   
                {
                    Fecha = new DateOnly(2026, 7, 17),
                    HoraInicio = new TimeOnly(19, 0),
                    HoraFin = new TimeOnly(20, 0),
                    PrecioTurno = 1000,
                    CupoMaximo = 5,
                    CupoDisponible = 5,
                    Estado = EstadoTurno.Disponible,
                    Actividad = voley,
                    Cancha = cancha2,
                    Profesor = profeJuan
                };

                var turnoSuelto3 = new Turno
                {
                    Fecha = new DateOnly(2026, 8, 12),
                    HoraInicio = new TimeOnly(20, 30),
                    HoraFin = new TimeOnly(21, 30),
                    PrecioTurno = 1000,
                    CupoMaximo = 10,
                    CupoDisponible = 10,
                    Estado = EstadoTurno.Disponible,
                    Actividad = basquet,
                    Cancha = cancha1,
                    Profesor = profeMessi
                };

                context.Turnos.AddRange(turnoSuelto1);
                context.Turnos.AddRange(turnoSuelto2);
                context.Turnos.AddRange(turnoSuelto3);
                context.Turnos.AddRange(turnosMiercolesJulio);
                context.Turnos.AddRange(turnosMiercolesAgosto);
                context.Turnos.AddRange(turnosJuevesJulio);
                context.Turnos.AddRange(turnosJuevesAgosto);
                context.SaveChanges();
            }
        }

        context.SaveChanges();

        // -------------------------------------------------------------------------
        // SECCIÓN: DATOS HISTÓRICOS PARA ESTADÍSTICAS (TURNOS FINALIZADOS Y RESERVAS)
        // -------------------------------------------------------------------------
        if (!context.Reservas.Any())
        {
            var futbol = context.Actividades.FirstOrDefault(a => a.Nombre == "Futbol");
            var voley = context.Actividades.FirstOrDefault(a => a.Nombre == "Voley");
            var cancha1 = context.Canchas.FirstOrDefault(c => c.Numero == 1);
            var cancha2 = context.Canchas.FirstOrDefault(c => c.Numero == 2);
            var profeMessi = context.Profesores.FirstOrDefault(p => p.Nombre == "Lionel");
            var profeJuan = context.Profesores.FirstOrDefault(p => p.Nombre == "Juan");

            // Buscamos los usuarios clientes para repartir las asistencias
            var clienteJoaco = context.Usuarios.FirstOrDefault(u => u.Email == "joa64919@gmail.com");
            var clienteMaria = context.Usuarios.FirstOrDefault(u => u.Email == "maria@gmail.com");
            var clienteNelson = context.Usuarios.FirstOrDefault(u => u.Email == "nelsonjp1999@gmail.com");

            if (futbol != null && voley != null && clienteJoaco != null && clienteMaria != null && clienteNelson != null)
            {
                // 1. CREAMOS TURNOS PASADOS (JUNIO 2026) EN ESTADO FINALIZADO
                var turnoPasadoFutbol1 = new Turno
                {
                    Fecha = new DateOnly(2026, 6, 16), // Martes pasado
                    HoraInicio = new TimeOnly(18, 0),
                    HoraFin = new TimeOnly(19, 0),
                    PrecioTurno = 5000,
                    CupoMaximo = 10,
                    CupoDisponible = 7,
                    Estado = EstadoTurno.Finalizado,
                    Actividad = futbol,
                    Cancha = cancha1!,
                    Profesor = profeMessi!
                };

                var turnoPasadoFutbol2 = new Turno
                {
                    Fecha = new DateOnly(2026, 6, 23), // Siguiente martes pasado
                    HoraInicio = new TimeOnly(18, 0),
                    HoraFin = new TimeOnly(19, 0),
                    PrecioTurno = 5000,
                    CupoMaximo = 10,
                    CupoDisponible = 8,
                    Estado = EstadoTurno.Finalizado,
                    Actividad = futbol,
                    Cancha = cancha1!,
                    Profesor = profeMessi!
                };

                var turnoPasadoVoley = new Turno
                {
                    Fecha = new DateOnly(2026, 6, 18), // Jueves pasado
                    HoraInicio = new TimeOnly(19, 0),
                    HoraFin = new TimeOnly(20, 0),
                    PrecioTurno = 2000,
                    CupoMaximo = 10,
                    CupoDisponible = 8,
                    Estado = EstadoTurno.Finalizado,
                    Actividad = voley,
                    Cancha = cancha2!,
                    Profesor = profeJuan!
                };

                context.Turnos.AddRange(turnoPasadoFutbol1, turnoPasadoFutbol2, turnoPasadoVoley);
                context.SaveChanges(); // Guardamos para obtener los IDs de los turnos

                // 2. CREAMOS LAS RESERVAS ASOCIADAS CON DIFERENTES COMPORTAMIENTOS

                var reservasHistoricas = new List<Reserva>
        {
            // --- TURNO FUTBOL 1 (Tuvo 3 Inscriptos: 2 Presentes, 1 Ausente) ---
            new Reserva
            {
                Id_Turno = turnoPasadoFutbol1.Id,
                Id_Usuario = clienteJoaco.Id,
                PrecioPagado = 5000,
                Estado = EstadoReserva.Confirmado,
                Asistencia = Asistencia.Presente, // <--- ASISTIÓ
                FechaReserva = new DateOnly(2026, 6, 14),
                FechaAsistencia = turnoPasadoFutbol1.Fecha,
                TipoReserva = TipoReserva.Ocasional
            },
            new Reserva
            {
                Id_Turno = turnoPasadoFutbol1.Id,
                Id_Usuario = clienteMaria.Id,
                PrecioPagado = 5000,
                Estado = EstadoReserva.Confirmado,
                Asistencia = Asistencia.Presente, // <--- ASISTIÓ
                FechaReserva = new DateOnly(2026, 6, 15),
                FechaAsistencia = turnoPasadoFutbol1.Fecha,
                TipoReserva = TipoReserva.Ocasional
            },
            new Reserva
            {
                Id_Turno = turnoPasadoFutbol1.Id,
                Id_Usuario = clienteNelson.Id,
                PrecioPagado = 5000,
                Estado = EstadoReserva.Confirmado,
                Asistencia = Asistencia.Ausente,  // <--- INASISTENCIA (Faltó sin avisar)
                FechaReserva = new DateOnly(2026, 6, 15),
                FechaAsistencia = turnoPasadoFutbol1.Fecha,
                TipoReserva = TipoReserva.Ocasional
            },

            // --- TURNO FUTBOL 2 (Tuvo 2 Inscriptos: 1 Presente, 1 Canceló la reserva) ---
            new Reserva
            {
                Id_Turno = turnoPasadoFutbol2.Id,
                Id_Usuario = clienteJoaco.Id,
                PrecioPagado = 5000,
                Estado = EstadoReserva.Confirmado,
                Asistencia = Asistencia.Presente, // <--- ASISTIÓ
                FechaReserva = new DateOnly(2026, 6, 20),
                FechaAsistencia = turnoPasadoFutbol2.Fecha,
                TipoReserva = TipoReserva.Ocasional
            },
            new Reserva
            {
                Id_Turno = turnoPasadoFutbol2.Id,
                Id_Usuario = clienteMaria.Id,
                PrecioPagado = 5000,
                Estado = EstadoReserva.Cancelado,   // <--- CANCELACIÓN (Avisó previo)
                Asistencia = Asistencia.Ausente,
                FechaReserva = new DateOnly(2026, 6, 20),
              //  FechaCancelacion = new DateOnly(2026, 6, 22),
                FechaAsistencia = turnoPasadoFutbol2.Fecha,
                TipoReserva = TipoReserva.Ocasional
            },

            // --- TURNO VOLEY (Tuvo 2 Inscriptos: 2 Presentes) ---
            new Reserva
            {
                Id_Turno = turnoPasadoVoley.Id,
                Id_Usuario = clienteMaria.Id,
                PrecioPagado = 2000,
                Estado = EstadoReserva.Confirmado,
                Asistencia = Asistencia.Presente, // <--- ASISTIÓ
                FechaReserva = new DateOnly(2026, 6, 17),
                FechaAsistencia = turnoPasadoVoley.Fecha,
                TipoReserva = TipoReserva.Ocasional
            },
            new Reserva
            {
                Id_Turno = turnoPasadoVoley.Id,
                Id_Usuario = clienteNelson.Id,
                PrecioPagado = 2000,
                Estado = EstadoReserva.Confirmado,
                Asistencia = Asistencia.Presente, // <--- ASISTIÓ
                FechaReserva = new DateOnly(2026, 6, 17),
                FechaAsistencia = turnoPasadoVoley.Fecha,
                TipoReserva = TipoReserva.Ocasional
            }
        };

                context.Reservas.AddRange(reservasHistoricas);
                context.SaveChanges();
            }
        }
    }

}