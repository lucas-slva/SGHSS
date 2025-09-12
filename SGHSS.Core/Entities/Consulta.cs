namespace SGHSS.Core.Entities;

public class Consulta
{
    public int Id { get; set; }
    public DateTime DataConsulta { get; set; }
    public string Status { get; set; } = "Agendada"; // Agendada, Cancelada, Realizada

    // FK → Paciente
    public int PacienteId { get; set; }
    public Paciente? Paciente { get; set; }

    // FK → Profissional
    public int ProfissionalId { get; set; }
    public Profissional? Profissional { get; set; }
}