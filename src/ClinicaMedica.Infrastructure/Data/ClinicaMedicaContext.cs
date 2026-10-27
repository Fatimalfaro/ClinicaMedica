
using ClinicaMedica.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicaMedica.Infrastructure.Data;

public class ClinicaMedicaContext : DbContext
{
    public ClinicaMedicaContext(
        DbContextOptions<ClinicaMedicaContext> options)
        : base(options)
    {
    }

    public DbSet<Especialidad> Especialidades { get; set; }

    public DbSet<Medico> Medicos { get; set; }

    public DbSet<Paciente> Pacientes { get; set; }

    public DbSet<Turno> Turnos { get; set; }

    public DbSet<HistoriaClinica> HistoriasClinicas { get; set; }

    public DbSet<Evolucion> Evoluciones { get; set; }

    public DbSet<Receta> Recetas { get; set; }

    public DbSet<PedidoLaboratorio> PedidosLaboratorio { get; set; }

    public DbSet<Usuario> Usuarios { get; set; }

    public DbSet<Pago> Pagos { get; set; }

    
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // Especialidad y medicos
    modelBuilder.Entity<Medico>()
        .HasOne(m => m.Especialidad)
        .WithMany(e => e.Medicos)
        .HasForeignKey(m => m.EspecialidadId)
        .OnDelete(DeleteBehavior.Restrict);

    // Paciente y turnos
    modelBuilder.Entity<Turno>()
        .HasOne(t => t.Paciente)
        .WithMany()
        .HasForeignKey(t => t.PacienteId)
        .OnDelete(DeleteBehavior.Restrict);

    // Medico y turnos
    modelBuilder.Entity<Turno>()
        .HasOne(t => t.Medico)
        .WithMany()
        .HasForeignKey(t => t.MedicoId)
        .OnDelete(DeleteBehavior.Restrict);

    // Paciente e historia clinica
    modelBuilder.Entity<HistoriaClinica>()
        .HasOne(h => h.Paciente)
        .WithMany()
        .HasForeignKey(h => h.PacienteId)
        .OnDelete(DeleteBehavior.Restrict);

    // Medico e historia clinica
    modelBuilder.Entity<HistoriaClinica>()
        .HasOne(h => h.Medico)
        .WithMany()
        .HasForeignKey(h => h.MedicoId)
        .OnDelete(DeleteBehavior.Restrict);

    // Historia clinica y evoluciones
    modelBuilder.Entity<Evolucion>()
        .HasOne(e => e.HistoriaClinica)
        .WithMany(h => h.Evoluciones)
        .HasForeignKey(e => e.HistoriaClinicaId)
        .OnDelete(DeleteBehavior.Restrict);

    // Historia clinica y recetas
    modelBuilder.Entity<Receta>()
        .HasOne(r => r.HistoriaClinica)
        .WithMany()
        .HasForeignKey(r => r.HistoriaClinicaId)
        .OnDelete(DeleteBehavior.Restrict);

    // Historia clinica y pedidos de laboratorio
    modelBuilder.Entity<PedidoLaboratorio>()
        .HasOne(p => p.HistoriaClinica)
        .WithMany()
        .HasForeignKey(p => p.HistoriaClinicaId)
        .OnDelete(DeleteBehavior.Restrict);

    // Turno y pago
    modelBuilder.Entity<Pago>()
        .HasOne(p => p.Turno)
        .WithMany()
        .HasForeignKey(p => p.TurnoId)
        .OnDelete(DeleteBehavior.Restrict);

    // Restricciones de datos
    modelBuilder.Entity<Especialidad>()
        .Property(e => e.Nombre)
        .HasMaxLength(100)
        .IsRequired();

    modelBuilder.Entity<Medico>()
        .Property(m => m.Matricula)
        .HasMaxLength(50)
        .IsRequired();

    modelBuilder.Entity<Medico>()
        .HasIndex(m => m.Matricula)
        .IsUnique();

    modelBuilder.Entity<Paciente>()
        .Property(p => p.DNI)
        .HasMaxLength(20)
        .IsRequired();

    modelBuilder.Entity<Paciente>()
        .HasIndex(p => p.DNI)
        .IsUnique();

    modelBuilder.Entity<Usuario>()
        .Property(u => u.Email)
        .HasMaxLength(150)
        .IsRequired();

    modelBuilder.Entity<Usuario>()
        .HasIndex(u => u.Email)
        .IsUnique();

    modelBuilder.Entity<Usuario>()
        .Property(u => u.PasswordHash)
        .HasMaxLength(255)
        .IsRequired();

    modelBuilder.Entity<Pago>()
        .Property(p => p.Monto)
        .HasPrecision(10, 2);

    modelBuilder.Entity<Medico>()
        .Property(m => m.PrecioConsulta)
        .HasPrecision(10, 2);
}

}
