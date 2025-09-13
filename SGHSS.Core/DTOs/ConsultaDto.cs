namespace SGHSS.Core.DTOs;

public class ConsultaDto
{
    public int Id { get; set; }
    public DateTime DataConsulta { get; set; }
    public string Status { get; set; } = string.Empty;

    public int PacienteId { get; set; }
    public string? PacienteNome { get; set; }

    public int ProfissionalId { get; set; }
    public string? ProfissionalNome { get; set; }
}