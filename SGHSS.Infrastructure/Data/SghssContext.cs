using Microsoft.EntityFrameworkCore;
using SGHSS.Core.Entities;

namespace SGHSS.Infrastructure.Data;

public class SghssContext(DbContextOptions<SghssContext> options) : DbContext(options)
{
    public DbSet<Usuario> Usuarios { get; set; } = null!;
    public DbSet<Paciente> Pacientes { get; set; } = null!;
    public DbSet<Profissional> Profissionais { get; set; } = null!;
    public DbSet<Consulta> Consultas { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Usuario
        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Paciente
        modelBuilder.Entity<Paciente>()
            .HasIndex(p => p.Cpf)
            .IsUnique();

        // Profissional
        modelBuilder.Entity<Profissional>()
            .HasIndex(p => p.Cpf)
            .IsUnique();

        // Consulta → Paciente (1:N)
        modelBuilder.Entity<Consulta>()
            .HasOne(c => c.Paciente)
            .WithMany(p => p.Consultas)
            .HasForeignKey(c => c.PacienteId);

        // Consulta → Profissional (1:N)
        modelBuilder.Entity<Consulta>()
            .HasOne(c => c.Profissional)
            .WithMany(p => p.Consultas)
            .HasForeignKey(c => c.ProfissionalId);
    }
}