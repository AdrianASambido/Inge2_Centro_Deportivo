using CentroDeportivo.Aplicacion.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Infraestructura.Persistencia.Contexto
{
    public class CentroDeportivoContext : DbContext
    {
        public CentroDeportivoContext(DbContextOptions<CentroDeportivoContext> options)
        : base(options)
        {
        }

        // Definimos las tablas
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<Actividad> Actividades { get; set; }
        public DbSet<Cancha> Canchas { get; set; }
        public DbSet<Profesor> Profesores { get; set; }
        public DbSet<Devolucion> Devoluciones { get; set; }
        public DbSet<ListaEsperaEntrada> ListaEsperaEntradas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Claves Primarias de todas las entidades
            modelBuilder.Entity<Usuario>().HasKey(u => u.Id);
            modelBuilder.Entity<Turno>().HasKey(t => t.Id);
            modelBuilder.Entity<Reserva>().HasKey(r => r.Id);
            modelBuilder.Entity<Actividad>().HasKey(a => a.Id);
            modelBuilder.Entity<Cancha>().HasKey(c => c.Id);
            modelBuilder.Entity<Profesor>().HasKey(p => p.Id);
            modelBuilder.Entity<Devolucion>().HasKey(d => d.Id);
            modelBuilder.Entity<ListaEsperaEntrada>().HasKey(e => e.Id);

            // Relaciónes
            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Usuario)
                .WithMany()
                .HasForeignKey(r => r.Id_Usuario);

            modelBuilder.Entity<Reserva>()
                        .HasOne(r => r.Turno)
                        .WithMany(t => t.Reservas) // <--- ACÁ: Le decimos que Turno tiene la lista "Reservas"
                        .HasForeignKey(r => r.Id_Turno);


            modelBuilder.Entity<Turno>()
                        .HasOne(t => t.Actividad)
                        .WithMany()
                        .HasForeignKey(t => t.Id_Actividad);

            modelBuilder.Entity<Turno>()
                        .HasOne(t => t.Cancha)
                        .WithMany()
                        .HasForeignKey(t => t.Id_Cancha);

            modelBuilder.Entity<Turno>()
                        .HasOne(t => t.Profesor)
                        .WithMany()
                        .HasForeignKey(t => t.Id_Profesor);

      

            modelBuilder.Entity<Devolucion>()
                .HasOne(d => d.Usuario)
                .WithMany()
                .HasForeignKey(d => d.Id_Usuario);

            modelBuilder.Entity<Devolucion>()
                .HasOne(d => d.Reserva)
                .WithMany()
                .HasForeignKey(d => d.Id_Reserva);

            modelBuilder.Entity<ListaEsperaEntrada>()
                .HasOne(e => e.Turno)
                .WithMany()
                .HasForeignKey(e => e.Id_Turno);

            modelBuilder.Entity<ListaEsperaEntrada>()
                .HasOne(e => e.Usuario)
                .WithMany()
                .HasForeignKey(e => e.Id_Usuario);

            base.OnModelCreating(modelBuilder);
        }
    }
}
