using FluentValidation;
using SGHSS.Core.DTOs;

namespace SGHSS.Core.Validations;

public class PacienteDtoValidator : AbstractValidator<PacienteDto>
{
    public PacienteDtoValidator()
    {
        RuleFor(p => p.Id)
            .GreaterThan(0).WithMessage("O ID deve ser válido");

        RuleFor(p => p.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório")
            .MaximumLength(100);

        RuleFor(p => p.Cpf)
            .NotEmpty().WithMessage("O CPF é obrigatório")
            .Length(11);

        RuleFor(p => p.DataNascimento)
            .LessThan(DateTime.Today);
    }
}