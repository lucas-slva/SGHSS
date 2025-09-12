namespace SGHSS.Core.Entities;

public class Paciente
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }

    // Relacionamento 1:N com Consulta
    public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();
}