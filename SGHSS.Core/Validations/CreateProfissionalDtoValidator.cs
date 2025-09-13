using FluentValidation;
using SGHSS.Core.DTOs;

namespace SGHSS.Core.Validations;

public class CreateProfissionalDtoValidator : AbstractValidator<CreateProfissionalDto>
{
    public CreateProfissionalDtoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório")
            .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres");

        RuleFor(x => x.Cpf)
            .NotEmpty().WithMessage("O CPF é obrigatório")
            .Length(11).WithMessage("O CPF deve ter 11 dígitos");

        RuleFor(x => x.Especialidade)
            .NotEmpty().WithMessage("A especialidade é obrigatória")
            .MaximumLength(100).WithMessage("A especialidade deve ter no máximo 100 caracteres");
    }
}