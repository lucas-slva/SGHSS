namespace SGHSS.Core.DTOs;

public class CreatePacienteDto
{
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
}