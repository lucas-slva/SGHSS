namespace SGHSS.Core.DTOs;

public class RegisterUsuarioDto
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string Role { get; set; } = "User"; // opcional no cadastro, default é User
}