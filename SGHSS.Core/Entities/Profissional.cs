namespace SGHSS.Core.Entities;

public class Profissional
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Especialidade { get; set; } = string.Empty;

    // Relacionamento 1:N com Consulta
    public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();
}