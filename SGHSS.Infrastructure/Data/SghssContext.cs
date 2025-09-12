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
            .HasForeignKey(c => c.PacienteId)
            .OnDelete(DeleteBehavior.Restrict);

        // Consulta → Profissional (1:N)
        modelBuilder.Entity<Consulta>()
            .HasOne(c => c.Profissional)
            .WithMany(p => p.Consultas)
            .HasForeignKey(c => c.ProfissionalId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Usuario Admin
        modelBuilder.Entity<Usuario>().HasData(new Usuario
        {
            Id = 1,
            Nome = "Administrador",
            Email = "admin@sghss.com",
            SenhaHash = "admin123", // futuramente trocamos pra hash
            Role = "Admin"
        });

        // Paciente inicial
        modelBuilder.Entity<Paciente>().HasData(new Paciente
        {
            Id = 1,
            Nome = "Lucas Silva",
            Cpf = "12345678901",
            DataNascimento = new DateTime(1998, 5, 12)
        });

        // Profissional inicial
        modelBuilder.Entity<Profissional>().HasData(new Profissional
        {
            Id = 1,
            Nome = "Dr. João Souza",
            Cpf = "98765432100",
            Especialidade = "Clínico Geral"
        });

        // Consulta inicial
        modelBuilder.Entity<Consulta>().HasData(new Consulta
        {
            Id = 1,
            DataConsulta = DateTime.UtcNow.AddDays(7),
            Status = "Agendada",
            PacienteId = 1,
            ProfissionalId = 1
        });
    }
}