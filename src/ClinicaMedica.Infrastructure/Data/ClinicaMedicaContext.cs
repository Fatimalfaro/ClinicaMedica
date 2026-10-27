
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
}
