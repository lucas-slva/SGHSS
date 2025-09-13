using FluentValidation;
using SGHSS.Core.DTOs;

namespace SGHSS.Core.Validations;

public class CreatePacienteDtoValidator : AbstractValidator<CreatePacienteDto>
{
    public CreatePacienteDtoValidator()
    {
        RuleFor(p => p.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório")
            .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres");

        RuleFor(p => p.Cpf)
            .NotEmpty().WithMessage("O CPF é obrigatório")
            .Length(11).WithMessage("O CPF deve ter 11 dígitos");

        RuleFor(p => p.DataNascimento)
            .LessThan(DateTime.Today).WithMessage("A data de nascimento deve ser no passado");
    }
}