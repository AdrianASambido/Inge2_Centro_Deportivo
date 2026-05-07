using CentroDeportivo.Aplicacion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Infraestructura.Persistencia.Contexto
{
    public static class DbInicializador
    {
        public static void Initialize(CentroDeportivoContext context)
        {
            // Crea la base de datos y las tablas si no existen
            context.Database.EnsureCreated();

           
            // Verificamos si ya existen usuarios para no duplicar el Admin 
            if (context.Usuarios.Any())
            {
                return; // La base ya tiene datos, no hacemos nada
            }

            // Crear el Administrador por defecto si la BD esta vacia
            var admin = new Usuario
            {
                Nombre = "Carlos",
                Apellido = "Tevez",
                Domicilio = "Calle 123",
                Dni = 112345534,
                Email = "admin@centrodeportivo.com",

                
                Password = BCrypt.Net.BCrypt.HashPassword("Admin1234"),
                Rol = Rol.Administrador,
                DebeCambiarPassword = false
            };

            context.Usuarios.Add(admin);

            // Guardar los cambios en el archivo .db
            context.SaveChanges();
        }
    }
    }

