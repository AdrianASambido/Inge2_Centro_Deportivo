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

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<Actividad> Actividades { get; set; }
        public DbSet<Cancha> Canchas { get; set; }
        public DbSet<Profesor> Profesores { get; set; }
        public DbSet<Devolucion> Devoluciones { get; set; }
        public DbSet<Credito> Creditos { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<InscripcionListaEspera> InscripcionListaEsperas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // claves Primarias de todas las entidades
            modelBuilder.Entity<Usuario>().HasKey(u => u.Id);
            modelBuilder.Entity<Turno>().HasKey(t => t.Id);
            modelBuilder.Entity<Reserva>().HasKey(r => r.Id);
            modelBuilder.Entity<Actividad>().HasKey(a => a.Id);
            modelBuilder.Entity<Cancha>().HasKey(c => c.Id);
            modelBuilder.Entity<Profesor>().HasKey(p => p.Id);
            modelBuilder.Entity<Devolucion>().HasKey(d => d.Id);
            modelBuilder.Entity<Credito>().HasKey(c => c.Id);
            modelBuilder.Entity<InscripcionListaEspera>().HasKey(a => a.Id);
            modelBuilder.Entity<Pago>().HasKey(p => p.Id);

            // relacipnes
            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Usuario)
                .WithMany()
                .HasForeignKey(r => r.Id_Usuario);

            modelBuilder.Entity<Reserva>()
                        .HasOne(r => r.Turno)
                        .WithMany(t => t.Reservas) //Le decimos que Turno tiene la lista "Reservas
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

            modelBuilder.Entity<Pago>()
    .HasOne(p => p.Usuario)
    .WithMany()
    .HasForeignKey(p => p.Id_Usuario);

            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Reserva)
                .WithMany()
                .HasForeignKey(p => p.Id_Reserva);

            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Turno)
                .WithMany()
                .HasForeignKey(p => p.Id_Turno);

            modelBuilder.Entity<Devolucion>()
                .HasOne(d => d.Usuario)
                .WithMany()
                .HasForeignKey(d => d.Id_Usuario);

            modelBuilder.Entity<Devolucion>()
                .HasOne(d => d.Reserva)
                .WithMany()
                .HasForeignKey(d => d.Id_Reserva);

            // InscripcionListaEspera
            modelBuilder.Entity<InscripcionListaEspera>()
                .HasOne(i => i.Usuario)
                .WithMany()
                .HasForeignKey(i => i.Id_Usuario);

            modelBuilder.Entity<InscripcionListaEspera>()
                .HasOne(i => i.Turno)
                .WithMany()
                .HasForeignKey(i => i.Id_Turno);

            // Credito
            modelBuilder.Entity<Credito>()
                .HasOne(c => c.Usuario)
                .WithMany()
                .HasForeignKey(c => c.Id_Usuario);

            modelBuilder.Entity<Credito>()
                .HasOne(c => c.Actividad)
                .WithMany()
                .HasForeignKey(c => c.Id_Actividad);

            base.OnModelCreating(modelBuilder);
        }
    }
}