namespace SGHSS.Core.DTOs;

public class CreateConsultaDto
{
    public DateTime DataConsulta { get; set; }
    public string Status { get; set; } = string.Empty;

    public int PacienteId { get; set; }
    public int ProfissionalId { get; set; }
}